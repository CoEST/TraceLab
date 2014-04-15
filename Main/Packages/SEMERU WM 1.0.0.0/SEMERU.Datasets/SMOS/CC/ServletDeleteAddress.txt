package smos.application.addressManagement;

import java.io.IOException;
import java.sql.SQLException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.Address;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerAddress;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
/**
 * Servlet utilizzata per cancellare un indirizzo dal database
 * 
 * @author Vecchione Giuseppe
 */
public class ServletDeleteAddress extends HttpServlet {

	private static final long serialVersionUID = -7383336226678925533L;
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest, HttpServletResponse pResponse){
		String errorMessage="";
		String gotoPage="./showAddressList";
		ManagerUser managerUser= ManagerUser.getInstance();
		ManagerAddress managerAddress= ManagerAddress.getInstance();
		HttpSession session= pRequest.getSession();
		User loggedUser= (User)session.getAttribute("loggedUser");
		Address address= null;
		try {
				if(loggedUser==null){		
					pResponse.sendRedirect("./index.htm");
					return;
				}
				if(!managerUser.isAdministrator(loggedUser)){
					errorMessage= "L' utente collegato non ha accesso alla funzionalita'!";
					gotoPage="./error.jsp";
				}
				
				address= (Address)session.getAttribute("address");
				managerAddress.delete(address);
				
		} 	  catch (IOException ioException) {
				errorMessage = Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
				gotoPage = "./error.jsp";
				ioException.printStackTrace();
			} catch (SQLException SQLException) {
				errorMessage = Environment.DEFAULT_ERROR_MESSAGE + SQLException.getMessage();
				gotoPage = "./error.jsp";
				SQLException.printStackTrace();
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
			} catch (MandatoryFieldException mandatoryFieldException) {
				errorMessage = Environment.DEFAULT_ERROR_MESSAGE
				+ mandatoryFieldException.getMessage();
				gotoPage = "./error.jsp";
				mandatoryFieldException.printStackTrace();
			} catch (InvalidValueException invalidValueException) {
				errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
				gotoPage = "./error.jsp";
				invalidValueException.printStackTrace();
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
