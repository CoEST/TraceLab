package smos.bean;

import java.io.Serializable;

public class RegisterLine implements Serializable {

	/**
	 * Classe che modella una linea del registro 
	 * @author Nicola Pisanti
	 * @version 1.0 
	 */
	private static final long serialVersionUID = -6010085925185873838L;
	
	private UserListItem student;
	private Absence absence;
	private Delay delay;
	
	public RegisterLine(){
		absence=null;
		delay=null;
		
	}
	
	/**
	 * Metodo che restituisce lo studente di questa riga del registro
	 * @return un oggetto di tipo User che rappresenta lo studente
	 */
	public UserListItem getStudent() {
		return student;
	}
	/**
	 * Metodo che setta lo studente di questa riga del registro
	 * @param un oggetto di tipo User che rappresenta lo studente da inserire
	 */
	public void setStudent(UserListItem student) {
		this.student = student;
	}
	/**
	 * Metodo che restituisce l'assenza dello studente di questa riga del registro
	 * @return un oggetto di tipo Absence che rappresenta l'assenza, oppure null se lo studente era presente
	 */
	public Absence getAbsence() {
		return absence;
	}
	/**
	 * Metodo che setta l'assenza dello studente di questa riga del registro 
	 * @param un oggetto di tipo Absence da settare
	 */
	public void setAbsence(Absence absence) {
		this.absence = absence;
	}
	/**
	 * Metodo che restituisce il ritardo dello studente di questa riga del registro 
	 * @return un oggetto di tipo Delay che rappresenta il ritardo, oppure null se lo studente era arrivato in orario o era assente
	 */
	public Delay getDelay() {
		return delay;
	}
	/**
	 * Metodo che setta il ritardo dello studente di questa riga del registro 
	 * @param un oggetto di tipo Delay da settare
	 */
	public void setDelay(Delay delay) {
		this.delay = delay;
	}

}
