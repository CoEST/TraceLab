package Servlet;

import java.io.IOException;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Bean.CartaIdentita;
import Bean.Cittadino;
import Bean.NucleoFamiliare;
import DB.DbException;
import Manager.CIManager;
import Manager.CittadinoManager;
import Manager.NucleoFamiliareManager;
/**
 * La classe ServletModificaCittadino che effettua l'operazione di modifica di un cittadino
 * La classe ServletModificaCittadino non ha dipendenze
 * @author Federico Cinque
 */
public class ServletModificaCittadino extends HttpServlet{

	private String nome;
	private String cognome;
	private String email;
	private int idNF;
	private Cittadino cittadino;
	private NucleoFamiliareManager NFM;
	private CittadinoManager CM;

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException{
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessione é nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				//Se l'attributo di sessione citté nullo devo effettuare la ricerca
				if(session.getAttribute("citt")==null){
					CittadinoManager CM = new CittadinoManager();
					Cittadino c = null;
					String cod = request.getParameter("ci").toUpperCase();
					CIManager CIM = new CIManager();
					CM = new CittadinoManager();
					CartaIdentita CI = CIM.getCartaByNumero(cod);

					if(CI!=null){
						c = CM.getCittadinoById(CI.id());

						NucleoFamiliareManager NFM = new NucleoFamiliareManager();
						int componenti = NFM.getNComponentiNucleo(c.getNucleoFamiliare());
						NucleoFamiliare nf = NFM.getNucleo(c.getNucleoFamiliare());
						if(componenti>1 && nf.getCapoFamiglia()== c.getIdCittadino()){
							String nc = "si"; 
							session.setAttribute("newCapo", nc);
						}

						session.setAttribute("citt", c);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=modifica&page=cittadino");
					}
					else{
						ris="Siamo spiacenti, il codice della carta d'identitˆ noné presente nel database";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
					}
					rd.forward(request,response);
				}
				else{
					nome = request.getParameter("nome");
					cognome = request.getParameter("cognome");
					email = request.getParameter("email");
					if(request.getParameter("email")!=null)
						email = request.getParameter("email");
					else
						email= "";
					idNF = Integer.parseInt(request.getParameter("nucleof"));

					CM = new CittadinoManager();
					NFM = new NucleoFamiliareManager();

					cittadino = (Cittadino) session.getAttribute("citt");
					if(idNF==0){	//Se l'id del nucleoé zero devo creare un nuovo nucleo familiare per il cittadino
						if(NFM.getNucleo(cittadino.getNucleoFamiliare()).getCapoFamiglia() != cittadino.getIdCittadino()){
							NFM.decrementaComponenti(cittadino.getNucleoFamiliare());
							idNF=creaNucleoF();	//Salvo l'id del nuovo nucleo
							cittadino.setNucleoFamiliare(idNF);	//setto l'id del nucleo del cittadino
							//effettuo le modifiche dei dati controllando l'esito positivo
							if(	CM.modificaNucleoFamiliare(cittadino.getIdCittadino(), idNF) &&
									CM.modificaEmail(cittadino.getIdCittadino(),email) &&
									CM.modificaNome(cittadino.getIdCittadino(), nome) &&
									CM.modificaCognome(cittadino.getIdCittadino(), cognome) &&
									idNF!=0){
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
						else{
							CIManager CIM = new CIManager();
							CartaIdentita CI = CIM.getCartaByNumero(request.getParameter("ci")); 
							if(CI!=null){	//Controllo che il nuovo capo famiglia esiste nel db
								Cittadino newCapo = CM.getCittadinoById(CI.id());
								NFM.setCapoFamiglia(cittadino.getNucleoFamiliare(), newCapo.getIdCittadino()); //modifico il capo famiglia del nucleo
								NFM.decrementaComponenti(cittadino.getNucleoFamiliare());
								idNF=creaNucleoF();	//Salvo l'id del nuovo nucleo
								cittadino.setNucleoFamiliare(idNF);	//setto l'id del nucleo del cittadino
								//effettuo le modifiche dei dati controllando l'esito positivo
								if(idNF!=0){
									if(	CM.modificaNucleoFamiliare(cittadino.getIdCittadino(), idNF) &&
											CM.modificaEmail(cittadino.getIdCittadino(),email) &&
											CM.modificaNome(cittadino.getIdCittadino(), nome) &&
											CM.modificaCognome(cittadino.getIdCittadino(), cognome)){
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
								else{
									ris="Errore creazione nuovo nucleo";
									request.setAttribute("ris", ris);
									rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
								}
							}
							else{
								ris="Siamo spiacenti, il codice della carta d'identità non è presente nel db";
								request.setAttribute("ris", ris);
								rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
							}
						}
					}
					else{	// Se l'id del nucleo familiare noné zero, devo aggiungere il cittadino ad un nucleo esistente
						if(NFM.controllaidFamiglia(idNF)){	//controllo l'esistenza del nucleo nel db
							if(NFM.getNucleo(cittadino.getNucleoFamiliare()).getCapoFamiglia() != cittadino.getIdCittadino()){
								NFM.incrementaComponenti(idNF);	//incremento i componenti del nucleo
								cittadino.setNucleoFamiliare(idNF);	//setto l'id del nucleo del cittadino
								//effettuo le modifiche dei dati controllando l'esito positivo
								if(	CM.modificaNucleoFamiliare(cittadino.getIdCittadino(), idNF) &&
										CM.modificaEmail(cittadino.getIdCittadino(),email) &&
										CM.modificaNome(cittadino.getIdCittadino(), nome) &&
										CM.modificaCognome(cittadino.getIdCittadino(), cognome) &&
										idNF!=0){
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
							CIManager CIM = new CIManager();
							CartaIdentita CI = CIM.getCartaByNumero(request.getParameter("ci")); 
							if(CI!=null){	//Controllo che il nuovo capo famiglia esiste nel db
								Cittadino newCapo = CM.getCittadinoById(CI.id());
								NFM.setCapoFamiglia(cittadino.getNucleoFamiliare(), newCapo.getIdCittadino()); //modifico il capo famiglia del nucleo
								NFM.decrementaComponenti(cittadino.getNucleoFamiliare());
								cittadino.setNucleoFamiliare(idNF);	//setto l'id del nucleo del cittadino
								NFM.incrementaComponenti(idNF);
								//effettuo le modifiche dei dati controllando l'esito positivo
								if(	CM.modificaNucleoFamiliare(cittadino.getIdCittadino(), idNF) &&
										CM.modificaEmail(cittadino.getIdCittadino(),email) &&
										CM.modificaNome(cittadino.getIdCittadino(), nome) &&
										CM.modificaCognome(cittadino.getIdCittadino(), cognome)){
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
							else{
								ris="Siamo spiacenti, il codice della carta d'identità del nuovo capofamiglia non è presente nel db";
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
					session.removeAttribute("citt");
					session.removeAttribute("newCapo");
				}
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
