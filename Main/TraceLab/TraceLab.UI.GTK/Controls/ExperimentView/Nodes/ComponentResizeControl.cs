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
// HERZUM SPRINT 1.2 TLAB-52


using System;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;
using TraceLab.Core.Experiments;
using MonoHotDraw.Connectors;
using System.Collections.Generic;
using MonoHotDraw.Locators;
using MonoHotDraw.Handles;
using MonoHotDraw.Tools;
using MonoHotDraw;
using Cairo;
using TraceLab.Core.Components;

namespace TraceLab.UI.GTK
{
    public class ComponentResizeControl :ComponentControl 
    {
        private static Gdk.Pixbuf s_maxIcon;
        protected PixButtonHandle m_maxScope;
        private static Gdk.Pixbuf s_minIcon;
        protected PixButtonHandle m_minScope;
        private static Gdk.Pixbuf s_normalIcon;
        protected PixButtonHandle m_normalScope;

        private static Gdk.Pixbuf s_maxDisabledIcon;
        protected PixButtonHandle m_maxDisabledScope;
        private static Gdk.Pixbuf s_minDisabledIcon;
        protected PixButtonHandle m_minDisabledScope;
        private static Gdk.Pixbuf s_normalDisabledIcon;
        protected PixButtonHandle m_normalDisabledScope;

        private static Gdk.Pixbuf s_resizeIcon;

        protected PixButtonHandle m_resizeScope_NorthEast;
        protected PixButtonHandle m_resizeScope_SouthEast;
        protected PixButtonHandle m_resizeScope_SouthWest;
        protected PixButtonHandle m_resizeScope_NorthWest;

        private SerializedVertexDataWithSize info;
        protected String stateWidget;

        double paddingLeftOriginal;
        double paddingRightOriginal;
        double paddingBottomOriginal;
        double xOriginal;
        double yOriginal;

        protected RectangleD rect;
        protected RectangleD rect2;

        protected double xCur;
        protected double yCur;

        // HERZUM SPRINT 2.4: TLAB-156
        protected double offsetPanX=0, offsetPanY=0;
        protected PointD point;
        // END HERZUM SPRINT 2.4: TLAB-156

        // HERZUM SPRINT 5.3: TLAB-185
        protected int edgeBorder=5;
        // END HERZUM SPRINT 5.3: TLAB-185

        static ComponentResizeControl()

        {
            s_maxIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max.png");
            s_minIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min.png");
            s_normalIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize.png");
            s_maxDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max_disabled.png");
            s_minDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min_disabled.png");
            s_normalDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize_disabled.png");
            s_resizeIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_resize.png");
        }

        // HERZUM SPRINT 5.1: TLAB-230
        public string StateWidget
        {
            get { return stateWidget;}
        }
        // END HERZUM SPRINT 5.1: TLAB-230

        public ComponentResizeControl(ExperimentNode node, ApplicationContext applicationContext) : base(node, applicationContext)
        {
           
            info = this.ExperimentNode.Data as SerializedVertexDataWithSize;
            double widthMeta = info.Width;          
            double heightMeta = info.Height;

            PaddingTop = 7.0;
            PaddingLeft= widthMeta/2;
            PaddingRight= widthMeta/2;           
            PaddingBottom= heightMeta;
            MoveTo (info.X, info.Y);

            if (info.WidgetStatus == "" || info.WidgetStatus== null)
                stateWidget = "normal";
            else
                stateWidget = info.WidgetStatus;


            paddingLeftOriginal = 80.0;
            paddingRightOriginal = 80.0;
            paddingBottomOriginal = 160.0;

            xOriginal = rect.X;
            yOriginal = rect.Y; 

            SetPixButtonHandle ();
            RedrawButton ();        

        }


        // HERZUM SPRINT 5.2: TLAB-248
        private void SetPixButtonHandle()
        { 
            m_minScope = new PixButtonHandle(this, new AbsoluteLocator(15.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),
                                         s_minIcon, IconizeScope);

            m_normalScope = new PixButtonHandle(this, new AbsoluteLocator(30.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),                                               
                                            s_normalIcon, NormalizeScope);

            m_maxScope = new PixButtonHandle(this, new AbsoluteLocator(50.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),
                                         s_maxIcon, MaximizeScope);

            // HERZUM SPRINT 5: TLAB-233, TLAB-234
            m_minDisabledScope = new PixButtonHandle(this, new AbsoluteLocator(15.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),
                                                 s_minDisabledIcon, DisableIcon);

            m_normalDisabledScope = new PixButtonHandle(this, new AbsoluteLocator(30.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),                                               
                                                    s_normalDisabledIcon, DisableIcon);

            m_maxDisabledScope = new PixButtonHandle(this, new AbsoluteLocator(50.0, 15.0, AbsoluteLocator.AbsoluteTo.TopLeft),
                                                 s_maxDisabledIcon, DisableIcon);
            // END HERZUM SPRINT 5: TLAB-233, TLAB-234
        }
        // HERZUM SPRINT 5.2: TLAB-248

