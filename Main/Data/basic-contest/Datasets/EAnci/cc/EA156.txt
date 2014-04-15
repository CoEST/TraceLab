package DB;
import Bean.*;
import com.mchange.v2.c3p0.ComboPooledDataSource;
import java.util.*;
import java.beans.PropertyVetoException;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.sql.*;

/** 
 * Questa classe fornisce un pool di connesione
 * @author Antonio Leone
 */

public class DbConnection  {
	
	private static Properties dbProperties;
	private static ComboPooledDataSource cpds=null;
	
	/**
	 * Questa porzione di codice crea un pool di connessione e definisce i db properties
	 */
	static 
	{
            try 
            {
				DbConnection.loadDbProperties();
				DbConnection.createPool();
			} 
            catch (IOException e) 
            {
				throw new DbException("Errore nella definizione dei DB properties");
			}
			catch (SQLException e) 
			{
				throw new DbException("Errore nella creazione del Poll di Connessioni");
			} 
			catch (PropertyVetoException e) 
			{
				throw new DbException("Errore nella creazione del Poll di Connessioni");
			} 
    }
	
	private static void createPool() throws SQLException, PropertyVetoException
	{
    	cpds = new ComboPooledDataSource(); 
		cpds.setJdbcUrl( DbConnection.dbProperties.getProperty("url")); 
		cpds.setDriverClass(DbConnection.dbProperties.getProperty("driver")); 
		cpds.setUser(DbConnection.dbProperties.getProperty("username")); 
		cpds.setPassword(DbConnection.dbProperties.getProperty("password"));
	}
	
	/**
	 * Ritorna una connessione al db, se non è già disponibile ne viene creata una nuova
	 * @return una connessione al db
	 * @throws SQLException 
	 */
	
	public static synchronized Connection getConnection() throws SQLException  
	{
		return cpds.getConnection();
	}
	
	/**
	 * Carica i db properties
	 * @throws IOException
	 */
	
	private static void loadDbProperties() throws IOException
	{
		 InputStream fileProperties = new FileInputStream("database.properties");
		 DbConnection.dbProperties = new Properties();
		 DbConnection.dbProperties.load(fileProperties);      	
	}
	
	/**
	 * Metodo che chiude il pool di connessione
	 */
	
	public void closePool()
	{
		cpds.close();
	}
}