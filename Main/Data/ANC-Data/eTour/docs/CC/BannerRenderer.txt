package unisa.gps.etour.gui.operatoreagenzia.tables;

import java.awt.Color;
import java.awt.Component;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JTree;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeCellRenderer;

public class BannerRenderer extends DefaultTreeCellRenderer
(

public Component getTreeCellRendererComponent (pTree JTree, Object pValue,
pSelected boolean, boolean pExpanded, boolean pLeaf, int prow,
boolean pHasFocus)
(
Object obj = ((DefaultMutableTreeNode) pValue). GetUserObject ();
if ((object instanceof ImageIcon))
(
throw new IllegalArgumentException ( "Value cell unexpected.");
)
ImageIcon image = (ImageIcon) object;
JLabel aLabel = new JLabel ();
aLabel.setIcon (image);
aLabel.setSize (immagine.getIconWidth () + 10, image
. getIconHeight () + 10);
if (pSelected)
(
aLabel.setBorder (BorderFactory.createLineBorder (Color.red, 2));
)
aLabel return;

)

) 