package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.util.Punto3D;


/ **
  * Interface for management of eateries in the database
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBPuntoDiRistoro
(
/ **
* Add a refreshment
*
* @ Param pPuntoDiRistoro Refreshment to add
* @ Throws SQLException
* /
public boolean inserisciPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws SQLException;

/ **
* Modify a refreshment
*
* @ Param pPuntoDiRistoro Refreshment to edit
* @ Throws SQLException
* @ Return True if and 'been changed otherwise false
* /
public boolean modificaPuntoDiRistoro (BeanPuntoDiRistoro pPuntoDiRistoro)
throws SQLException;

/ **
* Delete a refreshment
*
* @ Param ID pIdPuntoDiRistoro Refreshment to eliminate
* @ Throws SQLException
* @ Return True if and 'have been deleted false otherwise
* /
public boolean cancellaPuntoDiRistoro (int pIdPuntoDiRistoro)
throws SQLException;

/ **
* Returns data from a point of comfort with the ID given as argument
*
* @ Param pId point identification Refreshments
* @ Throws SQLException
* @ Return Refreshment
* /
public BeanPuntoDiRistoro ottieniPuntoDiRistoro (int pid)
throws SQLException;

/ **
* Advanced Search. Returns the list of eateries that have in
* Name or description given string as input, sorted according to
* Preferences of tourists, the tags and filtered according to the distance
* Max. The list returned contains only the number of catering outlets input data.
* To scroll the real list, which may contain multiple 'items, you
* Use paramtro numPagina.
*
* @ Param Id pIdTurista tourists who carried out the research
PKeyword * @ param string that contains the keyword to search the
* Name or description of refreshment
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pNumeroPagina The page number you want to view. O
* The 1 page (the first 10 results), 1 for 2 page (s
* Results from 11 to 20) etc ... *
* @ Param pPosizione position of the person who carried out the research
* @ Param int Number of elements to return pNumeroElementiPerPagina
* @ Param pDistanzaMassima Maximum distance from the user to refreshment
* To seek
* @ Throws SQLException
* @ Return list containing ten points Refreshments
* /
<BeanPuntoDiRistoro> ricercaAvanzata public ArrayList (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, pNumeroPagina int, int pNumeroElementiPerPagina,
Punto3D pPosizione, double pDistanzaMassima) throws SQLException;

/ **
* Method to get the number of elements to search.
*
* @ See ricercaAvanzata ()
* @ Param Id pIdTurista tourists who carried out the research
PKeyword * @ param string that contains the keyword to search the
* Name or description of refreshment
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored. *
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to refreshment
* To seek
* @ Throws SQLException
* @ Return number of pages.
* /
public int ottieniNumeroElementiRicercaAvanzata (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;

/ **
* Research. Returns the list of eateries that have the name or
* Description given string as input, filtered and tags
* According to the maximum distance. The returned list contains the number of
* Points Refreshments input data. To scroll the real list, which
* May contain more 'items, you use the paramtro
* NumPagina.
*
PKeyword * @ param string that contains the keyword to search the
* Name or description of refreshment
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pNumeroPagina The page number you want to view. O
* The 1 page (the first 10 results), 1 for 2 page (s
* Results from 11 to 20) etc ... *
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to refreshment
* @ Param int Number of elements to return pNumeroElementiPerPagina
* @ Throws SQLException
* @ Return list containing ten points Refreshments
* /
public ArrayList <BeanPuntoDiRistoro> search (String pKeyword,
ArrayList <BeanTag> pTags, pNumeroPagina int, int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;

/ **
* Method to get you the elements for an advanced search.
*
* @ See search ()
* @ Param username pUsernameTurista tourists who carried out the research
PKeyword * @ param string that contains the keyword to search the
* Name or description of refreshment
* @ Param pTags list of tags used to filter the search. The
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to refreshment
* To seek
* @ Throws SQLException
* @ Return number of pages.
* /
public int ottieniNumeroElementiRicerca (String pKeyword,
ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;

/ **
* Returns a list of all the refreshment
*
* @ Throws SQLException
* @ Return list of all the refreshment
* /
<BeanPuntoDiRistoro> ottieniListaPR public ArrayList () throws SQLException;
) 