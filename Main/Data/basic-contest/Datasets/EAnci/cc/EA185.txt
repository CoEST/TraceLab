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
import Bean.Cittadino;
import Manager.AccessManager;
/**
 * La classe ServletMostraAccessoA mostra i dati relativi all'accesso di un cittadino
 * La classe ServletMostraAccessoA non ha dipendenze
 * @author Federico Cinque
 */
public class ServletMostraAccessoA extends HttpServlet{
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
				if(session.getAttribute("c")!=null){
					Cittadino c = (Cittadino) session.getAttribute("c");
					login=c.getLogin();
					ac = AM.getAccesso(login);
					rd = sc.getRequestDispatcher("/user/home.jsp?func=mostra&page=accesso");
				}
				request.setAttribute("accesso", ac);
				rd.forward(request,response);
			}
			catch(DbException e){
				ris=e.getMessage();
				request.setAttribute("ris", ris);
				rd=sc.getRequestDispatcher("/user/home.jsp?func=operazione&page=fallita");
				rd.forward(request,response);
			}
		}
		else{
			String url;
			url="/myDoc/user/home.jsp";
			response.sendRedirect(url);
		}
	}
}
