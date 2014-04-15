package unisa.gps.etour.control.GestioneBeniCulturali;

import java.rmi.RemoteException;
import java.util.ArrayList;
import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanVisitaBC;

/ **
  * Interface for operations peculiar cultural heritage by
  * Operator Agency.
  *
  * @ Author Michelangelo De Simone
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public interface extends IGestioneBeniCulturaliAgenzia
IGestioneBeniCulturaliComune
(
/ **
* Method for the insertion of a new cultural
*
* @ Param pBeneCulturale The raw bean to be included in the database
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean inserisciBeneCulturale (BeanBeneCulturale pBeneCulturale) throws RemoteException;

/ **
* Method for the cancellation of a cultural object by id
*
* @ Param Id pBeneCulturaleID the bean to be deleted
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean cancellaBeneCulturale (int pBeneCulturaleID) throws RemoteException;

/ **
* Method for the return of all cultural property in the
* Database
*
* @ Return ArrayList all the beans in the database
* @ Throws RemoteException Exception flow
* /
<BeanBeneCulturale> ottieniBeniCulturali public ArrayList () throws RemoteException;

/ **
* Method for updating (or change) the data of a cultural
*
* @ Param pBeneCulturale The bean with the new information of the cultural
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean modificaBeneCulturale (BeanBeneCulturale pBeneCulturale) throws RemoteException;

/ **
* Method for setting a tag to a certain cultural
*
* @ Param pBeneCulturaleID The identifier of the cultural object to which to add a tag
* @ Param pTagID The ID tag to add to the cultural indicated
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean aggiungiTagBeneCulturale (pBeneCulturaleID int, int pTagID) throws RemoteException;

/ **
* Method for removing a tag from a certain cultural
* To ensure that 'the operation is successful it is necessary that the cultural property has
* Actually set the specified tag
*
* @ Param pBeneCulturaleID The identifier of the cultural object from which to remove the tag
* @ Param pTagID The ID tag to be removed from the cultural indicated
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean rimuoviTagBeneCulturale (pBeneCulturaleID int, int pTagID) throws RemoteException;
) 