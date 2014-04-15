package Servlet;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import Manager.AccessManager;
import Manager.AdminManager;
import Manager.CIManager;
import Bean.Accesso;
import Bean.Amministratore;
import Bean.CartaIdentita;
import Bean.Cittadino;
import Manager.CittadinoManager;
import Bean.Impiegato;
import DB.DbException;
import Manager.ImpiegatoManager;
/**
 * La classe ServletLogin effettua l'operazione di autenticazione di un utente nel sistema
 * La classe ServletLogin non ha dipendenze
 * @author Federico Cinque
 */
public class ServletLogin extends HttpServlet {

	public void doPost(HttpServletRequest request,HttpServletResponse response) throws ServletException, IOException {
		int flag = -1;
		String login = request.getParameter("login");
		String password = request.getParameter("password");
		String tipo = request.getParameter("tipo");

		HttpSession session = request.getSession(true);
		try{
			AccessManager AM = new AccessManager();
			String url;

			Accesso ac = AM.getAccesso(login);

			if(tipo!=null){	//Se tipo è diverso da null la servlet è stata invocata dal lato cittadino
				flag=0;
				if (AM.controllaAccesso(login, password) && ac.getTipo().equals("Cittadino")){

					CittadinoManager CM = new CittadinoManager();
					Cittadino c = CM.getCittadinoByLogin(login);
					CIManager ciM=new CIManager();
					CartaIdentita ci=ciM.getCartaByIdCStri(c.getIdCittadino());
					session.setAttribute("c", c);
					session.setAttribute("ci", ci);
					url="/myDoc/user/home.jsp";
				}
				else
					url="/myDoc/user/home.jsp?error=e";
			}
			else{	//Se tipo è null la servlet è stata invocata dal lato amministratore/impiegato
				flag = 1;
				if (AM.controllaAccesso(login, password) && !ac.getTipo().equals("Cittadino")){
					session.setAttribute("login", ac.getLogin());
					session.setAttribute("tipo", ac.getTipo());
					if(ac.getTipo().equals("Impiegato")){
						ImpiegatoManager IM = new ImpiegatoManager();
						Impiegato imp = IM.getImpiegatoByLogin(login);
						session.setAttribute("imp", imp);
					}
					else
						if(ac.getTipo().equals("Amministratore")){
							AdminManager AdM = new AdminManager();
							Amministratore am = AdM.getAmministratoreByLogin(login);
							session.setAttribute("am", am);
						}
					url="/myDoc/workers/index.jsp";
				}
				else
					url="/myDoc/workers/Accesso.jsp?error=e";
			}
			response.sendRedirect(url);
		}
		catch(DbException e){
			String url;
			if(flag==1)
				url="/myDoc/workers/Accesso.jsp?error="+e.getMessage();
			else
				url="/myDoc/user/home.jsp?error="+e.getMessage();
			response.sendRedirect(url);
		}
	}
}