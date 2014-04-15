package smos.application.classroomManagement;

import java.io.IOException;
import java.sql.SQLException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.Classroom;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerClassroom;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

public class ServletDeleteClassroom extends HttpServlet {

	/**
	 *  Servlet per cancellare una classe
	 *  
	 *  @author Nicola Pisanti
	 *  @version 0.9
	 */
	private static final long serialVersionUID = 5272269413504847511L;

	
	
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		
		int  aC=0;
		String gotoPage = "./showClassroomList?academicYear="+aC;
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		User loggedUser = (User) session.getAttribute("loggedUser");
		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerClassroom managerClassroom= ManagerClassroom.getInstance();
		
		// Verifica che l'utente abbia effettuato il login
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage =  "L'Utente collegato non ha accesso alla " +
						"funzionalita'!";
				gotoPage = "./error.jsp";
			}
		
			Classroom classroom = (Classroom) session.getAttribute("classroom");
			aC=Integer.valueOf(classroom.getAcademicYear());
			managerClassroom.delete(classroom);
			gotoPage = "./showClassroomList?academicYear="+aC;
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
		} catch (SQLException SQLException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ SQLException.getMessage();
			gotoPage = "./error.jsp";
			SQLException.printStackTrace();
		} catch (MandatoryFieldException mandatoryFieldException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ mandatoryFieldException.getMessage();
			gotoPage = "./error.jsp";
			mandatoryFieldException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		} /*catch (DeleteManagerException deleteManagerException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + deleteManagerException.getMessage();
			gotoPage = "./error.jsp";
			deleteManagerException.printStackTrace();
		} */catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
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
