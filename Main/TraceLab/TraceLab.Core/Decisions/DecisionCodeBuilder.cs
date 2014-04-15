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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using TraceLab.Core.Components;
using TraceLabSDK;

namespace TraceLab.Core.Decisions
{
    /// <summary>
    /// Code builder is responsible for constructing the final class of the decision modules to be compiled by DecisionModuleCompilator
    /// </summary>
    static class DecisionCodeBuilder
    {
        /// <summary>
        /// Builds the code source.
        /// Depending on metadata type final class will implement different interfaces and thus will have different class body.
        /// 1. metadata is a DecisionMetadata - the final source class is going to implement IDecisionModule interface,
        /// 2. metadata is a LoopScopeMetadata - the final source class is going to implement ILoopDecisionModule interface
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="typeDirectories">The type directories.</param>
        /// <param name="successorNodeLabelIdLookup">The successor node label id lookup.</param>
        /// <param name="predeccessorsOutputsNameTypeLookup">The predeccessors outputs name type lookup.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="assembliesReferenceLocations">Output the list of assemblies locations that must be reference by DecisionCompilator</param>
        /// <returns></returns>
        public static string BuildCodeSource(IDecision metadata, List<string> typeDirectories, Dictionary<string, string> successorNodeLabelIdLookup,
                                          Dictionary<string, string> predeccessorsOutputsNameTypeLookup, ComponentLogger logger, out HashSet<string> assembliesReferenceLocations)
        {   
            string bodyCode, moduleInterface;

            if (metadata is DecisionMetadata)
            {
                //parse the code written by user
                DecisionCodeParser codeParser = new DecisionCodeParser(metadata.DecisionCode, successorNodeLabelIdLookup, predeccessorsOutputsNameTypeLookup);
                string userDecisionCodeSnippet = codeParser.ParseCode();
                bodyCode = String.Format(DECISION_MODULE_BODY, userDecisionCodeSnippet);
                moduleInterface = typeof(IDecisionModule).FullName;
            }
            else
            {
                //append ';' to the end just in case so that user condition can be simplified
                string decisionCode = (metadata.DecisionCode.EndsWith(";")) ? metadata.DecisionCode : metadata.DecisionCode + ";";

                DecisionCodeParser codeParser = new DecisionCodeParser(decisionCode, successorNodeLabelIdLookup, predeccessorsOutputsNameTypeLookup);
                string userDecisionCodeSnippet = codeParser.ParseCode();
                bodyCode = String.Format(LOOP_DECISION_MODULE_BODY, userDecisionCodeSnippet);
                moduleInterface = typeof(ILoopDecisionModule).FullName;
            }

            //build the string of all usings of all namespace found in types assemblies
            string usingsNamespacesCodeSnippet = ReferenceTypesAssemblies(typeDirectories, logger, out assembliesReferenceLocations);

            return BuildDecisionCodeSource(metadata.Classname, moduleInterface, bodyCode, usingsNamespacesCodeSnippet);
        }

        /// <summary>
        /// Builds the decision final code source of the decision module class.
        /// 
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="decisionModuleInterface">The decision module interface.</param>
        /// <param name="bodyCode">The body code.</param>
        /// <param name="usingsNamespacesCodeSnippet">The usings namespaces code snippet.</param>
        /// <returns></returns>
        private static string BuildDecisionCodeSource(string classname, string decisionModuleInterface, string bodyCode, string usingsNamespacesCodeSnippet)
        {
            //Prepare the code to compile
            StringBuilder codeBuilder = new StringBuilder();

            //format the decision code by specyfying all parameters: using types, classname, and decision code
            codeBuilder.Append(String.Format(System.Globalization.CultureInfo.CurrentCulture, COMMON_DECISION_CLASS_CODE,
                                             usingsNamespacesCodeSnippet,
                                             classname,
                                             decisionModuleInterface,
                                             classname,
                                             bodyCode));

            return codeBuilder.ToString();
        }
        
        #region Code Snippets
        
