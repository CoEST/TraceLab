package unisa.gps.etour.gui.operatoreagenzia;

import java.awt .*;
import java.awt.event .*;
import java.io.FileNotFoundException;
import java.rmi.RemoteException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.util.ArrayList;
import java.util.Date;
import javax.swing .*;
import java.util .*;
import javax.swing.text.BadLocationException;
import javax.swing.text.Document;
import javax.swing.border .*;
import unisa.gps.etour.bean.BeanNews;
import unisa.gps.etour.control.GestioneAdvertisement.IGestioneAdvertisementAgenzia;
import unisa.gps.etour.gui.DeskManager;
import unisa.gps.etour.gui.HelpManager;
import unisa.gps.etour.gui.operatoreagenzia.document.LimitedDocument;
import unisa.gps.etour.gui.operatoreagenzia.tables.NewsTableModel;
import unisa.gps.etour.gui.operatoreagenzia.tables.PrioritaRenderer;
import unisa.gps.etour.gui.operatoreagenzia.tables.ScrollableTable;
import unisa.gps.etour.gui.operatoreagenzia.tables.TestoNewsRenderer;
import unisa.gps.etour.util.Data;

/ **
  * This class implements the interface for collecting news for the actor
  * Operator Agency.
  *
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class News extends JInternalFrame
(
private JPanel jContentPane = null;
Private JToolBar NewsToolbar = null;
private JButton btnEliminaN = null;
private JPanel rightPanel = null;
private JPanel formNews = null;
Private JSlider prSlider = null;
private JButton btnInsertModify = null;
private JButton btnReset = null;
private JPanel panelHelp = null;
private JTextPane textGuida = null;
private JScrollPane scrollTableNews = null;
private JTable tableNews = null;
private JTextArea testoNews = null;
private JComboBox durataNews = null;
private JButton btnModificaN = null;
private JLabel labelCaratteri;
private int idNews = -1;
private NewsTableModel TableModel;
Private HelpManager newsHelp;
protected DeskManager desktopManager;
protected JDesktopPane JDesktopPane;
Private IGestioneAdvertisementAgenzia gestioneNews;

/ **
* This is the default constructor.
* /
public News ()
(
super ( "News");
setPreferredSize (Home.CHILD_SIZE);
setMinimumSize (new Dimension (600, 480));
setResizable (true);
setFrameIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "News32.png ")));
setIconifiable (true);
setMaximizable (true);
setClosable (true);

/ / Setting up dell'help manager to manage the news.
textGuida = new JTextPane ();
TRY
(
newsHelp = new HelpManager (Home.URL_HELP + "news.txt" textGuida);
)
catch (FileNotFoundException e)
(
textGuida
. setText ( "<html> <b> Help not available </ b> </ html>");
)

setContentPane (getJContentPane ());

addInternalFrameListener (new InternalFrameAdapter ()
(

/ *
* Inclusion of the frame on the desktop desktop retrieves bread bread
* And desktop manager and initializes the remote objects
* Management of cultural heritage.
* /

public void internalFrameOpened (InternalFrameEvent pEvent)
(
PEvent.getInternalFrame JInternalFrame frame = ();
JDesktopPane frame.getDesktopPane = ();
desktopManager = (DeskManager) jDesktopPane.getDesktopManager ();

/ / Setting up objects for remote asset management
/ / Cultural.
TRY
(
Registry reg = LocateRegistry.getRegistry (Home.HOST);
gestioneNews = (IGestioneAdvertisementAgenzia) reg
. lookup ( "GestioneAdvertisementAgenzia");

/ / Load data.
caricaTabella ();
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
+ "<h3> <u> The dialog management request is closed. </ U> </ h3>"
+ "<p> <b> Possible Causes: </ b>"
+ "<ul> <li> No connection to the network. </ Li>"
+ "Server <li> inactive. </ Li>"
+ "Server <li> clogged. </ Li> </ ul>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass (). GetResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showMessageDialog (JDesktopPane, error,
"Error!" JOptionPane.ERROR_MESSAGE, err);
frame.dispose ();
)
)
));
)

