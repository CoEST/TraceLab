package smos.application.userManagement;

import smos.Environment;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.LoginException;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

import java.io.IOException;
import java.sql.SQLException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per effettuare il login dell'utente.
 * 
 * @author Napolitano Vincenzo.
 */
public class ServletLogin extends HttpServlet {

	private static final long serialVersionUID = 1L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,HttpServletResponse pResponse) {
		String gotoPage = "";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();		
		
		// Ottengo i dati dalla request
		String login = pRequest.getParameter("user");
		String password = pRequest.getParameter("password");
		
		// Login dell'utente
		try {
			
			ManagerUser managerUser = ManagerUser.getInstance();
			
			if(managerUser.getUserByLogin(login) != null){
			
				User loggedUser = managerUser.login(login, password);
				if (loggedUser != null)
					session.setAttribute("loggedUser", loggedUser);
				else throw new LoginException("Nome Utente e/o Password errati!");
				
				if (managerUser.isAdministrator(loggedUser) ){
					gotoPage="./homePage/homeAdmin.jsp";
				}else if (managerUser.isTeacher(loggedUser) ){ 
					gotoPage="./homePage/homeProfessor.jsp";
				}else if (managerUser.isStudent(loggedUser) ){ 
					gotoPage="./homePage/homeStudent.jsp";
				}else if (managerUser.isParent(loggedUser) ){ 
					gotoPage="./homePage/homeParent.jsp";
				}else if (managerUser.isAtaPeople(loggedUser) ){ 
					gotoPage="./homePage/homeAtaPeople.jsp";
				}else if (managerUser.isDirector(loggedUser) ){ 
					gotoPage="./homePage/homeDirector.jsp";
				}
				
			}
			
		} catch (LoginException loginException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + loginException.getMessage();
			gotoPage = "./error.jsp";
			loginException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (SQLException sqlException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + sqlException.getMessage();
			gotoPage = "./error.jsp";
			sqlException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		} 
		
		session.setAttribute("errorMessage", errorMessage);
		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
		}
	}

	/**
	 * Definizione del metodo doPost
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doPost(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		this.doGet(pRequest, pResponse);
	}

}
