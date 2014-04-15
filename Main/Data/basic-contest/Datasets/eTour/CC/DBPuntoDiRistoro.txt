package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.sql.Time;
import java.util.ArrayList;
import java.util.Date;

import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.util.Punto3D;

/ **
  * Class that implements the interface of Refreshment
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class DBPuntoDiRistoro implements IDBPuntoDiRistoro
(
/ / Empty constructor
public DBPuntoDiRistoro ()
(

)

public boolean cancellaPuntoDiRistoro (int pIdPuntoDiRistoro)
throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query cancellation
String query = "DELETE FROM puntodiristoro WHERE Id ="
+ PIdPuntoDiRistoro;
/ / You run the query Cancellation
int i = stat.executeUpdate (query);
/ / This returns the backup
return (i == 1);
)
/ / Always runs and takes care of closing the Statement and the
/ / Connect
finally
(
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)

)

public boolean inserisciPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws SQLException
(
/ / Create the organization of the Opening and Closing
java.sql.Time orarioApertura = new Time (pPuntoDiRistoro
. getOrarioApertura (). getTime ());
java.sql.Time orarioChiusura = new Time (pPuntoDiRistoro
. getOrarioChiusura (). getTime ());
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for the insertion of a refreshment
String query = "INSERT INTO puntodiristoro (Name,"
+ "Description, Telephone, Latitude, Longitude,"
+ "Altitude, OrarioApertura, OrarioChiusura, GiornoChiusura"
+ "Town, city, street, postcode, Province, MediaVoti, NumeroVoti"
+ "PartitaIva) VALUES ( '" + pPuntoDiRistoro.getNome ()
PPuntoDiRistoro.getDescrizione + "','" + () + "','"
PPuntoDiRistoro.getTelefono + () + " ',"
PPuntoDiRistoro.getPosizione + (). GetLatitudine () + ""
PPuntoDiRistoro.getPosizione + (). GetLongitudine () + ""
PPuntoDiRistoro.getPosizione + (). GetAltitudine () + " '"
OrarioApertura.toString + () + "','"
OrarioChiusura.toString + () + "','"
PPuntoDiRistoro.getGiornoChiusura + () + "','"
PPuntoDiRistoro.getLocalita + () + "','"
PPuntoDiRistoro.getCitta + () + "','"
PPuntoDiRistoro.getVia + () + "','"
PPuntoDiRistoro.getCap + () + "','"
PPuntoDiRistoro.getProvincia + () + " ',"
PPuntoDiRistoro.getMediaVoti + () + ""
PPuntoDiRistoro.getNumeroVoti + () + " '"
PPuntoDiRistoro.getPartitaIva + () + "');";
/ / You run the insert query
int i = stat.executeUpdate (query);
return (i == 1);
)
/ / Is always done and takes care of closing the statement, and
/ / Connect
finally
(
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

public boolean modificaPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws SQLException
(
/ / Create the organization of the Opening and Closing
java.sql.Time orarioApertura = new Time (pPuntoDiRistoro
. getOrarioApertura (). getTime ());
java.sql.Time orarioChiusura = new Time (pPuntoDiRistoro
. getOrarioChiusura (). getTime ());
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for amendment
String query = "UPDATE puntodiristoro SET" + "Name = '"
PPuntoDiRistoro.getNome + () + " ', Description ='"
PPuntoDiRistoro.getDescrizione + () + " ', Phone ='"
PPuntoDiRistoro.getTelefono + () + " ', Latitude ="
PPuntoDiRistoro.getPosizione + (). GetLatitudine ()
+ "Longitude ="
PPuntoDiRistoro.getPosizione + (). GetLongitudine ()
+ ", Height ="
PPuntoDiRistoro.getPosizione + (). GetAltitudine ()
+ ", OrarioApertura = '" + orarioApertura.toString ()
+ " ', OrarioChiusura ='" + orarioChiusura.toString ()
+ " ', GiornoChiusura ='"
PPuntoDiRistoro.getGiornoChiusura + () + " ', Location ='"
PPuntoDiRistoro.getLocalita + () + " ', City ='"
PPuntoDiRistoro.getCitta + () + " ', Via ='"
PPuntoDiRistoro.getVia + () + " ', postcode ='"
PPuntoDiRistoro.getCap + () + " ', State ='"
PPuntoDiRistoro.getProvincia + () + " ', MediaVoti ="
PPuntoDiRistoro.getMediaVoti + () + ", NumeroVoti ="
PPuntoDiRistoro.getNumeroVoti + () + ", party = '"
PPuntoDiRistoro.getPartitaIva + () + " 'WHERE Id ="
+ PPuntoDiRistoro.getId ();
/ / You run the query for Change
int i = stat.executeUpdate (query);
/ / This returns the backup
return (i == 1);
)
/ / Is always done and takes care of closing the Statement and the
/ / Connect
finally
(
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

public int ottieniNumeroElementiRicerca (String pKeyword,
ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query to get the number of pages
String query = "";
if (pTags.size () == 0)
(
/ / Query without the control tag
query = "SELECT count (number) FROM (SELECT count (puntodiristoro.Id) AS number"
+ "FROM puntodiristoro"
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ '%') "
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
+ PPosizione.getLatitudine ()
+ ""
+ PPosizione.getLongitudine ()
+ ") <"
+ PDistanzaMassima
+ "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC) AS table";
)
else
(
/ / Query with tags
query = "SELECT count (number) FROM (SELECT count (puntodiristoro.Id) AS number"
+ "FROM puntodiristoro"
+ "JOIN (appartenenzapr JOIN tag ON IdTag = Id)"
+ "ON puntodiristoro.Id = appartenenzapr.IdPuntoDiRistoro"
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ '%') AND (tag.Nome = ' "+ pTags.get (0). GetName ();
if (pTags.size ()> = 2)
(
query = query + " 'OR tag.Nome ='"
PTags.get + (1). GetName ();
)
if (pTags.size ()> = 3)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (2). GetName ();
)
if (pTags.size ()> = 4)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (3). GetName ();
)
if (pTags.size ()> = 5)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (4). GetName ();
)
query = query
+ " ')"
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC) AS table";

)
/ / You run the query
result = stat.executeQuery (query);
/ / It returns the value of count () that contains the number of
/ / Tuple
int i = 0;
if (result.next ())
(
i = result.getInt (1);
)
return i;
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

