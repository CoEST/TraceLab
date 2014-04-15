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

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Code parser for decision code
    /// </summary>
    class DecisionCodeParser
    {
        private Tokenizer Tokenizer;

        private StringBuilder CodeBuilder;

        private Dictionary<string, string> SuccessorNodeLabelIdLookup;

        //the key is the workspace unit name (output as), the value is the type of that unit
        private Dictionary<string, string> PredeccessorsOutputsNameTypeLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionCodeParser"/> class.
        /// </summary>
        /// <param name="decisionCode">The decision code.</param>
        /// <param name="successorNodeLabelIdLookup">The successor nodes label id lookup.</param>
        /// <param name="predeccessorsOutputsNameTypeLookup">The predeccessors outputs name type lookup.</param>
        public DecisionCodeParser(string decisionCode, Dictionary<string, string> successorNodeLabelIdLookup, Dictionary<string, string> predeccessorsOutputsNameTypeLookup)
        {
            Tokenizer = new Tokenizer(decisionCode);
            CodeBuilder = new StringBuilder();
            SuccessorNodeLabelIdLookup = successorNodeLabelIdLookup;
            PredeccessorsOutputsNameTypeLookup = predeccessorsOutputsNameTypeLookup;

            blacklist = new string[]
            {
                "System.",
                "TraceLab."
            };
        }

        /// <summary>
        /// Parses the code.
        /// </summary>
        /// <returns></returns>
        public string ParseCode()
        {
            while (Tokenizer.MoveToNextToken())
            {
                ParseStatement();
            }

            return CodeBuilder.ToString();
        }

        /// <summary>
        /// Parses the statement.
        /// </summary>
        public void ParseStatement()
        {
            switch (Tokenizer.Current)
            {
                case "if":
                    ParseIf();
                    break;
                case "Select":
                    ParseSelect();
                    break;
                default:
                    //assignment statement
                    ParseGeneralStatement();
                    break;
            }

        }

        /// <summary>
        /// Parses the block.
        /// </summary>
        public void ParseBlock()
        {
            Advance();
            Assert("{");
            CodeBuilder.Append(Tokenizer.Current);

            Advance();
            while (Tokenizer.Current.Equals("}", StringComparison.CurrentCulture) == false)
            {
                ParseStatement();
                Advance();
            }

            // add } to the code
            CodeBuilder.Append(Tokenizer.Current);

        }

        /// <summary>
        /// Advances to next token
        /// </summary>
        public void Advance()
        {
            if (Tokenizer.MoveToNextToken() == false)
            {
                throw new DecisionCodeParserException("Unexpected code ending");
            }
        }

        /// <summary>
        /// Parses if.
        /// </summary>
        private void ParseIf()
        {
            //start if statement
            CodeBuilder.Append(Tokenizer.Current);

            Advance();
            ParseInsideIfExpression();

            ParseBlock();

            if (Tokenizer.PeekIfNextTokenIsEqualTo("else"))
            {
                Advance();
                CodeBuilder.Append(Tokenizer.Current + " ");

                if (Tokenizer.PeekIfNextTokenIsEqualTo("if"))
                {
                    Advance();
                    ParseIf();
                }
                else
                {
                    //it should be
                    ParseBlock();
                }
            }

        }

        /// <summary>
        /// Parses the inside if expression.
        /// </summary>
        public void ParseInsideIfExpression()
        {

            Stack<string> leftParenthesis = new Stack<string>();

            Assert("(");
            leftParenthesis.Push("(");
            CodeBuilder.Append(Tokenizer.Current);

            //currently we simply parse expression until we reach the ending parenthesis
            while (leftParenthesis.Count > 0)
            {
                Advance();

                if (Tokenizer.Current.Equals("(", StringComparison.CurrentCulture))
                {
                    leftParenthesis.Push("(");
                    CodeBuilder.Append(Tokenizer.Current);
                }
                else if (Tokenizer.Current.Equals(")", StringComparison.CurrentCulture))
                {
                    if (leftParenthesis.Count == 0)
                    {
                        throw new DecisionCodeParserException("Missing parenthesis");
                    }
                    leftParenthesis.Pop();
                    CodeBuilder.Append(Tokenizer.Current);
                }
                else if (Tokenizer.Current.Equals("Load", StringComparison.CurrentCulture))
                {
                    ParseLoadAndAppend();
                }
                else
                {
                    //default - simply append
                    CheckIfCurrentTokenAllowed();
                    CodeBuilder.Append(Tokenizer.Current);
                }
            }
        }

        /// <summary>
        /// Parses the select.
        /// </summary>
        private void ParseSelect()
        {
            Assert("Select");
            CodeBuilder.Append(Tokenizer.Current);

            Advance();
            Assert("(");
            CodeBuilder.Append(Tokenizer.Current);

            //add first quote
            Advance();
            Assert("\"");
            CodeBuilder.Append(Tokenizer.Current);

            //parse inside quotes
            Advance();
            ParseLabel();

            Advance();
            Assert("\"");
            CodeBuilder.Append(Tokenizer.Current);

            Advance();
            Assert(")");
            CodeBuilder.Append(Tokenizer.Current);

            Advance();
            Assert(";");
            CodeBuilder.Append(Tokenizer.Current);
        }

        /// <summary>
        /// Parses the load and append it to the code
        /// </summary>
        private void ParseLoadAndAppend()
        {
            string workspaceUnitType;
            string parsedLoadStatement;

            ParseLoad(out workspaceUnitType, out parsedLoadStatement);

            CodeBuilder.Append("(" + workspaceUnitType + ")"); //casting
            CodeBuilder.Append(parsedLoadStatement);
        }

        /// <summary>
        /// Parses the load.
        /// </summary>
        /// <param name="workspaceUnitType">Type of the workspace unit.</param>
        /// <param name="parsedLoadStatement">The parsed load statement.</param>
        private void ParseLoad(out string workspaceUnitType, out string parsedLoadStatement)
        {
            StringBuilder localLoadStatementBuilder = new StringBuilder();

            Assert("Load");
            localLoadStatementBuilder.Append(Tokenizer.Current);

            Advance();
            Assert("(");
            localLoadStatementBuilder.Append(Tokenizer.Current);

            //add first quote
            Advance();
            Assert("\"");
            localLoadStatementBuilder.Append(Tokenizer.Current);

            //parse inside quotes
            Advance();
            string workspaceUnitName = Tokenizer.Current;
            string type = GetWorkspaceUnitType(workspaceUnitName);
            localLoadStatementBuilder.Append(workspaceUnitName);

            Advance();
            Assert("\"");
            localLoadStatementBuilder.Append(Tokenizer.Current);

            Advance();
            Assert(")");
            localLoadStatementBuilder.Append(Tokenizer.Current);

            //assign the type
            workspaceUnitType = type;

            //assign parsedLoadStatement
            parsedLoadStatement = localLoadStatementBuilder.ToString();
        }

        /// <summary>
        /// Gets the type of the workspace unit.
        /// </summary>
        /// <param name="workspaceUnitName">Name of the workspace unit.</param>
        /// <returns></returns>
        private string GetWorkspaceUnitType(string workspaceUnitName)
        {
            string workspaceUnitType;
            //check if predeccessor nodes output any unit of given name
            if (PredeccessorsOutputsNameTypeLookup.TryGetValue(workspaceUnitName, out workspaceUnitType) == false)
            {
                throw new DecisionCodeParserException(String.Format("None of the predecessor nodes output unit of the given name '{0}'.", workspaceUnitName));
            }
            return workspaceUnitType;
        }

        /// <summary>
        /// Parses the label.
        /// </summary>
        private void ParseLabel()
        {
            string successorNodeLabel = Tokenizer.Current;
            string successorNodeId;
            if (SuccessorNodeLabelIdLookup.TryGetValue(successorNodeLabel, out successorNodeId) == false)
            {
                throw new DecisionCodeParserException("The successor node of the given label has not been found.");
            }
            CodeBuilder.Append(successorNodeId);
        }

        /// <summary>
        /// first checks first three tokens in statement.
        /// If the Load pattern, ie. ' variable=Load ', is recognized, then proceed to ParseLoadAssignmentStatement
        /// Otherwise, check if tokens are allowed until reaching ";"
        /// </summary>
        private void ParseGeneralStatement()
        {
            //save variable and advance
            string firstToken = Tokenizer.Current;
            CheckIfCurrentTokenAllowed();
            if (Tokenizer.Current.Equals(";")) return;

            Advance();

            //check if next is sign equal and advance
            string secondToken = Tokenizer.Current;
            CheckIfCurrentTokenAllowed();
            if (Tokenizer.Current.Equals(";")) return;

            Advance();

            string thirdToken = Tokenizer.Current;
            CheckIfCurrentTokenAllowed();
            if (Tokenizer.Current.Equals(";")) return;

            if (secondToken.Equals("=") && thirdToken.Equals("Load"))
            {
                ParseLoadAssignmentStatement(firstToken);
            }
            else
            {
                CodeBuilder.Append(firstToken);
                CodeBuilder.Append(secondToken);
                CodeBuilder.Append(thirdToken);

                //parse the rest of statement until reach ";"
                while (Tokenizer.Current.Equals(";") == false)
                {
                    Advance();
                    CheckIfCurrentTokenAllowed();
                    if (Tokenizer.Current.Equals("Load", StringComparison.CurrentCulture))
                    {
                        ParseLoadAndAppend();
                    }
                    else
                    {
                        CodeBuilder.Append(Tokenizer.Current);
                    }
                }
            }
        }

        private string[] blacklist;

        /// <summary>
        /// Checks if current token is allowed.
        /// </summary>
        private void CheckIfCurrentTokenAllowed()
        {
            if (blacklist.Any(item => Tokenizer.Current.StartsWith(item)))
            {
                throw new DecisionCodeParserException("Not allowed statement.");
            }
        }

        /// <summary>
        /// special parsing, because casting and variable type has to be added
        /// </summary>
        /// <param name="variable"></param>
        private void ParseLoadAssignmentStatement(string variable)
        {
            //parse Load statement
            string type;
            string parsedLoadStatement;

            ParseLoad(out type, out parsedLoadStatement);

            //build the code
            CodeBuilder.Append(type); //add type
            CodeBuilder.Append(" ");
            CodeBuilder.Append(variable);
            CodeBuilder.Append("=");
            CodeBuilder.Append("(" + type + ")"); //add casting
            CodeBuilder.Append(parsedLoadStatement);

            Advance();
            Assert(";");
            CodeBuilder.Append(Tokenizer.Current);
        }


        /// <summary>
        /// Asserts if the token is a expected token.
        /// </summary>
        /// <param name="expectedToken">The expected token.</param>
        private void Assert(string expectedToken)
        {

            if (Tokenizer.Current.Equals(expectedToken, StringComparison.CurrentCulture) == false)
            {
                string errorMsg = String.Format(System.Globalization.CultureInfo.CurrentCulture, "Syntax Error - {0} is missing.", expectedToken);
                throw new DecisionCodeParserException(errorMsg);
            }
        }


    }
}
