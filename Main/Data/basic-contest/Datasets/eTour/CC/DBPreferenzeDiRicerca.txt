package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanPreferenzaDiRicerca;

/ **
  * Class that implements the interface PreferenzeDiRicerca
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class DBPreferenzeDiRicerca implements IDBPreferenzeDiRicerca
(
/ / Empty constructor
public DBPreferenzeDiRicerca ()
(
)

public boolean cancellaPreferenzaDiRicerca (int pIdPreferenza)
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
String query = "DELETE FROM preferenzediricerca WHERE Id ="
+ PIdPreferenza;
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

public boolean cancellaPreferenzaDiRicercaBC (int pIdBeneCulturale,
pIdPreferenzaDiRicerca int) throws SQLException
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
String query = "DELETE FROM associazionebc WHERE IdPreferenzeDiRicerca ="
+ PIdPreferenzaDiRicerca
+ "AND IdBeneCulturale ="
+ PIdBeneCulturale;
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

public boolean cancellaPreferenzaDiRicercaPR (int pIdPuntoDiRistoro,
pIdPreferenza int) throws SQLException
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
String query = "DELETE FROM associazionepr WHERE IdPreferenzeDiRicerca ="
+ PIdPreferenza
+ "AND IdPuntoDiRistoro ="
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

public boolean cancellaPreferenzaDiRicercaTurista (int pIdTurista,
pIdPreferenza int) throws SQLException
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
String query = "DELETE FROM rating WHERE IdTurista ="
PIdTurista + + "AND IdPreferenzeDiRicerca ="
+ PIdPreferenza;
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

public boolean inserisciPreferenzaDiRicercaDelBC (int pIdBeneCulturale,
pIdPreferenzaDiRicerca int) throws SQLException
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
/ / Query for the insertion
String query = "INSERT INTO associazionebc (IdPreferenzeDiRicerca, IdBeneCulturale) VALUES ("
PIdPreferenzaDiRicerca + + "," + pIdBeneCulturale + ")";
/ / You run the insert query
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

public boolean inserisciPreferenzaDiRicerca (
BeanPreferenzaDiRicerca pPreferenza) throws SQLException
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
/ / Query for the insertion
String query = "INSERT INTO preferenzediricerca (Id, Name) VALUES ("
PPreferenza.getId + () + " '" + pPreferenza.getNome ()
+ "')";
/ / You run the insert query
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

public boolean inserisciPreferenzaDiRicercaDelPR (int pIdPuntoDiRistoro,
pIdPreferenzaDiRicerca int) throws SQLException
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
/ / Query for the insertion
String query = "INSERT INTO associazionepr (IdPreferenzeDiRicerca, IdPuntoDiRistoro) VALUES ("
PIdPreferenzaDiRicerca + + "," + pIdPuntoDiRistoro + ")";
/ / You run the insert query
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

public boolean inserisciPreferenzaDiRicercaDelTurista (int pIdTurista,
pIdPreferenzaDiRicerca int) throws SQLException
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
/ / Query for the insertion
String query = "INSERT INTO rating (IdTurista, IdPreferenzeDiRicerca) VALUES ("
PIdTurista + + "," + pIdPreferenzaDiRicerca + ")";
/ / You run the insert query
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

public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelBC (
pIdBeneCulturale int) throws SQLException
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
/ / Query to extract the list of search preferences
/ / A cultural
String query = "SELECT * FROM associazionebc, preferenzediricerca WHERE IdBeneCulturale ="
+ PIdBeneCulturale
+ "AND IdPreferenzeDiRicerca preferenzediricerca.Id =";
/ / The query is executed
result = stat.executeQuery (query);
/ / List that will contain the BeanPreferenzaDiRicerca
<BeanPreferenzaDiRicerca> ArrayList list = new ArrayList <BeanPreferenzaDiRicerca> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
/ / Add to the list BeanPreferenzaDiRicerca
lista.add (new BeanPreferenzaDiRicerca (result.getInt ( "Id")
result.getString ( "Name ")));
)
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

public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelPR (
pIdPuntoDiRistoro int) throws SQLException
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
/ / Query to extract the list of search preferences
/ / A refreshment
String query = "SELECT * FROM associazionepr, preferenzediricerca WHERE IdPuntoDiRistoro ="
+ PIdPuntoDiRistoro
+ "AND IdPreferenzeDiRicerca preferenzediricerca.Id =";
/ / The query is executed
result = stat.executeQuery (query);
/ / List that will contain the BeanPreferenzaDiRicerca
<BeanPreferenzaDiRicerca> ArrayList list = new ArrayList <BeanPreferenzaDiRicerca> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
/ / Add to the list BeanPreferenzaDiRicerca
lista.add (new BeanPreferenzaDiRicerca (result.getInt ( "Id")
result.getString ( "Name ")));
)
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

public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelTurista (
pIdTurista int) throws SQLException
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
/ / Query to extract the list of search preferences
/ / A Turista
String query = "SELECT * FROM liking preferenzediricerca WHERE IdTurista ="
+ PIdTurista
+ "AND IdPreferenzeDiRicerca preferenzediricerca.Id =";
/ / The query is executed
result = stat.executeQuery (query);
/ / List that will contain the BeanPreferenzaDiRicerca
<BeanPreferenzaDiRicerca> ArrayList list = new ArrayList <BeanPreferenzaDiRicerca> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
/ / Add to the list BeanPreferenzaDiRicerca
lista.add (new BeanPreferenzaDiRicerca (result.getInt ( "Id")
result.getString ( "Name ")));
)
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

<BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicerca public ArrayList ()
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
/ / Query to extract the list of search preferences
/ / A Turista
String query = "SELECT * FROM preferenzediricerca";
/ / The query is executed
result = stat.executeQuery (query);
/ / List that will contain the BeanPreferenzaDiRicerca
<BeanPreferenzaDiRicerca> ArrayList list = new ArrayList <BeanPreferenzaDiRicerca> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
/ / Add to the list BeanPreferenzaDiRicerca
lista.add (new BeanPreferenzaDiRicerca (result.getInt ( "Id")
result.getString ( "Name ")));
)
/ / Return the list of search preferences in the DB
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