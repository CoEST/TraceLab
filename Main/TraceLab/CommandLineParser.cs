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

namespace TraceLab
{
    public class CommandLineProcessor
    {
        static CommandLineProcessor()
        {
            if (!TraceLabSDK.RuntimeInfo.IsRunInMono)
            {
                SwitchCharacter = "/";
            }
            else
            {
                SwitchCharacter = "-";
            }
        }

        public static string SwitchCharacter
        {
            get;
            set;
        }

        private Dictionary<string, Action<string>> m_commands = new Dictionary<string, Action<string>>();
        public IDictionary<string, Action<string>> Commands
        {
            get
            {
                return m_commands;
            }
        }

        public void Parse(string[] args)
        {
            if (string.IsNullOrEmpty(SwitchCharacter))
                throw new InvalidOperationException("Switch character must be set.");

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    string command = arg;
                    string commandValue = null;
                    if (command.StartsWith(SwitchCharacter))
                    {
                        int commandLength = SwitchCharacter.Length;
                        int valuePosition = command.IndexOf(':');
                        if (valuePosition != -1)
                        {
                            // Command starts AFTER the slash, and ends with the first colon.
                            command = command.Substring(1, valuePosition - 1);

                            // Length of the command increases by 
                            commandLength += command.Length + 1;

                            // Value starts AFTER the length of the command (plus 2: 1 for the slash, 1 for the colon)
                            commandValue = arg.Substring(commandLength);
                        }
                        else
                        {
                            command = command.Substring(1);
                        }
                    }

                    Action<string> func = null;
                    if (Commands.TryGetValue(command, out func) && func != null)
                    {
                        func(commandValue);
                    }
                }
            }
        }
    }
}
