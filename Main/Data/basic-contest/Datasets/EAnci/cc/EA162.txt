package Manager;

import java.util.Collection;

import Bean.Impiegato;
import DB.DbException;
import DB.DbImpiegato;
/**
 * La classe ImpiegatoManager interagisce con le classi di gestione del database
 * La classe ImpiegatoManager non ha dipendenze
 * @author Federico Cinque
 */
public class ImpiegatoManager {
	private DbImpiegato dbImpiegato;
	/**
	 * Costruttore di default della classe ImpiegatoManager
	 */
	public ImpiegatoManager(){
		dbImpiegato = new DbImpiegato();
	}
	/**
	 * Metodo che modifica un impiegato
	 * invocando il relativo metodo della classe db
	 * @param matricola la stringa che identifica l'impiegato
	 * @param newImpiegato impiegato con i dati aggiornati
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 */
	public boolean modificaImpiegato(String matricola, Impiegato newImpiegato)throws DbException{
		return dbImpiegato.modificaImpiegato(matricola, newImpiegato);
	}
	/**
	 * Metodo che restituisce un impiegato
	 * invocando il relativo metodo della classe db
	 * @param matricola stringa che viene utilizzato come matricola dell'impiegato
	 * @return Restituisce un oggetto di tipo Impiegato
	 * @throws DbException
	 */
	public Impiegato ricercaImpiegatoByMatricola(String matricola) throws DbException{
		return dbImpiegato.getImpiegatoByMatricola(matricola);
	}
	/**
	 * Metodo che inserisce un impiegato all'interno del db
	 * invocando il relativo metodo della classe db
	 * @param newImpiegato Oggetto di tipo Impiegato
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	public boolean inserisciImpiegato(Impiegato newImpiegato)throws DbException{
		return dbImpiegato.inserisciImpiegato(newImpiegato);
	}
	/**
	 * Metodo che elimina un impiegato  dal db
	 * invocando il relativo metodo della classe db
	 * @param matricola la stringa che viene utilizzato come matricola
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	public boolean eliminaImpiegato(String matricola)throws DbException{
		return dbImpiegato.eliminaImpiegato(matricola);
	}
	/**
	 * Metodo che restituisce un impiegato
	 * invocando il relativo metodo della classe db
	 * @param login stringa che viene utilizzata come login dell'impiegato
	 * @return Restituisce un oggetto di tipo impiegato
	 * @throws DbException
	 */
	public Impiegato getImpiegatoByLogin(String login) throws DbException{
		return dbImpiegato.getImpiegatoByLogin(login);
	}
	/** Metodo che restituisce un insieme di impiegati
	 * invocando il relativo metodo della classe db
	 * @param nomeImp stringa che viene utilizzata come nome dell'impiegato
	 * @param cognImp stringa che viene utilizzata come cognome dell'impiegato
	 * @return Restituisce una Collection di Impiegati
	 * @throws DbException
	 */
	public Collection<Impiegato> getImpiegatoByName(String nomeImp,String cognImp) throws DbException{
		return dbImpiegato.getImpiegatoByName(nomeImp, cognImp);
	}
	/**
	 * Metodo che restituisce tutti gli impiegati memorizzati
	 * invocando il relativo metodo della classe db
	 * @return Restituisce una Collection di impiegati
	 * @throws DbException
	 */
	public Collection<Impiegato> getImpiegati() throws DbException{
		return dbImpiegato.getImpiegati();
	}
}
