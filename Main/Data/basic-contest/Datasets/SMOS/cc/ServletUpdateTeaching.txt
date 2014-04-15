package smos.application.teachingManagement;

import smos.Environment;
import smos.bean.Teaching;
import smos.bean.User;
import smos.exception.DuplicatedEntityException;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerTeaching;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per modificare un insegnamento.
 * 
 * @author Giulio D'Amora.
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletUpdateTeaching extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 943677173076169934L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./teachingList";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		ManagerUser managerUser = ManagerUser.getInstance();
		Teaching teaching = (Teaching) session.getAttribute("teaching");
		ManagerTeaching managerTeaching = ManagerTeaching.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");

		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage = "L'Utente collegato non ha accesso alla "
						+ "funzionalita'!";
				gotoPage = "./error.jsp";
			}

			teaching.setName((pRequest.getParameter("name")));
			teaching.setId(teaching.getId());

			if (!managerTeaching.exists(teaching)) {
				managerTeaching.update(teaching);
			} 
			else if(teaching.getId() == managerTeaching.getTeachingId(teaching))
					managerTeaching.update(teaching);
			else {

				throw new DuplicatedEntityException(
						"Insegnamento già esistente");
			}

		} catch (SQLException SQLException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ SQLException.getMessage();
			gotoPage = "./error.jsp";
			SQLException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (MandatoryFieldException mandatoryFieldException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ mandatoryFieldException.getMessage();
			gotoPage = "./error.jsp";
			mandatoryFieldException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		} catch (DuplicatedEntityException duplicatedEntityException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ duplicatedEntityException.getMessage();
			gotoPage = "./error.jsp";
			duplicatedEntityException.printStackTrace();
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
		}

		session.setAttribute("errorMessage", errorMessage);
		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
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
