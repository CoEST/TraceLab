package Bean;
import java.util.Date;

/**
 * è un JavaBean che gestisce i metodi 
 * get e set degli attributi di un Cittadino
 * @author Francesco
 *
 */
public class Cittadino {
	private CartaIdentita ci;
    private int idCittadino;
    private String CodiceFiscale;
    private String Cognome;
    private String Nome;
    private Date DataNascita;
    private String LuogoNascita;
    private String Email;
    private boolean Advertise;
    private int nucleoFamiliare;
    private String Login;
    
	/**
	 * costruttore di default vuoto
	 */
	public Cittadino() {
		this.Advertise=false;
		this.CodiceFiscale="";
		this.Cognome="";
		this.DataNascita=null;
		this.LuogoNascita="";
		this.Email="";
		
		this.Login="";
		this.Nome="";
		
	}
	public Cittadino(String code,String cog,String nome,String res,String via){
		ci.setNumero(code);
		this.Cognome=cog;
		this.Nome=nome;
		ci.setResidenza(res);
		ci.setVia(via);
	}
	public Cittadino(int nf,String codfis,String cog,String name,Date data,String luogo){
		this.nucleoFamiliare=nf;
		this.CodiceFiscale=codfis;
		this.Cognome=cog;
		this.Nome=name;
		this.DataNascita=data;
		this.LuogoNascita=luogo;
		
	}
		// TODO Auto-generated constructor stub
	/**
	 * costruttore parametrico che crea l'oggetto
	 * cittadino con i dati inseriti da quest'ultimo 
	 * all'atto della registrazione nel sistema comunale
	 */
	public Cittadino(int id,String cod_fis,String surname,String name,Date data,String luogo,String mail,boolean adv,int nf,String l){
		this.idCittadino=id;
		this.CodiceFiscale=cod_fis;
		this.Cognome=surname;
		this.Nome=name;
		this.DataNascita=data;
		this.LuogoNascita=luogo;
		this.Email=mail;
		this.Advertise=adv;
		this.nucleoFamiliare=nf;
		this.Login=l;
	}
	public String getLogin(){
		return Login;
	}
	public void setLogin(String log){
		Login=log;
	}
	public int getIdCittadino(){
		return idCittadino;
	}
	public void setIdCittadino(int idCittadino) {
		this.idCittadino = idCittadino;
	}
	public String getCognome(){
		return Cognome;
	}
	public void setCognome(String surname){
		Cognome=surname;
	}
	public String getNome(){
		return Nome;
	}
	public void setNome(String name){
		Nome=name;
	}
	public Date getDataNascita(){
		return DataNascita;
	}
	public void setDataNascita(Date data){
		DataNascita=data;
	}
	public void setLuogoNascita(String luogo){
		LuogoNascita=luogo;
	}
	public String getLuogoNascita(){
		return LuogoNascita;
	}
	public String getEmail(){
		return Email;
	}
	public void setEmail(String mail){
		Email=mail;
	}
	public boolean isAdvertise(){
		return Advertise;
	}
	public void setIsAdvertise(boolean ad){
		Advertise=ad;
	}
	public void setNucleoFamiliare(int nf)
	{
		nucleoFamiliare = nf;
	}
	public int getNucleoFamiliare()
	{
		return nucleoFamiliare;
	}
	public String getCodiceFiscale(){
		return CodiceFiscale;
	}
	public void setCodiceFiscale(String cod_fis){
		CodiceFiscale=cod_fis;
	}
	public String toString(){
		return "ID : "+getIdCittadino()+"\n"+
		       "Login : "+getLogin()+"\n"+
		       "Codice fiscale : "+getCodiceFiscale()+"\n"+
		       "Nome : "+getNome()+"\n"+
		       "Cognome : "+getCognome()+"\n"+
		       "Data di nascita : "+getDataNascita()+"\n"+
		       "Luogo di Nascita : "+getLuogoNascita()+"\n"+
		       "e-mail : "+getEmail()+".\n";
	}

}

