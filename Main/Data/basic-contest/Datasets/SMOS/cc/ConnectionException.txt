package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione generata quando non è possibile
  * ottenere una connessione al database
  */
public class ConnectionException extends Exception {
	
	private static final long serialVersionUID = -6593436034986073011L;

	/**
	 * Genera l'eccezione senza un messagio di errore associato.
	 */
	public ConnectionException() {
		super("Unable to Connect to the DataBase!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public ConnectionException(String pMessage) {
		super(pMessage);
	}
	
	
}