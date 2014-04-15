package unisa.gps.etour.gui.operatoreagenzia;

import java.awt .*;
import java.awt.event .*;
import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.StringTokenizer;
import java.util.Vector;
import javax.swing .*;
import java.util .*;
import javax.swing.table.TableColumn;
import javax.swing.text.JTextComponent;
import javax.swing.border .*;
import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.BeanVisitaBC;
import unisa.gps.etour.control.GestioneBeniCulturali.IGestioneBeniCulturaliAgenzia;
import unisa.gps.etour.control.GestioneTag.IGestioneTagComune;
import unisa.gps.etour.gui.operatoreagenzia.document.LimitedDocument;
import unisa.gps.etour.gui.operatoreagenzia.document.NumericDocument;
import unisa.gps.etour.gui.operatoreagenzia.document.OnlyCharactersDocument;
import unisa.gps.etour.gui.operatoreagenzia.tables.FeedBackTableModel;
import unisa.gps.etour.gui.operatoreagenzia.tables.MediaVotiRenderer;
import unisa.gps.etour.util.Punto3D;


/ **
  * Class that models the interface for viewing the card,
  * Modify the data and the insertion of a new cultural object.
  *
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  *
  * /
public class SchedaBC extends JInternalFrame implements IScheda
(

private static final String [] txts = ( "Name", "Address", "City", "Location", "CAP"
"Province", "Geographical Location", "Phone",
"Opening Hours", "Closing Time"
"Closing Date", "Ticket price", "Description");
private static final String [] help = (
"Enter the name of cultural property."
"Enter the address where is located the cultural property."
"Enter the city 'where is located the cultural property."
"Enter the location 'of membership of a cultural object."
"Enter your zip code, the area where the cultural object is located."
"Select the province belonging to the cultural property."
"Incorporating three dimensional coordinates for the location of" +
"cultural heritage."
"Enter the phone for delivery of the cultural management office."
"Select the time of public opening of the cultural property."
"Select the closing time for the public of cultural property."
"Select the weekly closing day."
"Give the ticket price of admission to cultural property."
"<html> Enter a full and comprehensive description for the cultural property. <br> Please note that this" +
"description will be used as a source of keywords <br> in research by" +
"tourists </ html>"
"<html> Select the search tags for cultural property. <br> search tags allow tourists to seek" +
"The sites with the features of interest. </ Html>");
private JPanel jContentPane = null;
Private JToolBar toolbarSchedaBC = null;
Private JToggleButton btnModifica = null;
private JButton btnSalva = null;
private JButton btnAnnulla = null;
private JButton btnModificaCommento = null;
private JTabbedPane JTabbedPane = null;
private JPanel statistics = null;
private JPanel feedback = null;
private JTextField address2 = null;
private JComboBox Address1 = null;
private JTextField cittaBC = null;
private JTextField localitaBC = null;
private JTextField capBC = null;
private JTextField posGeoX = null;
private JScrollPane JScrollPane = null;
private JTextArea descrizioneBC = null;
private JTextField telefonoBC = null;
private JComboBox oreAP = null;
private JComboBox minAP = null;
Private TagPanel pannelloTag;
private JTextField costoBC = null;
private JComboBox Oreca = null;
private JComboBox mince = null;
private JComboBox provBC = null;
private JPanel datiBC = null;
private JTextField nomeBC = null;
private JScrollPane scrollPaneFeedback = null;
private JTable tableFeedback = null;
private JLabel txtNomeBene = null;
private JLabel mediaVotoBC = null;
private JPanel statMeseCorrente = null;
private JPanel statTotali = null;
private JTextField posGeoY = null;
private JTextField posGeoZ = null;
private Vector <JLabel> suggestions;
Private BeanBeneCulturale bc;
private JComboBox giornoChiusura;
private JLabel [] statMeseC;
private JLabel [] statt;
Private Beniculturali parent;
Private FeedBackTableModel feedbackModel;
protected IGestioneTagComune tags;
protected IGestioneBeniCulturaliAgenzia gestioneBC;
private ArrayList <Integer> idTag = null;

/ **
* The default constructor for inclusion of the interface model
* A new cultural object.
*
* /
public SchedaBC (Beniculturali pParent)
(
super ( "New Cultural Heritage");
frameIcon = new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "nuovoBC.png"));
closable = true;
resizable = false;
iconable = true;
setSize (600, 560);
setDefaultCloseOperation (WindowConstants.DO_NOTHING_ON_CLOSE);
suggestions <JLabel> = new Vector ();
parent = pParent;
bc = null;

/ / Initialize the content pane
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getToolbarSchedaBC (), BorderLayout.CENTER);
JTabbedPane = new JTabbedPane ();
jTabbedPane.addTab ( "Data Cultural Heritage"
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "dati.png")),
getDatiBCForm (), null);
jContentPane.add (JTabbedPane, BorderLayout.CENTER);
setContentPane (jContentPane);