        /// <summary>
        /// The main code snippet for the module class. It is used for both modules for Decision and Loop.
        /// 
        /// There are four parameters needed in the code
        /// {0} - usings for all namespaces that class may potentially need to use - it should have namespaces found in the Types assemblies
        /// {1} - classname 
        /// {2} - interface that class implements - in case of DecisionMetadata it is IDecisionModule, in case of LoopScopeMetadata it is ILoopDecisionModule
        /// {3} - constructor name - has to be the same as classname obviously
        /// {4} - body, that must implement all required methods for the specified interface
        /// </summary>
        private const string COMMON_DECISION_CLASS_CODE = @"
                {0}

                class {1} : MarshalByRefObject, {2}
                {{
                    private IWorkspace m_workspace;
                    private IWorkspace Workspace {{ get {{ return m_workspace; }} }} 

                    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
                    public override object InitializeLifetimeService()
                    {{
                        return null;
                    }}

                    public {3} (IWorkspace workspace) 
                    {{
                        m_workspace = workspace; 
                    }}
                    
                    public object Load(string unitname) {{
                        return Workspace.Load(unitname);
                    }}

                    {4}
                }}
            ";

        /// <summary>
        /// This code snippet represents the body for the class that implements IDecisionModule 
        /// It is used in case of DecisionMetadata
        /// </summary>
        private const string DECISION_MODULE_BODY = @"
                private RunnableNodeCollection CandidatePaths;
                private RunnableNodeCollection SelectedPaths;

                public RunnableNodeCollection Decide(RunnableNodeCollection candidatePaths)
                {{
                    CandidatePaths = candidatePaths;
                    SelectedPaths = new RunnableNodeCollection();

                    {0}

                    return SelectedPaths;
                }}

                public void Select(string nodeId) {{
                    IRunnableNode selectedNode = CandidatePaths.FindById(nodeId);
                        
                    if(selectedNode == null) {{
                        throw new ArgumentException(""Node does not exists."");
                    }}

                    SelectedPaths.Add(selectedNode);
                }}
        ";

        /// <summary>
        /// This code snippet represents the body for the class that implements ILoopDecisionModule
        /// It is used in case of LoopScopeMetadata
        /// </summary>
        private const string LOOP_DECISION_MODULE_BODY = @"
                
                public bool Condition()
                {{
                    return {0};
                }}
        ";

        #endregion Code Snippets

        #region Namespace & References Locator

        /// <summary>
        /// Methods searches for all assemblies in the given type directory, and adds references to these assemblies in the compiler parameters.
        /// It also looks for all namespaces in all found asseblies and returns in in the format
        /// using typenamespace;
        /// using typenamespace2;
        /// ...
        /// </summary>
        /// <param name="typeDirectories">directory to be search for assemblies</param>
        /// <param name="logger">The logger.</param>
        /// <param name="assembliesReferenceLocations">Outputs the assemblies locations that must be referenced by DecisionCompilator.</param>
        /// <returns>
        /// all namespaces found in the assemblies, formatted as described above
        /// </returns>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static string ReferenceTypesAssemblies(List<string> typeDirectories, TraceLabSDK.ComponentLogger logger, out HashSet<string> assembliesReferenceLocations)
        {
            string usingKeyword = "using ";
            string semicolon = ";";

            Dictionary<string, System.Reflection.Assembly> assemblies = new Dictionary<string, System.Reflection.Assembly>();
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = assembly.GetName();
                assemblies[name.Name + System.Text.UTF8Encoding.UTF8.GetString(name.GetPublicKeyToken())] = assembly;
            }

            assembliesReferenceLocations = new HashSet<string>();
            HashSet<string> namespaces = new HashSet<string>();
            StringBuilder typesNamespaces = new StringBuilder();

