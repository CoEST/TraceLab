	package smos.bean;

import smos.exception.InvalidValueException;

import java.io.Serializable;

/**
 *  Classe utilizzata per modellare un utente.
 *
 * 
 */
public class User implements Serializable{
	

	private static final long serialVersionUID = 7272532316912745508L;
	
	
	private int id;
	private String login;
	private String password;
	private String firstName;
	private String lastName;
	private int idParent;
	private String cell;
	private String eMail;
	
	/**
	 * Il costruttore della classe.
	 */
	public User(){
		this.id = 0 ;
	}
	
	
	/**
	 * @return Ritorna la login dell'utente.
	 */
	public String getLogin() {
		return this.login;
	}
	
	/**
	 * Setta la login dell'utente.
	 * @param pLogin
	 * 			La login da settare.
	 * 
	 * @throws InvalidValueException 
	 */
	public void setLogin(String pLogin) throws InvalidValueException {
		if(pLogin.length()<=4)
			throw new InvalidValueException();
		else
			this.login = pLogin;
	}
	
	/**
	 * @return Ritorna il nome dell'utente.
	 */
	public String getName() {
		return this.lastName + " " + this.firstName;
	}
	
	/**
	 * @return Ritorna il nome dell'utente.
	 */
	public String getFirstName() {
		return this.firstName;
	}
	
	/**
	 * Setta il nome dell'utente.
	 * @param pFirstName
	 * 			Il nome da settare.
	 */
	public void setFirstName(String pFirstName) {
		this.firstName = pFirstName;
	}
	
	
	/**
	 * @return Ritorna la password dell'utente.
	 */
	public String getPassword() {
		return this.password;
	}
	
	/**
	 * Setta la password dell'utente.
	 * @param pPassword
	 * 			La password da settare.
	 * 
	 * @throws InvalidValueException 
	 */
	public void setPassword(String pPassword) throws InvalidValueException {
		if(pPassword.length()<=4)
			throw new InvalidValueException();
		else
		this.password = pPassword;
	}
	
	/**
	 * @return Ritorna il cognome dell'utente.
	 */
	public String getLastName() {
		return this.lastName;
	}
	
	/**
	 * Setta il cognome dell'utente.
	 * @param pLastName
	 * 			Il cognome da settare.
	 */
	public void setLastName(String pLastName) {
		this.lastName = pLastName;
	}
	
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
	 * Ritorna una stringa contenente nome e cognome dell'utente.
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return this.getFirstName() 
		+ " " 
		+ this.getLastName();
	}
	
	/**
	 * @return the eMail
	 */
	public String getEMail() {
		return this.eMail;
	}
	/**
	 * @param pMail the eMail to set
	 */
	public void setEMail(String pMail) {
		this.eMail = pMail;
	}


	/**
	 * @return the cell
	 */
	public String getCell() {
		return this.cell;
	}


	/**
	 * @param cell the cell to set
	 */
	public void setCell(String pCell) {
		this.cell = pCell;
	}


	/**
	 * @return the idParent
	 */
	public int getIdParent() {
		return this.idParent;
	}


	/**
	 * @param idParent the idParent to set
	 */
	public void setIdParent(int pIdParent) {
		this.idParent = pIdParent;
	}
	
}
