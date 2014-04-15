package unisa.gps.etour.control.GestioneUtentiRegistrati;

import java.rmi.RemoteException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Iterator;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;
import unisa.gps.etour.repository.DBVisitaBC;
import unisa.gps.etour.repository.DBVisitaPR;
import unisa.gps.etour.repository.IDBVisitaBC;
import unisa.gps.etour.repository.IDBVisitaPR;
import unisa.gps.etour.util.MessaggiErrore;

/ **
  * Class that implements the interface for managing the side Tourists
  * Agency and extends UnicastRemoteObject for communication in remote
  * Provides basic methods for handling and additional methods for returning
  * Tourists with special characteristics of
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class GestioneTuristiAgenzia extends GestioneTuristaComune implements
IGestioneTuristiAgenzia
(

Private IDBVisitaBC feedbackBC;
Private IDBVisitaPR feedbackPR;

public GestioneTuristiAgenzia () throws RemoteException
(
super ();
TRY
(
/ / Instantiate objects for database connections
feedbackBC = new DBVisitaBC ();
feedbackPR = new DBVisitaPR ();
)
/ / Exception on the database connection
catch (Exception e)
(
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ / Method for obtaining all tourists alike present
/ / In the database
<BeanTurista> ottieniTuristi public ArrayList () throws RemoteException
(
/ / Object that will contain the return value
ArrayList <BeanTurista> toReturn;
/ / Retrieve data
TRY
(
/ / Invoke the method with empty string to get all Tourists
toReturn = turista.ottieniTuristi ("");
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Exception in database operations
catch (SQLException e)
(
/ / If the data layer is thrown an exception SQLException
/ / It throws RemoteException the remote exception
System.out.println ( "Error in method ottieniTuristi"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniTuristi"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Return the result from the data layer ottenuro
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
toReturn return;
)

/ / Method that allows to obtain all the tourists who have an account
/ / Active or not
public ArrayList <BeanTurista> ottieniTuristi (boolean statoAccount)
throws RemoteException
(
/ / ArrayList containing the results
ArrayList <BeanTurista> toReturn;
/ / Retrieve data
TRY
(
/ / All tourists are taken to the state as a parameter
toReturn = turista.ottieniTuristi (statoAccount);
)
/ / Exception in implementing the operation on the database
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method ottieniTuristi (boolean)"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniTuristi (boolean)"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the data to return, so you do not return null values
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
toReturn return;
)

/ / Method that allows the activation of a tourist is not yet activated
public boolean attivaTurista (int pIdTurista) throws RemoteException
(
/ / Check the validity of input data
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Bean that contains the data of tourists to activate
BeanTurista toChange;
/ / Retrieve and edit data required
TRY
(
/ / Get the data from the Database
toChange = turista.ottieniTurista (pIdTurista);
/ / Check that the tourist is not already activated
if (toChange.isAttiva ())
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / It sets the value indicating the activation status to true
toChange.setAttiva (true);
/ / You try to change the database
if (turista.modificaTurista (toChange))
/ / If the edit operation is successful returns
/ / True
return true;
)
/ / Exception in the execution of database operations
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method attivaTurista"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method attivaTurista"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If no exceptions were thrown but the changes
/ / Not have been successful returns false
return false;
)

/ / Method that allows you to disable a tourist
public boolean disattivaTurista (int pIdTurista) throws RemoteException
(
/ / Check the validity of data
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
BeanTurista toChange / / bean that contains the data from the tourist
/ / Disable
/ / Retrieve and edit data required
TRY
(
/ / Get the data from the database
toChange = turista.ottieniTurista (pIdTurista);
/ / Check that the tourist is active
if (! toChange.isAttiva ())
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / It sets the value indicating the activation status to false
toChange.setAttiva (false);
/ / You try to change the database
if (turista.modificaTurista (toChange))
/ / If the edit operation is successful returns
/ / True
return true;
)
/ / Exception in the execution of database operations
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method disattivaTurista"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method disattivaTurista"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If no exceptions were thrown but the changes
/ / Not have been successful returns false
return false;
)

/ / Method that erases a tourist from the Database
public boolean delete (int pIdTurista) throws RemoteException
(
/ / Check the validity of data
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
TRY
(
/ / If the cancellation is successful it returns true
if (turista.cancellaTurista (pIdTurista))
return true;
)
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in delete method"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in delete method"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Otherwise if you have not thrown exceptions, but the cancellation
/ / Not have been successful returns false
return false;
)

/ / Method that returns an ArrayList containing the feedback issued by
/ / Some for the Cultural Tourist
<BeanVisitaBC> ottieniFeedbackBC public ArrayList (int pIdTurista)
throws RemoteException
(
/ / Check the validity of data
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / ArrayList return
ArrayList <BeanVisitaBC> toReturn;
TRY
(
/ / Retrieve data from Database
toReturn = feedbackBC.ottieniListaVisitaBCTurista (pIdTurista);
)
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method ottieniFeedbackBC"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniFeedbackBC"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the data back in order not to return null values
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
toReturn return;
)

/ / Method that returns an ArrayList containing the feedback issued by
/ / Some for the Cultural Tourist
<BeanVisitaPR> ottieniFeedbackPR public ArrayList (int pIdTurista)
throws RemoteException
(
/ / Check the validity of data
if (pIdTurista <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
ArrayList <BeanVisitaPR> toReturn / / variable return
TRY
(
/ / Insert the result in toReturn
/ / Returned from the Data Layer
toReturn = feedbackPR.ottieniListaVisitaPRTurista (pIdTurista);
)
catch (SQLException e)
(
/ / If the data layer sends an exception is throws the exception remote
System.out.println ( "Error in method ottieniFeedbackPR"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniFeedbackPR"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the array of return, so you do not return null values
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
toReturn return;
)
)
