package unisa.gps.etour.gui.operatoreagenzia.tables;

import java.awt.Component;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JTable;
import javax.swing.table.DefaultTableCellRenderer;
import unisa.gps.etour.gui.operatoreagenzia.Home;

/ **
  * <b> MediaVotiRenderer </ b>
  * <p>
  * This class creates a custom renderer for the average ratings of a
  * Site. </ B>
  *
  * @ See javax.swing.table.DefaultTableRenderer;
  * @ See javax.swing.table.TableCellRenderer;
  * @ Version 1.0
  * @ Author Mario Gallo
  * /
public class extends MediaVotiRenderer DefaultTableCellRenderer
(

/ **
* Method that returns the custom component for the
* Display of the data contained in the cell of a table.
*
* @ Param pTable JTable - the table.
* @ Param Object pValue - the data.
* @ Param boolean pSelected --
* <ul>
* <li> <i> True </ i> if the selected cell.
* <li> <i> False </ i> otherwise.
* </ Ul>
* @ Param boolean pHasFocus --
* <ul>
* <li> <i> True </ i> if the cell has the focus.
* <li> <i> False </ i> otherwise.
* </ Ul>
* @ Param int pRow - the line number.
* @ Param int pColumn - the column number.
* @ Return Component - the component that customizes render the cell.
* @ Throws IllegalArgumentException - if the value of the cell can not
* Be rendered by this renderer.
* /
public Component getTableCellRendererComponent (JTable pTable,
Object pValue, boolean pSelected, boolean pFocus, int prow,
pColumn int) throws IllegalArgumentException
(
if ((pValue instanceof Double | | pValue instanceof Integer))
(
throw new IllegalArgumentException ( "Value cell unexpected.");
)
double rating = 0;
if (pValue instanceof Double)
(
Rate = (Double) pValue;
)
else
(
Rating + = (Integer) pValue;
)

JLabel aLabel = new JLabel ("");
aLabel.setHorizontalAlignment (JLabel.CENTER);

if (rating> 4)
(
aLabel.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella5.gif ")));
)
else if (rating <= 4 & & rating> 3)
(
aLabel.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella4.gif ")));
)
else if (rating <= 3 & & rating> 2)
(
aLabel.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella3.gif ")));
)
else if (grade <= 2 & & rating> 1)
(
aLabel.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella2.gif ")));
)
else
(
aLabel.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella1.gif ")));
)
aLabel return;

)

) 