package unisa.gps.etour.util;

/ **
  * Bean that contains the coordinates of a point on the surface of the earth "and
  * That it realizes the calculation of the distance from the system. The values of
  * Coordinates must be represented in radians and must fall in
  * Target range: 0 to greek-Pi / 4 for the latitude south of the equator
  * 0 to + Pi greek / 4 for the latitude north of the equator from 0 to Pi-greek /
  * 2 for the meridian of longitude west of Greenwitch greek from 0 to + Pi / 2
  * For the meridian of longitude east of Greenwitch
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

public class Punto3D
(
/ / Radius of the earth
final double EARTH_RADIUS = 6371.0;
private double latitude, longitude, altitude;

public Punto3D ()
(
latitude = longitude = height = 0;
)

public Punto3D (double pLatitudine, double pLongitudine, double pAltitudine)
(
N = pLatitudine;
longitude = pLongitudine;
altitude = pAltitudine;
)

/ **
* Returns the latitude
*
* @ Return
* /
public double getLatitudine ()
(
return latitude;
)

/ **
* Sets the latitude
*
* @ Param pLatitudine
* /
public void setLatitudine (double pLatitudine)
(
this.latitudine = pLatitudine;
)

/ **
* Returns the longitude
*
* @ Return
* /
public double getLongitudine ()
(
return longitude;
)

/ **
* Sets the longitude
*
* @ Param pLongitudine
* /
public void setLongitudine (double pLongitudine)
(
this.longitudine = pLongitudine;
)

/ **
* Returns the altitude
*
* @ Return
* /
public double getAltitudine ()
(
return altitude;
)

/ **
* Sets the altitude
*
* @ Param pAltitudine
* /
public void setAltitudine (double pAltitudine)
(
this.altitudine = pAltitudine;
)

/ **
* Calculate the distance between the point and another point given as argument
*
* @ Param p
* @ Return
* /
public double distance (Punto3D p)
(
double differenzaLongitudine = this.longitudine - p.longitudine;
double p1 = Math.pow (Math.cos (p.latitudine)
* Math.sin (differenzaLongitudine), 2);
double p2 = Math.pow (Math.cos (latitude) * Math.sin (p.latitudine)
- Math.sin (latitude) * Math.cos (p.latitudine)
* Math.cos (differenzaLongitudine), 2);
double p3 = Math.sin (latitude) * Math.sin (p.latitudine)
+ Math.cos (latitude) * Math.cos (p.latitudine)
* Math.cos (differenzaLongitudine);
return (Math.atan (Math.sqrt (p1 + p2) / p3) * EARTH_RADIUS);
)

/ **
* Method which creates a 3D point from coordinates measured in degrees. The
* 3D point instead represents the coordinates in radians
*
* @ Param pLatitudine latitude in degrees
* @ Param pLongitudine Longitude in degrees *
* @ Param pAltitudine
* @ Return Punto3D with the coordinates in radians
* /
public static Punto3D gradiRadianti (double pLatitudine,
pLongitudine double, double pAltitudine)
(
Punto3D point = new Punto3D ();
punto.setLatitudine (pLatitudine * Math.PI / 180);
punto.setLongitudine (pLongitudine * Math.PI / 180);
punto.setAltitudine (pAltitudine);
return point;
)
) 