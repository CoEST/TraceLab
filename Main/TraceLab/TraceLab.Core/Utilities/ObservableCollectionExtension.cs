using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace TraceLab.Core.Utilities
{
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Copies the collection. Needed because MONO does not implement constructor of ObservableCollection(IEnumerable<T> collection)
        /// Returns a new instance of the System.Collections.ObjectModel.ObservableCollection<T>
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="originalCollection">The original collection - The collection from which the elements are copied.</param>
        /// <returns></returns>
        public static ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> CopyCollection(this ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> originalCollection)
        {
            ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference> copyCollection;

            if (!TraceLabSDK.RuntimeInfo.IsRunInMono)
            {
                copyCollection = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>(originalCollection);
            }
            else
            {
                // Mono currently has not implemented constructor of ObservableCollection(IEnumerable<T> collection)
                // thus we have to add references manually
                copyCollection = new ObservableCollection<TraceLabSDK.PackageSystem.IPackageReference>();
                foreach (TraceLabSDK.PackageSystem.IPackageReference reference in originalCollection)
                {
                    copyCollection.Add(reference);
                }
            }

            return copyCollection;
        }
    }
}
