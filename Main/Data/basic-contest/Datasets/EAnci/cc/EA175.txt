package Servlet;

import java.io.IOException;
import java.util.Date;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Bean.Cittadino;
import Bean.NucleoFamiliare;
import DB.DbException;
import Manager.CittadinoManager;
import Manager.NucleoFamiliareManager;
/**
 * La classe ServletInserisciCittadino inserisce un cittadino nel database
 * La classe ServletInserisciCittadino non ha dipendenze
 * @author Federico Cinque
 */
public class ServletInserisciCittadino extends HttpServlet {

	private String nome;
	private String cognome;
	private String cf;
	private int giorno;
	private int mese;
	private int anno;
	private Date dataN = new Date();
	private String luogoN;
	private String email;
	private boolean advertise;
	private int idNF;
	private String login;
	private String tipo;
	private Cittadino cittadino;
	NucleoFamiliareManager NFM;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessione è nulla effettua il redirect alla pagina di autenticazione
			RequestDispatcher rd = null;
			ServletContext sc = getServletContext();
			String ris;
			try{
				nome = request.getParameter("nome");
				cognome = request.getParameter("cognome");
				cf = request.getParameter("cf").toUpperCase();
				giorno = Integer.parseInt(request.getParameter("gg"));
				mese = Integer.parseInt(request.getParameter("mm"));
				anno = Integer.parseInt(request.getParameter("aa"));
				dataN.setDate(giorno);
				dataN.setMonth(mese);
				dataN.setYear(anno);
				luogoN = request.getParameter("ln");
				if(request.getParameter("email")!=null)
					email = request.getParameter("email");
				else
					email= "";
				advertise = false;
				idNF = Integer.parseInt(request.getParameter("nucleof"));
				login = null;
				tipo = "Cittadino";

				CittadinoManager CM = new CittadinoManager();
				NFM = new NucleoFamiliareManager();

				cittadino = new Cittadino(0,cf,cognome,nome,dataN,luogoN,email,advertise,idNF,login);

				if(idNF==0){	//Se l'id del nucleo familiare è zero, devo creare un nuovo nucleo
					
					int idC = CM.inserisciCittadino(cittadino); //inserisco il cittadino nel db
					cittadino.setIdCittadino(idC);
					idNF=creaNucleoF(); //Salvo l'id del nuovo nucleo
					cittadino.setNucleoFamiliare(idNF); //setto l'id del nucleo del cittadino
					CM.modificaNucleoFamiliare(cittadino.getIdCittadino(), idNF);
					if(idNF!=0 && idC!=0){ //Se gli id restituiti sono diversi da zero l'operazione è andata a buon fine
						ris="ok";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=riuscita"); 
					}
					else{
						ris="fallita";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
					}
				}
				else{	// Se l'id del nucleo familiare non è zero, devo aggiungere il cittadino ad un nucleo esistente
					if(NFM.controllaidFamiglia(idNF)){	//controllo l'esistenza del nucleo nel db
						NFM.incrementaComponenti(idNF);	//incremento i componenti del nucleo
						int idC=0;
						if((idC = CM.inserisciCittadino(cittadino))!=0){ //inserisco il cittadino nel db e controllo se l'esito è positivo
							cittadino.setIdCittadino(idC);
							ris="ok";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=riuscita");
						}
						else{
							ris="Errore inserimento cittadino";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
						}
					}
					else{
						ris="Id non presente";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
					}
				}
				rd.forward(request,response);
			}
			catch(DbException e){
				ris=e.getMessage();
				request.setAttribute("ris", ris);
				rd=sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
				rd.forward(request,response);
			}
		}
		else{
			String url="/myDoc/workers/Accesso.jsp";
			response.sendRedirect(url);
		}
	}

	private int creaNucleoF() {
		NucleoFamiliare nf = new NucleoFamiliare();
		nf.setCapoFamiglia(cittadino.getIdCittadino());
		nf.setIdNucleoFamiliare(0);
		nf.setNComponenti(1);
		return NFM.inserisciNucleo(nf);
	}
}
