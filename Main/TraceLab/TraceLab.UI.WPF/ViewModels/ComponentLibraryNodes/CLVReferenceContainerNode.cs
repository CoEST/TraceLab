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
    class CLVReferenceContainerNode : CLVBaseNode
    {
        public CLVReferenceContainerNode(ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> observableCollection)
        {
            if (observableCollection != null)
            {
                foreach (TraceLabSDK.PackageSystem.IPackageReference reference in observableCollection)
                {
                    AddChild(new CLVReferenceNode(reference));
                }

                observableCollection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(observableCollection_CollectionChanged);
            }
        }

        void observableCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        TraceLabSDK.PackageSystem.IPackageReference newItem = e.NewItems[0] as TraceLabSDK.PackageSystem.IPackageReference;
                        AddChild(new CLVReferenceNode(newItem));
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        TraceLabSDK.PackageSystem.IPackageReference oldItem = e.OldItems[0] as TraceLabSDK.PackageSystem.IPackageReference;

                        foreach (CLVReferenceNode node in this.AllChildren)
                        {
                            if (node.ID == oldItem.ID)
                            {
                                RemoveChild(node);
                                break;
                            }
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    {
                        TraceLabSDK.PackageSystem.IPackageReference oldItem = e.OldItems[0] as TraceLabSDK.PackageSystem.IPackageReference;

                        foreach (CLVReferenceNode node in this.AllChildren)
                        {
                            if (node.ID == oldItem.ID)
                            {
                                RemoveChild(node);
                                break;
                            }
                        }

                        TraceLabSDK.PackageSystem.IPackageReference newItem = e.NewItems[0] as TraceLabSDK.PackageSystem.IPackageReference;
                        AddChild(new CLVReferenceNode(newItem));
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public override string Label
        {
            get { return "Package references"; }
        }

        public override void AddChild(CLVBaseNode child)
        {
            CLVReferenceNode reference = child as CLVReferenceNode;
            if (reference == null)
                throw new ArgumentException("Child must be of type CLVReferenceNode", "child");

            HasError |= !reference.Exists;
            base.AddChild(child);
        }

        public override void RemoveChild(CLVBaseNode child)
        {
            CLVReferenceNode reference = child as CLVReferenceNode;
            if (reference != null)
            {
                base.RemoveChild(child);
            }
        }

        private bool m_error;
        public bool HasError
        {
            get { return m_error; }
            set
            {
                if (m_error != value)
                {
                    m_error = value;
                    OnPropertyChanged("HasError");
                }
            }
        }
    }
}
