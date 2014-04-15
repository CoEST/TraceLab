	
/ *
  * SitoTableModel.java
  *
  * 1.0
  *
  * 21/05/2007
  *
  * © 2007 eTour Project - Copyright by SE @ SA Lab - DMI - University of Salerno
  * /
Handheld Package;

import Bean.BeanPuntoDiRistoro;
import javax.swing.table.AbstractTableModel;
import Bean .*;
import Util.Punto3D;

/ **
  * <b> SitoTableModel </ b>
  * Serves as a data container <p> of cultural or refreshment areas that need
  * Be displayed in a JTable. </ P>
  * @ See javax.swing.table.AbstractTableModel
  * @ See javax.swing.JTable
  * @ See unisa.gps.etour.bean.BeanBeneCulturale
  * @ See unisa.gps.etour.bean.BeanPuntoDiRistoro
  * @ Version 1.0
  * @ Author Raphael Landi
  * /

public class extends SitoTableModel AbstractTableModel (
     String [] columnNames = ( "Name",
     "City", "Distance");
     Object [] [] cells;
     Punto3D posizioneSito;
     Punto3D myLocation;
    
     SitoTableModel (BeanPuntoDiRistoro [] pr, Punto3D myLocation) (
         super ();
         cells = new Object [pr.length] [3] / / First value = second rows = columns
         for (int i = 0; i <pr.length i + +) (
             Cells [i] [0] = pr [i]. getName ();
             Cells [i] [1] = pr [i]. getCitta ();
            
            
            
            
         )
     )
    
     SitoTableModel (BeanBeneCulturale [] bc, Punto3D myLocation) (
         super ();
         cells = new Object [bc.length] [3] / / First value = second rows = columns
         for (int i = 0; i <bc.length i + +) (
             Cells [i] [0] = bc [i]. getName ();
             Cells [i] [1] = bc [i]. getCitta ();
            
         )
     )
    
     public int GetRowCount () (
         cells.length return;
     )
    
     public int getColumnCount () (
         columnNames.length return;
     )
    
     public Object getValueAt (int r, int c) (
         if (c <columnNames.length - 1)
             return cells [r] [c];
         else (
             double value = miaPosizione.distanza (posizioneSito);
             return new Double (value);
         )
        
     )
    
     public String getColumnName (int c) (
         return columnNames [c];
     )
)



