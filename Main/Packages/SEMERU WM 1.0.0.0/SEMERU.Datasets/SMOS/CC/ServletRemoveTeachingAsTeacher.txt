package smos.application.userManagement;

import smos.Environment;
import smos.bean.Teaching;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerClassroom;
import smos.storage.ManagerTeaching;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Vector;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per rimuovere Insegnamenti ad un docente
 * 
 * 
 * @author Giulio D'Amora
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletRemoveTeachingAsTeacher extends HttpServlet {




	/**
	 * 
	 */
	private static final long serialVersionUID = -8007609698841510837L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./persistentDataManagement/userManagement/teacherTeachings.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();

		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerTeaching managerTeaching = ManagerTeaching.getInstance();
		ManagerClassroom managerClass = ManagerClassroom.getInstance();
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
			// //Mi servono i 3 Id user class teachings(che non so quanti ne sono)
			int idTeacher = (int) ((User) session.getAttribute("user")).getId();
			User teacher = managerUser.getUserById(idTeacher); 
			int idClass = Integer.valueOf(pRequest.getParameter("classId"));
			String[] idTeachingList = pRequest.getParameterValues("unselectedTeachings");
			int nTeaching =idTeachingList.length;
			int temp;
			//Collection<Teaching> listSelcected = new Vector<Teaching>();
			if(idTeachingList==null)
				gotoPage = "./error.jsp";
			else{
				for(int i=0;i<nTeaching;i++){
					temp = Integer.valueOf(idTeachingList[i]);
					if(managerUser.hasTeaching(teacher,managerTeaching.getTeachingById(temp),managerClass.getClassroomByID(idClass))){
						managerUser.removeTeacherAtClassroomTeaching(teacher,idClass,temp);
						//listSelcected.add(managerTeaching.getTeachingById(temp));
					}
				}
			}
			//session.setAttribute("teachingListTeacher", listSelcected);
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
		} catch (InvalidValueException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
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
