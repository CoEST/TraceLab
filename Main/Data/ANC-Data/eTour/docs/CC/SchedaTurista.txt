package unisa.gps.etour.gui.operatoreagenzia;

import java.awt .*;
import java.awt.event .*;
import java.util.Date;
import java.util.Iterator;
import java.util.StringTokenizer;
import java.util.Vector;
import javax.swing .*;
import java.util .*;
import javax.swing.border .*;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.util.Data;

/ **
  * Class that models the interface for displaying the card and
  * Modify the data of an account tourist.
  *
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class SchedaTurista extends JInternalFrame implements IScheda tourist
(

private static final String [] help = ( "" "" "" "" "" "" "" "" ""
"" "" "" "" "");
private JPanel jContentPane = null;
Private tourist JToolBar toolbarscheda = null;
Private JToggleButton btnModifica = null;
private JButton btnSalva = null;
private JButton btnReimposta = null;
private JTabbedPane JTabbedPane = null;
private JTextField address2 = null;
private JComboBox Address1 = null;
private JTextField city = null;
private JTextField ch = null;
private JTextField phone = null;
private JComboBox province = null;
private JPanel datiTurista = null;
private JTextField name = null;
private Vector <JLabel> suggestions;
Private BeanTurista tourist;
private JTextField name;
private JComboBox day;
private JComboBox month;
private JComboBox years;
private JTextField luogoNascita;
private JTextField email;
private JTextField username;
Private JPasswordField password;
private JLabel dataRegistrazione;
Private Tourists parent;

/ **
*
* The only card manufacturer model of a tourist or modification of data
* From the bean.
*
* @ Param pParent unisa.gps.etour.gui.operatoreagenzia.Turisti - the window "father."
* @ Param pTurista unisa.gps.etour.bean.BeanTurista - the bean contentente data
* Of the tourist.
* @ Param boolean pModifica <ul> <li> true - if amendments are made to the
* Data. <li> False - if you are viewing the card.
*
* /
Public profile tourists (tourism pParent, BeanTurista pTurista,
boolean pModifica)
(
super ();
this.parent = pParent;
setIconifiable (true);
setSize (560, 520);
suggestions <JLabel> = new Vector ();
setDefaultCloseOperation (WindowConstants.DO_NOTHING_ON_CLOSE);
setClosable (true);
tourist = pTurista;
if (turista.isAttiva ())
(
frameIcon = new ImageIcon (
getClass ()
. getResource (
"/ unisa / gps / eTour / gui / operatoreagenzia / images / tab turista.png"));
)
else
(
frameIcon = new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/DisattivaTurista32.png"));
)
initialize ();
if (pModifica)
(
btnModifica.setSelected (true);
btnSalva.setVisible (true);
btnReimposta.setVisible (true);
)
else
(
mostraNascondiSuggerimenti ();
attivaDisattivaEdit ();
)
addInternalFrameListener (new InternalFrameAdapter ()
(
public void internalFrameClosing (InternalFrameEvent pEvent)
(
if (btnModifica.isSelected ())
(
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to close the tab of this tourist?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"Warning! Unsaved data will be lost." SwingConstants.CENTER);
avviso.setIcon (new ImageIcon (getClass (). getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Close", "Cancel");
int choice = JOptionPane.showInternalOptionDialog (jContentPane, root,
"Confirm closing Tourist Card" + turista.getNome () + "" + turista.getCognome (),
JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, frameIcon, options, options [1]);
if (choice == JOptionPane.OK_OPTION)
(
parent.closeScheda ((tourist board) pEvent.getInternalFrame ());
)
)
else
(
parent.closeScheda ((tourist board) pEvent.getInternalFrame ());
)
)
));
)

/ **
*
* This method initializes the interface card for tourists.
*
* @ Return void
*
* /
private void initialize ()
(
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (tourist getToolbarscheda (), BorderLayout.CENTER);
JTabbedPane = new JTabbedPane ();
jTabbedPane.setCursor (new Cursor (Cursor.DEFAULT_CURSOR));
jTabbedPane.addTab ( "Tourist Information", frameIcon, getDatiTuristaForm (),
null);
jContentPane.add (JTabbedPane, BorderLayout.CENTER);
setContentPane (jContentPane);
caricaDatiForm ();
)

/ **
*
* This method loads the bean data provided tourist camps
* Of the form.
*
* @ Return void
*
* /
private void caricaDatiForm ()
(
setTitle ( "Profile Tourist -" + turista.getNome () + ""
Turista.getCognome + ());
nome.setText (turista.getNome ());
cognome.setText (turista.getCognome ());
Date dob = turista.getDataNascita ();
giorno.setSelectedIndex (dataNascita.getDate ());
mese.setSelectedIndex (dataNascita.getMonth ());
anno.setSelectedIndex (dataNascita.getYear ());
StringTokenizer tokenizer = new StringTokenizer (turista.getVia ());
String string = tokenizer.nextToken ();
indirizzo1.setSelectedItem (string);
indirizzo2.setText (turista.getVia (). substring (stringa.length ()));
luogoNascita.setText (turista.getCittaNascita ());
telefono.setText (turista.getTelefono ());
citta.setText (turista.getCittaResidenza ());
password.setText (turista.getPassword ());
provincia.setSelectedItem (turista.getProvincia ());
username.setText (turista.getUsername ());
cap.setText (turista.getCap ());
email.setText (turista.getEmail ());
dataRegistrazione
. setText (Data.toEstesa (turista.getDataRegistrazione ()));
)

/ **
*
* This method shows or hides the suggestions relating to the form fields.
*
* @ Return void
*
* /
private void mostraNascondiSuggerimenti ()
(
<JLabel> S = suggerimenti.iterator iterator ();
while (s.hasNext ())
(
Current = s.next JLabel ();
corrente.setVisible (corrente.isVisible ()? false: true);
)
)

/ **
*
* This method makes it or not editable form fields.
*
* @ Return void
*
* /
private void attivaDisattivaEdit ()
(
Component [] components = datiTurista.getComponents ();
for (int i = 0; i <componenti.length i + +)
(
Current component = components [i];
if (current instanceof JTextField)
(
JTextField textbox = (JTextField) current;
textbox.setEditable (textbox.isEditable ()? false: true);
textbox.setBackground (Color.white);

)
else if (current instanceof JComboBox)
(
JComboBox combo = (JComboBox) current;
combobox.setEnabled (combobox.isEnabled ()? false: true);

)
)
)

/ **
* This method initializes the toolbar for the functionality of the card
* Tourist.
*
* @ Return javax.swing.JToolBar
*
* /
Private tourist JToolBar getToolbarscheda ()
(
if (toolbarscheda tourist == null)
(
tourist toolbarscheda JToolBar = new ();
toolbarscheda turista.setFloatable (false);
toolbarscheda turista.add (getBtnModifica ());
toolbarscheda turista.addSeparator ();
toolbarscheda turista.add (getBtnSalva ());
toolbarscheda turista.addSeparator ();
toolbarscheda turista.add (getBtnReimposta ());
toolbarscheda turista.addSeparator ();
)
return toolbarscheda tourist;
)

/ **
* This method initializes the button for editing data.
*
* @ Return javax.swing.JToggleButton
*
* /
Private JToggleButton getBtnModifica ()
(
if (null == btnModifica)
(
btnModifica JToggleButton = new ();
btnModifica.setText ( "Change Data");
btnModifica
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/ModificaTurista32.png ")));
btnModifica
. setToolTipText ( "Enable or disable data modification tourists selected.");
btnModifica.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
mostraNascondiSuggerimenti ();
attivaDisattivaEdit ();
btnSalva.setVisible ((btnModifica.isSelected ()? true
: False));
btnReimposta.setVisible ((btnModifica.isSelected ()? true
: False));

)

));
)
btnModifica return;
)

