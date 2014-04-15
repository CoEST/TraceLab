Wizard dll needs to be installed into the gac.

1. To Install open Visual Studio command prompt.

2. type in gacutil -i "<Full path to this dll>"

3. Restart VS and run.


If dll is already installed and needs to be edited you need to first uninstall the existant 
assembly and then reinstall

1. Edit wizard and template as needed.

2. Recompile everything

3. Open Visual Studio Command Prompt.

4. uninstall the assembly by typing in gacutil -u "TraceLabComponentWizard, Version=1.0.0.0, Culture=neutral, PublicKeyToken647faf833dc683be"

5. See installation of Gac.