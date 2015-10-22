using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TraceLab.Core.Exceptions
{
    public class PackageCreationFailureException : InvalidOperationException
    {
        public PackageCreationFailureException() : base() { }
        public PackageCreationFailureException(String message) : base(message) {}
        public PackageCreationFailureException(String message, Exception innerException) : base(message, innerException) { }
        protected PackageCreationFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
