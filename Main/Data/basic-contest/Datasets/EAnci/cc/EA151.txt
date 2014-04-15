package DB;
import Bean.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Collection;

/**
 * La classe DbAmministratore si occupa di gestire le connessioni al db
 * @author Antonio Leone
 * @version 1.0
 */

public class DbAmministratore {

	private Connection connection;
	
	public DbAmministratore() throws DbException
	{
		try
		{
			connection=DbConnection.getConnection();
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: connessione non riuscita");
		}
	}
	
	/**
	 * Metodo che inserisce un amministratore all'interno del db
	 * @param i Oggetto di tipo Amministratore
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean inserisciAmministratore(Amministratore a)throws DbException
	{
		String matr=a.getMatricola();
		String nome=a.getNome();
		String cogn=a.getCognome();
		String email=a.getEmail();
		String login=a.getLogin();
		int ret=0;
		PreparedStatement statement=null;
		try
		{
			statement=connection.prepareStatement("INSERT INTO amministratore VALUES (? ,? ,?,?,?)");
			statement.setString(1, matr);
			statement.setString(2,nome);
			statement.setString(3,cogn);
			statement.setString(4, email);
			statement.setString(5, login);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: inserimento amministratore non riuscito");
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
				throw new DbException("Errore: inserimento amministratore non riuscito");
			}
		}
	}
	
	/**
	 * Metodo che elimina un Amministratore  dal db
	 * @param matr l'intero che viene utilizzato come matricola
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean eliminaAmministratore(String  matr) throws DbException
	{
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("DELETE FROM amministratore WHERE matricola =?");
			statement.setString(1,matr);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: cancellazione amministratore non riuscita");
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
				throw new DbException("Errore: cancellazione amministratore non riuscita");
			}
		}
	}
	
	/** Metodo che restituisce un insieme di amministratori
	 * @param nomeImp stringa che viene utilizzata come nome dell'amministratore
	 * @param cognImp stringa che viene utilizzata come cognome dell'amministratore
	 * @return Restituisce una Collection di Amministratori
	 * @throws DbException
	 */
	
	public Collection<Amministratore> getAmministratoreByName(String nomeAmm,String cognAmm) throws DbException
	{
		ArrayList<Amministratore> ret = new ArrayList<Amministratore>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM amministratore WHERE nome =? and cognome =?");
			statement.setString(1,nomeAmm);
			statement.setString(2,cognAmm);
			rs= statement.executeQuery();
			while(rs.next())
			{
				String matr = rs.getString("matricola");
				String nome = rs.getString("nome");
				String cognome = rs.getString("cognome");
				String eMail = rs.getString("eMail");
				String login = rs.getString("login");
				ret.add(new Amministratore(nome,cognome,matr,eMail,login));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca amministratore tramite nome e cognome non riuscita");
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
				throw new DbException("Errore: ricerca amministratore tramite nome e cognome non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un amministratore
	 * @param matrImp stringa che viene utilizzato come matricola dell'amministratore
	 * @return Restituisce un oggetto di tipo Amministratore
	 * @throws DbException
	 */
	
	public Amministratore getAmministratoreByMatricola(String matrAmm) throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Amministratore ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM amministratore WHERE matricola =?");
			statement.setString(1,matrAmm);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String matr = rs.getString("matricola");
			String nome = rs.getString("nome");
			String cognome = rs.getString("cognome");
			String eMail = rs.getString("eMail");
			String login = rs.getString("login");
			ret=new Amministratore(nome,cognome,matr,eMail,login);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca amministratore tramite matricola non riuscita");
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
				throw new DbException("Errore: ricerca amministratore tramite matricola non riuscita");
			}
		}	
	}
	
	/**
	 * Metodo che restituisce tutti gli amministratori memorizzati
	 * @return Restituisce una Collection di Amministratori
	 * @throws DbException
	 */
	
	public Collection<Amministratore> getAmministratori() throws DbException
	{
		ArrayList<Amministratore> ret = new ArrayList<Amministratore>(); 
		Statement statement =null;
		ResultSet rs =null;
		try
		{
			statement=connection.createStatement();
			rs = statement.executeQuery("SELECT * FROM amministratore");
			while(rs.next())
			{
				String matr = rs.getString("matricola");
				String nome = rs.getString("nome");
				String cognome = rs.getString("cognome");
				String eMail = rs.getString("eMail");
				String login = rs.getString("login");
				ret.add(new Amministratore(nome,cognome,matr,eMail,login));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca amministratori non riuscita");
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
				throw new DbException("Errore: ricerca amministratori non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un amministratore
	 * @param log stringa che viene utilizzata come login dell'amministratore
	 * @return Restituisce un oggetto di tipo amministratore
	 * @throws DbException
	 */
	
	public Amministratore getAmministratoreByLogin(String log)throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Amministratore ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM amministratore WHERE login =?");
			statement.setString(1,log);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String matr = rs.getString("matricola");
			String nome = rs.getString("nome");
			String cognome = rs.getString("cognome");
			String eMail = rs.getString("eMail");
			String login = rs.getString("login");
			ret=new Amministratore(nome,cognome,matr,eMail,login);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca amministratore tramite login non riuscita");
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
				throw new DbException("Errore: ricerca amministratore tramite login non riuscita");
			}
		}	
	}
	
	/**
	 * Metodo che modifica un amministratore
	 * @param matr la stringa che identifica l'amministratore
	 * @param a Amministratore con i dati aggiornati
	 * @return True se è stato effettuata una modifica nel db, False altrimenti
	 */
	
	public boolean modificaAmministratore(String matr, Amministratore a)
	{
		String matricola=a.getMatricola();
		String nome=a.getNome();
		String cogn=a.getCognome();
		String email=a.getEmail();
		String login=a.getLogin();
		int ret=0;
		PreparedStatement statement=null;
		try
		{
			statement=connection.prepareStatement("UPDATE amministratore SET matricola = ?,nome = ?, cognome = ?, email = ?, login = ? WHERE matricola = ?");
			statement.setString(1, matricola);
			statement.setString(2,nome);
			statement.setString(3,cogn);
			statement.setString(4, email);
			statement.setString(5, login);
			statement.setString(6,matr);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: modifica amministratore non riuscita");
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
				throw new DbException("Errore: modifica amministratore non riuscita");
			}
		}
	}
}