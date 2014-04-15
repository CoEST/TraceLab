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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using TraceLab.UI.WPF.Controls;
using TraceLab.UI.WPF.ViewModels;


namespace TraceLab.UI.WPF.Converters
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class StringToFlowDocumentConverter : MarkupExtension, IValueConverter
    {
        public StringToFlowDocumentConverter()
        {
        }

        private static readonly string regexStatementsPattern = @"(Load\("".*?""\)|Select\("".*?""\))";

        /// <summary>
        /// Converts from decision code to a WPF FlowDocument.
        /// </summary>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Diagnostics.Trace.WriteLine("Converts from string decision code to FlowDocument");
            var flowDocument = new EnabledFlowDocument();
            
            string text = value as string;
            if (text != null)
            {
                using (StringReader reader = new StringReader(text))
                {
                    string newLine;
                    while ((newLine = reader.ReadLine()) != null)
                    {
                        Paragraph paragraph = ConvertStringLineToParagraph(newLine);
                        flowDocument.Blocks.Add(paragraph);
                    }
                }
            }
            return flowDocument;
        }

        /// <summary>
        /// Converts single line to Paragraph
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public Paragraph ConvertStringLineToParagraph(string line)
        {
            Paragraph paragraph = new Paragraph();
            AppendTextToParagraph(paragraph, line);
            return paragraph;
        }

        /// <summary>
        /// Appends given line to the given paragraph
        /// </summary>
        /// <param name="currentParagraph"></param>
        /// <param name="line"></param>
        public void AppendTextToParagraph(Paragraph currentParagraph, string line)
        {
            foreach (string token in Regex.Split(line, regexStatementsPattern))
            {
                if (token.StartsWith("Load") || token.StartsWith("Select"))
                {
                    //match the selected item
                    Match m = Regex.Match(token, @"""(.*)""");
                    string selectedItem = m.Groups[1].Value;

                    ComboBoxStatement statementBox;
                    if (token.StartsWith("Load"))
                    {
                        statementBox = new ComboBoxStatement(Statement.Load, selectedItem);
                    }
                    else
                    {
                        statementBox = new ComboBoxStatement(Statement.Select, selectedItem);
                    }

                    InlineUIContainer myInlineUIContainer = new InlineUIContainer(statementBox);
                    currentParagraph.Inlines.Add(myInlineUIContainer);
                }
                else
                {
                    currentParagraph.Inlines.Add(new Run(token));
                }
            }
        }

        /// <summary>
        /// Converts from a WPF FlowDocument to a decision code string.
        /// All comboboxes are translated to "selectedItem".
        /// </summary>
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Diagnostics.Trace.WriteLine("Converts from FlowDocument to string decision code.");

            if (value == null) return string.Empty;

            // Get flow document from value passed in
            EnabledFlowDocument flowDocument = value as EnabledFlowDocument;

            string text = ConvertFlowDocumentDataToStringWithinSelection(flowDocument.ContentStart, flowDocument.ContentEnd);
            return text;
        }

        /// <summary>
        /// Converts flowDocument data to string between start and end textpointer
        /// </summary>
        /// <param name="start">start textpointer</param>
        /// <param name="end">end textpointer</param>
        /// <returns></returns>
        public string ConvertFlowDocumentDataToStringWithinSelection(TextPointer start, TextPointer end)
        {
            // NOT WORKING - it should check if the start and end document are within the same document... 
            //if (start.IsInSameDocument(end))
            //{
            //    throw new ArgumentException("Start and end pointers has to be within the same document.");
            //}

            StringBuilder codeBuilder = new StringBuilder();

            for (TextPointer position = start;
                   position != null && position.CompareTo(end) <= 0;
                   position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
                {
                    // process only runs, paragraphs, and UiElements. all other elements are ignored
                    Paragraph paragraph;
                    Run run;
                    InlineUIContainer uiContainer;

                    if ((paragraph = position.Parent as Paragraph) != null)
                    {
                        codeBuilder.AppendLine();
                    }
                    else if ((run = position.Parent as Run) != null)
                    {
                        codeBuilder.Append(run.Text);
                    }
                    else if ((uiContainer = position.Parent as InlineUIContainer) != null)
                    {
                        ComboBoxStatement statementBox = uiContainer.Child as ComboBoxStatement;
                        codeBuilder.Append(statementBox.FormattedVisualStatement);
                    }

                }

            }

            return codeBuilder.ToString();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
