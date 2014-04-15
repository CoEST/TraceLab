using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;
using System.ComponentModel;
using TraceLab.Core.PackageSystem;

namespace TraceLab.UI.WPF.Views.PackageBuilder.PackageSource
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

            this.ExperimentPackageConfig = new ExperimentPackageConfig();

            CurrentState = PackageBuilderWizardPage.Config;
        }

        public ExperimentPackagingResults Pack()
        {
            ExperimentPackagingHelper ePkgHelper = new ExperimentPackagingHelper(this.ExperimentPackageConfig, m_supportedTypes);

            ExperimentPackagingResults ePkgResults = ePkgHelper.PackExperiment(m_experiment);

            return ePkgResults;
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
