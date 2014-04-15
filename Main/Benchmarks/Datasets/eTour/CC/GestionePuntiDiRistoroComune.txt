package unisa.gps.etour.control.GestionePuntiDiRistoro;

import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanVisitaPR;
/ *
  * For import junit tests
  * Unisa.gps.etour.control.GestionePuntiDiRistoro.test.stub.DBPuntoDiRistoro;
  * Import unisa.gps.etour.control.GestionePuntiDiRistoro.test.stub.DBVisitaPR;
  * /
import unisa.gps.etour.repository.DBPuntoDiRistoro;
import unisa.gps.etour.repository.DBVisitaPR;
import unisa.gps.etour.repository.DBTag;
import unisa.gps.etour.repository.IDBPuntoDiRistoro;
import unisa.gps.etour.repository.IDBTag;
import unisa.gps.etour.repository.IDBTurista;
import unisa.gps.etour.repository.IDBVisitaPR;
import unisa.gps.etour.util.CostantiGlobali;
import unisa.gps.etour.util.MessaggiErrore;

/ **
  * Class that implements the common tasks for the operator of dining and
  * For the Operator Agency
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
GestionePuntiDiRistoroComune UnicastRemoteObject public class extends implements
IGestionePuntiDiRistoroComune
(

/ / Instance for database connections
private static final long serialVersionUID = 1L;
protected IDBPuntoDiRistoro puntoRistoro;
protected IDBTag tags;
IDBVisitaPR protected feeds;
protected IDBTurista dbTurista;

public GestionePuntiDiRistoroComune () throws RemoteException
(
super ();
/ / Connect to the Database
TRY
(
puntoRistoro = new DBPuntoDiRistoro ();
tag = new DBTag ();
feed = new DBVisitaPR ();
)
/ / Note: no longer 'cause SQLException thrown
/ / Changes to the layer DB; changed Exception
catch (Exception e)
(
System.out.println ( "Error:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ *
* Method which allows to obtain a BeanPuntoDiRistoro through
* Connect to database
* /
public BeanPuntoDiRistoro ottieniPuntoDiRistoro (int pPuntoDiRistoroID)
throws RemoteException
(
/ / Check identifier passed
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Return Instance
BeanPuntoDiRistoro toReturn = null;
TRY
(
/ / Revenue data through the instance of the database connection
toReturn = puntoRistoro.ottieniPuntoDiRistoro (pPuntoDiRistoroID);
)
/ / Exception in database operations
catch (SQLException e)
(
System.out.println ( "Error:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error:" + ee.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the variable return, so they do not pass bad data
/ / To the caller
/ / And triggers an exception if the format of the bean
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Return the bean that contains information about Refreshment
/ / Required
toReturn return;
)

/ / Method that returns an ArrayList containing the tag identification of a
/ / Some Refreshment
<BeanTag> ottieniTagPuntoDiRistoro public ArrayList (int pPuntoDiRistoroID)
throws RemoteException
(
/ / Check identifier passed
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Return Instance
ArrayList <BeanTag> toReturn = null;
TRY
(
/ / Revenue data through the instance of the database connection
toReturn = tag.ottieniTagPuntoDiRistoro (pPuntoDiRistoroID);
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out.println ( "Error:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error:" + ee.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the variable return, so they do not pass bad data
/ / To the caller
/ / And triggers an exception if the format of the bean
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Return the ArrayList containing beans tag of refreshment
/ / Passed as parameter
toReturn return;
)

/ / Returns an array of 10 strings containing the last 10 comments
/ / Issued by tourists for Refreshment passed as parameter
public String [] ottieniUltimiCommenti (int pPuntoDiRistoroID)
throws RemoteException
(
/ / Check the validity identifier passed
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / String that contains only the last 10 comments made
String [] toReturn = new String [10];
/ / ArrayList temporary then save the comments contained in the beans
/ / Inside
<BeanVisitaPR> ArrayList temp = null;
TRY
(
/ / Attempt to retrieve information from the database via
/ / The connection instance
temp = feed.ottieniListaVisitaPR (pPuntoDiRistoroID);
)
/ / Exception in database operations
catch (SQLException e)
(
System.out.println ( "Error in method ottieniUltimiCommenti"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniUltimiCommenti"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / The bean that I receive from the database are already sorted by date
/ / Implicitly, so there is no need for sorting algorithms
for (int i = 0; i <10; i + +)
(
/ / Insert the text of the comments in cyclically
/ / Array
toReturn [i] = (temp.get (temp.size () - (i + 1))). getCommento ();
)
/ / E 'unnecessary control the format of the array
/ / Then return the array directly with comments
toReturn return;
)

/ / Method that returns an ArrayList containing the number of votes
/ / Release from 1 to 5 for
/ / The Refreshment passed as parameter. The method inserts in order
/ / Counters in positions
/ / From 0 to 1 vote, 1 vote for 2, etc. ..
public ArrayList <Integer> ottieniStatistichePuntoDiRistoro (
pPuntoDiRistoroID int) throws RemoteException
(
/ / Check the validity identifier passed
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / ArrayList that allows me to store the bean containing the feedback
ArrayList <BeanVisitaPR> bVisita = null;
/ / ArrayList that allows me to store the counters
/ / And then return to the calling method
ArrayList <Integer> listaRisultati <Integer> = new ArrayList (5);
/ / Initialize the array of counters
for (int i = 0; i <5; i + +)
listaRisultati.add (Integer.valueOf (0));
/ / Data useful to verify that the issued date back thirty Feedback
/ / Days from current date
Date ultimiTrentaGiorni = new Date (new Date (). GetTime ()
- CostantiGlobali.TRENTA_GIORNI);

System.out.println ( "The date of 30 days ago:" + ultimiTrentaGiorni);

TRY
(
/ / All feedback Revenue issued for Refreshment
/ / Passed as parameter
bVisita = feed.ottieniListaVisitaPR (pPuntoDiRistoroID);
/ / Iterate the collection of elements to control the bean
/ / Visits
for (Iterator <BeanVisitaPR> iteratoreVisitaPR = bVisita.iterator (); iteratoreVisitaPR
. hasNext ();)
(
/ / Recuperto the BeanVisitaPR
BeanVisitaPR bVisitaTemp = iteratoreVisitaPR.next ();
/ / ... Do not know what does ...
System.out.println ( "The date of this visit is:
BVisitaTemp.getDataVisita + ());
if (bVisitaTemp.getDataVisita (). after (ultimiTrentaGiorni))
listaRisultati.set (bVisitaTemp.getVoto () - 1, Integer
. valueOf (listaRisultati.get (
bVisitaTemp.getVoto () - 1). intValue () + 1));
)
)
/ / Exception in database operations
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method ottieniStatistichePuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method ottieniStatistichePuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
if (null == listaRisultati)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Return the list of results that should contain counters
/ / Comments
/ / Issued in the last 30 days
listaRisultati return;
)

/ / Method that allows you to change the comment issued for a
/ / Refreshment
public boolean modificaFeedbackPuntoDiRistoro (int pPuntoDiRistoroId,
BeanVisitaPR nuovaVisita) throws RemoteException
(
/ / Check the validity of past data
if ((pPuntoDiRistoroId <0) | | (! (nuovaVisita instanceof BeanVisitaPR)))
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Revenue from the database the bean and stores saved
/ / In order to verify that the vote has not changed
/ / Which had been previously released
BeanVisitaPR temp = null;
TRY
(
temp = feed.ottieniVisitaPR (pPuntoDiRistoroId, nuovaVisita
. getIdTurista ());
)
/ / Exception running the operation on Database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method modificaFeedbackPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method modificaFeedbackPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If the vote was amended returns an exception
if (temp.getVoto ()! nuovaVisita.getVoto = ())
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / If the vote has not been amended shall save the new
/ / Comment in the database using the specific method of the bean
else
(
TRY
(
feed.modificaVisitaPR (nuovaVisita);
return true;
)
/ / Exception running the operation on Database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method modificaFeedbackPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception caused by other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method modificaFeedbackPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)
)
)
