/*
 * AdaptorException
 *
 */

package smos.storage.connectionManagement.exception;

import java.rmi.RemoteException;

/**
 * Thrown when a problem occurs running code in ensj.
 */
public class AdaptorException extends RemoteException {

    private static final long serialVersionUID = 1L;

    /**
     * 
     */
    public AdaptorException() {
        super();
    }

    /**
     * @param pMessage
     */
    public AdaptorException(String pMessage) {
        super(pMessage + buildLabel());
    }

    /**
     * @param pMessage
     * @param pParentException
     */
    public AdaptorException(String pMessage, Exception pParentException) {
        super(pMessage + buildLabel(), pParentException);
    }

    /**
     * @param pParentException
     */
    public AdaptorException(Exception pParentException) {
        super(buildLabel(), pParentException);
    }

    private static String buildLabel() {
        return " [1]";
    }
}
