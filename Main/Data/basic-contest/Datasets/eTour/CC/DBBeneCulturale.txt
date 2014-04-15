package unisa.gps.etour.control.GestioneBeniCulturali.test.stub;

import java.sql.SQLException;
import java.util.ArrayList;
import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.repository.IDBBeneCulturale;
import unisa.gps.etour.util.Punto3D;

public class DBBeneCulturale implements IDBBeneCulturale
(
private ArrayList <BeanBeneCulturale> b;

public DBBeneCulturale ()
(
b = new ArrayList <BeanBeneCulturale> (0);
)

public boolean cancellaBeneCulturale (int pIdBene) throws SQLException
(
boolean res = false;

for (int i = 0; i <b.size (); i + +)
if (b.get (i). getId () == pIdBene)
(
b.remove (i);
res = true;
)

return res;
)

public boolean inserisciBeneCulturale (BeanBeneCulturale pBene)
throws SQLException
(
return (b.add (pBene));
)

public boolean modificaBeneCulturale (BeanBeneCulturale pBene)
throws SQLException
(
boolean res = false;

for (int i = 0; i <b.size (); i + +)
if (b.get (i). getId () == pBene.getId ())
(
b.set (i, pBene);
return true;
)

return res;
)

public BeanBeneCulturale ottieniBeneCulturale (int pid) throws SQLException
(
BeanBeneCulturale res = null;

for (int i = 0; i <b.size (); i + +)
if (b.get (i). getId () == pid)
res = b.get (i);

return res;
)

<BeanBeneCulturale> ottieniListaBC public ArrayList () throws SQLException
(
return b;
)

public int ottieniNumeroElementiRicerca (String pKeyword,
ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / TODO Auto-generated method stub
return 0;
)

public int ottieniNumeroElementiRicercaAvanzata (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / TODO Auto-generated method stub
return 0;
)

public ArrayList <BeanBeneCulturale> search (String pKeyword,
ArrayList <BeanTag> pTags, int pNumPagina,
int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)

<BeanBeneCulturale> ricercaAvanzata public ArrayList (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, int pNumPagina,
int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)
) 