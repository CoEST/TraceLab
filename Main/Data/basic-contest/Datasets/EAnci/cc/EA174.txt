package Servlet;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import Manager.AccessManager;
import Manager.AdminManager;
import Bean.Accesso;
import Bean.Amministratore;
import DB.DbException;
/**
 * La classe ServletInserisciAmministratore inserisce un amministratore nel database
 * La classe ServletInserisciAmministratore non ha dipendenze
 * @author Federico Cinque
 */
public class ServletInserisciAmministratore extends HttpServlet {
	private String nome;
	private String cognome;
	private String email;
	private String matricola;
	private String login;
	private String password;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();

		if(session!=null){	//Se la sessione + nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				nome = request.getParameter("nome");
				cognome = request.getParameter("cognome");
				email = request.getParameter("email");
				matricola = request.getParameter("matricola");
				login = request.getParameter("login");
				password = request.getParameter("password");
				tipo = request.getParameter("tipo");

				AccessManager AM = new AccessManager();
				AdminManager AdM = new AdminManager();

				Accesso ac = new Accesso(login,password,tipo);
				Amministratore am = new Amministratore(nome,cognome,matricola,email,login);

				rd = null;
				sc = getServletContext();

				if(AM.inserisciAccesso(ac) && AdM.inserisciAdmin(am)){ //inserisco idati relativi all'accesso e all'amministratore
																		//controllando l'esito positivo
					ris="ok";
					request.setAttribute("ris", ris);
					rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=riuscita"); 
				}
				else{
					ris="fallita";
					request.setAttribute("ris", ris);
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
		else{
			String url="/myDoc/workers/Accesso.jsp";
			response.sendRedirect(url);
		}
	}
}
