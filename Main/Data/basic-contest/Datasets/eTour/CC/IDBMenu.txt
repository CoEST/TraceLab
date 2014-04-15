package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanMenu;


/ **
  * Interface for managing the menu in the database
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBMenu
(
/ **
* Adds a menu in the database
*
* @ Param pMenu menu to add
* @ Throws SQLException
* /
public boolean inserisciMenu (BeanMenu pMenu) throws SQLException;

/ **
* Modify a menu in the database
*
* @ Param pMenu Contains the data to change
* @ Throws SQLException
* @ Return True if there 'was a modified false otherwise
* /
public boolean modificaMenu (BeanMenu pMenu) throws SQLException;

/ **
* Delete a menu from database
*
* @ Param ID pIdMenu menu to delete
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaMenu (int pIdMenu) throws SQLException;

/ **
* Returns the menu of the day of a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Param pGiorno Day of the week in which the menu
* Daily
* @ Throws SQLException
* @ Return Day menu de Refreshment
* /
public BeanMenu ottieniMenuDelGiorno (int pIdPuntoDiRistoro, String pGiorno)
throws SQLException;

/ **
* Returns a list of the menu of a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshment
* @ Throws SQLException
* @ Return List of menus
* /
<BeanMenu> ottieniMenu public ArrayList (int pIdPuntoDiRistoro)
throws SQLException;
) 