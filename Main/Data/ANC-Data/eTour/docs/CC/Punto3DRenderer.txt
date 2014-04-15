package unisa.gps.etour.gui.operatoreagenzia.tables;

import java.awt.Component;
import javax.swing.JLabel;
import javax.swing.JTable;
import javax.swing.SwingConstants;
import javax.swing.table.TableCellRenderer;
import unisa.gps.etour.util.Punto3D;

/ **
  * <b> Punto3DRenderer </ b>
  <p> * This class creates a custom renderer for
  * Objects of type Punto3D. </ P>
  *
  * @ See javax.swing.table.TableCellRenderer
  * @ See unisa.gps.etour.util.Punto3D
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  *
  * /
public class Punto3DRenderer implements TableCellRenderer
(
/ **
* Method that returns the custom component for the
* Display of the data contained in the cell of a table.
*
* @ Param pTable JTable - the table.
* @ Param Object pValue - the data.
* @ Param boolean pSelected --
* <ul>
* <li> <i> True </ i> if the cell is selected.
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
Object pValue, boolean pSelected, boolean pHasFocus, int prow,
int pColumn)
(
if ((pValue instanceof Punto3D))
(
throw new IllegalArgumentException ( "Value Cella unexpected.");
)
Pointe Punto3D = (Punto3D) pValue;
APoint.getLatitudine String point = () + ";"
APoint.getLongitudine + () + "" + aPoint.getAltitudine ();
JLabel aLabel = new JLabel (point, SwingConstants.CENTER);
if (pSelected)
(
aLabel.setForeground (pTable.getSelectionForeground ());
aLabel.setBackground (pTable.getSelectionBackground ());
)
aLabel return;

)

)
