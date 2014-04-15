package unisa.gps.etour.control.GestioneRicerche;

import java.rmi.Remote;
import java.rmi.RemoteException;

import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.util.Punto3D;

/ **
  * Interface for management of research
  *
  * @ Author Joseph Penna
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public interface extends Remote IRicerca
(

/ / Constants identify the types of site
public final static byte BENE_CULTURALE = 0;
public final static byte PUNTO_DI_RISTORO = 1;

/ **
* Initialization method for research
*
* @ Param pIdTurista ID of the tourist. Pass -1 in case of a Guest
* @ Param pParoleChiave Together keyword search
Together * @ param pTagsId Search Tags
* @ Param pRaggioMax Maximum distance between the user and the site
* @ Param numeroElementiPerPagina number of items to look for in a
* Search session
* @ Param pPosizioneUtente detected by the GPS user position
* @ Param type pTipologiaSito site search
* @ Return number of elements emerged from the research. On error
* Returns -1
* @ Throws RemoteException the remote exception
* /
public int search (int pIdTurista, pParoleChiave String, int [] pTagsId, double pRaggioMax,
int pElementiPerPagina, Punto3D pPosizioneUtente,
pTipologiaSito bytes) throws RemoteException;

/ **
* Method for returning the list of emerging from the Cultural Heritage
* Research, in a given interval
*
* @ Param pPagina range of items to be included in the results
* @ Return entirety of cultural property related to the range of results
* Selected search. In case of error returns null
* @ Throws RemoteException Exception Remote
* /
public BeanBeneCulturale [] ottieniPaginaRisultatiBeneCulturale (int pPagina)
throws RemoteException;

/ **
* Method to return the list of eateries have emerged from
* Research related to a specific intevallo
*
* @ Param pPagina range of items to be included in the results
* @ Return set of points relating to the range of refreshment
* Selected search results. In case of error returns null
* @ Throws RemoteException Exception Remote
* /
public BeanPuntoDiRistoro [] ottieniPaginaRisultatiPuntoDiRistoro (int pPagina)
throws RemoteException;

/ **
* Method for returning the number of elements results from
* Search
*
* @ Return number of elements emerged in the research phase. Where no
* Is initialized the search returns -1
* /
public int ottieniNumeroElementiRicerca () throws RemoteException;

/ **
* Method for returning the number of pages appear in results
*
* @ Return number of pages that have emerged in the research phase. Where no
* Is initialized the search returns -1
* /
public int ottieniNumeroPagineRicerca () throws RemoteException;

)
