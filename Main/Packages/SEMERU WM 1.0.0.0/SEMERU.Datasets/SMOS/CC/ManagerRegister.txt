package smos.storage;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Date;
import java.util.Vector;

import smos.bean.Absence;
import smos.bean.Delay;
import smos.bean.Justify;
import smos.bean.Note;
import smos.bean.RegisterLine;
import smos.bean.UserListItem;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.connectionManagement.DBConnection;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ManagerRegister {

	
	/**
	 * Classe che gestisce il Registro Digitale
	 * @author Nicola Pisanti
	 * @version 1.0
	 */
	
	private static ManagerRegister instance;
	
	public final static String TABLE_ABSENCE="absence";
	public final static String TABLE_DELAY="delay";
	public final static String TABLE_JUSTIFY="justify";
	public final static String TABLE_NOTE="note";
	
	
	private ManagerRegister(){
		super();
	}
	
	
	/**
	 * Ritorna la sola istanza della classe esistente.
	 * 
	 * @return Ritorna l'istanza della classe.
	 */
	public static synchronized ManagerRegister getInstance(){
		if(instance==null){
			instance = new ManagerRegister();
		}
		return instance;
	}
	
	/**
	 * Verifica se la classe data in input è nel database
	 * @param pAbsence
	 * 		La classe di cui bisogna verificare l'esistenza
	 * @return true se la classe è nel database, altrimenti false
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized boolean exists(Absence pAbsence) throws ConnectionException, SQLException {
		
		boolean result = false;
		Connection connect = null;

		try {
			//Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE
				+ " WHERE id_absence = "
				+ Utility.isNull(pAbsence.getIdAbsence());

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, sql);

			if (tRs.next()){
				result = true;
			}
			
			return result;
			
		} finally {
			DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Verifica se la classe data in input è nel database
	 * @param pDelay
	 * 		La classe di cui bisogna verificare l'esistenza
	 * @return true se la classe è nel database, altrimenti false
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized boolean exists(Delay pDelay) throws ConnectionException, SQLException {
		
		boolean result = false;
		Connection connect = null;

		try {
			//Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE
				+ " WHERE id_delay = "
				+ Utility.isNull(pDelay.getIdDelay());
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
	 * Verifica se la classe data in input è nel database
	 * @param pDelay
	 * 		La classe di cui bisogna verificare l'esistenza
	 * @return true se la classe è nel database, altrimenti false
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized boolean exists(Justify pJustify) throws ConnectionException, SQLException {
		
		boolean result = false;
		Connection connect = null;

		try {
			//Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " 
				+ ManagerRegister.TABLE_JUSTIFY
				+ " WHERE  id_justify = "
				+ Utility.isNull(pJustify.getIdJustify());

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
	 * Inserisce un assenza nel database
	 * @param pAbsence
	 * 		un oggetto di tipo Absence da inserire nel database
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insertAbsence(Absence pAbsence) throws  
		ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		
		Connection connect= null;
		try{
			
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			//Prepariamo la stringa Sql
			String sql =
				"INSERT INTO " 
				+ ManagerRegister.TABLE_ABSENCE 
				+ " (id_user, date_absence, id_justify, accademic_year) " 
				+ "VALUES (" 
				+ Utility.isNull(pAbsence.getIdUser()) 
				+ "," 
				+ Utility.isNull(pAbsence.getDateAbsence()) 
				+ "," 
				+ Utility.isNull(pAbsence.getIdJustify()) 
				+ "," 
				+ Utility.isNull(pAbsence.getAcademicYear())
				+ ")";
		
			Utility.executeOperation(connect,sql);
		
			pAbsence.setIdAbsence((Utility.getMaxValue("id_absence",ManagerRegister.TABLE_ABSENCE)));
		
		}finally {
		//rilascia le risorse
		
		DBConnection.releaseConnection(connect);
		}
	}
	

	/**
	 * Inserisce un ritardo nel database
	 * @param pDelay
	 * 		un oggetto di tipo Delay da inserire nel database
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insertDelay(Delay pDelay) throws  
		ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		
		Connection connect= null;
		try{
			
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			//Prepariamo la stringa Sql
			String sql =
				"INSERT INTO " 
				+ ManagerRegister.TABLE_DELAY 
				+ " (id_user, date_delay, time_delay, accademic_year) " 
				+ "VALUES (" 
				+ Utility.isNull(pDelay.getIdUser()) 
				+ "," 
				+ Utility.isNull(pDelay.getDateDelay()) 
				+ "," 
				+ Utility.isNull(pDelay.getTimeDelay()) 
				+ "," 
				+ Utility.isNull(pDelay.getAcademicYear())
				+ ")";
		
			Utility.executeOperation(connect,sql);
		
			pDelay.setIdDelay((Utility.getMaxValue("id_delay",ManagerRegister.TABLE_DELAY)));
		
		}finally {
		//rilascia le risorse
		
		DBConnection.releaseConnection(connect);
		}
	}
	


	/**
	 * Inserisce una nota nel database
	 * @param pNote
	 * 		un oggetto di tipo Note da inserire nel database
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insertNote(Note pNote) throws MandatoryFieldException,  
		ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		
		Connection connect= null;
		try{
			if (pNote.getDescription() == null || pNote.getDescription().equals(""))
				throw new MandatoryFieldException("Inserire il testo della nota");
			
			if (pNote.getTeacher() == null || pNote.getTeacher().equals("") )
				throw new MandatoryFieldException("Inserire l'insegnante");
			
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			//Prepariamo la stringa Sql
			String sql =
				"INSERT INTO " 
				+ ManagerRegister.TABLE_NOTE 
				+ " (id_user, date_note, description, teacher, accademic_year) " 
				+ "VALUES (" 
				+ Utility.isNull(pNote.getIdUser()) 
				+ "," 
				+ Utility.isNull(pNote.getDateNote()) 
				+ "," 
				+ Utility.isNull(pNote.getDescription()) 
				+ "," 
				+ Utility.isNull(pNote.getTeacher()) 
				+ "," 
				+ Utility.isNull(pNote.getAcademicYear())
				+ ")";
		
			Utility.executeOperation(connect,sql);
		
			pNote.setIdNote((Utility.getMaxValue("id_note",ManagerRegister.TABLE_NOTE)));
		
		}finally {
		//rilascia le risorse
		
		DBConnection.releaseConnection(connect);
		}
	}
	

	/**
	 * Inserisce una giustifica nel database
	 * @param pJustify 
	 * 		un oggetto di tipo Justify da inserire nel database
	 * @param pAbsence
	 * 		un oggetto di tipo Absence che rappresenta l'assenza giustificata
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insertJustify(Justify pJustify, Absence pAbsence) throws   
		ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		
		Connection connect= null;
		try{
			
			
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			//Prepariamo la stringa Sql
			String sql =
				"INSERT INTO " 
				+ ManagerRegister.TABLE_JUSTIFY 
				+ " (id_user, date_justify, accademic_year) " 
				+ "VALUES (" 
				+ Utility.isNull(pJustify.getIdUser()) 
				+ "," 
				+ Utility.isNull(pJustify.getDateJustify()) 
				+ "," 
				+ Utility.isNull(pJustify.getAcademicYear())
				+ ")";
		
			Utility.executeOperation(connect,sql);
		
			pJustify.setIdJustify((Utility.getMaxValue("id_justify",ManagerRegister.TABLE_JUSTIFY)));
			
			pAbsence.setIdJustify(pJustify.getIdJustify());
			this.updateAbsence(pAbsence);
		
		}finally {
		//rilascia le risorse
		
		DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Cancella un'assenza dal database
	 * @param pAbsence
	 * 		l'assenza da cancellare
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void deleteAbsence (Absence pAbsence) throws ConnectionException, 
			SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
		Connection connect = null;
		
		
		try {
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerRegister.TABLE_ABSENCE 
							+ " WHERE id_absence = "
							+ Utility.isNull(pAbsence.getIdAbsence());
			
				Utility.executeOperation(connect, sql);
				
				if (!(pAbsence.getIdJustify()==null)){
					deleteJustify(pAbsence.getIdJustify());
				}
		}finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}


	/**
	 * Cancella un ritardo dal database
	 * @param pDelay
	 * 		il ritardo da cancellare
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void deleteDelay (Delay pDelay) throws ConnectionException, 
			SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
		Connection connect = null;
		
		
		try {
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerRegister.TABLE_DELAY 
							+ " WHERE id_delay = "
							+ Utility.isNull(pDelay.getIdDelay());
			
				Utility.executeOperation(connect, sql);
		}finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Cancella una nota dal database
	 * @param pNote
	 * 		la nota da cancellare
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void deleteNote (Note pNote) throws ConnectionException, 
			SQLException, EntityNotFoundException, InvalidValueException {
		Connection connect = null;
		
		
		try {
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerRegister.TABLE_NOTE 
							+ " WHERE id_note = "
							+ Utility.isNull(pNote.getIdNote());
			
				Utility.executeOperation(connect, sql);
		}finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Cancella una nota dal database
	 * @param pJIDustify
	 * 		l'ID della nota da cancellare
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void deleteJustify (int pIDJustify) throws ConnectionException, 
			SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
		Connection connect = null;
		
		
		try {
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerRegister.TABLE_JUSTIFY 
							+ " WHERE id_justify = "
							+ Utility.isNull(pIDJustify);
			
				Utility.executeOperation(connect, sql);
				
				try{
					Absence temp= getAbsenceByIdJustify(pIDJustify);
					temp.setIdJustify(0);
					updateAbsence(temp);
				}catch(Exception e){
					// � normale se un exception viene generata
					// dato che pu� essere che stiamo cancellando una giustifica
					//di cui abbiamo appena cancellato l'assenza 
				}
				
				
		}finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Aggiorna le statistiche di un'assenza
	 * @param pAbsence
	 * 		L'assenza con le statistiche aggiornate (ma ID identico)
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	
	
	public synchronized void updateAbsence (Absence pAbsence) throws ConnectionException,
	SQLException, EntityNotFoundException{
		Connection connect= null;
		
		try{

			
			//Prepariamo la stringa SQL
			String sql=
				"UPDATE " 
				+	ManagerRegister.TABLE_ABSENCE 
				+ " SET" 
				+ " id_user = " 
				+ Utility.isNull(pAbsence.getIdUser()) 
				+ ", date_absence = " 
				+ Utility.isNull(pAbsence.getDateAbsence()) 
				+ ", id_justify = " 
				+ Utility.isNull(pAbsence.getIdJustify())  
				+ ", accademic_year = " 
				+ Utility.isNull(pAbsence.getAcademicYear())  
				+ " WHERE id_absence = " 
				+ Utility.isNull(pAbsence.getIdAbsence());
			
			//effettua una nuova connessione e invia la query
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			
			Utility.executeOperation(connect, sql);
		}finally {
		//rilascia le risorse
		DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Aggiorna le statistiche di un ritardo
	 * @param pDelay
	 * 		Il ritardo con le statistiche aggiornate (ma ID identico)
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	
	
	public synchronized void updateDelay (Delay pDelay) throws ConnectionException,
	SQLException, EntityNotFoundException, MandatoryFieldException{
		Connection connect= null;
		
		try{

			
			//Prepariamo la stringa SQL
			String sql=
				"UPDATE " 
				+	ManagerRegister.TABLE_DELAY 
				+ " SET" 
				+ " id_user = " 
				+ Utility.isNull(pDelay.getIdUser()) 
				+ ", date_delay = " 
				+ Utility.isNull(pDelay.getDateDelay()) 
				+ ", time_delay = " 
				+ Utility.isNull(pDelay.getTimeDelay())  
				+ ", accademic_year = " 
				+ Utility.isNull(pDelay.getAcademicYear())  
				+ " WHERE id_delay = " 
				+ Utility.isNull(pDelay.getIdDelay());
			
			//effettua una nuova connessione e invia la query
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			
			Utility.executeOperation(connect, sql);
		}finally {
		//rilascia le risorse
		DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Metodo che aggiorna le statistiche di una Nota
	 * @param pNote
	 * 		un oggetto di tipo Note con le statistiche aggiornate ma id identico
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	
	public synchronized void updateNote (Note pNote) throws ConnectionException,
	SQLException, EntityNotFoundException, MandatoryFieldException{
		Connection connect= null;
		
		try{
			if (pNote.getDescription() == null || pNote.getDescription().equals(""))
				throw new MandatoryFieldException("Inserire il testo della nota");
			
			if (pNote.getTeacher() == null || pNote.getTeacher().equals("") )
				throw new MandatoryFieldException("Inserire l'insegnante");
			//Prepariamo la stringa SQL
			String sql=
				"UPDATE " 
				+	ManagerRegister.TABLE_NOTE
				+ " SET" 
				+ " id_user = " 
				+ Utility.isNull(pNote.getIdUser()) 
				+ ", date_note = " 
				+ Utility.isNull(pNote.getDateNote())   
				+ ", description = " 
				+ Utility.isNull(pNote.getDescription())   
				+ ", teacher = " 
				+ Utility.isNull(pNote.getTeacher())   
				+ ", accademic_year = " 
				+ Utility.isNull(pNote.getAcademicYear())  
				+ " WHERE id_note = " 
				+ Utility.isNull(pNote.getIdNote());
			
			//effettua una nuova connessione e invia la query
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			
			Utility.executeOperation(connect, sql);
		}finally {
		//rilascia le risorse
		DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Aggiorna le statistiche di una giustifica 
	 * @param pJustify
	 * 		la giustifica con le statistiche aggiornate (ma ID identico)
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	

	public synchronized void updateJustify (Justify pJustify) throws ConnectionException,
	SQLException, EntityNotFoundException, MandatoryFieldException{
		Connection connect= null;
		
		try{

			
			//Prepariamo la stringa SQL
			String sql=
				"UPDATE " 
				+	ManagerRegister.TABLE_JUSTIFY
				+ " SET" 
				+ " id_user = " 
				+ Utility.isNull(pJustify.getIdUser()) 
				+ ", date_justify = " 
				+ Utility.isNull(pJustify.getDateJustify())   
				+ ", accademic_year = " 
				+ Utility.isNull(pJustify.getAcademicYear())  
				+ " WHERE id_justify = " 
				+ Utility.isNull(pJustify.getIdJustify());
			
			//effettua una nuova connessione e invia la query
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			
			Utility.executeOperation(connect, sql);
		}finally {
		//rilascia le risorse
		DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Metodo che restituisce una nota dato l'id della note stessa
	 * @param pIDJustify
	 * 		un intero che rappresenta l'id della nota  
	 * @return un oggetto di tipo Note che rappresenta la nota
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	
	public synchronized Note getNoteById( int pIDNote)throws InvalidValueException,
			EntityNotFoundException, ConnectionException, SQLException{
		Note result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pIDNote<=0)
				throw new EntityNotFoundException("Impossibile trovare la nota");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_NOTE 
				+ " WHERE id_note = " 
				+ Utility.isNull(pIDNote) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadNoteFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare la nota!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}	
	
	
	
	
	
	
	
	/**
	 * Metodo che restituisce un assenza dato l'id della giustifca associata a tale assenza
	 * @param pIDJustify
	 * 		un intero che rappresenta l'id della giustifica  
	 * @return un oggetto di tipo Absence che rappresenta l'assenza giustificata
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	
	public synchronized Absence getAbsenceByIdJustify( int pIDJustify)throws InvalidValueException,
			EntityNotFoundException, ConnectionException, SQLException{
		Absence result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pIDJustify<=0)
				throw new EntityNotFoundException("Impossibile trovare la l'assenza");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE 
				+ " WHERE id_justify = " 
				+ Utility.isNull(pIDJustify) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadAbsenceFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare l'assenza!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Metodo che restituisce un assenza dato l'id di questa
	 * @param pIDAbsence
	 * 		un intero che rappresenta l'id dell'assenza  
	 * @return un oggetto di tipo Absence che rappresenta l'assenza 
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	
	public synchronized Absence getAbsenceByIdAbsence( int pIDAbsence)throws InvalidValueException,
			EntityNotFoundException, ConnectionException, SQLException{
		Absence result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pIDAbsence<=0)
				throw new EntityNotFoundException("Impossibile trovare l' assenza");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE 
				+ " WHERE id_absence = " 
				+ Utility.isNull(pIDAbsence) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadAbsenceFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare l'assenza!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	
	/**
	 * Metodo che restituisce un ritardo dato l'id di questo
	 * @param pIDDelay
	 * 		un intero che rappresenta l'id del ritardo  
	 * @return un oggetto di tipo Delay che rappresenta il ritardo
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	
	public synchronized Delay getDelayById( int pIDDelay)throws InvalidValueException,
			EntityNotFoundException, ConnectionException, SQLException{
		Delay result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pIDDelay<=0)
				throw new EntityNotFoundException("Impossibile trovare il ritardo");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_DELAY
				+ " WHERE id_delay = " 
				+ Utility.isNull(pIDDelay) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadDelayFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare l'assenza!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Metodo che restituisce un assenza dato l'id di questa
	 * @param pIDAbsence
	 * 		un intero che rappresenta l'id dell'assenza  
	 * @return un oggetto di tipo Absence che rappresenta l'assenza 
	 * @throws InvalidValueException
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	
	public synchronized Justify getJustifyByIdJustify( int pIDJustify)throws InvalidValueException,
			EntityNotFoundException, ConnectionException, SQLException{
		Justify result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pIDJustify<=0)
				throw new EntityNotFoundException("Impossibile trovare la giustifica");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_JUSTIFY 
				+ " WHERE id_justify = " 
				+ Utility.isNull(pIDJustify) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				
				result= this.loadJustifyFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare la giustifica!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Metodo che restituisce true se l'assenza data in input ha una giustifica assegnata
	 * @param pAbsence
	 * 		un oggetto di valore Absence di cui bisogna controllare se ha giustifica
	 * @return true se l'assenza è giustificata, false altrimenti
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	
	
	public synchronized boolean hasJustify(Absence pAbsence)throws EntityNotFoundException, ConnectionException, SQLException{
		if(!exists(pAbsence)) throw new EntityNotFoundException("Assenza non presente in database");
		if(pAbsence.getIdJustify()==null) return false;
		return true;
	}
	
	/**
	 * Metodo che restituisce la giustifica legata a una data assenza
	 * @param pAbsence
	 * 		un oggetto di tipo Absence che rappresenta l'assenza
	 * @return	un oggetto di tipo Justify, oppure null se l'assenza non ha giustifica
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */

	public synchronized Justify getJustifyByAbsence(Absence pAbsence)throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		if(!exists(pAbsence)) throw new EntityNotFoundException("Assenza non presente in database");
		if(pAbsence.getIdJustify()==null) return null;
		
		Justify result=null;
		Connection connect = null;
		try{
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_JUSTIFY 
				+ " WHERE id_justify = " 
				+ Utility.isNull(pAbsence.getIdJustify()) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadJustifyFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare la giustifica!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
		
		
	}

	/**
	 * Metodo che restituisce le assenze preso un dato anno scolastico e utente in input
	 * @param pIdUser
	 * 		un intero che rappresenta l'id dell'utente
	 * @param pAcademicYear
	 * 		un intero che rappresenta l'anno accademico 
	 * @return una colleczione di assenze (vuota se l'utente non ha avuto assenze ) 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	
	
	public synchronized Collection<Absence> getAbsenceByIDUserAndAcademicYear(int pIdUser, int pAcademicYear) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Collection<Absence> result=new Vector<Absence>();
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pAcademicYear <= 1970)
				throw new EntityNotFoundException("Data troppo vecchia");

			// idem per l'id user
			if (pIdUser<=0)
				throw new EntityNotFoundException("Utente non trovato");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE 
				+ " WHERE accademic_year = " 
				+ Utility.isNull(pAcademicYear) 
				+ " AND id_user = "
				+ Utility.isNull(pIdUser);
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			while(tRs.next())  {
				result.add(loadAbsenceFromRs(tRs));
			} 
				
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}

	/**
	 * Metodo che restituisce una collezione di note per un dato utente ed un dato anno scolastico
	 * @param pIdUser
	 * 		un intero che rappresenta l'id dell'utente
	 * @param pAcademicYear
	 * 		un intero che rappresenta l'anno accademico 
	 * @return una collezione di note, vuota se l'utente non ne ha ricevute
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Collection<Note> getNoteByIDUserAndAcademicYear(int pIdUser, int pAcademicYear) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Collection<Note> result=new Vector<Note>();
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pAcademicYear <= 1970)
				throw new EntityNotFoundException("Data troppo vecchia");

			// idem per l'id user
			if (pIdUser<=0)
				throw new EntityNotFoundException("Utente non trovato");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_NOTE 
				+ " WHERE accademic_year = " 
				+ Utility.isNull(pAcademicYear) 
				+ " AND id_user = "
				+ Utility.isNull(pIdUser);
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			while(tRs.next())  {
				result.add(loadNoteFromRs(tRs));
			} 
				
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Metodo che restituisce l'assenza di una dato studente in un dato giorno 
	 * @param pIdUser
	 * 		un intero che rappresenta l'id dello studente
	 * @param pDate
	 * 		una stringa che rappresenta la data formattata per il database
	 * @return un oggetto di tipo Absence, oppure null se lo studente era presente
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */

	
	public synchronized Absence getAbsenceByIDUserAndDate(int pIdUser, Date pDate) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Absence result=new Absence();
		Connection connect = null;
		try
		{
			//TODO controlli sulla formattazione della stringa
			
			
			// idem per l'id user
			if (pIdUser<=0)
				throw new EntityNotFoundException("Utente non trovato");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_ABSENCE 
				+ " WHERE date_absence = " 
				+ Utility.isNull(pDate) 
				+ " AND id_user = "
				+ Utility.isNull(pIdUser);
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if(tRs.next())  {
				result=loadAbsenceFromRs(tRs);
			}else {
				result=null;
			}
				
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	
	/**
	 * Metodo che restituisce il ritardo di una dato studente in un dato giorno 
	 * @param pIdUser
	 * 		un intero che rappresenta l'id dello studente
	 * @param pDate
	 * 		una stringa che rappresenta la data formattata per il database
	 * @return un oggetto di tipo Delay, oppure null se lo studente era in orario o assente
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */

	
	public synchronized Delay getDelayByIDUserAndDate(int pIdUser, Date pDate) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Delay result=new Delay();
		Connection connect = null;
		try
		{
			//TODO controlli sulla formattazione della stringa
			
			
			// idem per l'id user
			if (pIdUser<=0)
				throw new EntityNotFoundException("Utente non trovato");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerRegister.TABLE_DELAY 
				+ " WHERE date_delay = " 
				+ Utility.isNull(pDate) 
				+ " AND id_user = "
				+ Utility.isNull(pIdUser);
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if(tRs.next())  {
				result=loadDelayFromRs(tRs);
			}else {
				result=null;
			}
				
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	
	public synchronized Collection<RegisterLine> getRegisterByClassIDAndDate(int pClassID, Date pDate) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		
		Collection<RegisterLine> result = new Vector<RegisterLine>();
		ManagerUser mg = ManagerUser.getInstance();
		
		Collection<UserListItem> students = mg.getStudentsByClassroomId(pClassID);
		
		
		for (UserListItem x : students){
			RegisterLine temp = new RegisterLine();
			temp.setStudent(x);
			temp.setAbsence(this.getAbsenceByIDUserAndDate(x.getId(), pDate));
			temp.setDelay(this.getDelayByIDUserAndDate(x.getId(), pDate));
			result.add(temp);
		}
		
		return result;
	}

	/**
	 * Metodo che verifica se c'� un'assenza in una linea del registro 
	 * @param pRegisterLine
	 * 		un oggetto di tipo RegisterLine
	 * @return	true se c'� un'assenza nella linea di registro passata, altrimenti false
	 */
	
	
	public boolean hasAbsence(RegisterLine pRegisterLine){
		if(pRegisterLine.getAbsence()==null)return false;
		return true;
	}

	/**
	 * Metodo che verifica se c'� un ritardo in una linea del registro 
	 * @param pRegisterLine
	 * 		un oggetto di tipo RegisterLine
	 * @return	true se c'� un ritardo nella linea di registro passata, altrimenti false
	 */
	
	
	public boolean hasDelay(RegisterLine pRegisterLine){
		if(pRegisterLine.getDelay()==null)return false;
		return true;
	}

	/**
	 * Consente la lettura di un solo record dal Result Set
	 * @param pRs
	 * 		Il result set da cui estrarre l'oggetto Absence
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Absence loadAbsenceFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Absence absence = new Absence();
		
		absence.setIdAbsence(pRs.getInt("id_absence"));
		absence.setIdUser(pRs.getInt("id_user"));
		absence.setDateAbsence((Date)pRs.getDate("date_absence"));
		absence.setIdJustify(pRs.getInt("id_justify"));
		absence.setAcademicYear(pRs.getInt("accademic_year"));
		
		return absence;
	}
	
	/**
	 * Consente la lettura di un solo record dal Result Set
	 * @param pRs
	 * 		Il result set da cui estrarre l'oggetto Justify
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Justify loadJustifyFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Justify justify = new Justify();
		
		justify.setIdJustify(pRs.getInt("id_justify"));
		justify.setIdUser(pRs.getInt("id_user"));
		justify.setDateJustify((Date)pRs.getDate("date_justify"));
		justify.setAcademicYear(pRs.getInt("accademic_year"));
		
		return justify;
	}
	/**
	 * Consente la lettura di un solo record dal Result Set
	 * @param pRs
	 * 		Il result set da cui estrarre l'oggetto Note
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	
	private Note loadNoteFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Note note= new Note();
		
		note.setIdNote(pRs.getInt("id_note"));
		note.setIdUser(pRs.getInt("id_user"));
		note.setDateNote((Date)pRs.getDate("date_note"));
		note.setDescription(pRs.getString("description"));
		note.setTeacher(pRs.getString("teacher"));	
		note.setAcademicYear(pRs.getInt("accademic_year"));
	
		return note;
	}

	/**
	 * Consente la lettura di un solo record dal Result Set
	 * @param pRs
	 * 		Il result set da cui estrarre l'oggetto Delay
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	
	private Delay loadDelayFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Delay delay = new Delay();
				
		delay.setIdDelay(pRs.getInt("id_delay"));
		delay.setIdUser(pRs.getInt("id_user"));
		delay.setDateDelay((Date)pRs.getDate("date_delay"));
		delay.setTimeDelay(pRs.getString("time_delay"));
		delay.setAcademicYear(pRs.getInt("accademic_year"));
	
		return delay;
	}
}
