/*
 * NotYetImplementedException
 *
 */

package smos.storage.connectionManagement.exception;

/**
 * This exception is thrown as a warning from a part of the code which has not
 * been implemented yet, but will be in future.
 */
public class NotImplementedYetException extends RuntimeException {
    private static final long serialVersionUID = 1L;

    /**
     * 
     */
    public NotImplementedYetException() {
        super();
    }

    /**
     * @param pMessage
     */
    public NotImplementedYetException(String pMessage) {
        super(pMessage);
    }

}
