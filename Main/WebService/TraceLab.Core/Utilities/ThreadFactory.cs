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

using System.Globalization;
using System.Threading;

namespace TraceLab.Core.Utilities
{
    /// <summary>
    /// Static class (Factory pattern) for creating threads in TraceLab that uses a standard 
    /// CultureInfo (U.S. English) for avioding problems with parsing inputs/outputs.
    /// </summary>
    public static class ThreadFactory
    {
        /// <summary>
        /// Default CultureInfo (U.S. English) used for creating threads in TraceLab.
        /// </summary>
        private static CultureInfo defaultCulture = new CultureInfo("en-US");
        public static CultureInfo DefaultCulture
        {
            get { return defaultCulture; }
        }

        /// <summary>
        /// Creates new thread using the default CultureInfo and running the given parameterless function.
        /// </summary>
        /// <param name="pDelegateFunction">The delegate function to be run by the thread.</param>
        /// <returns></returns>
        public static Thread CreateThread(ThreadStart pDelegateFunction)
        {
            Thread thread = new Thread(pDelegateFunction);
            thread.CurrentCulture = defaultCulture;
            return thread;
        }

        /// <summary>
        /// Creates new thread using the default CultureInfo and running the given parameterized function.
        /// </summary>
        /// <param name="pDelegateFunction">The delegate function to be run by the thread.</param>
        /// <returns></returns>
        public static Thread CreateThread(ParameterizedThreadStart pDelegateFunction)
        {
            Thread thread = new Thread(pDelegateFunction);
            thread.CurrentCulture = defaultCulture;
            return thread;
        }
    }
}
