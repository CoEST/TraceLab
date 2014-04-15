package Servlet;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import Bean.Amministratore;
import DB.DbException;
import Manager.AdminManager;
/**
 * La classe ServletRicercaAmministratore ricerca e restituisce i dati di un amministratore
 * La classe ServletRicercaAmministratore non ha dipendenze
 * @author Federico Cinque
 */
public class ServletRicercaAmministratore extends HttpServlet{

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		if(session!=null){
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				String matricola = request.getParameter("matricola");
				AdminManager AdM = new AdminManager();
				Amministratore A = AdM.ricercaAdminByMatricola(matricola);

				if(A != null){
					request.setAttribute("ris", A);
					rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=datiA"); 
				}
				else{
					ris="Amministratore non trovato";
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