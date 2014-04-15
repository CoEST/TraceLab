/ **
  * Class tests for NewsTableModel
  *
  * @ Author Mario Gallo
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
package unisa.gps.etour.gui.operatoreagenzia.tables.test;

import java.util.ArrayList;
import java.util.Date;

import unisa.gps.etour.bean.BeanNews;
import unisa.gps.etour.gui.operatoreagenzia.tables.NewsTableModel;
import junit.framework.TestCase;

public class TestCase extends NewsTableModelTest
(

private NewsTableModel TableModel;
Private BeanNews aNews;
Private BeanNews aNewsModificata;

public NewsTableModelTest (String pName)
(
super (pName);
aNews = new BeanNews ( "An example of news", new Date (), new Date (), 2,1);
aNewsModificata = new BeanNews ( "A news amended sample", new Date (), new Date (), 3,1);
)

protected void setUp () throws Exception
(
super.setUp ();
NewsTableModel = new TableModel ();
)

/ *
* Verify the behavior of the manufacturer with an ArrayList of BeanNews.
* /
public void testCostruttoreConArrayList ()
(
<BeanNews> ArrayList test = new ArrayList <BeanNews> ();
for (int i = 0; i <10; i + +)
(
test.add (new BeanNews ( "text" + i, new Date (), new Date (), 5, i));
)
NewsTableModel = new TableModel (test);
for (int i = 0; i <10; i + +)
(
assertSame (test.get (i). getId (), tableModel.getID (i));
)

)

/ *
* Verify the manufacturer with an ArrayList Compor zero.
* /
public void testCostruttoreConArrayListNull ()
(
NewsTableModel = new TableModel (null);
)

/ *
* Verify the behavior of the manufacturer with an empty ArrayList.
* /
public void testCostruttoreConArrayListVuoto ()
(
NewsTableModel = new TableModel (<BeanNews> new ArrayList ());
)

/ *
* Verify the behavior of the method with the correct parameters.
* /
public void testGetValueAtParametriCorretti ()
(
/ / Put bean in two model test.
tableModel.insertNews (aNews);
tableModel.insertNews (aNewsModificata);

/ / Verify the data entered.
assertSame (aNews.getNews (), tableModel.getValueAt (0, 0));
assertSame (aNews.getPriorita (), tableModel.getValueAt (0, 1));
assertSame (aNewsModificata.getNews (), tableModel.getValueAt (1, 0));
assertSame (aNewsModificata.getPriorita (), tableModel.getValueAt (1, 1));
)

/ *
* Verify Compor the method with an index row fold.
* /
public void testGetValueAtRigaSballata ()
(
TRY
(
tableModel.getValueAt (12, 0);
fail ( "Should be thrown");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify Compor of the method with a column index busted.
* /
public void testGetValueAtColonnaSballata ()
(
TRY
(
tableModel.getValueAt (0, -121334);
fail ( "Should be thrown");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify Compor method with proper parameter.
* /
public void testInsertNewsParametroCorretto ()
(
tableModel.insertNews (aNews);
assertSame (aNews.getId (), tableModel.getID (0));
)

/ *
* Verify Compor method with parameter to null
* /
public void testInsertNewsParametroNull ()
(
TRY
(
tableModel.insertNews (null);
fail ( "Should be thrown");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify Compor method with proper parameter.
* /
public void testUpdateNewsParametroCorretto ()
(
tableModel.insertNews (aNews);
tableModel.updateNews (aNewsModificata);
assertSame (aNewsModificata.getNews (), tableModel.getValueAt (0, 0));
assertSame (aNewsModificata.getPriorita (), tableModel.getValueAt (0, 1));
assertSame (aNewsModificata.getId (), tableModel.getID (0));
)

/ *
* Verify Compor method with parameter to null
* /
public void testUpdateNewsParametroNull ()
(

TRY
(
tableModel.updateNews (null);
fail ( "Should be thrown");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify Compor method with proper parameter.
* /
public void testRemoveNewsParametroCorretto ()
(
tableModel.insertNews (aNews);
assertSame (aNews.getId (), tableModel.removeNews (0));
)

/ *
* Verify Compor of the method with row index busted.
* /
public void testRemoveNewsRigaSballata ()
(
TRY
(
tableModel.removeNews (-1231);
fail ( "Should be thrown");
)
catch (IllegalArgumentException success) ()
)

) 