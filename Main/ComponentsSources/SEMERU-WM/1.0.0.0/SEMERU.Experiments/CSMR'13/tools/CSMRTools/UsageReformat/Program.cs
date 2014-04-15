using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UsageReformat
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"C:\Users\Evan\Downloads\Dataset\Dataset\EasyClinic_tc_cc\oracle.csv";
            string outfile = @"C:\Users\Evan\Downloads\Dataset\Dataset\EasyClinic_tc_cc\oracle.txt";

            TextReader reader = new StreamReader(filename);
            int numSources = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.None).Count();
            reader.Close();
            Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filename);
            parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            parser.SetDelimiters(",");
            List<List<string>> data = new List<List<string>>();
            for (int i = 0; i < numSources; i++)
                data.Add(new List<string>());
            int curID = 0;
            while (!parser.EndOfData)
            {
                string[] currentRow = parser.ReadFields();
                foreach (string currentField in currentRow)
                {
                    data[curID].Add(currentField);
                    curID++;
                }
                curID = 0;
            }

            TextWriter of = new StreamWriter(outfile);
            foreach (List<string> row in data)
            {
                of.WriteLine(String.Join(" ", row));
            }
            of.Flush();
            of.Close();
        }
    }
}
