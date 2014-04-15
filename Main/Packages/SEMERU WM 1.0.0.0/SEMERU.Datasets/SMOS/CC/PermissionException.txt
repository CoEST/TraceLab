package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione generata quando un utente
  * tenta di compiere un'operazione per la quale non ha il permesso.
  */
public class PermissionException extends Exception {
	
	private static final long serialVersionUID = 1881009447251825664L;

	/**
	 * Genera l'eccezione senza un messaggio di errore associato.
	 */
	public PermissionException() {
		super("Permission Denied!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public PermissionException(String pMessage) {
		super(pMessage);
	}
	
	
}