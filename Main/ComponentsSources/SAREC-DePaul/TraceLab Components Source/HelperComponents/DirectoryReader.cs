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
// Located in c:\Program Files (x86)\COEST\TraceLab\lib\TraceLabSDK.dll
using TraceLabSDK;
using System.IO;
using TraceLabSDK.Types.Generics.Collections;
using TraceLabSDK.Component.Config;

namespace HelperComponents
{
    [Component(GuidIDString = "3dd86801-e9a2-4286-823d-45521d62a80e",
                Name = "DirectoryReader",
                DefaultLabel = "Directory Reader",
                Description = "It reads files from the directory that match the specified pattern and outputs the files into the list of Strings. It also outputs the count of all the files.",
                Author = "DePaul RE Team",
                Version = "1.0",
                ConfigurationType=typeof(DirectoryReaderConfig))]
    [IOSpec(IOSpecType.Output, "files", typeof(StringList))]
    [IOSpec(IOSpecType.Output, "numberOfFiles", typeof(int))]
    [Tag("Helper components")]
    public class DirectoryReader : BaseComponent
    {
        public DirectoryReader(ComponentLogger log) : base(log) 
        {
            config = new DirectoryReaderConfig();
            Configuration = config;
        }

        private DirectoryReaderConfig config;

        /// <summary>
        /// Called when the component should do it's actual work.
        /// </summary>
        public override void Compute()
        {
            //validate config
            if (config.Directory == null)
            {
                throw new ComponentException("Directory has not been specified.");
            }
            if (Directory.Exists(config.Directory) == false)
            {
                throw new ComponentException(String.Format("Directory does not exist '{0}'.", config.Directory.Absolute));
            }

            string[] files;
            if (String.IsNullOrEmpty(config.SearchPattern) == true)
            {
                files = Directory.GetFiles(config.Directory, "*", TranslateSearchOption(config.SearchOption));
            }
            else
            {
                files = Directory.GetFiles(config.Directory, config.SearchPattern, TranslateSearchOption(config.SearchOption));
            }

            StringList listOfFiles = new StringList();
            listOfFiles.AddRange(files);

            Workspace.Store("files", listOfFiles);
            Workspace.Store("numberOfFiles", listOfFiles.Count);
            Logger.Trace(String.Format("Found {0} files in the given directory that match given search pattern.", listOfFiles.Count));
        }

        public static System.IO.SearchOption TranslateSearchOption(DirectoryReaderSearchOption value)
        {
            switch (value)
            {
                case DirectoryReaderSearchOption.TopDirectoryOnly:
                    return SearchOption.TopDirectoryOnly;
                    
                case DirectoryReaderSearchOption.AllDirectories:
                    return SearchOption.AllDirectories;

                default: return SearchOption.TopDirectoryOnly;
            }
        }
    }

    [Serializable]
    public class DirectoryReaderConfig
    {
        public DirectoryReaderConfig() { }

        public DirectoryPath Directory
        {
            get;
            set;
        }

        public string SearchPattern
        {
            get;
            set;
        }

        public DirectoryReaderSearchOption SearchOption
        {
            get;
            set;
        }
    }

    public enum DirectoryReaderSearchOption
    {
        // Summary:
        //     Includes only the current directory in a search.
        TopDirectoryOnly = 0,
        //
        // Summary:
        //     Includes the current directory and all the subdirectories in a search operation.
        //     This option includes reparse points like mounted drives and symbolic links
        //     in the search.
        AllDirectories = 1,
    }
}