/ **
* Initialize the content pane of the frame inside.
*
* @ Return javax.swing.JPanel - the content pane.
* /
private JPanel getJContentPane ()
(
if (null == jContentPane)
(
jContentPane = new JPanel ();
jContentPane.setLayout (new BorderLayout ());
jContentPane.add (getNewsToolbar (), BorderLayout.NORTH);
jContentPane.add (getRightPanel (), BorderLayout.EAST);
jContentPane.add (getTableNews (), BorderLayout.CENTER);
)
jContentPane return;
)

/ **
* This method initializes the toolbar to manage the news.
*
* @ Return javax.swing.JToolBar - the toolbar management news.
* /
Private JToolBar getNewsToolbar ()
(
if (null == NewsToolbar)
(
NewsToolbar JToolBar = new ();
NewsToolbar.setFloatable (false);
NewsToolbar.add (getBtnModificaN ());
NewsToolbar.addSeparator ();
NewsToolbar.add (getBtnEliminaN ());
)
NewsToolbar return;
)

/ **
* This method initializes the button to edit a news.
*
* @ Return javax.swing.JButton - button to change.
* /
private JButton getBtnModificaN ()
(
if (null == btnModificaN)
(
btnModificaN = new JButton ();
btnModificaN.setText ( "Edit News");
btnModificaN.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "ModificaNews32.png ")));
btnModificaN.setCursor (Cursor
. getPredefinedCursor (Cursor.HAND_CURSOR));
btnModificaN.setName (btnModifica ");
btnModificaN.addMouseListener (newsHelp);
btnModificaN.setEnabled (false);
btnModificaN.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent pActionEvent)
(
int selectedRow = tableNews.getSelectedRow ();
if (idNews == -1) / / In this way I know if she was
/ / Edit
(
btnInsertModify.setText ( "Change");
btnInsertModify.setIcon (new ImageIcon (getClass ()
. getResource (Home.URL_IMAGES + "Salva16.png ")));
btnReset.setText ( "Cancel");
btnReset.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "Annulla16.png ")));
formNews.setBorder (BorderFactory.createTitledBorder (
BorderFactory.createLineBorder (new Color (51,
102, 255), 3), "Edit News"
TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font (
"Dialog", Font.BOLD, 12), new Color (0,
102, 204)));
)
String text = (String) tableModel.getValueAt (selectedRow,
0);
numCaratteri int = 200 - testo.length () + 1;
labelCaratteri.setText ( "# Characters:" + numCaratteri);
Document testoNews.getDocument doctest = ();
TRY
(
docTesto.remove (0, docTesto.getLength ());
docTesto.insertString (0, text, null);
)
catch (BadLocationException s)
(
e.printStackTrace ();
)
Expiry date = (Date) TableModel
. getValueAt (selectedRow, 3);
durataNews.setSelectedIndex (Data.getNumDays (expires));
idNews = tableModel.getID (selectedRow);
prSlider.setValue ((Integer) tableModel.getValueAt (
selectedRow, 1));
)

));
)
btnModificaN return;
)

/ **
* This method initializes the button to delete a news.
*
* @ Return javax.swing.JButton - the button for deletion.
* /
private JButton getBtnEliminaN ()
(
if (null == btnEliminaN)
(
btnEliminaN = new JButton ();
btnEliminaN.setText ( "Delete News");
btnEliminaN.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "EliminaNews32.png ")));
btnEliminaN.setEnabled (false);
btnEliminaN.setCursor (Cursor
. getPredefinedCursor (Cursor.HAND_CURSOR));
btnEliminaN.setName (btnElimina ");
btnEliminaN.addMouseListener (newsHelp);
btnEliminaN.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent pEvent)
(
int selectedRow = tableNews.getSelectedRow ();

/ / Construction of the dialog for confirmation
/ / Elimination
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Are you sure you want to delete the selected news?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"The deleted data can not be filled again."
SwingConstants.CENTER);
avviso.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Delete", "Cancel");

