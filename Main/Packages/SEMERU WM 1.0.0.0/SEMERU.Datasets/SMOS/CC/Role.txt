package smos.bean;


import java.io.Serializable;
/**
 *  Classe utilizzata per modellare il ruolo di un utente.
 *
 * @author Bavota Gabriele, Carnevale Filomena.
 * 
 */
public class Role implements Serializable {
	
	private static final long serialVersionUID = 8833734317107515203L;
	
	
	
	/**
	 * L'id del ruolo amministratore
	 */
	public static final int ADMIN = 1;
	
	/**
	 * L'id del ruolo di docente
	 */
	public static final int TEACHER = 2;
	
	/**
	 * L'id del ruolo studente
	 */
	
	public static final int STUDENT	 = 3;
	
	/**
	 * L'id del ruolo geniotore
	 */
	
	public static final int PARENT = 4;
	
	/**
	 * L'id del ruolo ata
	 */
	
	public static final int ATA = 5;
	
	/**
	 * L'id del ruolo direzione
	 */
	
	public static final int DIRECTOR = 6;

}
