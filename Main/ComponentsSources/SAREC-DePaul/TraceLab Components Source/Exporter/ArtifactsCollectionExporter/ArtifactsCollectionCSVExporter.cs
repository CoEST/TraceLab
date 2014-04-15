using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;

namespace Exporter.ArtifactsCollectionExporter
{
    [Component(Name = "Artifacts Collection Exporter to CSV",
            DefaultLabel = "Artifacts Collection CSV Exporter",
            Description = "The components exports the artifacts collection to the csv file",
            Author = "DePaul RE Team",
            Version = "1.0",
            ConfigurationType = typeof(ArtifactsCollectionCSVExporterConfig))]
    [IOSpec(IOSpecType.Input, "artifactsCollection", typeof(TLArtifactsCollection))]
    [Tag("Exporters.TLArtifactsCollection.To CSV")]
    public class ArtifactsCollectionCSVExporter : BaseComponent
    {
        private ArtifactsCollectionCSVExporterConfig Config;

        public ArtifactsCollectionCSVExporter(ComponentLogger log)
            : base(log)
        {
            Config = new ArtifactsCollectionCSVExporterConfig();
            Configuration = Config;
        }

        public override void Compute()
        {
            TLArtifactsCollection artifactsCollection = (TLArtifactsCollection)Workspace.Load("artifactsCollection");

            CreateCSVReport(artifactsCollection, Config.Path.Absolute);

            Logger.Info(String.Format("Artifacts Collection has been saved into csv file '{0}'", Config.Path.Absolute));
        }

        private static void CreateCSVReport(TLArtifactsCollection artifacts, string outputPath)
        {
            if (artifacts == null)
            {
                throw new ComponentException("Received artifacts collection is null!");
            }

            if (outputPath == null)
            {
                throw new ComponentException("Output path cannot be null.");
            }

            if (!System.IO.Path.IsPathRooted(outputPath))
            {
                throw new ComponentException(String.Format("Absolute output path is required. Given path is '{0}'", outputPath));
            }

            if (outputPath.EndsWith(".csv", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                outputPath = outputPath + ".csv";
            }

            using (System.IO.TextWriter writeFile = new StreamWriter(outputPath))
            {
                WriteArtifactsToFile(artifacts, writeFile);
                writeFile.Flush();
                writeFile.Close();
            }
        }

        private static void WriteArtifactsToFile(TLArtifactsCollection artifacts, System.IO.TextWriter writeFile)
        {
            //header
            writeFile.WriteLine("Id,Text");

            foreach (TLArtifact artifact in artifacts.Values)
            {
                writeFile.WriteLine("\"{0}\",\"{1}\"", artifact.Id, artifact.Text);
            }

        }
    }
}