/ / Dialog closure to close the entry window.
addInternalFrameListener (new InternalFrameAdapter ()
(
/ *
* Inclusion of the frame on the desktop desktop retrieves bread bread
* And desktop manager and initializes the remote objects for managing
* Cultural heritage.
* /

public void internalFrameOpened (InternalFrameEvent pEvent)
(
PEvent.getInternalFrame JInternalFrame frame = ();

/ / Setting up of remote objects for the management of cultural heritage.
TRY
(
Registry reg = LocateRegistry.getRegistry (Home.HOST);
tag =
(IGestioneTagComune) reg.lookup (GestioneTagComune ");
gestioneBC =
(IGestioneBeniCulturaliAgenzia) reg.lookup (GestioneBeniCulturaliAgenzia ");
/ / Load data.
caricaTags ();
)
/ *
* Two exceptions: RemoteException and NotBoundException. The
* Result is the same. The management is not operable and
* After the error message window closes.
* /
catch (Exception ex)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "<h3> <u> Card for entering a new cultural asset will be closed. </ U> </ h3>"
+ "<p> <b> Possible Causes: </ b>"
+ "<ul> <li> No connection to the network. </ Li>"
+ "Server <li> inactive. </ Li>"
+ "Server <li> clogged. </ Li> </ ul>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass (). GetResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showMessageDialog (frame, error,
"Error!" JOptionPane.ERROR_MESSAGE, err);
frame.dispose ();
)
)

public void internalFrameClosing (InternalFrameEvent pEvent)
(
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to cancel the creation of a new cultural asset?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"Warning! Unsaved data will be lost." SwingConstants.CENTER);
avviso.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Close", "Cancel");
int choice = JOptionPane.showInternalOptionDialog (jContentPane, root,
"Confirm closure",
JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, frameIcon, options, options [1]);
if (choice == JOptionPane.OK_OPTION)
(
parent.closeScheda ((SchedaBC) pEvent.getInternalFrame ());
)
)
));

/ / Initialize button
btnModifica.setVisible (false);
btnSalva.setVisible (true);
btnAnnulla.setVisible (true);
btnAnnulla.setText ( "Clear");


)

/ **
* This interface models the manufacturer regarding modification of data and
* Display of the tab of a cultural object.
*
* @ Param BeanBeneCulturale pbc - the bean contains the data of
* Selected cultural.
* @ Param boolean pModifica <ul> <li> true - the fields will be editable, and then you are
* To amend the data of a cultural object. <li> False - the fields will not be
* Edit, and then you are viewing the tab of a cultural object. </ Ul>
*
* /
public SchedaBC (Beniculturali pParent, BeanBeneCulturale PBC, boolean pModifica)
(
super ();
frameIcon = new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "scheda.png"));
closable = true;
resizable = false;
iconable = true;
setSize (600, 560);
setDefaultCloseOperation (WindowConstants.DO_NOTHING_ON_CLOSE);

/ / Initialize instance variables
bc = pbc;
this.parent = pParent;
suggestions <JLabel> = new Vector ();
initializeSchedaBC ();

if (pModifica) / / Are we change the cultural property.
(
btnModifica.setSelected (true);
btnSalva.setVisible (true);
btnAnnulla.setVisible (true);
btnAnnulla.setText ( "Reset");
)
else / / We're viewing the tab of a cultural object.
(
mostraNascondiSuggerimenti ();
attivaDisattivaEdit ();
)

)

/ **
* This method returns the id of the cultural property for which you are viewing the
* Contact or changing data.
*
* @ Return int - the id of the cultural property.
*
* /
public int getId ()
(
if (bc == null)
(
return -1;
)
bc.getId return ();
)

