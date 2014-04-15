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
using System.Runtime.Serialization;
using System.Security;

namespace TraceLab.Core.PackageSystem
{
    public class PackageAlreadyExistsException : PackageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        public PackageAlreadyExistsException(string name, string id)
            : base(name, id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        /// <param name="message">The message.</param>
        public PackageAlreadyExistsException(string name, string id, string message)
            : base(name, id, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PackageAlreadyExistsException(string name, string id, string message, Exception innerException)
            : base(name, id, message, innerException)
        {
        }
    }
}
