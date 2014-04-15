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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;
using TraceLab.UI.WPF.Commands;
using TraceLabSDK.PackageSystem;

namespace TraceLab.UI.WPF.ViewModels
{

    class ComponentsLibraryViewModelWrapper : INotifyPropertyChanged, IGetCLNode
    {
        #region Fields

        private ComponentLibraryViewModel m_componentsLibraryViewModel;

        private bool m_isRescanning;
        private System.Windows.Threading.Dispatcher m_dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
        private System.Windows.Threading.DispatcherTimer m_searchTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Input);
        private string m_componentsCollectionViewFilter = string.Empty;
        private string m_lowercaseFilter = string.Empty;
        private ObservableCollection<CLVBaseNode> m_nodeCollection = new ObservableCollection<CLVBaseNode>();
        private Dictionary<string, CLVBaseNode> m_nodes = new Dictionary<string, CLVBaseNode>();
        private ICommand m_openOrganizer;
        private ICommand m_rescan;
        private Delegate m_setPropertyMethod;
        private ObservableCollection<IOSpecFilter> m_ioSpecFilters = new ObservableCollection<IOSpecFilter>();
        private ICommand m_addIoSpecFilter;
        private ICommand m_removeIoSpecFilter;

        #endregion Fields

        #region Constructors

        public ComponentsLibraryViewModelWrapper(ComponentLibraryViewModel componentsLibraryViewModel)
        {
            if (componentsLibraryViewModel == null)
                throw new ArgumentNullException("componentsLibraryViewModel", "Wrapped component view model cannot be null");

            m_componentsLibraryViewModel = componentsLibraryViewModel;
            m_componentsLibraryViewModel.PropertyChanged += m_componentsLibraryViewModel_PropertyChanged;
            m_componentsLibraryViewModel.Rescanned += m_componentsLibraryViewModel_Rescanned;
            m_componentsLibraryViewModel.Rescanning += new EventHandler(m_componentsLibraryViewModel_Rescanning);
            m_isRescanning = m_componentsLibraryViewModel.IsRescanning;

            Rescan = new DelegateCommand(RescanFunc, CanRescanFunc);
            OpenOrganizer = new DelegateCommand(OpenOrganizerFunc);
            m_setPropertyMethod = new Action<PropertyInfo, object>(SetProperty);

            ComponentsCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(VisibleNodes);
            //ComponentsCollectionView.SortDescriptions.Add(new SortDescription("Label", ListSortDirection.Ascending));

            m_searchTimer.Tick += new EventHandler(m_searchTimer_Tick);
            m_searchTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            AddIoSpecFilter = new DelegateCommand(AddIoSpecFilterFunc);
            RemoveIoSpecFilter = new DelegateCommand(RemoveIoSpecFilterFunc);

            IoSpecFilters.CollectionChanged += IoSpecFiltersCollectionChanged;

            // Don't build the hierarchy unless the library is done scanning.
            if (m_componentsLibraryViewModel.IsRescanning == false)
            {
                BuildTagsHierarchy();
            }

            if (Experiment != null)
            {
                Experiment.References.CollectionChanged += References_CollectionChanged;
            }
        }

        /// <summary>
        /// If references has changed rebuild tags hierarchy.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void References_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_componentsLibraryViewModel.IsRescanning == false)
            {
                RebuildTagsHierarchy();
            }
        }

        #endregion Constructors

        #region Enumerations

        public enum FilterType
        {
            Tag,
            Component
        }

        #endregion Enumerations

        #region Events

        public event EventHandler Rescanned;

        #endregion Events

        #region Event Handlers

        private void m_componentsLibraryViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyInfo targetInfo = typeof(ComponentsLibraryViewModelWrapper).GetProperty(e.PropertyName);
            if (targetInfo != null)
            {
                if (targetInfo.CanWrite)
                {
                    PropertyInfo sourceInfo = typeof(ComponentLibraryViewModel).GetProperty(e.PropertyName);
                    object value = sourceInfo.GetValue(m_componentsLibraryViewModel, null);

                    m_dispatcher.Invoke(m_setPropertyMethod, targetInfo, value);
                }
                else
                {
                    // If we can't write to it, it's probably a forwarding-only property, so we just want to say that it's changed.
                    NotifyPropertyChanged(e.PropertyName);
                }
            }
