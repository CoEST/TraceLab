package unisa.gps.etour.gui.operatoreagenzia;

import java.awt.BorderLayout;

import javax.swing.JDesktopPane;
import javax.swing.JPanel;
import javax.swing.JInternalFrame;
import java.awt.Dimension;
import javax.swing.ImageIcon;
import java.awt.GridBagLayout;
import javax.swing.JToolBar;
import javax.swing.JLabel;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

import javax.swing.JCheckBox;
import javax.swing.BorderFactory;
import java.awt.Color;
import javax.swing.border.BevelBorder;
import javax.swing.JButton;
import java.awt.GridBagConstraints;
import java.awt.Point;
import javax.swing.JScrollPane;
import javax.swing.border.TitledBorder;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.table.DefaultTableCellRenderer;

import java.awt.font;
import javax.swing.JTextPane;
import javax.swing.JTextField;
import java.awt.Insets;
import java.util.Date;

import javax.swing.SwingConstants;


import unisa.gps.etour.bean.BeanBeneCulturale;
import unisa.gps.etour.bean.BeanPuntoDiRistoro;
import unisa.gps.etour.bean.BeanTag;
import unisa.gps.etour.bean.util.Punto3D;

import javax.swing.JTable;
import java.awt.Rectangle;

extends JInternalFrame (public class PuntiDiRistoro

private Dimension SIZE;
private JPanel jContentPane = null;
private JPanel RightPanel = null;
Private JToolBar toolbarPR = null;
private JLabel status = null;
private JPanel CentrePanel = null;
private JButton btnSkeda = null;
private JButton btnModifica = null;
private JButton btnElimina = null;
private JScrollPane JScrollPane = null;
private JPanel HelpPanel = null;
private JTextPane jTextPane = null;
private JScrollPane PannelloTag = null;
private JButton btnRicerca2 = null;
private JButton btnAzzera2 = null;
private JPanel RicercaPanel = null;
private JLabel LabelPr = null;
private JTextField nomePR = null;
private JLabel LabelTag = null;
private JTable TablePr = null;
Private JDesktopPane JDesktopPane;
Private PuntiDiRistoro internalFrame;
private JButton btnAttiva = null;
private JButton btnStorico = null;
/ **
* This is the default constructor xxx
* /
public PuntiDiRistoro () (
super ();
initialize ();
)

/ **
* This method initializes this
*
* @ Return void
* /
private void initialize () (
SIZE = new Dimension (700.480);
setPreferredSize (SIZE);
this.setSize (new Dimension (700, 480));
this.setResizable (true);
this.setClosable (true);
this.setTitle ( "Refreshments");
this.setFrameIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / PR.png ")));
this.setIconifiable (true);
this.setMaximizable (true);
this.setContentPane (getJContentPane ());
)

/ **
* This method initializes jContentPane
*
* @ Return javax.swing.JPanel
* /
getJContentPane private JPanel () (
if (jContentPane == null) (
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getRightPanel (), BorderLayout.EAST);
jContentPane.add (getToolbarPR (), BorderLayout.NORTH);
jContentPane.add (getCentrePanel (), BorderLayout.CENTER);
)
jContentPane return;
)

/ **
* This method initializes RightPanel
*
* @ Return javax.swing.JPanel
* /
getRightPanel private JPanel () (
if (RightPanel == null) (
RightPanel = new JPanel ();
RightPanel.setLayout (new BorderLayout ());
RightPanel.add (getHelpPanel (), BorderLayout.CENTER);
RightPanel.add (getRicercaPanel (), BorderLayout.CENTER);
)
RightPanel return;
)

/ **
* This method initializes jJToolBarBar
*
* @ Return javax.swing.JToolBar
* /
Private JToolBar getToolbarPR () (
if (toolbarPR == null) (
toolbarPR JToolBar = new ();
toolbarPR.setLayout (null);
toolbarPR.setPreferredSize (new Dimension (1, 49));
toolbarPR.setFloatable (false);
toolbarPR.add (getBtnAttiva ());
toolbarPR.addSeparator ();
toolbarPR.add (getBtnStorico ());
toolbarPR.addSeparator ();
toolbarPR.add (getBtnModifica ());
toolbarPR.addSeparator ();
toolbarPR.add (getBtnSkeda ());
toolbarPR.addSeparator ();
toolbarPR.add (getBtnElimina ());
)
toolbarPR return;
)

