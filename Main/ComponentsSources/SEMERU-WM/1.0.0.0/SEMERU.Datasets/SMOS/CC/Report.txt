package smos.bean;

import java.util.ArrayList;

import smos.exception.InvalidValueException;

/**
 * 
 * Classe utilizzata per modellare una pagella.
 * 
 * @author Luigi Colangelo 
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */
public class Report {
	public ArrayList<Votes> pagella; 
	
	/**
	 * Il costruttore della classe
	 */
	public Report(){
		pagella=new ArrayList<Votes>();
	}
	
	/**
	 * Metodo che restituisce il voto dall'indice nell'array dato in input
	 * @param pInd indice del voto nell'array
	 * @return il voto dell'indice dato
	 * @throws InvalidValueException
	 */
	public Votes getVotes(int pInd) throws InvalidValueException{
		if(pInd<0 || pInd>=pagella.size())throw new InvalidValueException("indice non valido!");
		return (pagella.get(pInd));
	}
	
	/**
	 * Metodo che aggiunge un voto all'array.
	 * @param pVotes il voto da aggiungere
	 * @throws InvalidValueException 
	 */
	public void addVotes(Votes pVotes) throws InvalidValueException{
		if(pVotes==null)throw new InvalidValueException("voto non valido!");
		else pagella.add(pVotes);
	}
	
	/**
	 * Metodo che elimina un voto dall'array
	 * @param pId l'indice del voto da eliminare dall'array.
	 * @throws InvalidValueException 
	 */
	public void remove(int pId) throws InvalidValueException{
		if(pId<0 || pId>=pagella.size())throw new InvalidValueException("indice non valido!");
		pagella.remove(pId);
	}
	
	
	public String ToString(){
		String pag="";
		for(Votes e: pagella){
			pag=pag+"\n"+e.toString();
		}
	return pag;
	}
    
}
