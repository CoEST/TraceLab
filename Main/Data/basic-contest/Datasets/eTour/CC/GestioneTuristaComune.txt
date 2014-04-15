package unisa.gps.etour.control.GestioneUtentiRegistrati;

import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.sql.SQLException;

import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.repository.DBTurista;
import unisa.gps.etour.repository.IDBTurista;
import unisa.gps.etour.util.MessaggiErrore;

/ **
  * Class that implements the common tasks for Operators and Tourist Agency
  * Ie modificaTurista and ottieniTurista
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
GestioneTuristaComune UnicastRemoteObject public class extends implements
IGestioneTuristaComune
(

protected IDBTurista tourist;

/ / Constructor that richama turn the class constructor
/ / UnicastRemoteObject to connect via RMI
/ / Instantiate and connect to the database
public GestioneTuristaComune () throws RemoteException
(
super ();
/ / Connect to the Database
TRY
(
Tourists DBTurista = new ();
)
/ / Exception in the database connection
catch (Exception e)
(
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ / Method that allows you to change the data of a tourist through its
/ / Data
public boolean modificaTurista (BeanTurista pProfiloTurista)
throws RemoteException
(
/ / Check the validity of past data
if ((pProfiloTurista == null)
| | (! (PProfiloTurista instanceof BeanTurista)))
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Execution of the amendment
TRY
(
/ / If the changes were made returns true
if (turista.modificaTurista (pProfiloTurista))
return true;
)
/ / Exception in operations on database
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method modificaTurista"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method modificaTurista"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If there were no exceptions but the changes are not
/ / Returns false were made
return false;
)

/ / Method to obtain the bean with data from the Tourist
/ / Identified by
/ / The parameter passed
public BeanTurista ottieniTurista (int pIdTurista) throws RemoteException
(
/ / Check the validity identifier
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
BeanTurista toReturn = null, / / variable return
/ / Retrieve data
TRY
(
/ / Are requested to return the bean layer on the tourist
/ / With id equal to pIdTurista
toReturn = turista.ottieniTurista (pIdTurista);
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Exception in database operations
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method ottieniTurista"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniTurista"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Return the result
toReturn return;
)
) 