/ **
*
* This method initializes the interface for display board
* A cultural object.
*
* @ Return void
* /
private void initializeSchedaBC ()
(
setTitle (bc.getNome ());

/ / Dialog closed frame
addInternalFrameListener (new InternalFrameAdapter ()
(
/ *
* Inclusion of the frame on the desktop desktop retrieves bread bread
* And desktop manager and initializes the remote objects for managing
* Cultural heritage.
* /

public void internalFrameOpened (InternalFrameEvent pEvent)
(
PEvent.getInternalFrame JInternalFrame frame = ();

/ / Setting up of remote objects for the management of cultural heritage.
TRY
(
Registry reg = LocateRegistry.getRegistry (Home.HOST);
tag =
(IGestioneTagComune) reg.lookup (GestioneTagComune ");
gestioneBC =
(IGestioneBeniCulturaliAgenzia) reg.lookup (GestioneBeniCulturaliAgenzia ");
/ / Load data.
caricaTags ();
caricaStatistiche ();
)
/ *
* Two exceptions: RemoteException and NotBoundException. The
* Result is the same. The management is not operable and
* After the error message window closes.
* /
catch (Exception ex)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "<h3> <u> The board of the cultural inquiry will be closed. </ U> </ h3>"
+ "<p> <b> Possible Causes: </ b>"
+ "<ul> <li> No connection to the network. </ Li>"
+ "Server <li> inactive. </ Li>"
+ "Server <li> clogged. </ Li> </ ul>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass (). GetResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showMessageDialog (frame, error,
"Error!" JOptionPane.ERROR_MESSAGE, err);
frame.dispose ();
)
)

public void internalFrameClosing (InternalFrameEvent pEvent)
(
/ / If you are an amendment asks for confirmation.
if (btnModifica.isSelected ())
(
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to close the tab of this cultural asset?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"Warning! Unsaved data will be lost." SwingConstants.CENTER);
avviso.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Close", "Cancel");
int choice = JOptionPane.showInternalOptionDialog (jContentPane, root,
"Confirm closing tab Cultural Heritage" bc.getNome + (),
JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, frameIcon, options, options [1]);
if (choice == JOptionPane.OK_OPTION)
(
parent.closeScheda ((SchedaBC) pEvent.getInternalFrame ());
)
)
/ / Otherwise directly closes the window.
else
(
parent.closeScheda ((SchedaBC) pEvent.getInternalFrame ());
)
)
));

/ / Initialize the content pane.
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getToolbarSchedaBC (), BorderLayout.CENTER);
JTabbedPane = new JTabbedPane ();
jTabbedPane.addTab ( "Data Cultural Heritage"
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "dati.png")),
getDatiBCForm (), null);
New = new JScrollPane JScrollPane (getStatistiche ());
nuovo.setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
jTabbedPane.addTab ( "Statistics"
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stat24.png")), new, null);
jTabbedPane.addTab ( "Feedback received"
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "feedback.png")),
getFeedback (), null);
jContentPane.add (JTabbedPane, BorderLayout.CENTER);
setContentPane (jContentPane);
jTabbedPane.addChangeListener (new ChangeListener ()
(

public void stateChanged (ChangeEvent pChange)
(
/ / Data cultural
if (jTabbedPane.getSelectedIndex () == 0)
(
toolbarSchedaBC.setVisible (true);
btnModifica.setVisible (true);
if (btnModifica.isSelected ())
(
btnSalva.setVisible (true);
btnAnnulla.setVisible (true);
)
btnModificaCommento.setVisible (false);
)
/ / Statistics
else if (jTabbedPane.getSelectedIndex () == 1)
(
toolbarSchedaBC.setVisible (false);
)
/ / Feedback received
else
(
if (btnModifica.isSelected ())
(
btnSalva.setVisible (false);
btnAnnulla.setVisible (false);
)
toolbarSchedaBC.setVisible (true);
btnModificaCommento.setVisible (true);
btnModifica.setVisible (false);

)

)

));

/ / Load the data of the cultural and statistics.
caricaDatiForm ();
)

/ **
*
* This method loads the data supplied to the constructor of the cultural
* In the form.
*
* /
private void caricaDatiForm ()
(
nomeBC.setText (bc.getNome ());
capBC.setText (bc.getCap ());
cittaBC.setText (bc.getCitta ());
costoBC.setText ( "" + bc.getCostoBiglietto ());
descrizioneBC.setText (bc.getDescrizione ());
StringTokenizer tokenizer = new StringTokenizer (bc.getVia ());
String string = tokenizer.nextToken ();
indirizzo1.setSelectedItem (string);
indirizzo2.setText (bc.getVia (). substring (stringa.length ()));
provBC.setSelectedItem (bc.getProvincia ());
Punto3D pos = bc.getPosizione ();
posGeoX.setText ( "" + pos.getLatitudine ());
posGeoY.setText ( "" + pos.getLongitudine ());
posGeoZ.setText ( "" + pos.getAltitudine ());
telefonoBC.setText (bc.getTelefono ());
int minutes = bc.getOrarioApertura (). getMinutes ();
if (minutes == 0)
(
minAP.setSelectedIndex (0);
)
else
(
minAP.setSelectedItem (minutes);
)
int hours = bc.getOrarioApertura (). getHours ();
if (hours <10)
(
oreAP.setSelectedItem ( "0" + hours);
)
else
(
oreAP.setSelectedItem (hours);
)
bc.getOrarioChiusura minutes = (). getMinutes ();
if (minutes == 0)
(
minCH.setSelectedIndex (0);
)
else
(
minCH.setSelectedItem (minutes);
)
bc.getOrarioChiusura hours = (). getHours ();
if (hours <10)
(
oreCH.setSelectedItem ( "0" + hours);
)
else
(
oreCH.setSelectedItem (hours);
)
)

