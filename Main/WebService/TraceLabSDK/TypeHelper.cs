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

namespace TraceLabSDK
{
    /// <summary>
    /// A helper class for getting a TraceLab-friendly serializable type name.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Gets the name of the trace lab qualified.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetTraceLabQualifiedName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string result = type.AssemblyQualifiedName;

            // If type comes from a tracelab assembly, we need to strip out the version number from the AssemblyQualifiedName
            if (type.Assembly.GetName().Name.StartsWithAny("TraceLab.Core", "TraceLabSDK.Types", "TraceLabSDK"))
            {
                var version = result.IndexOf(", Version=");
                var endVersion = result.IndexOf(", ", version + 1);

                if (version != -1 && endVersion != -1)
                {
                    result = result.Remove(version, endVersion - version);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the old assembly-qualified typename into the new version that TraceLab uses.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ConvertOldTypeName(string type)
        {
            var entries = type.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            // this should split into either:
            // 1)
            //    Class name
            //    Assembly name
            //    Assembly Version
            //    Assembly Culture
            //    Assembly public key token
            // OR
            // 2)
            //    Assembly name
            //    Assembly Version
            //    Assembly Culture
            //    Assembly public key token

            bool isTraceLabType = false;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < entries.Length; ++i)
            {
                bool shouldAppend = true;
                var entry = entries[i];

                // If the assembly is a TraceLab assembly
                if (i == 1 && entry.StartsWith("TraceLab"))
                {
                    isTraceLabType = true;
                }
                if (isTraceLabType && entry.StartsWith("Version="))
                {
                    shouldAppend = false;
                }
                if (isTraceLabType && entry.StartsWith("PublicKeyToken="))
                {
                    entry = "PublicKeyToken=2c83cea59a8bb151";
                }

                if (shouldAppend)
                {
                    if (i != 0)
                    {
                        builder.Append(", ");
                    }

                    builder.Append(entry);
                }
            }

            return builder.ToString();
        }

        private static bool StartsWithAny(this string input, params string[] beginnings)
        {
            foreach (var x in beginnings)
                if (input.StartsWith(x.ToString()))
                    return true;

            return false;
        }
    }
}
