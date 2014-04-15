	
/ *
* TAGTableModel.java
*
* 1.0
*
* 28/05/2007
*
* © 2007 eTour Project - Copyright by SE @ SA Lab - DMI - University of Salerno
* /
package unisa.gps.etour.gui.operatoreagenzia;

import java.util.Vector;
import javax.swing.table.AbstractTableModel;
import unisa.gps.etour.bean.BeanTag;
/ **
* <p>
* <B> Title: </ B> TagTableModel
* </ P>
* <p>
* <B> Description: </ B> TableModel for dynamic management of Table
* Within the section GestioneTag
* </ P>
*
* @ Author _Lello_
* @ Version 1.0
* /

public class extends TAGTableModel AbstractTableModel (


private static final long serialVersionUID = 1L;
private static final String [] headers =
( "Name", "Description");
private static final Class [] = columnClasses
(String.class, String.class);
<Object[]> private Vector data;

/ **
* Constructor for class TagTableModel
*
* @ Param BeanTag []
*
* /
public TAGTableModel (BeanTag [] tags)
(
<Object[]> data = new Vector ();
for (int i = 0; i <tag.length i + +)
(
Object [] new = new Object [10];
new [0] = tag [i]. getId ();
New [1] = tag [i]. getName ();
new [2] = tag [i]. getDescrizione ();

)
)

/ **
* Returns the number of columns
*
* /
public int getColumnCount () (
headers.length return;
)

/ **
* Returns the number of rows
*
* /
public int GetRowCount () (
data.size return ();
)

/ **
* Returns the column heading i_esima
*
* @ Param pCol
*
* /
public String getColumnName (int pCol) (
return headers [pCol];
)

/ **
* Returns the coordinates given by the pair of row, column
*
* @ Param pCol
* @ Param pRow
*
* /
public Object getValueAt (int prow, int pCol) (
return data.get (pRow) [pCol];
)

/ **
* Returns the column pCol
*
* @ Param pCol
*
* /
public class getColumnClass (int pCol) (
return columnClasses [pCol];
)

/ **
* Always returns false because the cells in the table are not editable
*
* @ Param pCol
* @ Param pRow
*
* @ Return false
*
* /
public boolean isCellEditable (int row, int col) (
return false;
)

/ **
* This method is empty.
* Can not be included an element within a cell
*
* @ Deprecated
*
* /
public void setValueAt (Object value, int row, int col) (

)

) 