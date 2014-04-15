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
using MonoHotDraw.Handles;
using MonoHotDraw;
using MonoHotDraw.Figures;
using Cairo;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace TraceLab.UI.GTK
{
    public class NewConnectionHandle: PixbufHandle 
    {
        private IConnectionFigure m_connection;
        private IHandle m_handle;
        private static Gdk.Pixbuf s_arrowIcon;
        private ApplicationContext m_applicationContext;

        static NewConnectionHandle() 
        {
            s_arrowIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.arrow.png");
        }

        public NewConnectionHandle(IFigure owner, ApplicationContext applicationContext, ILocator locator): base (owner, locator, s_arrowIcon) 
        {
            m_applicationContext = applicationContext;
        }
                
        public override void InvokeStart(MouseEvent ev) 
        {
            m_connection = CreateConnection();
            m_connection.EndPoint = new PointD (ev.X, ev.Y);
            m_connection.StartPoint = new PointD (ev.X, ev.Y);
            m_connection.ConnectStart (Owner.ConnectorAt(ev.X, ev.Y));
            m_connection.UpdateConnection();
            ev.View.Drawing.Add(m_connection);
            ev.View.ClearSelection();
            ev.View.AddToSelection(m_connection);
            m_handle = ev.View.FindHandle(ev.X, ev.Y);
        }
        
        public override void InvokeStep(MouseEvent ev) 
        {
            if (m_handle != null) {
                m_handle.InvokeStep(ev);
            }
        }
        
        public override void InvokeEnd(MouseEvent ev) 
        {
            if (m_handle != null) {
                m_handle.InvokeEnd(ev);
            }
            
            if (m_connection.EndConnector == null) {
                m_connection.DisconnectStart ();
                m_connection.DisconnectEnd ();
                ev.View.Drawing.Remove(m_connection);
                ev.View.ClearSelection();
            }
        }
                
        private IConnectionFigure CreateConnection() 
        {
            NodeConnectionControl connection = m_applicationContext.NodeConnectionControlFactory.CreateNewNodeConnectionControl();
            connection.IsEditable = true;
            return connection;
        }
    }
}

