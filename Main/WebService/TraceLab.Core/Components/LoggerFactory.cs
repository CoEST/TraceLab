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
using TraceLabSDK;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Logger factory consists of method assisting in logger creation for experiments nodes based on their loggable metadata
    /// </summary>
    internal static class LoggerFactory
    {
        /// <summary>
        /// Creates the logger for the given metadata and node id
        /// </summary>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="loggableMetadata">The metadata.</param>
        /// <returns></returns>
        internal static ComponentLogger CreateLogger(LoggerNameRoot loggerNameRoot, string nodeId, ILoggable loggableMetadata)
        {
            string loggerName = GetLoggerSourceInfo(nodeId, loggableMetadata, loggerNameRoot);
            CreateLoggingRules(loggerName, loggableMetadata.LogLevels);

            if (string.IsNullOrEmpty(loggerName))
                loggerName = Guid.NewGuid().ToString();
            
            return new ComponentLoggerImplementation(loggerName);
        }

        /// <summary>
        /// Destroys the logger.
        /// </summary>
        /// <param name="logger">The logger.</param>
        internal static void DestroyLogger(ComponentLogger logger)
        {
            ComponentLoggerImplementation loggerImpl = logger as ComponentLoggerImplementation;
            if (loggerImpl != null)
            {
                NLog.Config.LoggingRule loggingRule = TryFindRule(loggerImpl.Name);

                lock (lockLoggingRules)
                {
                    NLog.LogManager.Configuration.LoggingRules.Remove(loggingRule);
                }
            }
        }

        /// <summary>
        /// Creates the logging rules for each log level for given logger name
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="logLevels">The log levels.</param>
        private static void CreateLoggingRules(string loggerName, IEnumerable<TraceLab.Core.Settings.LogLevelItem> logLevels)
        {
            NLog.Config.LoggingRule loggingRule = null;

            foreach (TraceLab.Core.Settings.LogLevelItem logLevelItem in logLevels)
            {
                if (logLevelItem.IsEnabled)
                {
                    if (loggingRule == null)
                    {
                        loggingRule = CreateRule(loggerName, logLevelItem.Level);
                    }

                    loggingRule.EnableLoggingForLevel(logLevelItem.Level);
                }
            }

            NLog.LogManager.ReconfigExistingLoggers();
        }

        /// <summary>
        /// Creates the rule in the nlog manager with a given logger name and level
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        private static NLog.Config.LoggingRule CreateRule(string loggerName, NLog.LogLevel level)
        {
            NLog.Config.LoggingRule loggingRule = TryFindRule(loggerName);

            // if it hasn't been created yet..
            if (loggingRule == null)
            {
                loggingRule = new NLog.Config.LoggingRule();
                loggingRule.LoggerNamePattern = loggerName;

                foreach (NLog.Targets.Target target in NLog.LogManager.Configuration.ConfiguredNamedTargets)
                {
                    loggingRule.Targets.Add(target);
                }
                NLog.LogManager.Configuration.LoggingRules.Add(loggingRule);
                NLog.LogManager.ReconfigExistingLoggers();
            }

            return loggingRule;
        }

        private static object lockLoggingRules = new object();

        /// <summary>
        /// Tries the find the logging rule based on the logger name in the nlog manager
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns></returns>
        private static NLog.Config.LoggingRule TryFindRule(string loggerName)
        {
            NLog.Config.LoggingRule loggingRule = null;

            IEnumerable<NLog.Config.LoggingRule> loggingRules;
            //do a local copy over which finding the rule is going to be executed
            lock(lockLoggingRules) 
            {
                loggingRules = new List<NLog.Config.LoggingRule>(NLog.LogManager.Configuration.LoggingRules);
            }

            // Try to find the rule
            foreach (NLog.Config.LoggingRule rule in loggingRules)
            {
                if (rule.LoggerNamePattern == loggerName)
                {
                    loggingRule = rule;
                    break;
                }
            }
            return loggingRule;
        }

        #region Logger Source Info

        /// <summary>
        /// Gets the logger source info.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="loggableMetadata">The loggable metadata.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <returns></returns>
        private static string GetLoggerSourceInfo(string nodeId, ILoggable loggableMetadata, LoggerNameRoot loggerNameRoot)
        {
            return GetLoggerSourceInfo(nodeId,
                                 loggerNameRoot,
                                 loggableMetadata.Classname,
                                 loggableMetadata.SourceAssembly,
                                 loggableMetadata.Label);
        }

        /// <summary>
        /// Gets the name of the logger constructed from several parts.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="loggerNameRoot">The logger name root.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="componentLabel">The component label.</param>
        /// <returns></returns>
        private static string GetLoggerSourceInfo(string nodeId, LoggerNameRoot loggerNameRoot, string componentName, string sourceAssembly, string componentLabel)
        {
            string loggerName;
            if (String.IsNullOrEmpty(loggerNameRoot.UserFriendlyRoot))
            {
                //skip the user friendly root
                loggerName = String.Format("{0}:{1}::Classname:{2}::SourceAssembly:{3}::Label:{4}", loggerNameRoot.ExperimentPathRoot, nodeId, componentName, sourceAssembly, componentLabel);
            }
            else
            {
                //otherwise show user friendly root
                loggerName = String.Format("{0}:{1}::Classname:{2}::SourceAssembly:{3}::Label:{4} : {5}", loggerNameRoot.ExperimentPathRoot, nodeId, componentName, sourceAssembly, loggerNameRoot.UserFriendlyRoot, componentLabel);
            }

            return loggerName;
        }

        /// <summary>
        /// Extracts the log source info. Log info is delimited by double colon '::'
        /// </summary>
        /// <param name="logEventLoggerFullName">Full name of the log event logger.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="componentLabel">The component label.</param>
        public static void ExtractLogSourceInfo(string logEventLoggerFullName, out string componentName, out string sourceAssembly, out string componentLabel)
        {
            string[] stringSeparators = new string[] {"::"};
            string[] parts = logEventLoggerFullName.Split(stringSeparators, StringSplitOptions.None);
            
            //ignore part 0 private logger root (experiment namespace + subgraphs namespaces)

            componentName = ExtractValue(parts[1]);
            sourceAssembly = ExtractValue(parts[2]);
            componentLabel = ExtractValue(parts[3]);
        }

        /// <summary>
        /// Extracts the value from key value pair delimited by single colon ':'
        /// </summary>
        /// <param name="keyValueString">The key value string.</param>
        /// <returns></returns>
        private static string ExtractValue(string keyValueString)
        {
            string value;
            int startIndex = keyValueString.IndexOf(":") + 1;
            value = keyValueString.Substring(startIndex, keyValueString.Length - startIndex);
            return value;
        }

        #endregion
    }
}
