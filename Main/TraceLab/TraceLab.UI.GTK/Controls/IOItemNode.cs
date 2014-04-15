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
using TraceLab.Core.Components;

namespace TraceLab.UI.GTK
{
    public class IOItemNode : Gtk.TreeNode 
    {
        private IOItem m_ioItem;
        
        public IOItemNode(IOItem ioItem) 
        {
            m_ioItem = ioItem;
        }
        
        [Gtk.TreeNodeValue(Column=0)]
        public string Name 
        {
            get { return m_ioItem.IOItemDefinition.Name; }
        }
        
        [Gtk.TreeNodeValue(Column=1)]
        public string MappedTo 
        {
            get { return m_ioItem.MappedTo; }
            set { m_ioItem.MappedTo = value; }
        }
        
        [Gtk.TreeNodeValue(Column=2)]
        public string FriendlyType
        {
            get { return m_ioItem.IOItemDefinition.FriendlyType; }
        }
        
        public string Type {
            get { return m_ioItem.IOItemDefinition.Type; }
        }
    }
}

