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
//
using System;
using System.Collections.Generic;
using TraceLab.Core.Components;
using TraceLab.Core.ViewModels;
using TraceLabSDK.PackageSystem;

namespace TraceLab.UI.GTK
{
    public class NodesTreeBuilder
    {
        public static ComponentsLibraryNode Build(ComponentLibraryViewModel viewModel)
        {
            ComponentsLibraryNode root = new ComponentsLibraryNode(String.Empty);

            // package references
            ComponentsLibraryNode packageReferencesNode = new ComponentsLibraryNode("Package References");
            root.AddChild(packageReferencesNode);

            if (viewModel.Experiment != null)
            {
                foreach (IPackageReference reference in viewModel.Experiment.References)
                {
                    ComponentsLibraryNode referenceNode = new ComponentsLibraryNode(reference.Name);
                    packageReferencesNode.AddChild(referenceNode);
                }
            }

            // components
            ComponentsLibraryNode allComponentsNode = new ComponentsLibraryNode("All Components");
            root.AddChild(allComponentsNode);
            
            foreach (MetadataDefinition component in viewModel.ComponentsCollection)
            {
                ComponentsLibraryNode componentNode = new ComponentsLibraryNode(component.Label, component);
                allComponentsNode.AddChild(componentNode);
                
                foreach (string qualifiedTag in component.Tags.Values)
                {
                    string[] tags = qualifiedTag.Split(new char[] {'.'});
                    if (tags.Length > 0)
                    {
                        ComponentsLibraryNode node = root;
                        
                        foreach (string tag in tags)
                        {
                            ComponentsLibraryNode childNode = node.GetChild(tag);
                            if (childNode == null)
                            {
                                childNode = new ComponentsLibraryNode(tag);
                                node.AddChild(childNode);
                            }
                            node = childNode;
                        }
                        node.AddChild(componentNode);
                    }
                }
            }
            return root;
        }
    }
}

