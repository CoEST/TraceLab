package Servlet;

import java.io.IOException;
import java.util.Random;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import Manager.AccessManager;
import Manager.CIManager;
import Manager.CittadinoManager;
import Bean.Accesso;
import Bean.CartaIdentita;
import Bean.Cittadino;
import DB.DbException;
/**
 * La classe ServletRecuperoPassword gestisce l'operazione di recupero password per un cittadino
 * La classe ServletRecuperoPassword non ha dipendenze
 * @author Federico Cinque
 */
public class ServletRecuperoPassword extends HttpServlet{

	private String email;
	private String ci;
	private String login;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		RequestDispatcher rd = null;
		ServletContext sc = getServletContext();
		String ris;

		try{
			ci = request.getParameter("ci").toUpperCase();
			login = request.getParameter("login");
			tipo = "Cittadino";

			CittadinoManager CM = new CittadinoManager();
			CIManager CIM = new CIManager();
			AccessManager AM = new AccessManager();
			CartaIdentita CI = CIM.getCartaByNumero(ci);

			if(CI!=null){
				if(AM.controllaLogin(login)){
					Accesso ac = AM.getAccesso(login);
					Cittadino c = CM.getCittadinoById(CI.id());
					if(c.getLogin().equals(login)){
						String p = generaPassword();	//nuova password auto-generata
						ac.setPassword(p);
						AM.modificaAccesso(login, ac);

						//inviare l'email a c.getEmail();

						ris="E' stata inviata un email al suo indirizzo di posta elettronica";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=riuscita");
					}
					else{
						ris="La login non corrisponde alla codice della carta";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
					}
				}
				else{
					ris="Siamo spiacenti, la login nonè presente";
					request.setAttribute("ris", ris);
					rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
				}
			}
			else{
				ris="Siamo spiacenti, il codice della carta d'identitˆ nonè presente nel database";
				request.setAttribute("ris", ris);
				rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
			}
			rd.forward(request,response);
		}
		catch(DbException e){
			ris=e.getMessage();
			request.setAttribute("ris", ris);
			rd=sc.getRequestDispatcher("/user/index.jsp?func=operazione&page=fallita");
			rd.forward(request,response);
		}
	}


	private static String generaPassword() {
		String pass="";
		Random r = new Random();
		for(int i=0;i<8;i++){
			int x = r.nextInt(10);   // genera un intero tra 0 e 9
			char c = (char) ((int) 'A' + r.nextInt(26));   // genera un char tra 'A' e 'Z
			boolean s = r.nextBoolean();
			if(s)
				pass=pass+c;
			else
				pass=pass+x;
		}
		return pass;
	}

}
