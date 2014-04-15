package unisa.gps.etour.bean;
import java.io.Serializable;
import java.util.Date;
/ **
  * Bean containing information relating to a tourist
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public class BeanTurista implements Serializable
(
private static final long serialVersionUID = 4214744963263090577L;
private int id;
private String username;
private String name;
private String name;
private String cittaNascita;
private String cittaResidenza;
private String phone;
private String cap;
private String street;
private String province;
private String email;
private String password;
private Date dob;
private Date dataRegistrazione;
private boolean active;

/ **
* Parameterized constructor
*
* @ Param pid
* @ Param pUsername
* @ Param Pnom
* @ Param pCognome
* @ Param pCittaNascita
* @ Param pCittaResidenza
* @ Param pTelefono
* @ Param pcap
* @ Param pVia
* @ Param pProvincia
* @ Param pEmail
* @ Param pPassword
* @ Param pDataNascita
* @ Param pDataRegistrazione
* @ Param pacts
* /
public BeanTurista (int pid, String pUsername, String Pnom,
PCognome String, String pCittaNascita, String pCittaResidenza,
PTelefono String, String pcap, pVia String, String pProvincia,
PEmail String, String pPassword, Date pDataNascita,
Date pDataRegistrazione, boolean Patti)
(
setId (pid);
setUsername (pUsername);
setNome (Phnom);
setCognome (pCognome);
setCittaNascita (pCittaNascita);
setCittaResidenza (pCittaResidenza);
setTelefono (pTelefono);
setCap (PCAP);
setvar (pVia);
setProvincia (pProvincia);
setEmail (pEmail);
setPassword (pPassword);
setDataNascita (pDataNascita);
setDataRegistrazione (pDataRegistrazione);
setAttiva (Patti);
)

/ **
* Empty Constructor
* /
public BeanTurista ()
(

)

/ **
* Returns the value of cap
*
* @ Return value cap.
* /
public String getCap ()
(
return cap;
)

/ **
* Sets the new value of cap
*
New pcap * @ param value cap.
* /
public void setCap (String PCAP)
(
ch = pcap;
)

/ **
* Returns the value of cittaNascita
*
* @ Return value cittaNascita.
* /
public String getCittaNascita ()
(
cittaNascita return;
)

/ **
* Sets the new value of cittaNascita
*
* @ Param value pCittaNascita New cittaNascita.
* /
public void setCittaNascita (String pCittaNascita)
(
cittaNascita = pCittaNascita;
)

/ **
* Returns the value of cittaResidenza
*
* @ Return value cittaResidenza.
* /
public String getCittaResidenza ()
(
cittaResidenza return;
)

/ **
* Sets the new value of cittaResidenza
*
* @ Param value pCittaResidenza New cittaResidenza.
* /
public void setCittaResidenza (String pCittaResidenza)
(
cittaResidenza = pCittaResidenza;
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
* Returns the value of dob
*
* @ Return value dob.
* /
public Date getDataNascita ()
(
return dob;
)

/ **
* Sets the new value of dob
*
* @ Param value New pDataNascita dob.
* /
public void setDataNascita (Date pDataNascita)
(
dob = pDataNascita;
)

/ **
* Returns the value of dataRegistrazione
*
* @ Return value dataRegistrazione.
* /
public Date getDataRegistrazione ()
(
dataRegistrazione return;
)

/ **
* Sets the new value of dataRegistrazione
*
* @ Param value pDataRegistrazione New dataRegistrazione.
* /
public void setDataRegistrazione (Date pDataRegistrazione)
(
dataRegistrazione = pDataRegistrazione;
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
* Returns the value of the province
*
* @ Return value of the province.
* /
public String getProvincia ()
(
return province;
)

/ **
* Sets the new value of the province
*
* @ Param pProvincia New value for the province.
* /
public void setProvincia (String pProvincia)
(
province = pProvincia;
)

/ **
* Returns the value of telephone
*
* @ Return Value of the phone.
* /
public String getTelefono ()
(
return phone;
)

/ **
* Sets the new value of telephone
*
* @ Param value New pTelefono phone.
* /
public void setTelefono (String pTelefono)
(
phone = pTelefono;
)

/ **
* Returns the value of street
*
* @ Return value on.
* /
public String getVar ()
(
return path;
)

/ **
* Sets the new value via
*
* @ Param value New pVia on.
* /
public void setvar (String pVia)
(
via = pVia;
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
Returns to be 1 or 0, indicating whether a tourist or not
* Active
*
* @ Return value of activation
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