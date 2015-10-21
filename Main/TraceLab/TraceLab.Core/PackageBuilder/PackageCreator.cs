using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TraceLab.Core.PackageBuilder
{
    public static class PackageCreator
    {
        #region Build Package

        /// <summary>
        /// Adds item and its content (file or folder) to package.
        /// </summary>
        /// <param name="pPkg">Package being created.</param>
        /// <param name="item">Item to be added.</param>
        /// <returns>True is there was no error during the operation, false otherwise.</returns>
        public static bool AddItemToPackage(TraceLab.Core.PackageSystem.Package pkg, PackageFileSourceInfo item, bool isExperimentPackage)
        {
            bool noError = true;

            string targetPath = System.IO.Path.Combine(pkg.Location, item.GetPath());

            PackageHeirarchyItem dir = item as PackageHeirarchyItem;
            if (dir != null)
            {
                if (item.Parent != null)
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                foreach (PackageFileSourceInfo child in dir.Children)
                {
                    noError = noError && AddItemToPackage(pkg, child, isExperimentPackage);
                }

                if (dir.HasComponents)
                {
                    pkg.SetDirectoryHasComponents(dir.GetPath(), true);
                }
                if (dir.HasTypes)
                {
                    pkg.SetDirectoryHasTypes(dir.GetPath(), true);
                }
            }
            else
            {
                System.IO.File.Copy(item.SourceFilePath, targetPath);
                //Add reference to this created package to all experiments and composite components
                if (isExperimentPackage && targetPath.EndsWith(".teml") || targetPath.EndsWith(".tcml"))
                {
                    noError = noError && AddPkgRefToExperiment(pkg, targetPath);
                }
                System.IO.File.SetAttributes(targetPath, System.IO.File.GetAttributes(targetPath) & ~System.IO.FileAttributes.ReadOnly);
                pkg.AddFile(targetPath);
            }

            return noError;
        }


        /// <summary>
        /// Modifies experiment to add reference to new package.
        /// </summary>
        /// <param name="pPkg">The package being created.</param>
        /// <param name="pExperimentFile">The experiment file.</param>
        /// <returns>True is there was no error during the operation, false otherwise.</returns>
        private static bool AddPkgRefToExperiment(TraceLab.Core.PackageSystem.Package pPkg, string pExperimentFile)
        {
            bool noError = true;

            if (System.IO.File.Exists(pExperimentFile))
            {
                try
                {
                    XmlDocument xmlExperiment = new XmlDocument();
                    xmlExperiment.Load(pExperimentFile);

                    XmlNode nodeReferences = xmlExperiment.SelectSingleNode("//References");

                    XmlElement newPkgReference = xmlExperiment.CreateElement("PackageReference");
                    newPkgReference.SetAttribute("ID", pPkg.ID);
                    newPkgReference.SetAttribute("Name", pPkg.Name);
                    nodeReferences.AppendChild(newPkgReference);

                    xmlExperiment.Save(pExperimentFile);
                }
                catch (Exception)
                {
                    noError = false;
                    throw new TraceLab.Core.Exceptions.PackageCreationFailureException("Unable to modify experiment - reference to new package could not be added.");
                }
            }
            else
            {
                noError = false;
            }
            return noError;
        }

        #endregion
    }
}
