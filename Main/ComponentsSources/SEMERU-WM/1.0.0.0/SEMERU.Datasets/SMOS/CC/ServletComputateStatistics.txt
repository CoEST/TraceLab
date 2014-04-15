package smos.application.registerManagement;

import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Date;
import java.util.Vector;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerClassroom;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

public class ServletComputateStatistics extends HttpServlet {

	private static final long serialVersionUID = 6690162445433486239L;
	
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	@SuppressWarnings("deprecation")
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./statisticsManagement/showStatistictsByAcademicYear.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		Integer academicYear = Integer.valueOf(pRequest.getParameter("academicYear"));
		Integer absenceLimit = Integer.valueOf(pRequest.getParameter("absenceLimit"));
		Integer noteLimit = Integer.valueOf(pRequest.getParameter("noteLimit"));
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		Collection <User> alertStudentAbsence = new Vector<User>();
		
		Date today = new Date();
		
		
		
		int [][] unjustifiedAbsence = null;
		
		Collection <User> alertStudentNote = new Vector<User>();
		
		int [][] note = null;
		
		User tmpUser = null;
		
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
		
		
			
		if (academicYear == 0){
			academicYear = today.getYear() + 1900;
		}
		
		Collection <Integer> academicYearList = ManagerClassroom.getInstance().getAcademicYearList();
		
		unjustifiedAbsence = managerUser.getHighlightsStudentAbsence(academicYear);
		
		if (unjustifiedAbsence != null){
			for (int i=0; i< unjustifiedAbsence.length; i++){
					if (unjustifiedAbsence[i][0] >= absenceLimit){
						tmpUser = managerUser.getUserById(unjustifiedAbsence[i][1]);
						alertStudentAbsence.add(tmpUser);
					}
			}
		}
		
		note = managerUser.getHighlightsStudentNote(academicYear);
		
		if (note != null){
			for (int i=0; i< note.length; i++){
					if (note[i][0] >= noteLimit){
						
							tmpUser = managerUser.getUserById(note[i][1]);
						
						alertStudentNote.add(tmpUser);
					}
			}
		}
		
		
		session.setAttribute("alertStudentAbsence", alertStudentAbsence);
		session.setAttribute("alertStudentNote", alertStudentNote);
		session.setAttribute("academicYearList", academicYearList);
		session.setAttribute("absenceLimit", absenceLimit);
		session.setAttribute("noteLimit", noteLimit);
		session.setAttribute("yearSelected", academicYear);
		
			
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

		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
		}

		session.setAttribute("errorMessage", errorMessage);
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
