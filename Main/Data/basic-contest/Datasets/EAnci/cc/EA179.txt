package Servlet;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import Bean.Accesso;
import DB.DbException;
import Manager.AccessManager;
/**
 * La classe ServletModificaAccesso che effettua l'operazione di modifica di un accesso
 * La classe ServletModificaAccesso non ha dipendenze
 * @author Federico Cinque
 */
public class ServletModificaAccesso extends HttpServlet{

	private String login;
	private String password;
	private String cpassword;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();

		if(session!=null){	//Se la sessione é nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				login = request.getParameter("login");
				password = request.getParameter("password");
				cpassword = request.getParameter("cpassword");
				tipo = request.getParameter("tipo");

				AccessManager AM = new AccessManager();
				Accesso ac = new Accesso(login,password,tipo);

				if(AM.modificaAccesso(login, ac)){	//modifico i dati relativi all'accesso controllando che l'esito sia positivo
					ris="ok";
					request.setAttribute("ris", ris);
					if(ac.getTipo().equals("Cittadino"))
						rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=riuscita");
					else
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=riuscita"); 
				}
				else{
					ris="fallita";
					request.setAttribute("ris", ris);
					if(ac.getTipo().equals("Cittadino"))
						rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
					else
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
				}
				rd.forward(request,response);
			}
			catch(DbException e){
				ris=e.getMessage();
				request.setAttribute("ris", ris);
				rd=sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
				rd.forward(request,response);
			}
		}
		else{	//questa servlet viene invocata sia dal lato cittadino sia da amm/imp
				//quindi effettuo un controllo da quale url proviene la richiesta
				//cosi posso effettuare il redirect alla pagina corretta
			String from = request.getRequestURL().toString();
			String url;
			if(from.contains("user"))
				url="/myDoc/user/home.jsp";
			else
				url="/myDoc/workers/Accesso.jsp";
			response.sendRedirect(url);
		}
	}
}