/ **
* This method initializes CentrePanel
*
* @ Return javax.swing.JPanel
* /
getCentrePanel private JPanel () (
if (CentrePanel == null) (
GridBagConstraints = GridBagConstraints new GridBagConstraints ();
gridBagConstraints.fill = GridBagConstraints.BOTH;
gridBagConstraints.weighty = 1.0;
gridBagConstraints.weightx = 1.0;
CentrePanel = new JPanel ();
CentrePanel.setLayout (new GridBagLayout ());
CentrePanel.setPreferredSize (new Dimension (1, 30));
CentrePanel.add (getJScrollPane (), GridBagConstraints);

)
CentrePanel return;
)

/ **
* This method initializes btnSkeda
*
* @ Return javax.swing.JButton
* /
private JButton getBtnSkeda () (
if (btnSkeda == null) (
btnSkeda = new JButton ();
btnSkeda.setText ( "Point Card <html> <br> Refreshments </ html>");
btnSkeda.setLocation (new Point (16, 3));
btnSkeda.setSize (new Dimension (130, 42));
btnSkeda.setPreferredSize (new Dimension (130, 42));
btnSkeda.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / Browse 1.png ")));
btnSkeda.addActionListener (new ActionListener () (
public void actionPerformed (ActionEvent arg0) (

)

));
)
btnSkeda return;
)

/ **
* This method initializes btnModifica
*
* @ Return javax.swing.JButton
* /
private JButton getBtnModifica () (
if (btnModifica == null) (
btnModifica = new JButton ();
btnModifica.setText ( "<html> Edit Point <br> Refreshments </ html>");
btnModifica.setPreferredSize (new Dimension (130, 42));
btnModifica.setMnemonic (KeyEvent.VK_UNDEFINED);
btnModifica.setEnabled (false);
btnModifica.setBounds (new Rectangle (413, 3, 140, 42));
btnModifica.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/edit-32x32.png ")));
)
btnModifica return;
)

/ **
* This method initializes btnElimina
*
* @ Return javax.swing.JButton
* /
private JButton getBtnElimina () (
if (btnElimina == null) (
btnElimina = new JButton ();
btnElimina.setText ( "Delete item <html> <br> Refreshments </ html>");
btnElimina.setPreferredSize (new Dimension (130, 42));
btnElimina.setEnabled (false);
btnElimina.setMnemonic (KeyEvent.VK_UNDEFINED);
btnElimina.setBounds (new Rectangle (555, 3, 130, 42));
btnElimina.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/edit-delete-32x32.png ")));
)
btnElimina return;
)

/ **
* This method initializes JScrollPane
*
* @ Return javax.swing.JScrollPane
* /
private JScrollPane getJScrollPane () (
if (JScrollPane == null) (
if (TablePr == null) (
/ *
* @ Param pId
* @ Param pNumeroVoti
* @ Param Pnom
* @ Param pCitta
* @ Param pTelefono
* @ Param pDescrizione
* @ Param pLocalita
* @ Param pVia
* @ Param pcap
* @ Param pProvincia
* @ Param pPosizione
* @ Param pOrarioApertura
* @ Param pOrarioChiusura
* @ Param pGiornoChiusura
* @ Param pCostoBiglietto
* @ Param pMediaVoti
* /

BeanPuntoDiRistoro [] test = new BeanPuntoDiRistoro [30];
for (int i = 0; i <30; i + +) (
test [i] = new BeanPuntoDiRistoro (1, 12, 3.5,
"Arturo", "Near the sea, great view, romantic and Miao,
"089203202", "the mountains", "Amalfi", "Via Principe 35", "84123rd"
"Salerno", "1234567898741", new Punto3D (34,34,34),
new Date (2,23,3), new Date (3,3,4), "Monday");
)

SitoTableModel new SitoTableModel = new (test);

TablePr = new ScrollableTable (new);
/ / Name
TablePr.getColumnModel (). GetColumn (0). SetPreferredWidth (140);
/ / Description
TablePr.getColumnModel (). GetColumn (1). SetPreferredWidth (80);
/ / Location
TablePr.getColumnModel (). GetColumn (2). SetPreferredWidth (140);
/ / City
TablePr.getColumnModel (). GetColumn (3). SetPreferredWidth (80);
/ / State
TablePr.getColumnModel (). GetColumn (4). SetPreferredWidth (70);
/ / Number of Votes
TablePr.getColumnModel (). GetColumn (5). SetPreferredWidth (70);
/ / Average Ratings
TablePr.getColumnModel (). GetColumn (6). SetPreferredWidth (60);
/ / PosGeo
TablePr.getColumnModel (). GetColumn (7). SetPreferredWidth (30);
TablePr.setDefaultRenderer (String.class, new DefaultTableCellRenderer ());
TablePr.setRowHeight (32);
TablePr.setShowVerticalLines (false);
TablePr.setDefaultRenderer (Double.class, new MediaVotiRenderer ());
TablePr.setSelectionMode (javax.swing.ListSelectionModel.SINGLE_SELECTION);
TablePr.setSelectionForeground (Color.RED);
TablePr.setSelectionBackground (Color.white);
TablePr.setColumnSelectionAllowed (false);
final javax.swing.ListSelectionModel this TablePr.getSelectionModel = ();
questo.addListSelectionListener (new ListSelectionListener () (

public void ValueChanged (ListSelectionEvent event) (
btnElimina.setEnabled (true);
btnModifica.setEnabled (true);
btnAttiva.setEnabled (true);
btnStorico.setEnabled (true);
)

));

TablePr.addKeyListener (new KeyListener () (

public void keyPressed (KeyEvent arg0) (
System.out.println ( "keyPressed");

)

public void keyReleased (KeyEvent arg0) (
System.out.println ( "keyRelased");

)

public void keyTyped (KeyEvent arg0) (
System.out.println ( "keyTyped");

)

));

/ / private static final String [] headers = ( "Name", "Phone", "Gone", "Location", "City", "CPC", "Province", "Media Votes", "Number Votes");

)

JScrollPane = new JScrollPane (TablePr);
jScrollPane.setHorizontalScrollBarPolicy (JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
jScrollPane.setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);

)
JScrollPane return;
)