public int ottieniNumeroElementiRicercaAvanzata (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query to get the number of pages of advanced
String query = "";
if (pTags.size () == 0)
(
/ / Query without the control tag
query = "SELECT count (number) FROM (SELECT count (puntodiristoro.Id) AS number"
+ "FROM (puntodiristoro LEFT JOIN"
+ "(SELECT IdPuntoDiRistoro"
+ "FROM associazionepr, ("
+ "SELECT IdPreferenzeDiRicerca"
+ "FROM rating"
+ "WHERE IdTurista ="
+ PIdTurista
+ ") AS pref"
+ "WHERE associazionepr.IdPreferenzeDiRicerca = pref.IdPreferenzeDiRicerca)"
+ Preferences AS ON puntodiristoro.Id preferenze.IdPuntoDiRistoro =) "
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ '%') "
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
+ PPosizione.getLatitudine ()
+ ""
+ PPosizione.getLongitudine ()
+ ") <"
+ PDistanzaMassima
+ "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC) AS table";
)
else
(
/ / Query with tags
query = "SELECT count (number) FROM (SELECT count (puntodiristoro.Id) AS number"
+ "FROM (puntodiristoro LEFT JOIN"
+ "(SELECT IdPuntoDiRistoro"
+ "FROM associazionepr, ("
+ "SELECT IdPreferenzeDiRicerca"
+ "FROM rating"
+ "WHERE IdTurista ="
+ PIdTurista
+ ") AS pref"
+ "WHERE associazionepr.IdPreferenzeDiRicerca = pref.IdPreferenzeDiRicerca)"
+ Preferences AS ON puntodiristoro.Id preferenze.IdPuntoDiRistoro =) "
+ "JOIN (appartenenzapr JOIN tag ON IdTag = Id)"
+ "ON puntodiristoro.Id = appartenenzapr.IdPuntoDiRistoro"
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ '%') AND (tag.Nome = ' "+ pTags.get (0). GetName ();
if (pTags.size ()> = 2)
(
query = query + " 'OR tag.Nome ='"
PTags.get + (1). GetName ();
)
if (pTags.size ()> = 3)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (2). GetName ();
)
if (pTags.size ()> = 4)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (3). GetName ();
)
if (pTags.size ()> = 5)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (4). GetName ();
)
query = query
+ " ')"
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC) AS table";
)
/ / You run the query
result = stat.executeQuery (query);
/ / It returns the value of count () that contains the number of
/ / Tuple
int i = 0;
if (result.next ())
(
i = result.getInt (1);
)
return i;
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

