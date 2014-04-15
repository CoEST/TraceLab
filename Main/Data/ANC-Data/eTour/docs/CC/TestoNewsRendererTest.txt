/ **
  * Test case for class TestoNewsRenderer
  *
  * @ Author Mario Gallo
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
package unisa.gps.etour.gui.operatoreagenzia.tables.test;

import java.util.Date;
import javax.swing.JTable;
import javax.swing.JTextArea;
import unisa.gps.etour.bean.BeanNews;
import unisa.gps.etour.gui.operatoreagenzia.tables.NewsTableModel;
import unisa.gps.etour.gui.operatoreagenzia.tables.TestoNewsRenderer;
import junit.framework.TestCase;

public class TestCase extends TestoNewsRendererTest
(

Private TestoNewsRenderer renderer;
Private BeanNews aNewsAttiva;
Private BeanNews aNewsScaduta;
private JTable aTable;

public TestoNewsRendererTest ()
(
super ();
renderer = new TestoNewsRenderer ();
aNewsAttiva = new BeanNews ( "Here's a news active", new Date (),
new Date (120, 1, 1), 5, 0);
aNewsScaduta = new BeanNews ( "Here's a news Expired", new Date (),
new Date (), 5, 0);
aTable = new JTable (new NewsTableModel ());
)

/ *
* Verify the behavior of the method with the correct parameters.
* /
public void testGetTableCellRendererParametriCorretti ()
(
NewsTableModel aModele = (NewsTableModel) aTable.getModel ();
aModel.insertNews (aNewsAttiva);
aModel.insertNews (aNewsScaduta);

/ / Test the renderer with a news active.
JTextArea aare = (JTextArea) renderer.getTableCellRendererComponent (
aTable, "Here's a news active", true, true, 0, 0);
assertEquals (aNewsAttiva.getNews (), aArea.getText ());

/ / Test the renderer with a news expired.
aare = (JTextArea) renderer.getTableCellRendererComponent (aTable,
"Here's a news Expired", true, true, 0, 0);
assertEquals (aNewsScaduta.getNews (), aArea.getText ());

)

/ *
* Verification Compor the table with a table without NewsTableModel
* Associated.
* /
public void testGetTabelCellRendererNoNewsModel ()
(

JTable anotherTable = new JTable ();
TRY
(
renderer.getTableCellRendererComponent (anotherTable,
"Here's a news", true, true, 0, 0);
fail ( "Should be thrown.");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify the behavior of the method with a parameter to null.
* /
public void testGetTableCellRendererParametroNull ()
(
TRY
(
renderer.getTableCellRendererComponent (aTable, null, true, true, 0,
0);
fail ( "Should be thrown.");
)
catch (IllegalArgumentException success)
(
)
)

/ *
* Verify the behavior of the method with a data type unexpected.
* /
public void testGetTableCellRendererTipoInatteso ()
(
TRY
(
renderer
. getTableCellRendererComponent (aTable, 12, true, true, 0, 0);
fail ( "Should be thrown.");
)
catch (IllegalArgumentException success)
(
)

)

)
