package DB;
import Bean.*;
import java.sql.Connection;
import java.sql.SQLException;

import Bean.CartaIdentita;

/**
 * Classe che si occupa di gestire le connessioni con il database
 *  e di schermare le servet con il DBMS.
 * @author Michelangelo Cianciulli
 *
 */
public class DbCambioResidenza 
{
	private Connection connection;
	
	public DbCambioResidenza() throws DbException
	{	
		try
		{
			connection=DbConnection.getConnection();
		}
		catch(SQLException exc)
		{
			throw new DbException("Errore : connessione non riuscita");
		}
	}
	
	/**
	 * Metodo che permette la cancellazione della carta d'identit‡† del cittadino nel 
	 * momento in cui esso cambia residenza verso un comune esterno (aggiornamento del db)
	 * @param cod Ë il numero della carta d'identit√† di chi ha chiesto il cambio di residenza verso un comune esterno
	 * @return true se l'operazione Ë eseguita con successo
	 * @throws DbException
	 */
	public boolean changeResidence (String cod) throws DbException
	{		
		return new DbCartaIdentita().cancellaCartaIdentita(cod);
	}
	
	/**
	 * Metodo che permette l'aggiornamento della residenza salvata nella carta d'identit√† del cittadino che ha effettuato un cambio
	 * di residenza nello stesso comune in cui attualmente risiede.(aggiornamento del db)
	 * @param cod Ë il numero della carta d'identit√†
	 * @param v Ë la nuova via in cui andr√† a risiedere il cittadino
	 * @param nc Ë il nuovo numero civico dell'abitazione del cittadino
	 * @return l'oggetto CartaIdentit‡† aggiornato 
	 * @throws DbException
	 */

	public CartaIdentita changeResidenceIn (String cod, String v,int nc) throws DbException
	{
		if (new DbCartaIdentita().modificaResidenzaCartaIdentita(cod, v, nc))
			return new DbCartaIdentita().ricercaCartaIdentitaByNumero(cod);
		else
			return null;
	}
}
