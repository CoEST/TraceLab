package unisa.gps.etour.control.GestioneAdvertisement;

import java.rmi.Remote;
import java.rmi.RemoteException;
import java.util.HashMap;

import javax.swing.ImageIcon;

import unisa.gps.etour.bean.BeanBanner;

/ **
  * Interface General Manager of Banner and news.
  *
  * @ Author Fabio Palladino
  * @ Version 0.1
  *
  * 2007 eTour Project - Copyright by SE @ SA Lab DMI University of Salerno
  * /
public interface extends Remote IGestioneAdvertisement
(
/ **
* Inserts a new banner.
*
* @ Param pBanner Bean contains the data of the banner
* @ Throws RemoteException
* /
public boolean inserisciBanner (int pIdPuntoDiRistoro, Imagelcon pImmagineBanner)
throws RemoteException;
/ **
* Delete a banner from the system.
*
* @ Param pBannerID ID banner to be deleted.
* @ Return true if the operation is successful false otherwise.
* @ Throws RemoteException
* /
public boolean cancellaBanner (int pBannerID) throws RemoteException;
/ **
* Modify the data of the banner or the image associated.
*
* @ Param pBanner Bean contains the data of the banner.
* @ Return true if the operation is successful, false otherwise.
* @ Throws RemoteException
* /
public boolean modificaBanner (int pBannerID, Imagelcon pImmagine)
throws RemoteException;
/ **
* Returns a list of Banner of a particular point of comfort.
*
* @ Param Id pPuntoDiRistoroID of refreshment owner of banner
* @ Return ArrayList containing the list of banner refreshment
* @ Throws RemoteException
* /
<BeanBanner,ImageIcon> ottieniBannersDaID public HashMap (int pIdPuntoDiRistoro)
throws RemoteException;

)

