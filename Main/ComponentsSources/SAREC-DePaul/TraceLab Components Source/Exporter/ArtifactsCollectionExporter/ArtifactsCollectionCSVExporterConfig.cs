using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TraceLabSDK.Component.Config;

namespace Exporter.ArtifactsCollectionExporter
{
    public class ArtifactsCollectionCSVExporterConfig
    {
        [DisplayName("CSV Output filepath")]
        public FilePath Path { get; set; }
    }
}
