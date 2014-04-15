package smos.exception;

import java.lang.Exception;

/**
  * Questa classe rappresenta l'eccezione generata quando si 
  * tenta di inserire un'entità già presente nel database.
  */
public class DuplicatedEntityException extends Exception {
	
	private static final long serialVersionUID = 4858261134352455533L;

	/**
	 * Genera l'eccezione senza un messagio di errore associato.
	 */
	public DuplicatedEntityException() {
		super("Duplicate Key into the Repository!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public DuplicatedEntityException (String pMessage) {
		super(pMessage);
	}
	
	
}