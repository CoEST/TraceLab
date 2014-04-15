package smos.bean;

import java.io.Serializable;
import java.util.Date;

/**
 * Classe che modella una nota sul registro 
 * @author Nicola Pisanti
 * @version 1.0
 */


public class Note implements Serializable{

	private static final long serialVersionUID = 5953926210895315436L;
	
	private int idNote;
	private int idUser;
	private Date dateNote;
	private String description;
	private String teacher;
	private int academicYear;
	
	
	public Note(){
		
	}
	
	
	
	/**
	 * Metodo che restituisce l'id della nota
	 * @return un intero che rappresenta l'id della nota
	 */
	public int getIdNote() {
		return idNote;
	}
	/**
	 * Metodo per settare l'id della nota
	 * @param un intero che rappresenta il nuovo valore dell'id
	 */
	public void setIdNote(int pIdNote) {
		this.idNote = pIdNote;
	}
	/**
	 * Metodo che restituisce l'id dello studente che ha ricevuto la nota
	 * @return l'id dell'utente che ha ricevuto la nota
	 */
	public int getIdUser() {
		return idUser;
	}
	/**
	 * Metodo per settare l'id dello studente che ha ricevuto la nota
	 * @param un intero che rappresenta il nuovo valore dell'id
	 */
	public void setIdUser(int pIdUser) {
		this.idUser = pIdUser;
	}
	/**
	 * Metodo che restituisce una stringa che rappresenta la data in cui è stata data la nota
	 * @return una stringa che rappresenta la data della nota
	 */
	public Date getDateNote() {
		return dateNote;
	}
	/**
	 * Metodo che setta una stringa che rappresenta la data in cui è stata data la nota
	 * @param la stringa che rappresenta la nuova data
	 */
	public void setDateNote(Date pDateNote) {
		this.dateNote = pDateNote;
	}
	/**
	 * Metodo che restituisce il testo della nota 
	 * @return una stringa che rappresenta il testo della nota
	 */
	public String getDescription() {
		return description;
	}
	/**
	 * Metodo che setta la descrizione della nota
	 * @param una stringa che contiene la descrizione della nota
	 */
	public void setDescription(String pDescription) {
		this.description = pDescription;
	}
	/**
	 * Metodo che restituisce l'id dell'insegnante che ha dato la nota 
	 * @return un intero che rappresenta l'id dell'insegnante
	 */
	public String getTeacher() {
		return teacher;
	}
	/**
	 * Metodo che setta l'id dell'insegnante che ha dato la nota 
	 * @param teacher the teacher to set
	 */
	public void setTeacher(String pTeacher) {
		this.teacher = pTeacher;
	}
	/**
	 * Metodo che restituisce l'anno accademico in corso
	 * @return un intero che indica l'anno di inizio delle lezioni 
	 */
	public int getAcademicYear() {
		return academicYear;
	}
	/**
	 * Medoto che setta l'anno accademico in corso durante l'assegnazione della nota
	 * @param un intero che indica l'anno di inizio delle lezioni da inserire
	 */
	public void setAcademicYear(int academicYear) {
		this.academicYear = academicYear;
	}

	
	
	
	
	
}
