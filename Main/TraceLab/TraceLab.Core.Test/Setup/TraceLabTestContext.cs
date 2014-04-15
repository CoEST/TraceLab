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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Workspaces.Serialization;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using TraceLab.Core.Experiments;
using TraceLab.Core.Workspaces;
using TraceLab.Core.Components;

namespace TraceLab.Core.Test.Setup
{
    internal class TraceLabTestApplication : IDisposable
    {
        public const string TestReaderComponentGUID = "25b373b0-e3ae-41a7-8915-914cc0c8637b";
        public const string TestWriterComponentGUID = "3e60ccf8-5ed0-4ee4-aa27-d3ae0ee2f0cc";
        public const string TestEmptyComponentGUID = "E8244E98-2D98-11E0-9317-5E44E0D72085";
        public const string IncrementerComponentGUID = "D45519DE-360C-11E0-9A39-3E21E0D72085";
        public const string MultiplierComponentGUID = "5653b972-3e37-4cf7-826f-f33a68030fa3";
        
        ~TraceLabTestApplication()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                WorkspaceInstance.Dispose();
            }

            if (File.Exists(OutputAssembly))
            {
                try
                {
                    System.IO.File.Delete(OutputAssembly);
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            ClearDecisions();
        }

        private string OutputAssembly
        {
            get
            {
                return System.IO.Path.Combine(BaseTestDirectory, "TestComponents", Context.TestName, "MockComponents.dll");
            }
        }

        public Workspace WorkspaceInstance
        {
            get;
            private set;
        }

        public StreamManager StreamManager
        {
            get;
            private set;
        }

        public TraceLab.Core.PackageSystem.PackageManager PackageManager
        {
            get;
            private set;
        }

        public TraceLab.Core.Components.ComponentsLibrary Components
        {
            get;
            private set;
        }

        public string BaseTestDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the workspace cache directory.
        /// </summary>
        public string CacheDirectory
        {
            get;
            private set;
        }

        public TraceLab.Core.Settings.Settings Settings
        {
            get;
            private set;
        }

        private string DeployedDirectory
        {
            get;
            set;
        }

        private TestContext Context
        {
            get;
            set;
        }

        internal TraceLabTestApplication(TestContext context) : this(context, true)
        {
        }

        internal TraceLabTestApplication(TestContext context, bool loadComponents)
        {
            Context = context;
            NLog.LogManager.Configuration = new NLog.Config.LoggingConfiguration();
            DecisionsToClear = new List<string>();

            //string inputDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //inputDir = System.IO.Path.Combine(inputDir, "..", "..", "..", "..", "Data", "GenericTestData");

            string testClass = context.FullyQualifiedTestClassName;
            int index = context.FullyQualifiedTestClassName.LastIndexOf('.');
            if (index != -1)
            {
                testClass = testClass.Substring(index + 1);
            }

            string outputDir = context.TestDeploymentDir;//Path.Combine(context.TestDeploymentDir, testClass, context.TestName);
            Directory.CreateDirectory(outputDir);

            DeployedDirectory = outputDir;
            BaseTestDirectory = DeployedDirectory;

            var types = System.IO.Path.Combine(BaseTestDirectory, "Types");
            System.IO.Directory.CreateDirectory(types);
            List<string> typesDir = new List<string>();
            typesDir.Add(types);

            string workspaceDirectory = System.IO.Path.Combine(BaseTestDirectory, "workspace");
            string cacheDirectory = System.IO.Path.Combine(BaseTestDirectory, "cache");
            CacheDirectory = cacheDirectory;

            PackageManager = new Core.PackageSystem.PackageManager();

            Settings = TraceLab.Core.Settings.Settings.GetSettings();
            StreamManager = StreamManager.CreateNewManager();
            WorkspaceInstance = (Workspace)typeof(WorkspaceManager).GetMethod("InitWorkspaceInternal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { typesDir, workspaceDirectory, cacheDirectory, StreamManager });
            Components = (ComponentsLibrary)typeof(ComponentsLibrary).GetMethod("InitInternal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { Settings.ComponentPaths }); ;

            //set the components directories
            string path = System.IO.Path.Combine(BaseTestDirectory, "TestComponents");
            Directory.CreateDirectory(path);
            Settings.ComponentPaths.Add(new Core.Settings.SettingsPath(true, path));

            TraceLab.Core.Decisions.DecisionCompilationRunner.DecisionDirectoryPath = path;
            Components.DataRoot = BaseTestDirectory;

            if (loadComponents)
            {
                path = Path.GetDirectoryName(OutputAssembly);
                Directory.CreateDirectory(path);
                Settings.ComponentPaths.Add(new Core.Settings.SettingsPath(true, path));
                LoadComponents();
            }
        }

        internal void LoadComponents()
        {
            ReadAppConfig();

            CompileMockComponents(OutputAssembly);
            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(Components));
            libraryAccessor.LoadComponentsDefinitions(WorkspaceInstance.TypeDirectories);
        }

        private void ReadAppConfig()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            try
            {
                doc.Load(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config");
            }
            catch (FileNotFoundException)
            {
                return;
            }

            System.Xml.XmlNamespaceManager manager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("bindings", "urn:schemas-microsoft-com:asm.v1");

            System.Xml.XmlNode root = doc.DocumentElement;

            System.Xml.XmlNode node = root.SelectSingleNode("//bindings:bindingRedirect", manager);

            if (node == null)
            {
                throw (new Exception("Invalid Configuration File"));
            }

            node = node.SelectSingleNode("@newVersion");

            if (node == null)
            {
                throw (new Exception("Invalid Configuration File"));
            }
        }

        public List<string> DecisionsToClear;

        private void ClearDecisions()
        {
            foreach (string decisionGUID in DecisionsToClear)
            {
                string decisionAssemblyDllPath = System.IO.Path.Combine(BaseTestDirectory, decisionGUID + ".dll");
                string decisionAssemblyPdbPath = System.IO.Path.Combine(BaseTestDirectory, decisionGUID + ".pdb");
                if (System.IO.File.Exists(decisionAssemblyDllPath))
                {
                    System.IO.File.Delete(decisionAssemblyDllPath);
                }
                if (System.IO.File.Exists(decisionAssemblyPdbPath))
                {
                    System.IO.File.Delete(decisionAssemblyPdbPath);
                }
            }
        }

        private static object s_lock = new object();
        public static void CompileMockComponents(string outputAssembly)
        {
            if(!File.Exists(outputAssembly))
            {
                // Create the C# compiler
                CSharpCodeProvider csCompiler = new CSharpCodeProvider();

                CompilerParameters compilerParams = new CompilerParameters();
                compilerParams.OutputAssembly = outputAssembly;
                compilerParams.ReferencedAssemblies.Add("System.dll");
                compilerParams.ReferencedAssemblies.Add("TraceLabSDK.dll");

                Assembly assembly = Assembly.GetExecutingAssembly();

                // generate the DLL
                compilerParams.GenerateExecutable = false;

                var sources = new string[] 
                { 
                    PrepareSource(assembly.GetManifestResourceStream("TraceLab.Core.Test.Setup.MockComponent.TestEmptyComponent.cs")),
                    PrepareSource(assembly.GetManifestResourceStream("TraceLab.Core.Test.Setup.MockComponent.TestReaderComponent.cs")),
                    PrepareSource(assembly.GetManifestResourceStream("TraceLab.Core.Test.Setup.MockComponent.TestWriterComponent.cs")),
                    PrepareSource(assembly.GetManifestResourceStream("TraceLab.Core.Test.Setup.MockComponent.IncrementerComponent.cs")),
                    PrepareSource(assembly.GetManifestResourceStream("TraceLab.Core.Test.Setup.MockComponent.MultiplierComponent.cs"))
                };

                // Run the compiler and build the assembly
                CompilerResults compilerResults = csCompiler.CompileAssemblyFromSource(compilerParams, sources);

                //if there are errors while throw exception
                if (compilerResults.Errors.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    foreach (CompilerError error in compilerResults.Errors)
                    {
                        errorMessage.AppendLine(error.ToString());
                    }
                    throw new ArgumentException("Errors occured in mock components compilation. " + errorMessage.ToString());
                }
            }
        }

        private static string PrepareSource(Stream stream)
        {
            StreamReader textStreamReader = new StreamReader(stream);

            StringBuilder builder = new StringBuilder();
            string line;
            while ((line = textStreamReader.ReadLine()) != null)
            {
                builder.AppendLine(line);
            }

            return builder.ToString();
        }
    }
}
