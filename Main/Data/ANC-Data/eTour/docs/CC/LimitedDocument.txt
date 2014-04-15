package unisa.gps.etour.gui.operatoreagenzia.documents;

import javax.swing.text.AttributeSet;
import javax.swing.text.BadLocationException;
import javax.swing.text.PlainDocument;

public class extends LimitedDocument PlainDocument
(
private static final long serialVersionUID = 1L;
private int maxLength;

public LimitedDocument (int pMaxLength)
(
maxLength = pMaxLength;
)

public void insertString (int pOffset, String pStringa, AttributeSet pAttribute) throws BadLocationException
(
if (null == pStringa)
(
return;
)
/ / Note: the content is always a newline at the end
int capacity = maxLength + 1 - getContent (). length ();
/ / If the maximum length is greater than or equal to the string, the part.
if (capacity> = pStringa.length ())
(
super.insertString (pOffset, pStringa, pAttribute);
)
/ / Otherwise add what may
else
(
if (capacity> 0)
(
super.insertString (pOffset, pStringa.substring (0, capacity), pAttribute);
)
)
)
)
