package Servlet;

import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import Bean.Cittadino;
/**
 * La classe ServletLogout effettua l'operazione di logout di un utente dal sistema
 * La classe ServletLogout non ha dipendenze
 * @author Federico Cinque
 */
public class ServletLogout extends HttpServlet{
	public void doGet(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		String url;
		Cittadino c = (Cittadino) session.getAttribute("c");

		if(c != null)
			url="/myDoc/user/home.jsp";
		else
			url="/myDoc/workers/Accesso.jsp";
		
		session.invalidate();
		response.sendRedirect(url);
	}
}