/ **
* This method initializes the button to save the changes
* Made to the data of the tourist.
*
* @ Return javax.swing.JButton
* /
private JButton getBtnSalva ()
(
if (null == btnSalva)
(
btnSalva = new JButton ();
btnSalva.setText ( "Save");
btnSalva.setIcon (new ImageIcon (getClass (). getResource (
"/ unisa / gps / eTour / gui / operatoreagenzia / images / salva.png ")));
btnSalva
. setToolTipText ( "Save changes to the tourist profile selected.");
btnSalva.setVisible (false);
btnSalva.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent pEvent)
(
/ / Construction of the dialog for confirmation of the change
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Updating the tourist profile of"
Turista.getNome + () + ""
Turista.getCognome + () + "with"
+ "Data form?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"The previous data can not be more recovered."
SwingConstants.CENTER);
Legal
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Edit", "Cancel");
/ / The dialog screen appears
int choice = JOptionPane
. showInternalOptionDialog (
jContentPane,
root
"Commit Changes tourist figures,
JOptionPane.YES_NO_OPTION,
JOptionPane.QUESTION_MESSAGE,
new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/ModificaTurista48.png")),
options, options [1]);
/ / If you chose to confirm the change
if (choice == JOptionPane.YES_OPTION)
(
turista.setNome (nome.getText ());
turista.setCognome (cognome.getText ());
turista.setCap (cap.getText ());
turista.setCittaNascita (luogoNascita.getText ());
turista.setDataNascita (new Date (
anno.getSelectedIndex (), month
. getSelectedIndex (), day
. getSelectedIndex ()));
turista.setCittaResidenza (citta.getText ());
turista.setUsername (username.getText ());
turista.setEmail (email.getText ());
turista.setTelefono (telefono.getText ());
turista.setVia (indirizzo1.getSelectedItem (). toString ()
+ "" + Indirizzo2.getText ());
turista.setProvincia (provincia.getSelectedItem ()
. toString ());
String pass = "";
char [] password = password.getPassword ();
for (int i = 0; i <passWord.length i + +)
(
pass + = password [i];
)
turista.setPassword (pass);
caricaDatiForm ();
attivaDisattivaEdit ();
btnSalva.setVisible (false);
btnReimposta.setVisible (false);
btnModifica.setSelected (false);
mostraNascondiSuggerimenti ();
parent.updateTableModel (tourists);
JOptionPane
. showInternalMessageDialog (
jContentPane,
The data of tourists have been updated successfully. "
"Modified Profile Tourist!"
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/ok48.png ")));
)
)
));
)
btnSalva return;
)

