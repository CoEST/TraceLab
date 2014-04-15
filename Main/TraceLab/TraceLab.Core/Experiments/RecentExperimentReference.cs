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
using System.Linq;
using System.Text;
using System.Xml;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// Collection of RecentExperimentReferences for displaying on TraceLab start
    /// </summary>
    public class RecentExperimentList : System.Collections.Generic.LinkedList<RecentExperimentReference>
    {
        private const int MAX_NUMBER_ITEMS = 10;

        public void Add(RecentExperimentReference item)
        {
            if (this.Count >= MAX_NUMBER_ITEMS)
            {
                this.RemoveLast();
            }

            base.AddFirst(item);
        }
    }

    /// <summary>
    /// Holds filepath and last access date for an experiment recently opened.
    /// </summary>
    public class RecentExperimentReference : IEquatable<RecentExperimentReference>
    {
        #region Members

        /// <summary>
        /// Complete path of experiment file
        /// </summary>
        private string m_fullPath;
        public string FullPath
        {
            get { return this.m_fullPath; }
        }

        /// <summary>
        /// Filename of experiment
        /// </summary>
        private string m_filename;
        public string Filename
        {
            get { return this.m_filename; }
        }

        /// <summary>
        /// Last time experiment file was accessed
        /// </summary>
        private DateTime m_lastAccessTime;
        public DateTime LastAccessTime
        {
            get { return this.m_lastAccessTime; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentExperimentReference"/> class from
        /// the path given and with the current time.
        /// </summary>
        /// <param name="path">The path of the experiment file.</param>
        public RecentExperimentReference(string path)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(path);

            this.m_fullPath = info.FullName;
            this.m_filename = info.Name;
            this.m_lastAccessTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentExperimentReference"/> class from
        /// the path given and time.
        /// </summary>
        /// <param name="path">The path of the experiment file.</param>
        /// <param name="time">The last access time of the experiment.</param>
        public RecentExperimentReference(string path, string time)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(path);

            this.m_fullPath = info.FullName;
            this.m_filename = info.Name;
            this.m_lastAccessTime = DateTime.Parse(time);
        }

        /// <summary>
        /// Creates the recent experiment item from the given path and time.
        /// </summary>
        /// <param name="path">The path of the experiment file.</param>
        /// <param name="time">The last access time of the experiment.</param>
        /// <returns></returns>
        public static RecentExperimentReference CreateRecentExperimentItem(string path, string time)
        {
            RecentExperimentReference newItem = null;

            if (System.IO.File.Exists(path))
            {
                newItem = new RecentExperimentReference(path, time);
            }

            return newItem;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the experiment filepath is equal to the <paramref name="other"/> experiment filepath; otherwise, false.
        /// </returns>
        public bool Equals(RecentExperimentReference other)
        {
            return this.m_fullPath.Equals(other.m_fullPath);
        }

        /// <summary>
        /// On Mono IEquatable method is not called when checking list.Contains,
        /// thus we override Equals
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="TraceLab.Core.Experiments.RecentExperimentReference"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="TraceLab.Core.Experiments.RecentExperimentReference"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(Object obj)
        {
            RecentExperimentReference other = obj as RecentExperimentReference; 
            if (other == null)
                return false;
            else 
                return Equals(other);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="TraceLab.Core.Experiments.RecentExperimentReference"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return this.m_fullPath.GetHashCode(); 
        }

        #endregion
    }
}
