package smos.application.reportManagement;

import java.io.IOException;
import java.sql.SQLException;
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
import smos.storage.ManagerVotes;
import smos.storage.connectionManagement.exception.ConnectionException;

public class ServletDeleteReport extends HttpServlet {

	/**
	 * Servlet utilizzata per visualizzare gli alunni associati ad una Classe.
	 * 
	 * @author Giulio D'Amora
	 * 
	 */

	/**
	 * 
	 */
	private static final long serialVersionUID = 2020233250419553067L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./reportsManagement/showStudentsByClass.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		ManagerVotes managerVotes = ManagerVotes.getInstance();
		ManagerUser managerUser = ManagerUser.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		int year= 0;
		int turn=0;
		int studentId=0;
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage = "L'Utente collegato non ha accesso alla "
						+ "funzionalita'!";
				gotoPage = "./error2.jsp";
			}
			UserListItem student = (UserListItem) (session.getAttribute("student"));
			studentId = student.getId();
			year=(Integer) session.getAttribute("selectedYear");
			turn=(Integer) session.getAttribute("q");
			managerVotes.deleteVotesByUserIdYearTurn(studentId, year, turn);
			pResponse.sendRedirect(gotoPage);
			

		} catch (NumberFormatException numberFormatException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ numberFormatException.getMessage();
			gotoPage = "./error1.jsp?Year="+year+"&turn="+turn+"&idStudent="+studentId;
			numberFormatException.printStackTrace();
		} catch (EntityNotFoundException entityNotFoundException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ entityNotFoundException.getMessage();
			gotoPage = "./error3.jsp";
			entityNotFoundException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ connectionException.getMessage();
			gotoPage = "./error4.jsp";
			connectionException.printStackTrace();
		} catch (SQLException SQLException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ SQLException.getMessage();
			gotoPage = "./error5.jsp?Year="+year+"&turn="+turn+"&idStudent="+studentId;
			SQLException.printStackTrace();
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error6.jsp";
			ioException.printStackTrace();
		} catch (InvalidValueException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
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

