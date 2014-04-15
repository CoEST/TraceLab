package unisa.gps.etour.gui.operatoreagenzia;

import java.awt.BorderLayout;
import javax.swing.JPanel;
import javax.swing.JInternalFrame;

import unisa.gps.etour.bean.BeanTag;

import java.awt.GridBagLayout;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import java.awt.GridBagConstraints;
import java.awt.Dimension;
import javax.swing.JToolBar;
import javax.swing.JButton;
import javax.swing.ImageIcon;
import javax.swing.JTextPane;
import javax.swing.BorderFactory;
import javax.swing.border.TitledBorder;
import java.awt.Color;
import java.awt.font;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.JTextArea;
import java.awt.Insets;

public class Tag extends JInternalFrame
(

private JPanel jContentPane = null;
private JPanel CenterPanel = null;
private JPanel EastPanel = null;
private JScrollPane JScrollPane = null;
private JTable JTable = null;
Private JToolBar jJToolBarBar = null;
private JButton btnModifica = null;
private JButton btnElimina = null;
private JButton btnExit = null;
private JPanel jPanelUp = null;
private JPanel jPanelHelp = null;
private JTextPane jTextPane = null;
private JLabel tagname = null;
private JTextField JTextField = null;
private JLabel description = null;
private JTextArea JTextArea = null;
private JButton btnOK = null;
private JButton btnResetta = null;

/ **
* This is the default constructor xxx
* /
public Tag ()
(
super ();
initialize ();
)

/ **
* This method initializes this
*
* @ Return void
* /
private void initialize ()
(
this.setSize (508, 398);
this.setFrameIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / Properties.png ")));
this.setTitle ( "Manage Tag");
this.setContentPane (getJContentPane ());
)

/ **
* This method initializes jContentPane
*
* @ Return javax.swing.JPanel
* /
private JPanel getJContentPane ()
(
if (null == jContentPane)
(
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getCenterPanel (), BorderLayout.CENTER);
jContentPane.add (getEastPanel (), BorderLayout.EAST);
jContentPane.add (getJJToolBarBar (), BorderLayout.NORTH);
)
jContentPane return;
)

/ **
* This method initializes CenterPanel
*
* @ Return javax.swing.JPanel
* /
private JPanel getCenterPanel ()
(
if (null == CenterPanel)
(
GridBagConstraints = GridBagConstraints new GridBagConstraints ();
gridBagConstraints.fill = GridBagConstraints.BOTH;
gridBagConstraints.weighty = 1.0;
gridBagConstraints.weightx = 1.0;
CenterPanel = new JPanel ();
CenterPanel.setLayout (new GridBagLayout ());
CenterPanel.add (getJScrollPane (), GridBagConstraints);


)
CenterPanel return;
)

/ **
* This method initializes EastPanel
*
* @ Return javax.swing.JPanel
* /
private JPanel getEastPanel ()
(
if (null == EastPanel)
(
GridBagConstraints gridBagConstraints3 = new GridBagConstraints ();
gridBagConstraints3.gridy = 1;
GridBagConstraints gridBagConstraints2 = new GridBagConstraints ();
gridBagConstraints2.gridx = 0;
gridBagConstraints2.gridy = 0;
EastPanel = new JPanel ();
EastPanel.setLayout (new GridBagLayout ());
EastPanel.add (getJPanelUp (), gridBagConstraints2);
EastPanel.add (getJPanelHelp (), gridBagConstraints3);
)
EastPanel return;
)

/ **
* This method initializes JScrollPane
*
* @ Return javax.swing.JScrollPane
* /
private JScrollPane getJScrollPane ()
(
if (JScrollPane == null)
(
JScrollPane = new JScrollPane ();
jScrollPane.setViewportView (getJTable ());
)
JScrollPane return;
)

