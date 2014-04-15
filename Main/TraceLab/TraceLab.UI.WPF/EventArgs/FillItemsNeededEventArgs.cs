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

using TraceLab.UI.WPF.ViewModels;

namespace TraceLab.UI.WPF.EventArgs
{
    /// <summary>
    /// Event arguments for the event requesting to get a list of items for the specific Statement
    /// </summary>
    public class FillItemsNeededEventArgs : System.EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statement">type of items requested - for example, for select statement items would be a list of successor nodes</param>
        /// <param name="currentlySelectedItem">currently selected item</param>
        public FillItemsNeededEventArgs(Statement statement, object currentlySelectedItem)
        {
            //Select or Load - it could be enum eventuallly, but at the moment it is fine
            Statement = statement;
            CurrentlySelectedStatement = currentlySelectedItem;
        }

        public Statement Statement
        {
            get;
            set;
        }

        public object CurrentlySelectedStatement
        {
            get;
            set;
        }
    }
}
