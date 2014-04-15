package unisa.gps.etour.control.GestioneTag;

import java.rmi.RemoteException;

import unisa.gps.etour.bean.BeanTag;

/ **
  * Interface for the tag handler by the Operator Agency
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IGestioneTagOperatoreAgenzia extends IGestioneTagComune
(

/ **
* Method to insert a new tag
*
* @ Param pTagNuovo containing all the data of the new Tag
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciTag (BeanTag pTagNuovo) throws RemoteException;

/ **
* Method for the cancellation of an existing tag
*
* @ Param pTagID to identify the tags in question
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean cancellaTag (int pTagID) throws RemoteException;

/ **
* Method for editing a Tag
*
* @ Param pTagModificato containing the details of the new Tag
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean modificaTag (BeanTag pTagModificato) throws RemoteException;

/ **
* Method which returns a tag identified by its ID
*
* @ Param pTagID to identify a particular tag
* @ Return a BeanTag containing data Tag concerned
* @ Throws RemoteException Exception Remote
* /
public BeanTag ottieniTag (int pTagID) throws RemoteException;

) 