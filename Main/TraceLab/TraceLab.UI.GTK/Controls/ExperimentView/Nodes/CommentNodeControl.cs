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

// HERZUM SPRINT 1.0 CLASS

using System;
using TraceLab.Core.Experiments;
using MonoHotDraw.Handles;
using System.Collections.Generic;
using MonoHotDraw.Util;

// HERZUM SPRINT 1.0
using TraceLab.Core.Components;
// END HERZUM SPRINT 1.0


namespace TraceLab.UI.GTK
{
    // HERZUM SPRINT 1.2 TLAB-52 - ComponentControl --> ComponentResizeControl
    public class CommentNodeControl :  ComponentResizeControl
    {

        public CommentNodeControl(ExperimentNode node, ApplicationContext applicationContext) 
        : base(node, applicationContext)
        {
            // HERZUM SPRINT 2.4: TLAB-156
            /*
            info = this.ExperimentNode.Data as SerializedVertexDataWithSize;
            double widthMeta = info.Width;          
            double heightMeta = info.Height;

            PaddingTop = 7.0;
            PaddingLeft= widthMeta/2;
            PaddingRight= widthMeta/2;           
            PaddingBottom= heightMeta;
            MoveTo (info.X, info.Y);
              */
            // END HERZUM SPRINT 2.4: TLAB-156

           
            // HERZUM SPRINT 1.0
            cw.Comment = ((CommentMetadata) node.Data.Metadata).Comment;

            cw.ExposeEvent += delegate {
                ((CommentMetadata) node.Data.Metadata).Comment = cw.Comment;
            };
            // END HERZUM SPRINT 1.0

            //HERZUM SPRINT 2.0 TLAB-136
            ecp = ExperimentCanvasPadFactory.GetExperimentCanvasPad (m_applicationContext, this);
            //END HERZUM SPRINT 2.0 TLAB-136
        
        }

        // HERZUM SPRINT 1.0
        public CommentWidget CommentCanvasWidget
        {
            get { return cw; }
        }
        //END HERZUM SPRINT 1.0

        // HERZUM SPRINT 1.0
        private CommentWidget cw = new   CommentWidget();
        private Boolean nodeCreated = false;
        // HERZUM SPRINT 2.4: TLAB-156
        //SerializedVertexDataWithSize info;
        // END HERZUM SPRINT 2.4: TLAB-156

        //END HERZUM SPRINT 2.0 TLAB-136
        ExperimentCanvasPad ecp=null;
        //END HERZUM SPRINT 2.0 TLAB-136

