package smos.application.userManagement;

import smos.Environment;
import smos.bean.User;
import smos.exception.DuplicatedEntityException;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.LoginException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

import java.io.IOException;
import java.sql.SQLException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per inserire un utente nel DataBase.
 * 
 * @author napolitano Vincenzo.
 *
 */
public class ServletInsertUser extends HttpServlet {

	private static final long serialVersionUID = -3860569607870099419L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	public void doGet(HttpServletRequest pRequest, 
			HttpServletResponse pResponse) {
		String gotoPage = "./showUserList";
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
		
			User user = new User();
			user.setFirstName(pRequest.getParameter("firstName"));
			user.setLastName(pRequest.getParameter("lastName"));
			user.setCell(pRequest.getParameter("cell"));
			user.setEMail(pRequest.getParameter("eMail"));
			
			/*
			 * verifichiamo che la login sia unica.
			 */
			String login = pRequest.getParameter("login");
			user.setLogin(login);
			
			if (managerUser.existsLogin(user)) 
				throw new InvalidValueException("La login inserita esiste gia'. Inserire una nuova login.");
			
			if (pRequest.getParameter("password").equals(pRequest.getParameter("password1"))){
						user.setPassword((pRequest.getParameter("password")));
			}
			else{
				throw new LoginException("I valori inseriti per la password non coincidono!");
			}
			//inserimento utente
			if (!managerUser.exists(user)){
				managerUser.insert(user);
				session.setAttribute("user", user);
			
				
			}else 
				throw new DuplicatedEntityException("Utente gia' esistente");
			
		} catch (SQLException SQLException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + SQLException.getMessage();
			gotoPage = "./error.jsp";
			SQLException.printStackTrace();
		} catch (ConnectionException connectionException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + connectionException.getMessage();
			gotoPage = "./error.jsp";
			connectionException.printStackTrace();
		} catch (MandatoryFieldException mandatoryFieldException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + mandatoryFieldException.getMessage();
			gotoPage = "./error.jsp";
			mandatoryFieldException.printStackTrace();
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
		} catch (LoginException loginException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + loginException.getMessage();
			gotoPage = "./error.jsp";
			loginException.printStackTrace();
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
