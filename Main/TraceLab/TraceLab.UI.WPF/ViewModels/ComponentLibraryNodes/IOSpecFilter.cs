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
using System.ComponentModel;
using TraceLab.Core.Components;
using System.Collections.ObjectModel;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// IOSpecFilter is the class that defines the single filter on input/output of the components.
    /// The components library will filter out components, which IOSpec does not match the IOSpecFilter.
    /// </summary>
    public class IOSpecFilter : INotifyPropertyChanged
    {
        #region Constructor

        private IOSpecFilter() { }

        private IOSpecFilter(string filterByDataType, string filterByDataTypeFriendlyName, bool requiresInput, bool requiresOutput)
        {
            FilterByDataType = new KeyValuePair<string, string>(filterByDataTypeFriendlyName, filterByDataType);
            RequiresInput = requiresInput;
            RequiresOutput = requiresOutput;
        }

        #endregion

        #region Methods

        #region Create methods

        public static IOSpecFilter CreateEmptyIOSpecFilter(ObservableCollection<IOSpecFilter> currentIoSpecFilters,
                                                           IEnumerable<KeyValuePair<string, string>> allAvailableTypes)
        {
            var newFilter = new IOSpecFilter();
            newFilter.RefreshCurrentlyAvailableFilterTypes(currentIoSpecFilters, allAvailableTypes);
            return newFilter;
        }
        
        public static IOSpecFilter CreateIOSpecFilter(IOItemDefinition item,
                                              ObservableCollection<IOSpecFilter> currentIoSpecFilters,
                                              IEnumerable<KeyValuePair<string, string>> allAvailableTypes)
        {
            bool requiresOutput = false;
            bool requiresInput = false;
            
            if (item.IOType == TraceLabSDK.IOSpecType.Input)
            {
                requiresInput = true;
            }
            else if (item.IOType == TraceLabSDK.IOSpecType.Output)
            {
                requiresOutput = true;
            }

            var newFilter = new IOSpecFilter(item.Type, item.FriendlyType, requiresInput, requiresOutput);
            newFilter.RefreshCurrentlyAvailableFilterTypes(currentIoSpecFilters, allAvailableTypes);
            return newFilter;
        }

        #endregion

        /// <summary>
        /// Refreshes the list of currently available types for this IOSpecFilter, excluding the one that have been already selected by other comboboxes.
        /// List includes the given item type, if the FilterByDataType is set, ie, it does not exclude that type.
        /// </summary>
        /// <param name="currentIoSpecFilters">The current io spec filters.</param>
        /// <param name="allAvailableTypes">All available types.</param>
        public void RefreshCurrentlyAvailableFilterTypes(ObservableCollection<IOSpecFilter> currentIoSpecFilters,
                                                    IEnumerable<KeyValuePair<string, string>> allAvailableTypes)
        {
            List<KeyValuePair<string, string>> selectedTypes = new List<KeyValuePair<string, string>>();
            foreach (IOSpecFilter filter in currentIoSpecFilters)
            {
                selectedTypes.Add(filter.FilterByDataType);
            }

            if (FilterByDataType.Equals(default(KeyValuePair<string, string>)))
            {
                AvailableFilteringTypes = allAvailableTypes.Where(filterType => !selectedTypes.Any(selectedType => selectedType.Equals(filterType)));
            }
            else
            {
                AvailableFilteringTypes = allAvailableTypes.Where(filterType => !selectedTypes.Any(selectedType => selectedType.Equals(filterType))
                                                    || filterType.Equals(FilterByDataType));
            }
        }

        #endregion

        #region Properties

        private IEnumerable<KeyValuePair<string, string>> m_availableFilteringTypes;
        /// <summary>
        /// Gets the available types which components library can be filter by.
        /// The key is user friendly type name, and its value is a full type name.
        /// The model view wrapper returns only subset of all available types - it removes available types 
        /// if they have been added as filters alraedy
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> AvailableFilteringTypes
        {
            get
            {
                return m_availableFilteringTypes;
            }
            private set
            {
                m_availableFilteringTypes = value;
                NotifyPropertyChanged("AvailableFilteringTypes");
            }
        }

        private KeyValuePair<string, string> m_filterByDataType;
        /// <summary>
        /// Gets or sets the type of the input/output that components must have.
        /// The key is user friendly type name, and its value is a full type name.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        public KeyValuePair<string, string> FilterByDataType
        {
            get { return m_filterByDataType; }
            set 
            {
                if (m_filterByDataType.Equals(value) == false)
                {
                    m_filterByDataType = value;
                    NotifyPropertyChanged("FilterByDataType");
                }
            }
        }

        private bool m_requiresInput;
        /// <summary>
        /// Gets or sets a value indicating whether components must have input of the specified Type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if components must have input of the specified Type; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresInput
        {
            get { return m_requiresInput; }
            set 
            {
                if (m_requiresInput != value)
                {
                    m_requiresInput = value;
                    NotifyPropertyChanged("RequiresInput");
                }
            }
        }

        private bool m_requiresOutput;
        /// <summary>
        /// Gets or sets a value indicating whether components must have output of the specified Type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if components must have output of the specified Type; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresOutput
        {
            get { return m_requiresOutput; }
            set 
            {
                if (m_requiresOutput != value)
                {
                    m_requiresOutput = value;
                    NotifyPropertyChanged("RequiresOutput");
                }
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether this filter is empty, meaning the filtering type has not been selected, so it would not couse any effect.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this filter is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return FilterByDataType.Equals(default(KeyValuePair<string, string>)) || (RequiresInput == false && RequiresOutput == false);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
