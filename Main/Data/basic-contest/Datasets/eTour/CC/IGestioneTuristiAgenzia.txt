package unisa.gps.etour.control.GestioneUtentiRegistrati;

import java.rmi.RemoteException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Interface for handling tourists from the side of the transaction Agency
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IGestioneTuristiAgenzia extends IGestioneTuristaComune
(

/ **
* Method for the cancellation of a tourist from the Database
*
* @ Param pIdTurista Identifier Tourist delete
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean delete (int pIdTurista) throws RemoteException;

/ **
* Method to activate a registered tourists
*
* @ Param pIdTurista ID to activate the Tourist
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean attivaTurista (int pIdTurista) throws RemoteException;

/ **
* Method to disable an active tourists
*
* @ Param to disable pIdTurista Identifier Tourist
* @ Return Boolean: true if the operation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean disattivaTurista (int pIdTurista) throws RemoteException;

/ **
* Method to obtain a collection of Tourists
*
* @ Return ArrayList of BeanTurista
* @ Throws RemoteException Exception Remote
* /
<BeanTurista> ottieniTuristi public ArrayList () throws RemoteException;

/ **
* Method to obtain a collection of active tourists or not
*
* @ Param boolean statoAccount Tourists can choose
* On whether
* @ Return ArrayList of BeanTurista
* @ Throws RemoteException Exception Remote
* /
public ArrayList <BeanTurista> ottieniTuristi (boolean statoAccount)
throws RemoteException;

/ **
* Method to get all the feedback issued by a tourist for the points
* Refreshments
*
* @ Param pIdTurista ID to pick up the tourists in
* Feedback
* @ Return ArrayList containing all the beans Feedback released
* @ Throws RemoteException Exception Remote
* /
<BeanVisitaPR> ottieniFeedbackPR public ArrayList (int pIdTurista)
throws RemoteException;

/ **
* Method to get all the feedback issued by a tourist for Heritage
* Cultural
*
* @ Param pIdTurista ID to pick up the tourists in
* Feedback
* @ Return ArrayList containing all the beans Feedback released
* @ Throws RemoteException Exception Remote
* /
<BeanVisitaBC> ottieniFeedbackBC public ArrayList (int pIdTurista)
throws RemoteException;

)
