To edit template file open zip and edit Cs file.
This is the starting code that will appear when you create the template
$something$ are tags for parameters that are either built into VS or are being used by the wizard.

.vstemplate is the template file that associates the .cs file with the wizard and displays the correct Icon in 
VS when adding a new Item.

To add template to VS first add the wizard .dll to the GAC and then proceed to put this .zip file into your 
"C:\Users\<YOUR NAME>\Documents\Visual Studio 2010\Templates\ItemTemplates\Visual C#" folder


If assembly version of TraceLabComponentWizard or TraceLabComponentProjectWizard changes, also update .vstemplate files to refer to correct version of assembly. 