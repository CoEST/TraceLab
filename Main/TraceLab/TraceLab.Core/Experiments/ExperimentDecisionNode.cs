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


namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Represents decision node in the experiment
    /// </summary>
    public class ExperimentDecisionNode : ExperimentNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentDecisionNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        public ExperimentDecisionNode(string id, SerializedVertexData data) : base(id, data)
        {
            ID = id;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ExperimentDecisionNode"/> class from being created.
        /// </summary>
        private ExperimentDecisionNode() : base()
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override ExperimentNode Clone()
        {
            var clone = new ExperimentDecisionNode();
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="other">The other.</param>
        protected override void CopyFrom(ExperimentNode other)
        {
            base.CopyFrom(other);
        }
    }
}
