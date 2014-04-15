package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanTurista;

/ **
  * Interface for the management of tourists in the database
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBTurista
(
/ **
* Add a tourist
*
* @ Param to add pTurista Tourist
* @ Throws SQLException
* /
public boolean inserisciTurista (BeanTurista pTurista) throws SQLException;

/ **
* Modify a tourist
*
* @ Param to change pTurista Tourist
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaTurista (BeanTurista pTurista) throws SQLException;

/ **
* Delete a tourist from the database
*
* @ Param pIdTurista Identificatie Tourist delete
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean delete (int pIdTurista) throws SQLException;

/ **
* Returns the data of the Tourist
*
* @ Param pUsername Username tourists
* @ Throws SQLException
* @ Return Information about tourist
* /
public BeanTurista ottieniTurista (String pUsername) throws SQLException;

/ **
* Attach a cultural tourists preferred
*
* @ Param ID pIdTurista tourists
* @ Param pIdBeneCulturale ID of the cultural
* @ Throws SQLException
* /
public boolean inserisciBeneCulturalePreferito (int pIdTurista,
pIdBeneCulturale int) throws SQLException;

/ **
* Attach a point of catering to the tourist favorite
*
* @ Param ID pIdTurista tourists
* @ Param pIdPuntoDiRistoro ID of the cultural
* @ Throws SQLException
* /
public boolean inserisciPuntoDiRistoroPreferito (int pIdTurista,
pIdPuntoDiRistoro int) throws SQLException;

/ **
* Delete a cultural favorite
*
* @ Param ID pIdTurista tourists
* @ Param pIdBeneCulturale ID of the cultural
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean cancellaBeneCulturalePreferito (int pIdTurista,
pIdBeneCulturale int) throws SQLException;

/ **
* Delete a favorite resting spot
*
* @ Param ID pIdTurista tourists
* @ Param pIdPuntoDiRistoro ID of the cultural
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaPuntoDiRistoroPreferito (int pIdTurista,
pIdPuntoDiRistoro int) throws SQLException;

/ **
* Returns an ArrayList of tourists who have a username like that
* Given as argument
*
* @ Param pUsernameTurista Usrername tourists to search
* @ Throws SQLException
* @ Return data for Tourists
* /
public ArrayList <BeanTurista> ottieniTuristi (String pUsernameTurista)
throws SQLException;

/ **
* Returns the list of tourists turned on or off
*
* @ Param select pact True False those tourists turned off
* @ Return data for Tourists
* @ Throws SQLException
* /
public ArrayList <BeanTurista> ottieniTuristi (boolean condition)
throws SQLException;

/ **
* Returns the data of the tourist with ID equal to that given in
* Input
*
* @ Param ID pIdTurista tourists to find
* @ Return Tourists with id equal to the input, null if there is
* @ Throws SQLException
* /
public BeanTurista ottieniTurista (int pIdTurista) throws SQLException;

/ **
* Returns the list of cultural favorites from a particular
* Tourist
*
* @ Param ID pIdTurista tourists to find
* @ Return List of Cultural Heritage Favorites
* @ Throws SQLException
* /
<Integer> ottieniBeniCulturaliPreferiti public ArrayList (int pIdTurista)
throws SQLException;

/ **
* Returns a list of favorite resting spot by a particular
* Tourist
*
* @ Param ID pIdTurista tourists to find
* @ Return List of Refreshment Favorites
* @ Throws SQLException
* /
<Integer> ottieniPuntoDiRistoroPreferiti public ArrayList (int pIdTurista)
throws SQLException;

) 