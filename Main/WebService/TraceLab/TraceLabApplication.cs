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
using System.IO;
using System.Linq;
using TraceLab.Core.Benchmarks;
using TraceLab.Core.Components;
using TraceLab.Core.Experiments;
using TraceLab.Core.PackageSystem;
using TraceLab.Core.Settings;
using TraceLab.Core.ViewModels;
using TraceLab.Core.Workspaces;

namespace TraceLab
{
    public abstract class TraceLabApplication
    {
        #region Run

        public void Run(string[] args)
        {
            Initialize (args);

            ApplicationViewModel.RegisteredUser = KeyValidator.ValidateKey (UserDirectory);

            if (InitViewModel ()) {
                RunUI ();
            } 
                      
        }

        protected abstract void RunUI();

        #endregion Run

        #region Command Line Processing

        /// <summary>
        /// Initializes the <see cref="TraceLabApplication"/> class.
        /// </summary>
        static TraceLabApplication()
        {
            ComponentDirectories = new List<string>();
            TypeDirectories = new List<string>();
            PackageDirectories = new List<string>();
            PackagesToInstall = new List<string>();

            Processor = new CommandLineProcessor();
        //    SetExperimentFileToBeOpen ( "C:"+System.IO.Path.DirectorySeparatorChar+"Users"+Path.DirectorySeparatorChar+"emanuele.forlano"+Path.DirectorySeparatorChar+"Desktop"+Path.DirectorySeparatorChar+"traelab-deploy"+Path.DirectorySeparatorChar+"WINDOWS"+Path.DirectorySeparatorChar+"twhile.teml");
          
            Processor.Commands["o"] = new Action<string>(SetExperimentFileToBeOpen);
            Processor.Commands["open"] = new Action<string>(SetExperimentFileToBeOpen);

            Processor.Commands["c"] = new Action<string>(SetComponentsDirectory);
            Processor.Commands["components"] = new Action<string>(SetComponentsDirectory);

            Processor.Commands["t"] = new Action<string>(SetTypesDirectory);
            Processor.Commands["types"] = new Action<string>(SetTypesDirectory);

            Processor.Commands["p"] = new Action<string>(SetPackagesDirectory);
            Processor.Commands["packages"] = new Action<string>(SetPackagesDirectory);

            Processor.Commands["d"] = new Action<string>(SetDecisionsDirectory);
            Processor.Commands["decisions"] = new Action<string>(SetDecisionsDirectory);

            Processor.Commands["w"] = new Action<string>(SetWorkspaceDirectory);
            Processor.Commands["workspace"] = new Action<string>(SetWorkspaceDirectory);

            Processor.Commands["x"] = new Action<string>(SetCacheDirectory);
            Processor.Commands["cache"] = new Action<string>(SetCacheDirectory);

            Processor.Commands["base"] = new Action<string>(SetBaseDirectory);

            Processor.Commands["?"] = new Action<string>(DisplayCommandLineHelp);

            Processor.Commands["installpackage"] = new Action<string>(InstallPackage);
        }

        private static CommandLineProcessor Processor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether TraceLab should only process the command line.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if TraceLab should only process command line and then exit; otherwise, <c>false</c>.
        /// </value>
        private static bool ShouldOnlyProcessCommandLine
        {
            get;
            set;
        }

        #region Commands Actions

        /// <summary>
        /// Sets the experiment file to be open at startup
        /// </summary>
        /// <param name="value">The value.</param>
        private static void SetExperimentFileToBeOpen(string value)
        {
         if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            ExperimentFileToBeOpen = value;
           
        }

        private static void SetComponentsDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            ComponentDirectories = SplitDirectoryList(value);
        }

