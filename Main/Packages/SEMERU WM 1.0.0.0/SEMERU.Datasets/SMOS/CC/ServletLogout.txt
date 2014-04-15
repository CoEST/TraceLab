package smos.application.userManagement;

import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 * Servlet utilizzata per effettuare il logout dell'utente.
 * 
 * @author napolitano Vincenzo.
 *
 */
public class ServletLogout extends HttpServlet {
	private static final long serialVersionUID = 1L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest, HttpServletResponse pResponse) throws ServletException, IOException {
		
		pRequest.getSession().invalidate();
		pResponse.sendRedirect("./index.htm");
	}
	
	/**
	 * Definizione del metodo doPost
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doPost(HttpServletRequest pRequest, HttpServletResponse pResponse) throws ServletException, IOException {
		this.doGet(pRequest, pResponse);
	}
}
