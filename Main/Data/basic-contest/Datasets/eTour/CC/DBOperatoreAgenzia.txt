/ **
  * Class that implements the Agency's Operator
  *
  * @ Author Joseph Martone
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import unisa.gps.etour.bean.BeanOperatoreAgenzia;

public class DBOperatoreAgenzia implements IDBOperatoreAgenzia
(

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBOperatoreAgenzia # ottieniOperatoreAgenzia (int)
* /
public BeanOperatoreAgenzia ottieniOperatoreAgenzia (String pUsername)
throws SQLException
(
/ / Connect to database
Connection conn = null;
/ / Statement for running queries
Statement stat = null;
/ / Resut set where the output of the query is inserted
ResultSet result = null;
/ / Try block which performs the query and the database connection
TRY
(
/ / You get the database connection from the pool
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the statement
stat = conn.createStatement ();
/ / Query
String query = "SELECT * FROM operatoreagenzia WHERE Username = '"
PUsername + + " '";
result = stat.executeQuery (query);
Or BeanOperatoreAgenzia = null;
if (result.next ())
(
/ / Build the bean when the query returns a
/ / Value
/ / Otherwise will return null
or BeanOperatoreAgenzia = new ();
oa.setId (result.getInt ( "Id"));
oa.setUsername (result.getString ( "Username"));
oa.setNome (result.getString ( "Name"));
oa.setCognome (result.getString ( "Name"));
oa.setPassword (result.getString ( "Password"));
)
or return;
)
/ / Finally block that contains the instructions to close the connections
/ / Hyenas run in any case
finally
(
/ / This closes the result set only if and 'the query was made
if (result! = null)
(
result.close ();
)
/ / This closes the if statement and 'opened
if (stat = null)
(
stat.close ();
)
/ / It returns the connection to the pool if and 'opened
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)

)

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBOperatoreAgenzia # modificaPassword (java.lang.String)
* /
public boolean modificaPassword (BeanOperatoreAgenzia poa) throws SQLException
(
/ / Connect to database
Connection conn = null;
/ / Statement for running queries
Statement stat = null;
/ / Try block which performs the query and the database connection
TRY
(
/ / You get the database connection from the pool
DBConnessionePool.ottieniConnessione conn = ();
/ / Create the statement
stat = conn.createStatement ();

/ / Query
String query = "UPDATE operatoreagenzia SET" + "Password = '"
POa.getPassword + () + " 'WHERE Id =" + pOa.getId ();
/ / You run the query
int i = stat.executeUpdate (query);

return (i == 1);
)
/ / Finally block that contains the instructions to close the connections
/ / Hyenas run in any case
finally
(
/ / This closes the if statement and 'opened
if (stat = null)
(
stat.close ();
)
/ / It returns the connection to the pool if and 'opened
if (conn! = null)
(
DBConnessionePool.rilasciaConnessione (conn);
)
)
)
)
