package unisa.gps.etour.control.fuzzy;

import java.io.Serializable;
import java.util.Hashtable;

public class Category implements Serializable
(

/ **
* Class that describes the characteristics of a Category
* Contains a Hashtable that represents the dictionary on
* The category that contains and for each term associated
* In the category values of frequency, distance and relevance.
* Provides methods to access, modify and auxiliary methods.
* /
private static final long serialVersionUID =-8652232946927756089L;
private String name; / / name of the category
private Hashtable <string, float[]> terms, / / list of terms and their frequencies and distance rilavanza

/ *
* Manufacturer:
* Get the category name as a parameter to create
* /
public Category (String Pnom)
(
name = Pnom;
terms <String,float[]> = new Hashtable ();
)

/ *
* Returns the output Hashtable containing the terms
* With the respective values of frequency, relevance and distance
* /
<string, float[]> getTermini public Hashtable ()
(
return terms;
)

/ *
* Returns the name of the output category
* /
public String getName ()
(
return name;
)

/ *
* Get the string as a parameter representing the term
* Of which you want to pick the values of frequency, range and bearing
* /
public float [] getval (pTermine String) throws NullPointerException
(
if (esisteTermine (pTermine))
return termini.get (pTermine);

return null;
)

/ *
* Agiunge an end to dizinario category
* /
public void addTermine (String pTermine)
(
termini.put (pTermine, new float [3]);
)

/ *
* Agiunge an end to dizinario category
* Seven also the values of frequency, distance and rilavanza
* /
public boolean addTermine (String pTermine, float [] pVal)
(
if ((pVal == null) | | (pTermine.equals ("")))
return false;
termini.put (pTermine, pVal);

return true;
)

/ *
* Set the values for the period pTermine
* /
public boolean setValTermine (String pTermine, float [] pVal)
throws NullPointerException
(
if (esisteTermine (pTermine))
(
termini.put (pTermine, pVal);
return true;
)

return false;
)


public void setTermini (Hashtable <String,float[]> pTermini)
(
Term = pTermini;
)

/ *
* Returns True if the term is present in
* Dictionary of Category False otherwise
* /
public boolean esisteTermine (String pTermine)
(
TRY
(
float [] ret = termini.get (pTermine);
if (ret! = null)
return true;
)
catch (NullPointerException e)
(
return false;
)

return false;
)

) 