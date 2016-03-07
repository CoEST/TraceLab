using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Policy;

namespace TraceLabSDK
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class LinkAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkAttribute"/> class.
        /// </summary>
        public LinkAttribute() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkAttribute"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="order">The order.</param>
        public LinkAttribute(string url, string title, string description, int order)
        {
            Url = url ?? String.Empty;
            Title = title ?? String.Empty;
            Description = description ?? String.Empty;
            Order = order;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
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
    }
}