/ **
*
* This method loads the statistics provided cultural
* Input to the constructor of the class.
*
* /
private void caricaStatistiche ()
(
txtNomeBene.setText (bc.getNome ());
double voting bc.getMediaVoti = ();
if (rating> = 4)
(
mediaVotoBC.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella5.gif ")));
)
else if (rating <4 & & rating> = 3)
(
mediaVotoBC.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella4.gif ")));
)
else if (rating <3 & & rating> = 2)
(
mediaVotoBC.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella3.gif ")));
)
else if (grade <2 & & rating> = 1)
(
mediaVotoBC.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella2.gif ")));
)
else
(
mediaVotoBC.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "stella1.gif ")));
)

ArrayList <Integer> stats = null;
TRY
(
stats =
gestioneBC.ottieniStatisticheBeneCulturale (bc.getId ());

)
catch (RemoteException e)
(
)
statMeseC [0]. setText ( "" + stats.get (0));
statMeseC [1]. setText ( "" + stats.get (1));
statMeseC [2]. setText ( "" + stats.get (2));
statMeseC [3]. setText ( "" + stats.get (3));
statMeseC [4]. setText ( "" + stats.get (4));
statMeseC [5]. setText ( "" + stats.get (5));
statt [0]. setText ( "142");
statt [1]. setText ( "112");
statt [2]. setText ( "132");
statt [3]. setText ( "212");
statt [4]. setText ( "152");
statt [5]. setText ( "748");
/ / END TEST
)

/ **
*
* This method shows or hides the label next to the suggestions
* Of the form.
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
* This method makes the form editable or not.
*
* /
private void attivaDisattivaEdit ()
(
Component [] components = datiBC.getComponents ();
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
descrizioneBC.setEditable (descrizioneBC.isEditable ()? false: true);
pannelloTag.attivaDisattiva ();
)

/ **
* This method initializes the toolbar tab of a cultural object.
*
* @ Return javax.swing.JToolBar - the toolbar.
* /
Private JToolBar getToolbarSchedaBC ()
(
if (null == toolbarSchedaBC)
(
toolbarSchedaBC JToolBar = new ();
toolbarSchedaBC.setFloatable (false);
toolbarSchedaBC.add (getBtnModifica ());
toolbarSchedaBC.addSeparator ();
toolbarSchedaBC.add (getBtnSalva ());
toolbarSchedaBC.addSeparator ();
toolbarSchedaBC.add (getBtnAnnulla ());
toolbarSchedaBC.addSeparator ();
if (bc! = null)
(
toolbarSchedaBC.add (getBtnModificaCommento ());
toolbarSchedaBC.addSeparator ();
)
)
toolbarSchedaBC return;
)

/ **
* This method initializes the button to modify data of good
* Cultural.
*
* @ Return javax.swing.JToggleButton - the button for the change.
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
Home.URL_IMAGES + "ModificaBC32.png ")));
btnModifica.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
btnModifica.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
mostraNascondiSuggerimenti ();
attivaDisattivaEdit ();
btnSalva.setVisible ((btnModifica.isSelected ()? true
: False));
btnAnnulla.setVisible ((btnModifica.isSelected ()? true
: False));

)

));
)
btnModifica return;
)

/ **
* This method initializes btnSalva
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
Home.URL_IMAGES + "salva.png ")));
btnSalva.setVisible (false);
btnSalva.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
btnSalva.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent pEvent)
(
if (bc == null)
(
bc = riversaDatiNelBean ();
TRY
(
gestioneBC.inserisciBeneCulturale (bc);
)
catch (RemoteException ex)
(
ex.printStackTrace ();
)
)
else (
/ / Construction of the dialog for confirmation of the change
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
Updating the data of the cultural "
Bc.getNome + () + "with"
+ "Data form?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"The previous data can not be more recovered."
SwingConstants.CENTER);
Legal
. setIcon (new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES + "warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Edit", "Cancel");
/ / The dialog screen appears
int choice = JOptionPane
. showInternalOptionDialog (
jContentPane,
root
"Edit Data Confirm Cultural Heritage"
JOptionPane.YES_NO_OPTION,
JOptionPane.QUESTION_MESSAGE,
new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES + "ModificaBC48.png")),
options, options [1]);
/ / If you chose to confirm the change
if (choice == JOptionPane.YES_OPTION)
(
bc = riversaDatiNelBean ();
caricaDatiForm ();
attivaDisattivaEdit ();
btnSalva.setVisible (false);
btnAnnulla.setVisible (false);
btnModifica.setSelected (false);
mostraNascondiSuggerimenti ();
parent.updateTableModel (bc);
JOptionPane
. showInternalMessageDialog (
jContentPane,
The data of the cultural object has been updated successfully. "
"Data cultural change!"
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES + "ok32.png ")));
)
)
)
));
)
btnSalva return;
)

