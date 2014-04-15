using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows;
using TraceLabSDK;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.UI.Components
{
    [Component(Name = "SEMERU - (UI) Simple Results Logger",
                Description = "Summarizes the output of a set of results metrics to the Log.",
                Author = "SEMERU; Evan Moritz",
                Version = "1.0.0.0",
                ConfigurationType=typeof(SimpleResultsGUIConfig))]
    [IOSpec(IOSpecType.Input, "PrecisionData", typeof(TLKeyValuePairsList))]
    [IOSpec(IOSpecType.Input, "RecallData", typeof(TLKeyValuePairsList))]
    [IOSpec(IOSpecType.Input, "AvgPrecisionData", typeof(TLKeyValuePairsList))]
    [Tag("SEMERU.UI")]
    [Tag("Metrics.Visualization")]
    public class SimpleResultsGUI : BaseComponent
    {
        private SimpleResultsGUIConfig _config;

        public SimpleResultsGUI(ComponentLogger log)
            : base(log)
        {
            _config = new SimpleResultsGUIConfig();
            Configuration = _config;
        }

        public override void Compute()
        {
            TLKeyValuePairsList precision = (TLKeyValuePairsList)Workspace.Load("PrecisionData");
            TLKeyValuePairsList recall = (TLKeyValuePairsList)Workspace.Load("RecallData");
            TLKeyValuePairsList avgprecision = (TLKeyValuePairsList)Workspace.Load("AvgPrecisionData");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\n", _config.Title);
            sb.AppendLine("--------------------------------------------------");
            sb.AppendFormat("Precision Data:\nMin:\t{0}\nMax:\t{1}\nAvg:\t{2}\n",
                GetMin(precision),
                GetMax(precision),
                GetAvg(precision)
            );
            sb.AppendLine("--------------------------------------------------");
            sb.AppendFormat("Recall Data:\nMin:\t{0}\nMax:\t{1}\nAvg:\t{2}\n",
                GetMin(recall),
                GetMax(recall),
                GetAvg(recall)
            );
            sb.AppendLine("--------------------------------------------------");
            sb.AppendFormat("Avg Precision Data:\nMin:\t{0}\nMax:\t{1}\nAvg:\t{2}\n",
                GetMin(avgprecision),
                GetMax(avgprecision),
                GetAvg(avgprecision)
            );
            sb.AppendLine("--------------------------------------------------");
            /*
            SimpleResultsWindow window = new SimpleResultsWindow();
            window.PrintToScreen(sb.ToString());
            window.ShowDialog();
            */
            Logger.Info(sb.ToString());
        }

        private double GetAvg(TLKeyValuePairsList list)
        {
            double avg = 0.0;
            foreach (KeyValuePair<string, double> val in list)
            {
                if (!Double.IsNaN(val.Value))
                    avg += val.Value;
            }
            return avg / list.Count;
        }

        private double GetMax(TLKeyValuePairsList list)
        {
            double max = Double.MinValue;
            foreach (KeyValuePair<string, double> val in list)
            {
                if (!Double.IsNaN(val.Value) && val.Value > max)
                    max = val.Value;
            }
            return max;
        }

        private double GetMin(TLKeyValuePairsList list)
        {
            double min = Double.MaxValue;
            foreach (KeyValuePair<string, double> val in list)
            {
                if (!Double.IsNaN(val.Value) && val.Value < min)
                    min = val.Value;
            }
            return min;
        }

        public override void PostCompute()
        {
            // cleans up window after closing
            //System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
        }
    }

    public class SimpleResultsGUIConfig
    {
        public string Title { get; set; }
    }
}