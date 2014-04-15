package unisa.gps.etour.bean;
import java.io.Serializable;
/ **
  * Bean containing information relating to food
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanOperatorePuntoDiRistoro implements Serializable
(
private int id;
private String name;
private String name;
private String username;
private String password;
private String email;
private int idPuntoDiRistoro;
private static final long serialVersionUID =-6485826396352557404L;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param Pnom
* @ Param pCognome
* @ Param pUsername
* @ Param pPassword
* @ Param pEmail
* @ Param pIdPuntoDiRistoro
* /
public BeanOperatorePuntoDiRistoro (int pid, Phnom String, String pCognome,
PUsername String, String pPassword, String pEmail,
int pIdPuntoDiRistoro)
(
setId (PID);
setNome (Phnom);
setCognome (pCognome);
setUsername (pUsername);
setPassword (pPassword);
setEmail (pEmail);
setIdPuntoDiRistoro (pIdPuntoDiRistoro);
)

/ **
* Empty Constructor
* /
public BeanOperatorePuntoDiRistoro ()
(

)

/ **
* Returns the value of name
*
* @ Return value of name.
* /
public String getCognome ()
(
return name;
)

/ **
* Sets the new value of name
*
* @ Param value New pCognome surname.
* /
public void setCognome (String pCognome)
(
last = pCognome;
)

/ **
* Returns the value of email
*
* @ Return value of email.
* /
public String getEmail ()
(
return email;
)

/ **
* Sets the new value of email
*
* @ Param pEmail New value of email.
* /
public void setEmail (String pEmail)
(
email = pEmail;
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
* Returns the value of password
*
* @ Return value of password.
* /
public String getPassword ()
(
return password;
)

/ **
* Sets the new password value
*
* @ Param pPassword new password value.
* /
public void setPassword (String pPassword)
(
password = pPassword;
)

/ **
* Returns the value of username
*
* @ Return value of username.
* /
public String GetUserName ()
(
return username;
)

/ **
* Sets the new value of username
*
* @ Param pUsername New value for username.
* /
public void setUsername (String pUsername)
(
username = pUsername;
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
