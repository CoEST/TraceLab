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
using System.Text.RegularExpressions;
using System.Linq;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Responsible for tokenizer the user code
    /// </summary>
    class Tokenizer
    {
        private List<string> m_tokenizedCode;
        private Queue<string> tokensStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tokenizer"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public Tokenizer(string code)
        {
            m_tokenizedCode = Tokenize(code);

            tokensStack = new Queue<string>(m_tokenizedCode);

        }

        /// <summary>
        /// Moves to next token.
        /// </summary>
        /// <returns></returns>
        public bool MoveToNextToken()
        {
            if (tokensStack.Count > 0)
            {
                Current = tokensStack.Dequeue();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Peeks if next token is equal to.
        /// </summary>
        /// <param name="expectedToken">The expected token.</param>
        /// <returns></returns>
        public bool PeekIfNextTokenIsEqualTo(string expectedToken)
        {
            bool retVal;
            if (tokensStack.Count > 0)
            {
                retVal = (tokensStack.Peek().Equals(expectedToken, StringComparison.CurrentCulture));
            }
            else
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// Gets or sets the current token
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public string Current
        {
            get;
            set;
        }

        /// <summary>
        /// Resets the token stack
        /// </summary>
        public void Reset()
        {
            tokensStack = new Queue<string>(m_tokenizedCode);
        }

        /// <summary>
        /// Tokenizes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static List<string> Tokenize(string code)
        {
            //replace all space characters and new lines with simply a space
            code = Regex.Replace(code, @"\s+", " ");

            var tokens = new HashSet<string> { "Load", "Select", "(", ")", "{", "}", "-", "*", @"\", "<", ">", "<=", ">=", "==", "=", "if[^a-zA-Z0-9]", "+", "-", "else", ";", "\"" };
            string pattern = "(" + String.Join("|", tokens.Select(d => Regex.Escape(d))
                                                              .ToArray())
                              + ")";

            string[] tmpResult = Regex.Split(code, pattern, RegexOptions.IgnoreCase);

            //iterate and remove empty strings and spaces
            List<string> result = new List<string>();
            foreach (string tmpToken in tmpResult)
            {
                if (string.IsNullOrWhiteSpace(tmpToken) == false)
                {
                    result.Add(tmpToken.Trim());
                }
            }

            return result;
        }
    }
}
