	
/ *
  * Tourists.java
  *
  * 1.0
  *
  * 26/05/2007
  *
  * © 2007 eTour Project - Copyright by SE @ SA Lab - DMI - University of Salerno
  * /
package unisa.gps.etour.gui.operatoreagenzia;

import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JInternalFrame;
import java.awt.Dimension;

import javax.swing.JDesktopPane;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.JTextPane;
import javax.swing.JToggleButton;
import javax.swing.JToolBar;
import javax.swing.JButton;
import javax.swing.ListSelectionModel;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;
import javax.swing.BorderFactory;
import javax.swing.JLabel;
import javax.swing.SwingConstants;
import java.awt.Color;
import java.awt.Insets;
import javax.swing.JTextField;
import javax.swing.JOptionPane;
import javax.swing.border.TitledBorder;
import javax.swing.event.InternalFrameAdapter;
import javax.swing.event.InternalFrameEvent;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.font;
import javax.swing.ImageIcon;
import unisa.gps.etour.util.Data;
import unisa.gps.etour.bean.BeanTurista;
import unisa.gps.etour.gui.DeskManager;
import unisa.gps.etour.gui.operatoreagenzia.tables.AttivazioneTuristaRenderer;
import unisa.gps.etour.gui.operatoreagenzia.tables.ScrollableTable;
import unisa.gps.etour.gui.operatoreagenzia.tables.TuristiTableModel;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.Date;

/ **
  * This class implements the interface for the management of tourists
  * Agency by the Operator.
  *
  * @ Version 1.0
  * @ See javax.swing.JInternalFrame
  * @ Author Mario Gallo
  *
  * /
public class JInternalFrame extends Tourists
(

private JPanel jContentPane = null;
Private JToolBar toolbarTuristi = null;
private JButton btnModificaDati = null;
private JScrollPane scrollTableTuristi = null;
private JTable tableTuristi = null;
private JPanel searchPanel = null;
private JTextField cognomeTurista = null;
private JPanel panelHelp = null;
private JTextPane textGuida = null;
private JPanel rightPanel = null;
private JButton btnAttiva;
private JButton btnVisualizzaScheda;
private JButton btnElimina;
private TuristiTableModel TableModel;
Private JToggleButton turistiDisattivati;
Private JToggleButton turistiAttivati;
private JButton btnAzzera;
private JButton btnRicerca;
protected DeskManager desktopManager;
protected JDesktopPane JDesktopPane;
<schedaturista> private ArrayList children;

/ **
* This is the default constructor.
*
* /
public Tourists ()
(
super ( "Tourists");
setPreferredSize (new Dimension (700, 480));
frameIcon = new ImageIcon (getClass (). getResource (
"/ unisa / gps / eTour / gui / operatoreagenzia / images / Turisti.png"));
setDefaultCloseOperation (JInternalFrame.DO_NOTHING_ON_CLOSE);
closable = true;
resizable = true;
iconable = true;
maximizable = true;
setContentPane (getJContentPane ());
<schedaturista> children = new ArrayList ();
addInternalFrameListener (new InternalFrameAdapter ()
(
public void internalFrameOpened (InternalFrameEvent pEvent)
(
JDesktopPane pEvent.getInternalFrame = (). getDesktopPane ();
desktopManager = (DeskManager) jDesktopPane.getDesktopManager ();
)

public void internalFrameClosing (InternalFrameEvent pEvent)
(
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to close the management of tourists?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"NB will be closed all the windows opened by this administration."
SwingConstants.CENTER);
Legal
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Close", "Cancel");
int choice = JOptionPane.showInternalOptionDialog (jContentPane, root, "Confirm closure Tourists"
JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, frameIcon, options, options [1]);
if (choice == JOptionPane.OK_OPTION)
(
for (int i = 0; i <figli.size (); i + +)
(
figli.get (i). dispose ();
)
pEvent.getInternalFrame (). dispose ();
)
)
));
)

