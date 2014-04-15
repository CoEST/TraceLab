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
using System.Linq;
using System.Windows.Data;

using TraceLab.Core.ViewModels;

namespace TraceLab.UI.WPF.ViewModels
{
    class ComponentLibraryOrganizerVM : IGetCLNode, INotifyPropertyChanged
    {
        #region Fields

        private CLVTagNode m_allComponents = new CLVTagNode("All Components");
        private string m_componentFilter = string.Empty;
        private ObservableCollection<CLVBaseNode> m_componentNodeCollection = new ObservableCollection<CLVBaseNode>();
        private ICollectionView m_components;
        private ComponentLibraryViewModel m_componentsLibraryViewModel;
        private bool m_isRefilterQueued;
        private string m_lowercaseComponentFilter = string.Empty;
        private string m_lowercaseTagFilter = string.Empty;
        private string m_selectedPath = string.Empty;
        private CLVTagNode m_selectedTag;
        private string m_tagFilter = string.Empty;
        private ObservableCollection<CLVBaseNode> m_tagNodeCollection = new ObservableCollection<CLVBaseNode>();
        private ICollectionView m_tags;

        #endregion Fields

        #region Constructors

        public ComponentLibraryOrganizerVM(ComponentLibraryViewModel componentLibrary)
        {
            m_componentsLibraryViewModel = componentLibrary;
            m_tagNodeCollection.Add(m_allComponents);
            BuildTagsHierarchy();

            Tags = CollectionViewSource.GetDefaultView(m_tagNodeCollection);
            Tags.SortDescriptions.Add(new SortDescription("Label", ListSortDirection.Ascending));
        }

        #endregion Constructors

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        public IEnumerable<string> AllTags
        {
            get
            {
                List<string> tags = new List<string>();

                foreach (TraceLab.Core.Components.MetadataDefinition component in m_componentsLibraryViewModel.ComponentsCollection)
                {
                    foreach (string tag in component.Tags.Values)
                    {
                        tags.Add(tag);
                    }
                }

                return tags;
            }
        }

        public string ComponentFilter
        {
            get { return m_componentFilter; }
            set
            {
                if (m_componentFilter != value)
                {
                    m_componentFilter = value;
                    m_lowercaseComponentFilter = m_componentFilter.ToLower();
                    if (Components != null)
                    {
                        Components.Filter = FilterComponents;
                    }
                    OnPropertyChanged("ComponentFilter");
                }
            }
        }

        public ICollectionView Components
        {
            get { return m_components; }
            set
            {
                m_components = value;
                if (m_components != null)
                {
                    m_components.Filter = FilterComponents;
                }
                OnPropertyChanged("Components");
            }
        }

        public string ComponentSectionName
        {
            get
            {
                string sectionName = "<Select a tag to see the tagged components>";
                if (SelectedTag == m_allComponents)
                {
                    sectionName = "All components:";
                }
                else if(SelectedTag != null)
                {
                    sectionName = string.Format("Components with tag: {0}", SelectedTag.Label);
                }

                return sectionName;
            }
        }

        public string SelectedComponentAuthor
        {
            get
            {
                string name = "";
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            name = node.Component.Author;
                        }
                        else if (name.Equals(node.Component.Author) == false)
                        {
                            name = "<Multiple Selection>";
                        }
                    }
                }

