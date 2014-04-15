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
using System;
using TraceLab.Core.Experiments;
using MonoHotDraw.Handles;
using System.Collections.Generic;
using MonoHotDraw.Util;
using MonoHotDraw.Locators;
using TraceLab.Core.Components;

namespace TraceLab.UI.GTK
{
    public class CompositeComponentControl: ComponentControl
    {
        static CompositeComponentControl()
        {
            s_magnifierGlassIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.loupe.png");
        }

        public CompositeComponentControl(ExperimentNode node, ApplicationContext applicationContext) : base(node, applicationContext)
        {
            m_displayComponentSubGraph = new PixButtonHandle(this, new QuickActionLocator (35, 0.8, QuickActionPosition.Right),
                                                             s_magnifierGlassIcon, DisplayComponentSubGraph);
        }

        private void DisplayComponentSubGraph() 
        {
            var metadata = (CompositeComponentMetadata)this.ExperimentNode.Data.Metadata;

            //componentGraph might be null if component metadata definition is not existing in the library 
            if(metadata.ComponentGraph != null)
            {
                m_applicationContext.MainWindow.ExperimentCanvasPad.DisplaySubgraph(metadata);
            }
        }
     

        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                foreach (IHandle handle in base.HandlesEnumerator) 
                {
                    yield return handle;
                }

                //always show magnifier glass button
                yield return m_displayComponentSubGraph;
            }
        }

        protected override void DrawFrame (Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor)
        {
            RectangleD rect = DisplayBox;
            rect.OffsetDot5();
            context.Rectangle(GdkCairoHelper.CairoRectangle(rect));
            context.Color = fillColor;
            context.FillPreserve();
            context.Color = lineColor;
            context.LineWidth = lineWidth;
            context.Stroke();
        }
        
        private static Gdk.Pixbuf s_magnifierGlassIcon;
        private PixButtonHandle m_displayComponentSubGraph;

    }
}

