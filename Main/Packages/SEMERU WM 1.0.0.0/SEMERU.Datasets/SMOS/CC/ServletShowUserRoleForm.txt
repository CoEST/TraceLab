package smos.application.userManagement;

import smos.Environment;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

import java.io.IOException;
import java.sql.SQLException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per visualizzare il form di gestione
 * dei ruoli degli utenti.
 * 
 * @author Napolitano Vincenzo.
 *
 */
public class ServletShowUserRoleForm extends HttpServlet {

	private static final long serialVersionUID = -2210761175435137331L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		String gotoPage = "./persistentDataManagement/userManagement/userRolez.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		
		
		
		boolean isTeacherBoolean = false;
		boolean isAdministratorBoolean = false;
		boolean isParentBoolean = false;
		boolean isStudentBoolean = false;
		boolean isAtaBoolean = false;
		boolean isDirectorBoolean = false;
		
		int isTeacher = 0;
		int isAdministrator = 0;
		int isDirector = 0;
		int isParent = 0;
		int isStudent = 0;
		int isAta = 0;
		
		User user = null;
		ManagerUser managerUser = ManagerUser.getInstance();
		
		User loggedUser = (User) session.getAttribute("loggedUser");
		
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
		
			if ((!managerUser.isAdministrator(loggedUser))) {
				errorMessage =  "L'Utente collegato non ha accesso alla " +
						"funzionalita'!";
				gotoPage = "./error.jsp";
			}
			user = (User) session.getAttribute("user");
			//prepariamo i valori da passare alla jsp
			isTeacherBoolean = managerUser.isTeacher(user);
			isAdministratorBoolean = managerUser.isAdministrator(user);
			isAtaBoolean = managerUser.isAtaPeople(user);
			isDirectorBoolean= managerUser.isDirector(user);
			isStudentBoolean= managerUser.isStudent(user);
			isParentBoolean= managerUser.isParent(user);
		
			isTeacher = Utility.BooleanToInt(isTeacherBoolean);
			isDirector = Utility.BooleanToInt(isDirectorBoolean);
			isAdministrator = Utility.BooleanToInt(isAdministratorBoolean);
			isAta = Utility.BooleanToInt(isAtaBoolean);
			isStudent = Utility.BooleanToInt(isStudentBoolean);
			isParent = Utility.BooleanToInt(isParentBoolean);
			
			gotoPage = "./persistentDataManagement/userManagement/userRolez.jsp?isTeacher="+isTeacher+"&isAdmin="+isAdministrator+"&isAta="+isAta
			+"&isStudent="+isStudent+"&isParent="+isParent+"&isDirector="+isDirector;
			pResponse.sendRedirect(gotoPage);
			
			return;  
			
		} catch (SQLException sqlException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + sqlException.getMessage();
			gotoPage = "./error.jsp";
			sqlException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		}catch (IOException ioException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		}
		
		pRequest.getSession().setAttribute("errorMessage", errorMessage);
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