/ **
* This method initializes HelpPanel
*
* @ Return javax.swing.JPanel
* /
getHelpPanel private JPanel () (
if (HelpPanel == null) (
GridBagConstraints gridBagConstraints1 = new GridBagConstraints ();
gridBagConstraints1.fill = GridBagConstraints.BOTH;
gridBagConstraints1.gridy = 0;
gridBagConstraints1.weightx = 1.0;
gridBagConstraints1.weighty = 1.0;
gridBagConstraints1.ipadx = 0;
gridBagConstraints1.gridwidth = 1;
gridBagConstraints1.ipady = 0;
gridBagConstraints1.gridx = 0;
HelpPanel = new JPanel ();
HelpPanel.setLayout (new GridBagLayout ());
HelpPanel.setBorder (BorderFactory.createTitledBorder (BorderFactory.createLineBorder (new Color (51, 153, 255), 3), "Help", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font ( "Dialog", Font.BOLD, 12), new Color (51, 102, 255)));
HelpPanel.setPreferredSize (new Dimension (100, 100));
HelpPanel.add (getJTextPane (), gridBagConstraints1);
)
HelpPanel return;
)

/ **
* This method initializes jTextPane
*
* @ Return javax.swing.JTextPane
* /
private JTextPane getJTextPane () (
if (jTextPane == null) (
jTextPane = new JTextPane ();
jTextPane.setPreferredSize (new Dimension (6, 30));
)
jTextPane return;
)

/ **
* This method initializes ricercaPanel1
*
* @ Return javax.swing.JPanel
* /

/ **
* This method initializes btnRicerca2
*
* @ Return javax.swing.JButton
* /
private JButton getBtnRicerca2 () (
if (btnRicerca2 == null) (
btnRicerca2 = new JButton ();
btnRicerca2.setPreferredSize (new Dimension (98, 26));
btnRicerca2.setText ( "Search");
btnRicerca2.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa/gps/etour/gui/operatoreagenzia/images/Ricerca16.png ")));
)
btnRicerca2 return;
)

/ **
* This method initializes btnAzzera2
*
* @ Return javax.swing.JButton
* /
private JButton getBtnAzzera2 () (
if (btnAzzera2 == null) (
btnAzzera2 = new JButton ();
btnAzzera2.setPreferredSize (new Dimension (98, 26));
btnAzzera2.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa/gps/etour/gui/operatoreagenzia/images/Azzera16.png ")));
btnAzzera2.setText ( "Clear");
btnAzzera2.setHorizontalTextPosition (SwingConstants.LEADING);
)
btnAzzera2 return;
)

