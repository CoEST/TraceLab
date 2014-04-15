package unisa.gps.etour.control.GestioneUtentiRegistrati;

import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.sql.SQLException;

import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.repository.DBTurista;
import unisa.gps.etour.util.ControlloDati;
import unisa.gps.etour.util.MessaggiErrore;

public class Authentication extends UnicastRemoteObject implements IAutenticazione
(

private static final long serialVersionUID = 0L;

public Authentication () throws RemoteException
(
super ();
)

/ / Objects to manipulate data Turista
Private DBTurista tourist DBTurista = new ();
Private BeanTurista bTurista;

public int login (String pUsername, String pPassword, byte pTipologiaUtente)
throws RemoteException
(
/ / Check if the string username and password
if (ControlloDati.controllaStringa (pUsername, true, true, "_-", null,
6, 12)
& & ControlloDati.controllaStringa (pPassword, true, true, "_-",
null, 5, 12))
TRY
(
switch (pTipologiaUtente)
(
/ / If the type is Turista
VISITORS homes:
/ / Invoke the method to obtain the Bean del Turista
/ / Given the username
bTurista = turista.ottieniTurista (pUsername);
/ / Check that the Bean is not null and
/ / Passwords match
if (bTurista = null
BTurista.getPassword & & (). Equals (pPassword))
bTurista.getId return ();
/ / If the type and eateries
homes OP_PUNTO_DI_RISTORO:
/ / Not implemented was the operational point of
/ / Refreshment
return -1;
/ / If not match any known type
default:
return -1;
)
)
catch (SQLException e)
(
throw new RemoteException (
MessaggiErrore.ERRORE_DBMS);
)
catch (Exception e) (
throw new RemoteException (MessaggiErrore.ERRORE_SCONOSCIUTO);
)
/ / If the data are incorrect returns -1
return -1;
)
) 