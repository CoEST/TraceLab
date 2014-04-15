package unisa.gps.etour.control.GestionePuntiDiRistoro;

import java.rmi.Remote;
import java.rmi.RemoteException;
import java.util.ArrayList;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Interface for common operations on the refreshment
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface extends Remote IGestionePuntiDiRistoroComune
(

/ **
* Method to return a particular Refreshment
*
* @ Param pPuntoDiRistoroID to identify the Refreshment from
* Return
* @ Return Bean contains the data of Refreshment concerned
* @ Throws RemoteException Exception Remote
* /
public BeanPuntoDiRistoro ottieniPuntoDiRistoro (int pPuntoDiRistoroID)
throws RemoteException;

/ **
* Method which returns the tags to some refreshment
*
* @ Param pPuntoDiRistoroID point identification Refreshment
* @ Return structure containing all BeanTag associated with the point
* Refreshments passed as parameter
* @ Throws RemoteException Exception Remote
* /
<BeanTag> ottieniTagPuntoDiRistoro public ArrayList (int pPuntoDiRistoroID)
throws RemoteException;

/ **
* Method which returns the last 10 comments made for a
* Refreshment
*
* @ Param pPuntoDiRistoroID ID for the point of rest in
* Question
* @ Return Array of strings containing 10 items
* @ Throws RemoteException Exception Remote
* /
public String [] ottieniUltimiCommenti (int pPuntoDiRistoroID)
throws RemoteException;

/ **
* Returns for the Refreshment specified, an array where each
* Location contains the number of ratings corresponding to the value
* Index of the array more 'one. The calculation and 'made in the period
* 30 days and today.
*
* @ Param pPuntoDiRistoroID unique identifier of Refreshment
* @ Return ArrayList containing the counters as explained above
* @ Throws RemoteException Exception Remote
* /
public ArrayList <Integer> ottieniStatistichePuntoDiRistoro (
pPuntoDiRistoroID int) throws RemoteException;

/ **
* Method which allows you to change the comment issued for a
* Refreshment
*
* @ Param pPuntoDiRistoroId unique identifier of Refreshment
* @ Param nuovaVisita Bean containing new comment
* @ Return Boolean value-true if the operation went successfully,
* False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean modificaFeedbackPuntoDiRistoro (int pPuntoDiRistoroId,
BeanVisitaPR nuovaVisita) throws RemoteException;
)
