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
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Components
{
    [Serializable]
    public class IOSpecDefinition
    {
        public IOSpecDefinition()
        {
            Input = new Dictionary<string, IOItemDefinition>();
            Output = new Dictionary<string, IOItemDefinition>();
        }

        public IOSpecDefinition(IOSpecDefinition iOSpec)
        {
            Input = new Dictionary<string, IOItemDefinition>(iOSpec.Input);
            Output = new Dictionary<string, IOItemDefinition>(iOSpec.Output);
        }
                
        private Dictionary<string, IOItemDefinition> m_input;

        public Dictionary<string, IOItemDefinition> Input
        {
            get
            {
                return m_input;
            }
            private set
            {
                this.m_input = value;
            }
        }

        private Dictionary<string, IOItemDefinition> m_output;

        public Dictionary<string, IOItemDefinition> Output
        {
            get
            {
                return m_output;
            }
            private set
            {
                this.m_output = value;
            }
        }

        #region Equals & HashCode

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            IOSpecDefinition other = obj as IOSpecDefinition;
            if (other == null)
                return false;

            bool isEqual = true;

            isEqual &= CollectionsHelper.DictionaryContentEquals<string, IOItemDefinition>(Output, other.Output);
            isEqual &= CollectionsHelper.DictionaryContentEquals<string, IOItemDefinition>(Input, other.Input);
            
            return isEqual;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int outputHash = Output.GetHashCode();
            int inputHash = Input.GetHashCode();
            
            return outputHash ^ inputHash;
        }

        #endregion
    }
}
