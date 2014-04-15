package Servlet;

import java.io.IOException;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Manager.AccessManager;
import Manager.CIManager;
import Manager.CittadinoManager;
import Manager.NucleoFamiliareManager;
import Bean.Accesso;
import Bean.CartaIdentita;
import Bean.Cittadino;
import Bean.NucleoFamiliare;
import DB.DbException;
/**
 * La classe ServletEliminaCittadino ricerca ed elimina un cittadino
 * La classe ServletEliminaCittadino non ha dipendenze
 * @author Federico Cinque
 */
public class ServletEliminaCittadino extends HttpServlet{

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException{
		HttpSession session = request.getSession();
		if(session!=null){	//Se la sessione è nulla effettua il redirect alla pagina di autenticazione
			ServletContext sc = getServletContext();
			RequestDispatcher rd = null;
			String ris;
			try{
				//Se gli attributi di sessione c e acc sono nulli devo effettuare la ricerca
				if(session.getAttribute("c")==null && session.getAttribute("acc")==null){
					String cod = request.getParameter("ci").toUpperCase();
					CIManager CIM = new CIManager();
					CittadinoManager CM = new CittadinoManager();
					CartaIdentita CI = CIM.getCartaByNumero(cod);

					if(CI!=null){
						Cittadino c = CM.getCittadinoById(CI.id());
						session.setAttribute("c", c);

						AccessManager AM = new AccessManager();
						Accesso ac = AM.getAccesso(c.getLogin());
						session.setAttribute("acc", ac);

						NucleoFamiliareManager NFM = new NucleoFamiliareManager();
						int componenti = NFM.getNComponentiNucleo(c.getNucleoFamiliare());
						NucleoFamiliare nf = NFM.getNucleo(c.getNucleoFamiliare());
						if(componenti>1 && nf.getCapoFamiglia()== c.getIdCittadino()){
							String nc = "si"; 
							session.setAttribute("newCapo", nc);
						}

						sc = getServletContext();
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=cancella&page=cittadino"); 
						rd.forward(request,response);
					}
					else{
						ris="Siamo spiacenti, il codice della carta d'identitˆ non è presente nel database";
						request.setAttribute("ris", ris);
						rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
					}
				}
				else{//Se gli attributi sono presenti procedo con la cancellazione
					AccessManager AM = new AccessManager();
					CittadinoManager CM = new CittadinoManager();

					Accesso ac = (Accesso) session.getAttribute("acc");
					Cittadino c = (Cittadino) session.getAttribute("c");

					String login = ac.getLogin();

					if(request.getParameter("ci").equals("")){	//Se non c'è il codice della carta d'identitˆ
																//il cittadino da cancellare è solo nel nucleo familiare
						if(AM.eliminaAccesso(login) && CM.cancellaCittadino(c.getIdCittadino())){	//elimina il cittadino e l'accesso 
																									//controllando che l'esito sia positivo
							NucleoFamiliareManager NFM = new NucleoFamiliareManager();
							NFM.getNComponentiNucleo(c.getNucleoFamiliare());
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
					else{	//Se è presente un codice devo sostituire il capo famiglia
						NucleoFamiliareManager NFM = new NucleoFamiliareManager();
						CIManager CIM = new CIManager();
						CartaIdentita CI = CIM.getCartaByNumero(request.getParameter("ci")); 
						if(CI!=null){	//Controllo che il nuovo capo famiglia esiste nel db
							Cittadino newCapo = CM.getCittadinoById(CI.id());
							NFM.setCapoFamiglia(c.getNucleoFamiliare(), newCapo.getIdCittadino()); //modifico il capo famiglia del nucleo
							if(CM.cancellaCittadino(c.getIdCittadino())){//elimina il cittadino e l'accesso 
																								//controllando che l'esito sia positivo
								NFM.decrementaComponenti(c.getNucleoFamiliare());	// Decremento il numero di componenti del nucleo
								AM.eliminaAccesso(login);
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
							ris="Siamo spiacenti, il codice della carta d'identità del nuovo capo famiglia non è presente nel database";
							request.setAttribute("ris", ris);
							rd = sc.getRequestDispatcher("/workers/index.jsp?func=operazione&page=fallita");
						}
					}
					rd.forward(request,response);
					session.removeAttribute("c");
					session.removeAttribute("acc");
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
}