/ **
* This method initializes the button to clear the form (well again
* Culture) or reload the data of the cultural (change data).
*
* @ Return javax.swing.JButton - the button above.
*
* /
private JButton getBtnAnnulla ()
(
if (null == btnAnnulla)
(
btnAnnulla = new JButton ();
btnAnnulla.setText ( "Cancel");
btnAnnulla
. setIcon (new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES + "Annulla32.png ")));
btnAnnulla.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
btnAnnulla.setVisible (false);
btnAnnulla.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
if (bc == null)
(
Component [] components = datiBC.getComponents ();
for (int i = 0; i <componenti.length i + +)
(
Current component = components [i];
if (current instanceof JTextComponent)
(
((JTextComponent) current). SetText ("");
)
else if (current instanceof JComboBox)
(
JComboBox combo = (JComboBox) current;
combobox.setSelectedIndex (-1);
)
)
pannelloTag.azzera ();
descrizioneBC.setText ("");
)
else
(
caricaDatiForm ();
)

)

));
)
btnAnnulla return;
)

/ **
* This method initializes the button to edit a comment.
*
* @ Return javax.swing.JButton - the button to edit a comment.
* /
private JButton getBtnModificaCommento ()
(
if (null == btnModificaCommento)
(
btnModificaCommento = new JButton ();
btnModificaCommento.setText ( "Edit Comment");
btnModificaCommento
. setIcon (new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES + "modificaCommento.png ")));
btnModificaCommento.setVisible (false);
btnModificaCommento.setEnabled (false);
btnModificaCommento.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent pEvent)
(
int selectedRow = tableFeedback.getSelectedRow ();
NuovoCommento String = (String) JOptionPane.showInternalInputDialog (
jContentPane, "Changing the selected comment:"
"Edit Comment", JOptionPane.QUESTION_MESSAGE,
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "modificaCommento.png")),
null feedbackModel.getValueAt (selectedRow, 1));
if (nuovoCommento = null)
(
feedbackModel.modificaCommento (nuovoCommento, selectedRow);
)
)

));
)
btnModificaCommento return;
)

/ **
* This method initializes the format for the data of a cultural object.
*
* @ Return javax.swing.JPanel - the form for the data.
*
* /
private JPanel getDatiBCForm ()
(
if (null == datiBC)
(
datiBC = new JPanel (null);
datiBC.setBorder (new SoftBevelBorder (SoftBevelBorder.LOWERED));

/ / Creation Tips
for (int i = 0; i <help.length i + +)
(
JLabel new = new JLabel ();
nuova.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "Info16.png ")));
nuova.setBounds (145, 8 + 30 * i, 24, 24);
nuova.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
nuova.setToolTipText (help [i]);
suggerimenti.add (new);
datiBC.add (new);
if (i == help.length - 1)
(
nuova.setBounds (400, 155, 24, 24);
)

)

for (int i = 0; i <txts.length i + +)
(
New = new JLabel JLabel (txts [i], SwingConstants.RIGHT);
nuova.setBounds (25, 10 + 30 * i, 120, 16);
nuova.repaint ();
datiBC.add (new, null);
)
/ / Name of Cultural Heritage
nomeBC = new JTextField ();
nomeBC.setColumns (12);
nomeBC.setDocument (new LimitedDocument (30));
nomeBC.setBounds (185, 10, 136, 20);
nomeBC.setName ( "Name Cultural Heritage");
datiBC.add (nomeBC, null);

/ / Address
address2 = new JTextField ();
indirizzo2.setBounds (270, 40, 136, 20);
indirizzo2.setDocument (new LimitedDocument (30));
Address1 = new JComboBox (address);
indirizzo1.setSelectedIndex (-1);
indirizzo1.setBounds (185, 40, 60, 20);
indirizzo2.setName ( "Address");
datiBC.add (address2, null);
datiBC.add (Address1, null);

/ / City
cittaBC = new JTextField ();
cittaBC.setColumns (12);
cittaBC.setBounds (185, 70, 136, 20);
cittaBC.setName ( "City");
cittaBC.setDocument (new OnlyCharactersDocument (25));
datiBC.add (cittaBC);