/ / The dialog screen appears
int choice = JOptionPane.showInternalOptionDialog (
jContentPane, root, "Confirm Delete News"
JOptionPane.YES_NO_OPTION,
JOptionPane.QUESTION_MESSAGE, new ImageIcon (
getClass (). getResource (
Home.URL_IMAGES
+ "EliminaNews48.png")),
options, options [1]);

/ / If you chose to confirm the deletion
if (choice == JOptionPane.YES_OPTION)
(
TRY
(
gestioneNews.cancellaNews (TableModel
. getID (selectedRow));
tableModel.removeNews (selectedRow);
JOptionPane
. showInternalMessageDialog (
jContentPane,
"The news has been selected successfully deleted"
"News out!"
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES
+ "Ok48.png ")));
azzeraForm ();
)
catch (Exception ex)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "<h3> <u> Delete operation request can not be completed. </ U> </ h3>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass ()
. getResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showMessageDialog (JDesktopPane, error,
"Error!" JOptionPane.ERROR_MESSAGE, err);
)
)
)
));
)
btnEliminaN return;
)

/ **
* This method initializes the panel that realizes the interface side
* Right of news management.
*
* @ Return javax.swing.JPanel - the right panel.
* /
private JPanel getRightPanel ()
(
if (null == rightPanel)
(
rightPanel = new JPanel ();
rightPanel.setLayout (new GridBagLayout ());
GridBagConstraints g = new GridBagConstraints ();
g.fill = GridBagConstraints.BOTH;
g.gridx = 0;
g.gridy = 0;
g.weighty = 0.7;
rightPanel.add (getFormNews (), g);
g.weighty = 0.3;
g.gridy = 1;
rightPanel.add (getPanelHelp (), g);

)
rightPanel return;
)

/ **
* This method initializes the form for entering and editing a
* News.
*
* @ Return javax.swing.JPanel - the format
* /
private JPanel getFormNews ()
(
if (null == formNews)
(
formNews = new JPanel (new GridBagLayout ());
formNews.setBorder (BorderFactory.createTitledBorder (BorderFactory
. createLineBorder (new Color (51, 102, 255), 3),
"New News", TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION, new Font ( "Dialog",
Font.BOLD, 12), new Color (0, 102, 204)));
GridBagConstraints g = new GridBagConstraints ();
g.anchor = GridBagConstraints.CENTER;
g.gridx = 0;
g.gridy = 0;
g.weighty = 0.1;
g.gridwidth = 1;
g.gridheight = 1;
g.insets = new Insets (5, 5, 5, 5);
formNews.add (new JLabel ( "Text of the news:"), g);
labelCaratteri = new JLabel ( "# Characters: 200");
g.gridx = 1;
formNews.add (labelCaratteri, g);
g.gridx = 0;
g.gridwidth = 2;
g.gridy = 1;
g.weighty = 0.3;
g.fill = GridBagConstraints.VERTICAL;
JScrollPane scrollTesto = new JScrollPane (getTestoNews ());
scrollTesto
. setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
formNews.add (scrollTesto, g);
g.weighty = 0.1;
g.gridy = 2;
g.fill = GridBagConstraints.NONE;
formNews.add (new JLabel ( "Priority:"), g);
g.gridy = 3;
formNews.add (getPrSlider (), g);
New JPanel = new JPanel (new FlowLayout ());
nuovo.add (new JLabel ( "Length of news :"));
durataNews = new JComboBox ();
for (int i = 0; i <= 60; i + +)
(
durataNews.addItem ( "" + i);
)
durataNews.setSelectedIndex (0);
durataNews.setEditable (false);
durataNews.setName ( "duration");
durataNews.addMouseListener (newsHelp);
nuovo.add (durataNews);
nuovo.add (new JLabel ( "days"));
g.gridy = 4;
g.fill = GridBagConstraints.HORIZONTAL;
formNews.add (new, g);
g.gridwidth = 1;
g.gridy = 5;
formNews.add (getBtnInsertModify (), g);
g.gridx = 1;
formNews.add (getBtnReset (), g);
)
formNews return;
)

