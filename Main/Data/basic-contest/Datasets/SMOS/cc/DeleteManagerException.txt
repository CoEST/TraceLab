package smos.exception;

import java.lang.Exception;

/**
 * Questa classe rappresenta l'eccezione generata quando un utente
 * tenta di eliminare l'unico utente Manager nel database.
 */
public class DeleteManagerException extends Exception {

	private static final long serialVersionUID = -6441256751177339494L;
	
	/**
	 * Genera l'eccezione senza un messaggio di errore associato.
	 * 
	 */
	public DeleteManagerException() {
		super("Impossibile eliminare l'utente, l'utente selezionato e' l'unico Manager presente nel database! Creare un nuovo Manager e riprovare!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public DeleteManagerException(String pMessage) {
		super(pMessage);
	}

}
