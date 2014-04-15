package Servlet;

import java.io.IOException;
import java.util.ArrayList;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Bean.CartaIdentita;
import Bean.Cittadino;
import DB.DbException;
import Manager.CIManager;
import Manager.CittadinoManager;
/**
 * La classe ServletRicercaCittadino ricerca e restituisce i dati di un cittadino
 * La classe ServletRicercaCittadino non ha dipendenze
 * @author Federico Cinque
 */
public class ServletRicercaCittadino extends HttpServlet{

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		Cittadino c = null;
		ServletContext sc = getServletContext();
		RequestDispatcher rd = null;
		String ris;
		if(session!=null){
			try{
				CittadinoManager CM = new CittadinoManager();
				if(request.getParameter("ci")!=null){
					String cod = request.getParameter("ci").toUpperCase();
					CIManager CIM = new CIManager();
					CartaIdentita CI = CIM.getCartaByNumero(cod);
					if(CI!=null){
						c = CM.getCittadinoById(CI.id());
						session.setAttribute("ci", c);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=risultati");
					}
					else{
						ris="Siamo spiacenti, il codice della carta d'identità nonè presente nel database";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=risultati");
					}
				}
				else{
					String nome = request.getParameter("nome");
					String cognome = request.getParameter("cognome");
					ArrayList<Cittadino> cittadini = (ArrayList<Cittadino>) CM.ricercaCittadino(nome,cognome);
					if(cittadini.size()>0){
						request.setAttribute("ris", cittadini);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=risultati"); 
					}
					else{
						ris="Siamo spiacenti, nessun risultato";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
					}
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
