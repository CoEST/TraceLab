package smos.utility;

import java.io.UnsupportedEncodingException;
import java.util.Properties;
import javax.mail.*;
import javax.mail.internet.*;


public class MailUtility {

	public static void sendMail(String pSender, String pReceiver, String pSubject, String pText) throws MessagingException, UnsupportedEncodingException {
		
		// Creazione di una mail session
		Properties props = new Properties();
		props.put("mail.smtp.host", Utility.getServerSmtp());
		Session session = Session.getDefaultInstance(props);

		// Creazione del messaggio da inviare
		MimeMessage message = new MimeMessage(session);
		message.setSubject(pSubject);
		message.setText(pText);

		// Aggiunta degli indirizzi del mittente e del destinatario
		InternetAddress fromAddress = new InternetAddress(pSender);
		InternetAddress toAddress = new InternetAddress(pReceiver);
		message.setFrom(fromAddress);
		message.setRecipient(Message.RecipientType.TO, toAddress);

		// Invio del messaggio
		Transport.send(message);
	}

}
