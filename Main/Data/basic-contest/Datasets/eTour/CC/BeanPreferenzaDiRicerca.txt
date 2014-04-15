package unisa.gps.etour.bean;
import java.io.Serializable;

/ **
  * Bean which contains data search preferences
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanPreferenzaDiRicerca implements Serializable
(
private static final long serialVersionUID = 7576354037868937929L;
private int id;
private String name;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param Pnom
* /
public BeanPreferenzaDiRicerca (int pid, String Pnom)
(
setId (PID);
setNome (Phnom);
)

/ **
* Empty Constructor
*
* /
public BeanPreferenzaDiRicerca ()
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

