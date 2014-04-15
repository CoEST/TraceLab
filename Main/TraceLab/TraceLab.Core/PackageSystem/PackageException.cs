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
using System.Security;
using System.Runtime.Serialization;

namespace TraceLab.Core.PackageSystem
{
    public class PackageException : System.Exception
    {
        // Summary:
        //     Initializes a new instance of the TraceLab.Core.PackageSystem.PackageException class.
        public PackageException() : base()
        {
        }

        public PackageException(string name, string id) : this()
        {
            m_name = name;
            m_id = id;
        }

        //
        // Summary:
        //     Initializes a new instance of the TraceLab.Core.PackageSystem.PackageException class with a specified
        //     error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public PackageException(string message): base(message)
        {
        }

        public PackageException(string name, string id, string message) : this(message)
        {
            m_name = name;
            m_id = id;
        }


        //
        // Summary:
        //     Initializes a new instance of the TraceLab.Core.PackageSystem.PackageException class with serialized
        //     data.
        //
        // Parameters:
        //   info:
        //     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        //     object data about the exception being thrown.
        //
        //   context:
        //     The System.Runtime.Serialization.StreamingContext that contains contextual
        //     information about the source or destination.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The info parameter is null.
        //
        //   System.Runtime.Serialization.SerializationException:
        //     The class name is null or TraceLab.Core.PackageSystem.PackageException.HResult is zero (0).
        [SecuritySafeCritical]
        protected PackageException(SerializationInfo info, StreamingContext context) : base (info, context)
        {
            m_name = info.GetString("m_name");
            m_id = info.GetString("m_id");
        }

        //
        // Summary:
        //     Initializes a new instance of the TraceLab.Core.PackageSystem.PackageException class with a specified
        //     error message and a reference to the inner exception that is the cause of
        //     this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public PackageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PackageException(string name, string id, string message, Exception innerException) : this(message, innerException)
        {
            m_name = name;
            m_id = id;
        }

        private string m_id;
        public string ID
        {
            get { return m_id; }
            private set { m_id = value; }
        }

        private string m_name= string.Empty;
        public string Name
        {
            get { return m_name; }
            private set { m_name = value; }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("m_name", m_name);
            info.AddValue("m_id", m_id);
        }
    }
}
