package unisa.gps.etour.util;

import java.awt.image.BufferedImage;
import java.util.Date;

import javax.swing.ImageIcon;

import unisa.gps.etour.bean.BeanBanner;
import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanConvenzione;
import unisa.gps.etour.bean.BeanMenu;
import unisa.gps.etour.bean.BeanNews;
import unisa.gps.etour.bean.BeanOperatorePuntoDiRistoro;
import unisa.gps.etour.bean.BeanPiatto;
import unisa.gps.etour.bean.BeanPreferenzaDiRicerca;
import unisa.gps.etour.bean.BeanPreferenzeGeneriche;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;

/ **
  * Class for managing the control of the correctness of the strings
  *
  * @ Author Joseph Penna
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class ControlloDati
(

private static final String letter = "abcdefghijklmnopqrstuvxywz"
+ "ABCDEFGHIJKLMNOPQRSTUVXYWZ";
private final static String numbers = "0123456789";

public final static int max_length = 64;

/ **
* Static method for verifying correctness of a string
*
* @ Param string to check pStringa
* @ Param letterePermesse Boolean: True if it is allowed to be present
* Letters in the string, False otherwise
* @ Param numeriPermessi Boolean: True if it is allowed to be present
* Numbers in the string, False otherwise
CaratteriPermessi * @ param string containing all characters
* Allowed in the string
* @ Param string contentente all caratteriNecessari characters
* Must be present in the string
* @ Param numeroCaratteriMin integer representing the minimum number of
* Characters allowed in string
* @ Param numeroCaratteriMax integer representing the maximum number of
* Characters allowed in string
* @ Return Boolean: True if the witch meets the conditions, False
* Otherwise
* /
public static boolean controllaStringa (String pStringa,
letterePermesse boolean, boolean numeriPermessi,
CaratteriPermessi String, String caratteriNecessari,
numeroCaratteriMin int, int numeroCaratteriMax)
(
Chance pStringa == null: the function returns false
if (null == pStringa)
return false;

int lunghezzaStringa = pStringa.length ();
CarattereCorrente character;

/ / Check the length of the string
if (lunghezzaStringa <numeroCaratteriMin
| | LunghezzaStringa> numeroCaratteriMax)
(
return false;
)

/ / Check the presence of the necessary characters in the string
if (caratteriNecessari = null)
(
if (! caratteriNecessari.equals (""))
for (int i = 0; i <caratteriNecessari.length (); i + +)
(
carattereCorrente = caratteriNecessari.charAt (i);
if (! pStringa.contains (carattereCorrente.toString ()))
(
return false;
)
)
)

/ / Create the string containing all characters allowed
String stringaCaratteriPermessi = caratteriPermessi
CaratteriNecessari + + (letterePermesse? Letters: "")
+ (NumeriPermessi? Numbers: "");

/ / Loop for the inspection of the characters of the string to check
for (int i = 0; i <lunghezzaStringa i + +)
(

carattereCorrente = pStringa.charAt (i);

/ / Condition: If the character you are watching is not
/ / In the string of characters allowed
/ / The string is incorrect and out of the loop.
if (! (stringaCaratteriPermessi.contains (carattereCorrente
. toString ())))
(

return false;
)
)

return true;
)

public static String correggiStringa (String pStringa,
letterePermesse boolean, boolean numeriPermessi,
CaratteriPermessi String, int numeroCaratteriMax)
(

if (null == pStringa)
return null;

String stringaCaratteriPermessi = caratteriPermessi
+ (LetterePermesse? Letters: "")
+ (NumeriPermessi? Numbers: "");

CarattereCorrente character;
int lunghezzaStringa = pStringa.length ();
int i = 0;

while (i <lunghezzaStringa)
(
carattereCorrente = pStringa.charAt (i);

if (! (stringaCaratteriPermessi.contains (carattereCorrente
. toString ())))
(
pStringa = pStringa.replaceAll ( "\ \"
CarattereCorrente.toString + (), "");
lunghezzaStringa = pStringa.length ();

)
else
i + +;
)
if (lunghezzaStringa> numeroCaratteriMax)
(

pStringa = pStringa.substring (0, numeroCaratteriMax);
)

pStringa return;
)

public static boolean controllaData (String pData)
(
/ / Still I have no idea how I will spend the time
return true;
)

public static boolean controllaDate (Date pDataInizio, Date pDataFine)
(

boolean back = false;
if (pDataInizio! = null & & pDataFine = null)
(

if (pDataInizio.before (pDataFine))
return = true;
)

return receipt;
)

public static boolean checkBeanTurista (BeanTurista pTurista)
(
if (pTurista = null & & pTurista instanceof BeanTurista)
return true;
return false;
)

public static boolean checkBeanPreferenzaDiRicerca (
BeanPreferenzaDiRicerca pPreferenzaDiRicerca)
(
if (pPreferenzaDiRicerca = null
& & PPreferenzaDiRicerca instanceof BeanPreferenzaDiRicerca)
return true;
return true;
)

public static boolean checkBeanPreferenzeGeneriche (
BeanPreferenzeGeneriche pPreferenzeGeneriche)
(
if (pPreferenzeGeneriche = null
& & PPreferenzeGeneriche instanceof BeanPreferenzeGeneriche)
return true;
return false;
)

public static boolean checkBeanBeneCulturale (
BeanBeneCulturale pBeneCulturale)
(
if (pBeneCulturale = null
& & PBeneCulturale instanceof BeanBeneCulturale)
return true;
return false;
)

public static boolean checkBeanPuntoDiRistoto (
BeanPuntoDiRistoro pPuntoDiRistoro)
(
if (pPuntoDiRistoro = null
& & PPuntoDiRistoro instanceof BeanPuntoDiRistoro)
return true;
return false;
)

public static boolean checkBeanOperatorePuntoDiRistoro (
BeanOperatorePuntoDiRistoro pOperaotorePuntoDiRistoro)
(
if (pOperaotorePuntoDiRistoro = null
& & POperaotorePuntoDiRistoro instanceof BeanOperatorePuntoDiRistoro)
return true;
return false;
)

/ **
* Please formal control and consistency on the data of the banner
* Content in the bean passed by parameter.
*
* @ Author Fabio Palladino
* @ Param pBanner bean contains the data of the banner.
* @ Return True if the data of the banner is correct false otherwise.
* /
public static boolean checkBeanBanner (BeanBanner pBanner)
(
toReturn boolean = false;

if (pBanner = null & & pBanner instanceof BeanBanner)
(
toReturn = (pBanner.getId ()> 0 & & pBanner.getPercorsoFile ()! = "" & & pBanner
. getIdPuntoDiRistoro ()> 0);
)

toReturn return;
)

/ **
* Method which controls the image contained in the object ImageIcon past
* Per parameter.
*
* @ Author Fabio Palladino
* @ Param image ImageIcon object containing the image to be checked
* @ Return true if the image contained in the object is an instance ImageIcon
* Class BufferedImage.
* /
public static boolean checkImmagine (ImageIcon image)
(

if (image! = null)
(
return (immagine.getImage () instanceof BufferedImage);
)
return false;
)

/ **
* Function that checks the data in a news;
*
* @ Author Fabio Palladino
* @ Param bean Pnews containing details of a news.
* @ Return
* /
public static boolean checkBeanNews (BeanNews Pnews)
(
toReturn boolean = false;

/ * Check the validity of the general method parameter * /
if (Pnews = null)
(

PNews.getDataPubblicazione Date dataPubb = () / / Date of
/ / Publishing
Date dataScad = pNews.getDataScadenza () / / Due Date
PNews.getNews String news = (), / / Text of News
int priority = pNews.getPriorita ();

/ * Checking the invalidity of the fields * /
if (dataPubb! dataScad = null & &! = null & & news = null)
(
/ * Check the consistency of the dates * /
toReturn = dataPubb.before (dataScad);
/ * Check that the text is not empty * /
toReturn = toReturn & & (news! = "");
/ * Check that the ID is greater than 0 * /
toReturn = toReturn & & (pNews.getId ()> 0);
/ * Check the priority value * /
toReturn = toReturn
& & (Priority <= CostantiGlobali.MAX_PRIORITY_NEWS)
& & (Priority> = CostantiGlobali.MIN_PRIORITY_NEWS);
)
)
toReturn return;
)

public static boolean checkBeanTag (BeanTag ptagi)
(
if (ptagi! = null & & instanceof ptagi BeanTag)
return true;
return false;
)

public static boolean checkBeanConvenzione (BeanConvenzione pConvenzione)
(
if (pConvenzione = null & & pConvenzione instanceof BeanConvenzione)
return true;
return false;
)

public static boolean checkBeanMenu (BeanMenu pMenu)
(
if (pMenu = null & & instanceof pMenu BeanMenu)
return true;
return false;
)

public static boolean checkBeanPiatto (BeanPiatto pPiatto)
(
if (pPiatto = null & & pPiatto instanceof BeanPiatto)
return true;
return false;
)

public static boolean checkBeanVisitaBC (BeanVisitaBC pVisitaBC)
(
if (pVisitaBC = null & & pVisitaBC instanceof BeanVisitaBC)
return true;
return false;
)

public static boolean checkBeanVisitaPR (BeanVisitaPR pVisitaPR)
(
if (pVisitaPR = null & & pVisitaPR instanceof BeanVisitaPR)
return true;
return false;
)
) 