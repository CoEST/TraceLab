package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanNews;

/ **
  * Interface for the management of news in the Database
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBNews
(
/ **
* Add a news database
*
* @ Param Pnews News to add
* @ Throws SQLException
* /
public boolean inserisciNews (BeanNews Pnews) throws SQLException;

/ **
* Modify a news database
*
* @ Param Pnews News to change with the new data
* @ Throws SQLException
* @ Return True if there 'was a modified false otherwise
* /
public boolean modificaNews (BeanNews Pnews) throws SQLException;

/ **
* Delete a database from news
*
* @ Param ID pIdNews News to eliminate
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaNews (int pIdNews) throws SQLException;

/ **
* Returns the active news
*
* @ Throws SQLException
* @ Return list of active news
* /
<BeanNews> ottieniNews public ArrayList () throws SQLException;
)