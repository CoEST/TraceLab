/*
 * ConnectionPoolDataSource
 *
 */

package smos.storage.connectionManagement;

import smos.storage.connectionManagement.exception.NotImplementedYetException;

import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.Driver;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.List;
import java.util.Properties;
import java.util.Vector;
import javax.sql.DataSource;

/**
 * Realizzazione del pool di connessioni tramite l'implementazione
 * dell'interfaccia java.sql.DataSource. Il pool controlla periodicamente le
 * connessioni attive e quelle che sono pool, ossia quelle rilasciate ma ancora
 * utilizzabili (presenti cioe' in memoria). Il tempo di rilascio delle
 * connessioni attive e di quelle pool � rappresentato da due parametri presenti
 * all'interno della classe e che sono connectionPoolCloseTimeout e
 * inactiveMaxTimeout; tali valori cosi come tutti gli altri inerenti al pool
 * hanno un loro valore di default, parametrizzabile tramite il file di
 * properties connection.properties
 */

public class ConnectionPoolDataSource implements DataSource {

    /**
     * Thread inside della classe ConnectionPoolDataSource che stabilisce ogni
     * connectionPoolCloseTimeout millisecondi il rilascio delle connessioni
     * pool.
     */
    private class ConnectionCloser extends Thread {

        private long connectionActionTimestamp = 0;

        private int connectionPoolCloseTimeout = 300000;

        private long timeStamp = 0;

        /**
         * Costruttore che setta il tempo di rilascio delle connessioni pool
         * 
         * @author Di Giorgio Domenico, Cris Malinconico
         * @param pTime
         *            intervallo di tempo entro cui il pool svuota la lista
         *            delle connessioni pool.
         */
		private ConnectionCloser(int pTime) {
            setDaemon(true);
            setName("ConnectionPoolCloser");
            if (pTime > 0)
            	this.connectionPoolCloseTimeout = pTime;
        }

        /**
         * Ogni volta che una connessione genera un evento con un'invocazione di
         * getConnection() o release() il timestamp viene settato al valore
         * temporale corrente tramite questo metodo.
         */
		public void connectionEvent() {
			this.connectionActionTimestamp = System.currentTimeMillis();
        }

        /**
         * Controlla ogni connectionPoolCloseTimeout millisecondi se le
         * connessioni pool possono essere chiuse liberando in tal caso memoria.
         */
		public void run() {
            boolean working = true;
            while (working) {
                try {
                	this.timeStamp = System.currentTimeMillis();
                    Thread.sleep(this.connectionPoolCloseTimeout);
                    if (this.connectionActionTimestamp < this.timeStamp) {
                        closeAllConnections(ConnectionPoolDataSource.this.pool);
                    }
                } catch (InterruptedException e) {
                    working = false;
                    e.printStackTrace();
                } catch (SQLException e) {
                    working = false;
                    e.printStackTrace();
                }
            }
        }
    }

    private List<SMOSConnection> active = new Vector<SMOSConnection>();

    private Properties config = new Properties();

    private ConnectionCloser connectionCloser;

    private Driver driver;

    private String fullConnectionString;

    private long inactiveMaxTimeout = 20000;

    private int maxPoolSize;

    private List<Connection> pool = new Vector<Connection>();

    /**
     * Crea una nuova istanza del pool di connessioni.
     * 
     * @param pJdbcDriverName
     *            nome del driver jdbc
     * @param pFullConnectionString
     *            stringa di connessione con il database
     * @param pUser
     *            nome utente (amministratore del database)
     * @param pPassword
     *            password amministratore
     * @param pMaxPoolSize
     *            massimo numero di connessioni attive nel pool, deve essere
     *            maggiore di 0
     * @param pPoolTime
     *            intervallo di tempo entro il quale il pool sar� svuotato ogni
     *            volta delle sue connessioni pool (in ms).
     * @throws ClassNotFoundException
     *             se il driver jdbc non pu� essere trovato
     * @throws SQLException
     *             se occorre un problema durante la connessione al database
     * @throws IllegalArgumentException
     *             se i parametri forniti in input non sono validi
     */

    
	public ConnectionPoolDataSource(String pJdbcDriverName,
            String pFullConnectionString, String pUser, String pPassword,
            int pMaxPoolSize, int pPoolTime) throws ClassNotFoundException,
            SQLException {

        if (pMaxPoolSize < 1) {
            throw new IllegalArgumentException(
                    "maxPoolSize deve essere >0 ma �: " + pMaxPoolSize);
        }
        if (pFullConnectionString == null) {
            throw new IllegalArgumentException("fullConnectionString "
                    + "ha valore Null");
        }
        if (pUser == null) {
            throw new IllegalArgumentException("il nome utente ha valore Null");
        }
        this.maxPoolSize = pMaxPoolSize;
        this.fullConnectionString = pFullConnectionString;
        this.config.put("user", pUser);
        if (pPassword != null) {
            this.config.put("password", pPassword);
        }
        Class.forName(pJdbcDriverName);
        this.driver = DriverManager.getDriver(pFullConnectionString);
        this.connectionCloser = new ConnectionCloser(pPoolTime);
        this.connectionCloser.start();
    }