/ **
* This method initializes the internal frame's content pane.
*
* @ Return javax.swing.JPanel - the content pane.
*
* /
private JPanel getJContentPane ()
(
if (null == jContentPane)
(
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getToolbarTuristi (), BorderLayout.NORTH);
jContentPane.add (getRightPanel (), BorderLayout.EAST);
jContentPane.add (getScrollTableTuristi (), BorderLayout.CENTER);
)
jContentPane return;
)

/ **
* This method initializes the toolbar on the management of
* Tourists.
*
* @ Return javax.swing.JToolBar - the toolbar.
*
* /
Private JToolBar getToolbarTuristi ()
(
if (null == toolbarTuristi)
(
toolbarTuristi JToolBar = new ();
toolbarTuristi.setLayout (null);
toolbarTuristi.setPreferredSize (new Dimension (1, 50));
toolbarTuristi.setOrientation (JToolBar.HORIZONTAL);
toolbarTuristi.setFloatable (false);
toolbarTuristi.add (getBtnAttiva ());
toolbarTuristi.add (getBtnModificaDati ());
toolbarTuristi.add (getBtnVisualizzaScheda ());
toolbarTuristi.add (getBtnElimina ());

)
toolbarTuristi return;
)

/ **
* This method initializes the button to activate
* A tourist.
*
* @ Return javax.swing.JButton - the button.
*
* /
private JButton getBtnAttiva ()
(
if (null == btnAttiva)
(
btnAttiva = new JButton ();
btnAttiva.setText ( "Enable <html> <br> region </ html>");
btnAttiva.setBounds (5, 5, 140, 40);
btnAttiva.setEnabled (false);
btnAttiva
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/AttivaTurista32.png ")));

btnAttiva.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent arg0)
(
int selected = tableTuristi.getSelectedRow ();
String [] fields = ( "Name", "Name", "Date of Birth",
"Place of Birth", "E-Mail", "Phone",
"Address", "CPC", "Province", "City");
int [] modelReference = (1, 2, 5, 6, 3, 4, 7, 9, 10, 8);
DatiTurista JPanel = new JPanel (new GridBagLayout ());
RootDialog JPanel = new JPanel (new GridBagLayout ());
datiTurista.setBorder (BorderFactory.createTitledBorder (
BorderFactory.createLineBorder (new Color (51, 102,
255), 2), "Tourist Information"
TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
GridBagConstraints g = new GridBagConstraints ();
g.insets = new Insets (5, 5, 5, 5);
g.anchor = GridBagConstraints.WEST;
g.gridx = 0;
g.gridy = 0;
for (int i = 0; i <campi.length i + +)
(
datiTurista.add (new JLabel (fields [i]), g);
g.gridx + +;
if (i == 2) / / Date of Birth
(
Date dob = (Date) tableModel.getValueAt (
selected, 5);
datiTurista.add (new JLabel (
Data.toCompact (dob)), g);

)
else
(
datiTurista.add (new JLabel (
tableModel.getValueAt (
selected, modelReference [i]). toString ()), g);
)
g.gridx--;
g.gridy + +;
)
g.gridx = 0;
g.gridy = 0;
rootDialog.add (datiTurista, g);
g.gridy + +;
JLabel txtAttiva = new JLabel ();
rootDialog.add (txtAttiva, g);
String [] options = new String [2];
ImageIcon iconDialog;
options [1] = "Cancel";
String title;
boolean enabled = (Boolean) tableModel.getValueAt (
selected, 0);
if (enabled) / / The process of decommissioning
(
title = "Turn off the tourist"
+ TableModel.getValueAt (selected, 1) + ""
+ TableModel.getValueAt (selected, 2) + "?"
options [0] = "Disable";
txtAttiva
. setText ( "Turn off the tourist selected?");
iconDialog = new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/DisattivaTurista48.png"));
)
else / / The process of activation
(
title = "Turn on the tourist"
+ TableModel.getValueAt (selected, 1) + ""
+ TableModel.getValueAt (selected, 2) + "?"
options [0] = "Enable";
txtAttiva.setText ( "Activate the tourists selected?");
iconDialog = new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/AttivaTurista48.png"));
)
txtAttiva.setForeground (Color.red);
JOptionPane.showInternalOptionDialog int choice = (
jContentPane, rootDialog, title,
JOptionPane.OK_CANCEL_OPTION,
JOptionPane.QUESTION_MESSAGE, iconDialog, options,
options [1]);
if (choice == JOptionPane.OK_OPTION)
(
tableModel.setValueAt ((activated)? false: true,
selected, 0);
tableModel.fireTableDataChanged ();
)
)
));
)
btnAttiva return;
)

