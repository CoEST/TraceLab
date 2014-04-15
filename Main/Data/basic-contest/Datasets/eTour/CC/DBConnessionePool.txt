package unisa.gps.etour.repository;

import Java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

/ **
  * Class that creates the connection to the database using JDBC and
  * Allows you to query both read and edit the contents of
  * Database. E 'implemented to provide a pool of connections to
  * Provide a connection to each thread.
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class DBConnessionePool
(
private final static String driver = "com.mysql.jdbc.Driver";
private final static String urlConnessione = "jdbc: mysql: / / localhost / eTour? user = & password = mauro mauro";
private static List <Connection> connessioniLibere;

/ * Private constructor that initiates the connection to the database * /

/ *
* Static initialization block is used to load the driver
* Memory
* /
static
(
connessioniLibere = <Connection> new ArrayList ();
TRY
(
Class.forName (driver);
)
catch (ClassNotFoundException e)
(
e.printStackTrace ();
)
)

/ **
* Method to get the connection to the server.
*
* @ Return Returns the database connection
* @ Throws SQLException
* /
public static synchronized Connection ottieniConnessione ()
throws SQLException
(
Connection connection;

if (! connessioniLibere.isEmpty ())
(
/ / Extract a connection from the free db connection queue
connection = connessioniLibere.get (0);
DBConnessionePool.connessioniLibere.remove (0);

TRY
(
/ / If the connection is not valid, a new connection will be
/ / Analyzed
if (connection.isClosed ())
DBConnessionePool.ottieniConnessione connection = ();
)
catch (SQLException e)
(
DBConnessionePool.ottieniConnessione connection = ();
)
)
else
/ / The free db connection queue is empty, so a new connection will
/ / Be created
DBConnessionePool.creaDBConnessione connection = ();

return connection;
)

public static void rilasciaConnessione (Connection pReleasedConnection)
(
/ / Add the connection to the free db connection queue
DBConnessionePool.connessioniLibere.add (pReleasedConnection);
)

private static Connection creaDBConnessione () throws SQLException
(
Connection nuovaConnessione = null;
/ / Create a new db connection using the db properties
nuovaConnessione = DriverManager.getConnection (urlConnessione);
nuovaConnessione.setAutoCommit (true);
nuovaConnessione return;
)
) 