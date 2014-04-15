package Servlet;
import javax.servlet.http.*;
import javax.servlet.*;
import java.io.*;
import java.util.ArrayList;
import javax.swing.*;
import DB.DbCartaIdentita;
import DB.DbException;

public class ServletModificaResidenza extends HttpServlet{

	public void init(ServletConfig conf)throws ServletException{
		super.init(conf);
	}
	/**
	 * metodo che cerca i file contenenti
	 * le richieste di cambio di residenza
	 * da parte dei cittadini.Tale cartella viene
	 * salvata sul server su cui l'impiegato
	 * può accedervi per reperire le informazioni
	 * utili alla compilazione del modulo
	 * che deve essere registrato all'interno
	 * dell'archivio comunale 
	 */
	public class CercaFiles {
		//contatore usato per numerare i file ottenuti
		private int count = 0;

		private File to;
		private String interno;

		private File from;

		public CercaFiles(){
			//costruttore di default
		}


		public String main(int id_citizen,String date){
			//nel caso dell'impiegato la path è c:\\RichiesteCambioResidenza
			//directory dove cercare i file con determinate estensioni
			from = new File("C:\\RichiesteCambioResidenza");

			//nome del file da cercare. Ad esempio cerco un file di nome pippo inserirò pippo
			//se non specificato cercherà tutti i file con qualsiasi nome
			//mi salvo come stringa l'id del cittadino per poi confrontarla con
			//l'id del cittadino che mi arriva dal database per cercare il
			//file del cambio associato a quel cittadino
			String id_citt=String.valueOf(id_citizen);
			String dataric=date;

			String param="interno";
			String param1="esterno";
			//nome del file da cercare
			String nome = dataric+"_"+id_citt+"_".concat(param);
			String nomefile2=dataric+"_"+id_citt+"_".concat(param1);


			String[] splitter=nome.split("_");     

			//estensione del file da cercare. Ad esempio *.mp3 inseriro .mp3
			//se non specificato cercherà tutti i files con qualsiasi estensione
			String tipo = ".pdf";

			
			File newfile=esploraFile(from,nome,tipo);
			if(newfile!=null){
				interno="interno";
			}else{
				newfile=esploraFile(from,nomefile2,tipo);
				if(newfile!=null){
					//file esterno
					interno="esterno";
				}
				else{
					interno=null;//file non presente
				}
			}
			return interno;
		}
		/**
		 * from è la cartella in cui cercare il
		 * file di nome "nome" e di tipo ".pdf"
		 * @param from
		 * @param nome
		 * @param tipo
		 */

		protected File esploraFile(File from, String nome, String tipo) {
			//utilizziamo per la ricerca un filtro
			File[] files = from.listFiles(new Filter(nome, tipo));
			//ordiniamo i file nella lista secondo la data
			if(files.length>1||files.length==0){
				return null;
			}else{
				return files[0];

			}

		}

		class Filter implements FilenameFilter {
			//estensione del file
			private String tipo, nome;

			public Filter(String nome, String tipo) {
				this.nome = nome;
				this.tipo = tipo;
			}

			//accettiamo tutti i file con estensione s e le directory che non siano
			//nascoste
			public boolean accept(File dir, String file) {
				File f = new File(dir, file);

				//controllo sul tipo.
				//Ad esempio se cerco \"pippo.txt\" la indexOf(.txt) è uguale a 6 ed è uguale
				// ed è uguale a 10(\"pippo.txt\".length()) - 4 (\".txt\".length)

				boolean flag1=true;
				if(tipo!=null && tipo!="\\")
					flag1=(file.indexOf(tipo) == file.length()-tipo.length());

				//controllo sul nome
				//flag2 è true se il file contiene la parola cercata

				boolean flag2=true;
				if(nome!=null && nome!="\\")
					flag2=file.toUpperCase().indexOf(nome.toUpperCase())!=-1;
				//ritorno i file che passano il controllo del tipo e del nome le directory

				return ( ( flag1 && flag2 ) || f.isDirectory()) && !f.isHidden();
			}
		}




