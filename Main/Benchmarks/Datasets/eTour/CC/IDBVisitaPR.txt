package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Interface for managing feedback related to a specific point
  * Refreshments
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBVisitaPR
(

/ **
* Add a visit to a refreshment
*
* @ Param PVIS visit to add
* @ Throws SQLException
* /
public boolean inserisciVisitaPR (BeanVisitaPR PVIS) throws SQLException;

/ **
* Modify a visit
*
* @ Param PVIS Visit to edit
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaVisitaPR (BeanVisitaPR PVIS) throws SQLException;

/ **
* Extract the list of visits to a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Throws SQLException
* @ Return List of visits
* /
<BeanVisitaPR> ottieniListaVisitaPR public ArrayList (int pIdPuntoDiRistoro)
throws SQLException;

/ **
* Extract a visit by a tourist at a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Param ID pIdTurista tourists
* @ Throws SQLException
* @ Return visit
* /
public BeanVisitaPR ottieniVisitaPR (pIdPuntoDiRistoro int, int pIdTurista)
throws SQLException;

/ **
* Extract the list of visits of a tourist
*
* @ Param ID pIdTurista tourists
* @ Return List of visits
* @ Throws SQLException
* /
<BeanVisitaPR> ottieniListaVisitaPRTurista public ArrayList (int pIdTurista)
throws SQLException;

) 