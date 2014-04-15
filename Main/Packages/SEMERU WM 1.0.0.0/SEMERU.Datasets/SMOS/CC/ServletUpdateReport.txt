package smos.application.reportManagement;

import smos.Environment;
import smos.bean.Classroom;
import smos.bean.Teaching;
import smos.bean.User;
import smos.bean.UserListItem;
import smos.bean.Votes;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerTeaching;
import smos.storage.ManagerUser;
import smos.storage.ManagerVotes;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Iterator;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per aggiornare la pagella di uno studente.
 * 
 * @author Giulio D'Amora.
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletUpdateReport extends HttpServlet {

	private static final long serialVersionUID = -1045906657573424217L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./showReports";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		Collection<Teaching> teachingList = null;
		ManagerVotes managerVotes = ManagerVotes.getInstance();
		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerTeaching managerTeaching = ManagerTeaching.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");

		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage = "L'Utente collegato non ha accesso alla funzionalita'!";
				gotoPage = "./error.jsp";
			}
			Classroom classroom = (Classroom) session.getAttribute("classroom");
			// Lista teaching
			teachingList = managerTeaching.getTeachingsByClassroomId(classroom
					.getIdAddress());
			Iterator<Teaching> itTeaching = teachingList.iterator();
			Integer year = (Integer) session.getAttribute("selectedYear");
			UserListItem student = (UserListItem) session
					.getAttribute("student");
			// Quadrimestre
			int turn = (Integer) session.getAttribute("q");
			Teaching teachingTemp = null;
			int idTemp;
			String write, oral, lab;
			gotoPage += "?student=" + student.getId() + "&q=" + turn;
			while (itTeaching.hasNext()) {
				teachingTemp = itTeaching.next();
				idTemp = teachingTemp.getId();
				write = "scritto_" + idTemp;
				oral = "orale_" + idTemp;
				lab = "laboratorio_" + idTemp;
				write = pRequest.getParameter(write);
				oral = pRequest.getParameter(oral);
				lab = pRequest.getParameter(lab);
				Votes newVotes = new Votes();
				// SE il voto non esiste dobbiamo crearlo
				int idVoto = managerVotes.getIdVotes(teachingTemp, year, turn,
						student);
				int writeInt=0, oralInt=0,labInt=0;
				if(write!="")
					writeInt = Integer.valueOf(write);
				if(oral!="")
					oralInt = Integer.valueOf(oral);
				if(lab!="")
					labInt = Integer.valueOf(lab);
				if (idVoto <= 0) {
					if (writeInt != 0 || oralInt != 0 || labInt != 0) {
						newVotes.setAccademicYear(year);
						newVotes.setId_user(student.getId());
						newVotes.setLaboratory(labInt);
						newVotes.setOral(oralInt);
						newVotes.setTeaching(idTemp);
						newVotes.setTurn(turn);
						newVotes.setWritten(writeInt);
						managerVotes.insert(newVotes);
					}
				}
				// Se il voto esiste dobbiamo aggiornarlo
				else {
					if (writeInt != 0 || oralInt != 0 || labInt != 0) {
						newVotes = managerVotes.getVotesById(idVoto);
						newVotes.setLaboratory(labInt);
						newVotes.setOral(oralInt);
						newVotes.setWritten(writeInt);
						managerVotes.update(newVotes);
					}
					else
						managerVotes.delete(managerVotes.getVotesById(idVoto));
				}
			}

			pResponse.sendRedirect(gotoPage);
			return;

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
		} catch (MandatoryFieldException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		pRequest.getSession().setAttribute("errorMessage", errorMessage);
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
