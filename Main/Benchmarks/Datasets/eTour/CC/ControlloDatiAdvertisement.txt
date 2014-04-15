/ **
  * Class that contains static methods that perform
  * Consistency checks on the data bean banner
  * And news.
  *
  * @ Author Fabio Palladino
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
package com.trapan.spg.control.GestioneAdvertisement;

import java.util.Date;

import com.trapan.spg.bean.BeanBanner;
import com.trapan.spg.bean.BeanNews;

public class ControlloDatiAdvertisement
(
/ **
* Please formal control and consistency on
* Data content of the banner in the bean passed by parameter.
* @ Param pBanner bean contains the data of the banner.
* @ Return
* /
public static boolean controlloBanner (BeanBanner pBanner) (
toReturn boolean = false;

if (pBanner = null) (
toReturn = (pBanner.getId ()> 0 & & & & pBanner.getPercorsoFile ()!=""
pBanner.getIdPuntoDiRistoro ()> 0);
)

toReturn return;
)
/ **
* Method that performs consistency checks and
* Correctness of the information contained in the bean past
* Per parameter, in particular check that the dates
* Publication and expiration of the news are consistent,
* Or that the second is the later.
*
* @ Param Pnews bean containing data news
* @ Return Returns true if the bean contains consistent data
* /
public static boolean controlloNews (BeanNews Pnews) (
toReturn boolean = false;

/ * Check the validity of the general method parameter * /
if (Pnews = null) (

Date dataPubb = pNews.getDataPubblicazione () / / Released
Date dataScad = pNews.getDataScadenza () / / Due Date
PNews.getNews String news = (), / / Text of News

/ * Checking the invalidity of the fields * /
if (dataPubb! dataScad = null & &! = null & & news = null) (
/ * Check the consistency of the dates * /
toReturn = dataPubb.before (dataScad);

/ * Check that the text is not empty * /
toReturn = toReturn & & (news! = "");

/ * Check that the ID is greater than 0 * /
toReturn = toReturn & & (pNews.getId ()> 0);

/ * Check the priority value * /
)

)

toReturn return;

)
) 