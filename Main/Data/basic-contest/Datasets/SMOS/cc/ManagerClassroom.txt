package smos.storage;


import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Collection;
import java.util.Iterator;
import java.util.Vector;

import smos.bean.Classroom;
import smos.bean.User;

import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.connectionManagement.DBConnection;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;

public class ManagerClassroom  {

	/**
	 * Classe che gestiste le classi dell'istituto 
	 * @author Nicola Pisanti
	 * @version 1.0
	 */
	
	private static ManagerClassroom instance;
	
	public static final String TABLE_CLASSROOM = "classroom";
	public static final String TABLE_ADDRESS ="address";
	public static final String TABLE_TEACHER_HAS_CLASSROOM = "teacher_has_classroom";
	public static final String TABLE_STUDENT_HAS_CLASSROOM = "student_has_classroom";
	
	private ManagerClassroom(){
		super();
	}
	
	
	
	/**
	 * Ritorna la sola istanza della classe esistente.
	 * 
	 * @return Ritorna l'istanza della classe.
	 */
	public static synchronized ManagerClassroom getInstance(){
		if(instance==null){
			instance = new ManagerClassroom();
		}
		return instance;
	}
	
	
	
	
	
	/**
	 * Verifica se la classe data in input è nel database
	 * @param La classe di cui bisogna verificare l'esistenza
	 * @return true se la classe è nel database, altrimenti false
	 * @throws MandatoryFieldException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized boolean exists (Classroom pClassroom) throws MandatoryFieldException, ConnectionException, SQLException {
		
		boolean result = false;
		Connection connect = null;

		if (pClassroom.getName() == null)
			throw new MandatoryFieldException("Specificare il nome della classe.");
		if (pClassroom.getAcademicYear() <=1970)
			throw new MandatoryFieldException("Specificare l'anno accademico");
		if (pClassroom.getIdAddress()<=0){
			throw new MandatoryFieldException("Specificare l'indirizzo");
			//l'utente inserisce l'indirizzo, viene convertito in idAddress
		}
		
		try {
			//Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			String sql = "SELECT * FROM " 
				+ ManagerClassroom.TABLE_CLASSROOM
				+ " WHERE name = " 
				+ Utility.isNull(pClassroom.getName()) 
				+ " AND accademic_year = "
				+ Utility.isNull(pClassroom.getAcademicYear()
				+ " AND id_address = "
				+ Utility.isNull(pClassroom.getIdAddress())
				
				);

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
	 * Inserisce l'oggetto di tipo classe nel database
	 * @param la classe da inserire nel database
	 * @throws MandatoryFieldException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException
	 */
	public synchronized void insert(Classroom pClassroom) throws MandatoryFieldException, 
		ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		
		Connection connect= null;
		try{
			// controllo dei campi obbligatori
			if (pClassroom.getName() == null)
				throw new MandatoryFieldException("Specificare il nome della classe.");
			if (pClassroom.getAcademicYear() <=1970)
				throw new MandatoryFieldException("Specificare l'anno accademico");
			if (pClassroom.getIdAddress()<=0){
				throw new MandatoryFieldException("Specificare l'indirizzo");
				//l'utente inserisce l'indirizzo, viene convertito in idAddress
			}
			
			connect = DBConnection.getConnection();
			if (connect==null)
				throw new ConnectionException();
			//Prepariamo la stringa Sql
			String sql =
				"INSERT INTO " 
				+ ManagerClassroom.TABLE_CLASSROOM 
				+ " (id_address, name, accademic_year) " 
				+ "VALUES (" 
				+ Utility.isNull(pClassroom.getIdAddress()) 
				+ "," 
				+ Utility.isNull(pClassroom.getName()) 
				+ "," 
				+ Utility.isNull(pClassroom.getAcademicYear())
				+ ")";
		
			Utility.executeOperation(connect,sql);
		
			pClassroom.setIdClassroom((Utility.getMaxValue("id_classroom",ManagerClassroom.TABLE_CLASSROOM)));
		
		}finally {
		//rilascia le risorse
		
		DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Aggiorna le statistiche di una classe
	 * @param La classe con le statistiche aggiornate (ma ID identico)
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 */
	public synchronized void update (Classroom pClassroom) throws ConnectionException,
	SQLException, EntityNotFoundException, MandatoryFieldException{
		Connection connect= null;
		
		try{
			if (pClassroom.getIdClassroom()<=0)
				throw new EntityNotFoundException("Impossibile trovare la classe!");
			
			if (pClassroom.getName() == null)
				throw new MandatoryFieldException("Specificare il nome della classe.");
			if (pClassroom.getAcademicYear() <=1970)
				throw new MandatoryFieldException("Specificare l'anno accademico");
			if (pClassroom.getIdAddress()<=0){
				throw new MandatoryFieldException("Specificare l'indirizzo");
				//l'utente inserisce l'indirizzo, viene convertito in idAddress
			}
			//Prepariamo la stringa SQL
			String sql=
				"UPDATE " 
				+	ManagerClassroom.TABLE_CLASSROOM 
				+ " SET" 
				+ " id_address = " 
				+ Utility.isNull(pClassroom.getIdAddress()) 
				+ ", name = " 
				+ Utility.isNull(pClassroom.getName()) 
				+ ", accademic_year = " 
				+ Utility.isNull(pClassroom.getAcademicYear())  
				+ " WHERE id_classroom = " 
				+ Utility.isNull(pClassroom.getIdClassroom());
			
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
	 * Cancella una classe dal database
	 * @param La classe da cancellare
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws MandatoryFieldException
	 * @throws InvalidValueException
	 */
	public synchronized void delete (Classroom pClassroom) throws ConnectionException, 
			SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
		Connection connect = null;
		
		
		try {
			//ManagerUser.getInstance().userOnDeleteCascade(pUser);
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerClassroom.TABLE_CLASSROOM 
							+ " WHERE id_classroom = "
							+ Utility.isNull(pClassroom.getIdClassroom());
			
				Utility.executeOperation(connect, sql);
		}finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	
	public synchronized Collection<Classroom> getClassroomsByStudent(User pUser) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException, MandatoryFieldException{
		Collection<Classroom> result=null;
		Connection connect = null;
		ManagerUser managerUser = ManagerUser.getInstance();
		try
		{
			// Se non esiste l'utente
			if (!managerUser.exists(pUser))
					throw new EntityNotFoundException("L'utente non esiste!!!");
			if(!managerUser.isStudent(pUser))
					throw new InvalidValueException("L'utente non � uno studente!");
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			int iduser=managerUser.getUserId(pUser);
			String tSql = 
				
				"SELECT " 
				+ ManagerClassroom.TABLE_CLASSROOM 
				+".* FROM " 
				+ ManagerClassroom.TABLE_STUDENT_HAS_CLASSROOM 
				+ ", "
				+ ManagerClassroom.TABLE_CLASSROOM 
				+ " WHERE "
				+ ManagerClassroom.TABLE_STUDENT_HAS_CLASSROOM
				+ ".id_user = "  
				+ Utility.isNull(iduser)
				+" AND "
				+ ManagerClassroom.TABLE_CLASSROOM 
				+".id_classroom = "
				+ ManagerClassroom.TABLE_STUDENT_HAS_CLASSROOM 
				+".id_classroom";
				
				
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
				result = this.loadRecordsFromRs(tRs);
				
			if(result.isEmpty()) 
				throw new EntityNotFoundException("Impossibile Trovare Classi per l'utente inserito");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Restituisce la classe che ha l'ID passato 
	 * @param L'ID della classe cercata
	 * @return una stringa che rappresenta la classe con l'ID fornito
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	public synchronized Classroom getClassroomByID(int pId) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Classroom result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pId <= 0)
				throw new EntityNotFoundException("Impossibile trovare la classe!");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerClassroom.TABLE_CLASSROOM 
				+ " WHERE id_classroom = " 
				+ Utility.isNull(pId) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = this.loadRecordFromRs(tRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare l'utente!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	
	/**
	 * Restituisce una collezione di classi dello stesso anno accademico
	 */
	public synchronized Collection<Classroom> getClassroomsByAcademicYear(int pAcademicYear) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Collection<Classroom> result=null;
		Connection connect = null;
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pAcademicYear <= 1970)
				throw new EntityNotFoundException("Data troppo vecchia");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT * FROM " 
				+ ManagerClassroom.TABLE_CLASSROOM 
				+ " WHERE accademic_year = " 
				+ Utility.isNull(pAcademicYear) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
				result = this.loadRecordsFromRs(tRs);
				
			if(result.isEmpty()) 
				throw new EntityNotFoundException("Impossibile Trovare Classi per la data inserita");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
		
	public synchronized Collection<Integer> getAcademicYearList() throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Collection<Integer> result=null;
		Connection connect = null;
		try
		{	
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			String tSql = 
				"SELECT DISTINCT accademic_year FROM " 
				+ ManagerClassroom.TABLE_CLASSROOM
				+ " order by accademic_year ";
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
				result = this.loadIntegersFromRs(tRs);
				
			if(result.isEmpty()) 
				throw new EntityNotFoundException("Impossibile Trovare Classi per la data inserita");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	
	public synchronized Classroom getClassroomByUserAcademicYear(User pUser, int pAcademicYear) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException, MandatoryFieldException{
		Classroom result = null;
		Classroom temp = null;
		ManagerClassroom managerClassroom = ManagerClassroom.getInstance();
		Collection<Classroom> list = null;
		list = managerClassroom.getClassroomsByStudent(pUser);
		Iterator<Classroom> it = list.iterator();
		while(it.hasNext()){
			temp = it.next();
			if(temp.getAcademicYear()==pAcademicYear){
				result = temp;
				break;
			}
		}
		return result;
	}
	public synchronized Collection<Classroom> getClassroomsByTeacherAcademicYear(User pUser, int pAcademicYear) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException, MandatoryFieldException{
		Collection<Classroom> result = null;
		Connection connect = null;
		int idUser = pUser.getId();
		try
		{
			// Se non è stato fornito l'id restituiamo un codice di errore
			if (pAcademicYear <= 1970)
				throw new EntityNotFoundException("Data troppo vecchia");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * 
			 */
			String tSql = 
				"SELECT DISTINCT " 
				+ ManagerClassroom.TABLE_CLASSROOM +".* FROM "  
				+ ManagerClassroom.TABLE_CLASSROOM + ", "
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM 
				+ " WHERE  "
				+ ManagerClassroom.TABLE_CLASSROOM + ".id_classroom = "
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM 
				+ ".id_classroom  AND "
				+ ManagerClassroom.TABLE_CLASSROOM + ".accademic_year = "
				+ Utility.isNull(pAcademicYear)
				+ " AND "
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM + ".id_user = "
				+ Utility.isNull(idUser)
				;
			
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
				result = this.loadRecordsFromRs(tRs);
				
			if(result.isEmpty()) 
				throw new EntityNotFoundException("Impossibile Trovare Classi per l'utente e l'anno inseriti");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	
	}
	public synchronized Collection<Classroom> getClassroomsByTeacher(User pUser) 
	throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException, MandatoryFieldException{
		Collection<Classroom> result=null;
		Connection connect = null;
		ManagerUser managerUser = ManagerUser.getInstance();
		try
		{
			// Se non esiste l'utente
			if (!managerUser.exists(pUser))
					throw new EntityNotFoundException("L'utente non esiste!!!");
			if(!managerUser.isTeacher(pUser))
					throw new InvalidValueException("L'utente non � uno studente!");
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti alla classe dell'id passato
			 */
			int iduser=managerUser.getUserId(pUser);
			String tSql = 
				
				"SELECT DISTINCT " 
				+ ManagerClassroom.TABLE_CLASSROOM 
				+".* FROM " 
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM 
				+ ", "
				+ ManagerClassroom.TABLE_CLASSROOM 
				+ " WHERE "
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM
				+ ".id_user = "  
				+ Utility.isNull(iduser)
				+" AND "
				+ ManagerClassroom.TABLE_CLASSROOM 
				+".id_classroom = "
				+ ManagerClassroom.TABLE_TEACHER_HAS_CLASSROOM 
				+".id_classroom";
				
				
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
				result = this.loadRecordsFromRs(tRs);
				
			if(result.isEmpty()) {
				
				throw new EntityNotFoundException("Impossibile Trovare Classi per l'utente inserito");
			}
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	/** COnsente la lettura di un intero dal recod resultSet
	 * 
	 * @param pRs
	 * 		resultSet
	 * @return
	 * 	collection<Integer>
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Collection<Integer> loadIntegersFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Collection<Integer> result = new Vector<Integer>();
		while(pRs.next())  {
			result.add(pRs.getInt("accademic_year"));
		} 
		return result;
	}



	/**
	 * Consente la lettura di un solo record dal Result Set
	 * @param Il result set da cui estrarre l'oggetto Classroom
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	private Classroom loadRecordFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Classroom classroom = new Classroom();
		classroom.setName(pRs.getString("name"));
		classroom.setAcademicYear(pRs.getInt("accademic_year"));
		classroom.setIdClassroom(pRs.getInt("id_classroom"));
		classroom.setIdAddress(pRs.getInt("id_address"));
		return classroom;
	}

	/**
	 * Consente la lettura di un più record dal Result Set
	 * @param Il result set da cui estrarre l'oggetto Classroom
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	
	private Collection<Classroom> loadRecordsFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Collection<Classroom> result = new Vector<Classroom>();
		while(pRs.next())  {
			result.add(loadRecordFromRs(pRs));
		} 
		return result;
	}

}
