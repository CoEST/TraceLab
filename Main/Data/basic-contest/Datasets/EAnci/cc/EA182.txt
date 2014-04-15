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
 * La classe ServletModificaImpiegato che effettua l'operazione di modifica di un impiegato
 * La classe ServletModificaImpiegato non ha dipendenze
 * @author Federico Cinque
 */
public class ServletModificaImpiegato extends HttpServlet{

	private String nome;
	private String cognome;
	private String email;
	private String matricola;
	private String login;
	private String password;
	private String tipo;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessioneè nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				//Se gli attributi di sessione amm e acc sono nulli devo effettuare la ricerca
				if(session.getAttribute("amm")==null && session.getAttribute("acc")==null){
					matricola = request.getParameter("matricola");
					ImpiegatoManager IM = new ImpiegatoManager();
					Impiegato imp = IM.ricercaImpiegatoByMatricola(matricola);
					if(imp != null){
						session.setAttribute("amm", imp);

						AccessManager AM = new AccessManager();
						Accesso ac = AM.getAccesso(imp.getLogin());
						session.setAttribute("acc", ac);

						rd = sc.getRequestDispatcher("/workers/index.jsp?func=modifica&page=impiegato"); 
						rd.forward(request,response);
					}
					else{
						ris="La matricola non corrisponde ad un impiegato";
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
					ImpiegatoManager IM = new ImpiegatoManager();

					Accesso ac = new Accesso(login,password,tipo);
					Impiegato imp = new Impiegato(nome,cognome,matricola,email,login);
					Impiegato impOld = (Impiegato) session.getAttribute("amm");
					
					if(AM.modificaAccesso(impOld.getLogin(), ac) && IM.modificaImpiegato(impOld.getMatricola(), imp)){	//procedo con la modifica dei dati
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
