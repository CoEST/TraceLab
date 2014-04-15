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

using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;

namespace InstallerActions
{
    public class ShortcutRemoval
    {
        /// <summary>
        /// Removes TraceLab shortcuts from Desktop and Start Menu.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        [CustomAction]
        public static ActionResult RemoveShortcuts(Session session)
        {
            try
            {
                session.Log("Removing TraceLab desktop shortcuts.");

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (String.IsNullOrEmpty(desktopPath) == false && Directory.Exists(desktopPath))
                {
                    string[] desktopShortcuts = Directory.GetFiles(desktopPath, "TraceLab*.lnk");
                    foreach (string shortcut in desktopShortcuts)
                    {
                        File.Delete(shortcut);
                    }
                }

                session.Log("Removing TraceLab start menu shortcuts.");

                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                if (String.IsNullOrEmpty(startMenuPath) == false && Directory.Exists(startMenuPath))
                {
                    string[] startMenuFolders = Directory.GetDirectories(startMenuPath, "TraceLab", SearchOption.AllDirectories);
                    foreach (string folder in startMenuFolders)
                    {
                        Directory.Delete(folder, true);
                    }
                }
            }
            catch (Exception)
            {
            }

            return ActionResult.Success;
        }

    }
}
