package unisa.gps.etour.bean;
import java.io.Serializable;
import java.util.Date;
/ **
  * Bean that contains the data for feedback to a refreshment
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanVisitaPR implements Serializable
(

private static final long serialVersionUID =-4065240072283418782L;
private int rating;
private int idPuntoDiRistoro;
private String comment;
private int IdTurista;
private Date dataVisita;

/ **
* Parameterized constructor
*
* @ Param pVoto
* @ Param pIdPuntoDiRistoro
* @ Param pCommento
* @ Param pIdTurista
* @ Param pDataVisita
* /
public BeanVisitaPR (pVoto int, int pIdPuntoDiRistoro,
PCommento String, int pIdTurista, Date pDataVisita)
(
setVoto (pVoto);
setIdPuntoDiRistoro (pIdPuntoDiRistoro);
setCommento (pCommento);
setIdTurista (pIdTurista);
setDataVisita (pDataVisita);
)

/ **
* Empty Constructor
* /
public BeanVisitaPR ()
(

)

/ **
* Returns the value of comment
*
* @ Return value of comment.
* /
public String getCommento ()
(
return comment;
)

/ **
* Sets the new value of comment
*
* @ Param value New pCommento comment.
* /
public void setCommento (String pCommento)
(
comment = pCommento;
)

/ **
* Returns the value of dataVisita
*
* @ Return value dataVisita.
* /
public Date getDataVisita ()
(
dataVisita return;
)

/ **
* Sets the new value of dataVisita
*
* @ Param value pDataVisita New dataVisita.
* /
public void setDataVisita (Date pDataVisita)
(
dataVisita = pDataVisita;
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
* Sets the new value of idPuntoDiRistoro
*
* @ Param value pIdPuntoDiRistoro New idPuntoDiRistoro.
* /
public void setIdPuntoDiRistoro (int pIdPuntoDiRistoro)
(
idPuntoDiRistoro = pIdPuntoDiRistoro;
)

/ **
* Returns the value of IdTurista
*
* @ Return value IdTurista.
* /
public int getIdTurista ()
(
IdTurista return;
)

/ **
* Sets the new value of IdTurista
*
* @ Param value pIdTurista New IdTurista.
* /
public void setIdTurista (int pIdTurista)
(
IdTurista = pIdTurista;
)

/ **
* Returns the value of voting
*
* @ Return value of vote.
* /
public int getVoto ()
(
return rating;
)

/ **
* Sets the new value of voting
*
New value * @ param pVoto to vote.
* /
public void setVoto (int pVoto)
(
vote = pVoto;
)

)
