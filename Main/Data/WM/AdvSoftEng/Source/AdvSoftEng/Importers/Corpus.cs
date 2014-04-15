using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using System.IO;

namespace AdvSoftEng.Importers
{
    public static class Corpus
    {
        public static TLArtifactsCollection Import(String idPath, String docPath)
        {
            TLArtifactsCollection artifacts = new TLArtifactsCollection();

            StreamReader idFile = new StreamReader(idPath);
            StreamReader docFile = new StreamReader(docPath);

            String origid;
            String doc;

            while ((origid = idFile.ReadLine()) != null)
            {
                // read doc
                doc = docFile.ReadLine().Trim();

                // set vars
                String id = origid.Trim();
                int num = 0;

                while (artifacts.ContainsKey(id))
                {
                    num++;
                    id = origid.Trim() + Properties.Settings.Default.MethodOverloadID + num.ToString();
                }

                artifacts.Add(new TLArtifact(id, doc));
            }

            return artifacts;
        }
    }
}
