using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;
using System.ComponentModel;
using TraceLab.Core.PackageSystem;

namespace TraceLab.Core.PackageBuilder
{
    public class PackageBuilderViewModel : INotifyPropertyChanged
    {
        private Experiment m_experiment;
        private Dictionary<string, Type> m_supportedTypes;

        public PackageBuilderViewModel() 
        {
            m_packageSourceInfo = new PackageSourceInfo();
            CurrentState = PackageBuilderWizardPage.FileViewer;
        }

        public PackageBuilderViewModel(Experiment originalExperiment, Dictionary<string, Type> supportedTypes)
        {
            //clone current experiment
            m_experiment = originalExperiment.Clone() as Experiment;
            m_supportedTypes = supportedTypes;
            m_isExperimentPackage = true;

            this.ExperimentPackageConfig = new ExperimentPackageConfig();

            CurrentState = PackageBuilderWizardPage.Config;
        }

        private PackageSourceInfo m_packageSourceInfo;

        public PackageSourceInfo PackageSourceInfo
        {
            get { return m_packageSourceInfo; }
            set 
            { 
                m_packageSourceInfo = value;
                NotifyPropertyChanged("PackageSourceInfo");
            }
        }

        private ExperimentPackageConfig m_experimentPackageConfig;

        public ExperimentPackageConfig ExperimentPackageConfig
        {
            get { return m_experimentPackageConfig; }
            set 
            { 
                m_experimentPackageConfig = value;
                NotifyPropertyChanged("ExperimentPackageConfig");
            }
        }

        private PackageBuilderWizardPage m_currentState;

        public PackageBuilderWizardPage CurrentState
        {
            get { return m_currentState; }
            set 
            {
                if (m_currentState != value)
                {
                    m_currentState = value;
                    NotifyPropertyChanged("CurrentState");
                }
            }
        }

        /// <summary>
        /// Especifies if the package is a self-contained experiment package
        /// </summary>
        private bool m_isExperimentPackage = false;

        public bool IsExperimentPackage
        {
            get { return m_isExperimentPackage; }
        }


        /// <summary>
        /// Sets the contents of the package from the given experiment, types and components.
        /// </summary>
        /// <param name="pInfo">Experiment info.</param>
        /// <param name="pTypes">Collection of type assembly files.</param>
        /// <param name="pComponents">Collection of component assembly files.</param>
        /// <returns></returns>
        public void GeneratePackageContent()
        {
            //firstly pack experiment
            ExperimentPackagingHelper ePkgHelper = new ExperimentPackagingHelper(this.ExperimentPackageConfig, m_supportedTypes);

            ExperimentPackagingResults pResults = ePkgHelper.PackExperiment(m_experiment);
            
            var info = new PackageSourceInfo();

            // Adding components
            if (pResults.ComponentAssemblies.Count > 0)
            {
                PackageHeirarchyItem componentsFolder = CreateFolder(info.Root, "Components");
                componentsFolder.HasComponents = true;
                foreach (string component in pResults.ComponentAssemblies)
                {
                    AddFile(componentsFolder, component);
                }
            }

            // Adding experiment
            PackageHeirarchyItem experimentFolder = CreateFolder(info.Root, "Experiment");
            AddFile(experimentFolder, pResults.Experiment.ExperimentInfo.FilePath);

            // HERZUM SPRINT 4.2: TLAB-215
            // info.Name = pResults.Experiment.ExperimentInfo.Name + " Package";
            info.Name = pResults.Experiment.ExperimentInfo.Name + "_package";
            // END HERZUM SPRINT 4.2: TLAB-215

            // Adding refered types into subfolder of Experiment
            if (pResults.Files.Count > 0)
            {
                foreach (PackageFileInfo file in pResults.Files)
                {
                    PackageHeirarchyItem lastFolder = CreateRelativeFolders(experimentFolder, file);
                    AddFile(lastFolder, file.AbsoluteLocation);
                }
            }

            //Adding refered directories into subfolder of Experiment
            if (pResults.Directories.Count > 0)
            {
                foreach (PackageFileInfo dir in pResults.Directories)
                {
                    PackageHeirarchyItem lastFolder = CreateRelativeFolders(experimentFolder, dir);
                    AddFolder(lastFolder, dir.AbsoluteLocation);
                }
            }

            // Adding types
            if (pResults.TypeAssemblies.Count > 0)
            {
                PackageHeirarchyItem typesFolder = CreateFolder(info.Root, "Types");
                typesFolder.HasTypes = true;
                foreach (string type in pResults.TypeAssemblies)
                {
                    AddFile(typesFolder, type);
                }
            }

            this.PackageSourceInfo = info;
        }

