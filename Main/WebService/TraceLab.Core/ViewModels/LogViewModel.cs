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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace TraceLab.Core.ViewModels
{
    public class LogInfo
    {
        /// <summary>
        /// Gets or sets the user friendly name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName { get; set; }
        
        /// <summary>
        /// Gets or sets the detailed source of the log. If it comes from nodes it contains {Experiment id}:{node id}::{label}
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }
    }

    public class ComponentLogInfo : LogInfo
    {
        public string AssemblySource { get; set; }

        /// <summary>
        /// Gets or sets the name of the component, not the label. 
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string ComponentName { get; set; }
    }
    
    public sealed class LogViewModel : INotifyPropertyChanged
    {
        internal string ExperimentId;
        private ObservableCollection<LogInfo> m_events = new ObservableCollection<LogInfo>();
        private ReadOnlyObservableCollection<LogInfo> m_roEvents;

        public ReadOnlyObservableCollection<LogInfo> Events
        {
            get { return m_roEvents; }
            private set
            {
                if (m_roEvents != value)
                {
                    m_roEvents = value;
                    NotifyPropertyChanged("Events");
                }
            }
        }

        private static void CreateTraceLabLoggingRule()
        {
            // Enable all messages from TraceLab
            var allErrorsRule = new NLog.Config.LoggingRule();
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Info);
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Trace);
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Debug);
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Warn);
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Error);
            allErrorsRule.EnableLoggingForLevel(NLog.LogLevel.Fatal);
            allErrorsRule.LoggerNamePattern = "TraceLab*";
            NLog.LogManager.Configuration.LoggingRules.Add(allErrorsRule);
        }

        public static void InitLogging()
        {
            NLog.LogManager.Configuration = new NLog.Config.LoggingConfiguration();

            CreateTraceLabLoggingRule();
            
            NLog.LogManager.EnableLogging();
        }

        public LogViewModel() : this(null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewModel"/> class.
        /// </summary>
        private LogViewModel(string experimentId)
        {
            m_roEvents = new ReadOnlyObservableCollection<LogInfo>(m_events);
                        
            var target = new OutputControlTarget(this);
            AsyncTargetWrapper asyncTargetWrapper = new AsyncTargetWrapper(target, 100, AsyncTargetWrapperOverflowAction.Grow);
            AddTargetToTraceLabRule(asyncTargetWrapper);

            if (NLog.LogManager.Configuration == null)
            {
                NLog.LogManager.Configuration = new NLog.Config.LoggingConfiguration();
            }

            if (experimentId != null)
            {
                ExperimentId = experimentId;
                NLog.LogManager.Configuration.AddTarget(ExperimentId, asyncTargetWrapper);
            }

            // Make sure the target we just added gets configured.
            NLog.LogManager.ReconfigExistingLoggers();
        }
                
        public LogViewModel(string experimentId, LogViewModel oldModel) : this(experimentId)
        {
            if (oldModel != null)
            {
                foreach (LogInfo logEvent in oldModel.m_events)
                {
                    m_events.Add(logEvent);
                }
            }
        }

        private void AddTargetToTraceLabRule(AsyncTargetWrapper targetWrapper)
        {
            // Try to find the rule
            foreach (NLog.Config.LoggingRule rule in NLog.LogManager.Configuration.LoggingRules)
            {
                if (rule.LoggerNamePattern == "TraceLab*")
                {
                    rule.Targets.Add(targetWrapper);
                    break;
                }
            }
        }

        internal void Write(NLog.LogEventInfo logEvent)
        {
            string logEventLoggerFullName = logEvent.LoggerName;

            //filtered events, and add to events only these events coming from experiment of this logViewModel, or from TraceLab application
            if (logEventLoggerFullName.StartsWith("TraceLab") || logEventLoggerFullName.StartsWith(ExperimentId))
            {
                string userFriendlySource;

                LogInfo info;
                if (logEventLoggerFullName.StartsWith("TraceLab"))
                {
                    userFriendlySource = logEventLoggerFullName;
                    info = new LogInfo { SourceName = userFriendlySource, Source = logEventLoggerFullName, Level = logEvent.Level, Message = logEvent.FormattedMessage, Exception = logEvent.Exception };
                }
                else
                {
                    string sourceAssembly, componentName;
                    TraceLab.Core.Components.LoggerFactory.ExtractLogSourceInfo(logEventLoggerFullName, out componentName, out sourceAssembly, out userFriendlySource);
                    info = new ComponentLogInfo
                    {
                        SourceName = userFriendlySource,
                        Source = logEventLoggerFullName,
                        Level = logEvent.Level,
                        Message = logEvent.FormattedMessage,
                        Exception = logEvent.Exception,
                        ComponentName = componentName,
                        AssemblySource = sourceAssembly
                    };

                    if (logEvent.Exception != null)
                    {
                        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(logEvent.Exception);
                    }
                }   
                
                m_events.Add(info);
            }
        }

        public void Clear()
        {
            m_events.Clear();
        }

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Internal log target

        internal class OutputControlTarget : Target
        {
            private LogViewModel m_model;

            internal OutputControlTarget(LogViewModel model)
                : base()
            {
                m_model = model;

                if (String.IsNullOrWhiteSpace(model.ExperimentId) == false)
                {
                    Name = m_model.ExperimentId.ToString();
                }
                else
                {
                    Name = new Guid().ToString();
                }
            }

            protected override void Write(NLog.Common.AsyncLogEventInfo[] logEvents)
            {
                var builder = new StringBuilder();
                for (int i = 0; i < logEvents.Length; ++i)
                {
                    m_model.Write(logEvents[i].LogEvent);
                }
            }

            protected override void Write(NLog.LogEventInfo logEvent)
            {
                m_model.Write(logEvent);
            }

            protected override void Write(NLog.Common.AsyncLogEventInfo logEvent)
            {
                m_model.Write(logEvent.LogEvent);
            }
        }

        #endregion

        /// <summary>
        /// Destroies the TraceLab Rule targets initiated by all LogViewModels - 
        /// Potentially asynchronous targets (AsyncTargetWrapper) 
        /// may wait forever preventing application to quit - especially on mono.
        /// </summary>
        public static void DestroyLogTargets()
        {
            //destroy all logging rules (particulary rule TraceLab* initiated by LogViewModel 
            foreach (NLog.Config.LoggingRule rule in NLog.LogManager.Configuration.LoggingRules)
            {
                rule.Targets.Clear();
            }
        }
    }
}