/ **
* This method initializes the slider to set the priority of a
* News.
*
* @ Return javax.swing.JSlider - the slider with ticks from 1 to 5.
* /
Private JSlider getPrSlider ()
(
if (null == prSlider)
(
prSlider = new JSlider (JSlider.HORIZONTAL, 5, 1);
prSlider.setMinimum (1);
prSlider.setMaximum (5);
prSlider.setMajorTickSpacing (1);
prSlider.setLabelTable (prSlider.createStandardLabels (1));
prSlider.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
prSlider.setName ( "priority");
prSlider.addMouseListener (newsHelp);
prSlider.setPaintLabels (true);
prSlider.setPaintTicks (true);
prSlider.setPaintTicks (true);
prSlider.setSnapToTicks (true);
)
prSlider return;
)

/ **
* This method initializes the radio button for submission of the form
* Insert / edit.
*
* @ Return javax.swing.JButton - the button of submission of the form.
* /
private JButton getBtnInsertModify ()
(
if (null == btnInsertModify)
(
btnInsertModify = new JButton ();
btnInsertModify.setText ( "Insert");
btnInsertModify.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "nuovaNews16.png ")));
btnInsertModify.setCursor (Cursor
. getPredefinedCursor (Cursor.HAND_CURSOR));
btnInsertModify.setName (btnInserisci ");
btnInsertModify.addMouseListener (newsHelp);
btnInsertModify.setFont (new Font ( "Dialog", Font.BOLD, 12));
btnInsertModify.setHorizontalTextPosition (SwingConstants.TRAILING);
btnInsertModify.addActionListener (new ActionListener ()
(

public void actionPerformed (ActionEvent pEvent)
(
if (testoNews.getText (). length () == 0)
(
JOptionPane.showInternalMessageDialog (jContentPane,
"The text of a news can not be empty!"
"Error New News", JOptionPane.ERROR_MESSAGE,
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "error32.png ")));
return;
)
if (idNews! = -1) / / We're making a change
(
/ / Construction of the dialog for confirmation of
/ / Edit
Root = new JPanel JPanel (new BorderLayout ());
JLabel message = new JLabel (
"Changing the selected news with"
+ "New data?");
message.setFont (new Font ( "Dialog", Font.BOLD, 14));
JLabel alert = new JLabel (
"The previous data can not be more recovered."
SwingConstants.CENTER);
avviso.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "warning16.png ")));
root.add (message, BorderLayout.NORTH);
root.add (notice, BorderLayout.CENTER);
String [] options = ( "Edit", "Cancel");
/ / The dialog screen appears
int choice = JOptionPane
. showInternalOptionDialog (
jContentPane,
root
"Confirmation Change News"
JOptionPane.YES_NO_OPTION,
JOptionPane.QUESTION_MESSAGE,
new ImageIcon (getClass (). getResource (
Home.URL_IMAGES
+ "ModificaNews48.png")),
options, options [1]);
/ / If you chose to confirm the change
if (choice == JOptionPane.YES_OPTION)
(
TRY
(
Date expires = new Date ();
scadenza.setDate (scadenza.getDate ()
DurataNews.getSelectedIndex + ());
BeanNews new = new BeanNews (testoNews
. getText (), new Date (), expiration
prSlider.getValue (), idNews);
gestioneNews.modificaNews (new);
tableModel.updateNews (new);
JOptionPane
. showInternalMessageDialog (
jContentPane,
"The news has been changed successfully selected."
"News changed!"
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES
+ "Ok48.png ")));
)
catch (Exception ex)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "<h3> <u> Change operation request can not be completed. </ U> </ h3>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES
+ "Error48.png"));
JOptionPane.showMessageDialog (JDesktopPane,
error, "Error!"
JOptionPane.ERROR_MESSAGE, err);
)
)
)
else
/ / We are posting
(
Date expires = new Date ();
scadenza.setDate (scadenza.getDate ()
DurataNews.getSelectedIndex + ());
BeanNews new = new BeanNews (testoNews.getText (),
new Date (), maturity, prSlider.getValue (), 33);
TRY
(
boolean ok = gestioneNews.inserisciNews (new);
if (ok)
(
caricaTabella ();
tableModel.insertNews (new);
JOptionPane
. showInternalMessageDialog (
jContentPane,
"The news is selected correctly inserted into the system."
"New news!"
JOptionPane.OK_OPTION,
new ImageIcon (
getClass ()
. getResource (
Home.URL_IMAGES
+ "NuovaNews48.png ")));
)
)
catch (RemoteException e)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "<h3> <u> Insertion operation request can not be completed. </ U> </ h3>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass ()
. getResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showMessageDialog (JDesktopPane, error,
"Error!" JOptionPane.ERROR_MESSAGE, err);
)
)
tableModel.fireTableDataChanged ();
azzeraForm ();
)
));

)
btnInsertModify return;
)

