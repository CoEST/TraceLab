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
using System.IO;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Data structure for holding description of a file (path, last modification date, size)
    /// </summary>
    [Serializable]
    class FileDescriptor
    {
        #region Members

        protected string m_absolutePath;
        public string AbsolutePath
        {
            get
            {
                return this.m_absolutePath;
            }
        }

        protected DateTime m_lastModificationTime;
        public DateTime LastModificationTime
        {
            get
            {
                return this.m_lastModificationTime;
            }
        }

        protected long m_size;
        public long Size
        {
            get
            {
                return this.m_size;
            }
        }

        #endregion Members

        public FileDescriptor(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                this.m_absolutePath = file.FullName;
                this.m_lastModificationTime = file.LastWriteTime;
                this.m_size = file.Length;
            }
            else
            {
                throw new FileNotFoundException("Type file \"" + filePath + "\" was not found.");
            }
        }

        /// <summary>
        /// Determines whether the file has been modified from the last time it was read.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if file is up to date; otherwise, <c>false</c>.
        /// </returns>
        public bool isUpToDate()
        {
            FileInfo info = new FileInfo(this.m_absolutePath);

            return (info.Exists && (DateTime.Compare(this.m_lastModificationTime, info.LastWriteTime) == 0)
                    && (this.m_size == info.Length));
        }

        /// <summary>
        /// Updates the file descriptor information.
        /// </summary>
        public void update()
        {
            FileInfo info = new FileInfo(this.m_absolutePath);
            this.m_lastModificationTime = info.LastWriteTime;
            this.m_size = info.Length;
        }
    }
}
