package smos.application.userManagement;

import smos.Environment;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerClassroom;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per visualizzare tutti gli anni accademici presenti nel
 * db.
 * 
 * @author Giulio D'Amora
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletLoadYear extends HttpServlet {

	

	/**
	 * 
	 */
	private static final long serialVersionUID = -6304881993204403909L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./persistentDataManagement";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		Collection<Integer> yearList = null;
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if ((!managerUser.isAdministrator(loggedUser)) && (!managerUser.isDirector(loggedUser))) {
				errorMessage = "L'Utente collegato non ha accesso alla "
						+ "funzionalita'!";
				gotoPage = "./error.jsp";
			}
			
			ManagerClassroom managerClassroom = ManagerClassroom.getInstance();
			yearList = managerClassroom.getAcademicYearList();
			gotoPage +=(String) pRequest.getParameter("goTo");
			session.setAttribute("accademicYearList", yearList);
			pResponse.sendRedirect(gotoPage);
			return;
			
		} catch (SQLException sqlException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ sqlException.getMessage();
			gotoPage = "./error.jsp";
			sqlException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		}

		pRequest.getSession().setAttribute("errorMessage", errorMessage);
		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error6.jsp";
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

