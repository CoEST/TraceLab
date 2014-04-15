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

using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.ViewModels.Nodes
{
    /// <summary>
    /// Represents the view model for info panel of decision nodes
    /// It is seperate type so that specific DataTemplate could be render for the info
    /// </summary>
    public class DecisionNodeInfo : ExperimentNodeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionNodeInfo"/> class.
        /// </summary>
        /// <param name="originVertexControl">The origin vertex control.</param>
        internal DecisionNodeInfo(GraphSharp.Controls.VertexControl originVertexControl)
            : base(originVertexControl)
        {
        }
    }
}
