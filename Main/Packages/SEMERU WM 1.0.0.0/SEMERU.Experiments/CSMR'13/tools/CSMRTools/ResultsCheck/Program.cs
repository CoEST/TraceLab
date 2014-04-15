using System;
using System.Collections.Generic;
using System.IO;

namespace ResultsCheck
{
    public struct Info
    {
        public DirectoryInfo BaseDirectory;
        public DirectoryInfo ResultsDirectory;
        public DirectoryInfo DataDirectory;
        public DirectoryInfo OutputDirectory;
        public List<string> IRModels;
        public List<string> StructuralModels;
    }

    class Program
    {
        /// <summary>
        /// MAIN
        /// </summary>
        /// <param name="args">Console arguments</param>
        static void Main(string[] args)
        {
            // basic info
            Info info = new Info();
            info.BaseDirectory = new DirectoryInfo(@"C:\Users\Evan\Documents\SEMERU\TraceLab\trunk\SEMERU.Experiments\CSMR'13");
            info.ResultsDirectory = new DirectoryInfo(info.BaseDirectory.FullName + @"\results");
            info.OutputDirectory = new DirectoryInfo(info.ResultsDirectory.FullName + @"\CSMRTools");
            info.DataDirectory = new DirectoryInfo(info.BaseDirectory.FullName + @"..\..\SEMERU.Datasets");
            info.IRModels = new List<string>(new string[] { "VSM", "JS" });
            info.StructuralModels = new List<string>(new string[] { "OCSTI", "UDCSTI" });
            Directory.CreateDirectory(info.OutputDirectory.FullName);

#if false
            // import dataset info
            List<CSMR13DataSet> Datasets = ExperimentalSetup.Import(info.BaseDirectory + @"\setup.xml");

            // run main results
            foreach (CSMR13DataSet dataset in Datasets)
            {
                Console.WriteLine("Running {0}...", dataset.Name);
                CheckLinkOrder.Run(ref info, dataset);
            }

            // Rocco results
            ConvertRoccoResults.Run(ref info);
#endif
            RunModelsWithRoccoResults.Run(ref info);

            Console.WriteLine("Press [Enter] to quit.");
            Console.ReadLine();
        }
    }
}
