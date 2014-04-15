/ *
  * FeedbackTableModel.java
  *
  * 1.0
  *
  * 21/05/2007
  *
  * 2007 eTour Project - Copyright by SE @ SA Lab - DMI University of Salerno
  * /
package unisa.gps.etour.gui.operatoreagenzia.tables;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;

table .*;

import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;



/ **
  * <b> FeedbackTableModel </ b>
  * <p> 'Container model of data for feedback received
  * To be a cultural or a refreshment. </ P>
  *
  * @ See javax.swing.table.AbstractTableModel
  * @ See unisa.gps.etour.bean.BeanVisitaBC
  * @ See unisa.gps.etour.bean.BeanVisitaPR
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  * /
public class extends FeedBackTableModel AbstractTableModel
(

private static final long serialVersionUID = 1L;
private static final String [] headers = ( "rating", "Comment",
"Release Date", "Issued by");
private static final Class [] (columnClasses = Integer.class, String.class,
Date.class, String.class);
<Object[]> private ArrayList data;


/ **
* Default Constructor. We only provide the model but not
* Loads no data in it.
*
* /
public FeedBackTableModel ()
(
<Object[]> data = new ArrayList ();
)

/ **
* Constructor that takes as input an ArrayList of BeanVisitaBC or
* BeanVisitaPR and copies of the data within the model
* Preparing for display.
*
* @ Param PVIS java.util.ArrayList - the ArrayList of bean.
* /
public FeedBackTableModel (HashMap <?, String> pDatiFeedback)
(
this ();
if (pDatiFeedback == null | | pDatiFeedback.size () == 0)
(
return;
)
Iterator <?> Iter = pDatiFeedback.keySet (). Iterator ();
while (itera.hasNext ())
(
Object current = itera.next ();
if (current instanceof BeanVisitaBC)
(
insertVisitaBC ((BeanVisitaBC) current pDatiFeedback.get (current));
)
else if (current instanceof BeanVisitaPR)
(
insertVisitaPR ((BeanVisitaPR) current pDatiFeedback.get (current));
)
)
)

/ **
* Returns the number of columns provided by the model.
*
* @ Return int - the number of columns.
*
* /
public int getColumnCount ()
(
headers.length return;
)

/ **
* Returns the number of rows currently in the model.
*
* @ Return int - the number of rows.
*
* /
public int GetRowCount ()
(
data.size return ();
)


/ **
* Returns the column name from the index provided.
*
* @ Return String - the name of the column.
* @ Exception IllegalArgumentException - if the column index is not provided
* In the model.
*
* /
public String getColumnName (int pColumn) throws IllegalArgumentException
(
if (pColumn> = getColumnCount () | | pColumn <0)
(
throw new IllegalArgumentException (
"The column index is not provided in the model.");
)
return headers [pColumn];
)

/ **
* Returns the object in the model to the row and column provided.
*
* @ Param pRow - the line number.
* @ Param pColumn - the column number.
* @ Return Object - the object contained in the selected cell.
* @ Exception IllegalArgumentException - if the index or the row or column not provided
* Are present in the model.
*
* /
public Object getValueAt (pRow int, int pColumn) throws IllegalArgumentException
(
if (pRow> = GetRowCount () | | pRow <0)
(
throw new IllegalArgumentException ( "The row index is not provided in the model.");
)
if (pColumn> = getColumnCount () | | pColumn <0)
(
throw new IllegalArgumentException ( "The column index is not provided in the model.");
)
return data.get (pRow) [pColumn];
)

/ **
* Returns the class of objects in the column of which is provided in the index.
*
* @ Param pColumn - the column number.
* @ Return Class - the class of objects of the selected column.
* @ Exception IllegalArgumentException - if the index column does not pro
* In the model.
*
* /
public class getColumnClass (int pColumn) throws IllegalArgumentException
(
if (pColumn> = getColumnCount () | | pColumn <0)
(
throw new IllegalArgumentException ( "The column index is not provided in the model.");
)
return columnClasses [pColumn];
)

/ **
* Returns whether the selected cell editable.
*
* @ Return boolean - true if the cell editable
* False otherwise
* @ Param pRow - the line number.
* @ Param pColumn - the column number.
* @ Exception IllegalArgumentException - if the index or the row or column are not provided
* In the model.
*
* /
public boolean isCellEditable (pRow int, int pColumn) throws IllegalArgumentException
(
return false;
)

/ **
* The method is inherited by setValueAt TableModel does not need to SitoTableModel
* Because it provided the possibility to change a single cell.
* @ Deprecated
*
* /
public void setValueAt (Object value, int row, int col)
(

)

/ **
*
* Enter data on the feedback received from a cultural
* In the model from Bean.
*
* @ Param BeanVisitaBC PBC - the bean that contains the feedback from
* Cultural.
* @ Throws IllegalArgumentException - if the parameters supplied input not
* Are valid.
*
* /
public void insertVisitaBC (BeanVisitaBC pVisitaBC, String pUsername) throws IllegalArgumentException
(
if (pVisitaBC == null | | pUsername == null | | pUsername.equals (""))
(
throw new IllegalArgumentException ( "invalid parameters supplied input.");
)
Object [] aRow = new Object [6];
aRow [0] = pVisitaBC.getVoto ();
aRow [1] = pVisitaBC.getCommento ();
aRow [2] = pVisitaBC.getDataVisita ();
aRow [3] = pUsername;
aRow [4] = pVisitaBC.getIdBeneCulturale ();
aRow [5] = pVisitaBC.getIdTurista ();
data.add (aRow);
)

/ **
*
* Enter data on the feedback received from a refreshment
* In the model from Bean.
*
* @ Param BeanVisitaPR PBC - the bean that contains the feedback from
* Refreshment.
* @ Throws IllegalArgumentException - if the parameters supplied input not
* Are valid.
*
* /
public void insertVisitaPR (BeanVisitaPR pVisitaPR, String pUsername) throws IllegalArgumentException
(
if (pVisitaPR == null | | pUsername == null | | pUsername.equals (""))
(
throw new IllegalArgumentException ( "invalid parameters supplied input.");
)
Object [] aRow = new Object [6];
aRow [0] = pVisitaPR.getVoto ();
aRow [1] = pVisitaPR.getCommento ();
aRow [2] = pVisitaPR.getDataVisita ();
aRow [3] = pUsername;
aRow [4] = pVisitaPR.getIdPuntoDiRistoro ();
aRow [5] = pVisitaPR.getIdTurista ();
data.add (aRow);
)
/ **
*
* Updates the comment feedback contained in the table row selected.
*
PNuovoCommento * @ param String - the new comment.
* @ Param pRow - the row to update.
* @ Throws IllegalArgumentException - if <ul> <li> the row index is not present in the model.
* <li> New comment supplied input is zero. </ Ul>
* /
public void modificaCommento (pNuovoCommento String, int pRow) throws IllegalArgumentException
(
if (pRow> = GetRowCount () | | pRow <0)
(
throw new IllegalArgumentException ( "The row index is not provided in the model.");
)
if (null == pNuovoCommento)
(
throw new IllegalArgumentException ( "The new comment can not be null.");
)
data.get (pRow) [1] = pNuovoCommento;
fireTableDataChanged ();
)

/ **
*
* Returns the id of the row receive feedback provided on input.
*
* @ Param pRow int - the row number.
* @ Return int [] - the id of the feedback.
* @ Throws IllegalArgumentException - if the row index does not pro
* In the model.
* /
public int [] getIDFeedback (int pRow) throws IllegalArgumentException
(
if (pRow> = GetRowCount () | | pRow <0)
(
throw new IllegalArgumentException ( "The row index is not provided in the model.");
)
int [] ids = new int [2];
ids [0] = (Integer) data.get (pRow) [4];
ids [1] = (Integer) data.get (pRow) [5];
return ids;
)

/ **
*
* Returns the id of feedback to line number provided as input and removes it from the model.
*
* @ Param pRow int - the row number.
* @ Return int [] - the id of the feedback.
* @ Exception IllegalArgumentException - if the row index does not pro
* In the model.
*
* /
public int [] removeFeedback (int pRow) throws IllegalArgumentException
(
int [] ids = getIDFeedback (pRow);
data.remove (pRow);
return ids;
)
)
