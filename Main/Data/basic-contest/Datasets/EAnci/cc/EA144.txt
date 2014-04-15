package Bean;

import java.util.Date;
/**
 * è un JavaBean che gestisce i metodi di settaggio
 * e restituzione degli attributi della carta
 * di identità di un cittadino
 * @author Francesco
 *
 */
public class CartaIdentita {
	/**
	 * rappresenta il codice della carta di identità
	 * che viene inserito dall'impiegato
	 */
	private String Numero;
	private int idCittadino;
	
	private String Cittadinanza;
	private String Residenza;
	private String Via;
	private String StatoCivile;
	private String Professione;
	private double Statura;
	private String Capelli;
	private String Occhi;
	private String SegniParticolari; 
	private Date DataRilascio;
	private Date DataScadenza;
	private boolean ValidaEspatrio;
	private int num_civico;
	/**
	 * Costruttore di default vuoto
	 */

	public CartaIdentita() {
		this.num_civico=0;
		this.Capelli=null;
		this.Cittadinanza=null;
		this.DataRilascio=null;
		this.DataScadenza=null;
		
		this.Numero=null;
		this.Occhi=null;
		this.Professione=null;
		this.Residenza=null;
		this.SegniParticolari=null;
		this.StatoCivile=null;
		this.Statura=0.00;
		this.ValidaEspatrio=false;
		this.Via=null;
	}
	/**
	 * oggetto che viene restituito dal database
	 * @param cod_carta
	 * @param citt
	 * @param res
	 * @param via
	 * @param stciv
	 * @param prof
	 * @param stat
	 * @param cap
	 * @param eyes
	 * @param sp
	 * @param dr
	 * @param ds
	 * @param validEsp
	 * @param num_civ
	 */
	public CartaIdentita(String cod_carta,int idC,String cittadinanza,String res,String via,int num,
			String stciv,String prof,double stat,String cap,String eyes,String sp,Date dr,Date ds,boolean validEsp){
		this.Numero=cod_carta;
		this.idCittadino=idC;
		
		this.Cittadinanza=cittadinanza;
		this.Residenza=res;
		this.Via=via;
		this.num_civico=num;
		this.StatoCivile=stciv;
		this.Professione=prof;
		this.Statura=stat;
		this.Capelli=cap;
		this.Occhi=eyes;
		this.SegniParticolari=sp;
		this.DataRilascio=dr;
		this.DataScadenza=ds;
		this.ValidaEspatrio=validEsp;
	}
		// TODO Auto-generated constructor stub
	/**
	 * crea la carta di identità del cittadino che si è 
	 * registrato nel sistema comunale prendendo dalla classe
	 * cittadino le informazioni necessarie per la creazione
	 * del documento di riconoscimento
	 */
	public CartaIdentita(String cod_carta,String surname,String name,Date borndate,String citt,String res,String via,int num,
			String stciv,String prof,double stat,String cap,String eyes,String sp,Date dr,Date ds,boolean validEsp){
		/**
		 * il codice univoco della carta di identità richiesta dal cittadino, viene 
		 * inserito dall'impiegato all'atto della creazione cartacea del documento e nel momento
		 * in cui le informazioni devono essere mantenute nel database.
		 * 
		 */
		this.Numero=cod_carta;
		/**
		 * assegno la carta di identità che sto creando al cittadino
		 * che ne ha fatto richiesta e che è presente all'interno del
		 * database comunale
		 */
		
		this.Cittadinanza=citt;
		this.Residenza=res;
		this.Via=via;
		this.num_civico=num;
		this.StatoCivile=stciv;
		this.Professione=prof;
		this.Statura=stat;
		this.Capelli=cap;
		this.Occhi=eyes;
		this.SegniParticolari=sp;
		this.DataRilascio=dr;
		this.DataScadenza=ds;
		this.ValidaEspatrio=validEsp;
		
	}
	public int id(){
		return idCittadino;
	}
	public void setNumero(String code){
		Numero=code;
	}
	public String getNumero(){
		return Numero;
	}
	
	public void setCittadinanza(String citta){
		Cittadinanza=citta;
	}
	public String getCittadinanza(){
		return Cittadinanza;
	}
	public String getResidenza(){
		return Residenza;
	}
	public void setResidenza(String res){
		Residenza=res;
	}
	public void setVia(String list){
		Via=list;
	}
	public String getVia(){
		return Via;
	}
	public int getNumCivico(){
		return num_civico;
	}
	public void setNumCivico(int n){
		num_civico=n;
	}
	public void setStatoCivile(String stat){
		StatoCivile=stat;
	}
	public String getStatoCivile(){
		return StatoCivile;
	}
	public void setProfessione(String prof){
		Professione=prof;
	}
	public String getProfessione(){
		return Professione;
	}
	public void setStatura(double stat){
		Statura=stat;
	}
	public double getStatura(){
		return Statura;
	}
	public void setCapelli(String hair){
		Capelli=hair;
	}
	public String getCapelli(){
		return Capelli;
	}
	public void setOcchi(String eyes){
		Occhi=eyes;
	}
	public String getOcchi(){
		return Occhi;
	}
	public String getSegniParticolari(){
		return SegniParticolari;
	}
	public void setSegniParticolari(String listSp){
		SegniParticolari=listSp;
	}
	public void setDataRilascio(Date data){
		DataRilascio=data;
	}
	public Date getDataRilascio(){
		return DataRilascio;
	}
	public void setDataScadenza(Date datas){
		DataScadenza=datas;
	}
	public Date getDataScadenza(){
		return DataScadenza;
	}
	public void setValiditaEspatrio(boolean val){
		ValidaEspatrio=val;
	}
	public boolean isValidaEspatrio(){
		return ValidaEspatrio;
	}
	
	@SuppressWarnings({ "deprecation", "deprecation", "deprecation" })
	public String toString(){
		
		int month2=0;
		month2 = getDataRilascio().getMonth();
		int month3=0;
		month3 = getDataScadenza().getMonth();
		return  //mi devo recuperare il cognome, nome, e data nascita dalla classe db
				
				"Cittadinanza : "+getCittadinanza()+"\n"+
				"Residenza : "+getResidenza()+"\n"+
				"Via : "+getVia()+"\n"+
				"Numero Civico : "+getNumCivico()+"\n"+
				"Stato Civile : "+getStatoCivile()+"\n"+
				"Professione : "+getProfessione()+"\n"+
				"\nCONNOTATI E CONTRASSEGNI SALIENTI\n\n"+
				"Statura : "+getStatura()+"\n"+
				"Capelli : "+getCapelli()+"\n"+
				"Occhi : "+getOcchi()+"\n"+
				"Segni Particolari : "+getSegniParticolari().toString()+"\n"+
				"li : "+getDataRilascio().getDate()+"/"+month2+1+"/"+getDataRilascio().getYear()+"\n"+
				"Data di Scadenza : "+getDataScadenza().getDate()+"/"+month3+1+"/"+getDataScadenza().getYear()+"\n";
	}
	

}

