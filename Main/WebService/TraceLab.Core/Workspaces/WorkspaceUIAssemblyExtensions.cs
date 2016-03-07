using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Workspaces;

namespace TraceLab.Core.Workspaces
{
    public class WorkspaceUIAssemblyExtensions
    {
        /// <summary>
        /// Assembly extensions checked by WorkspaceViewerLoader.
        /// For example for the type located in TraceLab.Types.dll 
        /// loader will search assemblies TraceLab.Types.UI.WPF.dll, and TraceLab.Types + DefaultExtension
        /// </summary>
        public static string[] Extensions = new string[] { DEFAULT_EXTENSION };

        public const string DEFAULT_EXTENSION = ".UI.dll";
    }
}
