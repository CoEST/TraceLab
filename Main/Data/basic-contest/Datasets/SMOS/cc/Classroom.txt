package smos.bean;

import java.io.Serializable;

public class Classroom implements Serializable{

	/**
	 * Classe impiegata per modellare una classe 
	 * @author Nicola Pisanti
	 * @version 1.0
	 */
	
	private static final long serialVersionUID = -8295647317972301446L; 

	private int idClassroom; //Id della classe
	private int idAddress;	//Id dell'indirizzo
	private String name;	//Nome della classe
	private int academicYear; //Anno accademico della classe, da inserire l'anno del primo semestre.
	
	
	public Classroom(){
		this.idAddress=0;
		this.idClassroom=0;
		
	}
	
	
	/**
	 * Metodo che restituisce l'anno accademico
	 * @return Un intero che rappresenta l'anno scolastico del primo semestre della classe.
	 */
	public int getAcademicYear() {
		return academicYear;
	}
	
	
	
	/**
	 * Metodo che setta l'anno accademico
	 * @param Il nuovo anno accademico da impostare
	 */
	public void setAcademicYear(int pAcademicYear) {
		this.academicYear = pAcademicYear;
	}
	
	
	/**
	 * Metodo per avere l'ID dell'indirizzo della classe
	 * @return Un intero che rappresenta l'ID dell'indirizzo della classe
	 */
	public int getIdAddress() {
		return idAddress;
	}
	
	
	/**
	 * Metodo che setta l'ID dell'indirizzo della classe
	 * @param Il nuovo ID da settare
	 */
	public void setIdAddress(int pIdAddress) {
		this.idAddress = pIdAddress;
	}
	
	
	/**
	 * Metodo che restituisce l'ID della classe 
	 * @return Un intero che rappresenta l'ID della classe
	 */
	public int getIdClassroom() {
		return idClassroom;
	}
	
	
	/**
	 * Metodo che setta l'ID della classe
	 * @param Il nuovo ID da settare
	 */
	public void setIdClassroom(int pIdClassroom) {
		this.idClassroom = pIdClassroom;
	}
	
	
	/**
	 * Metodo che restituisce il nome della classe
	 * @return Una stringa che rappresenta il nome della classe
	 */
	public String getName() {
		return name;
	}
	
	
	/**
	 * Metodo che setta il nome della classe
	 * @param Il nuovo nome da settare
	 */
	public void setName(String pName) {
		this.name = pName;
	}
	
	
	
	public String toString(){
		
		return (name + " "+ academicYear+ " ID: "+ idClassroom);
	}
	
	
}
