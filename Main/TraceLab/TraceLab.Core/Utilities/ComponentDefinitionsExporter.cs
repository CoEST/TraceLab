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
using TraceLab.Core.Components;

namespace TraceLab.Core.Utilities
{
    /// <summary>
    /// Exports components definition in the given components library in friendly format to publish it to website.
    /// </summary>
    internal class ComponentDefinitionsExporter
    {
        public static void Export(IEnumerable<MetadataDefinition> componentsDefinitions, string filepath)
        {
            StringBuilder builder = new StringBuilder();

            foreach (MetadataDefinition definition in componentsDefinitions)
            {
                builder.AppendLine(String.Format("h3. {0}", definition.Label));
                builder.AppendLine();
                builder.AppendLine(String.Format("* *Author:* {0}", definition.Author));
                builder.AppendLine(String.Format("* *Version:* {0}", definition.Version));
                builder.AppendLine(String.Format("* *Description:* {0}", definition.Description));
                
                builder.Append("* *Tags:* ");
                foreach (string tag in definition.Tags.Values)
                {
                    builder.Append(tag + ", ");
                }
                builder.Remove(builder.Length - 2, 2);
                builder.AppendLine();
                builder.AppendLine();
            }

            System.IO.File.WriteAllText(filepath, builder.ToString());
        }

    }
}