/ / Location
localitaBC = new JTextField ();
localitaBC.setBounds (185, 100, 136, 20);
localitaBC.setName (Localit ");
localitaBC.setDocument (new OnlyCharactersDocument (25));
datiBC.add (localitaBC, null);

/ / CAP
capBC = new JTextField ();
capBC.setColumns (8);
capBC.setBounds (185, 130, 92, 20);
capBC.setDocument (new NumericDocument (5));
datiBC.add (capBC, null);


/ / Geographical Location
JLabel txtX = new JLabel ( "X");
JLabel txtY = new JLabel ( "Y");
JLabel txtZ = new JLabel ( "Z");
New fonts = new Font ( "Dialog", Font.BOLD, 14);
txtX.setFont (new);
txtY.setFont (new);
txtZ.setFont (new);
txtZ.setBounds (365, 190, 10, 22);
txtY.setBounds (295, 190, 10, 22);
txtX.setBounds (227, 190, 14, 20);
posGeoX = new JTextField (12);
posGeoX.setBounds (185, 190, 40, 20);
posGeoY = new JTextField (12);
posGeoY.setBounds (255, 190, 40, 20);
posGeoZ = new JTextField (12);
posGeoZ.setBounds (325, 190, 40, 20);
posGeoX.setName ( "X-coordinate");
posGeoY.setName ( "Y coordinate");
posGeoZ.setName ( "z coordinate");
datiBC.add (txtX, null);
datiBC.add (txtY, null);
datiBC.add (txtZ, null);
datiBC.add (posGeoX, null);
datiBC.add (posGeoY, null);
datiBC.add (posGeoZ, null);

/ / State
provBC = new JComboBox (province);
provBC.setSelectedIndex (-1);
provBC.setBounds (185, 160, 50, 20);
datiBC.add (provBC, null);

/ / Description
descrizioneBC = new JTextArea ();
descrizioneBC.setCursor (new Cursor (Cursor.TEXT_CURSOR));
descrizioneBC.setWrapStyleWord (true);
descrizioneBC.setLineWrap (true);
JScrollPane = new JScrollPane (descrizioneBC);
JScrollPane
. setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
jScrollPane.setBounds (new Rectangle (185, 370, 395, 69));
descrizioneBC.setName ( "Description");
datiBC.add (JScrollPane, null);

/ / Phone
telefonoBC = new JTextField (12);
telefonoBC.setBounds (185, 220, 136, 20);
telefonoBC.setDocument (new NumericDocument (10));
telefonoBC.setName ( "Phone");
datiBC.add (telefonoBC, null);

/ / Opening
oreAP = new JComboBox ();
oreAP.setBounds (185, 250, 40, 20);
Oreca = new JComboBox ();
oreCH.setBounds (185, 280, 40, 20);
for (int i = 0; i <24; i + +)
(
if (i <10)
(
oreCH.addItem ( "0" + i);
oreAP.addItem ( "0" + i);
)
else
(
oreAP.addItem (i);
oreCH.addItem (i);
)
)
minAP = new JComboBox ();
minAP.setBounds (241, 250, 40, 20);
minAP.addItem ( "00");
minAP.addItem ( "15");
minAP.addItem ( "30");
minAP.addItem ( "45");
mince = new JComboBox ();
minCH.setBounds (241, 280, 40, 20);
minCH.addItem ( "00");
minCH.addItem ( "15");
minCH.addItem ( "30");
minCH.addItem ( "45");
minAP.setSelectedIndex (0);
oreAP.setSelectedIndex (-1);
minCH.setSelectedIndex (0);
oreCH.setSelectedIndex (-1);
new = new Font ( "Dialog", Font.BOLD, 18);
TAG 1 = new JLabel JLabel (":");
punto1.setBounds (230, 245, 10, 24);
punto1.setFont (new);
JLabel punto2 = new JLabel (":");
punto2.setBounds (230, 275, 10, 24);
punto2.setFont (new);
datiBC.add (oreAP, null);
datiBC.add (minAP, null);
datiBC.add (mince, null);
datiBC.add (Oreca, null);
datiBC.add (point 1, null);
datiBC.add (punto2, null);

/ / Closed
String [] days = ( "Monday", "Tuesday", "Wednesday", "Thursday",
"Friday", "Saturday", "Sunday");
giornoChiusura = new JComboBox (days);
giornoChiusura.setBounds (185, 310, 96, 20);
giornoChiusura.setSelectedIndex (-1);
datiBC.add (giornoChiusura, null);

/ / Cost
costoBC = new JTextField ();
costoBC.setColumns (8);
costoBC.setBounds (185, 340, 40, 20);
JLabel euro = new JLabel ( "Euro");
euro.setBounds (230, 340, 30, 16);
datiBC.add (costoBC, null);
datiBC.add (euro, null);

/ / PannelloTag
pannelloTag = new TagPanel ();
pannelloTag.setBounds (405, 180, 180, 170);
JLabel txtTag = new JLabel ( "Search Tags");
txtTag.setBounds (420, 150.140, 30);
datiBC.add (txtTag, null);
datiBC.add (pannelloTag, null);

)
datiBC return;
)

