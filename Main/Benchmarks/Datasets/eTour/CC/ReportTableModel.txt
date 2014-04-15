/ * ReportTableModel.java
*
* 1.0
*
* 21/05/2007
*
* © 2007 eTour Project - Copyright by SE @ SA Lab - DMI - University of Salerno
* /
package unisa.gps.etour.gui.operatoreagenzia;

import java.util.Vector;
import javax.swing.table.AbstractTableModel;

import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.util.Punto3D;

public class extends ReportTableModel AbstractTableModel (

/ **
*
* /
private static final long serialVersionUID = 1L;
private static final String [] headers =
( "Name", "Description", "Address", "City", "Province");
private static final Class [] = columnClasses
(String.class, String.class, String.class, String.class, String.class);
<Object[]> private Vector data;

public ReportTableModel (BeanBeneCulturale [] bc, BeanPuntoDiRistoro [] pr)
(
<Object[]> data = new Vector ();
for (int i = 0; i <pr.length i + +)
(
Object [] new = new Object [5];
new [0] = pr [i]. getName ();
new [1] = pr [i]. getDescrizione ();
new [2] = pr [i]. getVar ();
new [3] = pr [i]. getCitta ();
new [4] = pr [i]. getProvincia ();

setValueAt (new, i);
)
for (int i = 0; i <bc.length i + +)
(
Object [] new = new Object [5];
new [0] = bc [i]. getName ();
new [1] = bc [i]. getDescrizione ();
new [2] = bc [i]. getVar ();
new [3] = bc [i]. getCitta ();
new [4] = bc [i]. getProvincia ();
setValueAt (new, pr.length + i);
)
)

public int getColumnCount () (
headers.length return;
)

public int GetRowCount () (
data.size return ();
)
public String getColumnName (int col) (
return headers [col];
)

public Object getValueAt (int row, int col) (
data.get return (row) [col];
)
public class getColumnClass (int col) (
return columnClasses [col];
)

public boolean isCellEditable (int row, int col) (
return false;
)
public void setValueAt (Object value, int row, int col) (
if (row> = GetRowCount ()) (
Object [] new = new Object [headers.length];
New [col] = value;
data.add (new);
)
else (
data.get (row) [col] = value;
)
)
public void setValueAt (Object [] value, int row) throws IllegalArgumentException (
if (value.length! = headers.length) (
System.out.println (value.length);
System.out.println (headers.length);
throw new IllegalArgumentException ();)
if (row> = GetRowCount ()) (
data.add (value);
)
else (
data.remove (row);
data.add (row, value);
)
)
) 