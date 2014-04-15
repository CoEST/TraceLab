package Servlet;
import java.io.*;
import java.util.GregorianCalendar;
import javax.servlet.*;
import javax.servlet.http.*;
import com.lowagie.text.Document;
import com.lowagie.text.DocumentException;
import com.lowagie.text.Element;
import com.lowagie.text.Font;
import com.lowagie.text.Paragraph;
import com.lowagie.text.pdf.PdfWriter;

/**
 * La classe ServletCreaPdfCertificati crea un file PDF con i dati ricevuti da una pagina JSP
 * La classe non ha nessuna dipendenza
 * @author Christian Ronca
 */

public class ServletCreaPdfCertificati extends HttpServlet {
	private static final long serialVersionUID = -2298846540114609975L;

	public void doPost(HttpServletRequest request, HttpServletResponse response) 
		throws ServletException, IOException {
	    
		String certificato	= request.getParameter("certificato");
        String cognome		= request.getParameter("cognome").toUpperCase();
		String nome			= request.getParameter("nome").toUpperCase();
		String sesso		= request.getParameter("sesso").toUpperCase();
		String nascita		= request.getParameter("nascita").toUpperCase();
		String provincia	= request.getParameter("provincia").toUpperCase();
		String residenza	= request.getParameter("residenza").toUpperCase();
		String prov_res		= request.getParameter("prov_res").toUpperCase();
		String data			= request.getParameter("data");
		String indirizzo	= request.getParameter("indirizzo").toUpperCase();
		String dichiara_x 	= request.getParameter("dichiara").toUpperCase();
		

		response.setContentType("application/pdf");
		//PrintWriter out = response.getWriter();
		
		try {
			Document document = new Document();
			PdfWriter.getInstance(document, response.getOutputStream());
			document.open();
	
			Paragraph par = new Paragraph("\nDichiarazione sostitutiva della", new Font(Font.HELVETICA, 16, Font.BOLD));
			par.setAlignment(Element.ALIGN_CENTER);
			document.add(par);
			
			Paragraph titolo = new Paragraph("CERTIFICAZIONE DI " + certificato, new Font(Font.HELVETICA, 22, Font.BOLD));
			titolo.setAlignment(Element.ALIGN_CENTER);
			document.add(titolo);
			
			Paragraph dpr = new Paragraph("(Art. 46 - lettera a - D.P.R. 28 dicembre 2000, n. 445)\n\n\n\n\n", new Font(Font.HELVETICA, 10, Font.BOLD));
			dpr.setAlignment(Element.ALIGN_CENTER);
			document.add(dpr);
			
			Paragraph testo = new Paragraph("Il Sottoscritto/a " + cognome + " " + nome +" nato/a in "+ nascita +" il " + data + " residente " +
					"in "+residenza+" provincia di "+provincia+" in via "+indirizzo+", consapevole che chiunque rilascia dichiarazioni mendaci " +
					"è punito ai sensi del codice penale e delle leggi speciali in materia, ai sensi e per" +
					" gli effetti dell'art. 76 D.P.R. n. 445/2000\n\n\n", new Font(Font.HELVETICA, 14, Font.NORMAL));
			dpr.setAlignment(Element.ALIGN_CENTER);
			document.add(testo);
			
			Paragraph dichiara = new Paragraph("DICHIARA\n", new Font(Font.HELVETICA, 18, Font.BOLD));
			dichiara.setAlignment(Element.ALIGN_CENTER);
			document.add(dichiara);
	
			
			Paragraph cosa_dichiara = new Paragraph("di essere " + dichiara_x +"\n\n\n\n\n\n", new Font(Font.HELVETICA, 16, Font.NORMAL));
			cosa_dichiara.setAlignment(Element.ALIGN_CENTER);
			document.add(cosa_dichiara);
			
			Paragraph info = new Paragraph("Esente da imposta di bollo ai sensi dell'art. " +
					"37 D.P.R. 28 dicembre 2000, n. 455\n\n\n\n", new Font(Font.HELVETICA, 10, Font.NORMAL));
			info.setAlignment(Element.ALIGN_CENTER);
			document.add(info);
			
			GregorianCalendar gc = new GregorianCalendar();
			int gg = gc.get(GregorianCalendar.DATE);
			int mm = gc.get(GregorianCalendar.MONTH) + 1;
			int year = gc.get(GregorianCalendar.YEAR);
			Paragraph sdata = new Paragraph("Data: "+gg+"/"+mm+"/"+year, new Font(Font.HELVETICA, 14, Font.NORMAL));
			sdata.setAlignment(Element.ALIGN_LEFT);
			document.add(sdata);
			
			Paragraph firma = new Paragraph("___________________\nFirma del dichiarante", new Font(Font.HELVETICA, 14, Font.NORMAL));
			firma.setAlignment(Element.ALIGN_RIGHT);
			document.add(firma);
			
			document.close();
		} catch(DocumentException e) {
			e.printStackTrace();
		}
	}
}
