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

namespace TraceLab.Core.Components
{
    /// <summary>
    /// The class represents the definition of decision component
    /// </summary>
    [Serializable]
    public class DecisionMetadataDefinition : MetadataDefinition
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="DecisionMetadataDefinition"/> class from being created.
        /// </summary>
        private DecisionMetadataDefinition() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionMetadataDefinition"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public DecisionMetadataDefinition(string id) : base(id) 
        {
            Tags = new ComponentTags(id);
        }

        public const string DecisionGuid = @"dddf79bd-ceec-4681-b70e-ee5d9ffeb3a4";

        public const string GotoDecisionGuid = @"19a4c166-c30c-454b-bb6c-49c3e932091f";
    }
}
