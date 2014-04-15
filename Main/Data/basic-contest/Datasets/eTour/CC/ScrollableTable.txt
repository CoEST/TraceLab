package unisa.gps.etour.gui.operatoreagenzia.tables;

import java.awt .*;
import javax.swing.JTable;
import javax.swing.Scrollable;
import javax.swing.SwingConstants;
table .*;

/ **
  * Create a custom JTable that can be displayed through
  * Components that enable scrolling.
  * @ See javax.swing.JTable
  * @ See javax.swing.Scrollable
  * @ Author _OniZuKa_
  * @ Version 1.0
  * /
public class ScrollableTable extends JTable implements Scrollable
(

private static final int maxUnitIncrement = 20;

public ScrollableTable ()
(
super ();
)

public ScrollableTable (TableModel tm)
(
super (tm);
setGridColor (Color.LIGHT_GRAY);
setIntercellSpacing (new Dimension (5,0));
)

getPreferredScrollableViewportSize public Dimension ()
(
GetPreferredSize return ();
)

public int getScrollableUnitIncrement (Rectangle visibleRect,
int orientation, int direction)
(

posCorrente int = 0;
if (orientation == SwingConstants.HORIZONTAL)
(
posCorrente = visibleRect.x;
)
else
(
posCorrente = visibleRect.y;
)

if (direction <0)
(
int = newPos posCorrente - (posCorrente / maxUnitIncrement)
* MaxUnitIncrement;
return (newPos == 0)? maxUnitIncrement: newPos;
)
else
(
return ((posCorrente / maxUnitIncrement) + 1) * maxUnitIncrement
- PosCorrente;
)
)

public int getScrollableBlockIncrement (Rectangle visibleRect,
int orientation, int direction)
(
if (orientation == SwingConstants.HORIZONTAL)
(
return visibleRect.width - maxUnitIncrement;
)
else
(
return visibleRect.height - maxUnitIncrement;
)
)

public boolean getScrollableTracksViewportWidth ()
(
return false;
)

public boolean getScrollableTracksViewportHeight ()
(
return false;
)
) 