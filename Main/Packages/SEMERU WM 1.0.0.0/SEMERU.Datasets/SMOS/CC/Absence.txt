package smos.bean;

import java.io.Serializable;
import java.util.Date;
public class Absence implements Serializable {

	/**
	 * Classe che modella l'assenza di uno studente
	 * @author Nicola Pisanti
	 * @version 1.0 
	 */
	private static final long serialVersionUID = -8396513309450121449L;
	
	private int idAbsence;
	private int idUser;
	private Date dateAbsence;
	private Integer idJustify;
	private int academicYear;
	
	public Absence (){
		
	}
	
	/**
	 * Metodo che restituisce l'id dell'assenza
	 * @return un intero che rappresenta l'id dell'assenza
	 */
	public int getIdAbsence() {
		return idAbsence;
	}
	/**
	 * Metodo che setta l'id dell'assenza
	 * @param un intero che rappresenta l'id da settare
	 */
	public void setIdAbsence(int pIdAbsence) {
		this.idAbsence = pIdAbsence;
	}
	/**
	 * Metodo che restituisce l'id dello studente relativo all'assenza
	 * @return un intero che rappresenta l'id dello studente assente
	 */
	public int getIdUser() {
		return idUser;
	}
	/**
	 * Metodo che setta l'id dello studente relativo all'assenza
	 * @param un intero che rappresenta l'id da settare
	 */
	public void setIdUser(int pIdUser) {
		this.idUser = pIdUser;
	}
	/**
	 * Metodo che restituisce la data dell'assenza
	 * @return una stringa che rappresenta la data dell'assenza
	 */
	public Date getDateAbsence() {
		return dateAbsence;
	}
	/**
	 * Metodo che setta la data dell'assenza
	 * @param una stringa con la data da settare
	 */
	public void setDateAbsence(Date pDateAbsence) {
		this.dateAbsence = pDateAbsence;
	}
	/**
	 * Metodo che ritorna l'id della giustifica relativa all'assenza
	 * @return un intero che rappresenta l'id della giustifica relativa all'assenza, oppure null se l'assenza non è stata giustificata
	 */
	public Integer getIdJustify() {
		
		return idJustify;
		
	}
	/**
	 * Metodo che setta l'id della giustifica relativa all'assenza
	 * @param un intero che rappresenta l'id della giustifica da settare
	 */
	public void setIdJustify(Integer pIdJustify) {
		this.idJustify = pIdJustify;
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