/ **
* This method initializes RicercaPanel
*
* @ Return javax.swing.JPanel
* /
getRicercaPanel private JPanel () (
if (RicercaPanel == null) (
GridBagConstraints gridBagConstraints5 = new GridBagConstraints ();
gridBagConstraints5.fill = GridBagConstraints.BOTH;
gridBagConstraints5.gridy = 4;
gridBagConstraints5.weightx = 1.0;
gridBagConstraints5.weighty = 1.0;
gridBagConstraints5.gridwidth = 2;
gridBagConstraints5.gridx = 0;
GridBagConstraints gridBagConstraints8 = new GridBagConstraints ();
gridBagConstraints8.gridwidth = 2;
gridBagConstraints8.insets = new Insets (5, 5, 5, 5);
GridBagConstraints gridBagConstraints7 = new GridBagConstraints ();
gridBagConstraints7.gridx = 0;
gridBagConstraints7.insets = new Insets (5, 5, 5, 5);
gridBagConstraints7.gridy = 6;
GridBagConstraints gridBagConstraints6 = new GridBagConstraints ();
gridBagConstraints6.gridx = 1;
gridBagConstraints6.gridwidth = 2;
gridBagConstraints6.insets = new Insets (5, 5, 5, 5);
gridBagConstraints6.gridy = 6;
GridBagConstraints gridBagConstraints4 = new GridBagConstraints ();
gridBagConstraints4.gridx = 0;
gridBagConstraints4.gridwidth = 2;
gridBagConstraints4.insets = new Insets (5, 5, 5, 5);
gridBagConstraints4.gridy = 3;
LabelTag = new JLabel ();
LabelTag.setText ( "Select search tags:");
GridBagConstraints gridBagConstraints3 = new GridBagConstraints ();
gridBagConstraints3.fill = GridBagConstraints.VERTICAL;
gridBagConstraints3.gridx = 0;
gridBagConstraints3.gridy = 2;
gridBagConstraints3.gridwidth = 2;
gridBagConstraints3.insets = new Insets (5, 5, 5, 5);
gridBagConstraints3.weightx = 1.0;
LabelPr = new JLabel ();
LabelPr.setText ( "Name Refreshments:");
RicercaPanel = new JPanel ();
RicercaPanel.setLayout (new GridBagLayout ());
RicercaPanel.setBorder (BorderFactory.createTitledBorder (BorderFactory.createLineBorder (new Color (51, 153, 255), 3), "Searching for refreshments:" TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font ( "Dialog", Font. BOLD, 12), new Color (51, 102, 255)));
RicercaPanel.add (LabelPr, gridBagConstraints8);
RicercaPanel.add (getNomePR (), gridBagConstraints3);
RicercaPanel.add (LabelTag, gridBagConstraints4);
BeanTag [] test = new BeanTag [8];
test [0] = new BeanTag (0, "castle", "really a castle");
test [1] = new BeanTag (1, "stronghold", "really a hostel");
test [2] = new BeanTag (3, "Pub", "really a basket");
test [3] = new BeanTag (4, "Restaurant", "really a basket");
test [4] = new BeanTag (5, "Pizza", "really a basket");
test [5] = new BeanTag (6, "Trattoria", "really a basket");
test [6] = new BeanTag (7, "range", "really a basket");
test [7] = new BeanTag (8, "Romantic", "really a basket");
PannelloTag = new TagPanel (test);
PannelloTag.setPreferredSize (new Dimension (180.40));
RicercaPanel.add (getBtnAzzera2 (), gridBagConstraints6);
RicercaPanel.add (getBtnRicerca2 (), gridBagConstraints7);
RicercaPanel.add (PannelloTag, gridBagConstraints5);
)
RicercaPanel return;
)

/ **
* This method initializes nomePR
*
* @ Return javax.swing.JTextField
* /
private JTextField getNomePR () (
if (nomePR == null) (
nomePR = new JTextField ();
nomePR.setColumns (12);
)
nomePR return;
)

/ **
* This method initializes btnAttiva
*
* @ Return javax.swing.JButton
* /
private JButton getBtnAttiva () (
if (btnAttiva == null) (
btnAttiva = new JButton ();
btnAttiva.setText ( "Enable <html> <br> Convention </ html>");
btnAttiva.setPreferredSize (new Dimension (130, 42));
btnAttiva.setSize (new Dimension (130, 42));
btnAttiva.setLocation (new Point (280, 3));
btnAttiva.setMnemonic (KeyEvent.VK_UNDEFINED);
btnAttiva.setEnabled (false);
btnAttiva.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/wi0054-32x32.png ")));
)
btnAttiva return;
)

/ **
* This method initializes btnStorico
*
* @ Return javax.swing.JButton
* /
private JButton getBtnStorico () (
if (btnStorico == null) (
btnStorico = new JButton ();
btnStorico.setText ( "Historical <html> <br> Conventions </ html>");
btnStorico.setPreferredSize (new Dimension (130, 42));
btnStorico.setBounds (new Rectangle (148, 3, 130, 42));
btnStorico.setEnabled (false);
btnStorico.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / Browse 1.png ")));
btnStorico.addActionListener (new java.awt.event.ActionListener ()
(
public void actionPerformed (java.awt.event.ActionEvent e)
(
New StoricoConvenzioni ();
)
));
)
btnStorico return;
)



) / / @ JVE: decl-index = 0: visual-constraint = "10.10"
