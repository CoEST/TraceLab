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
using System.Xml.Serialization;
using TraceLab.Core.Experiments;
using TraceLabSDK;
using System.Xml.XPath;
using System.Xml;
using System.Collections.Specialized;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Represents decision scope.
    /// </summary>
    [XmlRoot("Metadata")]
    [Serializable]
    public class ScopeMetadata : ScopeBaseMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeMetadata"/> class.
        /// </summary>
        protected ScopeMetadata()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeMetadata"/> class.
        /// </summary>
        /// <param name="compositeComponentGraph">The composite component graph.</param>
        /// <param name="label">The label.</param>
        /// <param name="experimentLocationRoot">The experiment location root.</param>
        internal ScopeMetadata(CompositeComponentEditableGraph compositeComponentGraph, string label, string experimentLocationRoot)
            : base(compositeComponentGraph, label, experimentLocationRoot)
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override Metadata Clone()
        {
            var clone = new ScopeMetadata();
            clone.CopyFrom(this);

            return clone;
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(Metadata other)
        {
            base.CopyFrom(other);
        }
    }
}
