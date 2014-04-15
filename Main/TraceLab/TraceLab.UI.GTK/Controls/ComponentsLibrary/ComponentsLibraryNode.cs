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
//
using System;
using System.Collections.Generic;
using TraceLab.Core.Components;


namespace TraceLab.UI.GTK
{
    public class ComponentsLibraryNode : IComparable<ComponentsLibraryNode>, IEnumerable<ComponentsLibraryNode>
    {
        Dictionary<string, ComponentsLibraryNode> children = new Dictionary<string,ComponentsLibraryNode>();
        String label;

        public ComponentsLibraryNode (String label, MetadataDefinition data = null)
        {
            Label = label;
            Data = data;
        }

        public String Label 
        {
            get
            {
                return label;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                label = value;
            }
        }

        public MetadataDefinition Data 
        {
            get;
            set;
        }

        public ComponentsLibraryNode this[String label]
        {
            get
            {
                if (!children.ContainsKey(label))
                    return null;
                return children[label];
            }
        }

        public virtual ComponentsLibraryNode GetChild(string label)
        {
            if (!children.ContainsKey(label))
                return null;
            return children[label];
        }

        public virtual List<ComponentsLibraryNode> GetChildren()
        {
            return new List<ComponentsLibraryNode>(children.Values);
        }

        public virtual void AddChild(ComponentsLibraryNode child)
        {
            if (child == null)
                throw new ArgumentNullException ();
            children[child.Label] = child;
        }

        public virtual bool RemoveChild(ComponentsLibraryNode child)
        {
            if (child == null)
                return false;
            return children.Remove(child.Label);
        }

        #region IComparable
        public int CompareTo(ComponentsLibraryNode other)
        {
            return Label.CompareTo(other.Label);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<ComponentsLibraryNode> GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Lets call the generic version here
            return this.GetEnumerator();
        }

        #endregion
    }
}

