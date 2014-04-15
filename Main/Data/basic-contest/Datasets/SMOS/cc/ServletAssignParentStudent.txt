package smos.application.userManagement;

import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Iterator;
import java.util.Vector;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.User;
import smos.bean.UserListItem;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
/**
 * Servlet  che modifica il record dello studente con l'id del padre.
 * 
 * @author Napolitano Vincenzo.
 * 
 */
public class ServletAssignParentStudent extends HttpServlet {

	
	private static final long serialVersionUID = -4507225018030147979L;
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "showUserList";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		

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
			int idParent=user.getId();
			
			Collection<UserListItem> students = new Vector<UserListItem>();
			Iterator<UserListItem> itStu = null;
			
			students = managerUser.getStudents();
			
			
			String[] selectedStudent = pRequest.getParameterValues("selectedStudent");
			String[] unselectedStudent = pRequest.getParameterValues("unselectedStudent");
			
			if (selectedStudent != null) {
				int selectedlength = selectedStudent.length;
				UserListItem tmp = null;
				User stu=null;
				int idStudent=0;
				for (int i = 0; i < selectedlength; i++) {
					itStu = students.iterator();
					String dd = selectedStudent[i];
					idStudent=Integer.parseInt(dd);
					while(itStu.hasNext()){
						 tmp = (UserListItem)itStu.next();
						if(tmp.getId()==idStudent){	
							stu=managerUser.getUserById(tmp.getId());//recupero userStudente	
							managerUser.assignParent(stu, idParent);
						}
					}	
				}
				
			} else {
				gotoPage="./error.jsp";
			}
			if (unselectedStudent != null) {
				itStu=null;
				int unselectedlength = unselectedStudent.length;
				UserListItem tmp = null;
				User stu=null;
				int idStudent=0;
				for (int i = 0; i < unselectedlength; i++) {
					itStu = students.iterator();
					String dd = unselectedStudent[i];
					idStudent=Integer.parseInt(dd);
					while(itStu.hasNext()){
						 tmp = (UserListItem)itStu.next();
						if(tmp.getId()==idStudent){	
							stu=managerUser.getUserById(tmp.getId());//recupero userStudente	
							managerUser.removeParent(stu);
						}
					}
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
		}  catch (IOException ioException) {
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
