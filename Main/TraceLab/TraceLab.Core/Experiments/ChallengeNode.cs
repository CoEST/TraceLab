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

// HERZUM SPRINT 2.0: TLAB-65 CLASS

using System;

namespace TraceLab.Core.Experiments
{
    public class ChallengeNode : ScopeNodeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public ChallengeNode(string id, SerializedVertexDataWithSize data) : base(id, data) {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ChallengeNode"/> class from being created.
        /// </summary>
        private ChallengeNode() : base() {
        }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ExperimentNode Clone()
        {
            var clone = new ChallengeNode();
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Copies data from another node to this
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(ExperimentNode other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            base.CopyFrom(other);
            InitializeComponentGraph();
            //references to decision node and exit node have to be set outside - as these nodes are also cloned
        }

    }
}

