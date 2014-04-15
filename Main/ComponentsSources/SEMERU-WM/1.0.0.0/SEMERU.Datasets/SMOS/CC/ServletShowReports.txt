package smos.application.reportManagement;


import smos.Environment;
import smos.bean.User;
import smos.bean.UserListItem;
import smos.bean.Votes;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerUser;
import smos.storage.ManagerVotes;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per visualizzare tutti gli insegnamenti.
 * 
 * @author Giulio D'Amora.
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletShowReports extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1361713427864776624L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./reportsManagement/showReports.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		Collection<Votes> votesList = null;
		ManagerVotes managerVotes = ManagerVotes.getInstance();
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");

		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if ((!managerUser.isAdministrator(loggedUser)) && (!managerUser.isDirector(loggedUser))) {
				errorMessage = "L'Utente collegato non ha accesso alla funzionalita'!";
				gotoPage = "./error.jsp";
			}
			int studentId = Integer.valueOf(pRequest.getParameter("student"));
			Integer year=(Integer) session.getAttribute("selectedYear");
			Integer turn=Integer.valueOf(pRequest.getParameter("q"));
			session.setAttribute("q", turn);
			votesList = managerVotes.getVotesByUserIdYearTurn(studentId,year,turn);
			User u = (User) managerUser.getUserById(studentId);
			UserListItem st=new UserListItem();
			st.setName(u.getName());
			st.setEMail(u.getEMail());
			st.setId(u.getId());
			session.setAttribute("std", st);
			session.setAttribute("votesList", votesList);
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


