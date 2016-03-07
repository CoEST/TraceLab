using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;

namespace TraceLab.Core.PackageSystem
{
    public static class PackagesViewModelHelper
    {
        #region Remove Reference

        public static void RemoveReference(IExperiment experiment, PackageReference packageReference)
        {
            if (experiment.References.Contains(packageReference))
            {
                experiment.References.Remove(packageReference);

                RemoveReferenceFromScopes(experiment, packageReference);
            }
        }

        private static void RemoveReferenceFromScopes(IExperiment experiment, PackageReference packageReference)
        {
            foreach (ExperimentNode node in experiment.Vertices)
            {
                ScopeNodeBase scopeNode = node as ScopeNodeBase;
                if (scopeNode != null)
                {
                    var subgraph = scopeNode.CompositeComponentMetadata.ComponentGraph;
                    if (subgraph.References.Contains(packageReference))
                    {
                        subgraph.References.Remove(packageReference);
                    }
                }
            }
        }

        #endregion Remove Reference

        #region Add Reference

        public static void AddReference(IExperiment experiment, PackageReference packageReference)
        {
            if (!experiment.References.Contains(packageReference))
            {
                experiment.References.Add(packageReference);

                AddReferenceToScopes(experiment, packageReference);
            }
        }

        private static void AddReferenceToScopes(IExperiment experiment, PackageReference packageReference)
        {
            foreach (ExperimentNode node in experiment.Vertices)
            {
                ScopeNodeBase scopeNode = node as ScopeNodeBase;
                if (scopeNode != null)
                {
                    var subgraph = scopeNode.CompositeComponentMetadata.ComponentGraph;
                    if (!subgraph.References.Contains(packageReference))
                    {
                        subgraph.References.Add(packageReference);
                    }
                }
            }
        }

        #endregion Add Reference
    }
}
