package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione generata quando un utente
  * inserisce una password errata durante l'autenticazione
  */
public class LoginException extends Exception {
	
	private static final long serialVersionUID = -1213284697567763493L;

	/**
	 * Genera l'eccezione senza un messagio di errore associato.
	 */
	public LoginException() {
		super("Login or Password Incorrect or Invalid Session!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public LoginException(String pMessage) {
		super(pMessage);
	}
	
}