/ **
* This method initializes the button to clear the form or
* Cancel editing actions on a selected news.
*
* @ Return javax.swing.JButton - the button above.
* /
private JButton getBtnReset ()
(
if (null == btnReset)
(
btnReset = new JButton ();
btnReset.setText ( "Clear");
btnReset.setHorizontalTextPosition (SwingConstants.LEADING);
btnReset.setPreferredSize (new Dimension (103, 26));
btnReset.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "azzera16.png ")));
btnReset.setCursor (Cursor.getPredefinedCursor (Cursor.HAND_CURSOR));
btnReset.setName (btnAzzera ");
btnReset.addMouseListener (newsHelp);
btnReset.addActionListener (new ActionListener ()
(
public void actionPerformed (ActionEvent arg0)
(
azzeraForm ();
)
));
)
btnReset return;
)

/ **
* This method initializes the panel's online help.
*
* @ Return javax.swing.JPanel
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
textGuida.setEditable (false);
textGuida.setContentType ( "text / html");
textGuida.setOpaque (false);
textGuida
. setText ( "<html> Move your mouse pointer over a control"
+ "Of interest to display the context-sensitive help. </ Html>");
textGuida.setName (textGuida ");
textGuida.addMouseListener (newsHelp);
panelHelp.setPreferredSize(new Dimension(200, 100));
panelHelp.add (textGuida, BorderLayout.CENTER);
)
panelHelp return;
)

/ **
* This method initializes the table with all the news these
* In the system.
*
* @ Return javax.swing.JTable
* /
private JScrollPane getTableNews ()
(
if (null == tableNews)
(
NewsTableModel = new TableModel ();
tableNews = new ScrollableTable (TableModel);
tableNews.setRowHeight (64);
tableNews.setSelectionMode (ListSelectionModel.SINGLE_SELECTION);
tableNews.setSelectionBackground (new Color (0xe6, 0xe6, 0xFA));
tableNews.setColumnSelectionAllowed (false);
ListSelectionModel selectionModel = tableNews.getSelectionModel ();
selectionModel.addListSelectionListener (new ListSelectionListener ()
(
public void ValueChanged (ListSelectionEvent event)
(
int selectedRow = tableNews.getSelectedRow ();
btnModificaN.setEnabled ((selectedRow! = -1)? true: false);
btnEliminaN.setEnabled ((selectedRow! = -1)? true: false);
)
));
tableNews.addKeyListener (new KeyAdapter ()
(
public void keyPressed (KeyEvent PKEY)
(
int keyCode = pKey.getKeyCode ();
if (keyCode == KeyEvent.VK_ENTER)
(
btnModificaN.doClick ();
)
else if ((keyCode == KeyEvent.VK_CANCEL)
| | (KeyCode == KeyEvent.VK_BACK_SPACE))
(
btnEliminaN.doClick ();
)
)
));

scrollTableNews = new JScrollPane ();
scrollTableNews.setViewportView (tableNews);
scrollTableNews
. setVerticalScrollBarPolicy (JScrollPane.VERTICAL_SCROLLBAR_ALWAYS);
)
scrollTableNews return;
)

