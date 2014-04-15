	
/ **
  * Interface that provides management services dell'advertisement
  * Operator agency.
  *
  * @ Author author
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
package com.trapan.spg.control.GestioneAdvertisement;

import java.rmi.RemoteException;
import java.util.ArrayList;

import com.trapan.spg.bean.BeanNews;

public interface IGestioneAdvetisementAgenzia extends IGestioneAdvertisement
(
/ **
* Method that inserts a new news system.
*
* @ Param Pnews Bean containing data news
* @ Throws RemoteException
* /
public boolean inserisciNews (BeanNews Pnews) throws RemoteException;
/ **
* Method which removes from the news system.
*
* @ Param ID pNewsID news be erased.
* @ Throws RemoteException
* /
public boolean cancellaNews (int pNewsID) throws RemoteException;
/ **
* Method amending the text of a news.
*
* @ Param Pnews Bean containing data news
* @ Throws RemoteException
* /
public boolean modificaNews (BeanNews Pnews) throws RemoteException;
/ **
* Returns the list of active news.
*
* @ Return ArrayList of ArrayList News
* @ Throws RemoteException
* /
<BeanNews> ottieniAllNews public ArrayList () throws RemoteException;
)