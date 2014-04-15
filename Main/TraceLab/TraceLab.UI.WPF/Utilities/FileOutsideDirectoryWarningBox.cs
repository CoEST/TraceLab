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
using System.Windows;

namespace TraceLab.UI.WPF.Utilities
{
    class FileOutsideDirectoryWarningBox
    {
        /// <summary>
        /// Shows the warning box with provided message, if file is outside the given list of directories.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="directories">The directories.</param>
        /// <returns></returns>
        public static bool ShowWarningBox(string fileName, IEnumerable<string> directories, string messageBoxText)
        {
            bool tryagain = false;

            string dir = System.IO.Path.GetDirectoryName(fileName);
            if (directories.Contains(dir, TraceLab.Core.Utilities.FilePathComparerFactory.GetFileComparer())
                     == false)
            {
                //show dialog
                string caption = TraceLab.Core.Messages.DoYouWantToContinue;
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                // Display message box
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        tryagain = false;
                        break;
                    case MessageBoxResult.No:
                        tryagain = true;
                        break;
                }
            }
            return tryagain;
        }
    }
}
