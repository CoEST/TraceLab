package smos.application.addressManagement;
import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;

import smos.Environment;
import smos.bean.Address;
import smos.bean.User;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerAddress;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per visualizzare tutti gli indirizzi.
 * 
 * @author Vecchione Giuseppe
 * 
 */
public class ServletShowAddressList extends HttpServlet {

	
	private static final long serialVersionUID = 8797912020763935353L;
	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	
	protected void doGet(HttpServletRequest pRequest, HttpServletResponse pResponse){
		String errorMessage="";
		String gotoPage="./persistentDataManagement/addressManagement/showAddressList.jsp";
		HttpSession session=pRequest.getSession();
		Collection<Address> addressList=null;
		ManagerUser managerUser= ManagerUser.getInstance();
		ManagerAddress managerAddress= ManagerAddress.getInstance();
		User loggedUser = (User)session.getAttribute("loggedUser");
		
		
		try {
			if(loggedUser==null){
				pResponse.sendRedirect("./index.htm");
				return;
				}
			if(!managerUser.isAdministrator(loggedUser)){
				errorMessage="L' utente collegato non ha accesso alla funzionalita'!";
				gotoPage="./error.jsp";
				}
			addressList=managerAddress.getAddressList();
			
			
			session.setAttribute("addressList", addressList);
			pResponse.sendRedirect(gotoPage);
			return;
				
			} catch (IOException ioException) {
				errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
				gotoPage = "./error.jsp";
				ioException.printStackTrace();
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
			} catch (InvalidValueException invalidValueException) {
				errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + invalidValueException.getMessage();
				gotoPage = "./error.jsp";
				invalidValueException.printStackTrace();
			}
		pRequest.getSession().setAttribute("errorMessage",errorMessage);
		
		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage =  Environment.DEFAULT_ERROR_MESSAGE + ioException.getMessage();
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
