package smos.application;

import javax.servlet.ServletConfig;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;

import smos.utility.Utility;

/**
 * Servlet utilizzata per inizializzare i parametri del sistema.
 * 
 * @author Bavota Gabriele, Carnevale Filomena.
 *
 */
public class ServletInitialize extends HttpServlet {

	private static final long serialVersionUID = -2542143445249797492L;
	
	@SuppressWarnings("unused")
	private ServletConfig config;
	
	 /**

     * Inizializza i parametri

     */

    public void init(ServletConfig config) throws ServletException 

    {
    	this.config = config;
    	               
        
        //Setto il server smtp specificato nel file di configurazione xml
        Utility.setServerSmtp(config.getInitParameter("serverSmtp"));
        
        //Setto i parametri necessari alla connessione al Database
        Utility.setDriverMySql(config.getInitParameter("driverMySql"));
        Utility.setFullPathDatabase(config.getInitParameter("fullPathDatabase"));
        Utility.setUserName(config.getInitParameter("userName"));
        Utility.setPassword(config.getInitParameter("password"));
        Utility.setMaxPoolSize(Integer.valueOf(config.getInitParameter("maxPoolSize")));
        Utility.setWaitTimeout(Integer.valueOf(config.getInitParameter("waitTimeout")));
        Utility.setActiveTimeout(Integer.valueOf(config.getInitParameter("activeTimeout")));
        Utility.setPoolTimeout(Integer.valueOf(config.getInitParameter("poolTimeout")));
        Utility.setTextFooter(config.getInitParameter("textFooter"));
        
        
	}

}
