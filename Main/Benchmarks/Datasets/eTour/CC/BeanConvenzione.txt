package unisa.gps.etour.bean;
import java.io.Serializable;
import java.util.Date;
/ **
  * Bean containing information relating to a Convention
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanConvenzione implements Serializable
(

private static final long serialVersionUID =-3255500940680220001L;
private int id;
private int maxBanner;
private Date StartDate;
private Date EndDate;
private double price;
private boolean active;
private int idPuntoDiRistoro;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param pMaxBanner
* @ Param pDataInizio
* @ Param pDataFine
* @ Param pPrezzo
* @ Param pacts
* @ Param pidPuntoDiRistoro
* /
public BeanConvenzione (int pid, int pMaxBanner, Date pDataInizio,
Date pDataFine, double pPrezzo, boolean terms,
int pidPuntoDiRistoro)
(
setId (PID);
setMaxBanner (pMaxBanner);
setDataInizio (pDataInizio);
setDataFine (pDataFine);
setPrezzo (pPrezzo);
setAttiva (Patti);
setIdPuntoDiRistoro (pidPuntoDiRistoro);
)

/ **
* Empty Constructor
*
* /
public BeanConvenzione ()
(

)

/ **
* Returns the value of active
*
* @ Return value of assets.
* /
public boolean isAttiva ()
(
return active;
)

/ **
* Sets the new value of active
*
* @ Param new value terms of assets.
* /
public void setAttiva (boolean Patti)
(
active = Pact;
)

/ **
* Returns the value of EndDate
*
* @ Return Value EndDate.
* /
public Date getDataFine ()
(
EndDate return;
)

/ **
* Sets the new value for EndDate
*
* @ Param pDataFine New value for EndDate.
* /
public void setDataFine (Date pDataFine)
(
EndDate = pDataFine;
)

/ **
* Returns the value of StartDate
*
* @ Return value StartDate.
* /
public Date getDataInizio ()
(
StartDate return;
)

/ **
* Sets the new value of StartDate
*
* @ Param new value pDataInizio StartDate.
* /
public void setDataInizio (Date pDataInizio)
(
StartDate = pDataInizio;
)

/ **
* Returns the value of maxBanner
*
* @ Return value maxBanner.
* /
public int getMaxBanner ()
(
maxBanner return;
)

/ **
* Sets the new value of maxBanner
*
* @ Param value pMaxBanner New maxBanner.
* /
public void setMaxBanner (int pMaxBanner)
(
maxBanner = pMaxBanner;
)

/ **
* Returns the value of money
*
* @ Return value price.
* /
public double getPrezzo ()
(
return price;
)

/ **
* Sets the new value of money
*
* @ Param pPrezzo New value for money.
* /
public void setPrezzo (double pPrezzo)
(
Price = pPrezzo;
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
* Returns the value of idPuntoDiRistoro
*
* @ Return value idPuntoDiRistoro.
* /
public int getIdPuntoDiRistoro ()
(
idPuntoDiRistoro return;
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

/ **
* Sets the new value of idPuntoDiRistoro
*
* @ Param value pIdPuntoDiRistoro New idPuntoDiRistoro.
* /
public void setIdPuntoDiRistoro (int pIdPuntoDiRistoro)
(
idPuntoDiRistoro = pIdPuntoDiRistoro;
)

) 