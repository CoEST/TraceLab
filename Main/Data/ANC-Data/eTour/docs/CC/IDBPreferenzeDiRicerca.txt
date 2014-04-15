package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanPreferenzaDiRicerca;

/ **
  * Interface for managing search preferences in database
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBPreferenzeDiRicerca
(
/ **
* Add a preference of Search
*
* @ Param pPreferenza Search Preferences
* @ Throws SQLException
* /
public boolean inserisciPreferenzaDiRicerca (
BeanPreferenzaDiRicerca pPreferenza) throws SQLException;

/ **
* Delete a preference for research
*
* @ Param pPreferenza preference to eliminate
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPreferenzaDiRicerca (int pIdPreferenza)
throws SQLException;

/ **
* Returns the list of preferences to find a tourist
*
* @ Param Id pIdTurista tourists
* @ Throws SQLException
* @ Return List Search Preferences
* /
public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelTurista (
pIdTurista int) throws SQLException;

/ **
* Returns the list of preferences for research of a cultural
*
* @ Param pIdBeneCulturale ID of the cultural
* @ Throws SQLException
* @ Return list search preferences.
* /
public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelBC (
pIdBeneCulturale int) throws SQLException;

/ **
* Returns the list of preferences to find a resting spot
*
* @ Param identifier pIdPuntoDiRistoro a refreshment
* @ Throws SQLException
* @ Return list search preferences.
* /
public ArrayList <BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicercaDelPR (
pIdPuntoDiRistoro int) throws SQLException;

/ **
* Add a preference for a cultural
*
* @ Param pIdBeneCulturale ID of the cultural
* @ Param pIdPreferenzaDiRicerca ID PreferenzaDiRicerca
* @ Throws SQLException
* @ Param pPreferenza Search Preferences
* /
public boolean inserisciPreferenzaDiRicercaDelBC (int pIdBeneCulturale,
pIdPreferenzaDiRicerca int) throws SQLException;

/ **
* Add a search preference to a tourist
*
* @ Param Id pIdTurista tourists
* @ Param pIdPreferenzaDiRicerca ID PreferenzeDiRicerca
* @ Throws SQLException
* @ Param pPreferenza Search Preferences
* /
public boolean inserisciPreferenzaDiRicercaDelTurista (int pIdTurista,
pIdPreferenzaDiRicerca int) throws SQLException;

/ **
* Add a preference research to a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Param pIdPreferenzaDiRicerca ID PreferenzaDiRicerca
* @ Throws SQLException
* @ Param pPreferenza Search Preferences
* /
public boolean inserisciPreferenzaDiRicercaDelPR (int pIdPuntoDiRistoro,
pIdPreferenzaDiRicerca int) throws SQLException;

/ **
* Deletes a preference to find a Tourist
*
* @ Param Id pIdTurista tourists
* @ Param pIdPreferenza Search Preferences
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPreferenzaDiRicercaTurista (int pIdTurista,
pIdPreferenza int) throws SQLException;

/ **
* Deletes a preference for research of a cultural
*
* @ Param pIdPreferenzaDiRicerca Search Preferences
* @ Param pIdBeneCulturale ID of the cultural
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPreferenzaDiRicercaBC (int pIdBeneCulturale,
pIdPreferenzaDiRicerca int) throws SQLException;

/ **
* Deletes a preference to find a resting spot
*
* @ Param pIdPreferenza Search Preferences
* @ Param pIdPuntoDiistoro point identification Refreshments
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPreferenzaDiRicercaPR (int pIdPuntoDiistoro,
pIdPreferenza int) throws SQLException;

/ **
* Returns a list of all search preferences in the DB
*
* @ Throws SQLException
* @ Return List of search preferences in the DB
* /
<BeanPreferenzaDiRicerca> ottieniPreferenzeDiRicerca public ArrayList ()
throws SQLException;

)

