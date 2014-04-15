package smos.bean;

import java.io.Serializable;

import smos.exception.InvalidValueException;

/**
 * 
 * Classe utilizzata per modellare una voto.
 * 
 * @author Luigi Colangelo 
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class Votes implements Serializable {

	/**
	 * Classe utilizzata per modellare un voto
	 * 
	 */
	private static final long serialVersionUID = 3014235634635608576L;
    private int id_votes;
    private int id_user;
    private int teaching;
    private int written;
    private int oral;
    private int laboratory;
    private int accademicYear;
    private int turn;
    
    /**
     * Il costruttore della classe
     */
    public Votes(){
    	id_votes=0;
    }
    
    /**
     * Metodo che restituisce l'id del voto.
     * @return l'id del voto
     */
	public int getId_votes() {
		return id_votes;
	}
	
	/**
	 * Metodo che setta l'id del voto
	 * @param pId_votes
     *             l'id da settare
	 */
	public void setId_votes(int pId_votes) {
		this.id_votes = pId_votes;
	}
	
	/**
	 * Metodo che restituisce l'id dell'utente collegato al voto
	 * @return l'id dell'utente
	 */
	public int getId_user() {
		return id_user;
	}
	
	/**
	 * Metodo che setta l'id dell'utente relativo al voto
	 * @param pId_user
	 *               l'id da settare
	 */
	public void setId_user(int pId_user) {
		this.id_user = pId_user;
	}
	
	/**
	 * metodo che restituisce il codice dell'insegnamento del voto
	 * @return il metodo dell'insegnamento
	 */
	public int getTeaching() {
		return teaching;
	}
	
	/**
	 * Metodo che setta il codice dell'insegnamento relativo al voto
	 * @param pTeaching
	 *              il codice dell'insegnamento
	 */
	public void setTeaching(int pTeaching) {
		this.teaching = pTeaching;
	}
	
	/**
	 * Metodo che restituisce il voto dello scritto 
	 * @return il voto nello scritto 
	 */
	public int getWritten() {
		return written;
	}
	
	/**
	 * Metodo che setta il voto dello scritto, controllando che esso sia compreso tra 0 e 10
	 * @param pWritten
	 */
	public void setWritten(int pWritten) throws InvalidValueException {
		if (pWritten < 0 || pWritten > 10)
			throw new InvalidValueException();
		else
		this.written = pWritten;
	}
	
	/**
	 *  metodo che restituisce il voto dell' orale
	 * @return il voto dell'orale
	 */
	public int getOral() {
		return oral;
	}
	
	/**
	 * Metodo che setta il voto dell'orale, controllando che esso sia compreso tra 0 e 10 
	 * @param pOral
	 *            il voto dell'orale da settare
	 */
	public void setOral(int pOral) throws InvalidValueException{
		if (pOral < 0 || pOral > 10)
			throw new InvalidValueException();
		else
		this.oral = pOral;
	}
	
	/**
	 * Metodo che restituisce il voto del laboratorio
	 * @return il voto del laboratorio
	 */
	public int getLaboratory() {
		return laboratory;
	}
	
	/**
	 * metodo che setta il voto del laboratorio, controllando che esso sia compreso tra 0 e 10
	 * @param pLaboratory
	 *                 il voto del laboratorio da settare
	 */
	public void setLaboratory(int pLaboratory)throws InvalidValueException {
		if (pLaboratory < 0 || pLaboratory > 10)
			throw new InvalidValueException();
		else
		this.laboratory = pLaboratory;
	}
	
	/**
	 * Metodo che restituisce l'anno accademico del voto
	 * @return l'anno accademico
	 */
	public int getAccademicYear() {
		return accademicYear;
	}
	
	/**
	 * metodo che setta l'anno accademico del voto
	 * @param pAccademicYear
	 */
	public void setAccademicYear(int pAccademicYear) {
		this.accademicYear = pAccademicYear;
	}
	
	/**
	 * Metodo che restituisce il quadrimestre del voto 
	 * @return il semestre del voto (0 o 1)
	 */
	public int getTurn() {
		return turn;
	}
	
	/**
	 * Metodo che setta il quadrimestre del voto
	 * @param pTurn
	 *            il semestre del voto da settare
	 */
	public void setTurn(int pTurn) {
		this.turn = pTurn;
	}
	
	public String toString(){
		return("id voto= "+id_votes+" id user= "+id_user+" id insegnamento= "+teaching+" scritto= "+written+" orale= "+oral+" laboratorio= "+laboratory+" anno accademico= "+accademicYear+" quadrimestre= "+turn);
	}
    
 
}
