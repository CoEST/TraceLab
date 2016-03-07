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
using System.Reflection;
using TraceLab.Core.Exceptions;
using TraceLabSDK;
using TraceLab.Core.Components;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// DecisionLoader is used to create instances of decision module classes for Decisions and Loops (classes that has been dynamically compiled at runtime)
    /// DecisionLoader loads assembly and then create the Decision Module class.
    /// It is used this way to make sure that instances are created in components app domain for the experiment execution.
    /// </summary>
    class DecisionLoader : MarshalByRefObject
    {
        private string m_classname;
        private string m_sourceAssembly;
        private IWorkspace m_workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionLoader"/> class.
        /// 
        /// Note that DecisionLoader is created using reflection by DecisionModuleFactory
        /// </summary>
        /// <param name="classname">The classname.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="workspace">The workspace.</param>
        public DecisionLoader(string classname, string sourceAssembly, IWorkspace workspace)
        {
            m_classname = classname;
            m_sourceAssembly = sourceAssembly;
            m_workspace = workspace;
        }

        /// <summary>
        /// Gets the loaded decision module.
        /// </summary>
        internal object LoadedDecisionModule
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads, or actually creates instance of the given classname from the source assembly.
        /// </summary>
        public void Load()
        {
            if (LoadedDecisionModule == null)
            {
                Assembly tlcompAssembly = Assembly.LoadFrom(m_sourceAssembly);
                Type myLoadClass = tlcompAssembly.GetType(m_classname); 

                if (myLoadClass == null)
                {
                    throw new ComponentsLibraryException("Component class " + m_classname + " could not be loaded.");
                }

                LoadedDecisionModule = Activator.CreateInstance(myLoadClass, new Object[] { m_workspace });
            }
        }
    }
}