                return name;
            }
        }

        public string SelectedComponentDescription
        {
            get
            {
                string name = "";
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            name = node.Component.Description;
                        }
                        else if (name.Equals(node.Component.Description) == false)
                        {
                            name = "<Multiple Selection>";
                        }
                    }
                }

                return name;
            }
        }

        public string SelectedComponentName
        {
            get
            {
                string name = "";
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            name = node.Label;
                        }
                        else if (name.Equals(node.Label) == false)
                        {
                            name = "<Multiple Selection>";
                        }
                    }
                }

                return name;
            }
        }

        public IEnumerable<string> SelectedComponentTags
        {
            get
            {
                List<string> tags = new List<string>();
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    HashSet<string> validTags = null;

                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        HashSet<string> nodeTags = null;

                        if (node.Component.Tags == null)
                        {
                            nodeTags = new HashSet<string>();
                        }
                        else
                        {
                            nodeTags = new HashSet<string>(node.Component.Tags.Values);
                        }

                        if (validTags == null)
                        {
                            validTags = nodeTags;
                        }
                        else
                        {
                            validTags.IntersectWith(nodeTags);
                        }
                    }

                    if (validTags != null)
                    {
                        tags.AddRange(validTags);
                    }
                }

                return tags;
            }
            set
            {
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        if (node.Component.Tags == null)
                        {
                            node.Component.Tags = new Core.Components.ComponentTags(node.Component.ID);
                        }

                        foreach (string tag in value)
                        {
                            node.Component.Tags.SetTag(tag, true);
                        }
                    }
                }
            }
        }

        public string SelectedComponentLocation
        {
            get
            {
                string name = "";
                var selectedNodes = m_componentNodeCollection.Where(o => o.IsSelected);
                if (selectedNodes != null)
                {
                    foreach (CLVComponentNode node in selectedNodes)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            name = node.Component.Assembly;
                        }
                        else if (name.Equals(node.Component.Assembly) == false)
                        {
                            name = "<Multiple Selection>";
                        }
                    }
                }

                return name;
            }
        }


        public string SelectedPath
        {
            get { return m_selectedPath; }
            set
            {
                m_selectedPath = value;
                if (string.IsNullOrWhiteSpace(SelectedPath) == false)
                {
                    CLVBaseNode node = GetNode(SelectedPath);
                    SelectedTag = (CLVTagNode)node;
                    OnPropertyChanged("SelectedPath");
                }
            }
        }

        public CLVTagNode SelectedTag
        {
            get { return m_selectedTag; }
            set
            {
                if (m_selectedTag != value)
                {
                    if (m_selectedTag != null)
                    {
                        var oldComponents = from component in SelectedTag.AllChildren
                                         where component is CLVComponentNode
                                         select component;
                        foreach (CLVComponentNode node in oldComponents)
                        {
                            node.IsSelected = false;
                        }
                    }

                    m_selectedTag = value;

                    if (m_selectedTag != null)
                    {
                        var components = from component in SelectedTag.AllChildren
                                         where component is CLVComponentNode
                                         select component;

                        Components = CollectionViewSource.GetDefaultView(components);
                        Components.SortDescriptions.Add(new SortDescription("Label", ListSortDirection.Ascending));
                    }
                    else
                    {
                        Components = null;
                    }

                    OnPropertyChanged("SelectedTag");
                    OnPropertyChanged("ComponentSectionName");
                }
            }
        }

        public string TagFilter
        {
            get { return m_tagFilter; }
            set
            {
                if (m_tagFilter != value)
                {
                    m_tagFilter = value;
                    m_lowercaseTagFilter = m_tagFilter.ToLower();
                    ApplyTagFilter(Tags);
                    OnPropertyChanged("TagFilter");
                }
            }
        }

        public ICollectionView Tags
        {
            get { return m_tags; }
            set
            {
                m_tags = value;
                ApplyTagFilter(m_tags);
                OnPropertyChanged("Tags");
            }
        }

        #endregion Properties

        #region Methods

        public CLVBaseNode GetNode(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Node path cannot be empty.", "path");

            CLVBaseNode foundNode = null;
            string[] splitPath = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Get the starting parent:
            if (splitPath.Length > 0)
            {
                foundNode = FindTopLevelTag(splitPath[0]);
                if (foundNode == null)
                {
                    foundNode = new CLVTagNode(splitPath[0]);
                    m_tagNodeCollection.Add(foundNode);

                    QueueRefilter();
                }

                foundNode = GetChildNode(splitPath, foundNode);
            }

            return foundNode;
        }

        public void RemoveTagIfEmpty(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag) == false)
            {
                var tags = tag.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var topLevel = FindTopLevelTag(tags[0]);
                if (tags.Length > 1)
                {
                    RemoveEmptySubTag(tags, 1, topLevel);
                }

                if (topLevel.AllChildren.Count == 0)
                {
                    m_tagNodeCollection.Remove(topLevel);
                }
            }
        }

        private void ApplyTagFilter(ICollectionView tags)
        {
            m_isRefilterQueued = false;
            if (tags != null)
            {
                tags.Filter = FilterTags;
                foreach (CLVBaseNode node in tags)
                {
                    ApplyTagFilter(node.Children);
                }
            }
        }

        private void BuildTagsHierarchy()
        {
            foreach (var component in m_componentsLibraryViewModel.ComponentsCollection)
            {
                var node = new CLVComponentNode(component, this);
                m_componentNodeCollection.Add(node);
                node.AddTag("All Components");
                //if (component.Tags != null)
                //{
                //    component.Tags.TagAdded += new EventHandler<Core.Components.TagChangedEventArgs>(Tags_TagAdded);
                //    component.Tags.TagRemoved += new EventHandler<Core.Components.TagChangedEventArgs>(Tags_TagRemoved);
                //}
                node.PropertyChanged += new PropertyChangedEventHandler(ComponentNodePropertyChanged);
            }
        }

        void ComponentNodePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                RebuildSelection();
            }
        }

        private bool FilterComponents(object item)
        {
            bool show = false;
            CLVComponentNode node = item as CLVComponentNode;
            if (node != null)
            {
                show = true;
                if (!string.IsNullOrWhiteSpace(m_lowercaseComponentFilter))
                {
                    show = node.Label.ToLower().Contains(m_lowercaseComponentFilter);
                }

                if (show == false)
                {
                    node.IsSelected = false;
                }
            }

            return show;
        }

        private bool FilterTags(object item)
        {
            bool show = false;
            CLVTagNode node = item as CLVTagNode;
            if (node != null)
            {
                show = true;
                if (!string.IsNullOrWhiteSpace(m_lowercaseTagFilter))
                {
                    show = node.Label.ToLower().Contains(m_lowercaseTagFilter);
                }

                // One last chance to change whether this tag is shown - if there are any children nodes..
                if (show == false && node.Children.IsEmpty == false)
                {
                    show = true;
                }

                if (show == false)
                {
                    node.IsSelected = false;
                }
            }

            return show;
        }

        private CLVBaseNode FindTopLevelTag(string tag)
        {
            CLVBaseNode found = null;
            foreach (CLVBaseNode node in m_tagNodeCollection)
            {
                if (node.Label == tag)
                {
                    found = node;
                    break;
                }
            }
            return found;
        }

        private CLVBaseNode GetChildNode(string[] splitPath, CLVBaseNode parent)
        {
            for (int i = 1; i < splitPath.Length; ++i)
            {
                var found = parent.AllChildren.FirstOrDefault(o => { return o.Label.Equals(splitPath[i], StringComparison.CurrentCultureIgnoreCase); });
                if (found == null)
                {
                    found = new CLVTagNode(splitPath[i]);
                    //m_tagNodeCollection.Add(found);
                    parent.AddChild(found);

                    QueueRefilter();
                }

                parent = found;
            }

            return parent;
        }

        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        void QueueRefilter()
        {
            if (!m_isRefilterQueued)
            {
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { ApplyTagFilter(Tags); }));
                m_isRefilterQueued = true;
            }
        }

        private void RebuildSelection()
        {
            OnPropertyChanged("SelectedComponentName");
            OnPropertyChanged("SelectedComponentAuthor");
            OnPropertyChanged("SelectedComponentDescription");
            OnPropertyChanged("SelectedComponentTags");
            OnPropertyChanged("SelectedComponentLocation");
        }

        private void RemoveEmptySubTag(string[] tags, int index, CLVBaseNode parent)
        {
            if (index < tags.Length)
            {
                var found = parent.AllChildren.FirstOrDefault(o => { return o.Label.Equals(tags[index], StringComparison.CurrentCultureIgnoreCase); });
                if (found != null)
                {
                    RemoveEmptySubTag(tags, index++, found);
                    if (found.AllChildren.Count == 0)
                    {
                        parent.RemoveChild(found);
                    }
                }
            }
        }

        void Tags_TagAdded(object sender, Core.Components.TagChangedEventArgs e)
        {
            QueueRefilter();
        }

        void Tags_TagRemoved(object sender, Core.Components.TagChangedEventArgs e)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { RemoveTagIfEmpty(e.Tag); }));
        }

        #endregion Methods
    }
}