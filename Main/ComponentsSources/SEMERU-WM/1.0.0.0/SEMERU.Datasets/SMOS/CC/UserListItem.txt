package smos.bean;

import java.io.Serializable;

/**
 * Classe utilizzata per modellare le informazioni principali 
 * di un utente.
 */
public class UserListItem implements Serializable{

	private static final long serialVersionUID = 3436931564172045464L;

	private String name;
	private String eMail;
	private int id;
	
	
	
	/**
	 * @return Ritorna l'id dell'utente.
	 */
	public int getId() {
		return this.id;
	}
	
	/**
	 * Setta l'id dell'utente.
	 * @param pId
	 * 			L'id da settare.
	 */
	public void setId(int pId) {
		this.id = pId;
	}
	
	/**
	 * @return Ritorna il nome dell'utente.
	 */
	public String getName() {
		return this.name;
	}
	
	/**
	 * Setta il nome dell'utente.
	 * @param pName
	 * 			Il nome da settare.
	 */
	public void setName(String pName) {
		this.name = pName;
	}
	

	/**
	 * @return the eMail
	 */
	public String getEMail() {
		return this.eMail;
	}

	/**
	 * @param mail the eMail to set
	 */
	public void setEMail(String pMail) {
		this.eMail = pMail;
	}
}
