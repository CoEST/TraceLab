package unisa.gps.etour.control.GestioneBeniCulturali;

import unisa.gps.etour.bean.BeanVisitaBC;

/ **
  * This class has the task of monitoring data from a cultural visit.
  * Various consistency checks are performed, such as length of strings,
  * Null reference, dynamic types incorrect.
  *
  * @ Author Michelangelo De Simone
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
public class ControlloVisiteBeniCulturali
(
/ **
*
* Please consistency check by calling the appropriate methods
*
* @ Param bean The pVBC cultural visit to check
* @ Return boolean The result of the check: true if OK, false otherwise
* /
public static boolean controllaDatiVisitaBeneCulturale (BeanVisitaBC pVBC)
(
/ / If the bean is null
if (pVBC == null | |! (pVBC instanceof BeanVisitaBC))
return false;

/ / Check the ID of a cultural visit
/ / And the id of its tourist
if (! (pVBC.getIdBeneCulturale ()> 0) | |! (pVBC.getIdTurista ()> 0))
return false;

/ / Check the vote (of course ratings are accepted only between 1 and 5
if (! (pVBC.getVoto ()> = 1 & & pVBC.getVoto () <= 5))
return false;

/ / Check for null references in the bean
if (! controllaDatiNulli (pVBC))
return false;

/ / Check the correct length of string
if (! (pVBC.getCommento (). length ()> 0))
return false;

return true;
)

/ **
*
* Check for null data in a bean cultural visit
*
* @ Param bean The PBC cultural visit to check
* @ Return boolean The result of the check: true if there are no null references, false otherwise
* /
public static boolean controllaDatiNulli (BeanVisitaBC PBC)
(
/ / Check the nullity of the fields of feedback
if (pBC.getCommento () == null | | pBC.getDataVisita () == null)
return false;

return true;
)
) 