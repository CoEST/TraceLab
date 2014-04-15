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
 * La classe ServletModificaAmministratore che effettua l'operazione di modifica di un amministratore
 * La classe ServletModificaAmministratore non ha dipendenze
 * @author Federico Cinque
 */
public class ServletModificaAmministratore extends HttpServlet{

	private String nome;
	private String cognome;
	private String email;
	private String matricola;
	private String login;
	private String password;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException{
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessione é nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				//Se gli attributi di sessione amm e acc sono nulli devo effettuare la ricerca
				if(session.getAttribute("amm")==null && session.getAttribute("acc")==null){
					String matricola = request.getParameter("matricola");
					AdminManager AdM = new AdminManager();
					Amministratore am = AdM.ricercaAdminByMatricola(matricola);
					if(am != null){
						session.setAttribute("amm", am);

						AccessManager AM = new AccessManager();
						Accesso ac = AM.getAccesso(am.getLogin());
						session.setAttribute("acc", ac);

						rd = sc.getRequestDispatcher("/workers/index.jsp?func=modifica&page=amministratore"); 
						rd.forward(request,response);
					}
					else{
						ris="La matricola non corrisponde ad un amministratore";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita"); 
						rd.forward(request,response);
					}
				}
				else{
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
					Amministratore amOld = (Amministratore) session.getAttribute("amm");
					
					if(AM.modificaAccesso(amOld.getLogin(), ac) && AdM.modificaAdmin(amOld.getMatricola(), am)){ //procedo con la modifica dei dati
						//controllando l'esito
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
					session.removeAttribute("amm");
					session.removeAttribute("acc");
				}
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