		public void doPost(HttpServletRequest request,HttpServletResponse response)
		throws ServletException,IOException{

			//innanzitutto mi recupero la sessione di lavoro del cittadino
			HttpSession session=request.getSession();
			int idCittadino;
			String idtrovato;
			/**
			 * se la sessione è stata creata correttamente 
			 * all'accesso dell'impiegato, viene mandato in exe
			 * il metodo che controlla se sono presenti file pdf
			 * riguardanti il cambio di residenza interno o
			 * esterno richiesti dai cittadini
			 */	 
			if(session!=null){
				ServletContext sc=getServletContext();
				RequestDispatcher rd=null;
				String ris;
				try{
					/**
					 * (1)	cosa deve fare la servlet:
					 * 		deve prendere l'id del cittadino
					 * 		di cui ha inserito il codice della carta di identità
					 * 		dal form e lo passa al metodo main 
					 * 		per cercare il file corrispondente
					 */
					/**
					 * (2)	il codice della carta di identità e la data della richiesta
					 * 		li prende con request.getParameter
					 * 		salvandoli nelle apposite variabili,
					 * 		dopodichè viene chiamato il metodo che restituisce
					 * 		l'id di un cittadino dopo avergli 
					 * 		passato nome, cognome e codice_fiscale di
					 * 		quest'ultimo.
					 */
					String cod_carta=request.getParameter("cod_carta");
					String data=request.getParameter("year").concat(request.getParameter("month").concat(request.getParameter("day")));
					/**
					 * una volta salvati i parametri, li passo
					 * al metodo che restituisce l'id di questo
					 * particolare cittadino che ha inviato la
					 * richiesta di cambio di residenza e di cui
					 * se ne vuole cercare il file 
					 */
					DbCartaIdentita dbcarta=new DbCartaIdentita();
					idCittadino=dbcarta.ricercaCartaIdentitaByNumero(cod_carta).id();
					String risp;
					if(idCittadino==-1){
						risp="Non è possibile recuperare l'id del cittadino all'interno del database";
						response.sendRedirect("http://localhost:8080/E_ANCI/index.jsp?error=e");//da modificare i collegamenti
						request.setAttribute("risp", risp);
					}
					else{
						/**
						 * una volta conosciuto l'id del cittadino
						 * lo passo al metodo di ricerca del file
						 * ad esso corrispondente
						 */
						CercaFiles search=new CercaFiles();
						idtrovato=search.main(idCittadino,data);
						/**
						 * ora nella variabile idtrovato c'è l'esito della ricerca
						 * del file relativa ad un determinato cittadino 
						 * che ha inviato la richiesta di cambio di residenza.
						 * tale valore andrà controllato, se è true si lancia 
						 * all'impiegato il modulo da compilare per mantenere traccia 
						 * della richiesta dopo aver effettuato i controlli necessari
						 */
						if(idtrovato==null){
							ris="File della richiesta di cambio di residenza non trovato";
							request.setAttribute("ris", ris);
							rd=sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");//da modificare i collegamenti
						}
						else
							if(idtrovato.equals("interno")){
								rd=sc.getRequestDispatcher("/workers/index.jsp?func=pra&page=modulone");//da modificare i collegamenti
							}
							else if(idtrovato.equals("esterno")){
								rd=sc.getRequestDispatcher("/workers/index.jsp?func=pra&page=modulone");//da modificare i collegamenti
							}
						rd.forward(request, response);
					}
				}catch(DbException e){
					ris=e.getMessage();
					request.setAttribute("ris", ris);
					rd=sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");//da modificare i collegamenti
					rd.forward(request,response);
				}

			}else{
				String url="/workers/Accesso.jsp";//da modificare i collegamenti
				response.sendRedirect(url);
			}
		}
	}
}