/ **
* This method initializes the text area that contains the text of a news.
*
* @ Return javax.swing.JTextArea - the text area.
* /
private JTextArea getTestoNews ()
(
if (null == testoNews)
(
testoNews = new JTextArea ();
testoNews.setWrapStyleWord (true);
testoNews.setLineWrap (true);
testoNews.setBorder (BorderFactory.createLoweredBevelBorder ());
testoNews.setColumns (18);
testoNews.setLineWrap (true);
testoNews.setRows (4);
testoNews.setDocument (new LimitedDocument (200));
testoNews.setName ( "text");
testoNews.addMouseListener (newsHelp);
testoNews.addKeyListener (new KeyAdapter ()
(
public void keyTyped (KeyEvent pKeyEvent)
(
keyChar pKeyEvent.getKeyChar = char ();
if (Character.isDigit (keyChar)
| | Character.isLetter (keyChar)
| | Character.isWhitespace (keyChar))
(
int len = testoNews.getText (). length ();
if (len! = 200)
(
labelCaratteri
. setText ( "# Characters"
+ (200 - (testoNews.getText ()
. length () + 1)));
)
)
)

public void keyPressed (KeyEvent pKeyEvent)
(
int keyCode = pKeyEvent.getKeyCode ();
if (keyCode == KeyEvent.VK_CANCEL
| | KeyCode == KeyEvent.VK_BACK_SPACE)
(
int len = testoNews.getText (). length ();
if (len! = 0)
(
labelCaratteri.setText ( "# Characters"
+ (200 - len + 1));
)
)
)
));

)
testoNews return;
)

/ **
* This method resets the form fields.
* /
private void azzeraForm ()
(
btnInsertModify.setText ( "Insert");
btnReset.setText ( "Clear");
btnReset.setIcon (new ImageIcon (getClass (). getResource (
Home.URL_IMAGES + "azzera16.png ")));
formNews.setBorder (BorderFactory.createTitledBorder (BorderFactory
. createLineBorder (new Color (51, 102, 255), 3), "New News"
TitledBorder.DEFAULT_JUSTIFICATION,
TitledBorder.DEFAULT_POSITION,
new Font ( "Dialog", Font.BOLD, 12), new Color (0, 102, 204)));
durataNews.setSelectedIndex (0);
testoNews.setText ("");
tableNews.clearSelection ();
prSlider.setValue (1);
labelCaratteri.setText ( "# Characters: 200");
idNews = -1;
)

/ **
* This method imports the news downloaded from the server in the table.
* /
private void caricaTabella ()
(
ArrayList <BeanNews> news = null;
TRY
(
gestioneNews.ottieniAllNews news = ();
)
/ / If an error displays an error message.
catch (RemoteException e)
(
JLabel error = new JLabel (
"<html> <h2> Unable to communicate with the server eTour. </ h2>"
+ "The list of <h3> <u> News is not loaded. </ U> </ h3>"
+ "<p> Please try again later. </ P>"
+ "<p> If the error persists, please contact technical support. </ P>"
+ "<p> We apologize for the inconvenience. </ Html>");
Err = new ImageIcon ImageIcon (getClass (). GetResource (
Home.URL_IMAGES + "error48.png"));
JOptionPane.showInternalMessageDialog (this, error, "Error!"
JOptionPane.ERROR_MESSAGE, err);
)
finally
(
NewsTableModel = new TableModel (news);
tableNews.setModel (TableModel);
/ / Text of news
tableNews.getColumnModel (). GetColumn (0). setPreferredWidth (320);
tableNews.getColumnModel (). GetColumn (0). setCellRenderer (
New TestoNewsRenderer ());
/ / Priority
tableNews.getColumnModel (). GetColumn (1). setPreferredWidth (100);
tableNews.getColumnModel (). GetColumn (1). setCellRenderer (
New PrioritaRenderer ());
/ / Date of entry
tableNews.getColumnModel (). GetColumn (2). setPreferredWidth (80);
/ / End Date
tableNews.getColumnModel (). GetColumn (3). setPreferredWidth (80);
)

)
)