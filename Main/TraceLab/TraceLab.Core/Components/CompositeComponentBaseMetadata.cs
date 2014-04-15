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
using System.Xml.Serialization;
using System.Xml.XPath;
using TraceLab.Core.Experiments;
using TraceLab.Core.Settings;
using System.Security.Permissions;
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Composite Component Metadata represents metadata of the compound node - node that contains another subgraph.
    /// </summary>
    [Serializable]
    public abstract class CompositeComponentBaseMetadata : Metadata
    {
        protected string m_experimentLocationRoot;

        #region Constructor

        /// <summary>
        /// Initializes a new s_instance of the <see cref="CompositeComponentMetadata"/> class.
        /// </summary>
        protected CompositeComponentBaseMetadata()
        {
            InitLoggingNodeSettings();
        }
        
        #endregion

        public abstract void InitializeComponentGraph(CompositeComponentNode node, TraceLab.Core.Settings.Settings settings);

        public abstract CompositeComponentGraph ComponentGraph
        {
            get;
        }

        #region Logging setting

        protected override void InitLoggingNodeSettings()
        {
            base.InitLoggingNodeSettings();
            ListenToLogLevelChanges();
        }

        /// <summary>
        /// Listen to all its own log setting changes. (of this composite components log levels).
        /// </summary>
        private void ListenToLogLevelChanges()
        {
            foreach (LogLevelItem logLevel in LogLevels)
            {
                logLevel.LogLevelEnableChanged += LogLevelEnableChangedEventHandler;
            }
        }

        /// <summary>
        /// When log level of the composite component node changes, it also sets log level of all its subgraph components
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="TraceLab.Core.Settings.GlobalLogLevelSettingEventArgs"/> instance containing the event data.</param>
        private void LogLevelEnableChangedEventHandler(object sender, EventArgs args)
        {
            LogLevelItem logLevelItem = (LogLevelItem)sender;
            SetLogLevel(logLevelItem.Level, logLevelItem.IsEnabled);
        }

        /// <summary>
        /// Enables or disables the log level for the node, and its all components in the subgraph
        /// </summary>
        /// <param name="logLevel">The log level that is going to be enabled or disabled.</param>
        /// <param name="isEnabled">Enables or disables the given log level.</param>
        public override void SetLogLevel(NLog.LogLevel logLevel, bool isEnabled)
        {
            base.SetLogLevel(logLevel, isEnabled);
            //set log level of all components in the composite graph
            if (ComponentGraph != null)
            {
                foreach (ExperimentNode experimentNode in ComponentGraph.Vertices)
                {
                    experimentNode.Data.Metadata.SetLogLevel(logLevel, isEnabled);
                }
            }
        }

        #endregion
    }
}
