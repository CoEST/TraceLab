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
using Gtk;
using TraceLab.Core.ViewModels;
using System.Diagnostics;

namespace TraceLab.UI.GTK
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class WelcomePageWidget : Gtk.Bin
    {
        public WelcomePageWidget ()
        {
            this.Build ();

            //In Gtk it is possible to change the background of "window" widgets. 
            //Since Vbox is a window-less widget, we put main VBox inside an EventBox
            //widget (that widget will just provide you with a window).
            //thus we can change background to white color
            mainEventBox.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(255,255,255));
        }

        public WelcomePageWidget(ApplicationContext applicationContext) : this()
        {
            m_applicationContext = applicationContext;
            m_applicationContext.Actions.File.New.ConnectProxy(this.buttonCreateNewExperiment);
            m_applicationContext.Actions.File.Open.ConnectProxy(this.buttonOpenExperiment);

            SetRecentExperimentList();
            SetOnlineResourcesList();
            SetVideosTutorialsList();
        }

        #region Recent Experiments

        private void SetRecentExperimentList() 
        {
            Gtk.ListStore recentExperimentsStore = new Gtk.ListStore(typeof (Gdk.Pixbuf), typeof(RecentExperimentReference));
            this.recentExperimentNodeView.Model = recentExperimentsStore;

            this.recentExperimentNodeView.AppendColumn("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);

            CellRendererText fileNameRenderer = new CellRendererText();
            TreeViewColumn nameColumn = this.recentExperimentNodeView.AppendColumn("Filename", fileNameRenderer);
            nameColumn.SetCellDataFunc(fileNameRenderer, new TreeCellDataFunc(RenderFileName));

            foreach(RecentExperimentReference expRef in m_applicationContext.Application.RecentExperiments)
            {
                recentExperimentsStore.AppendValues(s_traceLabIcon, expRef);
            }

            this.recentExperimentNodeView.RowActivated += OpenExperimentOnRowActivated;
        }

        private void RenderFileName(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            RecentExperimentReference experimentReference = (RecentExperimentReference)model.GetValue(iter, 1);
            (cell as CellRendererText).Text = experimentReference.Filename;
        }

        /// <summary>
        /// Opens experiment when double clicked on the row
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="args">Arguments.</param>
        private void OpenExperimentOnRowActivated(object source, RowActivatedArgs args)
        {
            TreeIter item;
            if(this.recentExperimentNodeView.Selection.GetSelected(out item)) 
            {
                RecentExperimentReference expRef = (RecentExperimentReference)this.recentExperimentNodeView.Model.GetValue(item, 1);
                OpenExperimentAction.OpenExperiment(expRef.FullPath, m_applicationContext);
            }
        }

        #endregion Recent Experiments

        #region Online Resources & Videos

        private void SetOnlineResourcesList() 
        {
            Gtk.ListStore onlineResourcesStore = new Gtk.ListStore(typeof (Gdk.Pixbuf), typeof(WebsiteLink));

            InitLinksNodeView(onlineResourcesStore, this.onlineResourcesNodeView);

            foreach(WebsiteLink link in m_applicationContext.Application.Links)
            {
                onlineResourcesStore.AppendValues(s_websiteLinkIcon, link);
            }
        }

        private void SetVideosTutorialsList() 
        {
            Gtk.ListStore videosTutorials = new Gtk.ListStore(typeof (Gdk.Pixbuf), typeof(WebsiteLink));
            
            InitLinksNodeView(videosTutorials, this.videoTutorialsTreeView);
            
            foreach(WebsiteLink link in m_applicationContext.Application.Videos)
            {
                videosTutorials.AppendValues(s_videosTutorialLinkIcon, link);
            }
        }

        private void InitLinksNodeView(Gtk.ListStore store, NodeView nodeView)
        {
            nodeView.Model = store;
            nodeView.AppendColumn("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);
            
            CellRendererText websiteLinkRenderer = new CellRendererText();
            websiteLinkRenderer.WrapMode = Pango.WrapMode.Word;
            TreeViewColumn websiteColumn = nodeView.AppendColumn("Website Link", websiteLinkRenderer);
            websiteColumn.SetCellDataFunc(websiteLinkRenderer, new TreeCellDataFunc(RenderWebsiteLink));

            nodeView.RowActivated += OpenWebsiteLink;
        }

        private void RenderWebsiteLink(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            WebsiteLink websiteLink = (WebsiteLink)model.GetValue(iter, 1);
            (cell as CellRendererText).Text = websiteLink.Title + "\n" + websiteLink.Description;
        }

        /// <summary>
        /// Opens experiment when double clicked on the row
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="args">Arguments.</param>
        private void OpenWebsiteLink(object source, RowActivatedArgs args)
        {
            TreeIter item;
            NodeView nodeView = (NodeView)source;
            if(nodeView.Selection.GetSelected(out item)) 
            {
                WebsiteLink link = (WebsiteLink)nodeView.Model.GetValue(item, 1);
                link.OpenLink();
            }
        }

        #endregion Online Resources & Videos

        private static Gdk.Pixbuf s_traceLabIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_TraceLab16.png");
        private static Gdk.Pixbuf s_websiteLinkIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Link32.png");
        private static Gdk.Pixbuf s_videosTutorialLinkIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Link32.png");
        private ApplicationContext m_applicationContext;
    }
}

