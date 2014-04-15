/*
 * ConnectionWrapper
 *
 */

package smos.storage.connectionManagement;


import java.io.Serializable;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.SQLWarning;
import java.sql.Savepoint;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Map;
import java.util.logging.Logger;

/**
 * ConnectionWrapper è una classe che aggiunge a tutti i metodi della classe
 * Connection il settaggio del tempo in cui l'operazione sulla particolare
 * connessione è avvenuta informando il pool di quanto accaduto.
 */

public class ConnectionWrapper implements Connection, Serializable {
    private static final long serialVersionUID = 1L;

    private static final Logger LOGGER = Logger
            .getLogger(ConnectionWrapper.class.getName());

    private Connection connection;

    private ConnectionPoolDataSource manager;

    private ArrayList<Statement> statements = new ArrayList<Statement>();

    /**
     * @param pConnection
     * @param pPoolManager
     */
    public ConnectionWrapper(Connection pConnection,
            ConnectionPoolDataSource pPoolManager) {
        this.connection = pConnection;
        this.manager = pPoolManager;
        LOGGER.fine("Creating ConnectionWrapper");
    }

    private PreparedStatement cachePreparedStatement(PreparedStatement pPrepSt) {
        this.manager.setLastTime(this);
        this.statements.add(pPrepSt);
        return pPrepSt;
    }

    private Statement cacheStatement(Statement pStatement) {
        this.manager.setLastTime(this);
        this.statements.add(pStatement);
        return pStatement;
    }

    /**
     * @see java.sql.Connection#clearWarnings()
     */
    public void clearWarnings() throws SQLException {
        this.connection.clearWarnings();
    }

    /**
     * @see java.sql.Connection#close()
     */
    public void close() throws SQLException {
        closeAndReleaseStatements();
        this.manager.release(this);
    }

    private synchronized void closeAndReleaseStatements() throws SQLException {
        final int n = this.statements.size();
        for (int i = 0; i < n; i++) {
            ((Statement) this.statements.get(i)).close();
        }
        this.statements.clear();
    }

    /**
     * Close the wrapped connection.
     * @throws SQLException 
     */
    void closeWrappedConnection() throws SQLException {
        closeAndReleaseStatements();
        if (!this.connection.isClosed()) {
            LOGGER.fine("Closing db connection: " + this.getClass().getName()
                    + " [" + this + "]");
        }
        this.connection.close();
    }

    /**
     * @see java.sql.Connection#commit()
     */
    public void commit() throws SQLException {
        this.manager.setLastTime(this);
        this.connection.commit();
    }

    /**
     * @see java.sql.Connection#createStatement()
     */
    public Statement createStatement() throws SQLException {
        this.manager.setLastTime(this);
        return cacheStatement(this.connection.createStatement());
    }

    /**
     * @see java.sql.Connection#createStatement(int, int)
     */
    public Statement createStatement(int pResultSetType,
            int pResultSetConcurrency) throws SQLException {
        this.manager.setLastTime(this);
        return cacheStatement(this.connection.createStatement(pResultSetType,
                pResultSetConcurrency));
    }

    /**
     * @see java.sql.Connection#createStatement(int, int, int)
     */
    public Statement createStatement(int pResultSetType,
            int pResultSetConcurrency, int pResultSetHoldability)
            throws SQLException {
        this.manager.setLastTime(this);
        return cacheStatement(this.connection.createStatement(pResultSetType,
                pResultSetConcurrency, pResultSetHoldability));
    }

    /**
     * Closes the wrapped connection.
     */
    protected void finalize() throws Throwable {
        closeWrappedConnection();
    }