public BeanPuntoDiRistoro ottieniPuntoDiRistoro (int pid)
throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query
String query = "SELECT * FROM puntodiristoro WHERE Id =" + pid;
/ / You run the query
result = stat.executeQuery (query);
if (result.next ())
(
/ / We create the objects to be returned
Punto3D point = new Punto3D (result.getDouble ( "Latitude")
result.getDouble ( "Longitude"), result
. getDouble ( "Elevation"));
Date orarioApertura = new Date (result.getTime (OrarioApertura ")
. getTime ());
Date orarioChiusura = new Date (result.getTime (OrarioChiusura ")
. getTime ());
/ / This generates the resting spot of bean
BeanPuntoDiRistoro puntoTemp = new BeanPuntoDiRistoro (result
. getInt ( "Id"), result.getInt ( "NumeroVoti"), result
. getDouble ( "MediaVoti"), result.getString ( "Name")
result.getString ( "Description"), result
. getString ( "Phone"), result
. getString ( "Location"), result
. getString ( "City"), result.getString ( "Way"),
result.getString ( "Cap"), result.getString ( "Province"),
result.getString ( "PartitaIva"), point, orarioApertura,
orarioChiusura, result.getString ( "GiornoChiusura"));
/ / It returns the refreshment
puntoTemp return;
)
else
(
return null;
)
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

public ArrayList <BeanPuntoDiRistoro> search (String pKeyword,
ArrayList <BeanTag> pTags, int pNumeroPagina,
int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for search
String query = "";
if (pTags.size () == 0)
(
/ / Query without the control tag
query = "SELECT *"
+ "FROM puntodiristoro"
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ '%') "
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC LIMIT"
+ (PNumeroPagina * pNumeroElementiPerPagina) + ""
+ PNumeroElementiPerPagina;
)
else
(
/ / Query with tags
query = "SELECT *"
+ "FROM puntodiristoro"
+ "JOIN (appartenenzapr JOIN tag ON IdTag = Id)"
+ "ON puntodiristoro.Id = appartenenzapr.IdPuntoDiRistoro"
+ "WHERE (puntodiristoro.Nome LIKE '%" + pKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%" + pKeyword
+ '%') AND (tag.Nome = ' "+ pTags.get (0). GetName ();
if (pTags.size ()> = 2)
(
query = query + " 'OR tag.Nome ='"
PTags.get + (1). GetName ();
)
if (pTags.size ()> = 3)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (2). GetName ();
)
if (pTags.size ()> = 4)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (3). GetName ();
)
if (pTags.size ()> = 5)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (4). GetName ();
)
query = query
+ " ')"
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC LIMIT"
+ (PNumeroPagina * pNumeroElementiPerPagina) + ""
+ PNumeroElementiPerPagina;
)
/ / You run the query
result = stat.executeQuery (query);
/ / List that will contain the BeanPuntoDiRistoro
<BeanPuntoDiRistoro> ArrayList list = new ArrayList <BeanPuntoDiRistoro> ();
while (result.next ())
(
/ / We create the objects to be returned
Punto3D point = new Punto3D (result.getDouble ( "Latitude")
result.getDouble ( "Longitude"), result
. getDouble ( "Elevation"));
Date orarioApertura = new Date (result.getTime (OrarioApertura ")
. getTime ());
Date orarioChiusura = new Date (result.getTime (OrarioChiusura ")
. getTime ());
/ / Build the refreshment
BeanPuntoDiRistoro puntoTemp = new BeanPuntoDiRistoro (result
. getInt ( "Id"), result.getInt ( "NumeroVoti"), result
. getDouble ( "MediaVoti"), result.getString ( "Name")
result.getString ( "Description"), result
. getString ( "Phone"), result
. getString ( "Location"), result
. getString ( "City"), result.getString ( "Way"),
result.getString ( "Cap"), result.getString ( "Province"),
result.getString ( "PartitaIva"), point, orarioApertura,
orarioChiusura, result.getString ( "GiornoChiusura"));
/ / Insert the bean in the list
lista.add (puntoTemp);
)
/ / It returns the list
return list;
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

<BeanPuntoDiRistoro> ricercaAvanzata public ArrayList (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, int pNumeroPagina,
int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for advanced search
String query = "";
if (pTags.size () == 0)
(
/ / Query without tags
query = "SELECT *"
+ "FROM (puntodiristoro LEFT JOIN"
+ "(SELECT IdPuntoDiRistoro"
+ "FROM associazionepr, ("
+ "SELECT IdPreferenzeDiRicerca"
+ "FROM rating"
+ "WHERE IdTurista ="
+ PIdTurista
+ ") CI pref"
+ "WHERE associazionepr.IdPreferenzeDiRicerca = pref.IdPreferenzeDiRicerca)"
+ Preferences AS ON puntodiristoro.Id preferenze.IdPuntoDiRistoro =) "
+ "WHERE (puntodiristoro.Nome LIKE '%"
+ PKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%"
+ PKeyword
+ "%')"
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC LIMIT"
+ (PNumeroPagina * pNumeroElementiPerPagina) + ""
+ PNumeroElementiPerPagina;
)
else
(
/ / Query with tags
query = "SELECT *"
+ "FROM (puntodiristoro LEFT JOIN"
+ "(SELECT IdPuntoDiRistoro"
+ "FROM associazionepr, ("
+ "SELECT IdPreferenzeDiRicerca"
+ "FROM rating"
+ "WHERE IdTurista ="
+ PIdTurista
+ ") AS pref"
+ "WHERE associazionepr.IdPreferenzeDiRicerca = pref.IdPreferenzeDiRicerca)"
+ Preferences AS ON puntodiristoro.Id preferenze.IdPuntoDiRistoro =) "
+ "JOIN (appartenenzapr JOIN tag ON IdTag = Id)"
+ "ON puntodiristoro.Id = appartenenzapr.IdPuntoDiRistoro"
+ "WHERE (puntodiristoro.Nome LIKE '%" + pKeyword
+ "% 'OR puntodiristoro.Descrizione LIKE'%" + pKeyword
+ '%') AND (tag.Nome = ' "+ pTags.get (0). GetName ();
if (pTags.size ()> = 2)
(
query = query + " 'OR tag.Nome ='"
PTags.get + (1). GetName ();
)
if (pTags.size ()> = 3)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (2). GetName ();
)
if (pTags.size ()> = 4)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (3). GetName ();
)
if (pTags.size ()> = 5)
(
query = query + " 'OR tag.Nome ='"
+ PTags.get (4). GetName ();
)
query = query
+ " ')"
+ "AND distance (puntodiristoro.Latitudine, puntodiristoro.Longitudine"
PPosizione.getLatitudine + () + ""
PPosizione.getLongitudine + () + ") <"
PDistanzaMassima + + "GROUP BY puntodiristoro.Id"
+ "ORDER BY count (puntodiristoro.Id) DESC LIMIT"
+ (PNumeroPagina * pNumeroElementiPerPagina) + ""
+ PNumeroElementiPerPagina;
)
/ / You run the query
result = stat.executeQuery (query);

