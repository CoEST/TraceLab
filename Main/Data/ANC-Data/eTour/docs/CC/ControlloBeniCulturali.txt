package unisa.gps.etour.control.GestioneBeniCulturali;

import java.util.Date;
import unisa.gps.etour.bean.BeanBeneCulturale;

/ **
  * This class has the task of checking the data of a cultural object.
  * Various consistency checks are performed, such as length of strings,
  * Null reference, dynamic types incorrect.
  *
  * @ Author Michelangelo De Simone
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
public class ControlloBeniCulturali
(
/ **
*
* Please consistency check by calling the appropriate methods.
* This method wrappa "all other methods of control with a single call.
* At the first false value the flow is interrupted.
*
* @ Param bean The PBC of the cultural object to be inspected
* @ Return boolean The result of the check: true if OK, false otherwise
* /
public static boolean controllaDatiBeneCulturale (BeanBeneCulturale PBC)
(
/ / This method checks the input parameter in the cases
/ / Null reference or dynamic Wrong
if (PBC == null | |! (PBC instanceof BeanBeneCulturale))
return false;

/ / This method checks if the ID passed as a parameter BeanBeneCulturale
/ / Is valid or not
if (! controllaIdBeneCulturale (pBC.getId ()))
return false;

/ / This method checks the objects contained in BeanBeneCulturale Date
/ / As such, a check is made null and reference checks
/ / On the dynamic.
if (! controllaDateBeneCulturale (PBC))
return false;

/ / This method checks all the fields in BeanBeneCulturale, research
/ / Any null references
if (! controllaDatiNulli (PBC))
return false;

/ / Check the correct length of string, in this case the CAP must
/ / To force of circumstances than five digits, while the province of two.
/ / TODO: To be completed
if (pBC.getCap (). length ()! = 5 | | pBC.getProvincia (). length ()! = 2)
return false;

return true;
)

/ **
*
* Check for null data in a bean cultural property.
* The check is performed on all fields of the bean.
*
* @ Param bean The PBC cultural property to be checked
* @ Return boolean The result of the check: true if there are no null references, false otherwise
* /
public static boolean controllaDatiNulli (BeanBeneCulturale PBC)
(
if (pBC.getCap () == null | | pBC.getCitta () == null | | pBC.getDescrizione () == null | | pBC.getGiornoChiusura () == null | |
pBC.getLocalita () == null | | pBC.getNome () == null | | pBC.getOrarioApertura () == null | |
pBC.getOrarioChiusura () == null | | pBC.getProvincia () == null | | pBC.getTelefono () == null | | pBC.getVia () == null)
return false;

return true;
)

/ **
*
* Check the consistency of dates within this BeanBeneCulturale.
* The check is performed only on objects, while not carried out
* No validity check 'on a date as a cultural object may also have
* Dates later than today (see for example on open exhibitions).
*
* @ Param The PBC BeanBeneCulturale which check the dates
* @ Return boolean The result of the check: true if the dates have consistency; false otherwise
* /
public static boolean controllaDateBeneCulturale (BeanBeneCulturale PBC)
(
if (pBC.getOrarioApertura () == null | |! (pBC.getOrarioApertura () instanceof Date) | |
pBC.getOrarioChiusura () == null | |! (pBC.getOrarioChiusura () instanceof Date))
return false;

return true;
)

/ **
*
* Check the ID of BeanBeneCulturale
*
* @ Param pId Id BeanBeneCulturale be checked
* @ Return boolean The result of the check: true if the ID is correct, false otherwise
* /
public static boolean controllaIdBeneCulturale (int pid)
(
return (pid> 0);
)
) 