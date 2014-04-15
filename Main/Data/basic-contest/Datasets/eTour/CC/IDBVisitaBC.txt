package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanVisitaBC;

/ **
  * Interface for handling feedback on a given asset
  * Cultural
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBVisitaBC
(

/ **
* Inserts a visit
*
* @ Param PVIS Visit to insert
* @ Throws SQLException
* /
public boolean inserisciVisitaBC (BeanVisitaBC PVIS) throws SQLException;

/ **
* Modify a visit
*
* @ Param PVIS Visit to edit
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaVisitaBC (BeanVisitaBC PVIS) throws SQLException;

/ **
* Extract the list of visits to a cultural
*
* @ Param pIdBeneCulturale ID of the cultural
* @ Throws SQLException
* @ Return list of visits of the cultural
* /
<BeanVisitaBC> ottieniListaVisitaBC public ArrayList (int pIdBeneCulturale)
throws SQLException;

/ **
* Extract the list of cultural visited by a tourist
*
* @ Param ID pIdTurista tourists
* @ Throws SQLException
* @ Return ArrayList of all feedback issued by a tourist for a
* Specified cultural
* /
<BeanVisitaBC> ottieniListaVisitaBCTurista public ArrayList (int pIdTurista)
throws SQLException;

/ **
* Extract a visit by a tourist to a cultural
*
* @ Param pIdBeneCulturale ID of the cultural
* @ Param ID pIdTurista tourists
* @ Throws SQLException
* @ Return visit
* /
public BeanVisitaBC ottieniVisitaBC (pIdBeneCulturale int, int pIdTurista)
throws SQLException;
)
