package unisa.gps.etour.repository;

import java.sql.SQLException;

import unisa.gps.etour.bean.BeanOperatoreAgenzia;

/ **
  * Interface for managing the database OperatoreAgenzia
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBOperatoreAgenzia
(
/ **
* Returns the data Operator Agency with ID equal to that given in
* Input
*
* @ Param pUsername Username dell'OperatoreAgenzia to find
* @ Return OperatoreAGenzia with id equal to the input, null if there is
* @ Throws SQLException
* /
public BeanOperatoreAgenzia ottieniOperatoreAgenzia (String pUsername) throws SQLException;

/ **
* Returns the data Operator Agency with ID equal to that given in
* Input
*
* @ Param pUsername Username dell'OperatoreAgenzia to find
* @ Return OperatoreAGenzia with id equal to the input, null if there is
* @ Throws SQLException
* /
public boolean modificaPassword (BeanOperatoreAgenzia poa) throws SQLException;

)