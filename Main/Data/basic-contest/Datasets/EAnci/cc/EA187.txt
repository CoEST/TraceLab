package Servlet;

import java.io.IOException;
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
 * La classe ServletRegistraCittadino gestisce l'operazione di registrazione 
 * di un cittadino nel sistema
 * La classe ServletRegistraCittadino non ha dipendenze
 * @author Federico Cinque
 */
public class ServletRegistraCittadino extends HttpServlet  {
	private String nome;
	private String cognome;
	private String email;
	private String ci;
	private String cf;
	private String login;
	private String password;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		RequestDispatcher rd = null;
		ServletContext sc = getServletContext();
		String ris;
		try{
			nome = request.getParameter("nome");
			cognome = request.getParameter("cognome");
			email = request.getParameter("email");
			ci = request.getParameter("ci");
			cf = request.getParameter("cf").toUpperCase();
			login = request.getParameter("login");
			password = request.getParameter("password");
			tipo = "Cittadino";

			CittadinoManager CM = new CittadinoManager();
			CIManager CIM = new CIManager();
			AccessManager AM = new AccessManager();
			CartaIdentita CI = CIM.getCartaByNumero(ci);

			if(CI!=null){
				if(!AM.controllaLogin(login)){
					Accesso ac = new Accesso(login,password,tipo);
					Cittadino c = CM.getCittadinoById(CI.id());
					if(c.getCodiceFiscale().equals(cf) && c.getCognome().equals(cognome) && c.getNome().equals(nome)){
						if(AM.inserisciAccesso(ac) && CM.modificaLogin(c.getIdCittadino(), login) && CM.modificaEmail(c.getIdCittadino(), email)){
							ris="ok";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=riuscita");
						}
						else{
							ris="fallita";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
						}
					}
					else{
						ris="I dati inseriti non corrispondono";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
					}
				}
				else{
					ris="Siamo spiacenti, la login é giˆ presente";
					request.setAttribute("ris", ris);
					rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
				}
			}
			else{
				ris="Siamo spiacenti, il codice della carta d'identitˆ noné presente nel database";
				request.setAttribute("ris", ris);
				rd = sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
			}
			rd.forward(request,response);
		}
		catch(DbException e){
			ris=e.getMessage();
			request.setAttribute("ris", ris);
			rd=sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
			rd.forward(request,response);
		}
	}
}