        private void RedrawButton()
        {   
            //HERZUM SPRINT 5.5 TLAB-253    
            m_resizeScope_NorthWest = new PixButtonHandle(this, new AbsoluteLocator(0.0, -5.0, AbsoluteLocator.AbsoluteTo.TopLeft),
                                                          s_resizeIcon, ResizeScopeNorthWest,  (int)rect.X2, (int)rect.Y2, valueZoom, offsetPanX, offsetPanY);
            m_resizeScope_NorthEast = new PixButtonHandle(this, new AbsoluteLocator(0.0, -5.0, AbsoluteLocator.AbsoluteTo.TopRight),
                                                          s_resizeIcon, ResizeScopeNorthEast,  (int)rect.X, (int)rect.Y2, valueZoom, offsetPanX, offsetPanY);
            m_resizeScope_SouthEast = new PixButtonHandle(this, new AbsoluteLocator(0.0, PaddingBottom +25, AbsoluteLocator.AbsoluteTo.TopRight),
                                                          s_resizeIcon, ResizeScopeSouthEast, (int)rect.X, (int)rect.Y, valueZoom, offsetPanX, offsetPanY);
            m_resizeScope_SouthWest = new PixButtonHandle(this, new AbsoluteLocator(0.0, PaddingBottom + 25, AbsoluteLocator.AbsoluteTo.TopLeft),
                                                          s_resizeIcon, ResizeScopeSouthWest,  (int)rect.X2, (int)rect.Y, valueZoom, offsetPanX, offsetPanY);
            //END HERZUM SPRINT 5.5 TLAB-253
        }

        //HERZUM SPRINT 2.0 TLAB-155
        public void GetSizeAreaWidget(out int x, out int y, out int width, out int height)
        {
            x = (int)rect.X;
            y = (int)rect.Y;
            width = (int)rect.Width;
            height = (int)rect.Height;

            if (stateWidget=="iconized")
            {
                width = (int)paddingLeftOriginal*2 + (int)this.ExperimentNode.Data.Metadata.Label.Length*7;
                height = (int)paddingBottomOriginal;
            }
        }

        private Boolean isResize=false;

        public Boolean IsResize
        {
            get { return isResize; }
            set 
            {              
                isResize = value;
            }
        }
        //END HERZUM SPRINT 2.0 TLAB-155

        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            {
                // HERZUM SPRINT 5: TLAB-233, TLAB-234
                ZoomIcon ();
                // END HERZUM SPRINT 5: TLAB-233, TLAB-234
                foreach (IHandle handle in base.HandlesEnumerator) 
                {
                    yield return handle;
                }
                if (stateWidget=="normal")
                {
                    yield return m_minScope;
                    yield return m_normalDisabledScope;
                    yield return m_maxScope;
                    yield return m_resizeScope_NorthEast;
                    yield return m_resizeScope_SouthEast;
                    yield return m_resizeScope_SouthWest;
                    yield return m_resizeScope_NorthWest;
                }
                else if (stateWidget=="max")
                {
                    yield return m_minScope;
                    yield return m_normalScope;
                    yield return m_maxDisabledScope;
                }
                else if (stateWidget=="iconized")
                {
                    yield return m_minDisabledScope;
                    yield return m_normalScope;
                    yield return m_maxScope;
                }

                // HERZUM SPRINT 5.5 TLAB-253
                RedrawButton ();
                // END HERZUM SPRINT 5.5 TLAB-253
            }
        }