/ **
* This method initializes JTable
*
* @ Return javax.swing.JTable
* /
private JTable getJTable ()
(
if (JTable == null)
(
JTable = new JTable ();
BeanTag [] listaTag = new BeanTag [11];
listaTag [0] = new BeanTag (1, "romantic", "place for couples and unforgettable moments");
listaTag [1] = new BeanTag (2, "esoteric", "places of magic");
listaTag [2] = new BeanTag (3, "pizza", "The best pizza");
listaTag [3] = new BeanTag (6, "music", "live music venues, concerts ...");
listaTag [4] = new BeanTag (76, "trattoria", "typical");
listaTag [5] = new BeanTag (7, "fairs", "for important purchases or souvenirs bellismi");
listaTag [6] = new BeanTag (9, "Market", "typical");
listaTag [7] = new BeanTag (8, "History", "typical");
listaTag [8] = new BeanTag (5, "nineteenth century", "typical");
listaTag [9] = new BeanTag (4, "range", "typical");
listaTag [10] = new BeanTag (56, "Cinema", "typical");

)
return JTable;
)

/ **
* This method initializes jJToolBarBar
*
* @ Return javax.swing.JToolBar
* /
Private JToolBar getJJToolBarBar ()
(
if (null == jJToolBarBar)
(
jJToolBarBar JToolBar = new ();
jJToolBarBar.add (getBtnModifica ());
jJToolBarBar.add (getBtnElimina ());
jJToolBarBar.addSeparator ();
jJToolBarBar.add (getBtnExit ());

)
jJToolBarBar return;
)

/ **
* This method initializes btnModifica
*
* @ Return javax.swing.JButton
* /
private JButton getBtnModifica ()
(
if (null == btnModifica)
(
btnModifica = new JButton ();
btnModifica.setText ( "Edit Tag");
btnModifica.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/edit-32x32.png ")));
)
btnModifica return;
)

/ **
* This method initializes btnElimina
*
* @ Return javax.swing.JButton
* /
private JButton getBtnElimina ()
(
if (null == btnElimina)
(
btnElimina = new JButton ();
btnElimina.setText ( "Remove Tag");
btnElimina.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/button-cancel-32x32.png ")));
)
btnElimina return;
)

/ **
* This method initializes btnExit
*
* @ Return javax.swing.JButton
* /
private JButton getBtnExit ()
(
if (null == btnExit)
(
btnExit = new JButton ();
btnExit.setText ( "Exit");
btnExit.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / Forward 2.png ")));
)
btnExit return;
)

/ **
* This method initializes jPanelUp
*
* @ Return javax.swing.JPanel
* /
private JPanel getJPanelUp ()
(
if (null == jPanelUp)
(
GridBagConstraints gridBagConstraints9 = new GridBagConstraints ();
gridBagConstraints9.gridx = 1;
gridBagConstraints9.insets = new Insets (5, 5, 5, 5);
gridBagConstraints9.gridy = 4;
GridBagConstraints gridBagConstraints8 = new GridBagConstraints ();
gridBagConstraints8.gridx = 0;
gridBagConstraints8.insets = new Insets (5, 5, 5, 5);
gridBagConstraints8.gridy = 4;
GridBagConstraints gridBagConstraints7 = new GridBagConstraints ();
gridBagConstraints7.fill = GridBagConstraints.BOTH;
gridBagConstraints7.gridy = 3;
gridBagConstraints7.weightx = 1.0;
gridBagConstraints7.weighty = 1.0;
gridBagConstraints7.gridwidth = 2;
gridBagConstraints7.insets = new Insets (5, 0, 5, 0);
gridBagConstraints7.gridx = 0;
GridBagConstraints gridBagConstraints6 = new GridBagConstraints ();
gridBagConstraints6.gridx = 0;
gridBagConstraints6.gridwidth = 2;
gridBagConstraints6.insets = new Insets (5, 0, 5, 0);
gridBagConstraints6.gridy = 2;
Description = new JLabel ();
Descrizione.setText ( "Description:");
GridBagConstraints gridBagConstraints5 = new GridBagConstraints ();
gridBagConstraints5.fill = GridBagConstraints.VERTICAL;
gridBagConstraints5.gridy = 1;
gridBagConstraints5.weightx = 1.0;
gridBagConstraints5.gridwidth = 2;
gridBagConstraints5.insets = new Insets (5, 0, 5, 0);
gridBagConstraints5.gridx = 0;
GridBagConstraints gridBagConstraints4 = new GridBagConstraints ();
gridBagConstraints4.gridx = 0;
gridBagConstraints4.gridwidth = 2;
gridBagConstraints4.insets = new Insets (5, 0, 5, 0);
gridBagConstraints4.gridy = 0;
Tagname = new JLabel ();
Nometag.setText ( "Tag Name:");
jPanelUp = new JPanel ();
jPanelUp.setLayout (new GridBagLayout ());
jPanelUp.setBorder (BorderFactory.createTitledBorder (BorderFactory.createLineBorder (new Color (0, 102, 255), 3), "Insert New", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font ( "Dialog", Font.BOLD, 12 ), new Color (0, 102, 255)));
jPanelUp.add (tagname, gridBagConstraints4);
jPanelUp.add (getJTextField (), gridBagConstraints5);
jPanelUp.add (Description, gridBagConstraints6);
jPanelUp.add (getJTextArea (), gridBagConstraints7);
jPanelUp.add (getBtnOk (), gridBagConstraints8);
jPanelUp.add (getBtnResetta (), gridBagConstraints9);
)
jPanelUp return;
)

