using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AvalonDock
{
    public class DocumentClosingCancelEventArgs : CancelEventArgs
    {
        public DocumentClosingCancelEventArgs(bool cancel, object document)
            : base(cancel)
        {
            Document = document;
        }


        /// <summary>
        /// Gets or sets the document that is about being closed.
        /// </summary>
        /// <value>
        /// The document that will be closed, if not canceled.
        /// </value>
        public object Document
        {
            get;
            set;
        }
    }
}