        // HERZUM SPRINT 1.2 TLAB-32
        // private double xCur;
        // private double yCur;

        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                foreach (IHandle handle in base.HandlesEnumerator) 
                {
                    if (handle is PixToggleButtonHandle || handle is NewConnectionHandle || handle is RemoveNodeHandle)
                        yield return handle;
                }
                yield return m_resizeScope_NorthEast;
                yield return m_resizeScope_SouthEast;
                yield return m_resizeScope_SouthWest;
                yield return m_resizeScope_NorthWest;
            }
        }
        // END HERZUM SPRINT 1.2 TLAB-32

        // END HERZUM SPRINT 1.0

        // HERZUM SPRINT 1.1 FOCUS
        public override bool IsSelected {
            get {
                return base.IsSelected;
            }
            set {

                if (value && !base.IsSelected)
                    //HERZUM SPRINT 2.0 TLAB-136
                    // HERZUM SPRINT 2.4: TLAB-156
                    // HERZUM SPRINT 5.3 TLAB-185
                    // ecp.RedrawScope (cw,  rect.X+offsetPanX, rect2.Y2+offsetPanY, rect.Width -10, rect.Height - rect2.Height -10);
                    ecp.RedrawScope (cw,  rect.X+offsetPanX, rect2.Y2+offsetPanY);
                    // END HERZUM SPRINT 5.3 TLAB-185
                    // END HERZUM SPRINT 2.4: TLAB-156
                   //m_applicationContext.MainWindow.ExperimentCanvasPad.RedrawScope (cw,  rect.X, rect2.Y2, rect.Width -10, rect.Height - rect2.Height -10);
                   //END HERZUM SPRINT 2.0 TLAB-136   

                //update also corresponding value in model experiment node
                base.IsSelected = value;
            }
        }
        // END HERZUM SPRINT 1.1 FOCUS

       


        private static Cairo.Color s_selectedFillColor = new Cairo.Color(95.0/255, 129.0/255, 230.0/255);

        protected override void DrawFrame (Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor){

            SetupLayout(context);

            rect = DisplayBox;
            rect.OffsetDot5();
            double side = 10.0;

            context.LineWidth = LineWidth;
            // HERZUM SPRINT 2.4: TLAB-156
            //context.Save ();
            // END HERZUM SPRINT 2.4: TLAB-156
            //Box
            context.MoveTo (rect.X, rect.Y);
            context.LineTo (rect.X, rect.Y + rect.Height);
            context.LineTo (rect.X + rect.Width, rect.Y + rect.Height);
            context.LineTo (rect.X + rect.Width, rect.Y + side);
            context.LineTo (rect.X + rect.Width - side, rect.Y);
            context.LineTo (rect.X, rect.Y);
            // HERZUM SPRINT 2.4: TLAB-156
            //context.Save ();
            // END HERZUM SPRINT 2.4: TLAB-156
            //Triangle
            context.MoveTo (rect.X + rect.Width - side, rect.Y);
            context.LineTo (rect.X + rect.Width - side, rect.Y + side);
            context.LineTo (rect.X + rect.Width, rect.Y + side);
            context.LineTo (rect.X + rect.Width - side, rect.Y);
            // HERZUM SPRINT 2.4: TLAB-156
            //context.Restore ();
            // END HERZUM SPRINT 2.4: TLAB-156

            if (this.IsSelected)
                context.Color = s_selectedFillColor;
            else
                context.Color = FillColor;

            context.FillPreserve ();
            context.Color = LineColor;

            rect2 = DisplayBox;
            rect2.Width = DisplayBox.Width;
            rect2.Height = 30;
            rect2.OffsetDot5();

            context.Stroke ();

            // HERZUM SPRINT 1.0
            //m_applicationContext.MainWindow.ExperimentCanvasPad.CommentNodeControlCurrent = this;
            // END HERZUM SPRINT 1.0

            // HERZUM SPRINT 2.4: TLAB-156
            valueZoom = ecp.ExperimentCanvasWidget.ExperimentCanvas.View.Scale;
            // END HERZUM SPRINT 2.4: TLAB-156

            if (!nodeCreated)  {
                xCur = rect.X;              
                yCur = rect.Y; 
                // HERZUM SPRINT 2.4: TLAB-156
                offsetPanX =  ecp.ExperimentCanvasWidget.OffsetPanX;
                offsetPanY= ecp.ExperimentCanvasWidget.OffsetPanY;
                ecp.ExperimentCanvasWidget.ExperimentCanvas.View.AddWidget (cw, rect.X+offsetPanX, rect.Y+offsetPanY + 30); 
                // END HERZUM SPRINT 2.4: TLAB-156
                //ecp.ExperimentCanvasWidget.ExperimentCanvas.View.AddWidget (cw, rect.X, rect.Y + 30); 
                cw.Show ();
                nodeCreated=true;                   
                } 
            else if (xCur != rect.X || yCur != rect.Y) {
                //HERZUM SPRINT 2.0 TLAB-136   
                //m_applicationContext.MainWindow.ExperimentCanvasPad.ExperimentCanvasWidget.ExperimentCanvas.View.MoveWidget (cw, rect.X, rect.Y + 30);
                // HERZUM SPRINT 2.4: TLAB-156
                if ((valueZoom==zoomPrevious && valueZoom==1)) 
                    ecp.ExperimentCanvasWidget.ExperimentCanvas.View.MoveWidget (cw, rect.X+offsetPanX, rect.Y+offsetPanY + 30);
                // END HERZUM SPRINT 2.4: TLAB-156
                //END HERZUM SPRINT 2.0 TLAB-136   
                xCur = rect.X;
                yCur = rect.Y;               
            }
            ResizeCanvasWidget ();

            // HERZUM SPRINT 2.4: TLAB-156
            if (!(valueZoom==zoomPrevious && valueZoom==1)) 
            {
                valueZoom = ecp.ExperimentCanvasWidget.ExperimentCanvas.View.Scale;
                AdaptsZoom (valueZoom);
            }
            // END HERZUM SPRINT 2.4: TLAB-156

            // HERZUM SPRINT 2.4: TLAB-156
            //HERZUM SPRINT 5.5 TLAB-216  
            point = ecp.ExperimentCanvasWidget.ExperimentCanvas.View.DrawingToView(rect.X/valueZoom, rect2.Y2/valueZoom);
            offsetPanX = (point.X-rect.X)/valueZoom;
            offsetPanY= (point.Y-rect2.Y-30)/valueZoom;
            //END HERZUM SPRINT 5.5 TLAB-216   
            if (ecp.ExperimentCanvasWidget.IsPanToolButtonActive())    
            {
                if (valueZoom==1)
                    ecp.ExperimentCanvasWidget.ExperimentCanvas.View.MoveWidget (cw, rect.X+offsetPanX, rect.Y+offsetPanY + 30);
            } 

            // END HERZUM SPRINT 2.4: TLAB-156
        }


        // HERZUM SPRINT 1.2 TLAB-52
        protected override void ResizeCanvasWidget (){
            // HERZUM SPRINT 5.3 TLAB-219
            cw.WidthRequest = (int)Math.Truncate(rect.X2 - rect.X)-1;
            cw.HeightRequest = (int)Math.Truncate(rect.Y2 - rect2.Y2)-1;
            // END HERZUM SPRINT 5.3 TLAB-219
        }


        protected override void GetPointer(out int xMouse, out int yMouse){
            cw.GetPointer(out xMouse, out yMouse);
            // HERZUM SPRINT 2.4: TLAB-156
            xMouse = (int)((xMouse)/valueZoom);
            yMouse = (int)((yMouse)/valueZoom);
            // END HERZUM SPRINT 2.4: TLAB-156
        }


        protected override void ShowWidget(){ 
            cw.Show ();
        }

        protected override void HideWidget(){ 
            cw.Hide ();
        }
        // END HERZUM SPRINT 1.2 TLAB-52

        // HERZUM SPRINT 2.4: TLAB-156
        double zoomPrevious, valueZoom;
        public override void AdaptsZoom(double newZoom)
        {
            Gdk.Rectangle rectZoom;
            if (!(newZoom==zoomPrevious && newZoom==1))  
            {
                rectZoom = new Gdk.Rectangle ();
                rectZoom.Height= (int)((PaddingBottom-10)*newZoom);
                rectZoom.Width = (int)((PaddingLeft * 2 + ExperimentNode.Data.Metadata.Label.Length * 6 +10)*newZoom);
                // HERZUM SPRINT 2.4: TLAB-156
                rectZoom.X = (int)((rect2.X+offsetPanX+1)*newZoom);
                rectZoom.Y = (int)((rect2.Y2+offsetPanY+1)*newZoom);
                // END HERZUM SPRINT 2.4: TLAB-156
                cw.Allocation=rectZoom;
                zoomPrevious = newZoom;
                valueZoom = newZoom;
                if (newZoom==1)
                    ecp.RedrawScope (cw,  rect.X+offsetPanX, rect2.Y2+offsetPanY);
            }
        }
        // END HERZUM SPRINT 2.4: TLAB-156

    }

}