    /**
     * Restituisce la dimensione della lista delle connessioni attive.
     * 
     * @return la dimensione della lista delle connessioni attualmente attive.
     */
	public int activeSize() {
        return this.active.size();
    }

    /**
     * Svuota il pool di connessioni da quelle attive che non hanno pi� eseguito
     * operazioni per inactiveMaxTimeout millisecondi.
     * 
     */
    protected void clearActive() {
        long temp = 0;
        long TIME = System.currentTimeMillis();
        SMOSConnection adc = null;

        for (int count = 0; count < this.active.size(); count++) {
            adc = (SMOSConnection) this.active.get(count);
            temp = TIME - adc.getLastTime();
            if (temp >= this.inactiveMaxTimeout) {
                this.release(adc.getConnection());
            }
        }
    }

    /**
     * Chiude tutte le connessioni del pool sia quelle attive e sia quelle che
     * sono pool.
     * 
     * @author Di Giorgio Domenico, Cris Malinconico
     * @throws SQLException
     */
    public synchronized void closeAllConnections() throws SQLException {
        closeAllConnections(this.pool);
        closeAllConnections(this.active);
    }

    /**
     * Chiude tutte le connessioni indicate nella lista connection.
     * 
     * @author Di Giorgio Domenico, Cris Malinconico
     * @param pConnections
     *            la lista delle connesioni che devono essere chiuse.
     * @throws SQLException
     *             qualora sia impossibile chiudere una connessione.
     */
    private synchronized void closeAllConnections(List pConnections)
            throws SQLException {

        while (pConnections.size() > 0) {
            ConnectionWrapper conn = (ConnectionWrapper) pConnections.remove(0);
            conn.closeWrappedConnection();
        }
    }

    /**
     * Chiude tutte le connessioni del pool che sono nella lista pool.
     * 
     * @throws SQLException
     *             qualora sia impossibile chiudere una connessione.
     */
    public synchronized void closeAllPooledConnections() throws SQLException {
        closeAllConnections(this.pool);
    }

    /**
     * Metodo utilizzato da getConnection() per creare una nuova connessione
     * qualora nella lista delle pool non siano presenti.
     * 
     * @return una nuova connessione al DataBase.
     */
    private synchronized Connection createNewConnection() {
        Connection rawConn = null;
        try {
            rawConn = this.driver.connect(this.fullConnectionString, this.config);
            Connection conn = new ConnectionWrapper(rawConn, this);
            SMOSConnection ac = new SMOSConnection();
            ac.setConnection(conn);
            ac.setLastTime(System.currentTimeMillis());
            this.active.add(ac);
            return conn;
        } catch (SQLException e) {
            System.out.println("Creazione della connessione fallita "
                    + "in ConnectionPoolDataSource:" + e);
            return null;
        }
    }

    /**
     * Restituisce una connessione se il pool non � pieno, il controllo avviene
     * prima nella lista delle connessioni pool per evitare delle creazioni
     * inutili altrimenti una nuova connessione sar� creata.
     * 
     * @return la connessione al database qualora fosse possibile altrimenti
     *         un'eccezione viene generata
     * @see javax.sql.DataSource getConnection()
     * @throws SQLException
     *             Se un problema occorre durante la connessione al database
     *             incluso il fatto che il limite massimo delle connessioni
     *             attive venga raggiunto.
     */
    public synchronized Connection getConnection() throws SQLException {

        Connection connection = getPooledConnection(0);

        if (connection == null) {
            if (this.active.size() >= this.maxPoolSize) {
                throw new SQLException("Connection pool limit of "
                        + this.maxPoolSize + " exceeded");
            } else {
                connection = createNewConnection();
            }
        }
        this.connectionCloser.connectionEvent();
        //System.out.println("GET CONNECTION: " + active.size() + "/" + pool.size());
        return connection;
    }

    /**
     * Metodo non implementato
     * @param pArg1 
     * @param pArg2 
     * @return Connection
     * @throws SQLException 
     * 
     * @throws NotImplementedYetException
     */

    public Connection getConnection(String pArg1, String pArg2)
            throws SQLException {
        throw new NotImplementedYetException();
    }

