package smos.storage;

import smos.bean.Teaching;
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
 * Classe manager degli insegnamenti.
 * 
 * @author Giulio D'Amora
 * @version 1.0
 * 
 *          2009 – Copyright by SMOS
 */

public class ManagerTeaching {
	private static ManagerTeaching instance;

	/**
	 * Il nome della tabella degli insegnamenti.
	 */
	public static final String TABLE_TEACHING = "teaching";

	/**
	 * Il nome della tabella che modella l'associazione molti a molti tra
	 * indirizzi ed insegnamenti.
	 */
	public static final String TABLE_ADDRESS_TEACHINGS = "address_has_teaching";

	/**
	 * Il nome della tabella che modella l'associazione molti a molti tra utenti
	 * e insegnamenti.
	 */
	public static final String TABLE_TEACHER_CLASSROOM = "teacher_has_classroom";

	/**
	 * Il costruttore della classe.
	 */
	private ManagerTeaching() {
		super();
	}

	/**
	 * Ritorna la sola istanza dell'insegnamento esistente.
	 * 
	 * @return Ritorna l'istanza della classe.
	 */
	public static synchronized ManagerTeaching getInstance() {
		if (instance == null) {
			instance = new ManagerTeaching();
		}
		return instance;
	}

	/**
	 * Verifica l'esistenza di un insegnamento nel database.
	 * 
	 * @param pTeaching
	 *            L'insegnamento da controllare.
	 * @return Ritorna true se esiste l'insegnamento passato come parametro,
	 *         false altrimenti.
	 * 
	 * @throws MandatoryFieldException
	 * @throws SQLException
	 * @throws ConnectionException
	 */
	public synchronized boolean exists(Teaching pTeaching)
			throws MandatoryFieldException, ConnectionException, SQLException {

		boolean result = false;
		Connection connect = null;

		if (pTeaching.getName() == null)
			throw new MandatoryFieldException("Specificare il nome.");
		try {
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " + ManagerTeaching.TABLE_TEACHING
					+ " WHERE name = " + Utility.isNull(pTeaching.getName());

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
	 * Inserisce un nuovo insegnamento nella tabella teaching.
	 * 
	 * @param pTeaching
	 *            L'insegnamento da inserire.
	 * 
	 * @throws SQLException
	 * @throws ConnectionException
	 * @throws MandatoryFieldException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insert(Teaching pTeaching)
			throws MandatoryFieldException, ConnectionException, SQLException,
			EntityNotFoundException, InvalidValueException {
		Connection connect = null;
		try {
			// controllo dei campi obbligatori
			if (pTeaching.getName() == null)
				throw new MandatoryFieldException("Specificare il campo nome");

			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();
			// Prepariamo la stringa Sql
			String sql = "INSERT INTO " + ManagerTeaching.TABLE_TEACHING
					+ " (name) " + "VALUES ("
					+ Utility.isNull(pTeaching.getName()) + ")";

			Utility.executeOperation(connect, sql);

			pTeaching.setId(Utility.getMaxValue("id_teaching",
					ManagerTeaching.TABLE_TEACHING));

		} finally {
			// rilascia le risorse

			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Aggiorna un insegnamento presente nella tabella teaching.
	 * 
	 * @param pTeaching
	 *            L'insegnamento da modificare
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	public synchronized void update(Teaching pTeaching)
			throws ConnectionException, SQLException, EntityNotFoundException,
			MandatoryFieldException {
		Connection connect = null;

		try {
			if (pTeaching.getId() <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			if (pTeaching.getName() == null)
				throw new MandatoryFieldException("Specificare il campo nome");

			// Prepariamo la stringa SQL
			String sql = "UPDATE " + ManagerTeaching.TABLE_TEACHING + " SET"
					+ " name = " + Utility.isNull(pTeaching.getName())
					+ " WHERE id_teaching = "
					+ Utility.isNull(pTeaching.getId());

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
	 * Elimina un insegnamento dalla tabella teaching.
	 * 
	 * @param pTeaching
	 *            L'insegnamento da eliminare.
	 * 
	 * @throws MandatoryFieldException
	 * @throws EntityNotFoundException
	 * @throws SQLException
	 * @throws ConnectionException
	 * @throws InvalidValueException
	 * 
	 */
	public synchronized void delete(Teaching pTeaching)
			throws ConnectionException, SQLException, EntityNotFoundException,
			MandatoryFieldException, InvalidValueException {
		Connection connect = null;

		try {
			// ManagerTeaching.getInstance().teachingOnDeleteCascade(pTeaching);
			connect = DBConnection.getConnection();
			// Prepariamo la stringa SQL
			String sql = "DELETE FROM " + ManagerTeaching.TABLE_TEACHING
					+ " WHERE id_teaching = "
					+ Utility.isNull(pTeaching.getId());

			Utility.executeOperation(connect, sql);
		} finally {
			// rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Ritorna l'id dell'insegnamento passato come parametro.
	 * 
	 * @param pTeaching
	 *            L'insegnamento di cui si richiede l'id.
	 * @return Ritorna l'id dell'insegnamento passato come parametro.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized int getTeachingId(Teaching pTeaching)
			throws EntityNotFoundException, ConnectionException, SQLException {
		int result = 0;
		Connection connect = null;
		try {
			if (pTeaching == null)
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'insegnamento passato come parametro.
			 */
			String tSql = "SELECT id_teaching FROM "
					+ ManagerTeaching.TABLE_TEACHING + " WHERE name = "
					+ Utility.isNull(pTeaching.getName());

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);

			if (tRs.next())
				result = tRs.getInt("id_teaching");

			return result;
		} finally {
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Ritorna il nome dell'insegnamento corrispondente all'id passato come
	 * parametro.
	 * 
	 * @param pId
	 *            L'id dell'insegnamento.
	 * @return Ritorna una stringa contenente il nome dell'insegnamento.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized String getTeachingNameById(int pId)
			throws EntityNotFoundException, ConnectionException, SQLException {
		String result;
		Connection connect = null;
		try {
			// Se non e' stato fornito l'id restituiamo un codice di errore
			if (pId <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'insegnamento passato come parametro
			 */
			String tSql = "SELECT name FROM " + ManagerTeaching.TABLE_TEACHING
					+ " WHERE id_teaching = " + Utility.isNull(pId);

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);

			if (tRs.next())
				result = tRs.getString("name");
			else
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			return result;
		} finally {
			DBConnection.releaseConnection(connect);
		}
	}

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
	public synchronized Teaching getTeachingById(int pId)
			throws ConnectionException, SQLException, EntityNotFoundException,
			InvalidValueException {
		Teaching result = null;
		Connection connect = null;
		try {

			if (pId <= 0)
				throw new EntityNotFoundException(
						"Impossibile trovare l'insegnamento!");

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Prepariamo la stringa SQL
			String sql = "SELECT * FROM " + ManagerTeaching.TABLE_TEACHING
					+ " WHERE id_teaching = " + Utility.isNull(pId);

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

	/**
	 * Ritorna l'insieme di tutti gli insegnamenti presenti nel database.
	 * 
	 * @return Ritorna una collection di insegnamenti.
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 */
	public synchronized Collection<Teaching> getTeachings()
			throws ConnectionException, SQLException, InvalidValueException,
			EntityNotFoundException {
		Collection<Teaching> result = null;
		Connection connect = null;

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT * FROM " + ManagerTeaching.TABLE_TEACHING
					+ " ORDER BY name";

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
	 * Ritorna l'insieme degli insegnamenti associati all'utente corrispondente
	 * all'id passato come paramentro.
	 * 
	 * @param pId
	 *            L'id dell'utente.
	 * @return Ritorna una collection di insegnamenti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Teaching> getTeachingsByUserId(int pId)
			throws EntityNotFoundException, ConnectionException, SQLException,
			InvalidValueException {

		Collection<Teaching> result = null;
		Connection connect = null;

		if (pId <= 0)
			throw new EntityNotFoundException("specificare l'utente");

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT " + ManagerTeaching.TABLE_TEACHING
					+ ".* FROM " + ManagerTeaching.TABLE_TEACHER_CLASSROOM
					+ ", " + ManagerTeaching.TABLE_TEACHING + " WHERE ("
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM
					+ ".id_teaching = " + ManagerTeaching.TABLE_TEACHING
					+ ".id_teaching AND "
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM + ".id_user = "
					+ Utility.isNull(pId) + ")" + " ORDER BY name";

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
	 * Ritorna l'insieme degli insegnamenti che il docente insegna nella classe
	 * 
	 * @param pIdTeacher
	 *            L'id dell'utente.
	 * @param pIdClass
	 *            l'id della classe
	 * @return Ritorna una collection di insegnamenti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Teaching> getTeachingsByUserIdClassID(int pIdTeacher,int pIdClass)
			throws EntityNotFoundException, ConnectionException, SQLException,
			InvalidValueException {

		Collection<Teaching> result = null;
		Connection connect = null;

		if (pIdTeacher <= 0)
			throw new EntityNotFoundException("specificare l'utente");
		if (pIdClass <= 0)
			throw new EntityNotFoundException("specificare la classe");

		try {
			// Prepariamo la stringa SQL
			
			String sql = "SELECT DISTINCT " + ManagerTeaching.TABLE_TEACHING
					+ ".* FROM " + ManagerTeaching.TABLE_TEACHER_CLASSROOM
					+ ", " + ManagerTeaching.TABLE_TEACHING + " WHERE ("
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM + ".id_user = "
					+ Utility.isNull(pIdTeacher) +" AND "
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM 
					+ ".id_teaching= " + Utility.isNull(pIdClass)
					+ " AND "
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM
					+ ".id_teaching = " + ManagerTeaching.TABLE_TEACHING
					+ ".id_teaching "
					+") ORDER BY name";

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
	 * Verifica se un insegnamento ha un professore assegnato.
	 * 
	 * @param pTeaching
	 *            L'insegnamento da controllare.
	 * @return Ritorna true se l'insegnamento ha un professore assegnato, false
	 *         altrimenti.
	 * 
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws InvalidValueException
	 */
	public synchronized boolean hasTeacher(Teaching pTeaching)
			throws SQLException, EntityNotFoundException, ConnectionException,
			InvalidValueException {
		Connection connect = null;
		boolean result = false;
		if (pTeaching.getId() <= 0)
			throw new EntityNotFoundException("Specificare l'insegnamento");

		try {
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Preparimao la stringa sql
			String sql = "SELECT * FROM "
					+ ManagerTeaching.TABLE_TEACHER_CLASSROOM
					+ " WHERE id_teaching = "
					+ Utility.isNull(pTeaching.getId());
			// Inviamo la Query al database
			ResultSet pRs = Utility.queryOperation(connect, sql);
			if (pRs.next())
				result = true;

			return result;

		} finally {
			// rilasciamo le risorse
			DBConnection.releaseConnection(connect);

		}
	}

	/**
	 * Ritorna l'insieme degli insegnamenti associati alla classe specificata
	 * 
	 * @param pId
	 *            L'id della classe.
	 * @return Ritorna una collection di insegnamenti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Teaching> getTeachingsByClassroomId(int pId)
			throws EntityNotFoundException, ConnectionException, SQLException,
			InvalidValueException {

		Collection<Teaching> result = null;
		Connection connect = null;

		if (pId < 0)
			throw new EntityNotFoundException("specificare l'id della Classe!");

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT " + ManagerTeaching.TABLE_TEACHING
					+ ".* FROM " + ManagerClassroom.TABLE_CLASSROOM + ", "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS + ", "
					+ ManagerTeaching.TABLE_TEACHING + " WHERE "
					+ ManagerClassroom.TABLE_CLASSROOM + ".id_classroom = "
					+ Utility.isNull(pId) + " AND "
					+ ManagerClassroom.TABLE_CLASSROOM + ".id_address = "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS
					+ ".id_address AND " + ManagerTeaching.TABLE_TEACHING
					+ ".id_teaching= "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS + ".id_teaching ";

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
	 * Ritorna l'insieme degli insegnamenti associati alla classe specificata
	 * 
	 * @param name
	 *            Il nome della classe.
	 * @return Ritorna una collection di insegnamenti.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Teaching> getTeachingsByClassroomName(
			String name) throws EntityNotFoundException, ConnectionException,
			SQLException, InvalidValueException {

		Collection<Teaching> result = null;
		Connection connect = null;

		if ((name == null) || (name == ""))
			throw new EntityNotFoundException(
					"specificare il nome della Classe!");

		try {
			// Prepariamo la stringa SQL
			String sql = "SELECT " + ManagerTeaching.TABLE_TEACHING
					+ ".* FROM " + ManagerClassroom.TABLE_CLASSROOM + ", "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS + ", "
					+ ManagerTeaching.TABLE_TEACHING + " WHERE "
					+ ManagerClassroom.TABLE_CLASSROOM + ".name = "
					+ Utility.isNull(name) + " AND "
					+ ManagerClassroom.TABLE_CLASSROOM + ".id_address = "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS
					+ ".id_address AND " + ManagerTeaching.TABLE_TEACHING
					+ ".id_teaching= "
					+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS + ".id_teaching ";

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
	
	public synchronized Collection<Teaching> getTeachingsByIdUserIdClassroom(int pUser, int pClass) throws SQLException,
	EntityNotFoundException, ConnectionException, InvalidValueException {
		
		
		Collection<Teaching> result = null;
		Connection connect = null;
		try {
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// 	Preparimao la stringa sql
			//select teaching.* from teacher_has_classroom AS THC , teaching where thc.id_user = 54 
			//&& thc.id_classroom = 64 && thc.id_teaching = teaching.id_teaching
			
			String sql = "SELECT DISTINCT "
				+ManagerTeaching.TABLE_TEACHING+".*" 
				+" FROM " 
				+ ManagerUser.TABLE_TEACHER_CLASSROOM
				+" , "
				+ManagerTeaching.TABLE_TEACHING
				+ " WHERE "
				+ ManagerUser.TABLE_TEACHER_CLASSROOM
				+ ".id_user = "  
				+ Utility.isNull(pUser)
				+ " AND "
				+ ManagerUser.TABLE_TEACHER_CLASSROOM
				+ ".id_classroom= "
				+ Utility.isNull(pClass)
				+ " AND "
				+ ManagerUser.TABLE_TEACHER_CLASSROOM
				+".id_teaching ="
				+ManagerTeaching.TABLE_TEACHING
				+".id_teaching";
				// Inviamo la Query al database
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
	 * Consente la lettura di un record dal ResultSet.
	 * 
	 * @param pRs
	 *            Il risultato della query.
	 * @return Ritorna l'insegnamento letto.
	 * 
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Teaching loadRecordFromRs(ResultSet pRs) throws SQLException,
			InvalidValueException {
		Teaching teaching = new Teaching();
		teaching.setName(pRs.getString(("name")));
		teaching.setId(pRs.getInt("id_teaching"));

		return teaching;
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
	private Collection<Teaching> loadRecordsFromRs(ResultSet pRs)
			throws SQLException, InvalidValueException {
		Collection<Teaching> result = new Vector<Teaching>();
		do {
			result.add(loadRecordFromRs(pRs));
		} while (pRs.next());
		return result;
	}

}
