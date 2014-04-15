package unisa.gps.etour.control.GestionePuntiDiRistoro;

import java.rmi.RemoteException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import unisa.gps.etour.bean.BeanConvenzione;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.bean.BeanVisitaPR;
/ *
  * TEST CASE import
  * Unisa.gps.etour.control.GestionePuntiDiRistoro.test.stub.DBConvenzione;
  * Import unisa.gps.etour.control.GestionePuntiDiRistoro.test.stub.DBTurista;
  * /
import unisa.gps.etour.repository.DBConvenzione;
import unisa.gps.etour.repository.DBTurista;
import unisa.gps.etour.repository.IDBConvenzione;
import unisa.gps.etour.util.MessaggiErrore;

/ **
  * Class contentente methods for managing Refreshments by
  * Operator Agency
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class GestionePuntiDiRistoroAgenzia extends GestionePuntiDiRistoroComune
implements IGestionePuntiDiRistoroAgenzia
(

private static final long serialVersionUID = 1L;

/ / Constructor
public GestionePuntiDiRistoroAgenzia () throws RemoteException
(
/ / Call the constructor of the inherited class to instantiate
/ / Database connections
super ();
dbTurista = new DBTurista ();
)

/ / Method that allows the operator to cancel an agency point of
/ / Refreshment
/ / Passing as parameter the ID of the same Refreshment
public boolean cancellaPuntoDiRistoro (int pPuntoDiRistoroID)
throws RemoteException
(
/ / Check the validity identifier
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
TRY
(
/ / Execute the method that clears the Refreshment from the Database
/ / And in case of operation successful return true
if (puntoRistoro.cancellaPuntoDiRistoro (pPuntoDiRistoroID))
return true;
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out.println ( "Error in method cancellaPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method cancellaPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If no operations were successful return false end
return false;
)

/ / Method that allows the operator to include in the Agency database
/ / The new Refreshment with the information contained in the bean
public boolean inserisciPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws RemoteException
(
/ / Check the validity of the bean as a parameter and if
/ / Triggers except remote
if ((pPuntoDiRistoro == null)
| | (! (PPuntoDiRistoro instanceof BeanPuntoDiRistoro)))
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
TRY
(
/ / Calling the method of the class that operates on the database
/ / Insert the new Refreshment
if (puntoRistoro.inserisciPuntoDiRistoro (pPuntoDiRistoro))
/ / In the case where the operations were successful end
/ / Returns true
return true;
)
/ / Exception in database operations
catch (SQLException e)
(
System.out.println ( "Error in method inserisciPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method inserisciPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If the operation is not successful return false
return false;
)

/ / Method for obtaining an ArrayList with all the points Bean
/ / Refreshments
<BeanPuntoDiRistoro> ottieniPuntiDiRistoro public ArrayList ()
throws RemoteException
(
/ / ArrayList to return to the end of the method
ArrayList <BeanPuntoDiRistoro> toReturn = null;
TRY
(
/ / Get the list of Refreshments through the class
/ / Connect to database
/ / And save the list itself nell'ArrayList
toReturn = puntoRistoro.ottieniListaPR ();
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out.println ( "Error in method ottieniPuntiDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniPuntiDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the ArrayList to return so as not to pass null values
/ / To the caller
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Return the ArrayList with all the refreshment
toReturn return;
)

/ / Method that allows you to get all the refreshment that have
/ / A Convention on or off depending on the parameter passed
public ArrayList <BeanPuntoDiRistoro> ottieniPuntiDiRistoro (
statoConvenzione boolean) throws RemoteException
(
/ / Array that allows me to store all the refreshment and
/ / Which will remove
/ / Depending on the parameter passed to the refreshment active or not
ArrayList <BeanPuntoDiRistoro> toReturn = null;
/ / Array that allows me to store all the refreshment active
/ / Using the database connection
ArrayList <BeanPuntoDiRistoro> active = null;
/ / Instance to connect to the database
IDBConvenzione conv = new DBConvenzione ();
TRY
(
/ / Connect all proceeds from the refreshment Assets
conv.ottieniListaConvenzioneAttivaPR assets = ();
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method ottieniPuntiDiRistoro (boolean)"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method ottieniPuntiDiRistoro (boolean)"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If you want to get the refreshment active, then return
/ / Directly to those passed by the connection to the database
if (statoConvenzione)
(
/ / Check the contents dell'ArrayList so as not to return
/ / Null values to the caller
if (active == null)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
return active;
)
else
(
TRY
(
/ / Connect all proceeds from the refreshment then
/ / Perform comparisons
toReturn = puntoRistoro.ottieniListaPR ();
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method ottieniPuntiDiRistoro (boolean)"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method ottieniPuntiDiRistoro (boolean)"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Size dell'ArrayList containing all of gourmet
/ / Could
/ / Change size if you remove some element
int dim = toReturn.size ();
/ / Variable that allows me to understand whether to remove a Point
/ / Refreshments
/ / From array that then I must return
boolean present = false;
/ / First loop to loop through all the ArrayList elements of
/ / All Refreshments
for (int i = 0; i <dim; i + +)
(
/ / Second loop to loop through all the ArrayList elements
/ / Cones just Refreshments active
for (int j = 0 j <attivi.size () j + +)
(
/ / If the catering points in question has the ID equal to one
/ / Of those assets, then set this to true
if (attivi.get (j). getId () == toReturn.get (i). getId ())
present = true;
)
/ / If the catering points in question has a Convention active
/ / Removes it from those to be returned
if (present)
toReturn.remove (i);
)
)
/ / Return the ArrayList obtained
toReturn return;
)

/ / Method that allows you to change the past as a refreshment
/ / Parameter
public boolean modificaPuntoDiRistoro (
BeanPuntoDiRistoro pPuntoDiRistoroAggiornato)
throws RemoteException
(
/ / Check the validity of the bean as a parameter and if
/ / Trigger an exception remote
if (null == pPuntoDiRistoroAggiornato
| | (! (PPuntoDiRistoroAggiornato instanceof BeanPuntoDiRistoro)))
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
TRY
(
/ / Call the method to change the database connection
/ / The Refreshment
if (puntoRistoro.modificaPuntoDiRistoro (pPuntoDiRistoroAggiornato))
/ / Return a positive value if the operation was successful
/ / End
return true;
)
/ / Exception in operations on database
catch (SQLException e)
(
System.out.println ( "Error in method modificaPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method modificaPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Return false if the operation is successful you should
return false;
)

/ / Method to obtain the Bean a particular point
/ / Refreshment whose
/ / Identifier is passed as parameter
public BeanPuntoDiRistoro ottieniPuntoDiRistoro (int pPuntoDiRistoroID)
throws RemoteException
(
/ / Check the validity identifier
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Bean to return to the caller
BeanPuntoDiRistoro toReturn = null;
TRY
(
/ / Revenue catering points in the issue by connecting to
/ / Database
toReturn = puntoRistoro.ottieniPuntoDiRistoro (pPuntoDiRistoroID);
)
/ / Exception in the database opearazioni
catch (SQLException e)
(
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method ottieniPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check the bean to be returned in order not to return null values
/ / To the caller
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
/ / Return the bean of Refreshment
toReturn return;
)

/ / Method that allows you to activate a particular convention to a Point
/ / Passed as parameter Refreshments
public boolean attivaConvenzione (int pPuntoDiRistoroID,
BeanConvenzione pConv) throws RemoteException
(
/ / Check the validity of parameters passed
if ((pPuntoDiRistoroID <0) | | (pConv == null)
| | (! (PConv instanceof BeanConvenzione)))
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Check the data further
if (pConv.getIdPuntoDiRistoro ()! = pPuntoDiRistoroID)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
IDBConvenzione conv = null;
TRY
(
/ / Instantiate the class to connect to the database
conv = new DBConvenzione ();
/ / If the Convention is not yet active, previously provided to
/ / Activate it locally and then pass the bean to the database changed
if (conv.ottieniConvezioneAttiva (pPuntoDiRistoroID) == null)
(
pConv.setAttiva (true);
conv.modificaConvenzione (pConv);
return true;
)
)
/ / Exception in operations on database
catch (SQLException e)
(
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method attivaConvenzione"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If the operation is successful you should return false
return false;
)

/ / Method that returns a HashMap containing, for Refreshment
/ / Passed as a parameter, the feedback associated with it
public HashMap <BeanVisitaPR, String> ottieniFeedbackPuntoDiRistoro (
pPuntoDiRistoroID int) throws RemoteException
(
/ / Check the ID passed as a parameter
if (pPuntoDiRistoroID <0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Instantiate the map and the performance of ArrayList that I will use
/ / Method
HashMap <BeanVisitaPR, String> mappaRitorno = null;
ArrayList <BeanVisitaPR> bvisita = null;
TRY
(
/ / Here I take the list of all visits to the PR passed as
/ / Parameter
bvisita = feed.ottieniListaVisitaPR (pPuntoDiRistoroID);
/ / Instantiate the map of the same size as the list of
/ / BeanVisitaPR
mappaRitorno = new HashMap <BeanVisitaPR, String> (bvisita.size ());
/ / Here we begin to iterate on each visit to add its
/ / Username
for (Iterator <BeanVisitaPR> iteratoreVisitaPR = bvisita.iterator (); iteratoreVisitaPR
. hasNext ();)
(
/ / Recuperto the BeanVisitaPR
BeanVisitaPR bVisitaTemp = iteratoreVisitaPR.next ();
/ / Retrieve the tourist who left the comment that I
/ / Examining
BeanTurista bTuristaTemp = dbTurista.ottieniTurista (bVisitaTemp
. getIdTurista ());
/ / Get the username of the Tourist
String usernameTuristaTemp = bTuristaTemp.getUsername ();
/ / Put the pair in the map
mappaRitorno.put (bVisitaTemp, usernameTuristaTemp);
)
)
/ / Exception in database operations
catch (SQLException e)
(
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
/ / Check the return parameter so as not to pass null values
/ / To the database
if (null == mappaRitorno)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
mappaRitorno return;
)

/ / Method to insert a tag from those of a refreshment
public boolean cancellaTagPuntoDiRistoro (pPuntoDiRistoroId int, int pTagId)
throws RemoteException
(
/ / Check the validity of past data
if ((pPuntoDiRistoroId <0) | | (pTagId <0))
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / ArrayList which stores all the tags
ArrayList <BeanTag> tags;
/ / Boolean variable to check if the Refreshment
/ / Holds the tag you want to delete
boolean present = false;
TRY
(
/ / Use the method through the class of database connection
tags = tag.ottieniTagPuntoDiRistoro (pPuntoDiRistoroId);
)
/ / Exception in the execution of transactions in database
catch (SQLException e)
(
System.out.println ( "Error in method cancellaTagPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception due to other factors
catch (Exception ee)
(
System.out.println ( "Error in method cancellaTagPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / Check if the tag is present cycle currently
/ / Between those of Refreshment
for (t BeanTag: tags)
if (t.getId () == pTagId)
present = true;
/ / If the tag is present among those of eateries, then
/ / Provides for executing the erase operation
if (present)
(
TRY
(
return tag.cancellaTagPuntoDiRistoro (pPuntoDiRistoroId, pTagId);
)
/ / Exception in implementing the operation on the database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method cancellaTagPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exception due to other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method cancellaTagPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)
/ / In case something did not come to fruition
/ / Return false
return false;
)

/ / Method to delete a tag from those of a refreshment
/ / The operations are identical to those above, except for
/ / Control over the presence of the tag from those of Refreshment
/ / Which should give negative results, and the call here is the method of
/ / Insert
public boolean inserisciTagPuntoDiRistoro (pPuntoDiRistoroId int, int pTagId)
throws RemoteException
(
if ((pPuntoDiRistoroId <0) | | (pTagId <0))
throw new RemoteException (MessaggiErrore.ERRORE_DATI);

ArrayList <BeanTag> tags;
boolean present = false;
TRY
(
tags = tag.ottieniTagPuntoDiRistoro (pPuntoDiRistoroId);
)
catch (SQLException e)
(
System.out.println ( "Error in method inserisciTagPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
catch (Exception ee)
(
System.out.println ( "Error in method inserisciTagPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
for (t BeanTag: tags)
if (t.getId () == pTagId)
present = true;
/ / Check that the Refreshment has not already specified tag
if (present)
(
TRY
(
/ / Calling the method of adding the class via
/ / Connect to database
return tag.aggiungeTagPuntoDiRistoro (pPuntoDiRistoroId, pTagId);
)
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method inserisciTagPuntoDiRistoro"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method inserisciTagPuntoDiRistoro"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)
/ / Return false if some operation is not successful you should
return false;
)
)