        private static void SetTypesDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            TypeDirectories = SplitDirectoryList(value);
        }

        private static void SetPackagesDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            PackageDirectories = SplitDirectoryList(value);
        }

        private static List<string> SplitDirectoryList(string value)
        {
            var newList = new List<string>();

            // First some basic validation to ensure that all the listed directories exist.
            string[] splitDirectories = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string dir in splitDirectories)
            {
                newList.Add(dir);
            }

            return newList;
        }

        private static void SetDecisionsDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            DecisionDirectory = value;
        }

        private static void SetWorkspaceDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            WorkspaceDirectory = value;
        }

        private static void SetCacheDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            CacheDirectory = value;
        }

        private static void SetBaseDirectory(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            BaseDirectory = value;
        }

        private static void DisplayCommandLineHelp(string value)
        {
            Console.WriteLine("COEST TraceLab");
            Console.WriteLine("\tUsage: TraceLab.exe {{0}argument:value}", CommandLineProcessor.SwitchCharacter);
            Console.WriteLine();
            Console.WriteLine("Optional arguments:");
            Console.WriteLine("\topen\t- Specifies the experiment file to be open on startup of TraceLab.  Abbreviated as: o");
            Console.WriteLine("\tinstallpackage\t- Specifies a package file to be installed into the TraceLab package repository.");
            Console.WriteLine("\tcomponents\t- Specifies the directories to load components from in a semi-colon delimited list.  Abbreviated as: c");
            Console.WriteLine("\ttypes\t- Specifies the directories to load Workspace types from in a semi-colon delimited list.  Abbreviated as: t");
            Console.WriteLine("\tpackagess\t- Specifies the directories to load packages from in a semi-colon delimited list.  Abbreviated as: p");
            Console.WriteLine("\tdecisions\t- Specifies the directory to load/save decisions from.  This is largely a temporary folder.  Abbreviated as: d");
            //Console.WriteLine("\tworkspace\t- Specifies the directory that the Workspace will use as a file-based backing store. (Obsolete)  Abbreviated as: w");
            //Console.WriteLine("\tcache\t- Specifies the directory that the Workspace will use as a file-based fast-access backing store. (Obsolete) Abbreviated as: x");
            Console.WriteLine("\tbase\t- Specifies the base directory of TraceLab.  Used when running the portable version.");
            Console.WriteLine("\t\t\t- TraceLab then assumes where all the other directories should be.  This option overrides all others.");
            Console.WriteLine("\t?\t- This help message.");

            ShouldOnlyProcessCommandLine = true;
        }

        private static void InstallPackage(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value", "Package to install must not be an empty string.");
            if (!System.IO.File.Exists(value))
                throw new ArgumentException("Package file to install must exist.", "value");

            PackagesToInstall.Add(value);
        }

        #endregion Commands Actions

        #endregion Command Line Processing

        #region Initialization

        private const string TraceLabRegistryKey = @"Software\COEST\TraceLab";
        private const string TraceLabWow64RegistryKey = @"Software\Wow6432Node\COEST\TraceLab";

        private void Initialize (string[] args)
        {
            InitLog ();

            Processor.Parse (args);

            if (ShouldOnlyProcessCommandLine) {
                return;
            }

            //case 1: base directory is specified in commands arguments
            //init all directories residing in that directory
            if (BaseDirectory != null) 
            {
                UserDirectory = BaseDirectory;
                ComponentDirectories.Add (System.IO.Path.Combine (BaseDirectory, "Components"));
                TypeDirectories.Add (System.IO.Path.Combine (BaseDirectory, "Types"));
                PackageDirectories.Add (System.IO.Path.Combine (BaseDirectory, "Packages"));
                WorkspaceDirectory = System.IO.Path.Combine (BaseDirectory, "workspace");
                CacheDirectory = System.IO.Path.Combine (BaseDirectory, "cache");
                DecisionDirectory = System.IO.Path.Combine (BaseDirectory, "Decisions");
                BenchmarkDirectory = System.IO.Path.Combine (BaseDirectory, "Benchmarks");
            }

            //case 2. application run on mono && BaseDirectory has not been provided in command line arguments
            //
            if (TraceLabSDK.RuntimeInfo.IsRunInMono && TraceLabSDK.RuntimeInfo.OperatingSystem != TraceLabSDK.RuntimeInfo.OS.Windows 
                && BaseDirectory == null) 
            {
                string executionDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                UserDirectory = System.IO.Path.Combine (Environment.GetEnvironmentVariable ("HOME"), "TraceLab");

                //these directories are in the bundle so they should exists
                ComponentDirectories.Add (System.IO.Path.Combine (executionDirectory, "Components"));
                TypeDirectories.Add (System.IO.Path.Combine (executionDirectory, "Types"));
                PackageDirectories.Add (System.IO.Path.Combine (executionDirectory, "Packages"));
            
                //these directories in the user directory should be created first time (or during installation TODO)
                try
                {
                    string componentDir = System.IO.Path.Combine(UserDirectory, "Components");
                    ComponentDirectories.Add(componentDir);
                    System.IO.Directory.CreateDirectory(componentDir);
                   
                    string typeDir = System.IO.Path.Combine(UserDirectory, "Types");
                    TypeDirectories.Add(typeDir);
                    System.IO.Directory.CreateDirectory(typeDir);

                    string packageDir = System.IO.Path.Combine(UserDirectory, "Packages");
                    PackageDirectories.Add(packageDir);
                    System.IO.Directory.CreateDirectory(packageDir);
                }
                catch(Exception)
                {
                }

                if (WorkspaceDirectory == null)
                {
                    WorkspaceDirectory = System.IO.Path.Combine(UserDirectory, "workspace");
                }
                if (CacheDirectory == null)
                {
                    CacheDirectory = System.IO.Path.Combine(UserDirectory, "cache");
                }

                if (DecisionDirectory == null)
                {
                    DecisionDirectory = System.IO.Path.Combine(UserDirectory, "Decisions");
                }

                if (BenchmarkDirectory == null)
                {
                    BenchmarkDirectory = System.IO.Path.Combine(UserDirectory, "Benchmarks");
                }
            }
            
            //case 3. application run on windows & BaseDirectory has not been provided in command line arguments
            //thus get values from Windows Registry
            if (BaseDirectory == null && TraceLabSDK.RuntimeInfo.OperatingSystem == TraceLabSDK.RuntimeInfo.OS.Windows) 
            {
                // On 64bit machines, the installer might place the registry key in the Wow6432Node subkey of Software, so we have to check
                // both locations.
                var globalKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (TraceLabRegistryKey);
                if (globalKey == null) {
                    globalKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (TraceLabWow64RegistryKey);
                }

                var localKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey (TraceLabRegistryKey);
                if (localKey == null) {
                    localKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey (TraceLabWow64RegistryKey);
                }

                // If the directories were not specified on the commandline, then read whatever we have in the registry.
                if (ComponentDirectories.Count == 0) {
                    AddValueIfExists<string> (globalKey, "Components", ComponentDirectories);
                    AddValueIfExists<string> (localKey, "Components", ComponentDirectories);
                }

                // If the types were not specified on the commandline, the read whatever we have in the registry.
                if (TypeDirectories.Count == 0) {
                    AddValueIfExists<string> (globalKey, "Types", TypeDirectories);
                    AddValueIfExists<string> (localKey, "Types", TypeDirectories);
                }

                if (PackageDirectories.Count == 0) {
                    AddValueIfExists<string> (globalKey, "Packages", PackageDirectories);
                    AddValueIfExists<string> (localKey, "Packages", PackageDirectories);
                }

                if (localKey != null) {
                    UserDirectory = (string)localKey.GetValue ("UserDirectory");

                    if (WorkspaceDirectory == null)
                    {
                        WorkspaceDirectory = System.IO.Path.Combine(UserDirectory, "workspace");
                    }
                    if (CacheDirectory == null)
                    {
                        CacheDirectory = System.IO.Path.Combine(UserDirectory, "cache");
                    }

                    if (DecisionDirectory == null)
                    {
                        DecisionDirectory = System.IO.Path.Combine(UserDirectory, "Decisions");
                    }

                    if (BenchmarkDirectory == null)
                    {
                        BenchmarkDirectory = System.IO.Path.Combine(UserDirectory, "Benchmarks");
                    }
                }


            }
        }

        private static void InitLog()
        {
            LogViewModel.InitLogging();
            initialLogViewModel = new LogViewModel();
        }

        private static TraceLab.Core.ViewModels.LogViewModel initialLogViewModel;

        private static void AddValueIfExists<T>(Microsoft.Win32.RegistryKey key, string valueName, List<T> collection)
        {
            if (key != null)
            {
                var value = (T)key.GetValue(valueName);
                if (value != null)
                {
                    collection.Add(value);
                }
            }
        }

        #endregion Initialization

        #region View Model Initialization

        private bool InitViewModel()
        {
            bool startTraceLab = true;
            var settings = TraceLab.Core.Settings.Settings.GetSettings();

            for (int i = 0; i < TypeDirectories.Count; ++i)
            {
                var newSettingPath = new TraceLab.Core.Settings.SettingsPath(true, TypeDirectories[i]);

                if(!settings.TypePaths.Contains(newSettingPath))
                    settings.TypePaths.Insert(i, newSettingPath);
            }

            for (int i = 0; i < ComponentDirectories.Count; ++i)
            {
                var newSettingPath = new TraceLab.Core.Settings.SettingsPath(true, ComponentDirectories[i]);

                if(!settings.ComponentPaths.Contains(newSettingPath))
                    settings.ComponentPaths.Insert(i, newSettingPath);
            }

            for (int i = 0; i < PackageDirectories.Count; ++i)
            {
                settings.PackagePaths.Insert(i, new TraceLab.Core.Settings.SettingsPath(true, PackageDirectories[i]));
            }

            TraceLab.Core.PackageSystem.PackageManager.Instance.SetPackageLocations(settings.PackagePaths);
            TraceLab.Core.PackageSystem.PackageManager.Instance.ScanForPackages();

            // Install any packages that were queued at the start
            var packagesDir = settings.PackagePaths.LastOrDefault();
            if (PackagesToInstall.Count > 0)
                UnpackNewPackagesIn ((string)packagesDir);
                /*
                if (UnpackNewPackagesIn((string)packagesDir))
                {
                    System.Windows.Forms.MessageBox.Show("New package(s) have been installed. TraceLab needs to be re-launched.",
                        "Closing TraceLab", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Exclamation);
                    startTraceLab = false;
                }
                else
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Would you like to continue openning TraceLab?",
                        "Start TraceLab", System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        startTraceLab = false;
                    }
                }
            }

            if (TraceLabSDK.RuntimeInfo.IsRunInMono) {
                //  we force Tracelab execution because if Tracelab runs in mono when one opens a package mono.exe remains in execution 
                // also after the unpacking operation is done
                startTraceLab = true;
            }
*/
            if (startTraceLab)
            {
                // Append all the type directories from the package locations.
                var typeDirectories = new List<string>(settings.TypePaths.Select(path => path.Path));

                Workspace = InitWorkspace(typeDirectories, WorkspaceDirectory, CacheDirectory);
                MainViewModel = new ApplicationViewModel(Workspace);
                MainViewModel.LogViewModel = initialLogViewModel;
                MainViewModel.Settings = settings;
                MainViewModel.PropertyChanged += MainViewModel_PropertyChanged;

                var componentLibrary = CreateComponentLibraryViewModel(PackageManager.Instance, settings, DecisionDirectory, UserDirectory);
                Workspace.RegisterPackageTypes(componentLibrary.PackageTypeDirectories);
                MainViewModel.ComponentLibraryViewModel = componentLibrary;
                MainViewModel.BenchmarkWizard = new BenchmarkWizard(BenchmarkDirectory, ComponentsLibrary.Instance, Workspace, typeDirectories, UserDirectory, settings);
                           
                if (String.IsNullOrEmpty(ExperimentFileToBeOpen) == false || string.IsNullOrEmpty(settings.DefaultExperiment) == false)
                {
                    string file = string.IsNullOrEmpty(ExperimentFileToBeOpen) ? settings.DefaultExperiment : ExperimentFileToBeOpen;
                    try
                    {
                        var w2 = new List<string>(settings.TypePaths.Select(path => path.Path));
                        ComponentsLibrary.Instance.Rescan(PackageManager.Instance, w2, false);
                        MainViewModel.Experiment = TraceLab.Core.Experiments.ExperimentManager.Load(file, ComponentsLibrary.Instance);
                        RecentExperimentsHelper.UpdateRecentExperimentList(TraceLab.Core.Settings.Settings.RecentExperimentsPath, file);
                    }
                    catch (TraceLab.Core.Exceptions.ExperimentLoadException ex)
                    {
                        string msg = String.Format("Unable to open the file {0}. Error: {1}", file, ex.Message);
                        NLog.LogManager.GetCurrentClassLogger().Warn(msg);

                        if (!TraceLabSDK.RuntimeInfo.IsRunInMono)
                        {
                            System.Windows.Forms.MessageBox.Show(msg, "Experiment Loading Failure", 
                                                                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
         
            return startTraceLab;

        }

        private static TraceLab.Core.Workspaces.Workspace InitWorkspace(IEnumerable<string> typeDirectories, string workspaceDirectory, string cacheDirectory)
        {
            //Init workspace
            //TODO pass IEnumerable instead of List into workspaces... 
            WorkspaceManager.InitWorkspace(new List<string>(typeDirectories), workspaceDirectory, cacheDirectory);
            TraceLab.Core.Workspaces.Workspace workspace = (TraceLab.Core.Workspaces.Workspace)WorkspaceManager.WorkspaceInstance;

            System.Reflection.Assembly sdk = System.Reflection.Assembly.GetAssembly(typeof(TraceLabSDK.IWorkspace));

            foreach (Type type in sdk.GetTypes())
            {
                if (type.Namespace.Equals("TraceLabSDK.Types"))
                {
                    workspace.RegisterType(type);
                }
            }

            return workspace;
        }

        private static TraceLab.Core.ViewModels.ComponentLibraryViewModel CreateComponentLibraryViewModel(PackageManager pkgManager,  TraceLab.Core.Settings.Settings settings, string decisionDirectory, string dataRoot)
        {
            ComponentsLibrary.Init(settings.ComponentPaths);

            //Load stuff to components library
            TraceLab.Core.Decisions.DecisionCompilationRunner.DecisionDirectoryPath = decisionDirectory;
            ComponentsLibrary.Instance.DataRoot = dataRoot;

            var workspaceTypeDirectories = new List<string>(settings.TypePaths.Select(path => path.Path));
            ComponentsLibrary.Instance.Rescan(pkgManager, workspaceTypeDirectories, false);

            foreach (SettingsPath path in settings.ComponentPaths)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Info("Reading components from: {0}", path.Path);
            }

            var compvm = new ComponentLibraryViewModel(ComponentsLibrary.Instance, workspaceTypeDirectories);
            return compvm;
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Experiment")
            {
                var viewModel = sender as ApplicationViewModel;
                viewModel.Experiment.Settings = viewModel.Settings;
                viewModel.WorkspaceViewModel = new WorkspaceViewModel(Workspace, viewModel.Experiment.ExperimentInfo.Id);
                viewModel.LogViewModel = new LogViewModel(viewModel.Experiment.ExperimentInfo.Id, viewModel.LogViewModel);
            }
        }

        #endregion View Model Initialization

        #region Install Packages

        /// <summary>
        /// Unpacks new packages in the specified directory.
        /// </summary>
        /// <param name="packagesDirectory">Directory to unpack the packages.</param>
        /// <returns>Number of packages successfully unpacked.</returns>
        private static bool UnpackNewPackagesIn(string packagesDirectory)
        {
            int packagesInstalled = 0;

            foreach (string packagePath in PackagesToInstall)
            {
                try
                {
                    using (FileStream stream = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
                    {
                        bool createPkg = true;
                        Package pkg = new Package(stream);
                        String pkgDirectory = packagesDirectory + "\\" + pkg.Name;

                        if (Directory.Exists(pkgDirectory))
                        {
                           var result = System.Windows.Forms.MessageBox.Show("Package \"" + pkg.Name + "\" already exists.  Would you like to overwrite it?",
                                            "Package Overwrite Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo,
                                            System.Windows.Forms.MessageBoxIcon.Question);

                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                try
                                {
                                    Directory.Delete(pkgDirectory, true);
                                }
                                catch (System.IO.IOException e)
                                {
                                    createPkg = false;
                                    System.Windows.Forms.MessageBox.Show("Unable to install package - " + e.Message,
                                        "Package Installation Error", System.Windows.Forms.MessageBoxButtons.OK,
                                        System.Windows.Forms.MessageBoxIcon.Error);
                                }
                                catch (System.UnauthorizedAccessException)
                                {
                                    createPkg = false;
                                    System.Windows.Forms.MessageBox.Show("Unable to install package - Unauthorized access to path.",
                                        "Package Installation Error", System.Windows.Forms.MessageBoxButtons.OK,
                                        System.Windows.Forms.MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                createPkg = false;
                                System.Windows.Forms.MessageBox.Show("Package \"" + pkg.Name + "\" was not installed.",
                                    "Package Installation Cancelled", System.Windows.Forms.MessageBoxButtons.OK,
                                    System.Windows.Forms.MessageBoxIcon.Exclamation);
                            }
                        }

                        if (createPkg)
                        {
                            pkg.Unpack(packagesDirectory);
                            packagesInstalled++;
                            System.Windows.Forms.MessageBox.Show(
                                String.Format("Package \"" + pkg.Name + "\" was installed successfully into \n {0}.", packagesDirectory),
                                "New Package Installed", System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Unable to install package - " + e.Message,
                        "Package Installation Failed", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            return (packagesInstalled > 0);
        }

        #endregion

        #region Application Properties

        public static TraceLab.Core.ViewModels.ApplicationViewModel MainViewModel
        {
            get;
            private set;
        }

        public Workspace Workspace
        {
            get;
            private set;
        }

        public ComponentsLibrary Library
        {
            get;
            private set;
        }

        public static List<string> ComponentDirectories
        {
            get;
            private set;
        }

        public static List<string> TypeDirectories
        {
            get;
            private set;
        }

        public static List<string> PackageDirectories
        {
            get;
            private set;
        }

        public static string BenchmarkDirectory
        {
            get;
            private set;
        }

        public static string DecisionDirectory
        {
            get;
            private set;
        }

        public static string WorkspaceDirectory
        {
            get;
            private set;
        }

        public static string CacheDirectory
        {
            get;
            private set;
        }

        public static string UserDirectory
        {
            get;
            private set;
        }

        public static string BaseDirectory
        {
            get;
            private set;
        }

        public static List<string> PackagesToInstall
        {
            get;
            private set;
        }

        public static string ExperimentFileToBeOpen
        {
            get;
            private set;
        }

        #endregion Application Properties
    }
}