            //preadd fixed namespaces
            namespaces.Add("System");
            namespaces.Add("System.Security.Permissions");
            namespaces.Add("TraceLab.Core.ExperimentExecution");
            namespaces.Add("TraceLab.Core.Experiments");
            namespaces.Add("TraceLab.Core.Decisions");
            namespaces.Add("TraceLabSDK");
            foreach (string fixedNamespace in namespaces)
            {
                typesNamespaces.AppendLine(usingKeyword + fixedNamespace + semicolon);
            }

            //iterate through all type directories, search for all dll, and collect the namespaces of all found types in those assemblies
            foreach (string dir in typeDirectories)
            {
                string typesDir = System.IO.Path.GetFullPath(dir);

                var files = System.IO.Directory.GetFiles(typesDir, "*.dll");
                foreach (string file in files)
                {
                    var name = System.Reflection.AssemblyName.GetAssemblyName(file);
                    var lookup = name.Name + System.Text.UTF8Encoding.UTF8.GetString(name.GetPublicKeyToken());
                    if (assemblies.ContainsKey(lookup))
                    {
                        var assembly = assemblies[lookup];

                        try
                        {
                            Type[] typesInAssembly = assembly.GetTypes();

                            foreach (Type type in typesInAssembly)
                            {
                                if (namespaces.Contains(type.Namespace) == false && type.Namespace != String.Empty && type.Namespace != null)
                                {
                                    namespaces.Add(type.Namespace);
                                    typesNamespaces.AppendLine(usingKeyword + NamespaceFix(type.Namespace) + semicolon);
                                }
                            }

                            //collect also assemblies location - the assemblies must be added to the compiler parameters
                            assembliesReferenceLocations.Add(assembly.Location);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            if (logger != null)
                            {
                                //log warnings
                                logger.Warn(String.Format("Assembly {0} has been skipped in decision node compilation, because compiler was unable to load one or more of the requested types when compiling decision node. " +
                                                                             "See the Loader Exceptions for more information.", name), ex);
                                int i = 1;
                                foreach (Exception loaderException in ex.LoaderExceptions)
                                {
                                    logger.Warn(String.Format("Loader exception {0}: {1}", i++, loaderException.Message));
                                }
                            }
                        }

                    }
                }
            }

            return typesNamespaces.ToString();
        }

        /// <summary>
        /// Fixes the namespaces by adding @ in front of the parts of namespace that equal any of the .Net keyword
        /// The problem is that java allows creating namespaces, that in .Net would not be valid. Thus IKVMC compiled stubs can have 
        /// namespaced including net keyword. These has to be processed.
        /// For example, 
        /// using my.space.fixed;
        /// would not compile, as fixed is a keyword.
        /// It can be fixed in following way
        /// using my.space.@fixed;
        /// </summary>
        private static string NamespaceFix(string @namespace)
        {
            List<string> dotNetKeywords = new List<string>() {
                "abstract","as","base","bool","break","byte","case","catch","char","checked",
                "class","const","continue","decimal","default","delegate","do","double","else",
                "enum","event","explicit","extern","false","finally","fixed","float","for","foreach",
                "goto","if","implicit","in","in (generic modifier)","int","interface","internal","is",
                "lock","long","namespace","new","null","object","operator","out","out (generic modifier)",
                "override","params","private","protected","public","readonly","ref","return","sbyte","sealed",
                "short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try",
                "typeof","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while"
            };

            var namespaceParts = @namespace.Split(new[] { '.' });
            var hasAny = namespaceParts.Any(dotNetKeywords.Contains);
            if (hasAny)
            {
                StringBuilder fixedNamespace = new StringBuilder();

                //rebuild the namespace by adding @ to keyword parts - it is so rare case, so it doesn't have to be efficient
                foreach (string part in namespaceParts)
                {
                    if (dotNetKeywords.Contains(part))
                        fixedNamespace.Append("@" + part);
                    else
                        fixedNamespace.Append(part);

                    fixedNamespace.Append(".");
                }

                fixedNamespace.Remove(fixedNamespace.Length - 1, 1);

                @namespace = fixedNamespace.ToString();
            }

            return @namespace;
        }

        #endregion
    }
}
