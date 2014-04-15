using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using System.Collections.Generic;

namespace TraceLabComponentWizard
{
    public class TraceLabComponentWizard : IWizard
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

            replacementsDictionary.Add("$componentname$", newForm.InputLabel);
            replacementsDictionary.Add("$description$", newForm.InputDescription);
            replacementsDictionary.Add("$author$", newForm.InputAuthor);
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        } 
    }
}