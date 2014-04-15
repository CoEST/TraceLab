package unisa.gps.etour.bean;

/ **
  * Bean containing information relating to the General Preferences
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

import java.io.Serializable;

public class BeanPreferenzeGeneriche implements Serializable
(
private static final long serialVersionUID = 6805656922951334071L;
private int id;
private int dimensioneFont;
private String font;
private String subject;
private int idTurista;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param pDimensioneFont
* @ Param pFont
* @ Param pTema
* @ Param pIdTurista
* /
public BeanPreferenzeGeneriche (int pid, int pDimensioneFont, String pFont,
PTema String, int pIdTurista)
(
setId (PID);
setDimensioneFont (pDimensioneFont);
setFont (pFont);
September (pTema);
setIdTurista (pIdTurista);
)

/ **
* Empty Constructor
* /
public BeanPreferenzeGeneriche ()
(

)

/ **
* Returns the value of dimensioneFont
*
* @ Return value dimensioneFont.
* /
public int getDimensioneFont ()
(
dimensioneFont return;
)

/ **
* Sets the new value of dimensioneFont
*
* @ Param value pDimensioneFont New dimensioneFont.
* /
public void setDimensioneFont (int pDimensioneFont)
(
dimensioneFont = pDimensioneFont;
)

/ **
* Returns the value of font
*
* @ Return Value of fonts.
* /
public String getFont ()
(
return font;
)

/ **
* Sets the new value of font
*
New value * @ param pFont font.
* /
public void setFont (String pFont)
(
font = pFont;
)

/ **
* Returns the value of the subject
*
* @ Return value issue.
* /
public String getTema ()
(
return theme;
)

/ **
* Sets the new value of the subject
*
* @ Param value New pTema theme.
* /
public void September (String pTema)
(
topic = pTema;
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
* Returns the value of usernameTurista
*
* @ Return value usernameTurista.
* /
public int getIdTurista ()
(
idTurista return;
)

/ **
* Sets the new value of usernameTurista
*
* @ Param value pIdTurista New usernameTurista.
* /
public void setIdTurista (int pIdTurista)
(
idTurista = pIdTurista;
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