        // HERZUM SPRINT 5.2: TLAB-233, TLAB-234, TLAB-248
        double previousZoom=1;
        double valueZoom=1;
        private void ZoomIcon() {
            valueZoom =  m_applicationContext.MainWindow.ExperimentCanvasPad.ExperimentCanvasWidget.ExperimentCanvas.View.Scale;
            if (valueZoom<0.8)
                {
                    s_maxIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max_small.png");
                    s_minIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min_small.png");
                    s_normalIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize_small.png");
                    s_maxDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max_disabled_small.png");
                    s_minDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min_disabled_small.png");
                    s_normalDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize_disabled_small.png");
                }
                else
                {
                    s_maxIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max.png");
                    s_minIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min.png");
                    s_normalIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize.png");
                    s_maxDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_max_disabled.png");
                    s_minDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_min_disabled.png");
                    s_normalDisabledIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.icon_normalize_disabled.png");

                }
                SetPixButtonHandle ();
                previousZoom = valueZoom;
        }
        // END HERZUM SPRINT 5.2: TLAB-233, TLAB-234, TLAB-248

        // ************************************************

        public void DisableIcon() {
        }

        public void MaximizeScope() {
            if (stateWidget=="normal")
            {
                paddingLeftOriginal=PaddingLeft;
                paddingRightOriginal=PaddingRight;
                paddingBottomOriginal=PaddingBottom;
                xOriginal = rect.X;
                yOriginal = rect.Y;
            }

            int xPosInit = 5;
            int yPosInit = 5;
            double widthMax;
            double heightMax;

            //HERZUM SPRINT 2.3: TLAB-52
            ExperimentCanvasPad ecp=null;
            ecp = ExperimentCanvasPadFactory.GetExperimentCanvasPad (m_applicationContext, this);


            widthMax = ecp.ExperimentCanvasWidget.ExperimentCanvas.View.VisibleArea.Width-20;
            heightMax = ecp.ExperimentCanvasWidget.ExperimentCanvas.View.VisibleArea.Height-40;


            //widthMax = m_applicationContext.MainWindow.ExperimentCanvasPad.ExperimentCanvasWidget.ExperimentCanvas.View.VisibleArea.Width-20;
            //heightMax = m_applicationContext.MainWindow.ExperimentCanvasPad.ExperimentCanvasWidget.ExperimentCanvas.View.VisibleArea.Height-40;
            //HERZUM SPRINT 2.3: TLAB-52

            // HERZUM SPRINT 2.4: TLAB-156
            MoveTo(xPosInit-offsetPanX,yPosInit-offsetPanY);
            // END HERZUM SPRINT 2.4: TLAB-156

            PaddingTop = 7.0;
            PaddingLeft= PaddingLeft + (widthMax - (rect.X2 - rect.X))/2;
            PaddingRight= PaddingRight + (widthMax - (rect.X2 - rect.X))/2;;
            PaddingBottom= heightMax;

            ResizeCanvasWidget();

            xCur=rect.X;
            yCur=rect.Y;

            stateWidget="max"; 
            info.WidgetStatus=stateWidget;

            ShowWidget();
        }

        public void NormalizeScope() {
            PaddingLeft= paddingLeftOriginal;
            PaddingRight= paddingRightOriginal;
            PaddingBottom= paddingBottomOriginal;

            if (stateWidget=="max")
                MoveTo(xOriginal,yOriginal);

            RedrawButton ();
            ResizeCanvasWidget (); 

            xCur=rect.X;
            yCur=rect.Y;

            stateWidget="normal";
            info.WidgetStatus=stateWidget;

            // HERZUM SPRINT 5.1: TLAB-249
            MoveIconInfo (0.12);
            // END HERZUM SPRINT 5.1: TLAB-249

            ShowWidget ();
        }

        public void IconizeScope() {

            if (stateWidget=="normal")
            {
                xOriginal = rect.X;
                yOriginal = rect.Y;
            }

            if (stateWidget=="max")
                NormalizeScope (); 

            paddingLeftOriginal=PaddingLeft;
            paddingRightOriginal=PaddingRight;
            paddingBottomOriginal=PaddingBottom;

            PaddingBottom = 7.0;
            PaddingLeft = 80.0;
            PaddingRight = 80.0;

            xCur=rect.X;
            yCur=rect.Y;

            stateWidget="iconized";
            info.WidgetStatus=stateWidget;

            // HERZUM SPRINT 5.1: TLAB-249
            MoveIconInfo (0.8);
            // END HERZUM SPRINT 5.1: TLAB-249

            HideWidget();
        }


