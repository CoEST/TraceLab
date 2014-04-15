package unisa.gps.etour.bean;
import java.io.Serializable;
import java.util.Date;

import unisa.gps.etour.util.Punto3D;
/ **
  * Bean for the storage of data refreshment
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

public class BeanPuntoDiRistoro implements Serializable
(
private static final long serialVersionUID = 8417686685147484931L;
private int id;
private int numeroVoti;
private double mediaVoti;
private String name;
private String description;
private String phone;
private String location;
private String city;
private String street;
private String cap;
private String province;
private String PartitaIva;
Private Punto3D position;
private Date orarioApertura;
private Date orarioChiusura;
private String giornoChiusura;

/ **
* Parameterized constructor
*
* @ Param pId
* @ Param pNumeroVoti
* @ Param pMediaVoti
* @ Param Pnom
* @ Param pDescrizione
* @ Param pTelefono
* @ Param pLocalita
* @ Param pCitta
* @ Param pVia
* @ Param pcap
* @ Param pProvincia
* @ Param pPartitaIva
* @ Param pPosizione
* @ Param pOrarioApertura
* @ Param pOrarioChiusura
* @ Param pGiornoChiusura
* /
public BeanPuntoDiRistoro (int pid, int pNumeroVoti, double pMediaVoti,
Phnom String, String pDescrizione, String pTelefono,
PLocalita String, String pCitta, pVia String, String pcap,
PProvincia String, String pPartitaIva, Punto3D pPosizione,
Date pOrarioApertura, Date pOrarioChiusura, String pGiornoChiusura)
(
setId (PID);
setNumeroVoti (pNumeroVoti);
setMediaVoti (pMediaVoti);
setNome (Phnom);
setDescrizione (pDescrizione);
setTelefono (pTelefono);
setlocale (pLocalita);
setCitta (pCitta);
setvar (pVia);
setCap (PCAP);
setProvincia (pProvincia);
setPartitaIva (pPartitaIva);
setPosizione (pPosizione);
setOrarioApertura (pOrarioApertura);
setOrarioChiusura (pOrarioChiusura);
setGiornoChiusura (pGiornoChiusura);
)

/ **
* Empty Constructor
*
* /
public BeanPuntoDiRistoro ()
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
* Returns the value of city
*
* @ Return Value of city.
* /
public String getCitta ()
(
return city;
)

/ **
* Sets the new value of city
*
* @ Param value New pCitta city.
* /
public void setCitta (String pCitta)
(
City = pCitta;
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
* Returns the value of giornoChiusura
*
* @ Return value giornoChiusura.
* /
public String getGiornoChiusura ()
(
giornoChiusura return;
)

/ **
* Sets the new value of giornoChiusura
*
* @ Param value pGiornoChiusura New giornoChiusura.
* /
public void setGiornoChiusura (String pGiornoChiusura)
(
giornoChiusura = pGiornoChiusura;
)

/ **
* Returns the value of location
*
* @ Return locale values.
* /
public String getLocal ()
(
return location;
)

/ **
* Sets the new value of location
*
* @ Param pLocalita New locale values.
* /
public void setLocale (String pLocalita)
(
location = pLocalita;
)

/ **
* Returns the value of mediaVoti
*
* @ Return value mediaVoti.
* /
public double getMediaVoti ()
(
mediaVoti return;
)

/ **
* Sets the new value of mediaVoti
*
* @ Param value pMediaVoti New mediaVoti.
* /
public void setMediaVoti (double pMediaVoti)
(
mediaVoti = pMediaVoti;
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
* Returns the value of numeroVoti
*
* @ Return value numeroVoti.
* /
public int getNumeroVoti ()
(
numeroVoti return;
)

/ **
* Sets the new value of numeroVoti
*
* @ Param value pNumeroVoti New numeroVoti.
* /
public void setNumeroVoti (int pNumeroVoti)
(
numeroVoti = pNumeroVoti;
)

/ **
* Returns the value of orarioApertura
*
* @ Return value orarioApertura.
* /
public Date getOrarioApertura ()
(
orarioApertura return;
)

/ **
* Sets the new value of orarioApertura
*
* @ Param value pOrarioApertura New orarioApertura.
* /
public void setOrarioApertura (Date pOrarioApertura)
(
orarioApertura = pOrarioApertura;
)

/ **
* Returns the value of orarioChiusura
*
* @ Return value orarioChiusura.
* /
public Date getOrarioChiusura ()
(
orarioChiusura return;
)

/ **
* Sets the new value of orarioChiusura
*
* @ Param value pOrarioChiusura New orarioChiusura.
* /
public void setOrarioChiusura (Date pOrarioChiusura)
(
orarioChiusura = pOrarioChiusura;
)

/ **
* Returns the value of PartitaIva
*
* @ Return value of a political party.
* /
public String getPartitaIva ()
(
PartitaIva return;
)

/ **
* Sets the new value of PartitaIva
*
* @ Param pPartitaIva New value of political parties.
* /
public void setPartitaIva (String pPartitaIva)
(
PartitaIva = pPartitaIva;
)

/ **
* Returns the value of position
*
* @ Return value of position.
* /
public Punto3D getPosizione ()
(
return position;
)

/ **
* Sets the new position value
*
* @ Param pPosizione New position value.
* /
public void setPosizione (Punto3D pPosizione)
(
position = pPosizione;
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