<BeanPuntoDiRistoro> ArrayList list = new ArrayList <BeanPuntoDiRistoro> ();
while (result.next ())
(
/ / We create the objects to be returned
Punto3D point = new Punto3D (result.getDouble ( "Latitude")
result.getDouble ( "Longitude"), result
. getDouble ( "Elevation"));
Date orarioApertura = new Date (result.getTime (OrarioApertura ")
. getTime ());
Date orarioChiusura = new Date (result.getTime (OrarioChiusura ")
. getTime ());
/ / This creates the cultural
BeanPuntoDiRistoro beneTemp = new BeanPuntoDiRistoro (result
. getInt ( "Id"), result.getInt ( "NumeroVoti"), result
. getDouble ( "MediaVoti"), result.getString ( "Name")
result.getString ( "Description"), result
. getString ( "Phone"), result
. getString ( "Location"), result
. getString ( "City"), result.getString ( "Way"),
result.getString ( "Cap"), result.getString ( "Province"),
result.getString ( "PartitaIva"), point, orarioApertura,
orarioChiusura, result.getString ( "GiornoChiusura"));
/ / Insert the bean in the list
lista.add (beneTemp);
)
/ / It returns the list
return list;
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)

<BeanPuntoDiRistoro> ottieniListaPR public ArrayList () throws SQLException
(
/ / Variables for database connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for a list of all the refreshment
String query = "SELECT * FROM puntodiristoro";
/ / You run the query
result = stat.executeQuery (query);
/ / List that will contain the BeanPuntoDiRistoro
<BeanPuntoDiRistoro> ArrayList list = new ArrayList <BeanPuntoDiRistoro> ();
while (result.next ())
(
/ / We create the objects to be returned
Punto3D point = new Punto3D (result.getDouble ( "Latitude")
result.getDouble ( "Longitude"), result
. getDouble ( "Elevation"));
Date orarioApertura = new Date (result.getTime (OrarioApertura ")
. getTime ());
Date orarioChiusura = new Date (result.getTime (OrarioChiusura ")
. getTime ());
/ / Build the refreshment
BeanPuntoDiRistoro puntoTemp = new BeanPuntoDiRistoro (result
. getInt ( "Id"), result.getInt ( "NumeroVoti"), result
. getDouble ( "MediaVoti"), result.getString ( "Name")
result.getString ( "Description"), result
. getString ( "Phone"), result
. getString ( "Location"), result
. getString ( "City"), result.getString ( "Way"),
result.getString ( "Cap"), result.getString ( "Province"),
result.getString ( "PartitaIva"), point, orarioApertura,
orarioChiusura, result.getString ( "GiornoChiusura"));
/ / Insert the bean in the list
lista.add (puntoTemp);
)
/ / It returns the list
return list;
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
finally
(
if (result! = null)
(
result.close ();
)
if (stat = null)
(
stat.close ();
)
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)

)

)
