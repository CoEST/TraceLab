using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDock
{
    public class DocumentClosedEventArgs : EventArgs
    {
        public DocumentClosedEventArgs(object document)
        {
            Document = document;
        }

        /// <summary>
        /// Gets or sets the document that was closed.
        /// </summary>
        /// <value>
        /// The document which was closed
        /// </value>
        public object Document
        {
            get;
            set;
        }
    }
}