/ **
* This method initializes the button for changing
* Data of a tourist.
*
* @ Return javax.swing.JButton - the button.
* /
private JButton getBtnModificaDati ()
(
if (null == btnModificaDati)
(
btnModificaDati = new JButton ();
btnModificaDati
. setText ( "Edit Data <html> <br> region </ html>");
btnModificaDati.setBounds (155, 5, 140, 40);
btnModificaDati.setEnabled (false);
btnModificaDati
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/ModificaTurista32.png ")));
btnModificaDati.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent arg0)
(
/ / TEST
New Date = new Date ();
nuova.setMonth (12);
nuova.setDate (30);
nuova.setYear (82);
Nuova2 Date = new Date ();
nuova2.setMonth (4);
nuova2.setDate (30);
nuova2.setYear (107);
BeanTurista test = new BeanTurista (0, "mlmlml", "Ottabio"
"of Michil", "Ottawa", "Frattamaggiore", "61616161",
"84932nd", "V.le della Mimosa 33", "NA"
"ottavio_ottawa@wawa.com", "micacae" new nuova2, false);
/ / TEST
OpenOffice.org Messenger MSN Gaim (TEST, true);
)
));
)
btnModificaDati return;
)

/ **
* This method initializes the button to display the
* Board a tourist.
*
* @ Return javax.swing.JButton - the button.
* /
private JButton getBtnVisualizzaScheda ()
(
if (null == btnVisualizzaScheda)
(
btnVisualizzaScheda = new JButton ();
btnVisualizzaScheda
. setText ( "Show <html> <br> tourist profile </ html>");
btnVisualizzaScheda.setBounds (305, 5, 140, 40);
btnVisualizzaScheda
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa / gps / eTour / gui / operatoreagenzia / images / schedaturista.png ")));
btnVisualizzaScheda.setEnabled (false);
btnVisualizzaScheda.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
/ / TEST
New Date = new Date ();
nuova.setMonth (12);
nuova.setDate (30);
nuova.setYear (82);
Nuova2 Date = new Date ();
nuova2.setMonth (4);
nuova2.setDate (30);
nuova2.setYear (107);
BeanTurista test = new BeanTurista (0, "mlmlml", "Ottabio"
"of Michil", "Ottawa", "Frattamaggiore", "61616161",
"84932nd", "V.le della Mimosa 33", "NA"
"ottavio_ottawa@wawa.com", "micacae" new nuova2, false);
/ / TEST
OpenOffice.org Messenger MSN Gaim (TEST, false);
)

));
)
btnVisualizzaScheda return;
)

/ **
*
* This method initializes the delete button for a tourist.
*
* @ Return javax.swing.JButton - the button.
* /
private JButton getBtnElimina ()
(
if (null == btnElimina)
(
btnElimina = new JButton ();
btnElimina.setText ( "Delete <html> <br> region </ html>");
btnElimina.setBounds (455, 5, 140, 40);
btnElimina.setEnabled (false);
btnElimina
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/EliminaTurista32.png ")));
btnElimina.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent arg0)
(
int selectedRow = tableTuristi.getSelectedRow ();
if (selectedRow! = -1)
(
String name = (String) tableModel.getValueAt (
selectedRow, 1)
+ ""
+ (String) TableModel
. getValueAt (selectedRow, 2);
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to delete the account of the tourist"
+ Name + "?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
The data account and all personal settings "
+ "Can not be filled again."
SwingConstants.CENTER);
Legal
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Delete", "Cancel");
int choice = JOptionPane
. showInternalOptionDialog (
jContentPane,
root
"Confirm Delete",
JOptionPane.YES_NO_OPTION,
JOptionPane.QUESTION_MESSAGE,
new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/EliminaTurista48.png")),
options, options [1]);
if (choice == JOptionPane.YES_OPTION)
(
tableModel.removeTurista (selectedRow);
tableModel.fireTableDataChanged ();
JLabel confirm = new JLabel ( "Account Tourists" + name
+ "Was deleted.");
confirm.setFont (new Font ( "Dialog", Font.BOLD, 14));
JOptionPane
. showInternalMessageDialog (
jContentPane,
confirm,
Accounts Turista out! "
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/ok32.png ")));
)
)

)

));
)
btnElimina return;
)

