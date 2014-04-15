using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceLab.Core.Components
{
    [Serializable]
    public class DocumentationLink
    {
        static DocumentationLink()
        {
            SortLinksByOrderComparer = new SortLinksByOrder();
        }

        private DocumentationLink() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkAttribute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="order">The order.</param>
        public DocumentationLink(Uri url, string title, string description, int order)
        {
            Url = url;
            Title = title;
            Description = description;
            Order = order;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public Uri Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order
        {
            get;
            set;
        }

        public static IComparer<DocumentationLink> SortLinksByOrderComparer
        {
            get;
            private set;
        }   

        private class SortLinksByOrder : IComparer<DocumentationLink>
        {
            #region IComparer<DocumentationLink> Members

            public int Compare(DocumentationLink link1, DocumentationLink link2)
            {
                if (link1.Order > link2.Order)
                    return 1;
                if (link1.Order < link2.Order)
                    return -1;
                else
                    return 0;
            }

            #endregion
        }

    }
}
