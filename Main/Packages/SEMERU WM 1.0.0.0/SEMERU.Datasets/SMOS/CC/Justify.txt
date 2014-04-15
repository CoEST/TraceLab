package smos.bean;

import java.io.Serializable;
import java.util.Date;

public class Justify implements Serializable {

	/**
	 * Classe che modella una giustifica per un assenza
	 * @author Nicola Pisanti
	 * @version 1.0
	 * 
	 */
	private static final long serialVersionUID = -4726381877687167661L;

	private int idJustify;
	private int idUser;
	private Date dateJustify;
	private int academicYear;
	
	/**
	 * Metodo che restituisce l'id della giustifica
	 * @return un intero che rappresenta l'id della giustifica
	 */
	public int getIdJustify() {
		return idJustify;
	}
	/**
	 * Metodo che setta l'id della giustifica
	 * @param un intero che rappresenta l'id da settare
	 */
	public void setIdJustify(int pIdJustify) {
		this.idJustify = pIdJustify;
	}
	/**
	 * Metodo restituisce l'id dello studente relativo alla giustifica
	 * @return un intero che rappresenta l'id dello studente 
	 */
	public int getIdUser() {
		return idUser;
	}
	/**
	 * Metodo che setta l'id dello studente relativo alla giustifica
	 * @param un intero che rappresenta l'id da settare
	 */
	public void setIdUser(int pIdUser) {
		this.idUser = pIdUser;
	}
	/**
	 * Metodo che restituisce la data alla quale è stata giustificata l'assenza
	 * @return una stringa che rappresenta la data giustificata
	 */
	public Date getDateJustify() {
		return dateJustify;
	}
	/**
	 * Metodo che setta la data alla quale è stata giustificata l'assenza
	 * @param una stringa che rappresenta la data da settare
	 */
	public void setDateJustify(Date pDateJustify) {
		this.dateJustify = pDateJustify;
	}
	/**
	 * Metodo che restituisce l'anno accademico relativo alla giustifica
	 * @return un intero che rappresenta l'anno in cui è iniziato l'anno accademico 
	 */
	public int getAcademicYear() {
		return academicYear;
	}
	/**
	 * Metodo che setta l'anno accademico relativo alla giustifica
	 * @param un intero che rappresenta l'anno accademico da settare
	 */
	public void setAcademicYear(int pAcademicYear) {
		this.academicYear = pAcademicYear;
	}
	
	
	
}
