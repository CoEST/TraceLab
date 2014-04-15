package DB;
import Bean.*;
import java.sql.*;
import java.util.ArrayList;
import java.util.Collection;

/**
 * La classe DbImpiegato si occupa di gestire le connessioni al db
 * @author Antonio Leone
 * @version 1.0
 */

public class DbImpiegato {
	
	private Connection connection;
	
	public DbImpiegato() throws DbException
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
	 * Metodo che inserisce un impiegato all'interno del db
	 * @param i Oggetto di tipo Impiegato
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean inserisciImpiegato(Impiegato i)throws DbException
	{
		String matr=i.getMatricola();
		String nome=i.getNome();
		String cogn=i.getCognome();
		String email=i.getEmail();
		String login=i.getLogin();
		int ret=0;
		PreparedStatement statement=null;
		try
		{
			statement=connection.prepareStatement("INSERT INTO impiegato VALUES (? ,? ,?,?,?)");
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
			throw new DbException("Errore: inserimento impiegato non riuscito");
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
				throw new DbException("Errore: inserimento impiegato non riuscito");
			}
		}
	}
	
	/**
	 * Metodo che elimina un impiegato  dal db
	 * @param matr la stringa che viene utilizzato come matricola
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean eliminaImpiegato(String matr) throws DbException
	{
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("DELETE FROM impiegato WHERE matricola =?");
			statement.setString(1,matr);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: cancellazione impiegato non riuscita");
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
				throw new DbException("Errore: cancellazione impiegato non riuscita");
			}
		}
	}
	
	/** Metodo che restituisce un insieme di impiegati
	 * @param nomeImp stringa che viene utilizzata come nome dell'impiegato
	 * @param cognImp stringa che viene utilizzata come cognome dell'impiegato
	 * @return Restituisce una Collection di Impiegati
	 * @throws DbException
	 */
	
	public Collection<Impiegato> getImpiegatoByName(String nomeImp,String cognImp) throws DbException
	{
		ArrayList<Impiegato> ret = new ArrayList<Impiegato>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM impiegato WHERE nome =? and cognome =?");
			statement.setString(1,nomeImp);
			statement.setString(2,cognImp);
			rs= statement.executeQuery();
			while(rs.next())
			{
				String matr = rs.getString("matricola");
				String nome = rs.getString("nome");
				String cognome = rs.getString("cognome");
				String eMail = rs.getString("eMail");
				String login = rs.getString("login");
				ret.add(new Impiegato(nome,cognome,matr,eMail,login));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca impiegato tramite nome e cognome non riuscita");
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
				throw new DbException("Errore: ricerca impiegato tramite nome e cognome non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un impiegato
	 * @param matrImp stringa che viene utilizzato come matricola dell'impiegato
	 * @return Restituisce un oggetto di tipo Impiegato
	 * @throws DbException
	 */
	
	public Impiegato getImpiegatoByMatricola(String matrImp) throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Impiegato ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM impiegato WHERE matricola =?");
			statement.setString(1,matrImp);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String matr = rs.getString("matricola");
			String nome = rs.getString("nome");
			String cognome = rs.getString("cognome");
			String eMail = rs.getString("eMail");
			String login = rs.getString("login");
			ret=new Impiegato(nome,cognome,matr,eMail,login);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca impiegato tramite matricola non riuscita");
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
				throw new DbException("Errore: ricerca impiegato tramite matricola non riuscita");
			}
		}	
	}
	
	/**
	 * Metodo che restituisce tutti gli impiegati memorizzati
	 * @return Restituisce una Collection di impiegati
	 * @throws DbException
	 */
	
	public Collection<Impiegato> getImpiegati() throws DbException
	{
		ArrayList<Impiegato> ret = new ArrayList<Impiegato>(); 
		Statement statement =null;
		ResultSet rs =null;
		try
		{
			statement=connection.createStatement();
			rs = statement.executeQuery("SELECT * FROM impiegato");
			while(rs.next())
			{
				String matr = rs.getString("matricola");
				String nome = rs.getString("nome");
				String cognome = rs.getString("cognome");
				String eMail = rs.getString("eMail");
				String login = rs.getString("login");
				ret.add(new Impiegato(nome,cognome,matr,eMail,login));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca impiegati non riuscita");
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
				throw new DbException("Errore: ricerca impiegati non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un impiegato
	 * @param log stringa che viene utilizzata come login dell'impiegato
	 * @return Restituisce un oggetto di tipo impiegato
	 * @throws DbException
	 */
	
	public Impiegato getImpiegatoByLogin(String log)throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Impiegato ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM impiegato WHERE login =?");
			statement.setString(1,log);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String matr = rs.getString("matricola");
			String nome = rs.getString("nome");
			String cognome = rs.getString("cognome");
			String eMail = rs.getString("eMail");
			String login = rs.getString("login");
			ret=new Impiegato(nome,cognome,matr,eMail,login);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca impiegato tramite login non riuscita");
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
				throw new DbException("Errore: ricerca impiegato tramite login non riuscita");
			}
		}	
	}
	
	/**
	 * Metodo che modifica un impiegato
	 * @param matr la stringa che identifica l'impiegato
	 * @param a impiegato con i dati aggiornati
	 * @return True se è stato effettuato una modifica nel db, False altrimenti
	 */
	
	public boolean modificaImpiegato(String matr, Impiegato a)
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
			statement=connection.prepareStatement("UPDATE impiegato SET matricola = ?,nome = ?, cognome = ?, email = ?, login = ? WHERE matricola = ?");
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
			throw new DbException("Errore: modifica impiegato non riuscita");
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
				throw new DbException("Errore: modifica impiegato non riuscita");
			}
		}
	}
}