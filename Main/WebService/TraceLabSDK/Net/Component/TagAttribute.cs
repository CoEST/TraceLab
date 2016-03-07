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
    /// Is used to provide default tags for components
    /// Component class can be tagged to be added to certain category. To do it add Tag attribute to the component class.
    /// Tag can be simple, or can have subcategories.
    /// For example:
    /// [Tag("Importers")]
    /// will add component to Importers category.
    /// 
    /// [Tag("Importers.TLArtifactsCollection.From XML")]
    /// will add component to subcategory From XML of subcategory of TLArtifactsCollection of category Importers.
    /// 
    /// Component can have multiple tags.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagAttribute"/> class.
        /// </summary>
        /// <param name="tag">The tag to be applied.</param>
        public TagAttribute(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets the tag.
        /// </summary>
        public string Tag
        {
            get;
            private set;
        }
    }
}
