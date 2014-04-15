package Manager;

import java.util.Collection;

import Bean.Cittadino;
import DB.DbCittadino;
import DB.DbException;
/**
 * La classe CittadinoManager interagisce con le classi di gestione del database
 * La classe CittadinoManager non ha dipendenze
 * @author Federico Cinque
 */
public class CittadinoManager {

	private DbCittadino dbCittadino;
	/**
	 * Costruttore di default della classe CIManager
	 */
	public CittadinoManager(){
		dbCittadino = new DbCittadino();
	}
	/**
	 * Metodo che permette la ricerca di un cittadino tramite la sua login
	 * invocando il relativo metodo della classe db
	 * @param login è la login in base alla quale si vuole effettuare la ricerca
	 * @return l'oggetto di tipo cittadino
	 * @throws DbException
	 */
	public Cittadino getCittadinoByLogin(String login) throws DbException{
		return dbCittadino.getCittadinoByLogin(login);
	}
	/**
	 * Metodo che permette la modifica della login per uno specifico cittadino
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'id del cittadino
	 * @param newLogin è la nuova login del cittadino
	 * @return true se l'operazione è andata a buon fine, flase altrimenti
	 */
	public boolean modificaLogin(int idCitt, String newLogin)throws DbException{
		return dbCittadino.modificaLogin(idCitt,newLogin);
	}
	/**
	 * Metodo che permette la modifica dell'indirizzo e-mail di uno specifico cittadino
	 * invocando il relativo metodo della classe db
	 * @param idCittadino è l'identificativo del cittadino
	 * @param email è la nuova mail da assegnare al cittadino
	 * @return true se l'operazione è eseguita con successo, flase altrimenti
	 * @throws DbException
	 */
	public boolean modificaEmail(int idCittadino, String email) throws DbException{
		return dbCittadino.modificaEmailCittadino(idCittadino, email);
	}
	/**
	 * Metodo che permette di inserire un nuovo cittadino
	 * invocando il relativo metodo della classe db
	 * @param cittadino è l'istanza di cittadino
	 * @return l'id del cittadino inserito.
	 * @throws DbException
	 */
	public int inserisciCittadino(Cittadino cittadino)throws DbException{
		return dbCittadino.registraCittadino(cittadino);
	}
	/**
	 * Metodo che permette la ricerca di un insieme di cittadini in base al loro nome e cognome
	 * invocando il relativo metodo della classe db
	 * @param nome parametro su cui effettuare la ricerca
	 * @param cognome parametro su cui effettuare la ricerca
	 * @return una collection di cittadini con il nome e il cognome passato come parametro
	 * @throws DbException
	 */
	public Collection<Cittadino> ricercaCittadino(String nome, String cognome)throws DbException{
		return dbCittadino.getCittadinoByName(nome,cognome);
	}
	/**
	 * Metodo che permette la cancellazione di un cittadino
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'identificativo del cittadino
	 * @return true se l'operazione è eseguita con successo, flase altrimenti
	 * @throws DbException
	 */
	public boolean cancellaCittadino(int idCitt)throws DbException{
		return dbCittadino.cancellaCittadino(idCitt);
	}
	/**
	 * Metodo che permette la ricerca di un cittadino tramite il suo id
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'identificativo del cittadino
	 * @return oggetto di tipo cittadino con id uguale a quello passato come parametro
	 * @throws DbException
	 */
	public Cittadino getCittadinoById(int idCitt)throws DbException{
		return dbCittadino.getCittadinoByCodice(idCitt);
	}
	/**
	 * Metodo che modifica il nucleo familiare del cittadino dato il suo id
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'id del cittadino
	 * @param newid è l'id del nuovo nucleo familiare del cittadino
	 * @return true se l'operazione è eseguita con successo, flase altrimenti
	 * @throws DbException
	 */
	public boolean modificaNucleoFamiliare(int idCitt, int newid)throws DbException{
		return dbCittadino.modificaNucleoFamiliareCittadino(idCitt, newid);
	}
	/**
	 * Metodo che permette la modifica del nome di uno specifico cittadino
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'identificativo del cittadino
	 * @param nome è il nuovo nome da assegnare al cittadino
	 * @return true se l'operazione è eseguita con successo, flase altrimenti
	 * @throws DbException
	 */
	public boolean modificaNome(int idCitt, String nome)throws DbException{
		return dbCittadino.modificaNomeCittadino(idCitt, nome);
	}
	/**
	 * Metodo che permette la modifica del cognome di uno specifico cittadino
	 * invocando il relativo metodo della classe db
	 * @param idCitt è l'identificativo del cittadino
	 * @param cognome è il nuovo cognome da assegnare al cittadino
	 * @return true se l'operazione è eseguita con successo, flase altrimenti
	 * @throws DbException
	 */
	public boolean modificaCognome(int idCitt, String cognome)throws DbException{
		return dbCittadino.modificaCognomeCittadino(idCitt, cognome);
	}
}
