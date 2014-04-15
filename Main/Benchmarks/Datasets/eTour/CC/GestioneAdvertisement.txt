/ **
  * Class that implements the general
  * Management dell'advertisement.
  *
  * @ Author Fabio Palladino
  * @ Version 0.1
  *
  * 2007 eTour Project - Copyright by SE @ SA Lab DMI University of Salerno
  * /
package unisa.gps.etour.control.GestioneAdvertisement;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;

import unisa.gps.etour.bean.BeanBanner;
import unisa.gps.etour.bean.BeanConvenzione;
import unisa.gps.etour.repository.DBBanner;
/ / import unisa.gps.etour.control.GestioneAdvertisement.test.stubs .*;
import unisa.gps.etour.repository.DBConvenzione;
import unisa.gps.etour.repository.IDBBanner;
import unisa.gps.etour.repository.IDBConvenzione;
import unisa.gps.etour.util.ControlloDati;
import unisa.gps.etour.util.CostantiGlobali;
import unisa.gps.etour.util.MessaggiErrore;

GestioneAdvertisement UnicastRemoteObject public class extends implements
IGestioneAdvertisement
(
private static final long serialVersionUID = 1L;
/ ** An object that handles operations on the banner * /
Private IDBBanner dbBanner;
/ **
* Constructor, instantiates an object of type DBBanner that
* Contains methods that perform operations on data
* Entity banner.
*
* @ Throws RemoteException
* /
public GestioneAdvertisement () throws RemoteException
(
super ();
dbBanner = new DBBanner ();
)
/ **
* Delete a banner and the image associated with the system.
*
* @ Param id of the banner to remove pBannerID
* @ Throws RemoteException
* /
public boolean cancellaBanner (int pIdBanner) throws RemoteException
(
/ * Bean containing the data of the banner * /
BeanBanner banner;

TRY
(
/ * Load the banner and check the data * /
banner = dbBanner.ottieniBannerDaID (pIdBanner);
if (! ControlloDati.checkBeanBanner (banner)) (
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
)
/ * Remove the image associated with the banner * /
Image file = new File (banner.getPercorsoFile ());
immagine.delete ();
/ * Clear the whole bean banner * /
return (dbBanner.cancellaBanner (pIdBanner));
)
catch (SQLException e) (
throw new RemoteException (MessaggiErrore.ERRORE_CONNESSIONE_DBMS);
)
)
/ **
* Method to change the image associated with the banner:
* Delete the old image and stores the new
* Enjoy the same path as the old image.
*
* @ Param id pBannerID banner
* @ Param pImmagine ImageIcon object containing the new image of the banner
* @ Return returns true if the operation is successful
* /
public boolean modificaBanner (int pBannerID, Imagelcon pImmagine) throws RemoteException
(

TRY
(
/ * Performs a check on the image of the banner * /
if (! ControlloDati.checkImmagine (pImmagine)) (
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
)
/ * Load the icon image * /
BufferedImage buffImg = (BufferedImage) pImmagine.getImage ();
/ * Load the data of the banner * /
BeanBanner banner = dbBanner.ottieniBannerDaID (pBannerID);
/ * Check the banner on the data uploaded * /
if (! ControlloDati.checkBeanBanner (banner)) (
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
)
/ * Rewrite the image file and returns the result of the operation * /
Imgfile file = new File (banner.getPercorsoFile ());
return (ImageIO.write (buffImg, "jpg", Imgfile));
)
catch (SQLException e) (
throw new RemoteException (MessaggiErrore.ERRORE_CONNESSIONE_DBMS);
)
catch (Exception e) (
if (e instanceof RemoteException)
(
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
) else
(
throw new RemoteException (MessaggiErrore.ERRORE_FILE);
)
)
)
/ **
* Returns a list of banners for a refreshment.
* Use the method (@ link) DBBanner.ottieniBanner
*
* @ Param ID pPuntoDiRistoroID refreshment
* @ Throws RemoteException
* /
<BeanBanner,ImageIcon> ottieniBannersDaID public HashMap (int pIdPuntoDiRistoro)
throws RemoteException (
/ * Create a HashMap that will hold the banner and associated images * /
<BeanBanner,ImageIcon> ToReturn HashMap <BeanBanner, ImageIcon> = new HashMap ();

TRY
(
/ * Get the list of banners for a refreshment point * /
ArrayList <BeanBanner> listaBanner = dbBanner.ottieniBanner (pIdPuntoDiRistoro);
/ * For each banner valid list * /
for (BeanBanner banner: listaBanner) (
/ * If the banner is a valid charge in the HashMap with the image file * /
if (ControlloDati.checkBeanBanner (banner)) (
/ * Create the image file * /
FileImmagine file = new File (banner.getPercorsoFile ());
/ * Create an object ImageIcon from the image file * /
ImageIcon icon = new ImageIcon (ImageIO.read (fileImmagine));
/ * Add the banner and the image all'HashMap * /
toReturn.put (banner, icon);
)
)
toReturn return;
)
catch (SQLException e) (
throw new RemoteException (MessaggiErrore.ERRORE_CONNESSIONE_DBMS);
)
catch (Exception e) (
throw new RemoteException (MessaggiErrore.ERRORE_FILE);
)
catch (Exception e) (
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
)
/ **
* Create and insert a new banner, making sure the num. maximum
* Banner displayed for a refreshment point is
* Already been reached in this case throws an exception to the calling method.
*
* @ Param Id pIDPuntoDiRistoro of refreshment
* @ Param pImmagineBanner ImageIcon object containing the banner image
* @ Return Returns true if the insertion has been successful.
* @ Exception RemoteException
* /
public boolean inserisciBanner (int pIdPuntoDiRistoro, Imagelcon pImmagineBanner)
throws RemoteException
(

TRY
(
/ * Performs a check on the image * /
if (! ControlloDati.checkImmagine (pImmagineBanner)) (
throw new RemoteException (MessaggiErrore.ERRORE_DATI);
)
/ * Create an object that handles the conventions * /
IDBConvenzione dbConvenzione = new DBConvenzione ();
/ * Load the data of the convention of a refreshment point * /
BeanConvenzione Convention dbConvenzione.ottieniConvezioneAttiva = (pIdPuntoDiRistoro);
/ * Get the number of banners displayed by the Convention Masssimo * /
int maxBanner = convenzione.getMaxBanner ();
/ * Load the list of banners for a refreshment and stores the number * /
int numBanner = (dbBanner.ottieniBanner (pIdPuntoDiRistoro)). size ();
/ * Check that the maximum number of banners is not reached * /
if (! (numBanner <maxBanner))
(
throw new Exception (MessaggiErrore.ERRORE_NUM_BANNER);
)
/ * Create a blank banner * /
Banner BeanBanner = new BeanBanner ();
/ * Create a unique file name * /
String path = CostantiGlobali.SERVER_IMAGE_PATH + pIdPuntoDiRistoro;
int i = 0;
FileImg file = new File (path + "_" + i + ". Jpg");
while (fileImg.exists ()) (
i + +;
fileImg = new File (path + "_" + i + ". jpg");
)
/ * Stores the image in the file system * /
BufferedImage im = (BufferedImage) pImmagineBanner.getImage ();
if (! ImageIO.write (im, "jpg", fileImg)) (
/ * Write failed * /
throw new IOException ();
)
/ * Loads the data in the banner * /
banner.setIdPuntoDiRistoro (pIdPuntoDiRistoro);
banner.setPercorsoFile (fileImg.getPath ());
/ * Insert the banner in the database * /
return (dbBanner.inserisciBanner (banner));
)
catch (SQLException e)
(
throw new RemoteException (MessaggiErrore.ERRORE_CONNESSIONE_DBMS);
)
catch (Exception e)
(
throw new RemoteException (MessaggiErrore.ERRORE_FILE);
)
catch (Exception e)
(
if (e.getMessage (). equals (MessaggiErrore.ERRORE_NUM_BANNER))
(
/ * Has been reached on most of banner inserted num * /
throw new RemoteException (MessaggiErrore.ERRORE_NUM_BANNER);
) else
(
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)

)

)

)

