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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.CSharp;
using TraceLabSDK;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Decision module compilator is the class responsible for actual code compilation.
    /// It uses CSharp compiler to compile the given code into the given assembly.
    /// </summary>
    class DecisionModuleCompilator : MarshalByRefObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionModuleCompilator"/> class.
        /// 
        /// Note, that DecisionModuleCompilator is constructed via reflection in DecisionCompilationRunner.
        /// It is constructed via reflection so that compiled assembly is loaded into another app domain that can be unloaed and not kept in memory
        /// </summary>
        public DecisionModuleCompilator() { }

        /// <summary>
        /// Compiles given code into the output assembly.
        /// Requires list of assemblies to be referenced by the compiler.
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="outputAssembly">the name of the output assembly</param>
        /// <param name="assembliesReferenceLocations">The assemblies reference locations - list of assemblies to be referenced by the compiler.</param>
        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public void CompileDecisionModule(string code, string outputAssembly, IEnumerable<string> assembliesReferenceLocations)
        {
            // Create the C# compiler
            CSharpCodeProvider csCompiler = new CSharpCodeProvider();

            CompilerParameters compilerParams = new CompilerParameters();

            compilerParams.OutputAssembly = outputAssembly;
            compilerParams.GenerateExecutable = false;

            compilerParams.GenerateInMemory = true;
            compilerParams.IncludeDebugInformation = false;
            compilerParams.TempFiles.KeepFiles = false;
#if DEBUG
            if (!RuntimeInfo.IsRunInMono)
            {
                //in debug mode the temporary files that are compiled are being kept in temp folder
                compilerParams.TempFiles.KeepFiles = true;
                compilerParams.GenerateInMemory = false;
            }
#endif
            compilerParams.WarningLevel = 4;
            compilerParams.TreatWarningsAsErrors = true;

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParams.ReferencedAssemblies.Add(typeof(TraceLabSDK.IComponent).Assembly.Location);
            compilerParams.ReferencedAssemblies.Add(typeof(DecisionModuleCompilator).Assembly.Location);

            //add references to assemblies - the assemblies should include all types assemblies, as the user code may access some classes from these assemblies
            foreach (string reference in assembliesReferenceLocations)
            {
                compilerParams.ReferencedAssemblies.Add(reference);
            }

            //there are two attempts to compile the code, as in first attempt the output assembly may not be accessible if another or same process accesses it
            //often it is caused just because the lock to assembly has not yet been released by TraceLab itself - domain has not yet been unloaded
            bool repeat;
            bool firstAttempt = true;
            do
            {
                repeat = false;

                //compile assembly in memory
                System.Diagnostics.Debug.WriteLine("Try to compile");
                CompilerResults compilerResults = csCompiler.CompileAssemblyFromSource(compilerParams, new string[] { code });

                //if there are errors while throw exception
                if (compilerResults.Errors.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    foreach (CompilerError error in compilerResults.Errors)
                    {
                        //if error CS0016: Could not write to output file ... -- 'The process cannot access the file because it is being used by another process. '
                        //it happens 
                        if (error.ErrorNumber.Equals("CS0016") && firstAttempt)
                        {
                            //wait a second and reattempt compilatin just one time more
                            repeat = true;
                            System.Threading.Thread.Sleep(500);
                            System.Diagnostics.Debug.WriteLine("waited 500 miliseconds");
                            break;
                        }
                        errorMessage.AppendLine(error.ToString());
                    }

                    if (repeat == false)
                    {
                        throw new ArgumentException("Errors occured in decision code compilation. " + errorMessage.ToString());
                    }

                    firstAttempt = false;
                }
            } while (repeat == true);
        }

    }
}
