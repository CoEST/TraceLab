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

// HERZUM SPRINT 5.3 TLAB-251
using System;
using TraceLab.Core.Components;
// END HERZUM SPRINT 5.3 TLAB-251

namespace TraceLab.Core.Experiments
{
    public abstract class ScopeNodeBase : CompositeComponentNode
    {
        public ScopeNodeBase(string id, SerializedVertexDataWithSize data)
            : base(id, data)
        {
        }

        protected ScopeNodeBase()
            : base()
        {
        }

        public override IExperiment Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
            }
        }

        private SerializedVertexDataWithSize m_dataWithSize;
        /// <summary>
        /// Gets Vertex Data. Helper property to avoid frequent casting of SerializedVertexData Data to SerializedVertexDataWithSize
        /// </summary>
        /// <value>
        /// The size of the data with.
        /// </value>
        public SerializedVertexDataWithSize DataWithSize
        {
            get
            {
                if (m_dataWithSize == null)
                {
                    //cast it
                    m_dataWithSize = Data as SerializedVertexDataWithSize;
                }
                return m_dataWithSize;
            }
        }

        protected virtual void InitializeComponentGraph()
        {
            InitializeComponentGraph(null);
        }

        protected virtual void InitializeComponentGraph(TraceLab.Core.Settings.Settings settings)
        {
            CompositeComponentMetadata.InitializeComponentGraph(this, settings);
        }

        // HERZUM SPRINT 5.3 TLAB-251
        public override bool IsModified
        {
            get 
            {
                ScopeBaseMetadata meta = Data.Metadata as ScopeBaseMetadata;
                if (meta.ComponentGraph == null)
                    return base.IsModified;
                else
                    return base.IsModified || meta.ComponentGraph.CalculateModification(); 
            }
            set
            {
                base.IsModified = value;
            }
        }

        public override void ResetModifiedFlag()
        {
            base.ResetModifiedFlag ();
            if (Data != null)  {
                ScopeBaseMetadata meta = Data.Metadata as ScopeBaseMetadata;
                if (meta.ComponentGraph != null){
                    meta.ComponentGraph.ResetModifiedFlag ();
                }
            }
        }
        // END HERZUM SPRINT 5.3 TLAB-251

    }
}
