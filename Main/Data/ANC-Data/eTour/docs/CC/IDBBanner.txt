package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanBanner;

/ **
  * Interface for managing the banner on the database
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

public interface IDBBanner
(
/ **
* Add a banner in the database
*
* @ Param pBanner bean containing the information of the banner
* @ Throws SQLException
* /
public boolean inserisciBanner (BeanBanner pBanner) throws SQLException;

/ **
* Modify the contents of the advertisement, and returns the contents before
* Edit
*
* @ Param pBanner Bean that contains the new information of the banner
* @ Return True if there 'was a modified false otherwise
* @ Throws SQLException
* /
public boolean modificaBanner (BeanBanner pBanner) throws SQLException;

/ **
* Delete a banner from the database and returns
*
* @ Param pIdBanner ID BeanBanner
* @ Return True if and 'was deleted false otherwise
* @ Throws SQLException
* /
public boolean cancellaBanner (int pIdBanner) throws SQLException;

/ **
* Returns a list of banners for a refreshment point, if the id of
* Refreshment and 'equal to -1 will' return the complete list
* Banners
*
* @ Param Id pIdPuntoDiRistoro of refreshment point from which to obtain the list
* Banner
* @ Return list of banners linked to Refreshment
* @ Throws SQLException
* /
<BeanBanner> ottieniBanner public ArrayList (int pIdPuntoDiRistoro)
throws SQLException;

/ **
* Method which returns a banner given its id
*
* @ Param ID pIdBanner the banner to return
* @ Return Banner found in the database, null if there is' match
* @ Throws SQLException
* /
public BeanBanner ottieniBannerDaID (int pIdBanner) throws SQLException;
)
