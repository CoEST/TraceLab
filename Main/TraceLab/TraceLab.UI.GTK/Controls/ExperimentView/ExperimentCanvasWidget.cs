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

//HERZUM SPRINT 2.3: TLAB-156
using MonoHotDraw.Util;
//END HERZUM SPRINT 2.3: TLAB-156

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ExperimentCanvasWidget : Gtk.Bin
    {
        private bool shouldIToggleAgain;

        public ExperimentCanvasWidget ()
        {
            this.Build();

            this.experimentCrumbs.Changed += delegate(object sender, EventArgs e) 
            {
                experimentCrumbs.Active.EmitClicked();
            };
        }

        //HERZUM SPRINT 2: TLAB-156
        ExperimentCanvasPadFactory experimentCanvasPadFactory = new ExperimentCanvasPadFactory();
        //HERZUM END SPRINT 2: TLAB-156

        // HERZUM SPRINT 2.4: TLAB-156
        private double offsetPanX=0, offsetPanY=0;

        public double OffsetPanX
        {
            get { return this.offsetPanX; }
        }

        public double OffsetPanY
        {
            get { return this.offsetPanY; }
        }
        // END HERZUM SPRINT 2.4: TLAB-156

        // HERZUM SPRINT 1.0
        public void DestroyVbox1()
        {
            this.vbox1.Destroy ();
        }
        // END HERZUM SPRINT 1.0


        //HERZUM SPRINT 2: TLAB-156
        public bool IsPanToolButtonActive()
        {
            return panToolButton.Active;
        }
        //HERZUM END SPRINT 2: TLAB-156

        public MonoHotDraw.SteticComponent ExperimentCanvas
        {
            get { return this.experimentCanvas; }
        }

        public Crumbs ExperimentCrumbs
        {
            get { return this.experimentCrumbs; }
        }

        // TLAB-184
        public void PanTool()
        {
            if (this.panToolButton.Active != true) {  
                this.panToolButton.Click ();    
                shouldIToggleAgain = true;
            } else 
                shouldIToggleAgain = false;
        }
     
        public void PanToolButtonReleased()
        {
            if (shouldIToggleAgain) {
                this.panToolButton.Click ();
                shouldIToggleAgain = false;
            }
        }
        /// TLAB-184
        

        protected void ZoomValueChanged(object sender, EventArgs e)
        {
            ExperimentCanvas.View.Scale = zoomScale.Value;

            //HERZUM SPRINT 2: TLAB-156
            experimentCanvasPadFactory.ZoomValueChanged(zoomScale.Value);
            //HERZUM SPRINT 2: TLAB-156
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

            //double newZoomScale = ExperimentCanvas.View.ZoomToFit ();
            //zoomScale.Value = newZoomScale;

            //HERZUM SPRINT 2.5: TLAB-156
            //experimentCanvasPadFactory.ZoomValueChanged(1);


            // HERZUM SPRINT 3: TLAB-186
            if (panToolButton.Active)
                return;

            if (zoomScale.Value != 1)
            {
                zoomScale.Value = 1;
                experimentCanvasPadFactory.ZoomValueChanged(1);
            }


            // END HERZUM SPRINT 3: TLAB-186

            double xVisible = ExperimentCanvas.View.VisibleArea.X;
            double yVisible = ExperimentCanvas.View.VisibleArea.Y;
            double x2Visible = ExperimentCanvas.View.VisibleArea.X2;
            double y2Visible = ExperimentCanvas.View.VisibleArea.Y2;
            double widthVisible = ExperimentCanvas.View.VisibleArea.Width;
            double heightVisible = ExperimentCanvas.View.VisibleArea.Height;
            double maxWScale=1; 
            double maxHScale=1; 
            bool firstTime = true;
            double xMin=0, yMin=0, xMax=0, yMax=0;

            foreach (MonoHotDraw.Figures.IFigure figure in ExperimentCanvas.View.Drawing.FiguresEnumerator) {
                if (firstTime)                
                {
                    xMin=figure.DisplayBox.X;
                    yMin=figure.DisplayBox.Y; 
                    xMax=figure.DisplayBox.X2; 
                    yMax=figure.DisplayBox.Y2;
                    firstTime = false;                  
                } 
                else
                {
                    if (figure.DisplayBox.X<xMin)
                        xMin=figure.DisplayBox.X;
                    if (figure.DisplayBox.Y<yMin)
                        yMin=figure.DisplayBox.Y;

                    if (figure.DisplayBox.X2>xMax)
                        xMax=figure.DisplayBox.X2;                   
                    if (figure.DisplayBox.Y2>yMax)                      
                        yMax=figure.DisplayBox.Y2;
                }
            }

            // HERZUM SPRINT 3: TLAB-186
            if (!(xMin >= xVisible && xMax <= x2Visible && yMin >= yVisible && yMax <= y2Visible))
            {
                foreach (MonoHotDraw.Figures.IFigure figure in ExperimentCanvas.View.Drawing.FiguresEnumerator) {
                    figure.MoveTo (figure.DisplayBox.X - xMin, figure.DisplayBox.Y - yMin);
                } 
                xMin=0;
                yMin=0;
                xMax = xMax - xMin;
                yMax = yMax - yMin;
            }
            // END HERZUM SPRINT 3: TLAB-186


            // HERZUM SPRINT 3: TLAB-186
            if (offsetPanX!=0  || offsetPanY!=0 )
                if (!(xMin >= xVisible && xMax <= x2Visible && yMin >= yVisible && yMax <= y2Visible))
                {
                    foreach (MonoHotDraw.Figures.IFigure figure in ExperimentCanvas.View.Drawing.FiguresEnumerator) {
                        figure.MoveTo (figure.DisplayBox.X - offsetPanX, figure.DisplayBox.Y - offsetPanY);
                    } 
                    xMin=-offsetPanX;
                    yMin=-offsetPanY;
                    xMax = xMax - offsetPanX;
                    yMax = yMax - offsetPanY;
            }
            // END HERZUM SPRINT 3: TLAB-186

            // HERZUM SPRINT 3: TLAB-186
            if (offsetPanX!=0 || offsetPanY!=0)
            {
                xMin = ExperimentCanvas.View.VisibleArea.X; yMin = ExperimentCanvas.View.VisibleArea.Y; 
                xMax = ExperimentCanvas.View.VisibleArea.X2; yMax= ExperimentCanvas.View.VisibleArea.Y2;
            } 

            foreach (MonoHotDraw.Figures.IFigure figure in ExperimentCanvas.View.Drawing.FiguresEnumerator) {
                if (firstTime)                
                {
                    xMin=figure.DisplayBox.X;
                    yMin=figure.DisplayBox.Y; 
                    xMax=figure.DisplayBox.X2; 
                    yMax=figure.DisplayBox.Y2;
                    firstTime = false;                  
                 } 
                    else
                 {
                    if (figure.DisplayBox.X<xMin)
                        xMin=figure.DisplayBox.X;
                    if (figure.DisplayBox.Y<yMin)
                        yMin=figure.DisplayBox.Y;

                    if (figure.DisplayBox.X2>xMax)
                        xMax=figure.DisplayBox.X2;                   
                    if (figure.DisplayBox.Y2>yMax)                      
                        yMax=figure.DisplayBox.Y2;
                }
            }
            // END HERZUM SPRINT 3: TLAB-186


            if (xMin >= xVisible && xMax <= x2Visible && yMin >= yVisible && yMax <= y2Visible)
            { 
                maxWScale = (widthVisible+(x2Visible-xMax))/(widthVisible); 
                maxHScale = (heightVisible+(y2Visible-yMax))/(heightVisible);                
                if (maxWScale<=maxHScale)                    
                    zoomScale.Value = zoomScale.Value*maxWScale;
                else
                    zoomScale.Value = zoomScale.Value*maxHScale;
            }

            if (xMax > x2Visible || yMax > y2Visible)              
            {
                maxHScale = (heightVisible)/(heightVisible+Math.Abs (y2Visible-yMax));  
                zoomScale.Value = zoomScale.Value*maxHScale;
                if (xMax > x2Visible)                   
                {
                    maxWScale = (widthVisible)/(widthVisible+Math.Abs (x2Visible-xMax));                     
                    if (maxWScale<maxHScale) 
                        zoomScale.Value = zoomScale.Value*maxWScale;
                }
            }

            experimentCanvasPadFactory.ZoomValueChanged(zoomScale.Value);
            //END HERZUM SPRINT 2.5: TLAB-156          

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

            // HERZUM SPRINT 2.4: TLAB-156
            Cairo.PointD pointTranslation = ExperimentCanvas.View.DrawingToView(0, 0);
            offsetPanX = pointTranslation.X;
            offsetPanY= pointTranslation.Y;
            // END HERZUM SPRINT 2.4: TLAB-156
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

