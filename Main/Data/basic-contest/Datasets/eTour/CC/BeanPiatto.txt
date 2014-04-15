package unisa.gps.etour.bean;
import java.io.Serializable;

/ **
  * Bean containing information relating to food
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

public class BeanPiatto implements Serializable
(
private int id;
private String name;
private double price;
private int idMenu;
private static final long serialVersionUID =-3775462843748984482L;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param Pnom
* @ Param pPrezzo
* @ Param pIdMenu
* /
public BeanPiatto (int pid, String Pnom, double pPrezzo, int pIdMenu)
(
setId (PID);
setNome (Phnom);
setPrezzo (pPrezzo);
setIdMenu (pIdMenu);
)

/ **
* Empty Constructor
* /
public BeanPiatto ()
(

)

/ **
* Returns the value of name
*
* @ Return value of name.
* /
public String getName ()
(
return name;
)

/ **
* Sets the new name value
*
* @ Param name New value Pnom.
* /
public void setNome (String Pnom)
(
name = Pnom;
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
* Returns the value of idMenu
*
* @ Return value idMenu.
* /
public int getIdMenu ()
(
idMenu return;
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
* Sets the new value of idMenu
*
* @ Param value pIdMenu New idMenu.
* /
public void setIdMenu (int pIdMenu)
(
idMenu = pIdMenu;
)

) 