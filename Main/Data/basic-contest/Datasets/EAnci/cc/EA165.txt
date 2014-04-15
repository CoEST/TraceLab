package Bean;


/**
 * Classe che gestisce i metodi dell'oggetto Richiesta
 * @author 	Christian Ronca
 * @version 1.0
 */
public class Richiesta {
	private int idRichiesta;
	private String tipo;
	private String data;
	private int  richiedente;
	private String stato;
	private String documento;
	
	/**
	 * Costruttore di default
	 */
	public Richiesta() {
		
	}
	
	/**
	 * Costruttore paramentrico 
	 * @param idRichiesta 		id della richiesta
	 * @param tipo 				tipo della richiesta 
	 * @param data 				data in cui è stata effettuata la richiesta
	 * @param richiedente 		id del richiedente
	 * @param stato 			stato di avanzamento della richiesta
	 * @param documento 		link al documento richiesto
	 */
	public Richiesta(int idRichiesta, String tipo, String data, int richiedente, String stato, String documento) {
		this.idRichiesta = idRichiesta;
		this.tipo = tipo;
		this.data = data;
		this.richiedente = richiedente;
		this.stato = stato;
		this.documento = documento;
	}
	
	/**
	 * Costruttore paramentrico 
	 * @param tipo 				tipo della richiesta 
	 * @param data 				data in cui è stata effettuata la richiesta
	 * @param richiedente 		id del richiedente
	 * @param stato 			stato di avanzamento della richiesta
	 * @param documento 		link al documento richiesto
	 */
	public Richiesta(String tipo, String data, int richiedente, String stato, String documento) {
		this.tipo = tipo;
		this.data = data;
		this.richiedente = richiedente;
		this.stato = stato;
		this.documento = documento;
	}
	
	/**
	 * Preleva l'id della richiesta
	 * @return	una stringa con l'id della richiesta
	 */
	public int getIdRichiesta() {
		return idRichiesta;
	}
	
	/**
	 * Setta un nuovo id alla richiesta
	 * @param str	prende in input una stringa che contiene il nuovo id della richiesta
	 * @return		una stringa che contiene il nuovo id
	 */
	public int setIdRichiesta(int str) {
		idRichiesta = str;
		return str;
	}
	
	/**
	 * Preleva il tipo di richiesta effettuata
	 * @return	una stringa che contiene il tipo di richiesta
	 */
	public String getTipo() {
		return tipo;
	}
	
	/**
	 * Setta il tipo della richiesta
	 * @param str	prende in input una stringa che contiene il tipo della richiesta
	 * @return		una stringa che contiene il nuovo tipo
	 */
	public String setTipo(String str) {
		tipo = str;
		return str;
	}
	
	/**
	 * Preleva la data in cui è stata fatta la richiesta
	 * @return una stringa che contiene la data della richiesta
	 */
	public String getData() {
		return data;
	}
	
	/**
	 * Setta la data alla richiesta
	 * @param str	prende in input una stringa che contiene la data della richiesta
	 * @return		una stringa che contiene la nuova data
	 */
	public String setData(String str) {
		data = str;
		return str;
	}
	
	/**
	 * Preleva l'id del richiedente che ha effettuato la richiesta
	 * @return	una stringa con l'id della richiesta
	 */
	public int getRichiedente() {
		return richiedente;
	}
	
	/**
	 * Setta il richiedente della richiesta
	 * @param str	prende in input una stringa che contiene il richiedente della richiesta
	 * @return		una stringa che contiene il richiedente
	 */
	public int setRichiedente(int str) {
		richiedente = str;
		return str;
	}
	
	/**
	 * Preleva lo stato della richiesta
	 * @return	una stringa con l'id della richiesta
	 */
	public String getStato() {
		return stato;
	}
	
	/**
	 * Setta lo stato della richiesta
	 * @param str	prende in input una stringa che contiene la data della richiesta
	 * @return		una stringa che contiene la nuova data
	 */
	public String setStato(String str) {
		stato = str;
		return str;
	}
	
	/**
	 * Preleva il link al documento richiesto
	 * @return	una stringa con l'id della richiesta
	 */
	public String getDocumento() {
		return documento;
	}
	
	/**
	 * Setta il link al documento
	 * @param str	prende in input una stringa che contiene il link al documento richiesto
	 * @return		una stringa che contiene il nuovo documento
	 */
	public String setDocumento(String str) {
		documento = str;
		return str;
	}
	
	/**
	 * Restituisce un valore booleano nel caso in cui la richiesta sia stata accettata
	 * @return		true se la richiesta è stata accettata
	 */
	public boolean isAccettata() {
		return stato.equalsIgnoreCase("accettata");
	}
	
	/**
	 * Restituisce un valore booleano nel caso in cui la richiesta sia stata rifiutata
	 * @return		false se la richiesta è stata rifiutata
	 */
	public boolean isRifiutata() {
		return stato.equalsIgnoreCase("rifiutata");
	}
}