	
package unisa.gps.etour.control.GestioneRicerche;

import java.sql.SQLException;
import java.util.ArrayList;

/ **
  * Class for managing Standard Search
  *
  * @ Author Joseph Penna
  * @ Version 0.1 © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class Search extends RicercaStandard
(

/ ** Constructor of the class
*
* /
public RicercaStandard ()
(

)

protected int ottieniNumeroElementiRicercaSpecializzato ()
throws SQLException
(
/ / Check the type of site and gets the number of results
switch (pTipologiaSito)
(
homes BENE_CULTURALE:
return BeneCulturale.ottieniNumeroElementiRicerca (
this.pParoleChiave, this.pTags, this.pPosizioneUtente,
this.pRaggioMax);
homes PUNTO_DI_RISTORO:
return PuntoDiRistoro.ottieniNumeroElementiRicerca (
this.pParoleChiave, this.pTags, this.pPosizioneUtente,
this.pRaggioMax);
default:
return -1;
)
)

protected ArrayList <?> ricercaSpecializzataPerPagina (int pNumeroPagina)
throws SQLException
(
/ / Check the type of site and search
switch (pTipologiaSito)
(
homes BENE_CULTURALE:
return BeneCulturale.ricerca (pParoleChiave, pTags,
pNumeroPagina, numeroElementiPerPagina,
pPosizioneUtente, pRaggioMax);
homes PUNTO_DI_RISTORO:
return PuntoDiRistoro.ricerca (pParoleChiave, pTags,
pNumeroPagina, numeroElementiPerPagina,
pPosizioneUtente, pRaggioMax);
default:
return null;

)
)
) 