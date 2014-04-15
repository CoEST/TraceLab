package unisa.gps.etour.control.GestionePuntiDiRistoro;

import java.rmi.RemoteException;
import java.util.ArrayList;
import java.util.HashMap;

import unisa.gps.etour.bean.BeanConvenzione;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Interface for refreshments on the side of the agency
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University
  * Of Salerno
  * /
public interface extends IGestionePuntiDiRistoroAgenzia
IGestionePuntiDiRistoroComune
(

/ **
* Method for inserting a new Refreshment
*
* @ Param pPuntoDiRistoro containing all the data from the Refreshment
* Add
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws RemoteException;

/ **
* Method for deleting a refreshment bar with ID
*
* @ Param pIDPuntoDiRistoro for the unique identification of point
* Refreshments
* @ Throws RemoteException Exception Remote
* /
public boolean cancellaPuntoDiRistoro (int pPuntoDiRistoroID)
throws RemoteException;

/ **
* Method to return all the refreshment of the DataBase
*
* @ Return ArrayList containing all the beans of the present Refreshments
* In the DataBase
* @ Throws RemoteException Exception Remote
* /
<BeanPuntoDiRistoro> ottieniPuntiDiRistoro public ArrayList ()
throws RemoteException;

/ **
* Method to return all the refreshment with convention
* Active or not
*
* @ Param Boolean statoConvenzione for the type of eateries by
* Get (contracted or not)
* @ Return ArrayList containing all the beans of the present Refreshments
* In the database depending on the status of the Convention
* @ Throws RemoteException Exception Remote
* /
public ArrayList <BeanPuntoDiRistoro> ottieniPuntiDiRistoro (
statoConvenzione boolean) throws RemoteException;

/ **
* Method for inserting a new convention for a certain point
* Refreshments
*
* @ Param pPuntoDiRistoroID integer that uniquely identifies the point
* Refreshments
* @ Param pConv Convention that will enable (Parameter ID
* Refreshment create redundancy but is useful for security
* Data)
* @ Return boolean for confirmation of operation
* @ Throws RemoteException Exception Remote
* /
public boolean attivaConvenzione (int pPuntoDiRistoroID,
BeanConvenzione pConv) throws RemoteException;

/ **
* Method to get all the feedback associated to a certain point
* Refreshments
*
* @ Param pPuntoDiRistoroID unique identifier of the Refreshment
* To get feedback
* @ Return HashMap containing the bean as the key value of feedback and how
* The tourist who issued the feedback
* @ Throws RemoteException Exception Remote
* /
public HashMap <BeanVisitaPR, String> ottieniFeedbackPuntoDiRistoro (
pPuntoDiRistoroID int) throws RemoteException;

/ **
* Method for updating (or change) the data of a Refreshment
*
* @ Param pPuntoDiRistoroID for the unique identification of point
* Refreshments to be amended
* @ Param pPuntoDiRistoroAggiornato containing the new data to be saved
* @ Return Boolean value-true if the operation went successfully,
* False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean modificaPuntoDiRistoro (
BeanPuntoDiRistoro pPuntoDiRistoroAggiornato)
throws RemoteException;

/ **
* Method which allows you to insert a tag to search for a useful point
* Refreshments
*
* @ Param pPuntoDiRistoroId unique identifier of Refreshment
* @ Param pTagId unique ID tags to be inserted
* @ Return Boolean value-true if the operation went successfully,
* False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciTagPuntoDiRistoro (pPuntoDiRistoroId int, int pTagId)
throws RemoteException;

/ **
* Method which allows you to delete a tag to search for a useful point
* Refreshments
*
* @ Param pPuntoDiRistoroId unique identifier of Refreshment
* @ Param pTagId unique ID tags to be inserted
* @ Return Boolean value-true if the operation went successfully,
* False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean cancellaTagPuntoDiRistoro (pPuntoDiRistoroId int, int pTagId)
throws RemoteException;

) 