/ **
  * Stubs used for testing the class GestioneAdvertisementAgenzia
  *
  * @ Author Fabio Palladino
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
package unisa.gps.etour.control.GestioneAdvertisement.test.stubs;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanNews;
import unisa.gps.etour.repository.IDBNews;
import unisa.gps.etour.util.CostantiGlobali;

public class DBNews implements IDBNews
(
/ ** Static field that stores the number of test * /
private static int NUM_TEST = 0;

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBNews # cancellaNews (int)
* /
public boolean cancellaNews (int pIdNews) throws SQLException
(
if (NUM_TEST == 1) (
return true;
) else if (NUM_TEST == 2) (
throw new SQLException ();
)
return true;
)

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBNews # inserisciNews (unisa.gps.etour.bean.BeanNews)
* /
public boolean inserisciNews (BeanNews Pnews) throws SQLException
(
if (NUM_TEST == 7) (
return false;
) else (
return true;
)
)

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBNews # modificaNews (unisa.gps.etour.bean.BeanNews)
* /
public boolean modificaNews (BeanNews Pnews) throws SQLException
(
return true;
)

/ * (Non-Javadoc)
* @ See unisa.gps.etour.repository.IDBNews # ottieniNews ()
* /
<BeanNews> ottieniNews public ArrayList () throws SQLException
(
<BeanNews> ToReturn ArrayList <BeanNews> = new ArrayList ();

if (NUM_TEST == 5) (
for (int i = 1; i <= CostantiGlobali.MAX_NEWS_ATTIVE i + +) (
toReturn.add (new BeanNews ());
)
toReturn return;
) else (
toReturn.add (new BeanNews ());
toReturn return;
)
)

/ **
* Set the number of tests in progress
*
* @ Param num_test
* /
public static void setNUM_TEST (int num_test)
(
NUM_TEST = num_test;
)

) 