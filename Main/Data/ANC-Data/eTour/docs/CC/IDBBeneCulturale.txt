package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.util.Punto3D;

/ **
  * Interface for the management of cultural heritage database
  *
  * @ Author Mauro Miranda
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /

public interface IDBBeneCulturale
(
/ **
* Add a cultural heritage, given input
*
* @ Param pBene Cultural Heritage for inclusion in database
* @ Throws SQLException
* /
public boolean inserisciBeneCulturale (BeanBeneCulturale pBene)
throws SQLException;

/ **
* Modify the information in the cultural
*
* @ Param pBene contains the information to modify the database
* @ Throws SQLException
* @ Return True if there 'was a modified false otherwise
* /
public boolean modificaBeneCulturale (BeanBeneCulturale pBene)
throws SQLException;

/ **
* Delete a cultural object from the database
*
* @ Param ID pIdBene cultural property to delete
* @ Throws SQLException
* @ Return True if and 'was deleted false otherwise
* /
public boolean cancellaBeneCulturale (int pIdBene) throws SQLException;

/ **
* Returns the cultural object with id as input
*
* @ Param pId cultural property to be extracted from the database
* @ Throws SQLException
* @ Return cultural property obtained from the database
* /
public BeanBeneCulturale ottieniBeneCulturale (int pid) throws SQLException;

/ **
* Research. Returns the list of cultural property in their name or
* Description given string as input, filtered according to tags and
* Maximum distance. The returned list contains the number of goods given as input.
* To browse the real list, which may contain more 'of
* Ten elements, you use the paramtro numPagina.
*
PKeyword * @ param string that contains the keyword to search the
* Name or description of the cultural
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pNumPagina The page number you want to view. O for
* 1 page (the first 10 results), 1 for 2 page (s
* Results from 11 to 20) etc ...
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to search for good
* @ Param pNumeroElementiPerPagina number of items to return per page
* @ Throws SQLException
* @ Return list contained ten cultural
* /
public ArrayList <BeanBeneCulturale> search (String pKeyword,
ArrayList <BeanTag> pTags, pNumPagina int, int pNumeroElementiPerPagina, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;

/ **
* Advanced Search. Returns the list of cultural goods which have in
* Name or description given string as input, sorted according to
* Preferences of tourists and filtered according to the tag and the maximum distance. The
* Returned list contains the number of goods given as input. To scroll
* The actual list, which may contain multiple 'items, you
* Use paramtro numPagina.
*
* @ Param ID pIdTurista tourists who carried out the research
PKeyword * @ param string that contains the keyword to search the
* Name or description of the cultural
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pNumPagina The page number you want to view. O for
* 1 page (the first 10 results), 1 for 2 page (s
* Results from 11 to 20) etc ...
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to search for good
* @ Param pNumeroElementiPerPagina number of items to return per page
* @ Throws SQLException
* @ Return list contained ten cultural
* /
<BeanBeneCulturale> ricercaAvanzata public ArrayList (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, pNumPagina int, int pNumeroElementiPerPagina,
Punto3D pPosizione, double pDistanzaMassima) throws SQLException;

/ **
* Method to get the number of elements to search.
*
PKeyword * @ param string that contains the keyword to search the
* Name or description of the cultural
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to search for good
* @ Throws SQLException
* @ Return number of pages.
* /
public int ottieniNumeroElementiRicerca (String pKeyword,
ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;

/ **
* Method to get the number of elements to search.
*
* @ Param identifier pIdTurista tourists who carried out the research
PKeyword * @ param string that contains the keyword to search the
* Name or description of the cultural
* @ Param pTags list of tags used to filter the search. the
* Maximum number of tags to be included should not exceed five
* Units'. If you exceed this number the other tags
* Excess will be ignored.
* @ Param pPosizione position of the person who carried out the research
* @ Param pDistanzaMassima Maximum distance from the user to search for good
* @ Throws SQLException
* @ Return number of pages.
* /
public int ottieniNumeroElementiRicercaAvanzata (int pIdTurista,
PKeyword String, ArrayList <BeanTag> pTags, Punto3D pPosizione,
double pDistanzaMassima) throws SQLException;
/ **
* Returns a list of all cultural
*
* @ Throws SQLException
* @ Return List of all cultural
* /
<BeanBeneCulturale> ottieniListaBC public ArrayList () throws SQLException;
) 