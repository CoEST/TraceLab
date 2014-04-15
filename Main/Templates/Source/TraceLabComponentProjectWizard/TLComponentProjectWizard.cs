using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.Collections.Generic;

namespace TraceLabComponentProjectWizard
{
    public class TLComponentProjectWizard : IWizard
    {
        private UserInputForm newForm;

        // This method is called before opening any item that 
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            newForm = new UserInputForm();
            newForm.ShowDialog();

            replacementsDictionary.Add("$TraceLabSDKPath$", newForm.TraceLabSDKPath);
            replacementsDictionary.Add("$TraceLabTypePath$", newForm.TraceLabTypesPath);
            replacementsDictionary.Add("$OutputDirectory$", newForm.OutputDirectoryPath);
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}