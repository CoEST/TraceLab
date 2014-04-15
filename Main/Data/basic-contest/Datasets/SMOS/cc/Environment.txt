package smos;

/**
 * Classe utilizzata per contenere le variabili d'ambiente di GESA 
 */
public class Environment {

    /**
     * Messaggio di errore di default.
     */
    public static String DEFAULT_ERROR_MESSAGE = "Un errore si e' verificato durante l'elaborazione della richiesta.<br><br>";

    private static String poolPropertiesPath = "";

    /**
     * @return getPoolPropertiesPath()
     */
    public static String getPoolPropertiesPath() {
        return poolPropertiesPath;
    }
    
    /**
     * @param poolPropertiesPath
     */
    public static void setPoolPropertiesPath(String poolPropertiesPath) {
        Environment.poolPropertiesPath = poolPropertiesPath;
    }
}