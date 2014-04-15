package smos.application.userManagement;

import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Iterator;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.Classroom;
import smos.bean.Teaching;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerClassroom;
import smos.storage.ManagerTeaching;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ServletShowUserTeachingForm extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 2305151029867525356L;
	
	
	protected void doGet(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		String gotoPage = "./persistentDataManagement/userManagement/showTeacherDetails.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		
		User user = null;
		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerClassroom managerClassroom = ManagerClassroom.getInstance();
		
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
			
			if(!managerUser.isTeacher(user)){
				errorMessage =  "L'Utente non è un docente";
				gotoPage = "./error.jsp";
				
			}
			//int idTeacher= user.getId();
			
			Collection<Classroom> classList = managerClassroom.getClassroomsByTeacher(user);
			
			/*
			Iterator<Classroom> iteClass = classList.iterator();
			Classroom tmp = null;
			while(iteClass.hasNext()){
				tmp=iteClass.next();
				if(tmp.getAcademicYear()!= an){
					classList.remove(tmp);
				}
			}*/
			//@SuppressWarnings("unused")
			//Collection<Teaching> teachingListByClassroom=null;
			//Collection<Classroom,Teaching> list= new Vector <Classroom,Teaching>();
			
			session.setAttribute("classroomList", classList);
			
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
		} catch (MandatoryFieldException mandatoryFieldException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + mandatoryFieldException.getMessage();
			gotoPage = "./error.jsp";
			mandatoryFieldException.printStackTrace();
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
