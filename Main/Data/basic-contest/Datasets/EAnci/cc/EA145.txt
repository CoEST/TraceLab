package Manager;

import Bean.CartaIdentita;
import DB.*;
/**
 * La classe CIManager interagisce con le classi di gestione del database
 * La classe CIManager non ha dipendenze
 * @author Federico Cinque
 */
public class CIManager {
	private DbCartaIdentita dbCartaIdentita;
	/**
	 * Costruttore di default della classe CIManager
	 */
	public CIManager(){
		dbCartaIdentita = new DbCartaIdentita();
	}
	/**
	 * Metodo che permette la ricerca di una carta d'identita tramite il suo numero
	 * invocando il relativo metodo della classe db
	 * @param cod il numero della carta d'identità del cittadino.
	 * @return l'oggetto di tipo CartaIdentità associata al numero passato come parametro
	 * @throws DbException
	 */
	public CartaIdentita getCartaByNumero(String cod)throws DbException{
		return dbCartaIdentita.ricercaCartaIdentitaByNumero(cod);
	}
	
	public CartaIdentita getCartaByIdCStri(int id)throws DbException{
		return dbCartaIdentita.ricercaCartaIdentitaByProprietario(id);
	}
	
}
