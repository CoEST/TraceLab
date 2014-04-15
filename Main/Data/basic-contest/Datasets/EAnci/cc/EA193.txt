package Servlet;
import Bean.*;
import DB.*;
import java.io.IOException;
import java.util.ArrayList;
import javax.servlet.*;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * La classe ServletStatoDiFamiglia restituisce una lista dei membri di una famiglia
 * La classe dipende dalla classe DbNucleoFamiliare
 * @author Christian Ronca
 */

public class ServletStatoDiFamiglia extends HttpServlet {
	private static final long serialVersionUID = -6835425792119775069L;

	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		int id = Integer.parseInt(request.getParameter("id"));
		//int id = 200101;
		
		HttpSession session = request.getSession(true);
	    ArrayList<Cittadino> arrayList = null;
	    //Richiesta ric = null;
	    
	    
	    try {
	    	DbNucleoFamiliare dbnc =new DbNucleoFamiliare();
	    	arrayList = (ArrayList<Cittadino>)dbnc.getStatoFamiglia(id);
	    	for(int i=0; i<arrayList.size();i++) {
	    		System.out.println(arrayList.get(i).getNome());
	    	}
	    	session.setAttribute("array", arrayList);
	    } catch(Exception e) {
	    	//e.getMessage().toString();
	    	e.printStackTrace();
	    }
	    
	    ServletContext sc = getServletContext();
		RequestDispatcher rd = sc.getRequestDispatcher("/user/home.jsp?func=serv&page=stato_famiglia"); 
		rd.forward(request,response);
	}
}
