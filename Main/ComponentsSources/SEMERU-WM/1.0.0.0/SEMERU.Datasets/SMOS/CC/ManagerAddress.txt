package smos.storage;
import smos.bean.Address;
import smos.bean.Teaching;
import smos.exception.DuplicatedEntityException;
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
* Classe manager degli indirizzi 
*
*/

public class ManagerAddress {

	private static ManagerAddress instance;
	
     /**
	 * Il nome della tabella degli indirizzi
	 */
	public static final String TABLE_ADDRESS = "address";
	public static final String TABLE_ADDRESS_HAS_TEACHING = "address_has_teaching";
	
	private ManagerAddress() {
		super();
	}
	

	/**
	 * Ritorna la sola istanza della classe esistente.
	 * 
	 * @return Ritorna l'istanza della classe.
	 */
	public static synchronized ManagerAddress getInstance(){
		if(instance==null){
			instance = new ManagerAddress();
			}
			return instance;
		}
	/**
	 * Verifica l'esistenza di un indirizzo nel database.
	 * 
	 * @param pAddress
	 *            L'indirizzo da controllare.
	 * @return Ritorna true se esiste già l'indirizzo passato come parametro,
	 * 			false altrimenti.
	 *  
	 * @throws MandatoryFieldException 
	 * @throws SQLException
	 * @throws MandatoryFieldException
	 * @throws ConnectionException 
	 * @throws ConnectionException
	 * @throws SQLException 
	 */
	
	public synchronized boolean hasTeaching(Teaching pTeaching, Address pAddress)
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
			+ ManagerTeaching.TABLE_ADDRESS_TEACHINGS
			+ " WHERE id_teaching = "
			+ Utility.isNull(pTeaching.getId())
			+" AND id_address = "
			+ Utility.isNull(pAddress.getIdAddress());
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

	public synchronized boolean exists (Address pAddress) throws MandatoryFieldException, ConnectionException, SQLException {
	boolean result= false;
	Connection connect = null;
	
	if (pAddress.getName() == null)
		throw new MandatoryFieldException("Specificare il nome.");
	try{
		//Otteniamo la connessione al database
		connect= DBConnection.getConnection();
		
		if (connect == null)
			throw new ConnectionException();
		
		String sql =" SELECT * FROM "
		+ ManagerAddress.TABLE_ADDRESS 
		+ " WHERE name = "
		+ Utility.isNull(pAddress.getName());
		
		ResultSet tRs = Utility.queryOperation(connect, sql);
		
		if(tRs.next())
			result = true;
		
		return result;
		
	}
	finally{
		DBConnection.releaseConnection(connect);
	}
	}
	/**
	 * Inserisce un nuovo indirizzo nella tabella address.
	 * 
	 * @param pAddress 
	 * 			L'indirizzo da inserire.
	 * 
	 * @throws SQLException
	 * @throws ConnectionException
	 * @throws MandatoryFieldException 
	 * @throws EntityNotFoundException  
	 * @throws InvalidValueException 
	 */
	
