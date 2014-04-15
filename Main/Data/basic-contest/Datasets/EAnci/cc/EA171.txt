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
 * La classe ServletEliminaAmministratore ricerca ed elimina un amministratore
 * La classe ServletEliminaAmministratore non ha dipendenze
 * @author Federico Cinque
 */
public class ServletEliminaAmministratore extends HttpServlet{

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException{
		HttpSession session = request.getSession();
		if(session!=null){ //Se la sessione è nulla effettua il redirect alla pagina di autenticazione
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

						rd = sc.getRequestDispatcher("/workers/index.jsp?func=cancella&page=amministratore"); 
						rd.forward(request,response);
					}
					else{
						ris="La matricola non corrisponde ad un amministratore";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita"); 
						rd.forward(request,response);
					}
				}
				else{	//Se gli attributi sono presenti procedo con la cancellazione

					AccessManager AM = new AccessManager();
					AdminManager AdM = new AdminManager();

					Accesso ac = (Accesso) session.getAttribute("acc");
					Amministratore am = (Amministratore) session.getAttribute("amm");

					String matricola = am.getMatricola();
					String login = ac.getLogin();

					String risCanc = AdM.eliminaAmministratore(matricola);	//provo ad effettuare la cancellazione

					if(risCanc.equals("ok")){ // controllo che l'amministratore non è unico ed è stato cancellato
						if(AM.eliminaAccesso(login)){ //elimina l'accesso corrspondente
							ris="ok";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=riuscita"); 
						}
						else{
							ris="fallita";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
						}
					}
					else{
						if(risCanc.equals("unico")) //se l'amministratore è unico non è stato cancellato
							ris="Non si pu˜ cancellare l'ultimo amministratore";
						else
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
