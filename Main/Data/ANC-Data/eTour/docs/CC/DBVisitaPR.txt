package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.Date;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Class that implements the interface IDBVisitaPR
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class DBVisitaPR implements IDBVisitaPR
(
/ / Empty constructor
public DBVisitaPR ()
(

)

public boolean inserisciVisitaPR (BeanVisitaPR PVIS) throws SQLException
(
/ / Variable for the connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
/ / Variable for the query results
ResultSet result = null;
TRY
(
/ / Create the date of visit
java.sql.Date dataVisita = new Date (pVisita.getDataVisita ()
. getTime ());
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query to get the average rating of a property
QueryMediaVoti String = "SELECT MediaVoti, NumeroVoti FROM puntodiristoro WHERE Id ="
+ PVisita.getIdPuntoDiRistoro ();
result = stat.executeQuery (queryMediaVoti);
/ / Variable for the average rating
double average = 0;
/ / Variable for the number of votes
numeroVoti int = 0;
if (result.next ())
(
average = result.getDouble ( "MediaVoti");
numeroVoti = result.getInt (NumeroVoti ");
average = ((average * numeroVoti) + pVisita.getVoto ())
/ + + NumeroVoti;
)
/ / Query for the insertion
String query = "INSERT INTO visitapr (IdTurista, IdPuntoDiRistoro, DataVisita, Vote, Comment) VALUES ("
+ PVisita.getIdTurista ()
+ ""
+ PVisita.getIdPuntoDiRistoro ()
+ " '"
+ DataVisita
+ " ',"
PVisita.getVoto + () + " '" + pVisita.getCommento () + "')";
String query2 = "UPDATE puntodiristoro September MediaVoti =" + media
+ ", NumeroVoti =" + numeroVoti + "WHERE Id ="
+ PVisita.getIdPuntoDiRistoro ();
/ / You run the insert query
stat.executeQuery ( "BEGIN");
int i = stat.executeUpdate (query);
i = i * stat.executeUpdate (query2);
stat.executeQuery ( "COMMIT");
/ / This returns the backup
return (i == 1);
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

public boolean modificaVisitaPR (BeanVisitaPR PVIS) throws SQLException
(
/ / Variable for the connection
Connection conn = null;
/ / Variable for the query
Statement stat = null;
TRY
(
/ / Create the date of visit
java.sql.Date dataVisita = new Date (pVisita.getDataVisita ()
. getTime ());
/ / Get the connection
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the Statement
stat = conn.createStatement ();
/ / Query for amendment
String query = "UPDATE visitapr SET" + "DataVisita = '"
DataVisita + + " ', Comment ='" + pVisita.getCommento ()
+ " 'WHERE IdPuntoDiRistoro ="
PVisita.getIdPuntoDiRistoro + () + "AND IdTurista ="
+ PVisita.getIdTurista ();
/ / You run the query for Change
int i = stat.executeUpdate (query);
/ / This returns the backup
return (i == 1);
)
/ / Always runs and takes care to close the Result, the Statement
/ / And Connection
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

<BeanVisitaPR> ottieniListaVisitaPR public ArrayList (int pIdPuntoDiRistoro)
throws SQLException
(
/ / Variable for the connection
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
/ / Query to extract the list of requests for a
/ / Refreshment
String query = "SELECT * FROM visitapr WHERE IdPuntoDiRistoro ="
+ PIdPuntoDiRistoro;
/ / The query is executed
result = stat.executeQuery (query);
<BeanVisitaPR> ArrayList list = new ArrayList <BeanVisitaPR> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
java.util.Date dataVisita = new java.util.Date (result.getDate (
"DataVisita"). GetTime ());
lista.add (new BeanVisitaPR (result.getInt ( "Customer"), result
. getInt ( "IdPuntoDiRistoro"), result
. getString ( "Comment"), result.getInt ( "IdTurista"),
dataVisita));
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

<BeanVisitaPR> ottieniListaVisitaPRTurista public ArrayList (int pIdTurista)
throws SQLException
(
/ / Variable for the connection
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
/ / Query to extract the list of requests for a
/ / Eating place for tourists
String query = "SELECT * FROM visitapr WHERE IdTurista ="
+ PIdTurista;
/ / The query is executed
result = stat.executeQuery (query);
/ / List that will contain the BeanVisitaPR
<BeanVisitaPR> ArrayList list = new ArrayList <BeanVisitaPR> ();
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
while (result.next ())
(
/ / Add to the list BeanVisitaPR
java.util.Date dataVisita = new java.util.Date (result.getDate (
"DataVisita"). GetTime ());
lista.add (new BeanVisitaPR (result.getInt ( "Customer"), result
. getInt ( "IdPuntoDiRistoro"), result
. getString ( "Comment"), result.getInt ( "IdTurista"),
dataVisita));
)
/ / Return the list
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

public BeanVisitaPR ottieniVisitaPR (pIdPuntoDiRistoro int, int pIdTurista)
throws SQLException
(
/ / Variable for the connection
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
/ / Query for the extraction of the visit made by a tourist to
/ / A given point of comfort
String query = "SELECT * FROM visitapr WHERE IdPuntoDiRistoro ="
PIdPuntoDiRistoro + + "AND IdTurista =" + pIdTurista;
/ / The query is executed
result = stat.executeQuery (query);
/ / Get the bean's visit sought based on the ID of the tourist and
/ / Of refreshment
BeanVisitaPR beanTemp = null;
if (result.next ())
(
/ / Create the BeanVisitaPR
java.util.Date dataVisita = new java.util.Date (result.getDate (
"DataVisita"). GetTime ());
beanTemp = new BeanVisitaPR (result.getInt ( "Customer"), result
. getInt ( "IdPuntoDiRistoro"), result
. getString ( "Comment"), result.getInt ( "IdTurista"),
dataVisita);
)
/ / Return the BeanTemp
beanTemp return;
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
