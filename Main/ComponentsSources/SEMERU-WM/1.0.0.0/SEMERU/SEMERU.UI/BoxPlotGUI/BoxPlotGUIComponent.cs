using System.Collections.Generic;
using System.Windows;
using SEMERU.Types.Metrics;
using TraceLabSDK;

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

namespace SEMERU.UI.BoxPlotGUI
{
    [Component(Name = "SEMERU - (UI) Results BoxPlots",
        Description = "Creates a GUI from the results of an expemeriment.",
        Author = "ReLab; SEMERU; Evan Moritz",
        Version = "1.0.0.0")]
    [IOSpec(IOSpecType.Input, "ListOfDatasetResults", typeof(List<DatasetResults>))]
    [Tag("SEMERU.UI")]
    [Tag("Metrics.Visualization")]
    public class BoxPlotGUIComponent : BaseComponent
    {
        public BoxPlotGUIComponent(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            List<DatasetResults> data = (List<DatasetResults>) Workspace.Load("ListOfDatasetResults");
            Window gui = new EvaluationResultsView(data);
            gui.ShowDialog();
        }

        public override void PostCompute()
        {
            // cleans up window after closing
            System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
        }
    }
}