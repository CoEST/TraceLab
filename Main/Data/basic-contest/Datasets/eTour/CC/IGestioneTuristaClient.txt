package unisa.gps.etour.control.GestioneUtentiRegistrati;

import java.rmi.RemoteException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanPreferenzaDiRicerca;
import unisa.gps.etour.bean.BeanPreferenzeGeneriche;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Interface on the Management of Tourist Information
  *
  * @ Author Joseph Penna, Federico Leon
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab University of DMI
  * Salerno
  * /
public interface IGestioneTuristaClient extends IGestioneTuristaComune
(

/ **
* Method for the insertion of a Tourist
*
* @ Param pTurista container for all data relating to tourism by
* Insert
* @ Return Boolean: True if the insertion is successful, False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciTurista (BeanTurista pTurista)
throws RemoteException;

/ **
* Method for including the General Preferences Tourist
*
* @ Param pIdTurista Identifier Turista which involve
* General Preferences
* @ Param pPreferenzeGeneriche General Preferences for inclusion
* @ Return Boolean: True if the insertion is successful, False otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciPreferenzeGeneriche (
BeanPreferenzeGeneriche pPreferenzeGeneriche)
throws RemoteException;

/ **
* Method for the extraction Preferences generously given Tourists
*
* @ Param pIdTurista Identifier Turista which you want
* Get the General Preferences
* @ Return Preferences General information relating to tourism
* @ Throws RemoteException Exception Remote
* /
public BeanPreferenzeGeneriche ottieniPreferenzeGeneriche (int pIdTurista)
throws RemoteException;

/ **
* Method for changing the Preferences generously given Tourists
*
* @ Param ID pIdTurista of tourists for whom you want
* Change the General Preferences
* @ Param pPreferenzeGenericheNuove The Prefereferze General for inclusion
* @ Param pPreferenzeGenericheVecchie Preferences generous to replace
* @ Return Boolean: True if the MADIF successful, False otherwise
* @ Throws RemoteException
* /
public boolean modificaPreferenzeGeneriche (
BeanPreferenzeGeneriche pPreferenzeGenericheNuove)
throws RemoteException;

/ **
* Method for the removal of preferences associated with the General
* Tourist
*
* @ Param ID pIdTurista of tourists for whom you want
* Delete the General Preferences
* @ Return Preferences General erased
* @ Throws RemoteException Exception Remote
* /
public BeanPreferenzeGeneriche cancellaPreferenzeGeneriche (int pIdTurista)
throws RemoteException;

/ **
* Method to insert a Search Preferences
*
* @ Param ID pIdTurista of tourists for which you intend
* Insert a Search Preferences
* @ Param pPreferenzaDiRicerca Search Preferences be included
* @ Return Boolean: True if the insertion is successful, False otherwise
* @ Throws RemoteException
* /
public boolean inserisciPreferenzaDiRicerca (int pIdTurista,
BeanPreferenzaDiRicerca pPreferenzaDiRicerca)
throws RemoteException;

/ **
* Method for extracting the set of Search Preferences given
* Tourist
*
* @ Param ID pIdTurista of tourists for whom you want
* Extract search preferences
Together * @ return the search preferences associated with the Tourist information
* @ Throws RemoteException Exception Remote
* /
public BeanPreferenzaDiRicerca [] ottieniPreferenzeDiRicerca (
pIdTurista int) throws RemoteException;

/ **
* Method for deleting a Search Preference given its
* ID and Tourists
*
* @ Param ID pIdTurista of tourists for whom you want
* Delete a Search Preferences
* @ Param ID pIdPreferenzaDiRicerca Search Preferences
* To cancel
* @ Return The preference of search Delete
* @ Throws RemoteException Exception Remote
* /
public BeanPreferenzaDiRicerca cancellaPreferenzeDiRicerca (int pIdTurista,
pIdPreferenzaDiRicerca int) throws RemoteException;

/ **
* Method to extract the list references to the Cultural Heritage
* Visited by a tourist
*
* @ Param pIdTurista Identifier Turista
* @ Return list of references to the Cultural Heritage Visited
* @ Throws RemoteException Exception Remote
* /
public BeanVisitaBC [] ottieniBeniCulturaliVisitati (int pIdTurista)
throws RemoteException;

/ **
* Method for the extraction of the list when making reference to Refreshments
* Visited by a tourist
*
* @ Param pIdTurista Identifier Turista
* @ Return list of references to Refreshments Visited
* @ Throws RemoteException Exception Remote
* /
public BeanVisitaPR [] ottieniPuntiDiRistoroVisitati (int pIdTurista)
throws RemoteException;

/ **
* Method for the insertion of a cultural Visited
*
* @ Param pVisitaBC package containing all information relating to
* Visit
* @ Return true if the item is added successfully, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciBeneCulturaleVisitato (BeanVisitaBC pVisitaBC)
throws RemoteException;

/ **
* Method for inserting a refreshment Visited
*
* @ Param pVisitaPR package containing all information relating to
* Visit
* @ Return true if the item is added successfully, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciPuntoDiRistoroVisitato (BeanVisitaPR pVisitaPR)
throws RemoteException;

/ **
* Method for the insertion of a cultural object in the list of Favorites
*
* @ Param pIdTurista Identifier Turista
* @ Param ID pIdBeneCulturale of Cultural Heritage
* @ Return true if the insertion is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciBeneCulturalePreferito (int pIdTurista,
pIdBeneCulturale int) throws RemoteException;

/ **
* Method for inserting a refreshment to my Favorites
*
* @ Param pIdTurista Identifier Turista
* @ Param pIdPuntoDiRistoro point identification Refreshment
* @ Return true if the insertion is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean inserisciPuntoDiRistoroPreferito (int pIdTurista,
pIdPuntoDiRistoro int) throws RemoteException;

/ **
* Method for the cancellation of a cultural object from the list of Favorites
*
* @ Param pIdTurista Identifier Turista
* @ Param ID pIdBeneCulturale of Cultural Heritage
* @ Return true if the cancellation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean cancellaBeneCulturalePreferito (int pIdTurista,
pIdBeneCulturale int) throws RemoteException;

/ **
* Method for deleting a refreshment from the list of Favorites
*
* @ Param pIdTurista Identifier Turista
* @ Param pIdPuntoDiRistoro point identification Refreshment
* @ Return true if the cancellation is successful, false otherwise
* @ Throws RemoteException Exception Remote
* /
public boolean cancellaPuntoDiRistoroPreferito (int pIdTurista,
pIdPuntoDiRistoro int) throws RemoteException;

/ **
* Method to extract the list of Cultural Heritage Favorites
*
* @ Param pIdTurista Identifier Turista
* @ Return List of Cultural Heritage Favorites
* @ Throws RemoteException Exception Remote
* /
public BeanBeneCulturale [] ottieniBeniCulturaliPreferiti (int pIdTurista)
throws RemoteException;

/ **
* Method to extract the list of Refreshments
*
* @ Param pIdTurista Identifier Turista
* @ Return list of eateries Favorites
* @ Throws RemoteException Exception Remote
* /
public BeanPuntoDiRistoro [] ottieniPuntiDiRistoroPreferiti (int pIdTurista)
throws RemoteException;


)