        private PackageHeirarchyItem CreateRelativeFolders(PackageHeirarchyItem experimentFolder, PackageFileInfo file)
        {
            PackageHeirarchyItem lastFolder = experimentFolder;
            foreach (string folder in file.FoldersPath)
            {
                PackageHeirarchyItem folderInfo;
                if (ContainsFolder(lastFolder, folder, out folderInfo))
                {
                    lastFolder = folderInfo;
                }
                else
                {
                    lastFolder = CreateFolder(lastFolder, folder);
                }
            }
            return lastFolder;
        }

        #region Tree Manipulation

        /// <summary>
        /// Creates an empty folder inside the PackageHeirarchyItem provided.
        /// </summary>
        /// <param name="pParent">Parent node of the new folder.</param>
        /// <param name="pFolderName">Name of the folder.</param>
        /// <returns></returns>
        private PackageHeirarchyItem CreateFolder(PackageHeirarchyItem pParent, string pFolderName)
        {
            PackageHeirarchyItem newFolder = new PackageHeirarchyItem("");
            newFolder.Name = pFolderName;
            newFolder.Parent = pParent;
            pParent.Children.Add(newFolder);

            return newFolder;
        }

        public PackageFileSourceInfo Add(PackageHeirarchyItem parent, string filePath)
        {
            PackageFileSourceInfo newItem = null;

            if (System.IO.File.Exists(filePath))
            {
                newItem = AddFile(parent, filePath);
            }
            else if(System.IO.Directory.Exists(filePath))
            {
                newItem = AddFolder(parent, filePath);
            }

            return newItem;
        }

        private PackageFileSourceInfo AddFile(PackageHeirarchyItem parent, string filePath)
        {
            PackageFileSourceInfo newItem = null;

            var name = System.IO.Path.GetFileName(filePath);
            if (parent != null && !Contains(parent, name))
            {
                newItem = new PackageFileSourceInfo(filePath);
                newItem.Name = name;
                newItem.Parent = parent;

                parent.Children.Add(newItem);
            }

            return newItem;
        }

        private PackageFileSourceInfo AddFolder(PackageHeirarchyItem parent, string filePath)
        {
            PackageHeirarchyItem newItem = null;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(filePath);
            if (parent != null && !Contains(parent, dir.Name))
            {
                newItem = new PackageHeirarchyItem(filePath);
                newItem.Name = dir.Name;
                newItem.Parent = parent;

                parent.Children.Add(newItem);

                // EnumerateFiles is not working on mono for some reason, therefore use GetFiles instead,
                IEnumerable<System.IO.FileInfo> files;
                if(TraceLabSDK.RuntimeInfo.IsRunInMono) 
                {
                    files = dir.GetFiles();
                } 
                else 
                {
                    files = dir.EnumerateFiles();
                }

                foreach (System.IO.FileInfo file in files)
                {
                    AddFile(newItem, file.FullName);
                }

                foreach (System.IO.DirectoryInfo subDir in dir.EnumerateDirectories())
                {
                    AddFolder(newItem, subDir.FullName);
                }
            }

            return newItem;
        }

        static bool Contains(PackageHeirarchyItem info, string name)
        {
            bool contains = false;
            foreach (PackageFileSourceInfo item in info.Children)
            {
                if (string.Equals(name, item.Name))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        static bool ContainsFolder(PackageHeirarchyItem info, string name, out PackageHeirarchyItem resultFolder)
        {
            resultFolder = null;
            bool contains = false;
            foreach (PackageFileSourceInfo item in info.Children)
            {
                if (string.Equals(name, item.Name))
                {
                    resultFolder = item as PackageHeirarchyItem;
                    //if resultFolder now is different than null it means there is such folder in hierarchy,
                    //otherwise there is file of same name - so return false
                    if (resultFolder != null)
                    {
                        contains = true;
                    }
                    break;
                }
            }
            return contains;
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }

    public enum PackageBuilderWizardPage
    {
        Config,
        FileViewer
    }
}