/ **
* This method initializes the statistics of a container panel
* Cultural.
*
* @ Return javax.swing.JPanel - the panel statistics.
*
* /
private JPanel getStatistiche ()
(
if (statistics == null)
(
statistics = new JPanel (new GridBagLayout ());
GridBagConstraints g = new GridBagConstraints ();
g.gridx = 0;
g.gridy = 0;
g.insets = new Insets (5, 5, 5, 5);
g.anchor = GridBagConstraints.WEST;
Stat = new JLabel JLabel ( "Statistics");
New fonts = new Font ( "Dialog", Font.BOLD, 18);
stat.setFont (new);
statistiche.add (stat, g);
g.gridx = 1;
txtNomeBene = new JLabel ();
txtNomeBene.setFont (new);
statistiche.add (txtNomeBene, g);
g.gridx = 2;
mediaVotoBC = new JLabel ();
statistiche.add (mediaVotoBC, g);
g.gridwidth = 3;
g.gridx = 0;
g.anchor = GridBagConstraints.CENTER;
g.gridy = 1;
statistiche.add (getStatMeseCorrente (), g);
g.gridy = 2;
statistiche.add (getStatTotali (), g);
)
return statistics;
)

/ **
* This method initializes the panel to display feedback
* Received from a cultural object.
*
* @ Return javax.swing.JPanel - the panel of feedback.
*
* /
private JPanel getFeedback ()
(
if (feedback == null)
(
feedback = new JPanel ();
feedback.setLayout (new BorderLayout ());
feedbackModel = new FeedBackTableModel ();
tableFeedback = new JTable (feedbackModel);
TableColumn aColumn = tableFeedback.getColumnModel (). GetColumn (0);
/ / Rating
aColumn.setPreferredWidth (80);
aColumn.setCellRenderer (new MediaVotiRenderer ());
/ / Comment
aColumn = tableFeedback.getColumnModel (). GetColumn (1);
aColumn.setPreferredWidth (260);
/ / Release Date
aColumn = tableFeedback.getColumnModel (). GetColumn (2);
aColumn.setPreferredWidth (80);
/ / Username
aColumn = tableFeedback.getColumnModel (). GetColumn (3);
aColumn.setPreferredWidth (80);
tableFeedback.setSelectionMode (ListSelectionModel.SINGLE_SELECTION);
tableFeedback.setColumnSelectionAllowed (false);
ListSelectionModel selectionModel = tableFeedback.getSelectionModel ();
selectionModel
. addListSelectionListener (new ListSelectionListener ()
(
public void ValueChanged (ListSelectionEvent event)
(
btnModificaCommento.setEnabled (
(tableFeedback.getSelectedRow ()! = -1)
? true: false);
)
));
scrollPaneFeedback = new JScrollPane (tableFeedback);
scrollPaneFeedback
. setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
feedback.add (scrollPaneFeedback, BorderLayout.CENTER);

)
return feedback;
)

