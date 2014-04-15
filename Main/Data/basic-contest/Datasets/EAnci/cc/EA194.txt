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
import Bean.*;
import DB.*;

/**
 * La classe ServletVisualizzaPratiche restituisce una lista di richieste
 * La classe dipende dalla classe DbRichieste
 * @author Christian Ronca
 */

public class ServletVisualizzaPratiche extends HttpServlet {
	private static final long serialVersionUID = -6835425792119775069L;

	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		
		HttpSession session = request.getSession(true);
	    ArrayList<Richiesta> arrayList = null;
	    
	    try {
	    	DbRichiesta dbnc =new DbRichiesta();
	    	arrayList = (ArrayList<Richiesta>)dbnc.getRichieste();
	    	session.setAttribute("array", arrayList);
	    } catch(Exception e) {
	    	//e.getMessage().toString();
	    	e.printStackTrace();
	    }
	    
	    session.setAttribute("array", arrayList);
	    ServletContext sc = getServletContext();
		RequestDispatcher rd = sc.getRequestDispatcher("/workers/index.jsp?func=pra&page=visualizza"); 
		rd.forward(request,response);
	}
}
