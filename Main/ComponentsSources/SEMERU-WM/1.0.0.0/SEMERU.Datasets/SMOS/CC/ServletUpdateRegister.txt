package smos.application.registerManagement;

import java.io.IOException;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Date;
import java.util.Iterator;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import smos.Environment;
import smos.bean.Absence;
import smos.bean.Classroom;
import smos.bean.Delay;
import smos.bean.RegisterLine;
import smos.bean.User;
import smos.bean.UserListItem;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.ManagerRegister;
import smos.storage.ManagerUser;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ServletUpdateRegister extends HttpServlet {

	private static final long serialVersionUID = 5966298318913522686L;
	
	protected void doGet(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		String gotoPage = "./registerManagement/showClassroomList.jsp";
		String errorMessage = "";
		HttpSession session = pRequest.getSession();
		
		//Variabile booleana utilizzata per verificare se lo studente ha o meno un'assenza
		boolean flag = false;
		
		//Collection utilizzata per la memorizzazione del registro di una particolare data
		Collection<RegisterLine> register = null;
		//Iteratore necessario a scorrere la collection
		Iterator itRegister = null;
		//Variabile temporanea necessaria a leggere le informazioni dalla collection
		RegisterLine tmpRegisterLine = null;
		//Variabile temporanea necessaria a leggere le informazioni dalla collection
		UserListItem student = null;
		
		//Variabile temporanea necessaria all'inserimento delle nuove assenze
		Absence tmpAbsence = null;
		
		//Variabile temporanea necessaria all'inserimento di nuovi ritardi
		Delay tmpDelay = null;
		
		//Classi manager necessarie all'elaborazione
		ManagerUser managerUser = ManagerUser.getInstance();
		ManagerRegister managerRegister = ManagerRegister.getInstance();
		
		//Variabili necessarie per la memorizzazione dei dati provenienti dalla request
		String[] absences = null; //Memorizza gli alunni assenti
		String[] delays = null; //Memorizza gli alunni ritardatari
		
		//Recupero l'utente loggato dalla sessione
		User loggedUser = (User) session.getAttribute("loggedUser");
		//Verifico che l'utente loggato abbia i permessi necessari
		try {
			if (loggedUser == null) {
				pResponse.sendRedirect("./index.htm");
				return;
			}

			if (!managerUser.isAdministrator(loggedUser)) {
				errorMessage = "L'Utente collegato non ha accesso alla "
						+ "funzionalita'!";
				gotoPage = "./error.jsp";
				return;
			}
		
		//Recupero i parametri dalla pRequest
		Date date = Utility.String2Date(pRequest.getParameter("date"));
		absences = pRequest.getParameterValues("absences");
		delays = pRequest.getParameterValues("delays");
		
		//Recupero l'oggetto classroom dalla session
		Classroom classroom = ((Classroom) session.getAttribute("classroom"));
		
		/*Invoco il metodo della managerRegister per recuperare dal db le informazioni
		 * inerenti il registro di una classe ad una particolare data (Assenze, Ritardi)
		 */
		register = managerRegister.getRegisterByClassIDAndDate(classroom.getIdClassroom(),date);
			
		if (register != null){
			itRegister = register.iterator();
		}
		
		if (itRegister != null){
			while(itRegister.hasNext()){
				tmpRegisterLine = (RegisterLine) itRegister.next();
				//Recupero lo studente cui la register line si riferisce
				student = tmpRegisterLine.getStudent();
				
				//Verifico se per lo studente e' stata inserita o meno un'assenza
				if (absences != null){
					for (int i=0; i<absences.length; i++){
						if (Integer.valueOf(absences[i]) == student.getId()){
							flag = true;
							if (!managerRegister.hasAbsence(tmpRegisterLine)){
								tmpAbsence = new Absence();
								tmpAbsence.setAcademicYear(classroom.getAcademicYear());
								tmpAbsence.setDateAbsence(date);
								tmpAbsence.setIdJustify(0);
								tmpAbsence.setIdUser(student.getId());
								managerRegister.insertAbsence(tmpAbsence);
							}
						}
						
					}
					if (!flag){
						if (managerRegister.hasAbsence(tmpRegisterLine)){
							managerRegister.deleteAbsence(tmpRegisterLine.getAbsence());
						}
					}
				} else {
					if (managerRegister.hasAbsence(tmpRegisterLine)){
						managerRegister.deleteAbsence(tmpRegisterLine.getAbsence());
					}
				}
				flag = false;
				
				//Verifico se per lo studente e' stata inserito o meno un ritardo
				if (delays != null){
					for (int i=0; i<delays.length; i++){
						if (Integer.valueOf(delays[i]) == student.getId()){
							flag = true;
							if (!managerRegister.hasDelay(tmpRegisterLine)){
								tmpDelay = new Delay();
								tmpDelay.setAcademicYear(classroom.getAcademicYear());
								tmpDelay.setDateDelay(date);
								tmpDelay.setIdUser(student.getId());
								tmpDelay.setTimeDelay(pRequest.getParameter("hour_" + student.getId()));
								managerRegister.insertDelay(tmpDelay);
							} else {
								tmpDelay = tmpRegisterLine.getDelay();
								tmpDelay.setTimeDelay(pRequest.getParameter("hour_" + student.getId()));
								managerRegister.updateDelay(tmpDelay);
							}
							
						}
						
					}
					if (!flag){
						if (managerRegister.hasDelay(tmpRegisterLine)){
							managerRegister.deleteDelay(tmpRegisterLine.getDelay());
						}
					}
				} else {
					if (managerRegister.hasDelay(tmpRegisterLine)){
						managerRegister.deleteDelay(tmpRegisterLine.getDelay());
					}
				}
				flag = false;
			}
		}
			
			

		} catch (IOException ioException) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE
					+ ioException.getMessage();
			gotoPage = "./error.jsp";
			ioException.printStackTrace();
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
		} catch (InvalidValueException e) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE + e.getMessage();
			gotoPage = "./error.jsp";
			e.printStackTrace();
		} catch (MandatoryFieldException e) {
			errorMessage = Environment.DEFAULT_ERROR_MESSAGE + e.getMessage();
			gotoPage = "./error.jsp";
			e.printStackTrace();
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

	protected void doPost(HttpServletRequest pRequest,
			HttpServletResponse pResponse) {
		this.doGet(pRequest, pResponse);
	}

}
