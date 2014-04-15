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
// WindowShell.cs
//  
// Author:
//       Jonathan Pobst <monkey@jpobst.com>
// 
// Copyright (c) 2011 Jonathan Pobst
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;

namespace TraceLab.UI.GTK
{
    public class WindowShell : Window
    {
        private VBox m_shellLayout;
        private VBox m_menuLayout;
        private HBox m_workLayout;

        private MenuBar m_mainMenu;
        private Toolbar m_mainToolbar;
        private ExperimentProgressBar m_mainProgressBar;
        private static Gdk.Pixbuf s_icon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_TraceLab16.png");

        public WindowShell(string name, string title, int width, int height, bool maximize) : base (WindowType.Toplevel)
        {
            Name = name;
            Title = title;
            Icon = s_icon;
            DefaultWidth = width;
            DefaultHeight = height;

            WindowPosition = WindowPosition.Center;
            AllowShrink = true;

            if (maximize)
                Maximize();

            m_shellLayout = new VBox() { Name = "shell_layout" };
            m_menuLayout = new VBox() { Name = "menu_layout" };

            m_shellLayout.PackStart(m_menuLayout, false, false, 0);

            Add (m_shellLayout);

            m_shellLayout.ShowAll();
        }

        public MenuBar CreateMainMenu(string name)
        {
            m_mainMenu = new MenuBar();
            m_mainMenu.Name = name;

            m_menuLayout.PackStart(m_mainMenu, false, false, 0);
            m_mainMenu.Show();

            return m_mainMenu;
        }

        public Toolbar CreateToolBar(string name)
        {
            m_mainToolbar = new Toolbar();
            m_mainToolbar.Name = name;

            m_menuLayout.PackStart (m_mainToolbar, false, false, 0);
            m_mainToolbar.Show();

            return m_mainToolbar;
        }

        public ExperimentProgressBar CreateProgressBar(string name) 
        {
            m_mainProgressBar = new ExperimentProgressBar();
            m_mainProgressBar.Name = name;
            m_shellLayout.PackEnd(m_mainProgressBar, false, false, 0);
            m_mainProgressBar.Show();

            return m_mainProgressBar;
        }

        public ExperimentProgressBar StatusBar
        {
            get { return m_mainProgressBar; }
        }

        public HBox CreateLayout()
        {
            m_workLayout = new HBox();
            m_workLayout.Name = "work_layout";

            m_shellLayout.PackStart (m_workLayout);
            m_workLayout.ShowAll();

            return m_workLayout;
        }

        public void AddDragDropSupport(params TargetEntry[] entries)
        {
            Gtk.Drag.DestSet (this, Gtk.DestDefaults.Motion | Gtk.DestDefaults.Highlight | Gtk.DestDefaults.Drop, entries, Gdk.DragAction.Copy);
        }
    }
}
