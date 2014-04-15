// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceLabSDK
{
    /// <summary>
    /// This attribute is used to declare what inputs or outputs a Component has.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class IOSpecAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IOSpecAttribute"/> class.
        /// </summary>
        public IOSpecAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOSpecAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of input or output this is</param>
        /// <param name="name">The name that will be used to Load or Store this value in the Workspace</param>
        /// <param name="dataType">Type of the data.</param>
        public IOSpecAttribute(IOSpecType type, string name, Type dataType)
        {
            IOType = type;
            Name = name ?? string.Empty;
            DataType = dataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOSpecAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of input or output this is</param>
        /// <param name="name">The name that will be used to Load or Store this value in the Workspace</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="description">The description.</param>
        public IOSpecAttribute(IOSpecType type, string name, Type dataType, string description)
            : this(type, name, dataType) 
        {
            Description = description;
        }

        /// <summary>
        /// Gets or sets the type of the input or output
        /// </summary>
        /// <value>
        /// The type of the IO.
        /// </value>
        public IOSpecType IOType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name that will be used to Load or Store this value in the Workspace.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public Type DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description/documentation.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get;
            set;
        }
    }
}
