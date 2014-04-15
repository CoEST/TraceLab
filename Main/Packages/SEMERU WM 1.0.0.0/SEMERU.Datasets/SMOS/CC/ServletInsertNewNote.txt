package smos.application.registerManagement;

import java.io.IOException;
import java.sql.SQLException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import smos.Environment;
import smos.bean.Classroom;
import smos.bean.Note;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerRegister;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ServletInsertNewNote extends HttpServlet {

	
	
	
	
	
	/**
	 * 
	 */
	private static final long serialVersionUID = -6496360730201101300L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		String gotoPage = "";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		
		//instanziare gli oggetti qua
		
		ManagerRegister managerRegister = ManagerRegister.getInstance(); 
		
		
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
			//settare le cose da passare alla session, usare session.setAttribute(String, attribute) 
			
			
			if(pRequest.getParameter("insert")==null){
				User student =(User) session.getAttribute("student"); 
					//managerUser.getUserById(Integer.valueOf((String)pRequest.getAttribute("student")));
				session.setAttribute("student", student);
				session.setAttribute("idStudent", student.getId());
				gotoPage="./registerManagement/insertNewNote.jsp";
			}else{
				
				
				Note nNote= new Note();
				nNote.setAcademicYear(((Classroom) session.getAttribute("classroom")).getAcademicYear());
				nNote.setDateNote(Utility.String2Date(pRequest.getParameter("dateNote")));
				nNote.setIdUser((Integer) session.getAttribute("idStudent"));
				nNote.setTeacher(pRequest.getParameter("noteTeacher"));
				nNote.setDescription(pRequest.getParameter("noteDescription"));
				
				try{
					managerRegister.insertNote(nNote);
					gotoPage="./showNoteList?student="+session.getAttribute("idStudent");
				}catch(MandatoryFieldException e){
					session.setAttribute("error", e.getMessage());
					gotoPage="./registerManagement/insertNewNote.jsp";
				}				
				
			}
			
			
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
		}catch (InvalidValueException invalidValueException) {
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
