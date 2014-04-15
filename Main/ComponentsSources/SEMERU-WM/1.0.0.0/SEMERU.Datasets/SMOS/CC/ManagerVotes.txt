package smos.storage;

import smos.bean.Teaching;
import smos.bean.UserListItem;
import smos.bean.Votes;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.connectionManagement.DBConnection;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Vector;
import java.sql.Connection;

/**
 * 
 * Classe manager dei voti.
 * 
 * @author Luigi Colangelo
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */

public class ManagerVotes {
	private static ManagerVotes instance;

	/**
	 * Il nome della tabella dei voti.
	 */
	public static final String TABLE_VOTES = "votes";

	/**
	 * Il costruttore della classe.
	 */
	public ManagerVotes() {
		super();
	}

	/**
	 * Ritorna la sola istanza del voto esistente.
	 * 
	 * @return Ritorna l'istanza della classe.
	 */
	public static synchronized ManagerVotes getInstance() {
		if (instance == null) {
			instance = new ManagerVotes();
		}
		return instance;
	}

	/**
	 * Verifica l'esistenza di voto nel database.
	 * 
	 * @param pVotes
	 *            il voto da controllare.
	 * @return Ritorna true se esiste il voto passato come parametro,
	 *         false altrimenti.
	 * 
	 * @throws MandatoryFieldException
	 * @throws SQLException
	 * @throws ConnectionException
	 */
	public synchronized boolean exists(Votes pVotes)
			throws MandatoryFieldException, ConnectionException, SQLException {

		boolean result = false;
		Connection connect = null;

		if (pVotes.getId_votes() == 0)
			throw new MandatoryFieldException("Specificare l'id.");
		try {
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " + ManagerVotes.TABLE_VOTES
					+ " WHERE id_votes = " + Utility.isNull(pVotes.getId_votes());

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, sql);

			if (tRs.next())
				result = true;

			return result;

		} finally {
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Inserisce un nuovo voto nella tabella Votes.
	 * 
	 * @param pVotes
	 *            il voto da inserire.
	 * 
	 * @throws SQLException
	 * @throws ConnectionException
	 * @throws MandatoryFieldException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insert(Votes pVotes)
			throws MandatoryFieldException, ConnectionException, SQLException,
			EntityNotFoundException, InvalidValueException {
		Connection connect = null;
		try {
			

			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();
			// Prepariamo la stringa Sql
			String sql = "INSERT INTO " + ManagerVotes.TABLE_VOTES
			        + " (id_user, id_teaching, written, oral, laboratory, AccademicYear, turn) "
					+ "VALUES ("
					+ Utility.isNull(pVotes.getId_user())
					+", "
					+ Utility.isNull(pVotes.getTeaching()) 
					+", "
					+ Utility.isNull(pVotes.getWritten())
					+", "
					+ Utility.isNull(pVotes.getOral())
					+", "
					+ Utility.isNull(pVotes.getLaboratory())
					+", "
					+ Utility.isNull(pVotes.getAccademicYear())
					+", "
					+ Utility.isNull(pVotes.getTurn())+ " )";

			Utility.executeOperation(connect, sql);

			pVotes.setId_votes(Utility.getMaxValue("id_votes",
					ManagerVotes.TABLE_VOTES));

		} finally {
			// rilascia le risorse

			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Aggiorna un voto presente nella tabella votes.
	 * 
	 * @param pVotes
	 *            Un voto da modificare
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	public synchronized void update(Votes pVotes)
			throws ConnectionException, SQLException, EntityNotFoundException,
			MandatoryFieldException {
		Connection connect = null;

		try {
			if (pVotes.getId_votes() <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare il voto!");

			if (pVotes.getId_user() <= 0)
				throw new MandatoryFieldException("Specificare l'user del voto");
			if (pVotes.getTeaching() <= 0)
				throw new MandatoryFieldException("Specificare l'insegnamento del voto");
			if (pVotes.getAccademicYear() <= 0)
				throw new MandatoryFieldException("Specificare l'anno accademico");
			if (pVotes.getTurn() < 0)
				throw new MandatoryFieldException("Specificare il semestre ");
			// Prepariamo la stringa SQL
			String sql = "UPDATE " + ManagerVotes.TABLE_VOTES + " SET"
					+ " id_user = " + Utility.isNull(pVotes.getId_user())+","+" id_teaching= "
					+ Utility.isNull(pVotes.getTeaching())+","+" written= "
					+ Utility.isNull(pVotes.getWritten())+","+" oral= "
					+ Utility.isNull(pVotes.getOral())+","+" laboratory= "
					+ Utility.isNull(pVotes.getLaboratory())+","+" accademicYear= "
					+ Utility.isNull(pVotes.getAccademicYear())+","+" turn="
					+ Utility.isNull(pVotes.getTurn())
					+ " WHERE id_votes = "
					+ Utility.isNull(pVotes.getId_votes());

			// effettua una nuova connessione e invia la query
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			Utility.executeOperation(connect, sql);
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Verifica se uno studente passato come parametro ha un voto assegnato nell'insegnamento passato
	 * come parametro nell'anno passato come parametro e nel quadrimestre passato come parametro
	 * 
	 * 
	 * @param pTeaching
	 *            L'insegnamento da controllare.
	 * @param pUserListItem
	 *            Lo studente da controllare
	 * 
	 * @return Ritorna l'id del voto -1 altrimenti
	 * 
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws InvalidValueException
	 */
	public synchronized int getIdVotes(Teaching pTeaching, int academicYear, int turn, UserListItem pUser)
			throws SQLException, EntityNotFoundException, ConnectionException,
			InvalidValueException {
		Connection connect = null;
		int result = -1;
		Votes v = null;
		if (pTeaching.getId() <= 0)
			throw new EntityNotFoundException("Specificare l'insegnamento");
		if (pUser.getId() <=0 )
			throw new EntityNotFoundException("Specificare l'utente");
		try {
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Preparimao la stringa sql
			String sql = "SELECT * FROM "
					+ ManagerVotes.TABLE_VOTES
					+ " WHERE id_teaching = "
					+ Utility.isNull(pTeaching.getId())
					+ " AND "
					+ ManagerVotes.TABLE_VOTES
					+ ".AccademicYear= "
					+ Utility.isNull(academicYear)
					+ " AND "
					+ ManagerVotes.TABLE_VOTES
					+ ".turn= "
					+ Utility.isNull(turn)
					+ " AND "
					+ ManagerVotes.TABLE_VOTES
					+ ".id_user= "
					+ Utility.isNull(pUser.getId());
			// Inviamo la Query al database
			ResultSet pRs = Utility.queryOperation(connect, sql);
			if (pRs.next()){
				v = this.loadRecordFromRs(pRs);
				result =v.getId_votes();
				
			}

			return result;

		} finally {
			// rilasciamo le risorse
			DBConnection.releaseConnection(connect);

		}
	}

	/**
	 * Elimina un voto dalla tabella votes.
	 * 
	 * @param pVotes
	 *            Il voto da eliminare.
	 * 
	 * @throws MandatoryFieldException
	 * @throws EntityNotFoundException
	 * @throws SQLException
	 * @throws ConnectionException
	 * @throws InvalidValueException
	 * 
	 */
	public synchronized void delete(Votes pVotes)
			throws ConnectionException, SQLException, EntityNotFoundException,
			MandatoryFieldException, InvalidValueException {
		Connection connect = null;

		try {
			// ManagerTeaching.getInstance().teachingOnDeleteCascade(pTeaching);
			connect = DBConnection.getConnection();
			// Prepariamo la stringa SQL
			String sql = "DELETE FROM " + ManagerVotes.TABLE_VOTES
					+ " WHERE id_votes = "
					+ Utility.isNull(pVotes.getId_votes());

			Utility.executeOperation(connect, sql);
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Ritorna l'id dell'insegnamento corrispondente all'id del voto passato come
	 * parametro.
	 * 
	 * @param pId
	 *            L'id del voto.
	 * @return Ritorna l'id dell' insegnamento.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	/**
	 * Ritorna l'insegnamento corrispondente all'id passato come parametro.
	 * 
	 * @param pId
	 *            L'id dell'insegnamento.
	 * @return Ritorna l'insegnamento associato all'id passato come parametro.
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized Votes getVotesById(int pId)
			throws ConnectionException, SQLException, EntityNotFoundException,
			InvalidValueException {
		Votes result = null;
		Connection connect = null;
		try {

			if (pId <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare il voto!");

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Prepariamo la stringa SQL
			String sql = "SELECT * FROM " + ManagerVotes.TABLE_VOTES
					+ " WHERE id_votes = " + Utility.isNull(pId);

			// Inviamo la Query al DataBase
			ResultSet pRs = Utility.queryOperation(connect, sql);

			if (pRs.next())
				result = this.loadRecordFromRs(pRs);
			else
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			return result;
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	
	public synchronized String getTeachingIdByVotesId(int pId)
			throws EntityNotFoundException, ConnectionException, SQLException {
		String result;
		Connection connect = null;
		try {
			// Se non e' stato fornito l'id restituiamo un codice di errore
			if (pId <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare il voto!");

			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'insegnamento passato come parametro
			 */
			String tSql = "SELECT id_teaching FROM " + ManagerVotes.TABLE_VOTES
			        
					+ " WHERE id_votes = " + Utility.isNull(pId);

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);

			if (tRs.next())
				result = tRs.getString("id_teaching");
			else
				throw new EntityNotFoundException(
						"Impossibile trovare il voto!");

			return result;
		} finally {
			DBConnection.releaseConnection(connect);
		}
	}
	

	/**
	 * Ritorna l'insieme di tutti i voti presenti nel database.
	 * 
	 * @return Ritorna una collection di voti.
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 */
	public synchronized Collection<Votes> getVotes()
			throws ConnectionException, SQLException, InvalidValueException,
			EntityNotFoundException {
		Collection<Votes> result = null;
		Connection connect = null;

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT * FROM " + ManagerVotes.TABLE_VOTES
					+ " ORDER BY id_votes";

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet pRs = Utility.queryOperation(connect, sql);

			if (pRs.next())
				result = this.loadRecordsFromRs(pRs);

			return result;
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}

	}

	/**
	 * Ritorna l'insieme dei voti associati all'utente corrispondente
	 * all'id passato come paramentro.
	 * 
	 * @param pId
	 *            L'id dell'utente.
	 * @return Ritorna una collection di voti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Votes> getVotesByUserId(int pId)
			throws EntityNotFoundException, ConnectionException, SQLException,
			InvalidValueException {

		Collection<Votes> result = null;
		Connection connect = null;

		if (pId <= 0)
			throw new EntityNotFoundException("specificare l'utente");

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT " + ManagerVotes.TABLE_VOTES
					+ ".* FROM " + ManagerVotes.TABLE_VOTES
				    + " WHERE ("
					+ ManagerVotes.TABLE_VOTES + ".id_user = "
					+ Utility.isNull(pId) + ")" + " ORDER BY id_user";

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet pRs = Utility.queryOperation(connect, sql);

			if (pRs.next())
				result = this.loadRecordsFromRs(pRs);

			return result;
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Ritorna l'insieme dei voti associati all'utente corrispondente
	 * all'id passato come paramentro.
	 * 
	 * @param pId
	 *            L'id dell'utente.
	 * @return Ritorna una collection di voti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Votes> getVotesByUserIdYearTurn(int pId,int pYear, int pTurn)
			throws EntityNotFoundException, ConnectionException, SQLException,
			InvalidValueException {

		Collection<Votes> result = null;
		Connection connect = null;

		if (pId <= 0)
			throw new EntityNotFoundException("specificare l'utente");

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT " + ManagerVotes.TABLE_VOTES
					+ ".* FROM " + ManagerVotes.TABLE_VOTES
				    + " WHERE ("
					+ ManagerVotes.TABLE_VOTES + ".id_user = "
					+ Utility.isNull(pId)+" AND "
					+ ManagerVotes.TABLE_VOTES + ".accademicYear = "
					+ Utility.isNull(pYear)+" AND " 
					+ManagerVotes.TABLE_VOTES + ".turn = "
					+ Utility.isNull(pTurn)+ ")" + " ORDER BY id_user";

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet pRs = Utility.queryOperation(connect, sql);

			if (pRs.next())
				result = this.loadRecordsFromRs(pRs);

			return result;
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	
	public synchronized void deleteVotesByUserIdYearTurn(int pId,int pYear, int pTurn)
	throws EntityNotFoundException, ConnectionException, SQLException,
	InvalidValueException {

Connection connect = null;

if (pId <= 0)
	throw new EntityNotFoundException("specificare l'utente");

try {
	// Prepariamo la stringa SQL
	String sql = "DELETE "+ ManagerVotes.TABLE_VOTES+" FROM " + ManagerVotes.TABLE_VOTES
		    + " WHERE ("
			+ ManagerVotes.TABLE_VOTES + ".id_user="
			+ Utility.isNull(pId)+" AND "
			+ ManagerVotes.TABLE_VOTES + ".AccademicYear="
			+ Utility.isNull(pYear)+" AND " 
			+ManagerVotes.TABLE_VOTES + ".turn="
			+ Utility.isNull(pTurn)+ ")";

	// Otteniamo una Connessione al DataBase
	connect = DBConnection.getConnection();
	if (connect == null)
		throw new ConnectionException();

	// Inviamo la Query al DataBase
	 Utility.executeOperation(connect, sql);
     
} finally {
	// rilascia le risorse
	DBConnection.releaseConnection(connect);
}
}
	
	
	/**
	 * Consente la lettura di un record dal ResultSet.
	 * 
	 * @param pRs
	 *            Il risultato della query.
	 * @return Ritorna il voto letto.
	 * 
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Votes loadRecordFromRs(ResultSet pRs) throws SQLException,
			InvalidValueException {
		Votes votes = new Votes();
		votes.setId_votes(pRs.getInt(("id_votes")));
		votes.setId_user(pRs.getInt("id_user"));
		votes.setTeaching(pRs.getInt("id_teaching"));
		votes.setWritten(pRs.getInt("written"));
		votes.setOral(pRs.getInt("oral"));
		votes.setLaboratory(pRs.getInt("laboratory"));
		votes.setAccademicYear(pRs.getInt("AccademicYear"));
		votes.setTurn(pRs.getInt("turn"));

		return votes;
	}

	/**
	 * Consente la lettura dei record dal ResultSet.
	 * 
	 * @param pRs
	 *            Il risultato della query.
	 * @return Ritorna la collection di insegnamenti letti.
	 * 
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Collection<Votes> loadRecordsFromRs(ResultSet pRs)
			throws SQLException, InvalidValueException {
		Collection<Votes> result = new Vector<Votes>();
		do {
			result.add(loadRecordFromRs(pRs));
		} while (pRs.next());
		return result;
	}

}
