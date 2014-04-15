package Servlet;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Bean.Accesso;
import DB.DbException;
import Manager.AccessManager;
/**
 * La classe ServletMostraAccesso mostra i dati relativi all'accesso di un impieagto o amministratore
 * La classe ServletMostraAccesso non ha dipendenze
 * @author Federico Cinque
 */
public class ServletMostraAccesso extends HttpServlet{
	public void doGet(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();

		if(session!=null){
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
			Accesso ac = null;
			AccessManager AM = new AccessManager();
			String login;

			login = (String) session.getAttribute("login");
			ac = AM.getAccesso(login);

			//inviare i dati
			rd = sc.getRequestDispatcher("/workers/index.jsp?func=mostra&page=accesso");

			request.setAttribute("accesso", ac);
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
			String url;
			url="/myDoc/workers/Accesso.jsp";
			response.sendRedirect(url);
		}
	}
}
