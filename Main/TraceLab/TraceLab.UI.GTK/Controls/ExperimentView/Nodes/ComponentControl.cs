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
    public class ComponentControl: BasicNodeControl
    {
        public ComponentControl(ExperimentNode node, ApplicationContext applicationContext) : base(node, applicationContext)
        {
            InitControlButtons(applicationContext);
            AttachListenerToIOSpecHighlights(node);
        }

        protected ComponentControl(ExperimentNode node, ApplicationContext applicationContext, 
                                   double waitForAnyAllHandleXLocation, 
                                   double waitForAnyAllHandleYLocation) 
            : base(node, applicationContext, waitForAnyAllHandleXLocation, waitForAnyAllHandleYLocation)
        {
            InitControlButtons(applicationContext);
            AttachListenerToIOSpecHighlights(node);
        }

        private void InitControlButtons(ApplicationContext applicationContext)
        {
            m_controlButtons = new NodeControlButtons (this, applicationContext);
            m_controlButtons.InfoButton.Toggled += OnInfoToggled;
        }

        private void AttachListenerToIOSpecHighlights(ExperimentNode node)
        {
            //set convenience metadata field
            m_componentMetadata = node.Data.Metadata as IConfigurableAndIOSpecifiable;
            if(m_componentMetadata != null)
            {
                m_componentMetadata.IOSpec.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => 
                {
                    if(e.PropertyName.Equals("IsInputHighlighted") || e.PropertyName.Equals("IsOutputHighlighted"))
                    {
                        //redraw component
                        Invalidate();
                    }
                };
            }
        }

        #region Info Toggled

        /// <summary>
        /// When info handle is toggled it opens the info pad.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void OnInfoToggled(object sender, ToggleEventArgs args) 
        {
            if(args.Active) 
            {
                // get the default location for the floating window from the mouse event 
                // the location is relative to 0.0 screen origin point
                int defaultX = 500;
                int defaultY = 500;
                if(args.MouseEvent != null) 
                {
                    Gdk.EventButton gdkEvent = args.MouseEvent.GdkEvent as Gdk.EventButton;
                    if(gdkEvent != null) 
                    {
                        defaultX = (int)gdkEvent.XRoot;
                        defaultY = (int)gdkEvent.YRoot;
                    }
                }

                m_applicationContext.MainWindow.ShowComponentInfoPad(this, defaultX, defaultY, ToggleOffOnInfoClosed);
            } 
            else
            {
                m_applicationContext.MainWindow.HideComponentInfoPad(this);
            }
        }

        private void ToggleOffOnInfoClosed(bool infoVisibility) 
        {
            //only react if info visibility has been changed to false 
            if(infoVisibility == false) 
            {
                //since info pan visibility may be changed by toggling info icon, OR it may changed if info box is closed by clicking [X] button
                //check if toggle icon is alrady off
                if(m_controlButtons.InfoButton.Active == true)
                {
                    m_controlButtons.InfoButton.Active = false;
                    Invalidate(); //refreshes and redraws control
                }
            }
        }

        #endregion

        protected override void DrawFrame (Cairo.Context context, double lineWidth, Cairo.Color lineColor, Cairo.Color fillColor)
        {
            base.DrawFrame (context, lineWidth, lineColor, fillColor);

            //also draw input output indicators around the frame
            DrawIOIndicators(context);
        }

        private void DrawIOIndicators(Context context)
        {
            if(m_componentMetadata != null)
            {
                if (m_componentMetadata.IOSpec.IsInputHighlighted) 
                {
                    DrawInputndicator (context);
                }

                if (m_componentMetadata.IOSpec.IsOutputHighlighted) 
                {
                    DrawOutputndicator (context);
                }
            }
        }

        #region Draw IO Indicators

        private void DrawInputndicator(Cairo.Context context)
        {
            context.Save();
            RelativeLocator locator = new RelativeLocator(-0.03, -0.1);
            PointD point = locator.Locate(this);

            context.MoveTo(point);
            context.LineCap = LineCap.Round;
            context.Color = s_ioIndicatorColor;

            double l = 4; //arm lenght of indicator icon
            double s = 0.4; //spacing between arms in the indicator icon

            //draw >>
            //up
            context.RelLineTo (new Distance (s, -l));
            context.RelMoveTo (new Distance (-s, l)); //back

            //left
            context.RelLineTo (new Distance (-l, s));
            context.RelMoveTo (new Distance (l, -s)); //back

            context.RelMoveTo (new Distance (3, 3));

            //repeat above for second arrow
            context.RelLineTo (new Distance (s, -l));
            context.RelMoveTo (new Distance (-s, l)); //back
            context.RelLineTo (new Distance (-l, s));
            context.RelMoveTo (new Distance (l, -s)); //back

            context.Stroke();

            context.Restore();
        }

        private void DrawOutputndicator(Cairo.Context context)
        {
            context.Save();
            RelativeLocator locator = new RelativeLocator(-0.04, 1.2);
            PointD point = locator.Locate(this);

            context.MoveTo(point);
            context.LineCap = LineCap.Round;
            context.Color = s_ioIndicatorColor;

            double l = 4; //arm lenght of indicator icon
            double s = 0.4; //spacing between arms in the indicator icon

            //draw <<
            //up
            context.RelLineTo (new Distance (-s, -l));
            context.RelMoveTo (new Distance (s, l)); //back

            //right
            context.RelLineTo (new Distance (l, s));
            context.RelMoveTo (new Distance (-l, -s)); //back
            
            context.RelMoveTo (new Distance (3, -3));
            
            //repeat above for second arrow
            context.RelLineTo (new Distance (-s, -l));
            context.RelMoveTo (new Distance (s, l)); //back
            context.RelLineTo (new Distance (l, s));
            context.RelMoveTo (new Distance (-l, -s)); //back
            
            context.Stroke();

            context.Restore();
        }

        #endregion

        /// <summary>
        /// Gets the handles enumerator of all the icons:
        /// new connection, remove, and info
        /// </summary>
        /// <value>The handles enumerator.</value>
        public override IEnumerable<IHandle> HandlesEnumerator 
        {
            get 
            { 
                if(IsEditable) 
                {
                    foreach (IHandle handle in base.HandlesEnumerator) 
                    {
                        yield return handle;
                    }
                    
                    foreach(IHandle handle in m_controlButtons.ControlButtons) 
                    {
                        yield return handle;
                    }
                } 
                else 
                {
                    //show only info button
                    yield return m_controlButtons.InfoButton;
                }
            }
        }

        protected Gtk.Menu m_contextMenu;

        public override ITool CreateFigureTool(IPrimaryToolDelegator mainTool, IDrawingEditor editor, 
                                               ITool defaultTool, MouseEvent ev)
        {
            if(ev.IsRightButtonPressed())
            {
                PopupContextMenu(mainTool, editor, defaultTool, ev);
                return null;
            } 
            else
            {
                return base.CreateFigureTool(mainTool, editor, defaultTool, ev);
            }
        }

        private void PopupContextMenu(IPrimaryToolDelegator mainTool, IDrawingEditor editor, ITool dt, MouseEvent ev)
        {
            m_contextMenu = new Gtk.Menu();
            Gtk.MenuItem editLabel = new Gtk.MenuItem("Edit label");

            editLabel.Activated += delegate(object sender, EventArgs e) 
            {
                SimpleTextTool textTool = new SimpleTextTool(editor, this, dt, ev);
                mainTool.DelegateTool = textTool;
                textTool.StartEditing();
            };
    
            m_contextMenu.Add(editLabel);
            m_contextMenu.ShowAll();

            m_contextMenu.Popup();
        }

        private NodeControlButtons m_controlButtons;

        //convenience shortcut for component metadata; same as (IConfigurableAndIOSpecifiable)this.ExperimentNode.Data.Metadata 
        private IConfigurableAndIOSpecifiable m_componentMetadata;
        private static Color s_ioIndicatorColor = new Color (70.0/255, 130.0/255, 180.0/255, 1);


    }
}

