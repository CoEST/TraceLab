package unisa.gps.etour.util;

/ **
  * Standard error message self-describing
  *
  * @ Author Michelangelo De Simone
  * @ Version 0.1
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab - University of Salerno
  * /
public class MessaggiErrore
(
/ / Occurs when connecting to the DBMS
public static final String ERRORE_CONNESSIONE_DBMS = "Connection to DBMS Failed";

/ / Occurs when you can not perform an operation on the DBMS
public static final String ERRORE_DBMS = "Error DBMS;

/ / It occurs in conditions not specified
public static final String ERRORE_SCONOSCIUTO = "Unknown error";

/ / Occurs when there are format errors in a bean
public static final String ERRORE_FORMATO_BEAN = "Error data bean;

/ / Occurs when a data error
public static final String ERRORE_DATI = "Data Error";

/ / Occurs when an error occurs on read / write files
public static final String ERRORE_FILE = "Error reading / writing file";

/ / Occurs when you have reached the maximum number of banners displayed
public static final String ERRORE_NUM_BANNER = "count exceeded banner inserted";
)