    /**
     * Metodo non implementato
     * @return int
     * @throws SQLException 
     * 
     * @throws NotImplementedYetException
     */

    public int getLoginTimeout() throws SQLException {
        throw new NotImplementedYetException();
    }

    /**
     * Metodo non implementato
     * @return PrintWriter
     * @throws SQLException 
     * 
     * @throws NotImplementedYetException
     */

    public PrintWriter getLogWriter() throws SQLException {
        throw new NotImplementedYetException();
    }

    /**
     * Restituisce il numero massimo di connessioni attive
     * 
     * @return il numero massimo di connessioni attive.
     */

    public int getMaxPoolSize() {
        return this.maxPoolSize;
    }

    /**
     * Metodo utilizzato da getConnection() per stabilire se nella lista delle
     * connessioni pool ve ne sia qualcuna da poter riutilizzare.
     * 
     * @param pPoolIndex
     *            indice della lista delle connessioni pool (sempre 0).
     * @return una connesssione dalla lista di quelle pool qualora ne esista
     *         una.
     */
    private synchronized Connection getPooledConnection(int pPoolIndex) {
        SMOSConnection ac = new SMOSConnection();
        Connection connection = null;
        if (this.pool.size() > 0) {
            connection = (Connection) this.pool.remove(pPoolIndex);
            ac.setConnection(connection);
            ac.setLastTime(System.currentTimeMillis());
            this.active.add(ac);
        }
        return ac.getConnection();
    }

    /**
     * Restituisce la dimensione della lista delle connessioni pool
     * 
     * @return la dimensione della lista delle connessioni pool.
     */
    public int poolSize() {
        return this.pool.size();
    }

    /**
     * Rilascia una connessione, eliminandola da quelle attive ed inserendola in
     * quelle pool per poter essere successivamente riutilizzata.
     * 
     * @param pConnection
     *            La connessione che deve essere ritornata al pool.
     */
    public synchronized void release(Connection pConnection) {
        boolean exists = false;
        int activeIndex = 0;

        if (pConnection != null) {
            SMOSConnection adc = null;
            while ((activeIndex < this.active.size()) && (!exists)) {
                adc = (SMOSConnection) this.active.get(activeIndex);
                if (adc.equals(pConnection)) {
                	this.active.remove(adc);
                	this.pool.add(adc.getConnection());
                    exists = true;
                }
                activeIndex++;
            }
            this.connectionCloser.connectionEvent();
            //System.out.println("RELEASE CONNECTION: " + active.size() + "/" + pool.size());
        }
    }

    /**
     * Setta il tempo di vita delle connessioni attive in millisecondi.
     * 
     * @param pTimeOut
     *            tempo di vita della connessione.
     */

    public void setActivedTimeout(long pTimeOut) {
        if (pTimeOut > 0) {
        	this.inactiveMaxTimeout = pTimeOut;
        }
    }

    /**
     * Riazzera il tempo di vita della connessione dovutocall'esecuzione di
     * un'operazione.Da questo momento la connessione potr� essere attiva senza
     * eseguire alcuna operazione per altri inactiveMaxTimeout millisecondi.
     * 
     * @param pConnection
     *            la connessione che ha eseguito un'operazione e quindi pu�
     *            rimanere attiva.
     */

    void setLastTime(Connection pConnection) {
        boolean exists = false;
        int count = 0;
        SMOSConnection adc = null;

        while ((count < this.active.size()) && (!exists)) {
            adc = (SMOSConnection) this.active.get(count);
            count++;
            if (adc.equals(pConnection)) {
                adc.setLastTime(System.currentTimeMillis());
                exists = true;
            }
        }
    }

    /**
     * Metodo non implementato
     * @param pArg0 
     * @throws SQLException 
     * 
     * @throws NotImplementedYetException
     */
    public void setLoginTimeout(int pArg0) throws SQLException {
        throw new NotImplementedYetException();
    }

    /**
     * Metodo non implementato
     * @param pArg0 
     * @throws SQLException 
     * 
     * @throws NotImplementedYetException
     */
    public void setLogWriter(PrintWriter pArg0) throws SQLException {
        throw new NotImplementedYetException();
    }

    /**
     * Converte un oggetto della classe ConnectionPoolDataSource in String
     * 
     * @return la rappresentazione nel tipo String del pool di connessioni.
     */

    public String toString() {
        StringBuffer buf = new StringBuffer();

        buf.append("[");
        buf.append("maxPoolSize=").append(this.maxPoolSize);
        buf.append(", activeSize=").append(activeSize());
        buf.append(", poolSize=").append(poolSize());
        buf.append(", fullConnectionString=").append(this.fullConnectionString);
        buf.append("]");
        return (buf.toString());
    }

	
}
