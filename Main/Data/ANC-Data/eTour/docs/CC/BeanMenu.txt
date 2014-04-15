package unisa.gps.etour.bean;
import java.io.Serializable;
/ **
  * Bean containing information relating to a Menu
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanMenu implements Serializable
(
private static final long serialVersionUID =-3112032222839565409L;
private int id;
private String day;
private int idPuntoDiRistoro;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param pGiorno
* @ Param pIdPuntoDiRistoro
* /
public BeanMenu (int pid, String pGiorno, int pIdPuntoDiRistoro)
(
setId (PID);
setGiorno (pGiorno);
setIdPuntoDiRistoro (pIdPuntoDiRistoro);
)

/ **
* Empty Constructor
* /
public BeanMenu ()
(

)

/ **
* Returns the value of days
*
* @ Return Value of the day.
* /
public String getGiorno ()
(
return day
)

/ **
* Sets the new value of days
*
* @ Param value New pGiorno day.
* /
public void setGiorno (String pGiorno)
(
day = pGiorno;
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
