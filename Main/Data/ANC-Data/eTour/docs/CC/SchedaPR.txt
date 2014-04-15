/ *
* SchedaPR.java
*
* 1.0
*
* 28/05/2007
*
* © 2007 eTour Project - Copyright by SE @ SA Lab - DMI - University of Salerno
* /

package unisa.gps.etour.gui.operatoreagenzia;

import java.awt.BorderLayout;

import javax.swing.BorderFactory;
import javax.swing.InputVerifier;
import javax.swing.JComponent;
import javax.swing.JPanel;
import javax.swing.JInternalFrame;
import javax.swing.ImageIcon;
import javax.swing.WindowConstants;
import java.awt.Dimension;
import javax.swing.JTabbedPane;
import java.awt.GridBagLayout;
import javax.swing.JLabel;
import java.awt.GridBagConstraints;
import javax.swing.JTextField;
import javax.swing.JComboBox;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;

import java.awt.Color;
import java.awt.Component;
import java.awt.Insets;
import java.awt.Rectangle;
import javax.swing.JToolBar;
import javax.swing.JToggleButton;
import javax.swing.JButton;
import java.awt.font;
import java.awt.Cursor;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.util.StringTokenizer;

import javax.swing.JComboBox.KeySelectionManager;
import javax.swing.border.SoftBevelBorder;
import javax.swing.border.TitledBorder;
import javax.swing.text.AttributeSet;
import javax.swing.text.BadLocationException;
import javax.swing.text.PlainDocument;
import javax.swing.JCheckBox;
import javax.swing.JTable;
import unisa.gps.etour.bean .*;
import unisa.gps.etour.bean.util.Punto3D;
/ **
  * Class that models the interface for viewing the card,
  * Modify the data and the insertion of a new resting spot.
  *
  * @ Author Lello
  *
  * /
