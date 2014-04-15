/*
 * DBConnection
 *
 */

package smos.storage.connectionManagement;

import smos.Environment;
import smos.utility.Utility;

import java.sql.*;
import java.util.*;
import java.io.*;


/**
 * Classe che s'interfaccia con il pool di connessioni. In particolare crea un
 * unico oggetto ConnectionPoolDataSource (il pool di connessioni) ed ottiene i
 * suoi parametri di configurazione dal file di properties
 * connection.properties.
 */

public class DBConnection {

    private static int ACTIVE_TIMEOUT;

    private static String DRIVER_MYSQL = "";

    private static String FULL_PATH_DATABASE = "";

    private static ControlConnection linker = null;

    private static ConnectionPoolDataSource manager = null;

    private static int MAX_POOL_SIZE;

    private static String PASSWORD = "";

    private static int POOL_TIMEOUT;

    private static Properties properties = null;

    private static String USER_NAME = "";

    private static int WAIT_TIMEOUT;

    
   
    
    
    /**
     * Blocco d'inizializzazione statico che si occupa di generare il pool nel
     * momento in cui ci sarà una prima invocazione del metodo getConnection()
     */

    static {
        try {
            properties = new Properties();
            File fileProp = new File(Environment.getPoolPropertiesPath());

            if (fileProp.exists()) {
                properties.load(new FileInputStream(fileProp));

                DRIVER_MYSQL = properties.getProperty("connection.jdbc.name");
                if (DRIVER_MYSQL.equals("")) {
                    DRIVER_MYSQL = Utility.getDriverMySql();
                }

                FULL_PATH_DATABASE = properties
                        .getProperty("connection.jdbc.fullPath");

                if (FULL_PATH_DATABASE.equals("")) {
                    FULL_PATH_DATABASE = Utility.getFullPathDatabase();
                }

                USER_NAME = properties.getProperty("connection.username");
                if (USER_NAME.equals("")) {
                    USER_NAME = Utility.getUserName();
                }

                PASSWORD = properties.getProperty("connection.password");
                if (PASSWORD.equals("")) {
                    PASSWORD = Utility.getPassword();
                }

                try {
                    MAX_POOL_SIZE = Integer.parseInt(properties
                            .getProperty("connection.maxPoolSize"));
                } catch (Exception ex) {
                    MAX_POOL_SIZE = Utility.getMaxPoolSize();
                }

                try {
                    WAIT_TIMEOUT = Integer.parseInt(properties
                            .getProperty("connection.waitTimeout"));
                } catch (Exception ex) {
                    WAIT_TIMEOUT = Utility.getWaitTimeout();
                }

                try {
                    ACTIVE_TIMEOUT = Integer.parseInt(properties
                            .getProperty("connection.activeTimeout"));
                } catch (Exception ex) {
                    ACTIVE_TIMEOUT = Utility.getActiveTimeout();
                }

                try {
                    POOL_TIMEOUT = Integer.parseInt(properties
                            .getProperty("connection.poolTimeout"));
                } catch (Exception ex) {
                    POOL_TIMEOUT = Utility.getPoolTimeout();
                }
            } else {
                /* Se il file di properties non esiste carica valori di default */

                DRIVER_MYSQL = Utility.getDriverMySql();
                FULL_PATH_DATABASE = Utility.getFullPathDatabase();
                USER_NAME = Utility.getUserName();
                PASSWORD = Utility.getPassword();
                MAX_POOL_SIZE = Utility.getMaxPoolSize();
                WAIT_TIMEOUT = Utility.getWaitTimeout();
                ACTIVE_TIMEOUT = Utility.getActiveTimeout();
                POOL_TIMEOUT = Utility.getPoolTimeout();
            }

            loadPool(); // Crea il manager e prepara il pool di connessioni

        } catch (Exception e) {
            /* Se un'eccezione viene generata in precedenza */

        	DRIVER_MYSQL = Utility.getDriverMySql();
            FULL_PATH_DATABASE = Utility.getFullPathDatabase();
            USER_NAME = Utility.getUserName();
            PASSWORD = Utility.getPassword();
            MAX_POOL_SIZE = 100;
            WAIT_TIMEOUT = 2000;
            ACTIVE_TIMEOUT = 240000;
            POOL_TIMEOUT = 300000;
            loadPool(); // Crea il manager e prepara il pool di connessioni
        }

    }

    /**
     * Restituisce una connessione dal pool.
     * 
     * @return la connessione se possibile null altrimenti
     */

    public static Connection getConnection() {
        try {
            return manager.getConnection();
        } catch (SQLException e) {
            System.out.println("Eccezione generata"
                    + "in DBConnection.getConnection() " + e);
            return null;
        }
    }

    /**
     * Creazione effettiva del pool di connessione.
     * 
     */
    private static void loadPool() {
        try {
            manager = new ConnectionPoolDataSource(DRIVER_MYSQL,
                    FULL_PATH_DATABASE, USER_NAME, PASSWORD, MAX_POOL_SIZE,
                    POOL_TIMEOUT);
            manager.setActivedTimeout(ACTIVE_TIMEOUT);
            linker = new ControlConnection(manager, WAIT_TIMEOUT);
            linker.start();
        } catch (Exception e) {
            System.out.println("Impossibile creare il pool"
                    + "di connessioni in DBConnection:" + e);
        }
    }

    /**
     * Restituisce una connessione al pool che sarà inserita nella lista delle
     * connesioni pool, ossia quelle riutilizzabili in seguito.
     * 
     * @param pConnection
     *            la connessione non più attiva.
     */

    public static void releaseConnection(Connection pConnection) {
        manager.release(pConnection);
    }

}
