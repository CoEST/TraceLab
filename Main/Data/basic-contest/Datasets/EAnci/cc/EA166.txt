package Servlet;

import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
/**
 * La classe ServletAnnulla annulla l'operazione che l'utente stava eseguendo
 * La classe ServletAnnulla non ha dipendenze
 * @author Federico Cinque
 */
public class ServletAnnulla extends HttpServlet{
	public void doGet(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		
		session.removeAttribute("amm");
		session.removeAttribute("acc");
		session.removeAttribute("c");
		session.removeAttribute("newCapo");
		session.removeAttribute("citt");
		
		String url="/myDoc/workers/index.jsp";
		response.sendRedirect(url);
	}
}
