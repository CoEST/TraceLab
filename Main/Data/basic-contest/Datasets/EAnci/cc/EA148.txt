package Bean;

/**
 * La classe Comune permette la comunicazione con gli altri comuni
 * @author Antonio Leone
 * @version 1.0
 *
 */
public class Comune {
	
	private String nome;
	private String indirizzoIp;
	
	/**
	 * Costruttore di default
	 */
	
	public Comune()
	{
		
	}
	
	/**
	 * Costruttore parametrico
	 * @param n nome del comune
	 * @param i indirizzo ip del comune
	 */
	
	public Comune(String n, String i)
	{
		this.nome=n;
		this.indirizzoIp=i;
	}
	
	/**
	 * Preleva il nome del comune
	 * @return Restituisce una stringa che contiene il nome del comune
	 */
	
	public String getNome()
	{
		return this.nome;
	}
	
	/**
	 * Setta il nome del comune
	 * @param n la stringa che contiene il nuovo nome del comune
	 * @return Restituisce il nuovo nome del comune
	 */
	
	public String setNome(String n)
	{
		this.nome=n;
		return n;
	}
	
	/**
	 * Preleva l'indirizo ip del comune
	 * @return Restituisce una stringa che contiene l'indirizzo ip del comune
	 */
	
	public String getIndirizzoId()
	{
		return this.indirizzoIp;
	}
	
	/**
	 * Setta l'indirizzo ip del comune
	 * @param n la stringa che contiene il nuovo indirizzo ip del comune
	 * @return Restituisce il nuovo indirizzo ip del comune
	 */
	
	public String setIndirizzoId(String i)
	{
		this.indirizzoIp=i;
		return i;
	}
	
}