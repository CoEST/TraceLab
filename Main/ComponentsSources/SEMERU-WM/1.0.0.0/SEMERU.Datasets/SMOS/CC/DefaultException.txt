package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione predefinita generata dal sistema.
  */
public class DefaultException extends Exception {
	
	private static final long serialVersionUID = -8985617134055655964L;

	/**
	 * Genera l'eccezione senza un messagio di errore associato.
	 */
	public DefaultException() {
		super();
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public DefaultException(String pMessage) {
		super(pMessage);
	}
	
	
}