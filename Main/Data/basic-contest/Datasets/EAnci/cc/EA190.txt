package Servlet;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import Bean.Impiegato;
import DB.DbException;
import Manager.ImpiegatoManager;
/**
 * La classe ServletRicercaImpiegato ricerca e restituisce i dati di un impiegato
 * La classe ServletRicercaImpiegato non ha dipendenze
 * @author Federico Cinque
 */
public class ServletRicercaImpiegato extends HttpServlet{
	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		if(session!=null){
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				String matricola = request.getParameter("matricola");		
				ImpiegatoManager IdM = new ImpiegatoManager();
				Impiegato I = IdM.ricercaImpiegatoByMatricola(matricola);

				sc = getServletContext();
				rd = null;

				if(I!=null){
					request.setAttribute("ris", I);
					rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=datiI"); 
				}
				else{
					ris="Impiegato non trovato";
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
