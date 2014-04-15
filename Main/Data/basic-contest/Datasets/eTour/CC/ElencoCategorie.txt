package unisa.gps.etour.control.fuzzy;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.List;

public class ElencoCategorie implements Serializable
(

/ **
* Keeps track of data in each category
* /
private static final long serialVersionUID = 1L;
private Hashtable <string, Categoria> categories, / / hash table that keeps
/ / For each category a
/ / Class category
private Hashtable <string, float[]> totTermini / / hash table that keeps
/ / The terms of all
/ / Categories
maxDist float, / / contains the maximum distances

/ **
* The constructor initializes the two hash tables that contain
* Categories and terms of all categories
* /
public ElencoCategorie ()
(
categories <string, Categoria> = new Hashtable ();
totTermini <string, float[]> = new Hashtable ();
)

/ **
* Access method attribute maxDist
*
* @ Return the maximum distance of all the terms in all categories
* /
public float getMaxDist ()
(
maxDist return;
)

/ **
* Access method to the table of categories
*
* @ Return categories
* /
<string, Categoria> getAllCategorie public Hashtable ()
(
return categories;
)

/ **
* Method of accessing the table of total time
*
* @ Return totTermini
* /
<string, float[]> getTotTermini public Hashtable ()
(
totTermini return;
)

/ **
* Method to access a category in the table of
* Categories
*
* @ Param pNomeCategoria
* @ Return object categories representing the category name
* PNomeCategoria
* /
getCategoria public Category (String pNomeCategoria)
(

if (esisteCategoria (pNomeCategoria)) / / if there is the appropriate category
return categorie.get (pNomeCategoria) / / returns the
/ / Assciato to pNomeCategoria

return null, / / otherwise null
)

/ **
* Method of accessing the values of a particular term in this
* Category table
*
* @ Param pTermine
* @ Return Returns the values associated with the term pTermine
* /
public float [] getTermine (String pTermine)
(
if (esisteTermine (pTermine)) / / if the term is present in tebella
/ / Terms of total
return (float []) totTermini.get (pTermine) / / return the vaolre
/ / Associate

return null, / / null otherwise
)

/ **
* Method which allows you to add a category to the table of
* Categories
*
* @ Param pNomeCategoria category name to add
* @ Param object associated pCategoria category
* @ Return true if the operation was successfully carried out false
* Otherwise
* /
public boolean addCategoria (String pNomeCategoria, star pCategoria)
(
if (! esisteCategoria (pNomeCategoria)) / / if the category exists
return false; / / returns false

categorie.put (pNomeCategoria, pCategoria), / / otherwise load the
/ / Category in the table

return true; / / returns true
)

/ **
* Edit a category of the category table
*
* @ Param pNomeCategoria category name to edit
* @ Param object pCategoria be associated with this category
* @ Return true if the operation was successfully carried out false
* Otherwise
* /
public boolean setCategoria (String pNomeCategoria, star pCategoria)
(
if (esisteCategoria (pNomeCategoria)) / / if the category does not exist
return false; / / returns false

categorie.put (pNomeCategoria, pCategoria) / / update the table of
/ / Catogorie

return true; / / returns true
)

/ **
* Method which allows you to set the value of a term in the tables
* Total time
*
* @ Param name pTermine term
* @ Param pVal value to associate with the term
* /
public void setTermine (String pTermine, float [] pVal)
(

totTermini.put (pTermine, pVal);
)

/ **
* Method which allows the value of the seven kings of the maximum distance of
* Terms from one category
*
* @ Param pMaxDist
* /
public void setMaxDist (float pMaxDist)
(
maxDist = pMaxDist;
)

/ **
* Method which allows to derive a collection of names of iterable
* All categories in the categories tabela
*
* @ Return string iterable Collection
* /
public Iterable <String> Categories ()
(
List <String> toReturn <String> = new ArrayList ();// create a new list
for (Enumeration <String> categorie.keys val = (); val.hasMoreElements ();)// iterates
/ / N
/ / Times
/ / Where
/ / N is
/ / The
/ / Number
/ / By
/ / Categories
/ / Current
/ / In
/ / Table
(
toReturn.add (val.nextElement ());// adds to the list the name of
/ / A category
)

toReturn return;
)

/ **
* Method aids to verify the existence of a category
* In the table of categories
*
* @ Param name of the category PKEY
* @ Return true if the category exists false otherwise
* /
public boolean esisteCategoria (String PKEY)
(
TRY
(
categorie.get (PKEY), / / try to extract the category name PKEY
/ / The table of categories
return true; / / if the transaction does not raise exceptions category
/ / Exists and returns true
)
catch (NullPointerException e)
(
return false; / / false otherwise
)
)

/ **
* Method aids to verify the existence of a term
* In the table of total time
*
* @ Param PKEY term
* @ Return true if the term exists false otherwise
* /
public boolean esisteTermine (String PKEY)
(
/ / see esisteCategoria
TRY
(
if (totTermini.get (PKEY)! = null)
return true;
)
catch (NullPointerException e)
(
return false;
)

return false;
)

) 