#if DEBUG
            else
            {
                System.Diagnostics.Debug.WriteLine("ComponentLibraryViewModel has unwrapped property: " + e.PropertyName);
            }
#endif
        }

        void m_searchTimer_Tick(object sender, System.EventArgs e)
        {
            ComponentsCollectionView.Filter = ApplyFilter;

            m_searchTimer.Stop();
        }

        void m_componentsLibraryViewModel_Rescanning(object sender, System.EventArgs e)
        {
        }

        void m_componentsLibraryViewModel_Rescanned(object sender, System.EventArgs e)
        {
            var method = new Action(() =>
            {
                RebuildTagsHierarchy();

                // Reapply the filter so that the new nodes appear in the view.
                ComponentsCollectionView.Filter = ApplyFilter;
            });
            m_dispatcher.BeginInvoke(method, System.Windows.Threading.DispatcherPriority.Input, null);

            //also fire event
            if (Rescanned != null)
                Rescanned(this, new System.EventArgs());
        }

        #endregion

        #region Properties

        public IExperiment Experiment
        {
            get { return m_componentsLibraryViewModel.Experiment; }
        }

        public IEnumerable<MetadataDefinition> ComponentsCollection
        {
            get
            {
                return m_componentsLibraryViewModel.ComponentsCollection;
            }
        }

        public ListCollectionView ComponentsCollectionView
        {
            get;
            private set;
        }

        public string ComponentsCollectionViewFilter
        {
            get
            {
                return m_componentsCollectionViewFilter;
            }
            set
            {
                if (m_componentsCollectionViewFilter != value)
                {
                    m_searchTimer.Stop();

                    m_componentsCollectionViewFilter = value;
                    m_lowercaseFilter = m_componentsCollectionViewFilter.ToLower();
                    NotifyPropertyChanged("ComponentsCollectionViewFilter");

                    m_searchTimer.Start();
                }
            }
        }

        public FilterType FilterBy
        {
            get { return FilterType.Component; }
        }

        public ICommand OpenOrganizer
        {
            get { return m_openOrganizer; }
            set
            {
                m_openOrganizer = value;
                NotifyPropertyChanged("OpenOrganizer");
            }
        }

        public ICommand Rescan
        {
            get
            {
                return m_rescan;
            }
            private set
            {
                m_rescan = value;
                NotifyPropertyChanged("Rescan");
            }
        }

        public IEnumerable<CLVBaseNode> VisibleNodes
        {
            get { return m_nodeCollection; }
        }
        
        public bool IsRescanning
        {
            get { return m_isRescanning; }
            set
            {
                if (m_dispatcher != System.Windows.Threading.Dispatcher.CurrentDispatcher)
                {
                    var method = new Action<bool>(SetIsRescanning);
                    m_dispatcher.Invoke(method, value);
                }
                else
                {
                    SetIsRescanning(value);
                }
            }
        }

        public ObservableCollection<IPackageReference> References
        {
            get { return m_componentsLibraryViewModel.Experiment.References; }
        }

        #endregion Properties

        #region Methods

        public void ClearLoadErrors()
        {
            m_componentsLibraryViewModel.ClearLoadErrors();
        }

        void SetProperty(PropertyInfo property, object value)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            property.SetValue(this, value, null);
        }

        public CLVBaseNode GetNode(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Node path cannot be empty.", "path");

            CLVBaseNode foundNode = null;
            string[] splitPath = path.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);

            // Get the starting parent:
            if (splitPath.Length > 0)
            {
                m_nodes.TryGetValue(splitPath[0], out foundNode);
                if (foundNode == null)
                {
                    foundNode = new CLVTagNode(splitPath[0]);
                    m_nodes.Add(splitPath[0], foundNode);
                    m_nodeCollection.Add(foundNode);
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
                if (topLevel != null && tags.Length > 1)
                {
                    RemoveEmptySubTag(tags, 1, topLevel);
                }

                if (topLevel != null && topLevel.AllChildren.Count == 0)
                {
                    m_nodeCollection.Remove(topLevel);
                    m_nodes.Remove(tags[0]);
                }
            }
        }

        internal bool ApplyFilter(object item)
        {
            CLVBaseNode metadataDefinition = item as CLVBaseNode;
            if (metadataDefinition == null)
                return false;

            return !ShouldFilterNode(metadataDefinition, m_lowercaseFilter) && !ShouldFilterNodeBasedOnIoSpecFilter(metadataDefinition);
        }

        private void BuildTagsHierarchy()
        {
            if (Experiment != null)
            {
                var referenceCollection = new CLVReferenceContainerNode(Experiment.References);
                m_nodeCollection.Add(referenceCollection);
            }

            foreach (var component in ComponentsCollection)
            {
                AddNodeTag(component);
            }
        }

        private void RebuildTagsHierarchy()
        {
            m_nodeCollection.Clear();
            m_nodes.Clear();
            BuildTagsHierarchy();
        }

        private void AddNodeTag(MetadataDefinition component)
        {
            var node = new CLVComponentNode(component, this);
            node.AddTag("All Components");
        }

        public void AddReplaceCompositeComponentMetadataDefinition(CompositeComponentMetadataDefinition metadataDefinition) 
        {
            var library = (ComponentsLibrary)m_componentsLibraryViewModel;

            //if there is old composite component definition that had the same source, remove it from library
            CompositeComponentMetadataDefinition oldMetadataDefinition;
            if (library.TryFindCompositeComponentMetadataDefinition(metadataDefinition.Assembly, out oldMetadataDefinition))
            {
                RemoveFromCLVHierarchy(oldMetadataDefinition, m_nodeCollection);
                library.Remove(oldMetadataDefinition);
            }

            library.Add(metadataDefinition);
            AddNodeTag(metadataDefinition);
        }

        /// <summary>
        /// Removes defintion from CLV hierarchy. (from the view)s
        /// </summary>
        /// <param name="metadataDefinition">The metadata definition.</param>
        /// <param name="nodeCollection">The node collection.</param>
        /// <returns>true if metadefinition was found and removed from component list view</returns>
        private static bool RemoveFromCLVHierarchy(MetadataDefinition metadataDefinition, IEnumerable<CLVBaseNode> nodeCollection)
        {
            foreach (CLVBaseNode node in nodeCollection)
            {
                var tagNode = node as CLVTagNode;
                if (tagNode != null)
                { 
                    bool removed = RemoveFromCLVHierarchy(metadataDefinition, tagNode.AllChildren);
                    if (removed) return true;
                }
                else
                {
                    var comp = node as CLVComponentNode;
                    if (comp != null)
                    {
                        if (comp.Component == metadataDefinition)
                        {
                            comp.Remove();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CanRescanFunc(object param)
        {
            return (m_componentsLibraryViewModel.CanRescan == false || IsRescanning) ? false : true;
        }

        CLVBaseNode FindTopLevelTag(string tag)
        {
            CLVBaseNode found;
            m_nodes.TryGetValue(tag, out found);
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
                    parent.AddChild(found);
                }

                parent = found;
            }

            return parent;
        }

        private bool MatchLabel(CLVBaseNode node, string filter)
        {
            if(node == null)
                throw new ArgumentNullException("node");

            string label = node.Label.ToLower();
            return label.Contains(filter);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        private void OpenOrganizerFunc(object param)
        {
            var organizerVM = new ComponentLibraryOrganizerVM(m_componentsLibraryViewModel);
            var organizerView = new TraceLab.UI.WPF.Views.ComponentLibraryOrganizer();
            organizerView.DataContext = organizerVM;

            var organizerWindow = new System.Windows.Window();
            organizerWindow.Content = organizerView;
            foreach(System.Windows.Window window in System.Windows.Application.Current.Windows)
            {
                if(window.IsKeyboardFocusWithin)
                {
                    organizerWindow.Owner = window;
                }
            }
            organizerWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            organizerWindow.ShowActivated = true;
            organizerWindow.Title = "Component Tags Organizer";
            organizerWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/TraceLab.UI.WPF;component/Resources/Icon_Organizer16.png"));
            organizerWindow.ShowDialog();
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

        private void RescanFunc(object param)
        {
            m_componentsLibraryViewModel.Rescan();
        }

        private bool ShouldFilterNode(CLVBaseNode node, string filter)
        {
            bool shouldFilter = false;
            CLVTagNode tagNode = node as CLVTagNode;
            CLVComponentNode componentNode = node as CLVComponentNode;
            if (tagNode != null)
            {
                if (FilterBy == FilterType.Component)
                {
                    tagNode.Children.Filter = ApplyFilter;
                    if (tagNode.Children.IsEmpty)
                    {
                        shouldFilter = true;
                    }
                }
                else
                {
                    shouldFilter = !MatchLabel(tagNode, filter);
                }
            }
            else if(componentNode != null)
            {
                if (FilterBy == FilterType.Component)
                {
                    shouldFilter = !MatchLabel(componentNode, filter);
                }
            }

            return shouldFilter;
        }

        private void SetIsRescanning(bool value)
        {
            if (m_isRescanning != value)
            {
                m_isRescanning = value;
                NotifyPropertyChanged("IsRescanning");
                ((DelegateCommand)Rescan).RaiseCanExecuteChanged();
            }
        }

        #endregion Methods


        public static explicit operator ComponentsLibrary(ComponentsLibraryViewModelWrapper wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException("wrapper");

            return (ComponentsLibrary)wrapper.m_componentsLibraryViewModel;
        }

        #region IoSpecFilters 

        /// <summary>
        /// Gets the available types which components library can be filter by.
        /// The key is user friendly type name, and its value is a full type name.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> AvailableFilterTypes
        {
            get
            {

                return m_componentsLibraryViewModel.AvailableFilterTypes;
            }
        }

        /// <summary>
        /// Gets the collection of io spec filters.
        /// </summary>
        public ObservableCollection<IOSpecFilter> IoSpecFilters
        {
            get { return m_ioSpecFilters; }
        }

        /// <summary>
        /// The command responsible for adding the new IoSpecFilter
        /// </summary>
        /// <value>
        /// The add io spec filter.
        /// </value>
        public ICommand AddIoSpecFilter
        {
            get { return m_addIoSpecFilter; }
            set
            {
                if (m_addIoSpecFilter != value)
                {
                    m_addIoSpecFilter = value;
                    NotifyPropertyChanged("AddIoSpecFilter");
                }
            }
        }

        /// <summary>
        /// The method adds the empty io spec filter with currently available types.
        /// </summary>
        /// <param name="param">The param.</param>
        private void AddIoSpecFilterFunc(object param)
        {
            m_ioSpecFilters.Add(IOSpecFilter.CreateEmptyIOSpecFilter(IoSpecFilters, AvailableFilterTypes));
        }

        /// <summary>
        /// The command responsible for removing the new IoSpecFilter
        /// </summary>
        /// <value>
        /// The remove io spec filter.
        /// </value>
        public ICommand RemoveIoSpecFilter
        {
            get { return m_removeIoSpecFilter; }
            set
            {
                if (m_removeIoSpecFilter != value)
                {
                    m_removeIoSpecFilter = value;
                    NotifyPropertyChanged("RemoveIoSpecFilter");
                }
            }
        }

        /// <summary>
        /// The method removes the io spec filter from the list of current filters.
        /// </summary>
        /// <param name="param">The param.</param>
        private void RemoveIoSpecFilterFunc(object param)
        {
            IOSpecFilter iospecFilter = param as IOSpecFilter;
            if (iospecFilter != null)
            {
                m_ioSpecFilters.Remove(iospecFilter);
            }
        }

        /// <summary>
        ///  When collection of filters has changed, attach or detach event handlers from filters,
        ///  apply filters to components collection, and refresh the available types of all filter comboboxes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void IoSpecFiltersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IOSpecFilter filter in e.NewItems)
                    {
                        filter.PropertyChanged += new PropertyChangedEventHandler(OnIoSpecFilterChanged);
                        //if filter is not empty, meaning if the filering type is selected and at least one of input/output checkboxes is checked.
                        if (filter.IsEmpty == false)
                        {
                            ComponentsCollectionView.Filter = ApplyFilter;
                        }
                        RefreshFilterBoxesAvailableTypes(filter);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IOSpecFilter filter in e.OldItems)
                    {
                        filter.PropertyChanged -= new PropertyChangedEventHandler(OnIoSpecFilterChanged);
                        //if filter is not empty, meaning if the filering type is selected and at least one of input/output checkboxes is checked.
                        if (filter.IsEmpty == false) ComponentsCollectionView.Filter = ApplyFilter;
                        RefreshFilterBoxesAvailableTypes(filter);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when one of io spec filter changed. If the property that changed was AvailableFilteringTypes ignore it.
        /// If FilterByDataType changed refresh other checkboxes lists of available types.
        /// Also apply the filter to collection of visibleList components.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnIoSpecFilterChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AvailableFilteringTypes") return; //do not nothing 

            // on filter by data type field change refresh all filtering comboboxes
            if (e.PropertyName == "FilterByDataType")
            {
                var currentChangedFilter = sender as IOSpecFilter;
                RefreshFilterBoxesAvailableTypes(currentChangedFilter);
            }

            //apply new filters in all other cases (couse other properties like RequiresInput or RequiresOutput might have been changed
            ComponentsCollectionView.Filter = ApplyFilter;
        }

        /// <summary>
        /// Refreshes the list of currently available types for all filtering combo boxes. 
        /// Does not refreshes the triggering filter - the filter that raised the refresh event. 
        /// (otherwise it causes incorrect behavior as it tries to bind to collection, that just refreshed.)
        /// </summary>
        /// <param name="triggerFilter">The trigger filter.</param>
        private void RefreshFilterBoxesAvailableTypes(IOSpecFilter triggerFilter)
        {
            //get list of currently selected types
            List<KeyValuePair<string, string>> selectedTypes = new List<KeyValuePair<string, string>>();
            foreach (IOSpecFilter filter in IoSpecFilters)
            {
                selectedTypes.Add(filter.FilterByDataType);
            }
            //refresh the available types for all comboboxes
            foreach (IOSpecFilter filter in IoSpecFilters)
            {
                //refresh all filters except trigger filter
                if (filter.Equals(triggerFilter) == false)
                {
                    //update current available types - don't exlude the current filter selected type
                    filter.RefreshCurrentlyAvailableFilterTypes(IoSpecFilters, AvailableFilterTypes);
                }
            }
        }

        /// <summary>
        /// The method checks if node should be filtered based on io spec filter. It is used in ApplyFilter.
        /// If node does not have matching input/outputs it should be filtered out.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>true, if node should be filtered out, false, otherwise</returns>
        private bool ShouldFilterNodeBasedOnIoSpecFilter(CLVBaseNode node)
        {
            bool shouldFilter = false;

            //apply only to component nodes (not tag nodes)
            CLVComponentNode componentNode = node as CLVComponentNode;
            if (componentNode != null)
            {
                //if there are any io spec filters
                if (IoSpecFilters.Count > 0)
                {
                    //get component or composite component definition
                    IMetadataWithIOSpecDefinition compDef = componentNode.Component as IMetadataWithIOSpecDefinition;

                    // check each io spec filter
                    foreach (IOSpecFilter filter in IoSpecFilters)
                    {
                        //that is not empty
                        if (filter.IsEmpty == false)
                        {
                            if (compDef != null)
                            {
                                //proceed with filtering
                                if (filter.RequiresInput)
                                {
                                    //compare the full type (not friendly name)
                                    bool match = compDef.IOSpecDefinition.Input.Any(item => item.Value.Type.Equals(filter.FilterByDataType.Value));
                                    if (match == false)
                                    {
                                        shouldFilter = true;
                                        break;
                                    }
                                }
                                if (filter.RequiresOutput)
                                {
                                    //compare the full type (not friendly name)
                                    bool match = compDef.IOSpecDefinition.Output.Any(item => item.Value.Type.Equals(filter.FilterByDataType.Value));
                                    if (match == false)
                                    {
                                        shouldFilter = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //if it was not component metadefinition automatically filter it out (since it has not any input, output)
                                //we check it at this stage, to not exclude it, when the filters are empty
                                shouldFilter = true;
                                break; //just return 
                            }
                        } //filter.IsEmpty
                    } // foreach loop
                } // count > 0
            } // component != null

            return shouldFilter;
        }

        #endregion
    }
}