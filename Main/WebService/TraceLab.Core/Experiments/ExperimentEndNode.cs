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

using TraceLab.Core.Components;

namespace TraceLab.Core.Experiments
{
    public class ExperimentEndNode : ExperimentNode
    {
        public ExperimentEndNode() : base("End", new SerializedVertexData()) {
            Data.Metadata = new EndNodeMetadata();
        }

        public ExperimentEndNode(string id, string label) : base(id, new SerializedVertexData())
        {
            Data.Metadata = new EndNodeMetadata(label);
        }

        public ExperimentEndNode(string id, SerializedVertexData nodeData) : base(id, nodeData) { }

        public override ExperimentNode Clone()
        {
            var clone = new ExperimentEndNode();
            clone.CopyFrom(this);

            return clone;
        }

        protected override void CopyFrom(ExperimentNode other)
        {
            base.CopyFrom(other);
        }
    }
}
