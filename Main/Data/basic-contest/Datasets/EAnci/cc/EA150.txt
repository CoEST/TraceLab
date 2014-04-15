package DB;
import Bean.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Collection;


/**
 * La classe DbAccesso si occupa di gestire le connessioni al db
 * per consentire gli accessi.
 * @author Antonio Leone
 * @version 1.0
 */

public class DbAccesso {

	private Connection connection;
	
	public DbAccesso() throws DbException 
	{
		try
		{
			connection=DbConnection.getConnection();
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: Connessione non riuscita");
		}
	}
	
	/**
	 * Metodo che inserisce un accesso all'interno del db
	 * @param a Oggetto di tipo Accesso
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean inserisciAccesso(Accesso a) throws DbException
	{	
		int ret=0;
		PreparedStatement statement=null;
		String login=a.getLogin();
		String password=a.getPassword();
		String tipo=a.getTipo();
		try
		{
			statement=connection.prepareStatement("INSERT INTO accesso VALUES (? ,? ,?)");
			statement.setString(1,login);
			statement.setString(2, password);
			statement.setString(3, tipo);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: inserimento accesso non riuscito");
		}
		finally
		{
			try 
			{
				if(statement!=null)
					statement.close();
			} 
			catch (SQLException e) 
			{
				throw new DbException("Errore: inserimento accesso non riuscito");
			}
		}
	}
	
	/**
	 * Metodo che elimina un accesso  dal db
	 * @param log Stringa che viene usata come login
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean eliminaAccesso(String log) throws DbException
	{	
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("DELETE FROM accesso WHERE login =?");
			statement.setString(1,log);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: cancellazione accesso non riuscita");
		}
		finally
		{
			try 
			{
				if(statement!=null)
					statement.close();
			} 
			catch (SQLException e) 
			{
				throw new DbException("Errore: cancellazione accesso non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un accesso
	 * @param log Stringa che viene usata come login
	 * @return Restituisce un oggetto di tipo Accesso
	 * @throws DbException
	 */
	
	public Accesso getAccesso(String log) throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Accesso ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT* FROM accesso WHERE login=?");
			statement.setString(1,log);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String login = rs.getString("login");
			String password = rs.getString("password");
			String tipo = rs.getString("tipo");
			ret=new Accesso(login,password,tipo);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca accesso non riuscita");
		}
		finally
		{
			try
			{
				if(statement!=null)
					statement.close();
				if(rs!=null)
					rs.close();
			}
			catch(SQLException e)
			{
				throw new DbException("Errore: ricerca accesso non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce tutti gli accessi memorizzati
	 * @return Restituisce una Collection di Accessi
	 * @throws DbException
	 */
	
	public Collection<Accesso> getAccessi() throws DbException
	{
		ArrayList<Accesso> ret = new ArrayList<Accesso>(); 
		Statement statement =null;
		ResultSet rs =null;
		try
		{
			statement=connection.createStatement();
			rs = statement.executeQuery("SELECT * FROM accesso");
			while(rs.next())
			{
				String login = rs.getString("login");
				String password = rs.getString("password");
				String tipo = rs.getString("tipo");
				ret.add(new Accesso(login,password,tipo));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca accessi non riuscita");
		}
		finally
		{
			try
			{
				if(statement!=null)
					statement.close();
				if(rs!=null)
					rs.close();
			}
			catch(SQLException e)
			{
				throw new DbException("Errore: ricerca accessi non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che permette di controllare l’esistenza della login 
	 * @param login Stringa che viene usata come login
	 * @return True se la login è presente, False altrimenti
	 * @throws DbException
	 */
	
	public boolean controllaLogin(String login)throws DbException
	{
			PreparedStatement statement =null;
			ResultSet rs=null;
			boolean ret=false;
			try
			{
				statement=connection.prepareStatement("SELECT * FROM accesso WHERE login = ?");
				statement.setString(1,login);
				rs= statement.executeQuery();
				ret=rs.next();
				return ret;
			}
			catch(SQLException e)
			{
				throw new DbException("Errore: controllo login non riuscito");
			}
			finally
			{
				try
				{
					if(statement!=null)
						statement.close();
					if(rs!=null)
						rs.close();
				}
				catch(SQLException e)
				{
					throw new DbException("Errore: controllo login non riuscito");
				}
			}
	}
	
	/**
	 * Metodo che permette di controllare la correttezza della login e della 
	 * password di un accesso per garantire l'apertura di una sessione autenticata
	 * @param login Stringa che viene usata come login
	 * @param password Stringa che viene usata come password
	 * @return True se l'accesso è presente, False altrimenti
	 * @throws DbException
	 */
	
	public boolean controllaAccesso(String login,String password)throws DbException
	{
		PreparedStatement statement =null;
		ResultSet rs=null;
		boolean ret=false;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM accesso WHERE login = ? and password = ?");
			statement.setString(1,login);
			statement.setString(2,password);
			rs= statement.executeQuery();
			ret=rs.next();
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: verifica accesso non riuscita");
		}
		finally
		{
			try
			{
				if(statement!=null)
					statement.close();
				if(rs!=null)
					rs.close();
			}
			catch(SQLException e)
			{
				throw new DbException("Errore: verifica accesso non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che modifica un accesso
	 * @param log la login che identifica un accesso
	 * @param a Accesso con i dati aggiornati
	 * @return True se è stato effettuata la modifica nel db, False altrimenti
	 */
	
	public boolean modificaAccesso(String log, Accesso a)
	{
		String login=a.getLogin();
		String password=a.getPassword();
		String tipo=a.getTipo();
		int ret=0;
		PreparedStatement statement=null;
		try
		{
			statement=connection.prepareStatement("UPDATE accesso SET login = ?,password = ?, tipo = ? WHERE login = ?");
			statement.setString(1, login);
			statement.setString(2, password);
			statement.setString(3, tipo);
			statement.setString(4, log);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: modifica accesso non riuscita");
		}
		finally
		{
			try
			{
				if(statement!=null)
					statement.close();
			}
			catch(SQLException e)
			{
				throw new DbException("Errore: modifica accesso non riuscita");
			}
		}
	}
}