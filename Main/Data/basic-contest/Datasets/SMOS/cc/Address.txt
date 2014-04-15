package smos.bean;
import java.io.Serializable;

/**
 *  Classe utilizzata per modellare un indirizzo.
 *
 * 
 */
public class Address implements Serializable{
	
	
	
	/**
	 * 
	 */
	private static final long serialVersionUID = -9194626030239503689L;
	
	private int idAddress;
	private String name;
	
	/**
	 * Il costruttore della classe.
	 */
		public Address(){
		this.idAddress= 0;
	}
		
	/**
	 * @return Ritorna l' id dell' indirizzo.
	 */
		
	public int getIdAddress() {
		return idAddress;
	}
	
	/**
	 * Setta l' id dell' indirizzo.
	 * @param pIdAddress
	 * 			l' id da settare.
	 */
	public void setIdAddress(int pIdAddress) {
		this.idAddress = pIdAddress;
	}
	
	/**
	 * @return Ritorna il nome dell' indirizzo.
	 */
	public String getName() {
		return name;
	}
	
	/**
	 * Setta il nome dell' indirizzo.
	 * @param pName
	 * 			Il nome da settare.
	 */
	public void setName(String pName) {
		this.name = pName;
	}
	
	
}
