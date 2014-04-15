package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione generata quando si tenta
  * di inserire un'entitˆ senza specificare un campo obbligatorio
  */
public class MandatoryFieldException extends Exception {
	
	private static final long serialVersionUID = -4818814194670133466L;

	/**
	 * Genera l'eccezione senza un messagio di errore associato.
	 */
	public MandatoryFieldException() {
		super("Mandatory Field Missing!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public MandatoryFieldException(String pMessage) {
		super(pMessage);
	}
	
	
}