/ **
* This method initializes jPanelHelp
*
* @ Return javax.swing.JPanel
* /
private JPanel getJPanelHelp ()
(
if (null == jPanelHelp)
(
GridBagConstraints gridBagConstraints1 = new GridBagConstraints ();
gridBagConstraints1.fill = GridBagConstraints.BOTH;
gridBagConstraints1.weighty = 1.0;
gridBagConstraints1.weightx = 1.0;
jPanelHelp = new JPanel ();
jPanelHelp.setLayout (new GridBagLayout ());
jPanelHelp.setBorder (BorderFactory.createTitledBorder (BorderFactory.createLineBorder (new Color (0, 102, 255), 3), "Help", TitledBorder.DEFAULT_JUSTIFICATION, TitledBorder.DEFAULT_POSITION, new Font ( "Dialog", Font.BOLD, 12), new Color (51, 153, 255)));
jPanelHelp.add (getJTextPane (), gridBagConstraints1);
)
jPanelHelp return;
)

/ **
* This method initializes jTextPane
*
* @ Return javax.swing.JTextPane
* /
private JTextPane getJTextPane ()
(
if (null == jTextPane)
(
jTextPane = new JTextPane ();
jTextPane.setPreferredSize (new Dimension (190, 80));
)
jTextPane return;
)

/ **
* This method initializes JTextField
*
* @ Return javax.swing.JTextField
* /
private JTextField getJTextField ()
(
if (JTextField == null)
(
JTextField = new JTextField ();
jTextField.setColumns (10);
)
JTextField return;
)

/ **
* This method initializes JTextArea
*
* @ Return javax.swing.JTextArea
* /
private JTextArea getJTextArea ()
(
if (JTextArea == null)
(
JTextArea = new JTextArea ();
jTextArea.setRows (3);
)
JTextArea return;
)

/ **
* This method initializes btnOK
*
* @ Return javax.swing.JButton
* /
private JButton getBtnOk ()
(
if (btnOK == null)
(
btnOK = new JButton ();
btnOk.setText ( "Ok");
btnOk.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia/immagini/Button_ok16.png ")));
)
btnOK return;
)

/ **
* This method initializes btnResetta
*
* @ Return javax.swing.JButton
* /
private JButton getBtnResetta ()
(
if (null == btnResetta)
(
btnResetta = new JButton ();
btnResetta.setText ( "Reset");
btnResetta.setIcon (new ImageIcon (getClass (). getResource ( "/ interfacceAgenzia / images / Trash.png ")));
)
btnResetta return;
)

) / / @ JVE: decl-index = 0: visual-constraint = "10.10"


