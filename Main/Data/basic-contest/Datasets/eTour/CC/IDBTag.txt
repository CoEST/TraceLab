package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanTag;

/ **
  * Interface for managing the database Tag
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBTag
(
/ **
* Add a tag
*
* @ Param ptagi Tag to add
* @ Throws SQLException
* /
public boolean inserisciTag (BeanTag ptagi) throws SQLException;

/ **
* Modify the data in a tag
*
* @ Param ptagi Tag to modify
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaTag (BeanTag ptagi) throws SQLException;

/ **
* Delete a tag from the database
*
* @ Param pIdTag ID Tag to be deleted
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaTag (int pIdTag) throws SQLException;

/ **
* Returns the list of tags in the database
*
* @ Throws SQLException
* @ Return List containing the tags
* /
<BeanTag> ottieniListaTag public ArrayList () throws SQLException;

/ **
* Returns a single tag
*
* @ Param pId ID tag
* @ Throws SQLException
* @ Return Tags
* /
public BeanTag ottieniTag (int pid) throws SQLException;

/ **
* Tag with immovable cultural
*
* @ Param ID pIdBeneCulturale of Cultural Heritage
* @ Param pIdTag ID tag
* @ Throws SQLException
* /
public boolean aggiungeTagBeneCulturale (pIdBeneCulturale int, int pIdTag)
throws SQLException;

/ **
* Tag to a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Param pIdTag ID tag
* @ Throws SQLException
* /
public boolean aggiungeTagPuntoDiRistoro (pIdPuntoDiRistoro int, int pIdTag)
throws SQLException;

/ **
* Returns the list of tags of a cultural
*
* @ Param ID pIdBeneCulturale of Cultural Heritage
* @ Throws SQLException
* @ Return list of tags
* /
<BeanTag> ottieniTagBeneCulturale public ArrayList (int pIdBeneCulturale)
throws SQLException;

/ **
* Returns a list of tags of a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Throws SQLException
* @ Return list of tags
* /
<BeanTag> ottieniTagPuntoDiRistoro public ArrayList (int pIdPuntoDiRistoro)
throws SQLException;

/ **
* Delete a tag to a cultural
*
* @ Param ID pIdBeneCulturale of Cultural Heritage
* @ Param pIdTag ID tag
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaTagBeneCulturale (pIdBeneCulturale int, int pIdTag)
throws SQLException;

/ **
* Delete a tag to a refreshment
*
* @ Param pIdPuntoDiRistoro ID
* @ Param pIdTag ID tag
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaTagPuntoDiRistoro (pIdPuntoDiRistoro int, int pIdTag)
throws SQLException;

) 