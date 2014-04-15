package unisa.gps.etour.control.GestioneTag;

import java.rmi.RemoteException;
import java.sql.SQLException;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.util.MessaggiErrore;

/ **
  * Class that implements the methods for the functionality of the Operator Agency
  * Extending the class of common Tag Management
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class GestioneTagOperatoreAgenzia extends GestioneTagComune implements
IGestioneTagOperatoreAgenzia
(

private static final long serialVersionUID = 1L;

public GestioneTagOperatoreAgenzia () throws RemoteException
(
/ / Invoke the constructor of the superclass for communication with
/ / Database
super ();
)

/ / Method to delete from database the tag whose ID is passed
/ / As parameter
public boolean cancellaTag (int pTagID) throws RemoteException
(
/ / Check the valise of past data
if ((pTagID <= 0))
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
TRY
(
/ / Make the database operation
tag.cancellaTag (pTagID);
return true;
)
/ / Exception in the execution of database operations
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method cancellaTag:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method cancellaTag"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ / Method that allows the insertion of a new tag as a parameter
public boolean inserisciTag (BeanTag pTagNuovo) throws RemoteException
(
/ / Check the validity of the Bean and the data contained within
if (null == pTagNuovo)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
if ((pTagNuovo = checkTag (pTagNuovo)) == null)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
TRY
(
/ / Execute the operation on the Database
tag.inserisciTag (pTagNuovo);
return true;
)
/ / Exception running the operation on Database
catch (SQLException e)
(
System.out.println ( "Error in method inserisciTag"
+ E.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method inserisciTag"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ / Method that enables the modification of a tag that is passed as
/ / Parameter
public boolean modificaTag (BeanTag pTagModificato) throws RemoteException
(
/ / Check the validity of data
if ((pTagModificato = checkTag (pTagModificato)) == null)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
TRY
(
/ / Execute the operation on the Database
tag.modificaTag (pTagModificato);
return true;
)
/ / Exception running the operation on Database
catch (SQLException e)
(
System.out
. System.out.println ( "Error in method modificaTag:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out.println ( "Error in method modificaTag"
Ee.toString + ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)

/ / Method to obtain the tags whose identifier is passed
/ / As parameter
public BeanTag ottieniTag (int pTagID) throws RemoteException
(
/ / Check the validity of data
if (pTagID <= 0)
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
/ / Bean to return
BeanTag toReturn;
TRY
(
/ / Execute the operation on the Database
toReturn = tag.ottieniTag (pTagID);
)
/ / Exception running the operation on Database
catch (SQLException e)
(
System.out.println ( "Error in method ottieniTag:" + e.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_DBMS);
)
/ / Unexpected exceptions caused by other factors
catch (Exception ee)
(
System.out
. System.out.println ( "Error in method ottieniTag:" + ee.toString ());
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
if (null == toReturn)
throw new RemoteException (MessaggiErrore.ERRORE_FORMATO_BEAN);
toReturn return;
)

/ / Method that controls all the attributes of a BeanTag
Private BeanTag checkTag (BeanTag toControl)
(
/ / Check the ID
if (toControl.getId () <= 0)
return null;
/ / Check the description
if (toControl.getDescrizione (). equals (""))
toControl.setDescrizione ("***");
/ / Check the name
if (toControl.getNome (). equals (""))
return null;
/ / Check that the name does not contain a space
if (toControl.getNome (). Contains ( ""))
return null;
toControl return;
)
)
