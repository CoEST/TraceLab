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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TraceLab.Core.Components
{
    /// <summary>
    /// Static helper functions for the Components Library Cache
    /// </summary>
    class ComponentsLibraryCacheHelper
    {
        /// <summary>
        /// Default location of the components library cache (AppData folder)
        /// </summary>
        private static string s_defaultCacheLocation = string.Empty;

        #region Methods

        static ComponentsLibraryCacheHelper()
        {
            s_defaultCacheLocation = System.IO.Path.Combine(TraceLab.Core.Settings.ApplicationSettings.AppDataDirectory, "ComponentsLibrary.cache");
        }

        /// <summary>
        /// Checks if the cache file exists in its default location.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if file exists; otherwise, <c>false</c>.
        /// </returns>
        public static bool CacheFileExists()
        {
            return File.Exists(s_defaultCacheLocation);
        }

        /// <summary>
        /// Deletes cache file from its default location.
        /// </summary>
        public static void DeleteCacheFile()
        {
            if (File.Exists(s_defaultCacheLocation))
            {
                File.Delete(s_defaultCacheLocation);
            }
        }

        /// <summary>
        /// Tries loading the cache from a binary file. If reading fails, an empty cache is returned instead.
        /// </summary>
        /// <returns>
        /// The components library cache.
        /// </returns>
        public static ComponentsLibraryCache LoadCacheFile()
        {
            ComponentsLibraryCache cache = null;
            try
            {
                using (FileStream fs = File.OpenRead(s_defaultCacheLocation))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    cache = (ComponentsLibraryCache)formatter.Deserialize(fs);
                }
            }
            catch (Exception)   // Cache file probably corrupted
            {
                DeleteCacheFile();
                cache = null;
            }
            return cache;
        }

        /// <summary>
        /// Stores the cache in a binary file.
        /// </summary>
        /// <param name="cache">The library cache to be stored.</param>
        public static void StoreCacheFile(ComponentsLibraryCache cache)
        {
            cache.WasModified = false;
            try
            {
#if DEBUG
                Console.WriteLine("Storing components library cache file to " + s_defaultCacheLocation);
#endif
                using (FileStream fs = File.Create(s_defaultCacheLocation))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, cache);
                }
            }
            catch (Exception)
            {
                cache.Clear();
            }
        }

        #endregion Methods
    }
}