extends JInternalFrame (public class SchedaPR

private JPanel jContentPane = null;
Private JToolBar toolbarSchedaBC = null;
Private JToggleButton btnModifica = null;
private JButton btnSalva = null;
private JButton btnAnnulla = null;
btnModificaCommento JButton private = null;
private JTabbedPane JTabbedPane = null;
private JPanel statistics = null;
private JPanel feedback = null;
private JLabel txtNome = null;
private JLabel txtIndirizzo = null;
private JLabel txtCAP = null;
private JLabel txtCitta = null;
private JLabel txtLocalità = null;
private JLabel txtProvincia = null;
private JLabel txtPos = null;
private JLabel txtTel = null;
private JLabel txtOraAp = null;
private JLabel txtOraCh = null;

private JLabel JLabel = null;
private JTextField indirizzoPR = null;
private JComboBox indirizzoPR1 = null;
private JTextField cittaPR = null;
private JComboBox localitaPR = null;
private JTextField capPR = null;
private JScrollPane JScrollPane = null;
private JTextArea descrizionePR = null;
private JTextField telefonoPR = null;
private JComboBox orarioAPOrePR = null;
private JLabel jLabel1 = null;
private JComboBox orarioApMinPR = null;
Private TagPanel pannelloTag;
private JTextField costoBC = null;

private JLabel jLabel3 = null;
private JComboBox orarioCHMinPR = null;
private JComboBox provPR = null;
private JPanel datiPR = null;
private JTextField nomePR = null;
private JPanel JPanel = null;
private JScrollPane jScrollPane2 = null;
private JTable feedbackTable = null;
private JLabel txtNomeBene = null;
private JLabel mediaVotoPR = null;
private JPanel statisticheMeseCorrente = null;
private JPanel statisticheTotali = null;
private JLabel jLabel4 = null;
private JLabel jLabel41 = null;
private ActionListener campoCompilato;
Private FocusListener validating;
Private JToolBar ToolbarSchedaPR = null;
private JTextField posGeoX = null;
private JTextField posGeoY = null;
private JTextField posGeoZ = null;
private JLabel jLabel2 = null;
private JComboBox orarioCHOrePR = null;
/ **
* The default constructor for inclusion of the interface model
* A new refreshment.
*
* /
public SchedaPR ()
(
super ( "New Refreshment");
campoCompilato = new ActionListener () (

public void actionPerformed (ActionEvent actionEvent)
(
((JComponent) actionEvent.getSource ()). TransferFocus ();
)

);
validating FocusListener = new () (

private final ERROR_BACKGROUND Color = new Color (255, 215, 215);
private final WARNING_BACKGROUND Color = new Color (255, 235, 205);
private String text;

public void focusGained (FocusEvent fe) (
if (fe.getSource () instanceof JTextField) (
JTextField textbox = (JTextField) fe.getSource ();
textbox.getText text = ();
)

)

public void focusLost (FocusEvent fe) (
if (fe.getSource () instanceof JTextField) (
JTextField textbox = (JTextField) fe.getSource ();
if (! text.equals (textbox.getText ())) (
textbox.getText text = ();
if (text.equals ("")) (
textbox.setBackground (ERROR_BACKGROUND);
Rectangle bounds = textbox.getBounds ();
JLabel new = new JLabel ();
nuova.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / images / error.png ")));
nuova.setBounds (bounds.x-24, bounds.y, 24.24);
nuova.setToolTipText ( "Field" + textbox.getName () + "can not be empty!");
datiPR.add (new, null);
datiPR.repaint ();
)
)
)

)

);
initialize ();
)
/ **
* This interface models the manufacturer regarding modification of data and
* Display board a refreshment.
* @ Param unisa.gps.etour.bean.BeanPuntoDiRistoro PR - the bean contains the data of
* PuntoDiRistoro selected.
* @ Param boolean change - indicates whether the fields should be editable, so if
* You are viewing a card or change the cultural property.
*
* /
public SchedaPR (BeanPuntoDiRistoro pr, boolean edit)
(
this ();

nomePR.setText (pr.getNome ());
setTitle (pr.getNome ());
capPR.setText (pr.getCap ());
cittaPR.setText (pr.getCitta ());

descrizionePR.setText (pr.getDescrizione ());
StringTokenizer tokenizer = new StringTokenizer (pr.getVia ());
/ *
* IndirizzoPR1.addItem ( "Via");
indirizzoPR1.addItem (P.zza ");
indirizzoPR1.addItem ( "V.le");
indirizzoPR1.addItem (V.co ");
indirizzoPR1.addItem ( "Largo");
indirizzoPR1.addItem ( "Course");
* /
String [] path = ( "Street", "P.zza", "V.le", "V.co", "Largo", "Course");
String string = tokenizer.nextToken ();
int i;
for (i = 0; i <via.length i + +)
if (stringa.equalsIgnoreCase (via [i]))
break;
this.indirizzoPR1.setSelectedIndex (i);
while (tokenizer.hasMoreTokens ())
this.indirizzoPR.setText (indirizzoPR.getText () + "" + tokenizer.nextToken ());
this.provPR.setSelectedItem (pr.getProvincia ());
Punto3D pos = pr.getPosizione ();
this.posGeoX.setText ( "" + pos.getX ());
this.posGeoY.setText ( "" + pos.getY ());
this.posGeoZ.setText ( "" + pos.getZ ());
this.telefonoPR.setText (pr.getTelefono ());
int minutes = pr.getOrarioApertura (). getMinutes ();
if (minutes == 0)
this.orarioApMinPR.setSelectedIndex (0);
else
this.orarioApMinPR.setSelectedItem (minutes);
int hours = pr.getOrarioApertura (). getHours ();
if (hours <10)
this.orarioAPOrePR.setSelectedItem ( "0" + hours);
else
this.orarioAPOrePR.setSelectedItem (hours);
this.orarioCHMinPR.setSelectedItem (pr.getOrarioChiusura (). getMinutes ());
this.orarioAPOrePR.setSelectedItem (pr.getOrarioApertura (). getHours ());
this.orarioCHOrePR.setSelectedItem (pr.getOrarioChiusura (). getHours ());
if (change) (
btnModifica.setSelected (true);
)
else (
makeEditabled ();
)

)
/ **
* Method called by the constructor
*
* @ Return void
* /
private void initialize () (
this.setIconifiable (true);
this.setBounds (new Rectangle (0, 0, 600, 540));
this.setDefaultCloseOperation (WindowConstants.DO_NOTHING_ON_CLOSE);
this.setFrameIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / scheda.png ")));
this.setClosable (true);
this.setContentPane (getJContentPane ());
)

private void makeEditabled ()
(
Component [] components = datiPR.getComponents ();
for (int i = 0; i <componenti.length i + +) (
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
descrizionePR.setEditable (descrizionePR.isEditable ()? false: true);
pannelloTag.attivaDisattiva ();
)


/ **
* Method which initializes a jContentPane
*
* @ Return javax.swing.JPanel
* /
getJContentPane private JPanel () (
if (jContentPane == null) (
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());

jContentPane.add (getJTabbedPane (), BorderLayout.CENTER);
jContentPane.add (getToolbarSchedaPR (), BorderLayout.CENTER);
)
jContentPane return;
)

/ **
* This method initializes the button (ToggleButton) the alteration
* Data for puntoDiRistoro
*
* @ Return javax.swing.JToggleButton
* /
Private JToggleButton getBtnModifica () (
if (btnModifica == null) (
btnModifica JToggleButton = new ();
btnModifica.setText ( "Change Data");
btnModifica.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / modifica.png ")));
btnModifica.addActionListener (new ActionListener () (

public void actionPerformed (ActionEvent arg0) (
makeEditabled ();
btnSalva.setVisible ((btnModifica.isSelected ()? true: false));
btnAnnulla.setVisible ((btnModifica.isSelected ()? true: false));


)

));
)
btnModifica return;
)

/ **
* Method to initialize the Save button (btnSalva)
*
* @ Return javax.swing.JButton
* /
private JButton getBtnSalva () (
if (btnSalva == null) (
btnSalva = new JButton ();
btnSalva.setText ( "Save");
btnSalva.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / salva.png ")));
btnSalva.setVisible (false);
)
btnSalva return;
)

/ **
* Method to initialize the Cancel button (btnAnnulla)
*
* @ Return javax.swing.JButton
* /
private JButton getBtnAnnulla () (
if (btnAnnulla == null) (
btnAnnulla = new JButton ();
btnAnnulla.setText ( "Cancel");
btnAnnulla.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / annulla.png ")));
btnAnnulla.setVisible (false);
)
btnAnnulla return;
)

/ **
* Method to initialize the button for
* Changing a comment (btnModificaCommento)
*
* @ Return javax.swing.JButton
* /
private JButton getBtnModificaCommento () (
if (btnModificaCommento == null) (
btnModificaCommento = new JButton ();
btnModificaCommento.setText ( "Edit Comment");
btnModificaCommento.setIcon (new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / modificaCommento.png ")));
btnModificaCommento.setVisible (false);
)
btnModificaCommento return;
)

/ **
* Create and initialize a JTabbedPane
*
* @ Return javax.swing.JTabbedPane
* /
private JTabbedPane getJTabbedPane () (
if (JTabbedPane == null) (
JTabbedPane = new JTabbedPane ();
jTabbedPane.setCursor (new Cursor (Cursor.DEFAULT_CURSOR));
jTabbedPane.addTab ( "Data Refreshment", new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / dati.png")), getDatiPR (), null);
jTabbedPane.addTab ( "MenuTuristico", new ImageIcon (getClass (). getResource ( "/ unisa/gps/etour/gui/operatoreagenzia/images/stat24.png")), null, null);
jTabbedPane.addTab ( "Statistics", new ImageIcon (getClass (). getResource ( "/ unisa/gps/etour/gui/operatoreagenzia/images/stat24.png")), getStatistiche (), null);
jTabbedPane.addTab ( "Feedback received", new ImageIcon (getClass (). getResource ( "/ unisa / gps / eTour / gui / operatoreagenzia / images / feedback.png")), getFeedback (), null);

)
JTabbedPane return;
)
/ **
* Method to initialize a panel (datiPR)
*
* @ Return javax.swing.JPanel
* /
getDatiPR private JPanel () (
if (datiPR == null) (
GridBagConstraints gridBagConstraints27 = new GridBagConstraints ();
gridBagConstraints27.fill = GridBagConstraints.VERTICAL;
gridBagConstraints27.gridy = 9;
gridBagConstraints27.weightx = 1.0;
gridBagConstraints27.anchor = GridBagConstraints.WEST;
gridBagConstraints27.insets = new Insets (5, 5, 36, 0);
gridBagConstraints27.ipadx = 18;
gridBagConstraints27.gridx = 1;
GridBagConstraints gridBagConstraints34 = new GridBagConstraints ();
gridBagConstraints34.gridx = 7;
gridBagConstraints34.insets = new Insets (0, 0, 0, 0);
gridBagConstraints34.gridy = 6;
jLabel2 = new JLabel ();
jLabel2.setText ( "z");
GridBagConstraints gridBagConstraints33 = new GridBagConstraints ();
gridBagConstraints33.fill = GridBagConstraints.VERTICAL;
gridBagConstraints33.gridy = 6;
gridBagConstraints33.weightx = 1.0;
gridBagConstraints33.ipadx = 50;
gridBagConstraints33.insets = new Insets (5, 5, 5, 5);
gridBagConstraints33.anchor = GridBagConstraints.WEST;
gridBagConstraints33.gridx = 6;
GridBagConstraints gridBagConstraints38 = new GridBagConstraints ();
gridBagConstraints38.fill = GridBagConstraints.VERTICAL;
gridBagConstraints38.gridy = 6;
gridBagConstraints38.weightx = 1.0;
gridBagConstraints38.ipadx = 50;
gridBagConstraints38.insets = new Insets (5, 5, 5, 5);
gridBagConstraints38.anchor = GridBagConstraints.WEST;
gridBagConstraints38.gridx = 4;
GridBagConstraints gridBagConstraints22 = new GridBagConstraints ();
gridBagConstraints22.fill = GridBagConstraints.VERTICAL;
gridBagConstraints22.gridy = 6;
gridBagConstraints22.weightx = 0.0;
gridBagConstraints22.ipadx = 50;
gridBagConstraints22.anchor = GridBagConstraints.WEST;
gridBagConstraints22.insets = new Insets (5, 5, 5, 5);
gridBagConstraints22.gridx = 1;
GridBagConstraints gridBagConstraints36 = new GridBagConstraints ();
gridBagConstraints36.insets = new Insets (0, 5, 0, 5);
gridBagConstraints36.gridy = 6;
gridBagConstraints36.ipadx = 0;
gridBagConstraints36.ipady = 0;
gridBagConstraints36.gridwidth = 1;
gridBagConstraints36.gridx = 5;
GridBagConstraints gridBagConstraints35 = new GridBagConstraints ();
gridBagConstraints35.insets = new Insets (0, 0, 0, 0);
gridBagConstraints35.gridy = 6;
gridBagConstraints35.ipadx = 0;
gridBagConstraints35.ipady = 0;
gridBagConstraints35.gridwidth = 1;
gridBagConstraints35.anchor = GridBagConstraints.WEST;
gridBagConstraints35.gridx = 3;
GridBagConstraints gridBagConstraints32 = new GridBagConstraints ();
gridBagConstraints32.insets = new Insets (15, 20, 5, 0);
gridBagConstraints32.gridx = 16;
gridBagConstraints32.gridy = 4;
gridBagConstraints32.ipadx = 172;
gridBagConstraints32.ipady = 125;
gridBagConstraints32.gridwidth = 0;
gridBagConstraints32.gridheight = 6;
GridBagConstraints gridBagConstraints31 = new GridBagConstraints ();
gridBagConstraints31.fill = GridBagConstraints.VERTICAL;
gridBagConstraints31.gridwidth = 9;
gridBagConstraints31.gridx = 1;
gridBagConstraints31.gridy = 0;
gridBagConstraints31.weightx = 0.0;
gridBagConstraints31.ipadx = 240;
gridBagConstraints31.anchor = GridBagConstraints.WEST;
gridBagConstraints31.insets = new Insets (20, 5, 5, 0);
GridBagConstraints gridBagConstraints30 = new GridBagConstraints ();
gridBagConstraints30.fill = GridBagConstraints.BOTH;
gridBagConstraints30.gridwidth = 17;
gridBagConstraints30.gridx = 1;
gridBagConstraints30.gridy = 10;
gridBagConstraints30.ipadx = 265;
gridBagConstraints30.ipady = 70;
gridBagConstraints30.weightx = 1.0;
gridBagConstraints30.weighty = 1.0;
gridBagConstraints30.gridheight = 4;
gridBagConstraints30.anchor = GridBagConstraints.WEST;
gridBagConstraints30.insets = new Insets (5, 5, 2, 5);
GridBagConstraints gridBagConstraints29 = new GridBagConstraints ();
gridBagConstraints29.fill = GridBagConstraints.VERTICAL;
gridBagConstraints29.gridwidth = 3;
gridBagConstraints29.gridx = 4;
gridBagConstraints29.gridy = 9;
gridBagConstraints29.weightx = 1.0;
gridBagConstraints29.ipadx = 18;
gridBagConstraints29.anchor = GridBagConstraints.WEST;
gridBagConstraints29.insets = new Insets (5, 5, 36, 2);
GridBagConstraints gridBagConstraints28 = new GridBagConstraints ();
gridBagConstraints28.insets = new Insets (3, 5, 34, 4);
gridBagConstraints28.gridy = 9;
gridBagConstraints28.gridx = 3;
GridBagConstraints gridBagConstraints26 = new GridBagConstraints ();
gridBagConstraints26.fill = GridBagConstraints.VERTICAL;
gridBagConstraints26.gridwidth = 3;
gridBagConstraints26.gridx = 4;
gridBagConstraints26.gridy = 8;
gridBagConstraints26.weightx = 1.0;
gridBagConstraints26.anchor = GridBagConstraints.WEST;
gridBagConstraints26.ipadx = 18;
gridBagConstraints26.insets = new Insets (6, 5, 4, 2);
GridBagConstraints gridBagConstraints25 = new GridBagConstraints ();
gridBagConstraints25.insets = new Insets (4, 5, 2, 4);
gridBagConstraints25.gridy = 8;
gridBagConstraints25.anchor = GridBagConstraints.WEST;
gridBagConstraints25.gridx = 3;
GridBagConstraints gridBagConstraints24 = new GridBagConstraints ();
gridBagConstraints24.fill = GridBagConstraints.VERTICAL;
gridBagConstraints24.gridx = 1;
gridBagConstraints24.gridy = 8;
gridBagConstraints24.weightx = 1.0;
gridBagConstraints24.ipadx = 18;
gridBagConstraints24.gridwidth = 3;
gridBagConstraints24.anchor = GridBagConstraints.WEST;
gridBagConstraints24.insets = new Insets (6, 5, 4, 1);
GridBagConstraints gridBagConstraints23 = new GridBagConstraints ();
gridBagConstraints23.fill = GridBagConstraints.VERTICAL;
gridBagConstraints23.gridwidth = 9;
gridBagConstraints23.gridx = 1;
gridBagConstraints23.gridy = 7;
gridBagConstraints23.weightx = 1.0;
gridBagConstraints23.ipadx = 120;
gridBagConstraints23.anchor = GridBagConstraints.WEST;
gridBagConstraints23.insets = new Insets (4, 5, 4, 17);
GridBagConstraints gridBagConstraints21 = new GridBagConstraints ();
gridBagConstraints21.fill = GridBagConstraints.VERTICAL;
gridBagConstraints21.gridwidth = 7;
gridBagConstraints21.gridx = 1;
gridBagConstraints21.gridy = 5;
gridBagConstraints21.ipadx = 70;
gridBagConstraints21.ipady = 0;
gridBagConstraints21.weightx = 1.0;
gridBagConstraints21.anchor = GridBagConstraints.WEST;
gridBagConstraints21.insets = new Insets (5, 5, 5, 6);
GridBagConstraints gridBagConstraints20 = new GridBagConstraints ();
gridBagConstraints20.fill = GridBagConstraints.VERTICAL;
gridBagConstraints20.gridwidth = 7;
gridBagConstraints20.gridx = 1;
gridBagConstraints20.gridy = 4;
gridBagConstraints20.weightx = 1.0;
gridBagConstraints20.ipadx = 60;
gridBagConstraints20.anchor = GridBagConstraints.WEST;
gridBagConstraints20.insets = new Insets (0, 5, 0, 0);
GridBagConstraints gridBagConstraints19 = new GridBagConstraints ();
gridBagConstraints19.fill = GridBagConstraints.VERTICAL;
gridBagConstraints19.gridwidth = 4;
gridBagConstraints19.gridx = 1;
gridBagConstraints19.gridy = 3;
gridBagConstraints19.weightx = 1.0;
gridBagConstraints19.ipadx = 20;
gridBagConstraints19.anchor = GridBagConstraints.WEST;
gridBagConstraints19.insets = new Insets (6, 5, 5, 18);
GridBagConstraints gridBagConstraints18 = new GridBagConstraints ();
gridBagConstraints18.fill = GridBagConstraints.VERTICAL;
gridBagConstraints18.gridwidth = 6;
gridBagConstraints18.gridx = 1;
gridBagConstraints18.gridy = 2;
gridBagConstraints18.weightx = 1.0;
gridBagConstraints18.ipadx = 100;
gridBagConstraints18.anchor = GridBagConstraints.WEST;
gridBagConstraints18.insets = new Insets (0, 5, 0, 0);
GridBagConstraints gridBagConstraints17 = new GridBagConstraints ();
gridBagConstraints17.fill = GridBagConstraints.VERTICAL;
gridBagConstraints17.gridwidth = 9;
gridBagConstraints17.gridx = 2;
gridBagConstraints17.gridy = 1;
gridBagConstraints17.weightx = 1.0;
gridBagConstraints17.ipadx = 200;
gridBagConstraints17.anchor = GridBagConstraints.WEST;
gridBagConstraints17.insets = new Insets (5, 5, 5, 0);
GridBagConstraints gridBagConstraints16 = new GridBagConstraints ();
gridBagConstraints16.fill = GridBagConstraints.VERTICAL;
gridBagConstraints16.gridwidth = 3;
gridBagConstraints16.gridx = 1;
gridBagConstraints16.gridy = 1;
gridBagConstraints16.weightx = 1.0;
gridBagConstraints16.anchor = GridBagConstraints.WEST;
gridBagConstraints16.ipadx = 0;
gridBagConstraints16.insets = new Insets (5, 5, 5, 0);
GridBagConstraints gridBagConstraints15 = new GridBagConstraints ();
gridBagConstraints15.insets = new Insets (5, 15, 5, 0);
gridBagConstraints15.gridy = 10;
gridBagConstraints15.gridwidth = 1;
gridBagConstraints15.gridheight = 0;
gridBagConstraints15.gridx = 0;
GridBagConstraints gridBagConstraints14 = new GridBagConstraints ();
gridBagConstraints14.insets = new Insets (5, 15, 36, 0);
gridBagConstraints14.gridy = 9;
gridBagConstraints14.gridx = 0;
GridBagConstraints gridBagConstraints13 = new GridBagConstraints ();
gridBagConstraints13.insets = new Insets (5, 15, 5, 0);
gridBagConstraints13.gridy = 8;
gridBagConstraints13.gridx = 0;
GridBagConstraints gridBagConstraints12 = new GridBagConstraints ();
gridBagConstraints12.insets = new Insets (5, 15, 5, 0);
gridBagConstraints12.gridy = 7;
gridBagConstraints12.gridx = 0;
GridBagConstraints gridBagConstraints11 = new GridBagConstraints ();
gridBagConstraints11.insets = new Insets (5, 15, 5, 0);
gridBagConstraints11.gridy = 6;
gridBagConstraints11.gridx = 0;
GridBagConstraints gridBagConstraints10 = new GridBagConstraints ();
gridBagConstraints10.insets = new Insets (5, 15, 5, 0);
gridBagConstraints10.gridy = 5;
gridBagConstraints10.gridx = 0;
GridBagConstraints gridBagConstraints9 = new GridBagConstraints ();
gridBagConstraints9.insets = new Insets (5, 15, 5, 0);
gridBagConstraints9.gridy = 4;
gridBagConstraints9.gridx = 0;
GridBagConstraints gridBagConstraints8 = new GridBagConstraints ();
gridBagConstraints8.insets = new Insets (5, 15, 5, 0);
gridBagConstraints8.gridy = 3;
gridBagConstraints8.gridx = 0;
GridBagConstraints gridBagConstraints7 = new GridBagConstraints ();
gridBagConstraints7.insets = new Insets (5, 15, 5, 0);
gridBagConstraints7.gridy = 2;
gridBagConstraints7.gridx = 0;
GridBagConstraints gridBagConstraints6 = new GridBagConstraints ();
gridBagConstraints6.insets = new Insets (5, 15, 5, 0);
gridBagConstraints6.gridy = 1;
gridBagConstraints6.gridx = 0;
GridBagConstraints gridBagConstraints5 = new GridBagConstraints ();
gridBagConstraints5.insets = new Insets (20, 15, 5, 0);
gridBagConstraints5.gridy = 0;
gridBagConstraints5.gridwidth = 1;
gridBagConstraints5.gridx = 0;
jLabel41 = new JLabel ();
jLabel41.setFont (new Font ( "Dialog", Font.BOLD, 14));
jLabel41.setText ( "y");
jLabel4 = new JLabel ();
jLabel4.setFont (new Font ( "Dialog", Font.BOLD, 14));
jLabel4.setText ( "x");
jLabel3 = new JLabel ();
jLabel3.setFont (new Font ( "Dialog", Font.BOLD, 18));
jLabel3.setText (":");

jLabel1 = new JLabel ();
jLabel1.setFont (new Font ( "Dialog", Font.BOLD, 18));
jLabel1.setText (":");
JButton = new JLabel ();
jLabel.setText ( "Description");
txtOraCh = new JLabel ();
txtOraCh.setText ( "Closing Time");
txtOraAp = new JLabel ();
txtOraAp.setText ( "Opening Hours");
txtTel = new JLabel ();
txtTel.setText ( "Phone");
txtPos = new JLabel ();
txtPos.setText ( "Geographic Position");
txtProvincia = new JLabel ();
txtProvincia.setText ( "Province");
txtLocalità = new JLabel ();
txtLocalità.setText ( "Location");
txtCitta = new JLabel ();
txtCitta.setText ( "City");
txtCAP = new JLabel ();
txtCAP.setText ( "CAP");
txtIndirizzo = new JLabel ();
txtIndirizzo.setText ( "Address");
txtNome = new JLabel ();
txtNome.setText ( "Name Refreshment");
datiPR = new JPanel ();
datiPR.setLayout (new GridBagLayout ());
datiPR.setBorder (new SoftBevelBorder (SoftBevelBorder.LOWERED));
datiPR.add (txtNome, gridBagConstraints5);
datiPR.add (txtIndirizzo, gridBagConstraints6);
datiPR.add (txtCitta, gridBagConstraints7);
datiPR.add (txtLocalità, gridBagConstraints8);
datiPR.add (txtCAP, gridBagConstraints9);
datiPR.add (txtProvincia, gridBagConstraints10);
datiPR.add (txtPos, gridBagConstraints11);
datiPR.add (txtTel, gridBagConstraints12);
datiPR.add (txtOraAp, gridBagConstraints13);
datiPR.add (txtOraCh, gridBagConstraints14);
datiPR.add (JLabel, gridBagConstraints15);
datiPR.add (getIndirizzoPR1 (), gridBagConstraints16);
datiPR.add (getIndirizzoPR (), gridBagConstraints17);
datiPR.add (getCittaPR (), gridBagConstraints18);
datiPR.add (getLocalitaPR (), gridBagConstraints19);
datiPR.add (getCapPR (), gridBagConstraints20);
datiPR.add (getProvPR (), gridBagConstraints21);
datiPR.add (getTelefonoPR (), gridBagConstraints23);
datiPR.add (getOrarioAPOrePR (), gridBagConstraints24);
datiPR.add (jLabel1, gridBagConstraints25);
datiPR.add (getOrarioApMinPR (), gridBagConstraints26);
datiPR.add (jLabel3, gridBagConstraints28);
datiPR.add (getOrarioCHMinPR (), gridBagConstraints29);
datiPR.add (getJScrollPane (), gridBagConstraints30);
datiPR.add (getNomePR (), gridBagConstraints31);
datiPR.add (getJPanel (), gridBagConstraints32);
datiPR.add (jLabel4, gridBagConstraints35);
datiPR.add (jLabel41, gridBagConstraints36);
datiPR.add (getPosGeoX (), gridBagConstraints22);
datiPR.add (getPosGeoY (), gridBagConstraints38);
datiPR.add (getPosGeoZ (), gridBagConstraints33);
datiPR.add (jLabel2, gridBagConstraints34);
datiPR.add (getOrarioCHOrePR (), gridBagConstraints27);
)
datiPR return;
)

/ **
* Method for iniziailizzare a panel (statistics)
*
* @ Return javax.swing.JPanel
* /
getStatistiche private JPanel () (
if (statistics == null) (
GridBagConstraints gridBagConstraints4 = new GridBagConstraints ();
gridBagConstraints4.gridx = 0;
gridBagConstraints4.gridwidth = 0;
gridBagConstraints4.fill = GridBagConstraints.HORIZONTAL;
gridBagConstraints4.insets = new Insets (20, 0, 0, 0);
gridBagConstraints4.gridy = 2;
GridBagConstraints gridBagConstraints3 = new GridBagConstraints ();
gridBagConstraints3.gridx = 0;
gridBagConstraints3.gridwidth = 2;
gridBagConstraints3.fill = GridBagConstraints.HORIZONTAL;
gridBagConstraints3.insets = new Insets (0, 0, 20, 0);
gridBagConstraints3.gridy = 1;
GridBagConstraints gridBagConstraints2 = new GridBagConstraints ();
gridBagConstraints2.gridx = 1;
gridBagConstraints2.insets = new Insets (0, 30, 30, 0);
gridBagConstraints2.anchor = GridBagConstraints.WEST;
gridBagConstraints2.gridy = 0;
mediaVotoPR = new JLabel ();
mediaVotoPR.setText ( "JLabel");
GridBagConstraints gridBagConstraints1 = new GridBagConstraints ();
gridBagConstraints1.gridx = 0;
gridBagConstraints1.insets = new Insets (0, 0, 30, 0);
gridBagConstraints1.gridy = 0;
txtNomeBene = new JLabel ();
txtNomeBene.setText ( "Well name> Culturale>");
txtNomeBene.setFont (new Font ( "Dialog", Font.BOLD, 18));
statistics = new JPanel ();
statistiche.setLayout (new GridBagLayout ());
statistiche.add (txtNomeBene, gridBagConstraints1);
statistiche.add (mediaVotoPR, gridBagConstraints2);
statistiche.add (getStatisticheMeseCorrente (), gridBagConstraints3);
statistiche.add (getStatisticheTotali (), gridBagConstraints4);
)
return statistics;
)

/ **
* Method to initialize a panel (feedback)
*
* @ Return javax.swing.JPanel
* /
getFeedback private JPanel () (
if (feedback == null) (
GridBagConstraints = GridBagConstraints new GridBagConstraints ();
gridBagConstraints.fill = GridBagConstraints.BOTH;
gridBagConstraints.gridy = 0;
gridBagConstraints.weightx = 1.0;
gridBagConstraints.weighty = 1.0;
gridBagConstraints.gridx = 0;
feedback = new JPanel ();
feedback.setLayout (new GridBagLayout ());
feedback.add (getJScrollPane2 (), GridBagConstraints);
)
return feedback;
)

/ **
* Initialize a JexField (indirizzoPR)
*
* @ Return javax.swing.JTextField
* /
private JTextField getIndirizzoPR () (
if (indirizzoPR == null) (
indirizzoPR = new JTextField ();
indirizzoPR.setColumns (12);
indirizzoPR.addActionListener (campoCompilato);
)
indirizzoPR return;
)

/ **
* Method to initialize the type field address (indirizzoPR)
* Or via, piazza ....
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getIndirizzoPR1 () (
if (indirizzoPR1 == null) (
indirizzoPR1 = new JComboBox ();
indirizzoPR1.setPreferredSize (new Dimension (60, 20));
indirizzoPR1.setMinimumSize (new Dimension (60, 25));
indirizzoPR1.setCursor (new Cursor (Cursor.DEFAULT_CURSOR));
indirizzoPR1.addItem ( "Via");
indirizzoPR1.addItem (P.zza ");
indirizzoPR1.addItem ( "V.le");
indirizzoPR1.addItem (V.co ");
indirizzoPR1.addItem ( "Largo");
indirizzoPR1.addItem ( "Course");

)
indirizzoPR1 return;
)

/ **
* Initialize a JTextField for entering
* Uan city CittaPR
*
* @ Return javax.swing.JTextField
* /
private JTextField getCittaPR () (
if (cittaPR == null) (
cittaPR = new JTextField ();
cittaPR.setColumns (12);
cittaPR.addActionListener (campoCompilato);
)
cittaPR return;
)

/ **
* This method initializes localitaPR
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getLocalitaPR () (
if (localitaPR == null) (
localitaPR = new JComboBox ();
localitaPR.setMinimumSize (new Dimension (80, 25));
localitaPR.setPreferredSize (new Dimension (80, 20));
localitaPR.addActionListener (campoCompilato);
)
localitaPR return;
)

/ **
* Code of refreshment. Definition capPR JTextField
*
* @ Return javax.swing.JTextField
* /
private JTextField getCapPR () (
if (capPR == null) (
capPR = new JTextField ();
capPR.setColumns (8);
capPR.addActionListener (campoCompilato);
)
capPR return;
)

/ **
* Creation JScrollPane
*
* @ Return javax.swing.JScrollPane
* /
private JScrollPane getJScrollPane () (
if (JScrollPane == null) (
JScrollPane = new JScrollPane ();
jScrollPane.setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
jScrollPane.setViewportView (getDescrizionePR ());
)
JScrollPane return;
)

/ **
* Method to create JTextArea's whole descrizionePR
*
* @ Return javax.swing.JTextArea
* /
getDescrizionePR private JTextArea () (
if (descrizionePR == null) (
descrizionePR = new JTextArea ();
descrizionePR.setColumns (12);
descrizionePR.setCursor (new Cursor (Cursor.TEXT_CURSOR));

)
descrizionePR return;
)

/ **
* Method to create the JTextField telefonoPR
*
* @ Return javax.swing.JTextField
* /
private JTextField getTelefonoPR () (
if (telefonoPR == null) (
telefonoPR = new JTextField ();
telefonoPR.setColumns (12);
telefonoPR.addActionListener (campoCompilato);
)
telefonoPR return;
)

/ **
* method to initialize a JComboBox with the hours (orarioAPOrePR)
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getOrarioAPOrePR () (
if (orarioAPOrePR == null) (
orarioAPOrePR = new JComboBox ();
orarioAPOrePR.setPreferredSize (new Dimension (40, 20));
for (int i = 0; i <24; i + +) (
if (i <10)
orarioAPOrePR.addItem ( "0" + i);
else
orarioAPOrePR.addItem (i);

)
orarioAPOrePR.addActionListener (campoCompilato);
)
orarioAPOrePR return;
)

/ **
* Method to initialize a JComboBox with the minutes (orarioApMinPR)
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getOrarioApMinPR () (
if (orarioApMinPR == null) (
orarioApMinPR = new JComboBox ();
orarioApMinPR.setLightWeightPopupEnabled (true);
orarioApMinPR.setPreferredSize (new Dimension (40, 20));
orarioApMinPR.addItem ( "00");
orarioApMinPR.addItem ( "15");
orarioApMinPR.addItem ( "30");
orarioApMinPR.addItem ( "45");
orarioApMinPR.addActionListener (campoCompilato);
)
orarioApMinPR return;
)



/ **
* Method to initialize a JComboBox with the minutes (orarioCHMinPR)
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getOrarioCHMinPR () (
if (orarioCHMinPR == null) (
orarioCHMinPR = new JComboBox ();
orarioCHMinPR.setPreferredSize (new Dimension (40, 20));
orarioCHMinPR.addItem ( "00");
orarioCHMinPR.addItem ( "15");
orarioCHMinPR.addItem ( "30");
orarioCHMinPR.addItem ( "45");
orarioCHMinPR.addActionListener (campoCompilato);
)
orarioCHMinPR return;

)

/ **
* Create and initialize a jCombo Box with all the provinces (provPR)
*
* @ Return javax.swing.JTextField
* /
private JComboBox getProvPR () (
if (provPR == null) (
final String [] Province = ( "AG", "AL", "an", "AO", "AQ", "AR", "AP", "AT", "AV", "BA", "BL" , "BN", "BG", "BI", "BO", "BR", "BS", "BZ",
"CA", "CB", "CE", "CH", "CI", "CL", "CN", "CO", "CR", "CS", "KR", "en", "FC "," FE "," FI "," FG "," FR "," GE "," GO "," GR "," IM "," IS "," LC ",
"LE", "LI", "LO", "LT", "LU", "MC", "ME", "MF", "MN", "MO", "MS", "MT", "NA "," NO "," NU "," OG "," OR "," OT "," PA "," PC "," PD "," PE "," PG "," PO "," PR ", "PU", "R", "RA", "RC", "RE", "RG",
"RI", "RM", "RN", "RO", "SA", "YES", "SO", "SP", "SS", "SV", "TA", "TE", "TN "," TP "," TR "," TS "," TV "," UD "," VA "," VB "," VC "," VE "," VI ",
"VR", "VS", "VT", "VV");
provPR = new JComboBox ();
for (int i = 0; i <province.length i + +) (
provPR.addItem (provinces [i]);
)
provPR.addActionListener (campoCompilato);
)
provPR return;
)

DocumentoNumerico PlainDocument class extends (


private int limit;


public DocumentoNumerico (int limit) (

this.limit = limit;

)

/ **
* Initialization and management position
*
* @ Param integer pOffset
* @ Param String pString
* @ Param Attribute Pattra
*
* /
public void insertString (int pOffset, String pStr, AttributeSet Pattra) throws BadLocationException (
if (pStr == null)
return;

if ((getLength () + pStr.length ()) <= limit) (
super.insertString (pOffset, pStr, Pattra);
)
)
)


/ **
* Initialization of a data point of the snack (nomePR)
*
* @ Return javax.swing.JTextField
* /
private JTextField getNomePR () (
if (nomePR == null) (
nomePR = new JTextField ();
nomePR.setColumns (12);
nomePR.setPreferredSize (new Dimension (180, 20));
nomePR.addActionListener (campoCompilato);
nomePR.addFocusListener (validating);
nomePR.setDocument (new DocumentoNumerico (20));

)
nomePR return;
)

/ **
* Initialize and create a panel (JPanel)
*
* @ Return javax.swing.JPanel
* /
getJPanel private JPanel () (
if (JPanel == null) (
JPanel = new JPanel ();
jPanel.setLayout (new BorderLayout ());
jPanel.setBorder (BorderFactory.createTitledBorder (
BorderFactory.createEmptyBorder (),
"Tag the 'Search TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog", Font.BOLD, 12),
Color.black));
BeanTag [] test = new BeanTag [8];
test [0] = new BeanTag (0, "castle", "really a castle");
test [1] = new BeanTag (1, "stronghold", "really a hostel");
test [2] = new BeanTag (3, "statue", "really a basket");
test [3] = new BeanTag (4, "Column", "really a basket");
test [4] = new BeanTag (5, "internal", "really a basket");
test [5] = new BeanTag (6, "external", "really a basket");
test [6] = new BeanTag (7, "eight hundred", "really a basket");
test [7] = new BeanTag (8, "Novecento", "really a basket");
pannelloTag = new TagPanel (test);
jPanel.add (pannelloTag, BorderLayout.CENTER);
)
JPanel return;
)

/ **
* Creating a JScrollPane (jScrollPane2)
*
* @ Return javax.swing.JScrollPane
* /
private JScrollPane getJScrollPane2 () (
if (jScrollPane2 == null) (
jScrollPane2 = new JScrollPane ();
jScrollPane2.setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
jScrollPane2.setViewportView (getFeedbackTable ());
)
jScrollPane2 return;
)

/ **
* Create a JTable (feedbackTable)
*
* @ Return javax.swing.JTable
* /
private JTable getFeedbackTable () (
if (feedbackTable == null) (
feedbackTable = new JTable ();
)
feedbackTable return;
)

/ **
* Creation of a panel (statisticheMeseCorrente)
*
* @ Return javax.swing.JPanel
* /
getStatisticheMeseCorrente private JPanel () (
if (statisticheMeseCorrente == null) (
statisticheMeseCorrente = new JPanel ();
statisticheMeseCorrente.setLayout (new GridBagLayout ());
statisticheMeseCorrente.setPreferredSize (new Dimension (500, 120));
)
statisticheMeseCorrente return;
)

/ **
* Creation of a panel (statisticheTotali)
*
* @ Return javax.swing.JPanel
* /
getStatisticheTotali private JPanel () (
if (statisticheTotali == null) (
statisticheTotali = new JPanel ();
statisticheTotali.setLayout (new GridBagLayout ());
statisticheTotali.setPreferredSize (new Dimension (500, 120));
)
statisticheTotali return;
)
/ **
* Method for creating a toolbar
* (ToolbarSchedaPR)
*
* @ Return javax.swing.JToolBar
* /
Private JToolBar getToolbarSchedaPR () (
if (ToolbarSchedaPR == null) (

ToolbarSchedaPR JToolBar = new ();
ToolbarSchedaPR.setFloatable (false);
ToolbarSchedaPR.add (getBtnModifica ());
ToolbarSchedaPR.addSeparator ();
ToolbarSchedaPR.add (getBtnSalva ());
ToolbarSchedaPR.addSeparator ();
ToolbarSchedaPR.add (getBtnAnnulla ());
ToolbarSchedaPR.addSeparator ();
ToolbarSchedaPR.add (getBtnModificaCommento ());
ToolbarSchedaPR.addSeparator ();
)
ToolbarSchedaPR return;
)
/ **
* Method to initialize posGeoX
* The X position of the GPS
*
* @ Return javax.swing.JTextField
* /
private JTextField getPosGeoX () (
if (posGeoX == null) (
posGeoX = new JTextField ();
)
posGeoX return;
)
/ **
* Method to initialize posGeoY
* The Y position of the GPS
*
* @ Return javax.swing.JTextField
* /
private JTextField getPosGeoY () (
if (posGeoY == null) (
posGeoY = new JTextField ();
)
posGeoY return;
)
/ **
* Method to initialize posGeoZ
* The Z position of the GPS
*
* @ Return javax.swing.JTextField
* /
private JTextField getPosGeoZ () (
if (posGeoZ == null) (
posGeoZ = new JTextField ();
)
posGeoZ return;
)
/ **
* Method to initialize a JComboBox with the hours (orarioCHOrePR)
*
* @ Return javax.swing.JComboBox
* /
private JComboBox getOrarioCHOrePR () (
if (orarioCHOrePR == null) (
orarioCHOrePR = new JComboBox ();
orarioCHOrePR.setPreferredSize (new Dimension (40, 20));
)
orarioCHOrePR return;
)

)
