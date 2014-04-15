	
package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;

/ **
  * Class that implements the local statistical
  *
  * @ Author Joseph Martone
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class DBStatisticheLocalita implements IDBStatisticheLocalita
(
/ / Empty constructor
public DBStatisticheLocalita ()
(

)

<String> ottieniListaLocalita public ArrayList () throws SQLException
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
/ / Query for the extraction of location between the PR and BC
String query = "(SELECT DISTINCT Locations"
+ "FROM beneculturale) UNION"
+ "(SELECT DISTINCT FROM Localita puntodiristoro)";
/ / The query is executed
result = stat.executeQuery (query);
/ / We extract the results from the result set and moves in
/ / List
/ / To be returned
/ / List that includes the results obtained
ArrayList list = new ArrayList <String> <String> ();
while (result.next ())
(
/ / Add to the list the locations obtained
lista.add (result.getString ( "Location"));
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

public double ottieniMedieVotiLocalita (String plocalita)
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
/ / Query to extract the average of the votes of catering outlets
/ / According to a Passo
String query = "SELECT avg (MediaVoti) as MediaVoti FROM puntodiristoro WHERE Location = '"
Plocalita + + " '";
/ / The query is executed
result = stat.executeQuery (query);
/ / We extract the results from the result set
double point = 0.0;
if (result.next ())
(
point = result.getDouble ( "MediaVoti");
)
/ / Query to extract the average of the votes of cultural
/ / According to a Passo
query = "SELECT avg (MediaVoti) AS MediaVoti FROM beneculturale WHERE Location = '"
Plocalita + + " '";
/ / The query is executed
result = stat.executeQuery (query);
/ / We extract the results from the result set
double good = 0.0;
if (result.next ())
(
well result.getDouble = ( "MediaVoti");
)
/ / It returns the average of the refreshment and heritage
return (good point +) / 2;

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