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
using System.Text;
using TraceLabSDK;
using System.Reflection;
using TraceLab.Core.Utilities;
using TraceLab.Core.Exceptions;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Set of helper methods for the component scanner.
    /// It includes methods for creation configuration wrapper, reading io specification based on component attributes,
    /// as well as creating component id based on its name, io spec, version, and configuration parameters.
    /// </summary>
    internal static class ComponentScannerHelper
    {
        /// <summary>
        /// Creates the ID for the component based on its name, io spec, version, and configuration parameters.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="componentIOSpec">The component IO spec.</param>
        /// <param name="version">The version.</param>
        /// <param name="componentConfiguration">The component configuration.</param>
        /// <returns>Id that represents the component</returns>
        public static string CreateComponentId(string componentName, IOSpecDefinition componentIOSpec, string version, ConfigWrapperDefinition componentConfiguration)
        {
            if (componentName == null)
                throw new Exceptions.ComponentsLibraryException("Component name is required to generate component id.");

            StringBuilder componentInfo = new StringBuilder();
            
            //changes in name and version are included in guid creation
            componentInfo.Append(componentName);
            componentInfo.Append(version);

            //changes in iospec are included in guid creation
            if (componentIOSpec != null)
            {
                var inputlist = new List<string>();
                foreach (KeyValuePair<string, IOItemDefinition> pair in componentIOSpec.Input)
                {
                    inputlist.Add(pair.Key + pair.Value.Type);
                }
                inputlist.Sort();

                var outputlist = new List<string>();
                foreach (KeyValuePair<string, IOItemDefinition> pair in componentIOSpec.Output)
                {
                    outputlist.Add(pair.Key + pair.Value.Type);
                }
                outputlist.Sort();

                foreach(string itemString in inputlist) 
                {
                    componentInfo.Append(itemString);
                }
                foreach (string itemString in outputlist)
                {
                    componentInfo.Append(itemString);
                }
            }

            //changes in configuration are included in guid creation
            if (componentConfiguration != null)
            {
                var configList = new List<string>();
                foreach (KeyValuePair<string, ConfigPropertyObject> pair in componentConfiguration.Properties)
                {
                    configList.Add(pair.Key + pair.Value.Type);
                }
                configList.Sort();

                componentInfo.Append(componentConfiguration.ConfigurationTypeFullName);

                foreach (string configItemString in configList)
                {
                    componentInfo.Append(configItemString);
                }
            }

            Guid id = GuidUtility.Create(GuidUtility.TraceLabNamespace, componentInfo.ToString());

            return id.ToString();
        }

        /**
         * Componend if generation without sorting - bug
         * 
         */
        [Obsolete]
        public static string CreateComponentIdNoSortingOld(string componentName, IOSpecDefinition componentIOSpec, string version, ConfigWrapperDefinition componentConfiguration)
        {
            if (componentName == null)
                throw new Exceptions.ComponentsLibraryException("Component name is required to generate component id.");

            StringBuilder componentInfo = new StringBuilder();

            //changes in name and version are included in guid creation
            componentInfo.Append(componentName);
            componentInfo.Append(version);

            //changes in iospec are included in guid creation
            if (componentIOSpec != null)
            {
                foreach(KeyValuePair<string, IOItemDefinition> pair in componentIOSpec.Input) 
                {
                    componentInfo.Append(pair.Key);
                    componentInfo.Append(pair.Value.Type);
                }
                foreach (KeyValuePair<string, IOItemDefinition> pair in componentIOSpec.Output)
                {
                    componentInfo.Append(pair.Key);
                    componentInfo.Append(pair.Value.Type);
                }
            }

            //changes in configuration are included in guid creation
            if (componentConfiguration != null)
            {
                componentInfo.Append(componentConfiguration.ConfigurationTypeFullName);

                foreach (KeyValuePair<string, ConfigPropertyObject> pair in componentConfiguration.Properties)
                {
                    componentInfo.Append(pair.Key);
                    componentInfo.Append(pair.Value.Type);
                }
            }

            Guid id = GuidUtility.Create(GuidUtility.TraceLabNamespace, componentInfo.ToString());

            return id.ToString();
        }

        /// <summary>
        /// This method creates ConfigWrapperDefinition based on the given configType. In case in config files written in C# it will use Properties as Config parameters.
        /// In case of Java it will scan for pairs of getters and setters, and if it will find one it will create a property for it. For example void setTest(string value) and string getTest()
        /// will generate property 'string Test'
        /// </summary>
        /// <param name="configType"></param>
        /// <returns></returns>
        internal static ConfigWrapperDefinition CreateConfigWrapperDefinition(Type configType)
        {
            ConfigWrapperDefinition configWrapperDefinition = null;

            if (configType != null)
            {
                bool isJava = CheckIfJavaType(configType);

                configWrapperDefinition = new ConfigWrapperDefinition(isJava, configType.FullName);

                if (isJava == false)
                {
                    System.ComponentModel.PropertyDescriptorCollection properties = System.ComponentModel.TypeDescriptor.GetProperties(configType);
                    configWrapperDefinition.AddProperties(properties);
                }
                else
                {
                    //check for java if there are any java getters
                    String getterPrefix = "get";
                    String setterPrefix = "set";
                    String getterPropertyPrefix = "get_"; // .net style getter generated for properties

                    MethodInfo[] methods = configType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    foreach (MethodInfo method in methods)
                    {
                        if (method.Name.StartsWith(getterPrefix, StringComparison.InvariantCulture) == true && method.Name.StartsWith(getterPropertyPrefix, StringComparison.InvariantCulture) == false)
                        {
                            String propertyName = method.Name.Substring(getterPrefix.Length);
                            Type propertyType = method.ReturnParameter.ParameterType;
                            //check if there is any corresponding set method for this property
                            if (methods.Any(item => item.Name.Equals(setterPrefix + propertyName, StringComparison.CurrentCulture)))
                            {
                                //check if setter has one input of the same type as the getter return type
                                MethodInfo setter = configType.GetMethod(setterPrefix + propertyName, BindingFlags.Public | BindingFlags.Instance, null, new Type[] { propertyType }, null);
                                if (setter != null)
                                {
                                    //TODO: Description for hava config values
                                    string propertyDescription = propertyName;
                                    //finally add the property
                                    configWrapperDefinition.AddProperty(propertyName, method.ReturnParameter.ParameterType.FullName, method.ReturnParameter.ParameterType.AssemblyQualifiedName, propertyName, propertyDescription);
                                }
                            }
                        }
                    }
                }
            }
            return configWrapperDefinition;
        }

        /// <summary>
        /// Method does recursion until it reaches basetype java.lang.Object, which indicates that config type was a java class, or until it reaches System.Object which indicates
        /// that it was c# object. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckIfJavaType(Type type)
        {
            Type baseType = type.BaseType;

            if (baseType.Equals(typeof(System.Object)))
            {
                return false;
            }
            else if (baseType.FullName.Equals("java.lang.Object", StringComparison.CurrentCulture))
            {
                return true;
            }
            else
            {
                return CheckIfJavaType(baseType);
            }
        }

        /// <summary>
        /// Reads the IO spec attributes and creates iospec definition
        /// </summary>
        /// <param name="componentType">Type of the component.</param>
        /// <exception cref="ComponentsLibraryException">throws components exception if there are two same inputs or outputs of the same name 
        /// added to io spec defnition</exception>
        /// <returns>IOSpecDefinition if there were no errors</returns>
        internal static IOSpecDefinition ReadIOSpec(Type componentType)
        {
            var spec = new IOSpecDefinition();

            object[] inputMapping = componentType.GetCustomAttributes(typeof(IOSpecAttribute), true);
            if (inputMapping.Length != 0)
            {
                foreach (IOSpecAttribute attrib in inputMapping)
                {
                    if (attrib.IOType == IOSpecType.Input)
                    {
                        if (spec.Input.ContainsKey(attrib.Name))
                        {
                            throw new ComponentsLibraryException(String.Format("There cannot be multiple input attributes of the same name '{0}'", attrib.Name));
                        }
                        spec.Input.Add(attrib.Name, new IOItemDefinition(attrib.Name, attrib.DataType.FullName, attrib.Description, IOSpecType.Input));
                    }
                    else if (attrib.IOType == IOSpecType.Output)
                    {
                        if (spec.Output.ContainsKey(attrib.Name))
                        {
                            throw new ComponentsLibraryException(String.Format("There cannot be multiple output attributes of the same name '{0}'", attrib.Name));
                        }
                        spec.Output.Add(attrib.Name, new IOItemDefinition(attrib.Name, attrib.DataType.FullName, attrib.Description, IOSpecType.Output));
                    }
                    else
                    {
                        throw new ComponentsLibraryException("IOSpecType enumeration is invalid");
                    }
                }
            }

            return spec;
        }
    }
}
