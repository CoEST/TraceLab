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
using Microsoft.Deployment.WindowsInstaller;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Xml.XPath;

namespace InstallerActions
{
    public class LayoutUpgrades
    {
        [CustomAction]
        public static ActionResult UpgradeLayoutFile(Session session)
        {
            string appData = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TraceLab");
            var path = System.IO.Path.Combine(appData, "layout.xml");

            bool resetLayout = false;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    session.Log("Upgrading layout file.");

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (var fileStream = File.OpenRead(path))
                        {
                            memStream.SetLength(fileStream.Length);
                            fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                            memStream.Flush();
                            fileStream.Close();
                        }

                        UpgradeLayoutToCurrentVersion(memStream);

                        // The memory stream has been transformed in place, it is now time to copy it back to the file.
                        using (var fileStream = File.OpenWrite(path))
                        {
                            fileStream.Write(memStream.GetBuffer(), 0, (int)memStream.Length);
                            fileStream.SetLength(memStream.Length);
                            fileStream.Close();
                        }
                    }

                    session.Log("Done upgrading layout file.");
                }
            }
            catch (Exception e)
            {
                session.Log("Exception occurred: {0}\n{1}", e.Message, e.StackTrace);
                resetLayout = true;
            }

            if (resetLayout)
            {
                try
                {
                    session.Log("Resetting layout");
                    System.IO.File.Delete(path);
                }
                catch (Exception)
                {
                    return ActionResult.Failure;
                }
            }

            return ActionResult.Success;
        }

        private static void UpgradeLayoutToCurrentVersion(Stream memStream)
        {
            UpgradeLayoutTo0320(memStream);
        }

        /// <summary>
        /// Upgrades the layout file to version 0320 from unversioned.
        /// </summary>
        /// <param name="memStream">The mem stream.</param>
        private static void UpgradeLayoutTo0320(Stream memStream)
        {
            var transform = GetLayoutTransform("0320");

            RunTransform(memStream, transform);
        }

        private static void RunTransform(Stream memStream, XslCompiledTransform transform)
        {
            memStream.Position = 0;

            // We have to create a string reader here due to the fact that the Layout file will not have
            // an XML header, this means that the XmlReader class cannot determine which encoding is used
            // and will default to a potentially incorrect one.
            StreamReader strmreader = new StreamReader(memStream);
            string str = strmreader.ReadToEnd();
            memStream.Position = 0;

            System.Console.Write(str);
            StringReader strReader = new StringReader(str);
            using (XmlReader reader = XmlReader.Create(strReader))
            {
                XPathDocument doc = new XPathDocument(reader);
                XmlWriterSettings settings = transform.OutputSettings.Clone();
                settings.CloseOutput = false;
                settings.Indent = true;

                memStream.SetLength(0);
                using (XmlWriter writer = XmlWriter.Create(memStream, settings))
                {
                    transform.Transform(doc, writer);
                    writer.Close();

                    memStream.Position = 0;
                }
            }
        }

        private static XslCompiledTransform GetLayoutTransform(string version)
        {
            XslCompiledTransform myXslTrans = new XslCompiledTransform();

            var resourceUri = string.Format("InstallerActions.LayoutTransforms.{0}.xslt", version);
            var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceUri);

            XmlReader xslReader = XmlReader.Create(stream);
            myXslTrans.Load(xslReader);

            return myXslTrans;
        }
    }
}
