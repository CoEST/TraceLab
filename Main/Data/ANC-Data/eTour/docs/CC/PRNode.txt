package unisa.gps.etour.gui.operatoreagenzia.tables;

import javax.swing.tree.DefaultMutableTreeNode;

/ **
  * <b> PRNode </ b>
  <p> * This class creates a node in a JTree to store
  * Information for a refreshment. </ P>
  *
  * @ See javax.swing.JTree;
  * @ See javax.swing.tree.DefaultMutableTreeNode;
  * @ Version 1.0
  * @ Author Mario Gallo
  *
  * /
public class DefaultMutableTreeNode extends PRNode
(
private int id;

public PRNode ()
(
super ();
)

/ **
* Create a node with the name of refreshment and
* Your id supplied as parameters.
*
PNomePR * @ param String - the name of refreshment.
PID * @ param int - the id of refreshment.
* @ Throws IllegalArgumentException - if the name provided as input is invalid.
* /
public PRNode (Phnom String, int pid) throws IllegalArgumentException
(
super ();
if (Phnom == null | | pNome.equals (""))
(
throw new IllegalArgumentException (
"Name of refreshment supplied invalid input.");
)
setUserObject (Phnom);
id = pid;
)

/ **
*
* Returns the id of the point of comfort for which information
* Are stored in this node.
*
* @ Return int - the id of refreshment.
* /
public int getID ()
(
return id;
)

/ **
*
* Stores the id of the refreshment provided input.
*
* @ Param int PID - an ID of an eating place.
* /
public void setID (int pid)
(
id = pid;
)

/ **
*
* Return the name of refreshment.
*
* @ Return String - the name of refreshment.
* /
public String getName ()
(
return (String) super.getUserObject ();
)

/ **
*
* Stores the name of the refreshment provided input.
*
Pnom * @ param String - the name of a refreshment.
* @ Throws IllegalArgumentException - if the name provided as input is invalid.
* /
public void setNome (String Pnom) throws IllegalArgumentException
(
if (Phnom == null | | pNome.equals (""))
(
throw new IllegalArgumentException (
"Name of refreshment supplied invalid input.");
)
setUserObject (Phnom);
)
) 