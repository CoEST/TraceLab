package unisa.gps.etour.repository;

import java.sql.SQLException;

import unisa.gps.etour.bean.BeanPreferenzeGeneriche;

/ **
  * Interface for handling general preferences in database
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBPreferenzeGeneriche
(
/ **
* Add a preference General
*
* @ Param pPreferenza preference to be added
* @ Throws SQLException
* /
public boolean inserisciPreferenzaGenenerica (
BeanPreferenzeGeneriche pPreferenza) throws SQLException;

/ **
* Edit a general preference
*
* @ Param pPreferenza preference to change
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaPreferenzaGenerica (
BeanPreferenzeGeneriche pPreferenza) throws SQLException;

/ **
* Delete a general preference
*
* @ Param ID pIdPreferenza preference to delete
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPreferenzaGenerica (int pIdPreferenza)
throws SQLException;

/ **
* Returns the generic preference for tourists
*
* @ Param Id pIdTurista tourists
* @ Throws SQLException
* @ Return generic preference
* /
public BeanPreferenzeGeneriche ottieniPreferenzaGenerica (int pIdTurista)
throws SQLException;
) 