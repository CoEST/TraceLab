package unisa.gps.etour.gui;

import java.awt.Dimension;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.beans.PropertyVetoException;
import java.util.Iterator;
import java.util.Vector;
import javax.swing.DefaultDesktopManager;
import javax.swing.ImageIcon;
import javax.swing.JComponent;
import javax.swing.JDesktopPane;
import javax.swing.JInternalFrame;
import javax.swing.JMenuItem;
import javax.swing.JPopupMenu;
import unisa.gps.etour.gui.operatoreagenzia.IScheda;

/ **
  * Class for handling custom internal frame inserted in a
  * JDesktopPane.
  *
  * @ Version 0.1
  * @ Author Mario Gallo
  *
  * © 2007 eTour Project - Copyright by DMI SE @ SA Lab --
  * University of Salerno
  * /
public class DeskManager extends DefaultDesktopManager
(
private static final String URL_IMAGES = "/ unisa / gps / eTour / gui / images /";
private JPopupMenu deskMenu;
private JMenuItem riduciTutti;
private JMenuItem ripristinaTutti;
private JMenuItem Closeall;
private Vector <JInternalFrame> iconifiedFrames;
private int locationX;
private int locationY;

/ **
* Default Constructor.
* /
public DeskManager ()
(
super ();
iconifiedFrames = <JInternalFrame> new Vector ();
initializeDeskMenu ();
locationX = 0;
locationY = -1;
)

/ **
* Manages the movement of JInternalFrame inside the area of
* JDesktopPane, preventing the frames are brought out of the visible area.
*
* @ Param javax.swing.JComponent AComponent - the component of which
* Manage the move.
* @ Param int x - x cordinate the point where it was moved
* Component.
* @ Param int y - y cordinate the point where it was moved
* Component.
* /
public void dragFrame (AComponent JComponent, int x, int y)
(
if (AComponent instanceof JInternalFrame)
(
JInternalFrame frame = (JInternalFrame) AComponent;
if (frame.isIcon ())
(
x = frame.getLocation (). x;
y = frame.getLocation (). y;
)
else
(
JDesktopPane desk frame.getDesktopPane = ();
Dimension d = desk.getSize ();
if (x <0)
(
x = 0;
)

else
(
if (x + frame.getWidth ()> d.width)
(
x = d.width - frame.getWidth ();
)
)

if (y <0)
(
y = 0;
)
else
(
if (y + frame.getHeight ()> d.height)
(
y = d.height - frame.getHeight ();
)
)
)
)

super.dragFrame (AComponent, x, y);
)

/ **
* Customize the action of reducing the JInternalFrame an icon, creating
* Clickable bars on the bottom of JDesktopPane.
*
* @ Param JInternalFrame frame - a frame inside a
* JDesktopPane.
* /
public void iconifyFrame (JInternalFrame frame)
(
TRY
(
JDesktopPane desk frame.getDesktopPane = ();
Dimension d = desk.getSize ();
frame.setClosable (false);
frame.setMaximizable (true);
frame.setIconifiable (false);
Rectangle features;
if (frame.isMaximum ())
(
features frame.getNormalBounds = ();
)
else
features frame.getBounds = ();
frame.setSize (200, 30);
setPreviousBounds (frame, features);
if (iconifiedFrames.isEmpty ())
(
locationX = 0;
)
else
(
locationX + = 200;
)
if (locationY == -1)
(
locationY d.height = - 30;
)
if (locationX + 200> d.width)
(
locationX = 0;
locationY -= 30;
)
frame.setLocation (locationX, locationY);
frame.setResizable (false);
iconifiedFrames.add (frame);
)
catch (Exception ex)
(
ex.printStackTrace ();
)
)

/ **
* Restore the frame from the effect of minimizing, resetting the
* Position and size it had before.
*
* @ Param javax.swing.JInternalFrame frame - a frame inside a
* JDesktopPane.
* /
public void deiconifyFrame (JInternalFrame frame)
(
TRY
(
JDesktopPane desk frame.getDesktopPane = ();
Dimension deskSize = desk.getSize ();
iconifiedFrames.remove (frame);
Rectangle features getPreviousBounds = (frame);
if (caratteristiche.width> deskSize.width)
(
caratteristiche.width = deskSize.width;
caratteristiche.x = 0;
)
if (caratteristiche.width + caratteristiche.x> deskSize.width)
(
caratteristiche.x = (deskSize.width - caratteristiche.width) / 2;
)
if (caratteristiche.height> deskSize.height)
(
caratteristiche.height = deskSize.height;
caratteristiche.y = 0;
)
if (caratteristiche.height + caratteristiche.y> deskSize.height)
(
caratteristiche.y = (deskSize.height - caratteristiche.height) / 2;
)
frame.setSize (caratteristiche.width, caratteristiche.height);
frame.setLocation (caratteristiche.x, caratteristiche.y);
frame.setIconifiable (true);
frame.setClosable (true);
if (frame instanceof IScheda)
(
frame.setMaximizable (false);
frame.setResizable (false);
)
else
(
frame.setMaximizable (true);
frame.setResizable (true);
)
locationX -= 200;
if (locationX <0)
(
locationX = deskSize.width / 200 - 200;
if (locationY! = deskSize.height - 30)
(
locationY -= 30;
)
)
repaintIconifiedFrames (desk);
)
catch (Exception ex)
(
ex.printStackTrace ();
)
)

/ **
* Return the focus to a selected frame, and, if the frame
* Is iconificato, the deiconifica.
*
* @ Param JInternalFrame frame - a frame within a
* JDesktopPane
* /
public void activateFrame (JInternalFrame frame)
(
TRY
(
if (frame.isIcon ())
frame.setIcon (false);
frame.setSelected (true);
super.activateFrame (frame);
)
catch (PropertyVetoException s)
(
e.printStackTrace ();
)

)

/ **
* Center the frame supplied as a parameter in JDesktopPane.
*
* @ Param javax.swing.JInternalFrame frame - a frame inside a
* JDesktopPane.
* @ Return void
* /
public void centerFrame (JInternalFrame frame)
(
JDesktopPane desk frame.getDesktopPane = ();
Dimension d = desk.getSize ();
F = Dimension frame.getSize ();
frame.setLocation (d.width / 2 - f.width / 2, d.height / 2 - f.height
/ 2);
)

/ **
* Redraw the frames in the desktop iconificati bread provided.
*
* @ Param javax.swing.JDesktopPane Desk - a desktop bread associated with a
* Desk manager.
* @ Throws IllegalArgumentException - was supplied as a parameter
* JDesktopPane which is not associated with a Desk Manager.
* /
public void repaintIconifiedFrames (JDesktopPane desk)
throws IllegalArgumentException
(
if (desk.getDesktopManager ()! = this)
throw new IllegalArgumentException (
"I found no object"
+ "Type DeskManager associated");
<JInternalFrame> Iconificati = iconifiedFrames.iterator iterator ();
int i = 0;
int xLocation;
int yLocation = desk.getHeight () - 30;
while (iconificati.hasNext ())
(
Current = iconificati.next JInternalFrame ();
xLocation = 200 * i;
if (xLocation + 200> = desk.getWidth ())
(
xLocation = 0;
yLocation -= 30;
i = 0;
)
corrente.setLocation (xLocation, yLocation);
i + +;
)
)

/ **
* Redraw (and resize if necessary) all the frames contained in a
* Since JDesktopPane.
*
* @ Param javax.swing.JDesktopPane Desk - a desktop pane.
* @ Throws IllegalArgumentException - if the desktop bread supply is not
* Associated with a desktop manager like DeskManager.
* /
public void repaintAllFrames (JDesktopPane desk)
throws IllegalArgumentException
(
if (desk.getDesktopManager ()! = this)
throw new IllegalArgumentException (
"I found no object"
+ "Type DeskManager associated");
JInternalFrame [] frames = desk.getAllFrames ();
Dimension deskSize = desk.getSize ();
for (int i = 0; i <frames.length i + +)
(
JInternalFrame current = frames [i];
if (! corrente.isIcon ())
(
Rectangle frameBounds = corrente.getBounds ();
if (frameBounds.width> deskSize.width)
frameBounds.width = deskSize.width;
if (frameBounds.height> deskSize.height)
frameBounds.height = deskSize.height;
if (frameBounds.x + frameBounds.width> deskSize.width)
frameBounds.x = deskSize.width - frameBounds.width;
if (frameBounds.y + frameBounds.height> deskSize.height)
frameBounds.y = deskSize.height - frameBounds.height;
corrente.setBounds (frameBounds);
)

)
repaintIconifiedFrames (desk);
)

/ **
* Open a frame of the class specified using the display
* Waterfall. If you already have a frame of classes given, the frame is
* Activated.
*
* @ Param class class - a class type that extends JInternalFrame.
* @ Param javax.swing.JDesktopPane Desk - a desktop pane.
* @ Throws IllegalArgumentException - The class provided is not a
* JInternalFrame.
* /
public void openFrame (Class class, JDesktopPane desk)
throws IllegalArgumentException
(
if (classe.getSuperclass ()! = JInternalFrame.class)
throw new IllegalArgumentException (
"The class provided input has"
+ "As a superclass javax.swing.JInternalFrame.");
TRY
(
JInternalFrame [] frames = desk.getAllFrames ();
int i;
for (i = 0; i <frames.length i + +)
if (frames [i]. getClass (). equals (class))
break;
if (i == frames.length)
(
JInternalFrame new = (JInternalFrame) classe.newInstance ();
desk.add (new, Integer.MAX_VALUE);
Dimension frameSize = nuovo.getPreferredSize ();
nuovo.setSize (frameSize);
Dimension deskSize = desk.getSize ();
PosNuovo Point = new Point (10, 10);
for (i = frames.length - 1, i> = 0; i -)
(
if (frames [i]. getLocation (). equals (posNuovo))
(
posNuovo.x = frames [i]. getLocation (). x + 30;
posNuovo.y = frames [i]. getLocation (). y + 30;
)
)
if ((posNuovo.x + frameSize.width> deskSize.width)
| | (PosNuovo.y + frameSize.height> deskSize.height))
centerFrame (new);
else
nuovo.setLocation (posNuovo);
nuovo.setVisible (true);
)
else
(
activateFrame (frames [i]);

)
)
catch (Exception ex)
(
ex.printStackTrace ();
)
)

/ **
* Displays a popup menu with options for frames of a desktop bread
* The selected location.
*
* @ Param java.awt.Point Pointe - the point where to place the menu.
* @ Param javax.swing.JDesktopPane desk - a JDesktopPane which &grave; an associated
* Instance of DeskManager.
* @ Throws IllegalArgumentException - &grave; was provided as a parameter
* JDesktopPane that &grave; not associated with a Desk Manager.
* /
public void showPopupMenu (Point Pointe, JDesktopPane desk)
(
if (desk.getDesktopManager ()! = this)
throw new IllegalArgumentException (
"I found no object"
+ "Type DeskManager associated");
ripristinaTutti.setEnabled (true);
chiudiTutti.setEnabled (true);
riduciTutti.setEnabled (true);
JInternalFrame [] frames = desk.getAllFrames ();
if (frames.length == 0)
(
ripristinaTutti.setEnabled (false);
chiudiTutti.setEnabled (false);
riduciTutti.setEnabled (false);
)
if (iconifiedFrames.size () == 0)
(
ripristinaTutti.setEnabled (false);

)
if (iconifiedFrames.size () == frames.length)
(
riduciTutti.setEnabled (false);
)
deskMenu.show (desk, aPoint.x, aPoint.y);
)

/ **
* Deiconifica all frames previously iconificati.
*
* /
public void deiconifyAll ()
(
if (iconifiedFrames.size ()! = 0)
(
<JInternalFrame> Vector copy = (Vector <JInternalFrame>) iconifiedFrames
. clone ();
<JInternalFrame> Frames = copia.iterator iterator ();
while (frames.hasNext ())
(
TRY
(
frames.next (). setIcon (false);
)
catch (PropertyVetoException s)
(
e.printStackTrace ();
)

)
copy = null;
iconifiedFrames.removeAllElements ();

)
)

/ **
* Minimize all frames of a JDesktopPane provided in &grave; an associated
* DeskManager.
*
* @ Param JDesktopPane Desk - a desktop pane.
* @ Throws IllegalArgumentException - &grave; was provided as a parameter
* JDesktopPane that &grave; not associated with a Desk Manager.
* /
public void iconifyAll (JDesktopPane desk)
(
if (desk.getDesktopManager ()! = this)
throw new IllegalArgumentException (
"I found no object"
+ "Type DeskManager associated");
JInternalFrame [] frames = desk.getAllFrames ();
for (int i = 0; i <frames.length i + +)
TRY
(
frames [i]. setIcon (true);
)
catch (PropertyVetoException s)
(
e.printStackTrace ();
)
)

/ **
* Close all frames in a given JDesktopPane.
*
* @ Param javax.swing.JDesktopPane Desk - a desktop &grave; bread in an associated
* DeskManager.
* @ Throws IllegalArgumentException - &grave; was provided as a parameter
* JDesktopPane that &grave; not associated with a Desk Manager.
* /
public void closeAll (JDesktopPane desk)
(
if (desk.getDesktopManager ()! = this)
throw new IllegalArgumentException (
"I found no object"
+ "Type DeskManager associated");
JInternalFrame [] frames = desk.getAllFrames ();
if (frames.length! = 0)
(
for (int i = 0; i <frames.length i + +)
frames [i]. dispose ();
iconifiedFrames.removeAllElements ();
)
)

/ **
* Initialize the DeskMenu.
*
* /
public void initializeDeskMenu ()
(
deskMenu = new JPopupMenu ();
riduciTutti = new JMenuItem ( "Collapse All");
riduciTutti.setIcon (new ImageIcon (getClass (). getResource (
URL_IMAGES + "reduceAll.png ")));
ripristinaTutti = new JMenuItem ( "Reset All");
ripristinaTutti.setIcon (new ImageIcon (getClass (). getResource (
URL_IMAGES + "activateall.png ")));
Closeall = new JMenuItem ( "Close All");
chiudiTutti.setIcon (new ImageIcon (getClass (). getResource (
URL_IMAGES + "closeall.png ")));
deskMenu.add (riduciTutti);
deskMenu.addSeparator ();
deskMenu.add (ripristinaTutti);
deskMenu.addSeparator ();
deskMenu.add (Closeall);
ActionListener menuListener = new ActionListener ()
(
public void actionPerformed (ActionEvent aEvent)
(
if (aEvent.getSource () == ripristinaTutti)
deiconifyAll ();
if (aEvent.getSource () == Closeall)
closeAll ((JDesktopPane) deskMenu.getInvoker ());
if (aEvent.getSource () == riduciTutti)
iconifyAll ((JDesktopPane) deskMenu.getInvoker ());
)

);
riduciTutti.addActionListener (menuListener);
ripristinaTutti.addActionListener (menuListener);
chiudiTutti.addActionListener (menuListener);
)
)
