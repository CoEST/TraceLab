How to use the script:
1. Install Inno-Setup. You can download it from http://www.jrsoftware.org/isinfo.php or you can use the installer provided
2. Open the file "TraceLab_installer_script.iss"
3. Update the path to the RelaseMono or DebugMono folder on your file system in the section [Files]
4. Compile or Run it
5. After the compilation you can find the genereted "TraceLab-setup.exe" in the "Output" folder


More information about Sections:
[Registry]
This optional section defines any registry keys/values you would like Setup to create, modify, or delete on the user's system.
We added it to link the TraceLab extensions (teml, temlx, and tpkg) with the correct icons and command Windows has to execute when you double clck on it.

[Icons]
This optional section defines any shortcuts Setup is to create in the Start Menu and/or other locations, such as the desktop.
We use it to add the correct icon for the shortcut installed on the Desktop

