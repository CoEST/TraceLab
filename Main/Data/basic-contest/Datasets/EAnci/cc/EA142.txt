package Manager;

import java.util.Collection;

import Bean.Amministratore;
import DB.DbAmministratore;
import DB.DbException;
/**
 * La classe AdminManager interagisce con le classi di gestione del database
 * La classe AdminManager non ha dipendenze
 * @author Federico Cinque
 */
public class AdminManager {
	private DbAmministratore dbAmministratore;
	/**
	 * Costruttore di default della classe AdminManager
	 */
	public AdminManager(){
		dbAmministratore = new DbAmministratore();
	}
	/**
	 * Metodo che modifica un amministratore
	 * invocando il relativo metodo della classe db
	 * @param matricola la stringa che identifica l'amministratore
	 * @param newAdmin Amministratore con i dati aggiornati
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 */
	public boolean modificaAdmin(String matricola, Amministratore newAdmin)throws DbException{
		return dbAmministratore.modificaAmministratore(matricola, newAdmin);
	}
	/**
	 * Metodo che restituisce un amministratore
	 * invocando il relativo metodo della classe db
	 * @param matricola stringa che viene utilizzato come matricola dell'amministratore
	 * @return Restituisce un oggetto di tipo Amministratore
	 * @throws DbException
	 */
	public Amministratore ricercaAdminByMatricola(String matricola)throws DbException{
		return dbAmministratore.getAmministratoreByMatricola(matricola);
	}
	/**
	 * Metodo che inserisce un amministratore all'interno del db
	 * invocando il relativo metodo della classe db
	 * @param newAdmin Oggetto di tipo Amministratore
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	public boolean inserisciAdmin(Amministratore newAdmin)throws DbException{
		return dbAmministratore.inserisciAmministratore(newAdmin);
	}
	/**
	 * Metodo che elimina un Amministratore  dal db
	 * invocando il relativo metodo della classe db
	 * @param matricola l'intero che viene utilizzato come matricola
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	public String eliminaAmministratore(String matricola)throws DbException{
		Collection<Amministratore> Amministratori = dbAmministratore.getAmministratori();
		if(Amministratori.size()>1){
			if(dbAmministratore.eliminaAmministratore(matricola))
				return "ok";
			else
				return "errore";
		}
		else
			return "unico";
	}
	/**
	 * Metodo che restituisce un amministratore
	 * invocando il relativo metodo della classe db
	 * @param login stringa che viene utilizzata come login dell'amministratore
	 * @return Restituisce un oggetto di tipo amministratore
	 * @throws DbException
	 */
	public Amministratore getAmministratoreByLogin(String login) throws DbException{
		return dbAmministratore.getAmministratoreByLogin(login);
	}
	/** Metodo che restituisce un insieme di amministratori
	 * invocando il relativo metodo della classe db
	 * @param nomeAmm stringa che viene utilizzata come nome dell'amministratore
	 * @param cognAmm stringa che viene utilizzata come cognome dell'amministratore
	 * @return Restituisce una Collection di Amministratori
	 * @throws DbException
	 */
	public Collection<Amministratore> getAmministratoreByName(String nomeAmm,String cognAmm) throws DbException{
		return dbAmministratore.getAmministratoreByName(nomeAmm, cognAmm);
	}
	/**
	 * Metodo che restituisce tutti gli amministratori memorizzati
	 * @return Restituisce una Collection di Amministratori
	 * @throws DbException
	 */
	public Collection<Amministratore> getAmministratori() throws DbException{
		return dbAmministratore.getAmministratori();
	}
}
