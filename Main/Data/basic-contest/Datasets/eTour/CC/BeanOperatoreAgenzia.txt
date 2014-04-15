package unisa.gps.etour.bean;
import java.io.Serializable;
/ **
  * Bean containing information relating to an Agency Operator
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanOperatoreAgenzia implements Serializable
(

/ **
*
* /
private static final long serialVersionUID =-3489147679484477440L;
private int id;
private String username;
private String name;
private String name;
private String password;

/ **
* Parameterized constructor
*
* @ Param pid
* @ Param pUsername
* @ Param Pnom
* @ Param pCognome
* @ Param pPassword
* /
public BeanOperatoreAgenzia (int pid, String pUsername, String Pnom,
PCognome String, String pPassword)
(
setId (pid);
setUsername (pUsername);
setNome (Phnom);
setCognome (pCognome);
setPassword (pPassword);
)

/ **
* Empty Constructor
* /
public BeanOperatoreAgenzia ()
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
* Sets the new value of id
*
* @ Param new value of id pid.
* /
public void setId (int pid)
(
id = pid;
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

) 