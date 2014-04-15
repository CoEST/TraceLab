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
using Gtk;
using TraceLab.Core.Components;
using System.Collections.Generic;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ExperimentCanvasWidget : Gtk.Bin
    {
        public ExperimentCanvasWidget ()
        {
            this.Build();

            this.experimentCrumbs.Changed += delegate(object sender, EventArgs e) 
            {
                experimentCrumbs.Active.EmitClicked();
            };
        }

        public MonoHotDraw.SteticComponent ExperimentCanvas
        {
            get { return this.experimentCanvas; }
        }

        public Crumbs ExperimentCrumbs
        {
            get { return this.experimentCrumbs; }
        }

        protected void ZoomValueChanged(object sender, EventArgs e)
        {
            ExperimentCanvas.View.Scale = zoomScale.Value;
        }

        protected void ZoomToOriginal(object sender, EventArgs e)
        {
            ZoomToOriginal();
        }

        protected void ZoomToFit(object sender, EventArgs e)
        {
            ZoomToFit();
        }

        internal void ZoomToFit()
        {
            double newZoomScale = ExperimentCanvas.View.ZoomToFit ();
            zoomScale.Value = newZoomScale;
        }

        internal void ZoomToOriginal()
        {
            zoomScale.Value = 1;
        }

        internal double ZoomScale
        {
            get { return zoomScale.Value; }
        }

        protected void SelectionToolButtonToggled(object sender, EventArgs e)
        {
            if(selectionToolButton.Active == true) 
            {
                ExperimentCanvas.SetSelectionTool();
            }

            //toggle the other button 
            if(selectionToolButton.Active == panToolButton.Active)
            {
                panToolButton.Active = !panToolButton.Active;
            }
        }

        protected void PanToolButtonToggled(object sender, EventArgs e)
        {
            if(panToolButton.Active == true) 
            {
                ExperimentCanvas.SetPanTool();
            }

            //toggle the other button 
            if(selectionToolButton.Active == panToolButton.Active)
            {
                selectionToolButton.Active = !selectionToolButton.Active;
            }
        }
    }
}

