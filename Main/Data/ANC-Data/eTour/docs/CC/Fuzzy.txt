package unisa.gps.etour.control.fuzzy;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.rmi.RemoteException;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Scanner;

public class Fuzzy
(
/ **
* Class that implements the methods used to calculate the Fuzzy
* Category of membership of a refreshment or a cultural property.
* /

/ **
* Method for calculating the relevance of a term.
*
* @ Param distance: Contains the distance of the term from category
* Analyzed
* @ Param pMaxDist: Contains distaza maximum of all terms in all
* Categories.
* @ Return Returns the relevance of the term in the category.
* /
private static float relevance (float distance, float pMaxDist)
(
return (distance / pMaxDist);

)

/ **
* Method for calculating the distance between a term and a category
*
* @ Param pTermine: Contains the period analyzed
* @ Param pTotTermini: Tables of the total frequency of terms.
* @ Param frequenzaTesto: Table of the terms of the text analyzed.
* @ Param pCategoria: Category analyzed.
* @ Return Returns the distance of the term pTermine by category
* PCategoria
* /
private static float distance (String pTermine,
Hashtable <string, float[]> pTotTermini, float frequenzaTesto,
Category pCategoria)
(

/ / The first variable tracks the frequency of a term
/ / Relating to a category
/ / The second keeps track of fraquenza a deadline for all
/ / Categories
float [] frequenzaCategoria = new float [3], frequenzaTotale = new float [3];

if (pCategoria.esisteTermine (pTermine)) / / if the term is
/ / In category
(
/ / Its frequency in this category is equal to the frequency
/ / KnowledgeBase
/ / More frequency in the search text
frequenzaCategoria = pCategoria.getVal (pTermine);
frequenzaCategoria [0] + = frequenzaTesto;
)
else
(

/ / Otherwise it is equal to the frequency of the term in the text
/ / Analyzed
frequenzaCategoria [0] = frequenzaTesto;
)
if (pTotTermini.get (pTermine)! = null) / / if the term exists in
/ / Table of the total time
frequenzaTotale = pTotTermini.get (pTermine) / / Get the value

frequenzaTotale [0] + = frequenzaTesto, / / the total frequency is given
/ / Frequency in the text
/ / Analizzatp
/ / Plus any frequency stored in memory in the table
/ / Total time

return (frequenzaCategoria [0] / frequenzaTotale [0 ]);// distance is
/ / Equal to
/ / Frequency in
/ / Category
/ / The total frequency Fratto

)

/ **
* Method for calculating the distance of a term from one category.
* Used for training
*
* @ Param pTermine period to analyze
* @ Param pCategoria category from which you must calculate the distance
* @ Param table pTotTermini total time
* @ Return Returns the distance of a term from one category
* /
private static float distance (String pTermine, star pCategoria,
Hashtable <string, float[]> pTotTermini)
(
return ((pCategoria.getVal (pTermine)) [0] / (pTotTermini.get (pTermine)) [0]);
)

/ **
* Implementation of a function T-Norm
*
* @ Param a first value
* @ Param b the second value
* @ Return returns the value calculated using a function T-Norm
* /
private static float tNorm (float a, float b)
(
return ((a * b) / ((2 - ((a + b) - (a * b )))));
/ / Return Math.min (a, b);
/ / Return a * b;
/ / Return Math.max (0, a + b-1);
)

/ **
* Implementation of a function S-Norm
*
* @ Param a first value
* @ Param b the second value
* @ Return returns the value calculated using a function S-Norm
* /
private static float sNorm (float a, float b)
(
return ((a + b) / (1 + a * b));
/ / Return Math.max (a, b);
/ / Return (a + b-(a * b));
/ / Return Math.min (1.1 + b);
)

/ **
* Calculation of similarity between a category and a given text input
*
* @ Param table pTermini worded. It must contain to
* Pgni term values of importance and belonging situated in
Vector float * in positions 1 and 2.
* @ Return returns a numeric value that indicates the similarity of a
* Text with the category on which one has calculated the values of
Importance and belonging *
* /
private static float similarity (Hashtable <string, float[]> pTermini)
(
float sum = 0, / / return value

for (float [] val: pTermini.values ())// for all elements of
/ / Table
(
sum + = (tNorm (val [1], val [0])) / (sNorm (val [1], val [0 ]));// performs
/ / Sum of the values given by the division of function T-Norm
/ / With the function S-Norm made
/ / Between relevance and belonging
)

return sum;
)

/ **
* Method for the calculation of membership of a text to a category
*
* @ Param val indicates the similarity of a text with a category
* @ Param maxSimilarity indicates the maximum similarity found
* @ Return restiuisce a value in the interval [0,1] that indicates the degree of
* Membership of the text to the category x
* /
private static float membership (float val, float maxSimilarity)
(
return (val / maxSimilarity);

)

/ **
* Method to delete a tense special characters and to bring
* All uppercase to lowercase
*
* @ Param string pStr transform
* @ Return restiuisce the text to lowercase characters and no special
* /
private static String replaceAndLower (String pStr)
(
pStr pStr.toLowerCase = ();
pstr = pStr.replace (",", "");
pstr = pStr.replace (".", "");
pStr pStr.replace ("!", = "");
pStr pStr.replace ("?", = "");
pStr pStr.replace ("'", = "");

pStr return;
)

/ **
* Method of retrieving the category you belong to a text
*
* @ Param pDescrizione text to analyze
Restu * @ return a string indicating the category
* @ Throws RemoteException
* /
public static String calcolaCategoria (String pDescrizione)
throws RemoteException
(
if ((pDescrizione == null) | | (pDescrizione.equals ("")))
return "NULL";
String text = pDescrizione;
/ / Table of terms associated with the text portion. Will contain
/ / Values of frequency, rilevamza, membership for each term
Hashtable <string, float[]> datiTesto <string, float[]> = new Hashtable ();
/ / Table of categories, each category will contain the value of
/ / Similarity and belonging Text
Hashtable <string, float[]> testoCategoria <string, float[]> = new Hashtable ();
replaceAndLower text = (text) / / delete characters and spaeciali
/ / Returns the text by replacing
/ / Uppercase with lowercase
String [] testoSplit = testo.split ( "");
for (int i = 0; i <testoSplit.length i + +)
(/ / For each end of the text
float [] toPut = new float [3], / / value to assign to the string
/ / In the hash table
float [] valTmp / / temporary variable containing the values
/ / Associated with the string if it already exists in the hash table
/ / If the string is present in the table picks up the values
/ / And an increase in saving them in to put
/ / Otherwise initialize the new string with frequency = 1
if (testoSplit [i]. length () <= 3) / / delete undefined terms
/ / As important, the inter ...
continue;
if (exists (testoSplit [i], datiTesto)) / / if the time analyzed
/ / Is present in tebella the terms of the analyzed text
(
/ / We get the value of frequency in the table and there
/ / Adds one
valTmp = datiTesto.get (testoSplit [i]);
toPut [0] = valTmp [0];
toPut [0] + = (float) 1 / testoSplit.length;

)
else
(
toPut [0] = (float) 1 / testoSplit.length;
/ / Otherwise initialize the value of frequency to a
/ / Fratto, the total number of terms (relative frequency)
)
/ / Insert the new entry in the table
/ / System.out.println (toPut [0]);
datiTesto.put (testoSplit [i], toPut);
)

/ / You try to open the knowledge base
ElencoCategorie list;
TRY
(
apriElenco list = ();
)
catch (ClassNotFoundException e) / / error opening file kb.sbt
(
throw new RemoteException (
"The knowledge base is missing or corrupt");
)
catch (Exception e)
(
throw new RemoteException (
"The knowledge base is missing or corrupt");
)

/ / Is taken from the base of knowledge to the table of total time
<string, Float[]> totTermini = elenco.getTotTermini Hashtable ();
float maxSimilarity = -1, / / holds the value of maximum similarity
for (String CategoryName: elenco.Categorie ())
(/ / For all the categories in the knowledge base
float [] toPut = new float [3], / / value to assign to the string
/ / In the hash table
for (Enumeration <String> datiTesto.keys val = (); val
. hasMoreElements ();)
(/ / For all elements of the table of the terms of the text
String term = val.nextElement ();
/ / We get the value of a term
float [] tmp = datiTesto.get (term);
/ / Calculate range and bearing
tmp [1] = distance (term totTermini, tmp [0], list
. getCategoria (CategoryName));
tmp [2] = importance (tmp [1], elenco.getMaxDist ());
datiTesto.put (term, tmp);
)
/ / We calculate the similarity Once the analysis
/ / All the terms in a category
toPut [0] = similarity (datiTesto);
testoCategoria.put (CategoryName, toPut);
if (maxSimilarity <toPut [0]) / / we update the value of maximum
/ / If necessary similarity
(
maxSimilarity = toPut [0];
)
)

for (String CategoryName: elenco.Categorie ())
(/ / For each category
/ / We get the value of similarity of the text with the category
/ / Analyzed
float [] tmp = testoCategoria.get (CategoryName);
tmp [1] = membership (tmp [0], maxSimilarity), / / we calculate
/ / Membership
/ / Text to the similarity
testoCategoria.put (CategoryName, tmp), / / save everything in
/ / Category table
)

return maxAppartenenza (testoCategoria) / / returns the name output
/ / Category
/ / With the maximum degree of membership
)

/ **
* Method to find the category with which the text has the highest degree of
* Membership
*
* @ Param pTestoCategoria table of categories to the text
Restiuisce * @ return a string indicating the name of the category with which
* The text has the highest degree of membership
* /
private static String maxAppartenenza (
Hashtable <string, float[]> pTestoCategoria)
(
ToReturn String = null, / / return value
float max = -1, / / Maximum value of membership

for (Enumeration <String> pTestoCategoria.keys elm = (); elm
. hasMoreElements ();)
(/ / For all categories of the table of categories of text
String category = elm.nextElement ();
/ / Values are taken of similarity and belonging associated with
/ / Category
float [] tmp = pTestoCategoria.get (category);
if (tmp [1]> max)
(/ / If the degree of membership affiliation just uploaded
/ / Is greater than the previous update data max and toReturn
toReturn = category;
max = tmp [1];
)

)
toReturn return;
)

/ **
* Method used to check whether a term is presented in table
* The terms of the text
*
* @ Param pStr period to analyze
* @ Param pTable tables in terms of the text
* @ Return returns true if the term exists false otherwise
* /
private static boolean exists (String pStr, Hashtable <string, float[]> pTable)
(
TRY
(
if (pTable.get (pStr)! = null)
return true;
)
catch (NullPointerException e)
(
return false;
)

return false;
)

/ **
* Method used to retrieve the knowledge base
*
Restiuisce * @ return an object representing the type ElencoCategorie
* KnowledgeBase
* @ Throws IOException
* @ Throws ClassNotFoundException
* /
private static ElencoCategorie apriElenco () throws IOException,
ClassNotFoundException
(
KBase file = new File ( "kb.sbt ");// you open the file kb.sbt
FileInputStream kBaseStream = new FileInputStream (KBase) / / creates
/ / A stream with the file
ObjectInputStream kBaseObj = new ObjectInputStream (kBaseStream), / / si
/ / Create a stream object with the file
ElencoCategorie toReturn;

toReturn = (ElencoCategorie) kBaseObj.readObject ();
/ / Object is extracted and saved in the file returned in output
toReturn return;
)

/ **
* Method used to create the file. Used in training
*
* @ Param path string indicating the path in which to create the file
* @ Return returns an ObjectOutputStream to the file created
* @ Throws IOException
* /
private static ObjectOutputStream CreateFile (String path)
throws IOException
(
ObjectOutputStream toReturn;
File f = new File (path) / / file is created
if (f.exists ())
f.delete ();

FileOutputStream fout = new FileOutputStream (path);
toReturn = new ObjectOutputStream (fOut), / / create the stream

toReturn return;
)

/ **
* Method used to create the knowledge base
*
* @ Throws RemoteException
* /
public static void training () throws RemoteException
(

String [] elencoCategorie = new String [4] / / array contenentei names
/ / Of categrie be analyzed
/ / Knowledge base
ElencoCategorie list = new ElencoCategorie ();

ObjectOutputStream elencoOut;
TRY
(
/ / Try to create the file
elencoOut = CreateFile ( "kb.sbt");
)
catch (Exception e)
(
throw new RemoteException ( "Error creating file kb.sbt");
)

elencoCategorie [0] = "art";
elencoCategorie [1] = "cinema";
elencoCategorie [2] = "sport";
/ / ElencoCategorie [3 ]="";

for (int i = 0; i <3; the ++)// for each category
(
/ / Create a new object of type Category, which will contain all
/ / Category data to be analyzed
ToPutCat category = new Category (elencoCategorie [i]);
/ / If the inclusion of the category in the table of categories
/ / Not successful
/ / We throw an exception
if (! elenco.addCategoria (elencoCategorie [i], toPutCat))
(
throw new RemoteException (
"Error creating data of category"
+ ElencoCategorie [i]);
)
/ / You try to read from the folder containing the lyrics of a
/ / Category
/ / 100 sample test
for (Integer j = new Integer (1), j <= 100; j + +)
(
/ / Path of the folder categria
String path = "kb /" + elencoCategorie [i] + "/" + j.ToString ();
/ / Try to read the file ith
FileReader testoReader;
TRY
(
testoReader = new FileReader (path);
)
catch (FileNotFoundException e)
(
/ / If the file does not exist it continues execution from
/ / File i +1
/ / System.out.println ( "Error on file" + path);
continue;
)
TestoScanner scanner = new Scanner (testoReader);
while (testoScanner.hasNextLine ())
(
/ / Read the text file line by line
TestoScanner.nextLine txt = String ();
txt = replaceAndLower (txt);
String [] toIterate = txt.split ( "");
for (int k = 0 k <toIterate.length k + +)
(/ / For each end of the line
if (toIterate [k]. length () <= 3) / / remove the effect
/ / Undefined terms
/ / Relevant
continue;
float [] valTerm, valTotTerm;
/ / If the term is present in the table of terms
/ / The class analyzed
if (elenco.getCategoria (elencoCategorie [i])
. esisteTermine (toIterate [k]))
(
/ / Its frequency is equal to the value stored in
/ / Table plus one fratto the total number of
/ / Terms of the text
valTerm = elenco.getCategoria (elencoCategorie [i])
. getval (toIterate [k]);
valTerm [0] + = (float) 1 / toIterate.length;
valTotTerm = elenco.getTermine (toIterate [k]);
valTotTerm [0] + = (float) 1 / toIterate.length;
)
else
(
/ / otherwise it is equal to one fratto the total number of words of text
valTerm = new float [3];
valTotTerm = new float [1];
valTerm [0] = (float) 1 / toIterate.length;
valTotTerm [0] = (float) 1 / toIterate.length;
)
/ / save the values calculated in the table of terms of the category analyzed
elenco.setTermine (toIterate [k], valTotTerm);
elenco.getCategoria (elencoCategorie [i]). addTermine (
toIterate [k], valTerm);
if (elenco.getMaxDist () <valTotTerm [0])
elenco.setMaxDist (valTotTerm [0]);
)
)

)
)
for (String CategoryName: elenco.Categorie ())
(/ / for each category
/ / is preflushed the table of terms
Hashtable elenco.getCategoria <string, float[]> terminiCategoria = (
CategoryName). getTermini ();

/ / all the terms are analyzed in the table of loaded terms
for (Enumeration enumTerm <String> terminiCategoria.keys = (); enumTerm
. hasMoreElements ();)
(
/ / is effattuato calculating bearing and distance
String term = enumTerm.nextElement ();
float [] val = terminiCategoria.get (term);
val [1] = distance (term elenco.getCategoria (CategoryName)
elenco.getTotTermini ());
val [2] = importance (val [1], elenco.getMaxDist ());
)
/ / data is stored in the table of the terms of the class
elenco.getCategoria (CategoryName). setTermini (terminiCategoria);
)
TRY
(/ / writing the results of operations on files
elencoOut.writeObject (list);
)
catch (Exception e)
(
throw new RemoteException ( "Error writing file");
)
)
) 