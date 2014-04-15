package Manager;

import java.util.Collection;

import Bean.Accesso;
import DB.DbAccesso;
import DB.DbException;

/**
 * La classe AccessManager interagisce con le classi di gestione del database
 * La classe AccessManager non ha dipendenze
 * @author Federico Cinque
 */
public class AccessManager {
	private DbAccesso dbAccesso;
	/**
	 * Costruttore di default della classe AccessManager
	 */
	public AccessManager(){
		dbAccesso=new DbAccesso();
	}
	/**
	 * Metodo che permette di controllare la correttezza della login e della 
	 * password di un accesso invocando il relativo metodo della classe db
	 * @param login Stringa che viene usata come login
	 * @param password Stringa che viene usata come password
	 * @return True se l'accesso è presente, False altrimenti
	 * @throws DbException
	 */
	public boolean controllaAccesso(String login, String password)throws DbException{
		return dbAccesso.controllaAccesso(login, password);
	}
	/**
	 * Metodo che permette di controllare l'esistenza della login 
	 * invocando il relativo metodo della classe db
	 * @param login Stringa che viene usata come login
	 * @return True se la login è presente, False altrimenti
	 * @throws DbException
	 */
	public boolean controllaLogin(String login)throws DbException{
		return dbAccesso.controllaLogin(login);
	}
	/**
	 * Metodo che restituisce un accesso invocando il relativo metodo della classe db
	 * @param login Stringa che viene usata come login
	 * @return Restituisce un oggetto di tipo Accesso
	 * @throws DbException
	 */
	public Accesso getAccesso(String login)throws DbException{
		return dbAccesso.getAccesso(login);
	}

	public boolean modificaAccesso(String login, Accesso newAccesso)throws DbException{
		return dbAccesso.modificaAccesso(login, newAccesso);
	}
	/**
	 * Metodo che inserisce un accesso all'interno del db 
	 * invocando il relativo metodo della classe db
	 * @param ac Oggetto di tipo Accesso
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	public boolean inserisciAccesso(Accesso ac)throws DbException{
		return dbAccesso.inserisciAccesso(ac);
	}
	/**
	 * Metodo che elimina un accesso  dal db invocando il relativo metodo della classe db
	 * @param login Stringa che viene usata come login
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	public boolean eliminaAccesso(String login)throws DbException{
		return dbAccesso.eliminaAccesso(login);
	}
	/**
	 * Metodo che restituisce tutti gli accessi memorizzati 
	 * invocando il relativo metodo della classe db
	 * @return Restituisce una Collection di Accessi
	 * @throws DbException
	 */
	public Collection<Accesso> getAccessi()throws DbException{
		return dbAccesso.getAccessi();
	}
}
