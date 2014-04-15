package unisa.gps.etour.control.GestioneBeniCulturali.test.stub;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.repository.IDBTag;

public class DBTag implements IDBTag
(
private ArrayList <BeanTag> b;

public DBTag ()
(
b = new ArrayList <BeanTag> (0);
)

public boolean aggiungeTagBeneCulturale (pIdBeneCulturale int, int pIdTag)
throws SQLException
(
return true;
)

public boolean aggiungeTagPuntoDiRistoro (pIdPuntoDiRistoro int, int pIdTag)
throws SQLException
(
return true;
)

public boolean cancellaTag (int pIdTag) throws SQLException
(
return true;
)

public boolean cancellaTagBeneCulturale (pIdBeneCulturale int, int pIdTag)
throws SQLException
(
return true;
)

public boolean cancellaTagPuntoDiRistoro (pIdPuntoDiRistoro int, int pIdTag)
throws SQLException
(
return true;
)

public boolean inserisciTag (BeanTag ptagi) throws SQLException
(
return true;
)

public boolean modificaTag (BeanTag ptagi) throws SQLException
(
/ / TODO Auto-generated method stub
return false;
)

<BeanTag> ottieniListaTag public ArrayList () throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)

public BeanTag ottieniTag (int pid) throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)

<BeanTag> ottieniTagBeneCulturale public ArrayList (int pIdBeneCulturale)
throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)

<BeanTag> ottieniTagPuntoDiRistoro public ArrayList (int pIdPuntoDiRistoro)
throws SQLException
(
/ / TODO Auto-generated method stub
return null;
)

) 