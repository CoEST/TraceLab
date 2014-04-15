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
using System.Diagnostics;
using System.IO;

namespace TraceLab.UI.WPF.ViewModels
{
    public class ComponentExceptionDisplayViewModel
    {
        public ComponentExceptionDisplayViewModel(TraceLab.Core.ViewModels.ComponentLogInfo logInfo)
        {
            LogInfo = logInfo;

            if (LogInfo.Exception != null)
            {
                ExceptionType = LogInfo.Exception.GetType().FullName;

                StringBuilder shortStack = new StringBuilder();
                using (StringReader reader = new StringReader(LogInfo.Exception.StackTrace)) 
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Server stack trace: "))
                        {
                            shortStack.AppendLine("Exception trace:");
                            continue;
                        }
                        if(line.Trim().StartsWith("at System.Runtime.Remoting.Messaging")) 
                        {
                            break;
                        }
                        shortStack.AppendLine(line);
                    }
                }

                ShortStackTrace = shortStack.ToString();
            }
        }

        public TraceLab.Core.ViewModels.ComponentLogInfo LogInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the short stack trace of the component exception.
        /// Short stact trace does not include the trace from TraceLab part. 
        /// User does not need to know how its component is being called from TraceLab.
        /// </summary>
        public string ShortStackTrace
        {
            get;
            private set;
        }

        public string ExceptionType
        {
            get;
            private set;
        }
    }
}
