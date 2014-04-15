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
using System.Text.RegularExpressions;

namespace Preprocessor
{
    class PreprocessorCleanUp
    {
        public static string Process(string text, PreprocessorCleanUpComponentEnum removeDigits)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();

            // Reduces the text to only characters - using Regular Expressions - TODO: test if this is correct
            string cleanText = Regex.Replace(text, "[^A-Za-z0-9 ]", " ");
            if (removeDigits == PreprocessorCleanUpComponentEnum.Yes)
            {
                cleanText = Regex.Replace(cleanText, @"\b[0-9]+\b", "");
            }

            //remove duplicate white spaces... 
            //this method is apparently faster than Regex.Replace(input, "[\s]+", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //for significantly larger files
            string[] parts = cleanText.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                builder.AppendFormat("{0} ", part);
            }
            result = builder.ToString();

            //convert to lower case
            result = result.ToLower().Trim();

            return result;

        }
    }
}
