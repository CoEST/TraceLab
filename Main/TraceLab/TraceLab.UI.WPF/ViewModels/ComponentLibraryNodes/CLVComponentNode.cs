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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using TraceLab.Core.Components;

namespace TraceLab.UI.WPF.ViewModels
{

    class CLVComponentNode : CLVBaseNode
    {
        #region Fields

        IGetCLNode m_componentsLibrary;
        MetadataDefinition m_definition;
        private List<CLVBaseNode> m_parents = new List<CLVBaseNode>();

        #endregion Fields

        #region Constructors

        public CLVComponentNode(MetadataDefinition definition, IGetCLNode componentsLibrary)
            : base()
        {
            if (definition == null)
                throw new ArgumentNullException("definition");
            if (componentsLibrary == null)
                throw new ArgumentNullException("componentsLibrary");

            m_definition = definition;
            m_componentsLibrary = componentsLibrary;

            if (m_definition.Tags != null)
            {
                if (m_definition.Tags.Values.Count == 0)
                {
                    AddTag("Uncategorized");
                }
                else
                {
                    foreach (string tag in m_definition.Tags.Values)
                    {
                        AddTag(tag);
                    }
                }

                m_definition.Tags.TagAdded += OnTagAdded;
                m_definition.Tags.TagRemoved += OnTagRemoved;
            }

            PartialId = m_definition.ID.Substring(m_definition.ID.Length - 5);
        }

        #endregion Constructors

        #region Properties

        public MetadataDefinition Component
        {
            get { return m_definition; }
        }

        public override string Label
        {
            get
            {
                return m_definition.Label;
            }
        }

        public string PartialId
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        public override void AddChild(CLVBaseNode child)
        {
            throw new InvalidOperationException();
        }

        public void AddTag(string tag)
        {
            CLVBaseNode parent = m_componentsLibrary.GetNode(tag);
            if (parent == null)
                throw new InvalidOperationException();

            parent.AddChild(this);
            m_parents.Add(parent);
        }

        public override void RemoveChild(CLVBaseNode child)
        {
            throw new InvalidOperationException();
        }

        private void OnTagAdded(object sender, TagChangedEventArgs args)
        {
            AddTag(args.Tag);

            // Ensure that this tag is no longer "uncategorized".
            RemoveTag("Uncategorized");
        }

        private void OnTagRemoved(object sender, TagChangedEventArgs args)
        {
            RemoveTag(args.Tag);
            m_componentsLibrary.RemoveTagIfEmpty(args.Tag);
        }

        private void RemoveTag(string tag)
        {
            CLVBaseNode parent = m_componentsLibrary.GetNode(tag);
            if (parent == null)
                throw new InvalidOperationException();

            parent.RemoveChild(this);
            m_parents.Remove(parent);
        }

        public void Remove()
        {
            foreach (CLVBaseNode parent in m_parents)
            {
                parent.RemoveChild(this);
                m_componentsLibrary.RemoveTagIfEmpty(parent.Label);
            }
        }

        #endregion Methods
    }
}
