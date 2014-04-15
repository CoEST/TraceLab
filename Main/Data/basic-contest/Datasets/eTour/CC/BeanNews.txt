package unisa.gps.etour.bean;

/ **
  * Bean containing information relating to the News
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

import java.io.Serializable;
import java.util.Date;

public class BeanNews implements Serializable
(
private String news;
private Date dataPubblicazione;
private Date dataScadenza;
private int priority;
private int id;
private static final long serialVersionUID =-6249491056436689386L;

/ **
* Parameterized constructor
*
* @ Param Pnews
* @ Param pDataPubblicazione
* @ Param pDataScadenza
* @ Param pPriorita
* @ Param pId
* /
public BeanNews (String Pnews, Date pDataPubblicazione, Date pDataScadenza,
pPriorita int, int pid)
(
setNews (Pnews);
setDataPubblicazione (pDataPubblicazione);
setDataScadenza (pDataScadenza);
setPriorita (pPriorita);
setId (PID);
)

/ **
* Empty Constructor
*
* /
public BeanNews ()
(

)

/ **
* Returns the value of dataPubblicazione
*
* @ Return value dataPubblicazione.
* /
public Date getDataPubblicazione ()
(
dataPubblicazione return;
)

/ **
* Sets the new value of dataPubblicazione
*
* @ Param value pDataPubblicazione New dataPubblicazione.
* /
public void setDataPubblicazione (Date pDataPubblicazione)
(
dataPubblicazione = pDataPubblicazione;
)

/ **
* Returns the value of dataScadenza
*
* @ Return value dataScadenza.
* /
public Date getDataScadenza ()
(
dataScadenza return;
)

/ **
* Sets the new value of dataScadenza
*
* @ Param value pDataScadenza New dataScadenza.
* /
public void setDataScadenza (Date pDataScadenza)
(
dataScadenza = pDataScadenza;
)

/ **
* Returns the value of news
*
* @ Return value of news.
* /
public String getNews ()
(
return news;
)

/ **
* Sets the new value of news
*
* @ Param value New Pnews news.
* /
public void setNews (String Pnews)
(
news = Pnews;
)

/ **
* Returns the priority value
*
* @ Return the priority value.
* /
public int getPriorita ()
(
return priority;
)

/ **
* Set the new priority value
*
* @ Param pPriorita New priority value.
* /
public void setPriorita (int pPriorita)
(
Priority = pPriorita;
)

/ **
* Returns the value of id
*
* @ Return value id.
* /
public int getId ()
(
return id;
)

/ **
* Sets the new value of id
*
* @ Param pId New value for id.
* /
public void setId (int pid)
(
id = pid;
)

) 