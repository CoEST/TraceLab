package smos.application.registerManagement;

import java.io.IOException;
import java.sql.SQLException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.Absence;
import smos.bean.Justify;
import smos.bean.User;
import smos.exception.DuplicatedEntityException;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerRegister;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ServletInsertJustify extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1252760418542867296L;
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	public void doGet(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		String gotoPage = "./registerManagement/showClassroomList.jsp";
		String errorMessage = "";
		
		HttpSession session = pRequest.getSession();
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		
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
		ManagerRegister mR= ManagerRegister.getInstance();
			Justify justify=new Justify();
			justify.setAcademicYear(Integer.parseInt(pRequest.getParameter("academicYear")));
			
			justify.setDateJustify(Utility.String2Date(pRequest.getParameter("date")));
			
			justify.setIdUser(Integer.parseInt(pRequest.getParameter("idUser")));
			String idA= pRequest.getParameter("idAbsence");
			int idAbsence=Integer.parseInt(idA);
			
			//String idC = pRequest.getParameter("idClassroom");
			//int idClassroom= Integer.parseInt(idC);
			
			//gotoPage+=idClassroom;
			Absence absence = mR.getAbsenceByIdAbsence(idAbsence);
			
			if(!mR.exists(absence)){
				errorMessage =  "assenza non prensente nel db!";	
					gotoPage = "./error.jsp";
			}
			
			//inserimento giustifica
			
			if(!mR.exists(justify)){
				mR.insertJustify(justify, absence);
				session.setAttribute("justify", justify);
				
			}else 
				throw new DuplicatedEntityException("Giustifica gia' esistente");
			
		} catch (SQLException SQLException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + SQLException.getMessage();
			gotoPage = "./error.jsp";
			SQLException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + entityNotFoundException.getMessage();
			gotoPage = "./error.jsp";
			entityNotFoundException.printStackTrace();
		} catch (InvalidValueException invalidValueException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
			gotoPage = "./error.jsp";
			invalidValueException.printStackTrace();
		} catch (DuplicatedEntityException duplicatedEntityException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + duplicatedEntityException.getMessage();
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
