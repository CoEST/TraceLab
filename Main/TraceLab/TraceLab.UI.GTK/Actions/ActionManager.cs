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

using System;
using Gtk;
using TraceLab.UI.GTK.Extensions;
using TraceLab.Core.ViewModels;
using System.Diagnostics;
using System.Collections.Generic;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.GTK
{
    public class ActionManager
    {
        public ActionManager(ApplicationContext applicationContext)
        {
            m_applicationContext = applicationContext;
            File = new FileActions();
        }

        public void CreateToolBar(Gtk.Toolbar toolbar)
        {
            toolbar.AppendItem(File.New.CreateToolBarItem());
            toolbar.AppendItem(File.Open.CreateToolBarItem());

            ToolItem recentExperimentButton = ConstrucRecentExperimentsMenuButton();
            toolbar.AppendItem(recentExperimentButton);
            toolbar.AppendItem(File.Save.CreateToolBarItem());
            toolbar.AppendItem(File.SaveAs.CreateToolBarItem());

            toolbar.AppendItem(new SeparatorToolItem());

            toolbar.AppendItem(File.PackageBuilder.CreateToolBarItem());
	        toolbar.AppendItem(File.Settings.CreateToolBarItem());

            ToolItem helpMenuButton = ConstructHelpMenuButton();
            toolbar.AppendItem(helpMenuButton);
        }

        #region Recent Experiments

        private MenuButton ConstrucRecentExperimentsMenuButton()
        {
            Menu recentExperimentsMenu = new Menu();

            foreach (RecentExperimentReference recentExp in m_applicationContext.Application.RecentExperiments) 
            {
                RecentExperimentMenuItem item = new RecentExperimentMenuItem(recentExp);
                item.Image = new Image(s_traceLabIcon);
                item.ExposeEvent += GtkMenuHelper.DrawImageMenuItemImage; 
                item.Activated += OpenExperiment;
                recentExperimentsMenu.Append(item);
            }

            recentExperimentsMenu.ShowAll();
            MenuButton recentExperimentButton = new MenuButton(new Image(Stock.Open, IconSize.SmallToolbar), recentExperimentsMenu, true);
            recentExperimentButton.TooltipText = "Select recent opened experiment";
            return recentExperimentButton;
        }

        private void OpenExperiment(object sender, EventArgs e)
        {
            RecentExperimentMenuItem item = (RecentExperimentMenuItem)sender;
            OpenExperimentAction.OpenExperiment(item.RecentExperimentReference.FullPath, m_applicationContext);
        }

        #endregion

        #region Help Menu

        private MenuButton ConstructHelpMenuButton()
        {
            Menu helpMenu = new Menu();

            MenuItem onlineResources = ConstructSubmenu("Online Resources", 
                                                        m_applicationContext.Application.Links, 
                                                        s_onlineLinkIcon);

            MenuItem videosTutorials = ConstructSubmenu("Videos Tutorials", 
                                                        m_applicationContext.Application.Videos, 
                                                        s_videoLinkIcon);
            helpMenu.Append(onlineResources);
            helpMenu.Append(videosTutorials);
            helpMenu.ShowAll();
            MenuButton helpMenuButton = new MenuButton(new Image(s_helpIcon), helpMenu, false);
            return helpMenuButton;
        }

        private MenuItem ConstructSubmenu(string menuLabel, IEnumerable<WebsiteLink> links, Gdk.Pixbuf linkIcon)
        {
            ImageMenuItem onlineResources = new ImageMenuItem(menuLabel);
            onlineResources.Image = new Image(linkIcon);
            onlineResources.ExposeEvent += GtkMenuHelper.DrawImageMenuItemImage;

            Menu onlineResourcesSubmenu = new Menu();
            onlineResources.Submenu = onlineResourcesSubmenu;
            foreach (WebsiteLink websiteLink in links) 
            {
                WebsiteLinkMenuItem linkItem = new WebsiteLinkMenuItem(websiteLink);
                linkItem.Image = new Image(linkIcon);
                linkItem.ExposeEvent += GtkMenuHelper.DrawImageMenuItemImage; 
                linkItem.Activated += OpenLinkActivated;
                onlineResourcesSubmenu.Append(linkItem);
            }
            return onlineResources;
        }

        private void OpenLinkActivated(object sender, EventArgs e)
        {
            WebsiteLinkMenuItem linkItem = (WebsiteLinkMenuItem)sender;
            linkItem.Link.OpenLink();
        }

        #endregion

        public void RegisterHandlers()
        {
            File.RegisterHandlers();
        }

        public FileActions File 
        { 
            get; 
            private set; 
        }
                
        private ApplicationContext m_applicationContext;
        private static Gdk.Pixbuf s_traceLabIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_TraceLab16.png");
        private static Gdk.Pixbuf s_onlineLinkIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Link16.png");
        private static Gdk.Pixbuf s_videoLinkIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Video16.png");
        private static Gdk.Pixbuf s_helpIcon = Gdk.Pixbuf.LoadFromResource("TraceLab.UI.GTK.Resources.Icon_Help16.png");
    }

    class WebsiteLinkMenuItem : ImageMenuItem
    {
        public WebsiteLinkMenuItem(WebsiteLink link) : base(link.Title)
        {
            m_websiteLink = link;
        }

        private WebsiteLink m_websiteLink;
        public WebsiteLink Link
        {
            get { return m_websiteLink; }
        }
    }

    class RecentExperimentMenuItem : ImageMenuItem
    {
        public RecentExperimentMenuItem(RecentExperimentReference recentExperimentReference) 
            : base(recentExperimentReference.Filename)
        {
            m_recerentExperimentReference = recentExperimentReference;
        }

        private RecentExperimentReference m_recerentExperimentReference;
        public RecentExperimentReference RecentExperimentReference
        {
            get { return m_recerentExperimentReference; }
        }
    }

}

