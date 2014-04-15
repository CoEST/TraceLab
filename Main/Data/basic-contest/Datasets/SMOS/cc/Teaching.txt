package smos.bean;

import smos.exception.InvalidValueException;

import java.io.Serializable;

/**
 * Classe utilizzata per modellare un insegnamento.
 * 
 * @author Giulio D'Amora
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 * 
 */

public class Teaching implements Serializable {

	private static final long serialVersionUID = 2523612738702790957L;
	private int id_teaching;
	private String name;

	/**
	 * Il costruttore della classe.
	 */
	public Teaching() {
		this.id_teaching = 0;
	}

	/**
	 * Ritorna il nome dell'insegnamento
	 * 
	 * @return Ritorna il nome dell'insegnamento.
	 */
	public String getName() {
		return this.name;
	}

	/**
	 * Setta il nome dell'insegnamento.
	 * 
	 * @param pName
	 *            Il nome da settare.
	 * 
	 * @throws InvalidValueException
	 */
	public void setName(String pName) throws InvalidValueException {
		if (pName.length() <= 4)// da verificare il test
			throw new InvalidValueException();
		else
			this.name = pName;
	}

	/**
	 * Ritorna l'id dell'insegnamento.
	 * 
	 * @return l'id dell'insegnamento.
	 */
	public int getId() {
		return this.id_teaching;
	}

	/**
	 * Setta l'id dell'insegnamento.
	 * 
	 * @param pId
	 *            L'id da settare.
	 */
	public void setId(int pId) {
		this.id_teaching = pId;
	}

}
