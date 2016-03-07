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


namespace TraceLab.Core.Components
{
    /// <summary>
    /// Interface IConfigurableAndIOSpecifiable defines if metadata has IOSpec and Configuration, as well as allow to set the label. 
    /// All types of nodes which have IOSpec and Config, and Label should implement this interface.
    /// </summary>
    public interface IConfigurableAndIOSpecifiable
    {
        /// <summary>
        /// Gets or sets the IO spec.
        /// </summary>
        /// <value>
        /// The IO spec.
        /// </value>
        IOSpec IOSpec
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the config wrapper.
        /// </summary>
        /// <value>
        /// The config wrapper.
        /// </value>
        ConfigWrapper ConfigWrapper
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the metadata definition
        /// </summary>
        /// <value>The metadata definition.</value>
        MetadataDefinition MetadataDefinition 
        {
            get;
        }
    }
}
