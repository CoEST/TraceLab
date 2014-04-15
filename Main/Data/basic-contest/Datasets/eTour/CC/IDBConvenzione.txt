package unisa.gps.etour.repository;

import java.sql.SQLException;
import java.util.ArrayList;

import unisa.gps.etour.bean.BeanConvenzione;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;

/ **
  * Interface for managing the database Business
  *
  * @ Author Joseph Martone
  * @ Version 0.1 2007 eTour Project - Copyright by SE @ SA Lab DMI University
  * Of Salerno
  * /
public interface IDBConvenzione
(

/ **
* Add a convention in the database
*
* @ Param pConvenzione Convention by adding
* @ Throws SQLException
* /
public boolean inserisciConvenzione (BeanConvenzione pConvenzione)
throws SQLException;

/ **
* Modify a convention in the database
*
* @ Param data pConvenzione Convention of the Convention to be updated
* @ Return True if there 'was a modified false otherwise
* @ Throws SQLException
* /
public boolean modificaConvenzione (BeanConvenzione pConvenzione)
throws SQLException;

/ **
* Delete an agreement by the database
*
* @ Param pIdConvenzione ID of the Convention by removing
* @ Return True if been deleted false otherwise
* @ Throws SQLException
* /
public boolean cancellaConvenzione (int pIdConvenzione) throws SQLException;

/ **
* Returns the historical conventions of a refreshment
*
* @ Param idPuntoDiRistoro point identification Refreshments
* @ Return List of conventions of Refreshment given as argument
* @ Throws SQLException
* /
public ArrayList <BeanConvenzione> ottieniStoricoConvenzione (
idPuntoDiRistoro int) throws SQLException;

/ **
* Returns the Convention active a refreshment
*
* @ Param pIdPuntoDiRistoro point identification Refreshments
* @ Return Convention Turns
* @ Throws SQLException
* /
public BeanConvenzione ottieniConvezioneAttiva (int pIdPuntoDiRistoro)
throws SQLException;

/ **
* Returns a list of all the PR that have a Convention active
*
* @ Return list of all the PR with the Convention active
* @ Throws SQLException
* /
<BeanPuntoDiRistoro> ottieniListaConvenzioneAttivaPR public ArrayList ()
throws SQLException;

) 