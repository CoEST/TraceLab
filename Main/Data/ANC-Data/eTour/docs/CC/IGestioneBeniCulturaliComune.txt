package unisa.gps.etour.control.GestioneBeniCulturali;

import java.rmi.Remote;
import java.rmi.RemoteException;
import java.util.ArrayList;
import java.util.HashMap;
import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanVisitaBC;

/ **
  * Interface for operations common to users and operators on Agency
  * Beniculturali
  *
  * @ Author Michelangelo De Simone
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
public interface extends Remote IGestioneBeniCulturaliComune
(
/ **
*
* Method to return a particular Cultural Heritage
*
* @ Param pBeneCulturaleID The identifier of the cultural property to be returned
* @ Return BeanBeneCulturale Contains data required of Cultural Heritage
* @ Throws RemoteException Exception flow
* /
public BeanBeneCulturale ottieniBeneCulturale (int pBeneCulturaleID) throws RemoteException;

/ **
* Returns the list of tags of a cultural
*
* @ Param ID pBeneCulturaleID of Cultural Heritage
* @ Return ArrayList of the cultural <BeanTag> tags specified
* @ Throws RemoteException Exception flow
* /
<BeanTag> ottieniTagBeneCulturale public ArrayList (int pBeneCulturaleID) throws RemoteException;

/ **
*
* Returns a list of feedback to the cultural specified
*
* @ Param ID pBeneCulturaleID of Cultural Heritage
* @ Return HashMap <BeanVisitaBC, String> The feedback of Cultural Heritage
* @ Throws RemoteException Exception flow
* /
<BeanVisitaBC, String> ottieniFeedbackBeneCulturale public HashMap (int pBeneCulturaleID) throws RemoteException;

/ **
*
* Returns for the cultural property specified, an array where each position contains the number of
* Feedback corresponding to the value of the array more than 'one.
* The calculation and 'made in the period between 30 days and today.
*
* @ Param ID pBeneCulturaleID of Cultural Heritage
* @ Return ArrayList <Integer> The statistics of last thirty days
* @ Throws RemoteException Exception flow
* /
<Integer> ottieniStatisticheBeneCulturale public ArrayList (int pBeneCulturaleID) throws RemoteException;

/ **
* Method for updating (or modification) of a feedback for a certain good
* Cultural. The method has the burden of
*
* @ Param pBeneCulturaleID The identifier of the cultural change which the feedback
* @ Param pBeanVisitaBC The new feedback to the cultural indicated
* @ Return boolean The result of the operation; true if was successful, false otherwise
* @ Throws RemoteException Exception flow
* /
public boolean modificaFeedbackBeneCulturale (int pBeneCulturaleID, BeanVisitaBC pBeanVisitaBC) throws RemoteException;
) 