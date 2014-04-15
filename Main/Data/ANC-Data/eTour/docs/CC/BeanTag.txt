package unisa.gps.etour.bean;

/ **
  * Bean that contains the data for a Tag
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

import java.io.Serializable;

public class BeanTag implements Serializable
(
private static final long serialVersionUID =-6320421006595188597L;
private int id;
private String name;
private String description;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param Pnom
* @ Param pDescrizione
* /
public BeanTag (int pid, Phnom String, String pDescrizione)
(
setId (PID);
setNome (Phnom);
setDescrizione (pDescrizione);
)

/ **
* Empty Constructor
* /
public BeanTag ()
(

)

/ **
* Returns the value of description
*
* @ Return value of description.
* /
public String getDescrizione ()
(
return description;
)

/ **
* Sets the new value of description
*
* @ Param pDescrizione New value of description.
* /
public void setDescrizione (String pDescrizione)
(
description = pDescrizione;
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
* Sets the new value of name
*
* @ Param Phnom New value for name.
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
