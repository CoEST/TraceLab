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
import Manager.ImpiegatoManager;
import Bean.Accesso;
import Bean.Impiegato;
import DB.DbException;
/**
 * La classe ServletEliminaImpiegato ricerca ed elimina un impiegato
 * La classe ServletEliminaImpiegato non ha dipendenze
 * @author Federico Cinque
 */
public class ServletEliminaImpiegato extends HttpServlet{

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException{
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessione è nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				//Se gli attributi di sessione amm e acc sono nulli devo effettuare la ricerca
				if(session.getAttribute("amm")==null && session.getAttribute("acc")==null){
					String matricola = (String) request.getParameter("matricola");
					ImpiegatoManager IM = new ImpiegatoManager();
					Impiegato imp = IM.ricercaImpiegatoByMatricola(matricola);
					if(imp != null){
					session.setAttribute("amm", imp);

					AccessManager AM = new AccessManager();
					Accesso ac = AM.getAccesso(imp.getLogin());
					session.setAttribute("acc", ac);

					rd = sc.getRequestDispatcher("/workers/index.jsp?func=cancella&page=impiegato"); 
					rd.forward(request,response);
					}
					else{
						ris="La matricola non corrisponde ad un impiegato";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita"); 
						rd.forward(request,response);
					}
				}
				else{	//Se gli attributi sono presenti procedo con la cancellazione

					AccessManager AM = new AccessManager();
					ImpiegatoManager IM = new ImpiegatoManager();

					Impiegato imp = (Impiegato) session.getAttribute("amm");

					String matricola = imp.getMatricola();
					String login = imp.getLogin();
					
					if(IM.eliminaImpiegato(matricola) && AM.eliminaAccesso(login)){ //elimina l'impiegato e l'accesso 
																					//controllando che l'esito sia positivo
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