    /**
     * @see java.sql.Connection#getAutoCommit()
     */
    public boolean getAutoCommit() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getAutoCommit();
    }

    /**
     * @see java.sql.Connection#getCatalog()
     */
    public String getCatalog() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getCatalog();
    }

    /**
     * @see java.sql.Connection#getHoldability()
     */
    public int getHoldability() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getHoldability();
    }

    /**
     * @see java.sql.Connection#getMetaData()
     */
    public DatabaseMetaData getMetaData() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getMetaData();
    }

    /**
     * @see java.sql.Connection#getTransactionIsolation()
     */
    public int getTransactionIsolation() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getTransactionIsolation();
    }

    /**
     * @see java.sql.Connection#getTypeMap()
     */
    @SuppressWarnings("unchecked")
	public Map getTypeMap() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getTypeMap();
    }

    /**
     * @see java.sql.Connection#getWarnings()
     */
    public SQLWarning getWarnings() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.getWarnings();
    }

    /**
     * @see java.sql.Connection#isClosed()
     */
    public boolean isClosed() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.isClosed();
    }

    /**
     * @see java.sql.Connection#isReadOnly()
     */
    public boolean isReadOnly() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.isReadOnly();
    }

    /**
     * @see java.sql.Connection#nativeSQL(java.lang.String)
     */
    public String nativeSQL(String sql) throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.nativeSQL(sql);
    }

    /**
     * @see java.sql.Connection#prepareCall(java.lang.String)
     */
    public CallableStatement prepareCall(String sql) throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.prepareCall(sql);
    }

    /**
     * @see java.sql.Connection#prepareCall(java.lang.String, int, int)
     */
    public CallableStatement prepareCall(String pStatementSql,
            int pResultSetType, int pResultSetConcurrency) throws SQLException {

        this.manager.setLastTime(this);
        return this.connection.prepareCall(pStatementSql, pResultSetType,
                pResultSetConcurrency);
    }

    /**
     * @see java.sql.Connection#prepareCall(java.lang.String, int, int, int)
     */
    public CallableStatement prepareCall(String pStatementSql,
            int pResultSetType, int pResultSetConcurrency,
            int pResultSetHoldability) throws SQLException {

        this.manager.setLastTime(this);
        return this.connection.prepareCall(pStatementSql, pResultSetType,
                pResultSetConcurrency, pResultSetHoldability);
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String)
     */
    public PreparedStatement prepareStatement(String pStatementSql)
            throws SQLException {
        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection
                .prepareStatement(pStatementSql));
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String, int)
     */
    public PreparedStatement prepareStatement(String pStatementSql,
            int pAutoGeneratedKeys) throws SQLException {

        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection.prepareStatement(
                pStatementSql, pAutoGeneratedKeys));
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String, int, int)
     */
    public PreparedStatement prepareStatement(String pStatementSql,
            int pResultSetType, int pResultSetConcurrency) throws SQLException {

        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection.prepareStatement(
                pStatementSql, pResultSetType, pResultSetConcurrency));
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String, int, int, int)
     */
    public PreparedStatement prepareStatement(String pStatementSql,
            int pResultSetType, int pResultSetConcurrency,
            int pResultSetHoldability) throws SQLException {

        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection.prepareStatement(
                pStatementSql, pResultSetType, pResultSetConcurrency,
                pResultSetHoldability));
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String, int[])
     */
    public PreparedStatement prepareStatement(String pStatementSql,
            int[] columnIndexes) throws SQLException {
        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection.prepareStatement(
                pStatementSql, columnIndexes));
    }

    /**
     * @see java.sql.Connection#prepareStatement(java.lang.String, java.lang.String[])
     */
    public PreparedStatement prepareStatement(String pStatementSql,
            String[] pColumnNames) throws SQLException {
        this.manager.setLastTime(this);
        return cachePreparedStatement(this.connection.prepareStatement(
                pStatementSql, pColumnNames));
    }

    /**
     * @see java.sql.Connection#releaseSavepoint(java.sql.Savepoint)
     */
    public void releaseSavepoint(Savepoint pSavepoint) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.releaseSavepoint(pSavepoint);
    }

    /**
     * @see java.sql.Connection#rollback()
     */
    public void rollback() throws SQLException {
        this.manager.setLastTime(this);
        this.connection.rollback();
    }

    /**
     * @see java.sql.Connection#rollback(java.sql.Savepoint)
     */
    public void rollback(Savepoint pSavepoint) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.rollback(pSavepoint);
    }

    /**
     * @see java.sql.Connection#setAutoCommit(boolean)
     */
    public void setAutoCommit(boolean pAutoCommit) throws SQLException {
        this.manager.setLastTime(this);
        if (this.connection.getAutoCommit() != pAutoCommit) {
            this.connection.setAutoCommit(pAutoCommit);
        }
    }

    /**
     * @see java.sql.Connection#setCatalog(java.lang.String)
     */
    public void setCatalog(String pCatalog) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.setCatalog(pCatalog);
    }

    /**
     * @see java.sql.Connection#setHoldability(int)
     */
    public void setHoldability(int pHoldability) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.setHoldability(pHoldability);
    }

    /**
     * @see java.sql.Connection#setReadOnly(boolean)
     */
    public void setReadOnly(boolean pReadOnly) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.setReadOnly(pReadOnly);
    }

    /**
     * @see java.sql.Connection#setSavepoint()
     */
    public Savepoint setSavepoint() throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.setSavepoint();
    }

    /**
     * @see java.sql.Connection#setSavepoint(java.lang.String)
     */
    public Savepoint setSavepoint(String pName) throws SQLException {
        this.manager.setLastTime(this);
        return this.connection.setSavepoint(pName);
    }

    /**
     * @see java.sql.Connection#setTransactionIsolation(int)
     */
    public void setTransactionIsolation(int pLevel) throws SQLException {
        this.manager.setLastTime(this);
        this.connection.setTransactionIsolation(pLevel);
    }

    /**
     * @see java.sql.Connection#setTypeMap(java.util.Map)
     */
    

    /**
     * @see java.lang.Object#toString()
     */
    public String toString() {
        return this.connection.toString();
    }

	public void setTypeMap(Map<String, Class<?>> arg0) throws SQLException {
		// TODO Auto-generated method stub
		
	}

	

}
