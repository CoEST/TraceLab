package smos.application.userManagement;

import smos.Environment;
import smos.bean.Role;
import smos.bean.User;
import smos.bean.UserListItem;
import smos.exception.DeleteManagerException;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Iterator;
import java.util.Vector;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per assegnare uno o piu ruoli ad un utente.
 * 
 * @author Napolitano Vincenzo.
 * 
 */
public class ServletAssignRole extends HttpServlet {

	private static final long serialVersionUID = 537330195407987283L;
	
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "showUserList";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		
		Collection<UserListItem> administrators = new Vector<UserListItem>();
		Iterator<UserListItem> itAdmin = null;
		
		ManagerUser managerUser = ManagerUser.getInstance();

		User loggedUser = (User) session.getAttribute("loggedUser");

		// Verifica che l'utente abbia effettuato il login
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
			
			User user = (User) session.getAttribute("user");
			
			administrators = managerUser.getAdministrators();
			itAdmin = administrators.iterator();
			itAdmin.next();
			
			String[] selectedRoles = pRequest.getParameterValues("selectedRoles");
			String[] unselectedRoles = pRequest.getParameterValues("unselectedRoles");
			
			if (selectedRoles != null) {
				int selectedlength = selectedRoles.length;
				for (int i = 0; i < selectedlength; i++) {
					int role = Integer.valueOf(selectedRoles[i]);
					/*
					 * controlliamo se il ruolo che stiamo assegnando e'
					 * quello di docente*/
					 
					/*if ((role == Role.TEACHER) && (!managerUser.isTeacher(user))){
						gotoPage="./loadYearForTeachings";
						
					}*/
					/*
					 * controlliamo se il ruolo che stiamo assegnando e'
					 * quello di studente*/
					/*if ((role == Role.STUDENT) && (!managerUser.isStudent(user))){
						gotoPage="./showUserList";
						
					} */
					/*
					 * controlliamo se il ruolo che stiamo assegnando e'
					 * quello di genitore*/
					/*if((role==Role.PARENT)&& (!managerUser.isParent(user))){
						gotoPage="./persistentDataManagement/userManagement/showStudentParentForm.jsp";
					}*/
					managerUser.assignRole(user, role);
				}
			} 
			
			if (unselectedRoles != null) {
				int unselectedlength = unselectedRoles.length;
				for (int i = 0; i < unselectedlength; i++) {
					int role = Integer.valueOf(unselectedRoles[i]);
					if ((managerUser.isAdministrator(user))&&(!itAdmin.hasNext())&&(role==Role.ADMIN)) {
						throw new DeleteManagerException ("Impossibile modificare il ruolo dell'utente, e' l'unico Amministratore del sistema! Creare un nuovo Amministratore e riprovare!");
					}
					managerUser.removeRole(user, role);
				}
			}
			
			session.setAttribute("user", user);
		} catch (NumberFormatException numberFormatException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + numberFormatException.getMessage();
			gotoPage = "./error.jsp";
			numberFormatException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (SQLException SQLException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + SQLException.getMessage();
			gotoPage = "./error.jsp";
			SQLException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		} catch (DeleteManagerException deleteManagerException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + deleteManagerException.getMessage();
			gotoPage = "./error.jsp";
			deleteManagerException.printStackTrace();
		} catch (IOException ioException) {
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