        private void ResizeScopeSouthEast() {
            int xMouse;
            int yMouse; 

            GetPointer(out xMouse, out yMouse);

            yMouse = yMouse + 15;
            double lengthLabel = rect.X2 - rect.X - 2*PaddingLeft;
           
            PaddingLeft = xMouse/2 - lengthLabel/2;
            PaddingRight= xMouse/2 - lengthLabel/2;
            PaddingBottom=yMouse;

            ResizeCanvasWidget ();
            SaveMetadata ();

            SaveOriginal ();
            RedrawButton ();

            //HERZUM SPRINT 2.0 TLAB-155
            isResize = true;
            //END HERZUM SPRINT 2.0 TLAB-155

            //HERZUM SPRINT 5.2 TLAB-249
            //MoveIconInfo (0.8-yMouse*0.00085);
            //HERZUM SPRINT 5.2 TLAB-249

            ShowWidget ();

        }



        private void ResizeScopeSouthWest() {
            int xMouse;
            int yMouse; 

            GetPointer(out xMouse, out yMouse);


            xMouse = xMouse + 10;
            yMouse = yMouse + 20;

            MoveTo (rect.X+xMouse, rect.Y);
            PaddingLeft = PaddingLeft - xMouse/2;
            PaddingRight = PaddingRight - xMouse/2;
            PaddingBottom=yMouse;


            ResizeCanvasWidget ();
            SaveMetadata ();

            SaveOriginal ();
            RedrawButton ();

            //HERZUM SPRINT 2.0 TLAB-155
            isResize = true;
            //END HERZUM SPRINT 2.0 TLAB-155

            ShowWidget ();
        }

        private void ResizeScopeNorthWest() {
            int xMouse;
            int yMouse; 

            GetPointer(out xMouse, out yMouse);

            xMouse = xMouse + 10;
            yMouse = yMouse + 50;

            MoveTo (rect.X+xMouse, rect.Y+yMouse);

            PaddingLeft = PaddingLeft - xMouse/2;
            PaddingRight = PaddingRight - xMouse/2;
            PaddingBottom=PaddingBottom-yMouse;

            ResizeCanvasWidget ();
            SaveMetadata ();

            SaveOriginal ();
            RedrawButton ();

            //HERZUM SPRINT 2.0 TLAB-155
            isResize = true;
            //END HERZUM SPRINT 2.0 TLAB-155

            ShowWidget ();

        }



        private void ResizeScopeNorthEast() {           
            int xMouse;
            int yMouse; 

            GetPointer(out xMouse, out yMouse);

            //xMouse = xMouse - 20;
            //yMouse = yMouse + 50;

            //HERZUM SPRINT 1.2 TLAB-133

            //xMouse = xMouse - 25;
            yMouse = yMouse + 50;

            double lengthLabel = rect.X2 - rect.X - 2*PaddingLeft;

            MoveTo (rect.X, rect.Y+yMouse);
            PaddingLeft = xMouse/2 - lengthLabel/2;
            PaddingRight = xMouse/2 - lengthLabel/2;
            PaddingBottom=PaddingBottom-yMouse;

            ResizeCanvasWidget ();
            SaveMetadata ();
            //END HERZUM SPRINT 1.2 TLAB-133

            SaveOriginal ();
            RedrawButton ();

            //HERZUM SPRINT 2.0 TLAB-155
            isResize = true;
            //END HERZUM SPRINT 2.0 TLAB-155

            ShowWidget();
        }


        protected virtual void ResizeCanvasWidget (){}
        protected virtual void GetPointer(out int xMouse, out int yMouse){
            xMouse = 0;
            yMouse = 0;
        }
        protected virtual void ShowWidget(){}
        protected virtual void HideWidget(){}

        private void SaveMetadata() {
            info.X = rect.X;
            info.Y = rect.Y;
            info.Width = PaddingLeft*2;
            info.Height = PaddingBottom;
            info.WidgetStatus = stateWidget;
        }

        private void SaveOriginal() {
            paddingLeftOriginal=PaddingLeft;
            paddingRightOriginal=PaddingRight;
            paddingBottomOriginal=PaddingBottom;

            // m_scopeCanvasWidget.SetSizeRequest ((int)PaddingRight*2+20, (int)PaddingBottom-30);
            ResizeCanvasWidget ();

            //widthOriginal = m_scopeCanvasWidget.WidthRequest;
            //heightOriginal = m_scopeCanvasWidget.HeightRequest;
            xOriginal = rect.X;           
            yOriginal = rect.Y;
        }
        // ************************************************

 
    }
}

