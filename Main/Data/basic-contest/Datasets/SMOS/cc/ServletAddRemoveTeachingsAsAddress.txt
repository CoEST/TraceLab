package smos.application.addressManagement;

import smos.Environment;
import smos.bean.Address;
import smos.bean.User;
import smos.exception.DuplicatedEntityException;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerAddress;
import smos.storage.ManagerTeaching;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import java.io.IOException;
import java.sql.SQLException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

/**
 * Servlet utilizzata per assegnare Insegnamenti ad un docente
 * 
 * 
 * @author Giulio D'Amora
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class ServletAddRemoveTeachingsAsAddress extends HttpServlet {



	
	/**
	 * 
	 */
	private static final long serialVersionUID = -6692711286746163446L;

	/**
	 * Definizione del metodo doGet
	 * 
	 * @param pRequest
	 * @param pResponse
	 * 
	 */
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./showAddressList";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerTeaching managerTeaching = ManagerTeaching.getInstance();
		ManagerAddress managerAddress = ManagerAddress.getInstance();
		User loggedUser = (User) session.getAttribute("loggedUser");
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}
			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage = "L'Utente collegato non ha accesso alla "
					+ "funzionalita'!";
				gotoPage = "./error.jsp";
			}
			// //Mi servono l'id dell'address e le due liste di insegnamenti!
			Address address = (Address) session.getAttribute("address"); 
			String[] idSelectedList = pRequest.getParameterValues("selectedTeachings");
			String[] idUnselectedList = pRequest.getParameterValues("unselectedTeachings");
			int nSelected =idSelectedList.length;
			int nUnselected =idUnselectedList.length;
			int temp;
			//Aggiungiamo gli insegnamenti selezionati!!
				for(int i=0;i<nSelected;i++){
					temp = Integer.valueOf(idSelectedList[i]);
					if(!managerAddress.hasTeaching(managerTeaching.getTeachingById(temp), address)){
						managerAddress.assignTeachingAsAddress(address, managerTeaching.getTeachingById(temp));
					}
				}
			//Rimuoviamo gli insegnamenti non selezionati
				for(int i=0;i<nUnselected;i++){
					temp = Integer.valueOf(idUnselectedList[i]);
					if(managerAddress.hasTeaching(managerTeaching.getTeachingById(temp), address)){
						managerAddress.removeTeachingAsAddress(address, managerTeaching.getTeachingById(temp));
					}
				}
			//session.setAttribute("teachingListTeacher", listSelcected);
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
		} catch (InvalidValueException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (MandatoryFieldException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (DuplicatedEntityException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		pRequest.getSession().setAttribute("errorMessage", errorMessage);
		try {
			pResponse.sendRedirect(gotoPage);
		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
			+ ioException.getMessage();
			gotoPage = "./error6.jsp";
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
