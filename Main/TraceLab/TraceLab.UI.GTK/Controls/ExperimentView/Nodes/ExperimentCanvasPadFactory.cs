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

// HERZUM SPRINT 2.0 CLASS TLAB-136

using System;

using System.Collections.Generic;
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;

// HERZUM SPRINT 2.4 TLAB-157
using Gdk;
using Gtk;
using MonoDevelop.Components.Docking;
// END HERZUM SPRINT 2.4 TLAB-157

namespace TraceLab.UI.GTK
{
    public class ExperimentCanvasPadFactory
    {
        private static Dictionary<string, ExperimentCanvasPad> m_mapPadToNodes = new Dictionary<string, ExperimentCanvasPad>();

        //HERZUM SPRINT 2: TLAB-156
        private static Dictionary<string, BasicNodeControl> m_mapIdToNodes = new Dictionary<string, BasicNodeControl>();
        //HERZUM END SPRINT 2: TLAB-156

        public static ExperimentCanvasPad CreateExperimentCanvasPad(ApplicationContext applicationContext, BasicNodeControl basicNodeControl) 
        {
            IExperiment experiment = null;
            ScopeBaseMetadata scopeMetadata = basicNodeControl.ExperimentNode.Data.Metadata as ScopeBaseMetadata;
            if (scopeMetadata != null)
                experiment = scopeMetadata.ComponentGraph.GetExperiment ();
            else
                return applicationContext.MainWindow.ExperimentCanvasPad;

            ExperimentCanvasPad   experimentCanvasPad = new  ExperimentCanvasPad(applicationContext);
            if (!m_mapPadToNodes.ContainsKey(applicationContext.GetHashCode() + experiment.ExperimentInfo.Id))
                //HERZUM SPRINT 2: TLAB-156
                {
                  // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
                  // m_mapPadToNodes.Add(experiment.ExperimentInfo.Id, experimentCanvasPad);
                  // m_mapIdToNodes.Add (experiment.ExperimentInfo.Id, basicNodeControl);
                  m_mapPadToNodes.Add(applicationContext.GetHashCode() + experiment.ExperimentInfo.Id, experimentCanvasPad);
                  m_mapIdToNodes.Add (applicationContext.GetHashCode() + experiment.ExperimentInfo.Id, basicNodeControl);
                // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
                }
            //HERZUM END SPRINT 2: TLAB-156
            return experimentCanvasPad;
        }


        // HERZUM SPRINT 2.4 TLAB-157
        public static ExperimentCanvasPad CreateCompositeExperimentCanvasPad(ApplicationContext applicationContext, ExperimentCanvasWidget experimentCanvasWidget, CompositeComponentGraph experiment) 
        {   
            ExperimentCanvasPad experimentCanvasPad = null;
            if (m_mapPadToNodes.TryGetValue (applicationContext.GetHashCode() + experiment.ExperimentInfo.Id, out experimentCanvasPad))
                return experimentCanvasPad;
            else
            {
                experimentCanvasPad = new  ExperimentCanvasPad(applicationContext);
                m_mapPadToNodes.Add(applicationContext.GetHashCode() + experiment.ExperimentInfo.Id, experimentCanvasPad);
            } 
 
            DockFrame m_dockFrame = new DockFrame();
            Gdk.WindowAttr attributes = new Gdk.WindowAttr();
            attributes.WindowType = Gdk.WindowType.Child;
            attributes.X = 100;
            attributes.Y = 100;
            attributes.Width = 100;
            attributes.Height = 100;    
            Gdk.WindowAttributesType mask = WindowAttributesType.X | WindowAttributesType.Y;
            m_dockFrame.GdkWindow = new Gdk.Window(null, attributes, (int) mask);
            experimentCanvasPad.Initialize (m_dockFrame);
            experimentCanvasPad.SetApplicationModel(applicationContext.Application, experimentCanvasWidget, experiment);
            return experimentCanvasPad;
        }
        // END HERZUM SPRINT 2.4 TLAB-157


        public static ExperimentCanvasPad GetExperimentCanvasPad(ApplicationContext applicationContext, BasicNodeControl basicNodeControl) 
        {
            ExperimentCanvasPad experimentCanvasPad=null;
            // HERZUM SPRINT 2.4: TLAB-156
            if (basicNodeControl is CommentNodeControl && !m_mapIdToNodes.ContainsValue(basicNodeControl))
                m_mapIdToNodes.Add (applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.ID, basicNodeControl);
            // END HERZUM SPRINT 2.4: TLAB-156

            // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
            // if (m_mapPadToNodes.TryGetValue (basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id, out experimentCanvasPad))
            if (m_mapPadToNodes.TryGetValue (applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id, out experimentCanvasPad))
            // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
                return experimentCanvasPad;
            else
                return applicationContext.MainWindow.ExperimentCanvasPad;

        }

        // HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59
        public static void RemoveExperimentCanvasPad(ApplicationContext applicationContext,BasicNodeControl basicNodeControl) 
        {   
            m_mapPadToNodes.Remove (applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id);
            //HERZUM SPRINT 2: TLAB-156
            m_mapIdToNodes.Remove (applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id);
            //HERZUM END SPRINT 2: TLAB-156
        } 
        // END HERZUM SPRINT 2.3 TLAB-56 TLAB-57 TLAB-58 TLAB-59

        // HERZUM SPRINT 2.5: TLAB-173
        public static void RemoveSubExperimentCanvasPad(ApplicationContext applicationContext,BasicNodeControl basicNodeControl) 
        {   
            IExperiment experiment = null;
            ScopeBaseMetadata scopeMetadata = basicNodeControl.ExperimentNode.Data.Metadata as ScopeBaseMetadata;
            if (scopeMetadata != null)
                experiment = scopeMetadata.ComponentGraph.GetExperiment ();

            if (basicNodeControl is CommentNodeControl)
            {   
                if (m_mapIdToNodes.ContainsKey(applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id))
                    m_mapIdToNodes.Remove (applicationContext.GetHashCode() + basicNodeControl.ExperimentNode.Owner.ExperimentInfo.Id);
                return;
            }
            m_mapPadToNodes.Remove (applicationContext.GetHashCode() + experiment.ExperimentInfo.Id);
            m_mapIdToNodes.Remove (applicationContext.GetHashCode() + experiment.ExperimentInfo.Id);
        } 

        // END HERZUM SPRINT 2.5: TLAB-173
        //HERZUM SPRINT 2: TLAB-156
        public void ZoomValueChanged(double valueZoom)
        {
        // HERZUM SPRINT 2.4: TLAB-156
            for (IEnumerator<KeyValuePair<string, BasicNodeControl>> enumerator = m_mapIdToNodes.GetEnumerator(); enumerator.MoveNext();)     
            {   KeyValuePair<string, BasicNodeControl> element = enumerator.Current; 
                if ( element.Value!= null)
                    element.Value.AdaptsZoom(valueZoom);
            }
          
        //END HERZUM SPRINT 2.4: TLAB-156
        }
        //HERZUM END SPRINT 2: TLAB-156

    }
}

