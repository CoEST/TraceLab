package smos.exception;
/**
 * Questa classe rappresenta l'eccezione generata quando un utente
 * tenta di eliminare l'unico utente Admin nel database.
 */

public class DeleteAdministratorException extends Exception {

	
	/**
	 * 
	 */
	private static final long serialVersionUID = -2081143475624381775L;

	/**
	 * Genera l'eccezione senza un messaggio di errore associato.
	 * 
	 */
	public DeleteAdministratorException() {
		super("Impossibile eliminare l'utente, l'utente selezionato e' l'unico Admin presente nel database! Creare un nuovo Manager e riprovare!");
	}
	
	/**
	  * Genera l'eccezione con un messagio di errore associato.
	  *
	  * @param pMessage 	Il messaggio di errore che deve essere associato
	  *						all'eccezione.
	  */
	public DeleteAdministratorException(String pMessage) {
		super(pMessage);
	}
}
