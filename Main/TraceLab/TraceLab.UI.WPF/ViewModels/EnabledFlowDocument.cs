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

using System.Windows.Documents;

namespace TraceLab.UI.WPF.ViewModels
{
    /// <summary>
    /// EnabledDocument overrides FlowDocuments in which IsEnabledCore is set to true 
    /// that makes all UIElements to be active, and clickable.
    /// </summary>
    class EnabledFlowDocument : FlowDocument
    {
        public EnabledFlowDocument()
            : base()
        {
            FontFamily = new System.Windows.Media.FontFamily("Verdana");
            FontSize = 12.0;
            System.Diagnostics.Debug.WriteLine("Constructing document");
        }

        protected override bool IsEnabledCore
        {
            get
            {
                return true;
            }
        }
    }

}
