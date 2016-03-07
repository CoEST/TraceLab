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

namespace TraceLab.Core.Utilities
{
    public static class CollectionsHelper
    {
        /// <summary>
        /// Method evaluates if content of the two dictionaries are the same.
        /// Note, it does not take into consideration, the order of elements, ie. for example if one IDictionary is a SortedDictionary, and other not, 
        /// the method may return true, despite different order of items.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="d1">The d1.</param>
        /// <param name="d2">The d2.</param>
        /// <returns></returns>
        public static bool DictionaryContentEquals<K, V>(IDictionary<K, V> d1, IDictionary<K, V> d2)
        {
            if (d1.Count != d2.Count)
                return false;

            foreach (KeyValuePair<K, V> pair in d1)
            {
                if (!d2.ContainsKey(pair.Key))
                    return false;

                if (!Equals(d2[pair.Key], pair.Value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Merges two dictionaries
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TVal">The type of the val.</typeparam>
        /// <param name="dictA">The dict A.</param>
        /// <param name="dictB">The dict B.</param>
        /// <param name="checkForCollisions">if set to <c>true</c> it checks for collisions, and does not fail in case of duplicate keys</param>
        /// <returns></returns>
        public static void AddRange<TKey, TVal>(this IDictionary<TKey, TVal> dictA, IDictionary<TKey, TVal> dictB, bool checkForCollisions)
        {
            foreach (KeyValuePair<TKey, TVal> pair in dictB)
            {
                if (checkForCollisions)
                {
                    if (dictA.ContainsKey(pair.Key) == false)
                    {
                        dictA.Add(pair.Key, pair.Value);
                    }
                }
                else
                {
                    dictA.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