/ **
* This method initializes the panel of statistics for the current month.
*
* @ Return javax.swing.JPanel - the panel of statistics for the current month.
*
* /
private JPanel getStatMeseCorrente ()
(
if (null == statMeseCorrente)
(
statMeseCorrente = new JPanel ();
statMeseCorrente.setLayout (new GridBagLayout ());
statMeseCorrente.setPreferredSize (new Dimension (500, 280));
statMeseCorrente.setBorder (BorderFactory.createTitledBorder (
BorderFactory.createLineBorder (new Color (51, 102, 255), 3),
"Statistics Current Month"
TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
statMeseCorrente.setBackground (Color.white);
statMeseC = new JButton [6];
GridBagConstraints g = new GridBagConstraints ();
g.gridx = 0;
g.gridy = 0;
g.gridwidth = 3;
g.insets = new Insets (5, 5, 5, 8);
g.anchor = GridBagConstraints.WEST;
statMeseCorrente.add (new JLabel (
"Details votes received this month:"), g);
g.anchor = GridBagConstraints.CENTER;
g.gridwidth = 1;
New fonts = new Font ( "Dialog", Font.BOLD, 16);
for (int i = 5; i> = 1, i -)
(

int gridX = g.gridx;
g.gridy + +;
JLabel aLabel = new JLabel (new ImageIcon (getClass ()
. getResource (
Home.URL_IMAGES + "star" + i
+. "Gif")), JLabel.CENTER);
statMeseCorrente.add (aLabel, g);
g.gridx + +;
statMeseCorrente.add (new JLabel ("=="), g);
g.gridx + +;
g.anchor = GridBagConstraints.EAST;
statMeseC [i - 1] = new JLabel ();
statMeseC [i - 1]. setFont (new);
statMeseCorrente.add (statMeseC [i - 1], g);
g.gridx = gridX;
g.anchor = GridBagConstraints.CENTER;
)
g.gridy = 6;
g.anchor = GridBagConstraints.WEST;
g.gridwidth = 2;
g.gridx = 0;
statMeseCorrente.add (new JLabel (
"Number of ratings released this month:"), g);
statMeseC [5] = new JLabel ();
statMeseC [5]. setFont (new Font ( "Dialog", Font.BOLD, 18));
g.gridx = 2;
g.gridwidth = 1;
statMeseCorrente.add (statMeseC [5], g);

)
statMeseCorrente return;
)

/ **
* This method initializes the panel on the total statistics
* The cultural property.
*
* @ Return javax.swing.JPanel - the panel statistics totals.
*
* /
private JPanel getStatTotali ()
(
if (null == statTotali)
(
statTotali = new JPanel ();
statTotali.setLayout (new GridBagLayout ());
statTotali.setPreferredSize (new Dimension (500, 280));
statTotali.setBorder (BorderFactory.createTitledBorder (BorderFactory
. createLineBorder (new Color (51, 102, 255), 3),
"Statistics Total", TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
statTotali.setBackground (Color.white);
statt = new JButton [6];
GridBagConstraints g = new GridBagConstraints ();
g.gridx = 0;
g.gridy = 0;
g.gridwidth = 3;
g.insets = new Insets (5, 5, 5, 8);
g.anchor = GridBagConstraints.WEST;
statTotali.add (new JLabel (
"Details votes received this month:"), g);
g.anchor = GridBagConstraints.CENTER;
g.gridwidth = 1;
New fonts = new Font ( "Dialog", Font.BOLD, 16);
for (int i = 5; i> = 1, i -)
(
int gridX = g.gridx;
g.gridy + +;
JLabel aLabel = new JLabel (new ImageIcon (getClass ()
. getResource (
"/ unisa / gps / eTour / gui / images / star" + i
+. "Gif")), JLabel.CENTER);
statTotali.add (aLabel, g);
g.gridx + +;
statTotali.add (new JLabel ("=="), g);
g.gridx + +;
g.anchor = GridBagConstraints.EAST;
statt [i - 1] = new JLabel ();
statt [i - 1]. setFont (new);
statTotali.add (statt [i - 1], g);
g.gridx = gridX;
g.anchor = GridBagConstraints.CENTER;
)
g.gridy = 6;
g.anchor = GridBagConstraints.WEST;
g.gridwidth = 2;
g.gridx = 0;
statTotali.add (new JLabel (
"Number of ratings released this month:"), g);
statt [5] = new JLabel ();
statt [5]. setFont (new Font ( "Dialog", Font.BOLD, 18));
g.gridx = 2;
g.gridwidth = 1;
statTotali.add (statt [5], g);
)
statTotali return;
)

Private BeanBeneCulturale riversaDatiNelBean ()
(
BeanBeneCulturale new BeanBeneCulturale = new ();
nuovo.setNome (nomeBC.getText ());
nuovo.setDescrizione (descrizioneBC.getText ());
nuovo.setCap (capBC.getText ());
nuovo.setCostoBiglietto (Double.parseDouble (costoBC.getText ()));
nuovo.setGiornoChiusura ((String) giornoChiusura.getSelectedItem ());
nuovo.setTelefono (telefonoBC.getText ());
nuovo.setCitta (cittaBC.getText ());
nuovo.setLocalita (localitaBC.getText ());
/ / Date (int year, int month, int date, int hrs, int min)
Date orarioAP = new Date (0,0,0, oreAP.getSelectedIndex (),
minAP.getSelectedIndex ());
Date orarioCH = new Date (0,0,0, oreCH.getSelectedIndex (),
minCH.getSelectedIndex ());
nuovo.setOrarioApertura (orarioAP);
nuovo.setOrarioChiusura (orarioCH);
nuovo.setProvincia ((String) provBC.getSelectedItem ());
nuovo.setVia (((String) indirizzo1.getSelectedItem ()) + "" + indirizzo2.getText ());
Punto3D position = new Punto3D (
Double.parseDouble (posGeoX.getText ()),
Double.parseDouble (posGeoY.getText ()),
Double.parseDouble (posGeoZ.getText ()));
nuovo.setPosizione (position);
return new;
)

private void caricaTags ()
(
ArrayList <BeanTag> beanTags = null;
TRY
(
beanTags = tag.ottieniTags ();
if (bc! = null)
(
idTag <Integer> = new ArrayList ();
ArrayList <BeanTag> tagDaSelezionare =
gestioneBC.ottieniTagBeneCulturale (bc.getId ());
for (BeanTag b: tagDaSelezionare)
(
idTag.add (b.getId ());
)
)
)
/ / If an error panel tag remains blank.
catch (RemoteException e)
(
)
finally
(
for (BeanTag b: beanTags)
(
pannelloTag.insertTag (b);
)
pannelloTag.setSelectedTags (idTag);
pannelloTag.repaint ();
)
)
)
