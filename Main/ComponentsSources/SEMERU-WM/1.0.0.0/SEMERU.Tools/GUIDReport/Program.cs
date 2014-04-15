using System;
using System.Collections.Generic;
using System.IO;
using TraceLab.Core.Components;
using TraceLab.Core.PackageSystem;
using TraceLab.Core.Settings;

namespace GUIDReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string outfile = Properties.Settings.Default.Outfile;
            List<SettingsPath> ComponentDirectories = CreateSettingsPath(Properties.Settings.Default.ComponentDirectoriesFile);
            string decisionDirectory = Properties.Settings.Default.DecisionDirectory;
            string dataRoot = Properties.Settings.Default.DataRoot;
            List<string> typeDir = CreateTypeDirectories(Properties.Settings.Default.TypeDirectoriesFile);

            Console.WriteLine("Scanning component directories...");
            
            ComponentsLibrary.Init(ComponentDirectories);
            ComponentsLibrary.Instance.DecisionsDirectoryPath = decisionDirectory;
            ComponentsLibrary.Instance.DataRoot = dataRoot;
            ComponentsLibrary.Instance.Rescan(PackageManager.Instance, typeDir, true);
            while (ComponentsLibrary.Instance.IsRescanning) { } // wait

            if (ComponentsLibrary.Instance.DefinitionErrors != null)
            {
                int errors = 0;
                foreach (string error in ComponentsLibrary.Instance.DefinitionErrors)
                {
                    errors++;
                    Console.WriteLine(error);
                }
                Console.WriteLine();
                Console.WriteLine("{0} errors found.", errors);
            }

            int count = 0;

            if (ComponentsLibrary.Instance.Components != null)
            {
                string lineFormat = String.Format("{{0,-{0}}}\t{{1}}\t{{2}}\t{{3}}", Properties.Settings.Default.GUIDLength);
                TextWriter file = new StreamWriter(outfile, false);
                foreach (MetadataDefinition metadata in ComponentsLibrary.Instance.Components)
                {
                    count++;
                    file.WriteLine(lineFormat,
                        metadata.ID,
                        PadName(metadata.Label, Properties.Settings.Default.NameLength),
                        PadName(metadata.Version, Properties.Settings.Default.VersionLength),
                        metadata.Assembly);
                }
                file.Flush();
                file.Close();
            }

            Console.WriteLine("{0} components found.", count);
            Console.WriteLine("Output written to {0}", Properties.Settings.Default.Outfile);
        }

        private static List<string> CreateTypeDirectories(string path)
        {
            return new List<string>(File.ReadLines(path));
        }

        private static List<SettingsPath> CreateSettingsPath(string path)
        {
            List<string> dirs = new List<string>(File.ReadLines(path));
            List<SettingsPath> sp = new List<SettingsPath>();
            foreach (string dir in dirs)
            {
                sp.Add(new SettingsPath(false, dir));
            }
            return sp;
        }

        private static string PadName(string name, int size)
        {
            if (name == null)
            {
                name = "(none)";
            }
            if (name.Length <= size)
            {
                return String.Format(String.Format("{{0,-{0}}}", size), name);
            }
            else
            {
                return name.Substring(0, size - 3) + "...";
            }
        }
    }
}
