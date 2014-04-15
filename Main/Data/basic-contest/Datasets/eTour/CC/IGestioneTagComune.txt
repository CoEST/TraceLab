	
package unisa.gps.etour.control.GestioneTag;

import java.rmi.Remote;
import java.rmi.RemoteException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanTag;

/ **
  * Common interface for managing Tag
  *
  * @ Author Joseph Morelli
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface extends Remote IGestioneTagComune
(

/ **
* Method which returns all the tags stored in the database
*
* @ Return an ArrayList of BeanTag
* @ Throws RemoteException Exception Remote
* /
<BeanTag> ottieniTags public ArrayList () throws RemoteException;

)