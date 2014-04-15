package DB;
import Bean.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Collection;

/**
 * La classe DbRichiesta si occupa di gestire le connessioni al db
 * @author Antonio Leone
 * @version 1.0
 */

public class DbRichiesta {
	
private Connection connection;
	
	public DbRichiesta() throws DbException
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
	 * Metodo che inserisce una richiesta all'interno del db
	 * @param ric oggetto di tipo Richiesta
	 * @return True se è stato effettuato un inserimento nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean inserisciRichiesta(Richiesta ric)throws DbException
	{
		int ret=0;
		PreparedStatement statement=null;
		String tipo=ric.getTipo();
		String data=ric.getData();
		int richiedente=ric.getRichiedente();
		String stato=ric.getStato();
		String documento=ric.getDocumento();
		try
		{
			statement=connection.prepareStatement("INSERT INTO richiesta VALUES (?, ?, ?, ?, ?, ?)");
			statement.setInt(1,0);
			statement.setString(2, tipo);
			statement.setString(3,data);
			statement.setInt(4, richiedente);
			statement.setString(5, stato);
			statement.setString(6, documento);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: inserimento richiesta non riuscito");
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
				throw new DbException("Errore: inserimento richiesta non riuscito");
			}
		}
	}
	
	/**
	 * Metodo che elimina una richiesta dal db
	 * @param id l'intero che viene usato come id della richiesta
	 * @return True se è stato effettuato una cancellazione nel db, False altrimenti
	 * @throws DbException
	 */
	
	public boolean eliminaRichiesta(int id) throws DbException
	{	
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("DELETE FROM richiesta WHERE idRichiesta =?");
			statement.setInt(1,id);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: cancellazione richiesta non riuscita");
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
				throw new DbException("Errore: cancellazione richiesta  non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce una richiesta
	 * @param id l'intero che viene usato come id della richiesta
	 * @return Restituisce un oggetto di tipo Richiesta
	 * @throws DbException
	 */
	
	public Richiesta getRichiestaById(int id)throws DbException
	{
		PreparedStatement statement=null;
		ResultSet rs=null;
		Richiesta ret=null;
		try
		{
			statement=connection.prepareStatement("SELECT* FROM richiesta WHERE idRichiesta = ?");
			statement.setInt(1,id);
			rs= statement.executeQuery();
			if(!rs.next())
				return ret;
			int idR=rs.getInt("idRichiesta");
			String tipo=rs.getString("tipo");
			String data=rs.getString("data");
			int richiedente=rs.getInt("richiedente");
			String stato=rs.getString("stato");
			String documento=rs.getString("documento");
			ret=new Richiesta(idR,tipo,data,richiedente,stato,documento);
			return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca tramite l'id della richiesta non riuscita");
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
				throw new DbException("Errore: ricerca tramite l'id della richiesta non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un insieme di richieste
	 * @param idR l'intero che viene usato come id del richiedente
	 * @return Restituisce una Collection di Richieste
	 * @throws DbException
	 */
	
	public Collection<Richiesta> getRichiestaByRichiedente(int idR)throws DbException
	{
		ArrayList<Richiesta> ret = new ArrayList<Richiesta>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM richiesta WHERE richiedente = ? ORDER BY  data desc");
			statement.setInt(1,idR);
			rs= statement.executeQuery();
			while(rs.next())
			{
				int idRic=rs.getInt("idRichiesta");
				String tipo=rs.getString("tipo");
				String data=rs.getString("data");
				int richiedente=rs.getInt("richiedente");
				String statoR=rs.getString("stato");
				String documento=rs.getString("documento");
				ret.add(new Richiesta(idRic,tipo,data,richiedente,statoR,documento));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca richieste tramite l'id del richiedente non riuscita");
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
				throw new DbException("Errore: ricerca richieste tramite l'id del richiedente non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un insieme di richieste
	 * @param idR l'intero che viene usato come id del richiedente
	 * @param stato la stringa che viene usata come stato della richiesta
	 * @return Restituisce una Collection di tipo Richiesta
	 * @throws DbException
	 */
	
	public Collection<Richiesta> getRichiestaByStato(int idR, String stato) throws DbException
	{
		ArrayList<Richiesta> ret = new ArrayList<Richiesta>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM richiesta WHERE richiedente =? and stato =? ORDER BY  data desc");
			statement.setInt(1,idR);
			statement.setString(2,stato);
			rs= statement.executeQuery();
			while(rs.next())
			{
				int idRic=rs.getInt("idRichiesta");
				String tipo=rs.getString("tipo");
				String data=rs.getString("data");
				int richiedente=rs.getInt("richiedente");
				String statoR=rs.getString("stato");
				String documento=rs.getString("documento");
				ret.add(new Richiesta(idRic,tipo,data,richiedente,statoR,documento));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca richieste tramite l'id del richiedente e stato richiesta non riuscita");
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
				throw new DbException("Errore: ricerca tramite l'id del richiedente e stato richiesta non riuscita ");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un insieme di richieste
	 * @param idR l'intero che viene usato come id del richiedente
	 * @param tipo la stringa che viene usata come tipologia della richiesta
	 * @return Restituisce una Collection di tipo Richiesta
	 * @throws DbException
	 */
	
	public Collection<Richiesta> getRichiestaByTipo(int idR, String tipo) throws DbException
	{
		ArrayList<Richiesta> ret = new ArrayList<Richiesta>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM richiesta WHERE richiedente =? and tipo =? ORDER BY  data desc");
			statement.setInt(1,idR);
			statement.setString(2,tipo);
			rs= statement.executeQuery();
			while(rs.next())
			{
				int idRic=rs.getInt("idRichiesta");
				String tipoR=rs.getString("tipo");
				String data=rs.getString("data");
				int richiedente=rs.getInt("richiedente");
				String statoR=rs.getString("stato");
				String documento=rs.getString("documento");
				ret.add(new Richiesta(idRic,tipoR,data,richiedente,statoR,documento));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca richieste tramite l'id del richiedente e tipo richiesta non riuscita");
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
				throw new DbException("Errore: ricerca tramite l'id del richiedente e tipo richiesta non riuscita ");
			}
		}
	}
		
	/**
	 * Metodo che modifica lo stato di una richiesta
	 * @param idR l'intero che viene utilizzato come id della richiesta
	 * @param stato la stringa che viene utilizzato come stato della richiesta
	 * @return True se la modifica ha avuto successo, altrimenti False
	 * @throws SQLException
	 */
	
	public boolean setStatoRichiesta(int idR, String stato)throws DbException
	{
		PreparedStatement statement=null;
		int ret=0;
		try
		{
			statement=connection.prepareStatement("UPDATE richiesta SET stato = ? WHERE idRichiesta = ?");
			statement.setString(1,stato);
			statement.setInt(2,idR);
			ret= statement.executeUpdate();
			return (ret==1);
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: modifica stato richiesta non riuscita");
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
				throw new DbException("Errore: modifica stato richiesta non riuscita");
			}
		}
	}
	
	/**
	 * Metodo che restituisce un insieme di richieste
	 * @return Restituisce una Collection di Richieste
	 * @throws DbException
	 */
	
	public Collection<Richiesta> getRichieste() throws DbException
	{
		ArrayList<Richiesta> ret = new ArrayList<Richiesta>(); 
		PreparedStatement statement=null;
		ResultSet rs=null;
		try
		{
			statement=connection.prepareStatement("SELECT * FROM richiesta ORDER BY data desc");
			rs= statement.executeQuery();
			while(rs.next())
			{
				int idRic=rs.getInt("idRichiesta");
				String tipo=rs.getString("tipo");
				String data=rs.getString("data");
				int richiedente=rs.getInt("richiedente");
				String statoR=rs.getString("stato");
				String documento=rs.getString("documento");
				ret.add(new Richiesta(idRic,tipo,data,richiedente,statoR,documento));
			}
			if(ret.isEmpty())
				return null;
			else
				return ret;
		}
		catch(SQLException e)
		{
			throw new DbException("Errore: ricerca richieste non riuscita");
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
				throw new DbException("Errore: ricerca richieste tramite l'id del richiedente non riuscita");
			}
		}
	}
}