package smos.bean;

import java.io.Serializable;
import java.util.Date;

public class Delay implements Serializable {

	/**
	 * Classe che modella un entrata in ritardo di uno studente
	 * @author Nicola Pisanti
	 * @version 1.0 
	 */
	private static final long serialVersionUID = 78680884161960621L;

	private int idDelay;
	private int idUser;
	private Date dateDelay;
	private String timeDelay;
	private int academicYear;
	
	
	/**
	 * Metodo che restituisce l'id del Ritardo 
	 * @return un intero che rappresenta l'id del ritardo 
	 */
	public int getIdDelay() {
		return idDelay;
	}
	/**
	 * Metodo che setta l'id del Ritardo 
	 * @param intero che rappresenta l'id da settare
	 */
	public void setIdDelay(int pIdDelay) {
		this.idDelay = pIdDelay;
	}
	/**
	 * Metodo che restituisce l'id dello studente ritardatario 
	 * @return un intero che rappresenta l'id dello studente
	 */
	public int getIdUser() {
		return idUser;
	}
	/**
	 * Metodo che setta l'id dello studente relativo al ritardo 
	 * @param un intero che rappresenta l'id da settare
	 */
	public void setIdUser(int pIdUser) {
		this.idUser = pIdUser;
	}
	/**
	 * Metodo che restituisce la data del ritardo 
	 * @return una stringa che rappresenta la data del ritardo 
	 */
	public Date getDateDelay() {
		return dateDelay;
	}
	/**
	 * Metodo che setta la data del ritardo
	 * @param una stringa che rappresenta la data del ritardo
	 */
	public void setDateDelay(Date pDateDelay) {
		this.dateDelay = pDateDelay;
	}
	/**
	 * Metodo che restituisce l'ora d'entrata dello studente
	 * @return una stringa che rappresenta l'ora di entrata dello studente ritardatario
	 */
	public String getTimeDelay() {
		if (this.timeDelay.length() > 0){
			return timeDelay.substring(0, 5);
		} else {
			return this.timeDelay;
		}
	}
	/**
	 * Metodo che setta l'ora di entrata dello studente 
	 * @param una stringa che rappresenta l'ora di entrata da settare
	 */
	public void setTimeDelay(String pTimeDelay) {
		this.timeDelay = pTimeDelay;
	}
	/**
	 * Metodo che restituisce l'anno accademico relativo all'assenza
	 * @return un intero che rappresenta l'anno in cui è iniziato l'anno accademico 
	 */
	public int getAcademicYear() {
		return academicYear;
	}
	/**
	 * Metodo che setta l'anno accademico relativo all'assenza
	 * @param un intero che rappresenta l'anno accademico da settare
	 */
	public void setAcademicYear(int pAcademicYear) {
		this.academicYear = pAcademicYear;
	}
}
