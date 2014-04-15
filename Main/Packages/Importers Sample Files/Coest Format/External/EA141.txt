package Bean;
/**
 * La classe Accesso permette la gestione degli accessi
 * La classe Accesso non ha dipendenze
 * @author Federico Cinque
 */
public class Accesso {
	
	private String Login;
	private String Password;
	private String Tipo;
	
	/**
	 * Costruttore vuoto della classe Accesso
	 */
	public Accesso(){
		Login=null;
		Password=null;
		Tipo=null;
	}
	/**
	 * Costruttore della classe Accesso
	 * @param Login
	 * @param Password
	 * @param Tipo
	 */
	public Accesso(String Login, String Password, String Tipo){
		this.Login=Login;
		this.Password=Password;
		this.Tipo=Tipo;
	}
	/**
	 * Metodo che restituisce una login
	 * @return Login
	 */
	public String getLogin() {
		return Login;
	}
	/**
	 * Metodo che imposta una login
	 * @param login
	 */
	public void setLogin(String login) {
		Login = login;
	}
	/**
	 * Metodo che restituisce una password
	 * @return Password
	 */
	public String getPassword() {
		return Password;
	}
	/**
	 * Metodo che imposta una password
	 * @param password
	 */
	public void setPassword(String password) {
		Password = password;
	}
	/**
	 * Metodo che restituisce il tipo di utente che accede
	 * @return Tipo
	 */
	public String getTipo() {
		return Tipo;
	}
	/**
	 * Metodo che imposta il tipo di utente che accede 
	 * @param tipo
	 */
	public void setTipo(String tipo) {
		Tipo = tipo;
	}
	/**
	 * Metodo che converete in una stringa le informazioni di un accesso
	 * @return String
	 */
	public String toString() {
		return "Login: "+Login+", Password: "+Password+", Tipo: "+Tipo;
	}
}