	public synchronized void insert(Address pAddress) 
	throws MandatoryFieldException, ConnectionException, 
	SQLException, EntityNotFoundException, 
	InvalidValueException{
	Connection connect= null;
try{
// controllo dei campi obbligatori
if(pAddress.getName()==null)
	throw new MandatoryFieldException("Specificare il campo nome");

	connect = DBConnection.getConnection();
if (connect==null)
	throw new ConnectionException();
	//Prepariamo la stringa Sql
	String sql =
	"INSERT INTO " 
	+ ManagerAddress.TABLE_ADDRESS 
	+ " (name) " 
	+ "VALUES (" 
	+ Utility.isNull(pAddress.getName()) 
	+ ")";

	Utility.executeOperation(connect,sql);

	pAddress.setIdAddress(Utility.getMaxValue("id_address",ManagerAddress.TABLE_ADDRESS));

	}finally {
		//rilascia le risorse

		DBConnection.releaseConnection(connect);
}
}
	/**
	 * Elimina un indirizzo dalla tabella address.
	 * 
	 * @param pAddress 
	 * 			L'indirizzo da eliminare.
	 * 
	 * @throws MandatoryFieldException 
	 * @throws EntityNotFoundException 
	 * @throws SQLException 
	 * @throws ConnectionException 
	 * @throws InvalidValueException 
	 * 
	 */
	public synchronized void delete (Address pAddress) throws ConnectionException, 
			SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
		Connection connect = null;

		
		try {
			//ManagerAddress.getInstance().AddressOnDeleteCascade(pAddress);
			connect = DBConnection.getConnection();
				//Prepariamo la stringa SQL
				String sql = "DELETE FROM " 
							+ ManagerAddress.TABLE_ADDRESS 
							+ " WHERE id_address = "
							+ Utility.isNull(pAddress.getIdAddress());
			
				Utility.executeOperation(connect, sql);
		}  
		finally {
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	public synchronized void assignTeachingAsAddress (Address pAddress, Teaching pTeaching) throws ConnectionException, 
	SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException, DuplicatedEntityException {
Connection connect = null;
ManagerAddress managerAddress = ManagerAddress.getInstance();
if(managerAddress.hasTeaching(pTeaching, pAddress))
	throw new DuplicatedEntityException("Questo indirizzo ha già quest'insegnamento associato");

try {
	//ManagerAddress.getInstance().AddressOnDeleteCascade(pAddress);
	connect = DBConnection.getConnection();
		//Prepariamo la stringa SQL
		String sql = "INSERT INTO " 
					+ ManagerAddress.TABLE_ADDRESS_HAS_TEACHING
					+ " (id_address, id_teaching) "
					+ " VALUES( "
					+ Utility.isNull(pAddress.getIdAddress())
					+ " , "
					+ Utility.isNull(pTeaching.getId())
					+ " )";
	
		Utility.executeOperation(connect, sql);
}  
finally {
	//rilascia le risorse
	DBConnection.releaseConnection(connect);
}
}
	public synchronized void removeTeachingAsAddress (Address pAddress, Teaching pTeaching) throws ConnectionException, 
	SQLException, EntityNotFoundException, MandatoryFieldException, InvalidValueException {
Connection connect = null;
ManagerAddress managerAddress = ManagerAddress.getInstance();
if(!managerAddress.hasTeaching(pTeaching, pAddress))
	throw new EntityNotFoundException("Questo indirizzo non contiene l'insegnamento da rimuovere");

try {
	//ManagerAddress.getInstance().AddressOnDeleteCascade(pAddress);
	connect = DBConnection.getConnection();
		//Prepariamo la stringa SQL
		String sql = "DELETE FROM " 
					+ ManagerAddress.TABLE_ADDRESS_HAS_TEACHING
					+ " WHERE id_address= "
					+ Utility.isNull(pAddress.getIdAddress())		
					+ " AND id_teaching = "
					+ Utility.isNull(pTeaching.getId());
	
		Utility.executeOperation(connect, sql);
}  
finally {
	//rilascia le risorse
	DBConnection.releaseConnection(connect);
}
}

	/**
	 * Ritorna l'id dell'indirizzo passato come parametro.
	 * 
	 * @param pAddress
	 *            L'indirizzo di cui si richiede l'id.
	 * @return Ritorna l'id dell'indirizzo passato come parametro.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized int getAddressId(Address pAddress)
			throws EntityNotFoundException, ConnectionException, SQLException {
		int result = 0;
		Connection connect = null;
		try {
			if (pAddress == null)
				throw new EntityNotFoundException("Impossibile trovare l'indirizzo!");

			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'indirizzo passato come parametro.
			 */
			String tSql = "SELECT id_address FROM " 
				+ ManagerAddress.TABLE_ADDRESS
				+ " WHERE name = " 
				+ Utility.isNull(pAddress.getName());

			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null)
				throw new ConnectionException();

			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);

			if (tRs.next())
				result = tRs.getInt("id_address");

			return result;
		} finally {
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Ritorna l'indirizzo corrispondente all'id passato come parametro.
	 * 
	 * @param pIdAddress
	 * 			L'id dell'indirizzo.
	 * @return Ritorna l'indirizzo associato all'id passato come parametro.
	 * 
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws EntityNotFoundException
	 * @throws InvalidValueException 
	 */
	public synchronized Address getAddressById (int pIdAddress) throws ConnectionException, SQLException, EntityNotFoundException, InvalidValueException{
		Address result = null;
		Connection connect = null;
		try
		{
						
			if (pIdAddress <= 0) 
				throw new EntityNotFoundException("Impossibile trovare l'indirizzo!");
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Prepariamo la stringa SQL
			String sql = 
				"SELECT * FROM " 
				+ ManagerAddress.TABLE_ADDRESS
				+ " WHERE id_address = " 
				+ Utility.isNull(pIdAddress);
			
			// Inviamo la Query al DataBase
			ResultSet pRs = Utility.queryOperation(connect, sql);
			
			if (pRs.next()) 
				result = this.loadRecordFromRs(pRs);
			else 
				throw new EntityNotFoundException("Impossibile trovare l'utente!");
							
			return result;
		}finally{
			//rilascia le risorse
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Ritorna l'insieme di tutti gli indirizzi presenti nel database.
	 * 
	 * @return Ritorna una collection di indirizzi.
	 * 
	 * @throws ConnectionException 
	 * @throws EntityNotFoundException 
	 * @throws SQLException 
	 * @throws InvalidValueException 
	 */
	public synchronized Collection<Address> getAddressList() throws ConnectionException, EntityNotFoundException, SQLException, InvalidValueException{
		Connection connect = null;
		Collection<Address> result = new Vector<Address>();;
		
		try {
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			//Prepariamo la stringa sql
			String sql = "SELECT * "  
				+ " FROM " 
				+ ManagerAddress.TABLE_ADDRESS 
				+ " ORDER BY id_address";
				
			//Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, sql);
			
			if (tRs.next())
				result = this.loadRecordsFromRs(tRs);		
			return result;
		} finally {
			//rilascia le risorse			
			DBConnection.releaseConnection(connect);
		}
	}
	/**
	 * Ritorna il nome dell'indirizzo corrispondente all'id 
	 * passato come parametro.
	 * 
	 * @param pIdAddress
	 * 			L'id dell'indirizzo.
	 * @return Ritorna una stringa contenente il nome dell'indirizzo.
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 */
	public synchronized String getAddressNameById(int pIdAddress) throws EntityNotFoundException, ConnectionException, SQLException{
		String result;
		Connection connect = null;
		try
		{
			// Se non e' stato fornito l'id restituiamo un codice di errore
			if (pIdAddress <= 0)
				throw new EntityNotFoundException("Impossibile trovare l'indirizzo!");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'utente passato come parametro 
			 */
			String tSql = 
				"SELECT name FROM " 
				+ ManagerAddress.TABLE_ADDRESS 
				+ " WHERE id_address = " 
				+ Utility.isNull(pIdAddress) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			if (tRs.next()) 
				result = tRs.getString("name");
			else 
				throw new EntityNotFoundException("Impossibile trovare l'indirizzo!");
			
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}
	
	/**
	 * Ritorna una collection con gli id degli insegnamenti associati all'id 
	 * passato come parametro.
	 * 
	 * @param pIdAddress
	 * 			L'id dell'indirizzo.
	 * @return Ritorna una collection con di int 
	 * 
	 * @throws EntityNotFoundException
	 * @throws ConnectionException
	 * @throws SQLException
	 * @throws InvalidValueException
	 */
	
	public synchronized Collection<Integer> getAddressTechings(int pIdAddress) throws EntityNotFoundException, ConnectionException, SQLException, InvalidValueException{
		Collection<Integer> result;
		Connection connect = null;
		try
		{
			// Se non e' stato fornito l'id restituiamo un codice di errore
			if (pIdAddress <= 0)
				throw new EntityNotFoundException("Impossibile trovare l'indirizzo!");
			
			
			/*
			 * Prepariamo la stringa SQL per recuperare le informazioni
			 * corrispondenti all'id dell'utente passato come parametro 
			 */
			String tSql = 
				"SELECT id_teaching FROM " 
				+ ManagerAddress.TABLE_ADDRESS_HAS_TEACHING
				+ " WHERE id_address = " 
				+ Utility.isNull(pIdAddress) ;
			
			// Otteniamo una Connessione al DataBase
			connect = DBConnection.getConnection();
			if (connect == null) 
				throw new ConnectionException();
			
			// Inviamo la Query al DataBase
			ResultSet tRs = Utility.queryOperation(connect, tSql);
			
			result = this.loadIntegersFromRs(tRs);
			return result;
		}finally{
			DBConnection.releaseConnection(connect);
		}
	}

	/*
	 * Consente la lettura di un record dal ResultSet.
	 */
	private Address loadRecordFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Address address = new Address();
		address.setName(pRs.getString("name"));
		address.setIdAddress(pRs.getInt("id_address"));
		return address;
	}
	
	/*
	 * Consente la lettura dei record dal ResultSet.
	 */
	private Collection<Address> loadRecordsFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Collection<Address> result = new Vector<Address>();
		do  {
			result.add(loadRecordFromRs(pRs));
		} while (pRs.next());
		return result;
	}
	
	private Collection<Integer> loadIntegersFromRs(ResultSet pRs) throws SQLException, InvalidValueException{
		Collection<Integer> result = new Vector<Integer>();
		while(pRs.next())  {
			result.add(pRs.getInt("id_teaching"));
		} 
		return result;
	}
	
		
	
}
