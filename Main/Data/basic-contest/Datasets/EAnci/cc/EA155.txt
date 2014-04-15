package DB;
import Bean.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * La classe DbComune si occupa di gestire le connessioni al db
 * @author Antonio Leone
 * @version 1.0
 */

public class DbComune {
	
private Connection connection;
	
	public DbComune() throws DbException
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
	 * Metodo che inserisce un comune all'interno del db
	 * @param c oggetto di tipo Comune
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean inserisciComune(Comune c)throws DbException
	{
		int ret=0;
		PreparedStatement statement=null;
		String nome=c.getNome();
		String indirizzo=c.getIndirizzoId();
		try
		{
			statement=connection.prepareStatement("INSERT INTO comune VALUES (?, ?)");
			statement.setString(1, nome);
			statement.setString(2, indirizzo);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: inserimento comune non riuscito");
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
				throw new DbException("Errore: inserimento comune non riuscito");
			}
		}
	}
	
	/**
	 * Metodo che elimina un comune dal db
	 * @param nome che identifica il comune
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean eliminaComune(String nome) throws DbException
	{	
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("DELETE FROM comune WHERE nome = ?");
			statement.setString(1,nome);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: cancellazione comune non riuscita");
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
				throw new DbException("Errore: cancellazione comune non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un comune
	 * @param nome la stringa che rappresenta il nome del comune
	 * @return Restituisce un oggetto di tipo Comune
	 * @throws DbException
	 */
	
	public Comune getComuneByName(String nome)throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Comune ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT* FROM comune WHERE nome = ?");
			statement.setString(1,nome);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			String nomeC=rs.getString("nome");
			String indirizzo=rs.getString("indirizzoIp");
			ret=new Comune(nomeC,indirizzo);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca del comune tramite il nome non riuscita");
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
				throw new DbException("Errore: ricerca del comune tramite il nome non riuscita");
			}
		}
	}
}