/ **
* This method initializes the button to reset the data of the tourist
* In the form.
*
* @ Return javax.swing.JButton
* /
private JButton getBtnReimposta ()
(
if (null == btnReimposta)
(
btnReimposta = new JButton ();
btnReimposta.setText ( "Reset");
btnReimposta
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/Annulla32.png ")));
btnReimposta
. setToolTipText ( "Reload the selected tourist information.");
btnReimposta.setVisible (false);
btnReimposta.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent arg0)
(
caricaDatiForm ();
)
));
)
btnReimposta return;
)

/ **
* This method initializes the form contentente data of the tourist.
*
* @ Return javax.swing.JPanel
*
* /
private JPanel getDatiTuristaForm ()
(
if (null == datiTurista)
(
datiTurista = new JPanel (null);
datiTurista.setBorder (new SoftBevelBorder (SoftBevelBorder.LOWERED));
/ / Creation Tips
String [] txts = ( "Name", "Name", "Date of Birth",
"Place of Birth", "Phone", "Address", "City",
"CPC", "Province", "E-Mail", "Username", "Password",
"Save");

for (int i = 0; i <help.length i + +)
(
JLabel new = new JLabel ();
nuova.setIcon (new ImageIcon (getClass (). getResource (
"/ unisa/gps/etour/gui/images/Info16.png ")));
nuova.setBounds (145, 8 + 30 * i, 24, 24);
nuova.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
nuova.setToolTipText (help [i]);
suggerimenti.add (new);
datiTurista.add (new);

)

for (int i = 0; i <txts.length i + +)
(
New = new JLabel JLabel (txts [i], SwingConstants.RIGHT);
nuova.setBounds (25, 10 + 30 * i, 120, 20);
nuova.repaint ();
datiTurista.add (new, null);
)
/ / Name
name = new JTextField (12);
nome.setBounds (185, 10, 136, 20);
nome.setName ( "Name");
datiTurista.add (name, null);

/ / Surname
name = new JTextField (12);
cognome.setBounds (185, 40, 136, 20);
cognome.setName ( "Name");
datiTurista.add (name, null);

/ / Date of Birth
day = new JComboBox ();
giorno.setBounds (185, 70, 40, 20);
for (int i = 1; i <= 31; i + +)
(
giorno.addItem (i);
)
month = new JComboBox ();
mese.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent pEvent)
(
int number = giorno.getItemCount ();
switch (mese.getSelectedIndex ())
(
case 0:
case 2:
case 4:
case 6:
case 7:
case 9:
case 11:
for (int i = number + 1; i <= 31; i + +)
(
giorno.addItem (i);
)
break;

case 1:
int year = (Integer) anno.getSelectedItem ();
boolean leap = ((year% 4 == 0 & & year% 100! = 0) | | (year% 400 == 0));
if (number! = 28)
(
for (int i = number - 1, i> 27; i -)
(
giorno.removeItemAt (i);
)
)
if (leap & & number! = 29)
(
giorno.addItem ( "29");
)
break;

case 3:
case 5:
case 8:
case 10:
if (number == 31)
(
giorno.removeItemAt (30);
)
else
(
for (int i = number + 1; i <= 30; i + +)
(
giorno.addItem (i);
)
)
break;
)
)

));
mese.setBounds (245, 70, 40, 20);
for (int i = 1; i <= 12; i + +)
(
mese.addItem (i);
)
year = new JComboBox ();
anno.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
if (mese.getSelectedIndex () == 1)
(
int year = (Integer) anno.getSelectedItem ();
boolean leap = ((year% 4 == 0 & & year% 100! = 0) | | (year% 400 == 0));
int number = giorno.getItemCount ();
if (leap & & number! = 29)
(
giorno.addItem ( "29");
)
else if (leap & & number == 29)
(
giorno.removeItemAt (28);
)
)

)

));
anno.setBounds (305, 70, 80, 20);
Date today = new Date ();
for (int i = 0; i <= odierna.getYear () - 14; i + +)
(
anno.addItem (1900 + i);
)
datiTurista.add (day, null);
datiTurista.add (month, null);
datiTurista.add (year, null);

