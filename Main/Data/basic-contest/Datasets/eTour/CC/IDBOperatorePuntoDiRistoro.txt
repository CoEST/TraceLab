package unisa.gps.etour.repository;

import java.sql.SQLException;

import unisa.gps.etour.bean.BeanOperatorePuntoDiRistoro;

/ **
  * Interface for the operator to the point of comfort in the database
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBOperatorePuntoDiRistoro
(
/ **
* Adds an operator refreshment
*
* @ Param popera Additional operating
* @ Throws SQLException
* /
public boolean inserisciOperatorePuntoDiRistoro (
BeanOperatorePuntoDiRistoro popera) throws SQLException;

/ **
* Modify an operator in the database
*
* @ Param popera New data Operator
* @ Throws SQLException
* @ Return True if there 'was a modified false otherwise
* /
public boolean modificaOperatorePuntoDiRistoro (
BeanOperatorePuntoDiRistoro popera) throws SQLException;

/ **
* Delete an operator
*
* @ Param pIdOperatore Operator ID to delete
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaOperatorePuntoDiRistoro (int pIdOperatore)
throws SQLException;

/ **
Returns data operator
*
* @ Param pIdOperatore Operation ID
* @ Throws SQLException
* @ Return Operator refreshment
* /
public BeanOperatorePuntoDiRistoro ottieniOperatorePuntoDiRistoro (
pIdOperatore int) throws SQLException;
) 