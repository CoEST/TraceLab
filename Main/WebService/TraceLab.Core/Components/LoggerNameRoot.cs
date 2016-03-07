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

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Logger name root is a name that consists of experiment id and all subgraphs ids delimited by colon ':'
    /// </summary>
    public class LoggerNameRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerNameRoot"/> class.
        /// LoggerNameRoot always starts with the top experiment id.
        /// </summary>
        /// <param name="topExperimentId">The top experiment id.</param>
        public LoggerNameRoot(string topExperimentId)
        {
            m_experimentPathRoot = topExperimentId;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LoggerNameRoot"/> class from being created.
        /// </summary>
        /// <param name="experimentPathRoot">The experiment path root.</param>
        /// <param name="userFriendlyName">Name of the user friendly.</param>
        private LoggerNameRoot(string experimentPathRoot, string userFriendlyName)
        {
            m_experimentPathRoot = experimentPathRoot;
            m_userFriendlyRoot = userFriendlyName;
        }

        /// <summary>
        /// Creates the new instance of logger name root for composite graph, based on this LoggerName with addition of owner id and owner node label
        /// </summary>
        /// <param name="compositeComponentMetadata">The composite component metadata.</param>
        /// <returns></returns>
        public LoggerNameRoot CreateLoggerNameRootForCompositeNode(CompositeComponentBaseMetadata compositeComponentMetadata)
        {
            string ownerNodeId = compositeComponentMetadata.ComponentGraph.OwnerNode.ID;
            string ownerNodeLabel = compositeComponentMetadata.Label;

            string experimentPathRoot = String.Format("{0}:{1}", m_experimentPathRoot, ownerNodeId);
            string userFriendlyRoot;
            if (String.IsNullOrEmpty(m_userFriendlyRoot))
            {
                //it is first sub level
                userFriendlyRoot = ownerNodeLabel;
            }
            else
            {
                userFriendlyRoot = String.Format("{0} : {1}", m_userFriendlyRoot, ownerNodeLabel);
            }

            return new LoggerNameRoot(experimentPathRoot, userFriendlyRoot);
        }

        /// <summary>
        /// Gets or sets the root of the logger name that consists of Ids of the experiments and composite graphs, delimited by delimiter.
        /// </summary>
        private readonly string m_experimentPathRoot;

        public string ExperimentPathRoot
        {
            get { return m_experimentPathRoot; }
        } 

        /// <summary>
        /// the user friendly root of the logger name, that consists of the name of composite graphs, delimited by delimiter.
        /// </summary>
        private readonly string m_userFriendlyRoot;

        public string UserFriendlyRoot
        {
            get { return m_userFriendlyRoot; }
        } 

    }
}
