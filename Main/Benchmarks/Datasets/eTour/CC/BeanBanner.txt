package unisa.gps.etour.bean;

/ **
  * Bean which contains data on the Banner
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

import java.io.Serializable;

public class BeanBanner implements Serializable
(
private static final long serialVersionUID =-872783211721655763L;
private int id;
private int idPuntoDiRistoro;
private String filepath;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param pPercorsoFile
* @ Param pidPuntoDiRistoro
* /

public BeanBanner (int pid, String pPercorsoFile, int pidPuntoDiRistoro)
(
setId (PID);
setPercorsoFile (pPercorsoFile);
setIdPuntoDiRistoro (pidPuntoDiRistoro);
)

/ **
* Empty Constructor
*
* /
public BeanBanner ()
(

)

/ **
* Returns the value of FilePath
*
* @ Return value of FilePath.
* /

public String getPercorsoFile ()
(
return filepath;
)

/ **
* Sets the new value of filepath
*
* @ Param pPercorsoFile New value filepath.
* /
public void setPercorsoFile (String pPercorsoFile)
(
filepath = pPercorsoFile;
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