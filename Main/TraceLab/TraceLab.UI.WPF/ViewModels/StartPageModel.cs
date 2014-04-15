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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TraceLab.Core.Experiments;
using TraceLab.Core.ViewModels;

namespace TraceLab.UI.WPF.ViewModels
{
    internal class StartPageModel : INotifyPropertyChanged
    {
        #region Members

        /// <summary>
        /// Wrapper for the Application View Model
        /// </summary>
        private ApplicationViewModelWrapper m_wrapper;

        /// <summary>
        /// Command for creating a new experiment
        /// </summary>
        private ICommand m_new;
        public ICommand New
        {
            get { return this.m_new; }
            private set
            {
                this.m_new = value;
                this.NotifyPropertyChanged("New");
            }
        }

        /// <summary>
        /// Command for openning an existing experiment
        /// </summary>
        private ICommand m_open;
        public ICommand Open
        {
            get { return this.m_open; }
            private set
            {
                this.m_open = value;
                this.NotifyPropertyChanged("Open");
            }
        }

        /// <summary>
        /// List of experiments recently opened sorted by date
        /// </summary>
        private RecentExperimentList m_recentExperiments;
        public RecentExperimentList RecentExperiments
        {
            get { return this.m_recentExperiments; }
        }

        /// <summary>
        /// List of video links
        /// </summary>
        private List<WebsiteLink> m_videos;
        public List<WebsiteLink> Videos
        {
            get { return this.m_videos; }
        }

        /// <summary>
        /// List of website links
        /// </summary>
        private List<WebsiteLink> m_links;
        public List<WebsiteLink> Links
        {
            get { return this.m_links; }
        }

        #endregion

        #region Methods

        public StartPageModel(ApplicationViewModelWrapper wrapper)
        {
            this.m_wrapper = wrapper;
            this.m_new = m_wrapper.New;
            this.m_open = m_wrapper.Open;

            this.m_recentExperiments = m_wrapper.RecentExperiments;
            this.m_links = m_wrapper.Links;
            this.m_videos = m_wrapper.Videos;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
