package unisa.gps.etour.control.GestioneBeniCulturali.test.stub;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.bean.BeanVisitaPR;
import unisa.gps.etour.repository.IDBVisitaBC;

public class DBVisitaBC implements IDBVisitaBC
(
public boolean inserisciVisitaBC (BeanVisitaBC PVIS) throws SQLException
(
return false;
)

public boolean modificaVisitaBC (BeanVisitaBC PVIS) throws SQLException
(
return false;
)

<BeanVisitaBC> ottieniListaVisitaBC public ArrayList (int pIdBeneCulturale)
throws SQLException
(
<BeanVisitaBC> FinteVisite ArrayList = new ArrayList <BeanVisitaBC> (0);

finteVisite.add (new BeanVisitaBC (4, 1, "beautiful exhibition", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (3, 1, "show particular but interesting", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (4, 1, "beautiful exhibition", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (3, 1, "show particular but interesting", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (4, 1, "beautiful exhibition", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (3, 1, "show particular but interesting", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (4, 1, "beautiful exhibition", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (3, 1, "show particular but interesting", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (4, 1, "beautiful exhibition", 1, new Date ()));
finteVisite.add (new BeanVisitaBC (3, 1, "show particular but interesting", 1, new Date ()));


/ / FinteVisite.add (new BeanVisitaBC (5, 1, "This show is not 'evil', 1, new Date (new Date (). GetTime () - (unisa.gps.etour.util.CostantiGlobali.TRENTA_GIORNI * 1 ))));
finteVisite.add (new BeanVisitaBC (5, 1, "This show is not 'evil', 1, new Date (new Date (). getTime () - 2591000000L)));
finteVisite.add (new BeanVisitaBC (3, 1, "This show is not 'evil', 1, new Date (new Date (). getTime () - (unisa.gps.etour.util.CostantiGlobali.TRENTA_GIORNI * 2)) ));


/ / FinteVisite.add (new BeanVisitaBC (1, 1, "E 'nice but you pay so much!", 2, new Date (106, 0, 23)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "The food is pretty good!", 1, new Date (106, 3, 23)));
/ / FinteVisite.add (new BeanVisitaBC (4, 1, "We eat very well!", 2, new Date (107, 4, 4)));
/ / FinteVisite.add (new BeanVisitaBC (1, 1, "We eat!", 3, new Date (107, 5, 24)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "Beautiful place", 4, new Date (107, 4, 25)));
/ / FinteVisite.add (new BeanVisitaBC (4, 1, "Excellent views of the sea", 5, new Date (107, 4, 25)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "Bell", 6, new Date (107, 4, 25)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "I think it's a bad place", 7, new Date (107, 4, 26)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "W open air", 8, new Date (107, 4, 27)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "better than others", 9, new Date (107, 5, 2)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "Forza Napoli", 10, new Date (107, 5, 8)));
/ / FinteVisite.add (new BeanVisitaBC (4, 1, "The food is pretty good!", 11, new Date (107, 5, 9)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "We eat very well!", 12, new Date (107, 5, 11)));
/ / FinteVisite.add (new BeanVisitaBC (4, 1, "very good", 13, new Date (107, 5, 12)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "very good", 14, new Date (107, 5, 13)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "I was really good", 15, new Date (107, 5, 13)));
/ / FinteVisite.add (new BeanVisitaBC (4, 1, "good place", 16, new Date (107, 5, 14)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "I guess I'm not going back", 17, new Date (107, 5, 23)));
/ / FinteVisite.add (new BeanVisitaBC (3, 1, "I think there is better", 18, new Date (107, 5, 24)));
/ / FinteVisite.add (new BeanVisitaBC (2, 1, "sucks", 19, new Date (107, 5, 24)));
/ / FinteVisite.add (new BeanVisitaBC (5, 1, "Too beautiful", 20, new Date (107, 5, 25)));

finteVisite return;
)

<BeanVisitaBC> ottieniListaVisitaBCTurista public ArrayList (int pIdTurista)
throws SQLException
(
return null;
)

public BeanVisitaBC ottieniVisitaBC (pIdBeneCulturale int, int pIdTurista)
throws SQLException
(
return null;
)
)
