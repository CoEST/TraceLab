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
using System.Windows.Data;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using TraceLab.UI.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using TraceLab.Core.Components;
using System.Collections.Specialized;
using System.ComponentModel;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// The control represents the checkbox that allows adding filters directly from the component node info.
    /// </summary>
    public class IOSpecFilterCheckBox : CheckBox
    {
        #region Constructor

        public IOSpecFilterCheckBox()
        {
            Checked += new RoutedEventHandler(OnCheckboxChecked);
            Unchecked += new RoutedEventHandler(OnCheckboxUnchecked);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Called when checkbox is unchecked. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            if (IoSpecFilters != null)
            {
                var ioItem = ItemDefinition as IOItemDefinition;

                if (ioItem != null)
                {
                    //find matching filter by type
                    var filter = IoSpecFilters.FirstOrDefault(item => item.FilterByDataType.Equals(default(KeyValuePair<string, string>)) == false
                                                                && item.FilterByDataType.Value.Equals(ioItem.Type));

                    //it can be null if the filter has been totally removed from filters (which triggers uncheck event)
                    if (filter != null)
                    {
                        if (ioItem.IOType == TraceLabSDK.IOSpecType.Input)
                        {
                            filter.RequiresInput = false;
                        }
                        else if (ioItem.IOType == TraceLabSDK.IOSpecType.Output)
                        {
                            filter.RequiresOutput = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when checkbox is checked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCheckboxChecked(Object sender, RoutedEventArgs e)
        {
            if (IoSpecFilters != null)
            {
                string itemType = String.Empty;
                if (ItemDefinition != null) itemType = ItemDefinition.Type;
                
                //try check first if the filter has been already added... (assure that FilterByDataType was set - there might be filter that has not been yet set)
                var filter = IoSpecFilters.FirstOrDefault(item => item.FilterByDataType.Equals(default(KeyValuePair<string, string>)) == false 
                                                          && item.FilterByDataType.Value.Equals(itemType));
                if (filter != null)
                {
                    if (ItemDefinition.IOType == TraceLabSDK.IOSpecType.Input) filter.RequiresInput = true;
                    if (ItemDefinition.IOType == TraceLabSDK.IOSpecType.Output) filter.RequiresOutput = true;
                }
                else
                {
                    //otherwise add new filter
                    IOSpecFilter newfilter = null;
                    newfilter = IOSpecFilter.CreateIOSpecFilter(ItemDefinition, IoSpecFilters, AvailableFilteringTypes);
                    
                    IoSpecFilters.Add(newfilter);
                }
            }
        }

        /// <summary>
        /// Called when IoSpecFilters dependency property was updated.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIoSpecFiltersPropertyUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IOSpecFilterCheckBox iospecFilterToBool = (IOSpecFilterCheckBox)d;
            var oldFilters = (ObservableCollection<IOSpecFilter>)e.OldValue;
            var newFilters = (ObservableCollection<IOSpecFilter>)e.NewValue;
            
            if (oldFilters != null)
            {
                //detach the event handler
                oldFilters.CollectionChanged -= iospecFilterToBool.IoSpecFiltersCollectionChanged;
            }

            if (newFilters != null)
            {
                //register to events
                newFilters.CollectionChanged += iospecFilterToBool.IoSpecFiltersCollectionChanged;
                iospecFilterToBool.CheckMatch(newFilters);
            }
        }

        /// <summary>
        /// When collection of filters has changed, attach or detach event handlers from filters.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void IoSpecFiltersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var filters = (ObservableCollection<IOSpecFilter>)sender;
            if (filters != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (IOSpecFilter filter in e.NewItems)
                        {
                            filter.PropertyChanged += new PropertyChangedEventHandler(OnIoSpecFilterChanged);
                        }
                        CheckMatch(filters);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (IOSpecFilter filter in e.OldItems)
                        {
                            filter.PropertyChanged -= new PropertyChangedEventHandler(OnIoSpecFilterChanged);
                        }
                        CheckMatch(filters);
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
        }

        /// <summary>
        /// Called when specific IOSpecFilter has changed. The handler check the match for the checkbox, and determines, if checkbox should be checked or not.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnIoSpecFilterChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckMatch(IoSpecFilters);
        }

        #endregion

        #region Dependency Properties

        public static DependencyProperty ItemDefinitionProperty = DependencyProperty.Register("ItemDefinition", typeof(IOItemDefinition), typeof(IOSpecFilterCheckBox));

        /// <summary>
        /// Gets or sets the item definition.
        /// </summary>
        /// <value>
        /// The item definition.
        /// </value>
        public IOItemDefinition ItemDefinition
        {
            get { return (IOItemDefinition)GetValue(ItemDefinitionProperty); }
            set { SetValue(ItemDefinitionProperty, value); }
        }

        public static DependencyProperty IoSpecFiltersProperty = DependencyProperty.Register("IoSpecFilters",
                                                                                            typeof(ObservableCollection<IOSpecFilter>),
                                                                                            typeof(IOSpecFilterCheckBox),
                                                                                            new FrameworkPropertyMetadata(null,
                                                                                                    FrameworkPropertyMetadataOptions.AffectsArrange,
                                                                                                    new PropertyChangedCallback(OnIoSpecFiltersPropertyUpdated)));
        /// <summary>
        /// Gets or sets the io spec filters.
        /// The access to all current io spec filters is needed to determine if the checkbox should be marked as checked or not.
        /// </summary>
        /// <value>
        /// The io spec filters.
        /// </value>
        public ObservableCollection<IOSpecFilter> IoSpecFilters
        {
            get { return (ObservableCollection<IOSpecFilter>)GetValue(IoSpecFiltersProperty); }
            set { SetValue(IoSpecFiltersProperty, value); }
        }

        public static DependencyProperty AvailableFilteringTypesProperty = DependencyProperty.Register("AvailableFilteringTypes",
                                                                                                    typeof(IEnumerable<KeyValuePair<string, string>>),
                                                                                                    typeof(IOSpecFilterCheckBox));

        /// <summary>
        /// Gets or sets the available filtering types.
        /// The property is needed to have an access to all availalble filtering types in the components library for constructing
        /// new IOSpecFilters, when the checkbox is checked.
        /// The keys represent user friendly type names, and their values are a full type names.
        /// </summary>
        /// <value>
        /// The available filtering types.
        /// </value>
        public IEnumerable<KeyValuePair<string, string>> AvailableFilteringTypes
        {
            get { return (IEnumerable<KeyValuePair<string, string>>)GetValue(AvailableFilteringTypesProperty); }
            set { SetValue(AvailableFilteringTypesProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the ItemDefinition of the current checkbox matches any of the existing filters in the given collection of filters.
        /// If match is found the checkbox is marked as checked, otherwise it is unchecked. 
        /// </summary>
        /// <param name="filters">The filters.</param>
        private void CheckMatch(ObservableCollection<IOSpecFilter> filters)
        {
            //retrieve the item
            bool match = false;
            if (ItemDefinition.IOType == TraceLabSDK.IOSpecType.Input)
            {
                match = filters.Any(item => item.IsEmpty == false && item.FilterByDataType.Value.Equals(ItemDefinition.Type) && item.RequiresInput == true);
            }
            else if (ItemDefinition.IOType == TraceLabSDK.IOSpecType.Output)
            {
                match = filters.Any(item => item.IsEmpty == false && item.FilterByDataType.Value.Equals(ItemDefinition.Type) && item.RequiresOutput == true);
            }

            IsChecked = match;
        }

        #endregion
    }
}