/ **
* This method initializes the scroll with the table bread tourists.
*
* @ Return javax.swing.JScrollPane - the scroll pane.
* /
private JScrollPane getScrollTableTuristi ()
(
if (null == scrollTableTuristi)
(
TuristiTableModel = new TableModel ();
/ / TEST
New Date = new Date ();
nuova.setMonth (12);
nuova.setDate (30);
nuova.setYear (82);
Nuova2 Date = new Date ();
nuova2.setMonth (4);
nuova2.setDate (30);
nuova2.setYear (107);
BeanTurista new = new BeanTurista (0, "mlmlml", "Ottabio"
"of Michil", "Ottawa", "Frattamaggiore", "61616161",
"84932nd", "V.le della Mimosa 33", "NA"
"ottavio_ottawa@wawa.com", "micacae" new nuova2, false);
BeanTurista nuovo2 = new BeanTurista (1, "mlmlml", "Ottabiolino"
"of Michil", "Ottawa", "Frattamaggiore", "61616161",
"84932nd", "V.le della Mimosa 33", "NA"
"ottavio_ottawa@wawa.com", "micacae" new nuova2, true);
BeanTurista nuovo3 = new BeanTurista (2, "mlmlml", "Ottavio",
"Michil", "Ottawa", "Frattamaggiore", "61616161", "84932nd"
"V.le Mimose 33", "NA", "ottavio_ottawa@wawa.com"
"micacae" new nuova2, false);
tableModel.insertTurista (new);
tableModel.insertTurista (nuovo2);
tableModel.insertTurista (nuovo3);
for (int i = 0; i <12; i + +)
(
tableModel.insertTurista (new);
)
/ / END TEST
tableTuristi = new ScrollableTable (TableModel);
tableTuristi.setAutoCreateColumnsFromModel (true);
tableTuristi.setSelectionMode (ListSelectionModel.SINGLE_SELECTION);
tableTuristi.setSelectionForeground (Color.RED);
tableTuristi.setSelectionBackground (Color.white);
tableTuristi.setColumnSelectionAllowed (false);
tableTuristi.setShowVerticalLines (false);
tableTuristi.setRowHeight (32);
/ / Status
tableTuristi.getColumnModel (). GetColumn (0). setPreferredWidth (40);
tableTuristi.getColumnModel (). GetColumn (0). setCellRenderer (
New AttivazioneTuristaRenderer ());
/ / Name
tableTuristi.getColumnModel (). GetColumn (1). setPreferredWidth (100);
/ / Surname
tableTuristi.getColumnModel (). GetColumn (2). setPreferredWidth (100);
/ / Email
tableTuristi.getColumnModel (). GetColumn (3). setPreferredWidth (140);
/ / Phone
tableTuristi.getColumnModel (). GetColumn (4). setPreferredWidth (80);
/ / Date of Birth
tableTuristi.getColumnModel (). GetColumn (5). setPreferredWidth (100);
/ / City of Birth
tableTuristi.getColumnModel (). GetColumn (6). setPreferredWidth (100);
/ / Address
tableTuristi.getColumnModel (). GetColumn (7). setPreferredWidth (100);
/ / Residence
tableTuristi.getColumnModel (). GetColumn (8). setPreferredWidth (80);
/ / CAP
tableTuristi.getColumnModel (). GetColumn (9). setPreferredWidth (60);
/ / State
tableTuristi.getColumnModel (). GetColumn (10). setPreferredWidth (30);
/ / Data entry
tableTuristi.getColumnModel (). GetColumn (11). setPreferredWidth (90);

ListSelectionModel selectionModel = tableTuristi
. getSelectionModel ();
selectionModel.addListSelectionListener (new ListSelectionListener ()
(
public void ValueChanged (ListSelectionEvent event)
(
int selectedRow = tableTuristi.getSelectedRow ();
if (selectedRow! = -1)
(
btnAttiva.setEnabled (true);
btnModificaDati.setEnabled (true);
btnVisualizzaScheda.setEnabled (true);
btnElimina.setEnabled (true);
if ((Boolean) tableModel.getValueAt (selectedRow, 0))
(
btnAttiva.setText ( "Enable <html> <br> region </ html>");
btnAttiva
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/AttivaTurista32.png ")));
)
else
(
btnAttiva
. setText ( "Disable <html> <br> region </ html>");
btnAttiva
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/DisattivaTurista32.png ")));
)
)
else
(
btnElimina.setEnabled (false);
btnModificaDati.setEnabled (false);
btnAttiva.setEnabled (false);
btnVisualizzaScheda.setEnabled (false);
)

)
));
scrollTableTuristi = new JScrollPane ();
scrollTableTuristi.setViewportView (tableTuristi);
scrollTableTuristi
. setHorizontalScrollBarPolicy (JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
scrollTableTuristi
. setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
)
scrollTableTuristi return;
)

/ **
* This method initializes the form to the search for tourists.
*
* @ Return javax.swing.JPanel
*
* /
private JPanel getSearchPanel ()
(
if (null == searchPanel)
(
searchPanel = new JPanel ();
searchPanel.setLayout (new GridBagLayout ());
searchPanel.setBorder (BorderFactory.createTitledBorder (
BorderFactory.createLineBorder (new Color (51, 102, 255), 3),
Research Tourists ", TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
GridBagConstraints g = new GridBagConstraints ();
/ / Create Panel, choose tourists
Tourists JPanel = new JPanel (new GridBagLayout ());
turisti.setPreferredSize (new Dimension (200, 120));
turisti.setBorder (BorderFactory.createLoweredBevelBorder ());
turistiAttivati JToggleButton = new ( "active tourists");
turistiAttivati.setPreferredSize (new Dimension (165, 30));
turistiDisattivati JToggleButton = new ( "Tourists Off");
turistiDisattivati.setPreferredSize (new Dimension (165, 30));
turistiAttivati
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/schedaturista24.png ")));
ActionListener almenoUno = new ActionListener ()
(
public void actionPerformed (ActionEvent pEvent)
(
if (pEvent.getSource () == turistiAttivati)
(
if (! turistiAttivati.isSelected ()
& &! TuristiDisattivati.isSelected ())
(
turistiDisattivati.setSelected (true);
)
)
else
(
if (! turistiAttivati.isSelected ()
& &! TuristiDisattivati.isSelected ())
(
turistiAttivati.setSelected (true);
)
)

)
);
turistiAttivati.addActionListener (almenoUno);
turistiDisattivati.addActionListener (almenoUno);
turistiAttivati.setSelected (true);
turistiDisattivati.setSelected (true);
turistiDisattivati
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/DisattivaTurista24.png ")));
g.insets = new Insets (8, 8, 8, 8);
g.gridx = 0;
g.gridy = 0;
g.weighty = 0.2;
g.fill = GridBagConstraints.HORIZONTAL;
turisti.add (turistiAttivati, g);
g.gridy + +;
turisti.add (turistiDisattivati, g);

/ / Create search panel
g.gridx = 0;
g.gridy = 0;
g.gridwidth = 2;
g.anchor = GridBagConstraints.CENTER;
g.fill = GridBagConstraints.NONE;
g.insets = new Insets (5, 5, 5, 5);
searchPanel.add (new JLabel ( "Name Turista"), g);
cognomeTurista = new JTextField (12);
g.gridy + +;
searchPanel.add (cognomeTurista, g);
g.gridy + +;
searchPanel.add (new JLabel ( "View"), g);
g.gridy + +;
g.weighty = 0.2;
g.insets = new Insets (5, 5, 20, 5);
searchPanel.add (tourists, g);
g.insets = new Insets (5,5,5,5);
g.gridwidth = 1;
g.gridy + +;
searchPanel.add (getBtnRicerca (), g);
g.gridx = 1;
searchPanel.add (getBtnAzzera (), g);
)
searchPanel return;
)

/ **
* This method initializes the panel's online help.
*
* @ Return javax.swing.JPanel
*
* /
private JPanel getPanelHelp ()
(
if (null == panelHelp)
(
panelHelp = new JPanel ();
panelHelp.setLayout (new BorderLayout ());
panelHelp.setBorder (BorderFactory.createTitledBorder (BorderFactory
. createLineBorder (new Color (51, 102, 255), 3),
"Help", TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
panelHelp.setPreferredSize (new Dimension (200, 100));
textGuida = new JTextPane ();
textGuida.setOpaque (false);
textGuida.setContentType ( "text / html");
textGuida.setEditable (false);
textGuida.setOpaque (false);
panelHelp.add (textGuida, BorderLayout.CENTER);

)
panelHelp return;
)

/ **
* This method initializes the side panel of the interface.
*
* @ Return javax.swing.JPanel
*
* /
private JPanel getRightPanel ()
(
if (null == rightPanel)
(
rightPanel = new JPanel ();
rightPanel.setLayout (new GridBagLayout ());
GridBagConstraints g = new GridBagConstraints ();
g.gridx = 0;
g.gridy = 1;
g.fill = GridBagConstraints.BOTH;
g.weighty = 0.3;
rightPanel.add (getPanelHelp (), g);
g.weighty = 0.7;
g.gridx = 0;
g.gridy = 0;
rightPanel.add (getSearchPanel (), g);
)
rightPanel return;
)

/ **
* This method initializes the search button tourists.
*
* @ Return javax.swing.JButton
*
* /
private JButton getBtnRicerca ()
(
if (null == btnRicerca)
(
btnRicerca = new JButton ();
btnRicerca.setText ( "Search");
btnRicerca.setPreferredSize (new Dimension (98, 26));
btnRicerca
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/Ricerca16.png ")));
)
btnRicerca return;
)

/ **
* This method initializes the button to reset form
* Search.
*
* @ Return javax.swing.JButton
* /
private JButton getBtnAzzera ()
(
if (null == btnAzzera)
(
btnAzzera = new JButton ();
btnAzzera.setText ( "Clear");
btnAzzera.setHorizontalTextPosition (SwingConstants.LEADING);
btnAzzera.setPreferredSize (new Dimension (98, 26));
btnAzzera
. setIcon (new ImageIcon (
getClass ()
. getResource (
"/ unisa/gps/etour/gui/operatoreagenzia/images/Azzera16.png ")));
btnAzzera.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent pAcEvent)
(
cognomeTurista.setText ("");
turistiAttivati.setSelected (true);
turistiDisattivati.setSelected (true);
)
));
)
btnAzzera return;
)

/ **
*
* This method opens a tab for the bean tourists supplied input.
*
* @ Param pTurista BeanTurista - the bean tourists
* @ Param boolean pModifica <ul> <li> true - if you change the data.
<li> * False - if you receive the card. </ Ul>
* @ Return void
* /
private void OpenOffice.org Messenger MSN Gaim (BeanTurista pTurista, boolean pModifica)
(
for (int i = 0; i <figli.size (); i + +)
(
filing current figli.get = (i);
if (pTurista.getId () == corrente.getId ())
(
desktopManager.activateFrame (current);
return;
)
)
new = new filing filing (this, pTurista, pModifica);
jDesktopPane.add (new, Integer.MAX_VALUE);
nuova.setVisible (true);
figli.add (new);
)

/ **
*
* Closes the selected tab.
*
* @ Param pScheda profiling - the tab to close.
* @ Return void
* /
protected void closeScheda (filing pScheda)
(
figli.remove (pScheda);
pScheda.dispose ();
)

/ **
*
* Update the table model of the tourists with the bean supplied
* Input.
*
* @ Param pTurista BeanTurista - the bean of the tourist.
* @ Return void
* /
protected void updateTableModel (BeanTurista pTurista)
(
tableModel.updateTurista (pTurista);
)
)