/ / Place of Birth
luogoNascita = new JTextField (12);
luogoNascita.setBounds (185, 100, 136, 20);
luogoNascita.setName ( "Birth Place");
datiTurista.add (luogoNascita, null);

/ / Phone
phone = new JTextField (12);
telefono.setBounds (185, 130, 136, 20);
telefono.setName ( "Phone");
datiTurista.add (telephone, null);

/ / Address
address2 = new JTextField (12);
indirizzo2.setBounds (270, 160, 136, 20);
Address1 = new JComboBox (address);
indirizzo1.setSelectedIndex (-1);
indirizzo1.setBounds (185, 160, 60, 20);
datiTurista.add (address2, null);
datiTurista.add (Address1, null);

/ / City
city = new JTextField (12);
citta.setBounds (185, 190, 136, 20);
citta.setName ( "City");
datiTurista.add (city, null);

/ / CAP
ch = new JTextField (8);
cap.setBounds (185, 220, 92, 20);
datiTurista.add (cap, null);

/ / State
province = new JComboBox (province);
provincia.setSelectedIndex (-1);
provincia.setBounds (185, 250, 50, 20);
datiTurista.add (province, null);

/ / E-Mail
email = new JTextField ();
email.setBounds (185, 280, 200, 20);
email.setName ( "E-Mail");
datiTurista.add (email, null);

/ / Username
username = new JTextField ();
username.setBounds (185, 310, 136, 20);
username.setName ( "Username");
datiTurista.add (username, null);

/ / Password
password = new JPasswordField (12);
password.setBounds (185, 340, 136, 20);
password.setName ( "Password");
datiTurista.add (password, null);

/ / Data entry
dataRegistrazione = new JLabel ();
dataRegistrazione.setBounds (185, 370, 140, 20);
datiTurista.add (dataRegistrazione, null);
)
datiTurista return;
)

/ **
* This method returns the id of the tourist who is viewing /
* Edit.
*
* @ Return int - the id of the tourist.
*
* /
public int getId ()
(
turista.getId return ();
)
) 