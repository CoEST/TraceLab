using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvSoftEng.Components
{
    class DocumentVectorConfig
    {
        private static String[] ValidReps = {"Boolean", "Ordinal"};

        private String _rep;
        public String Representation
        {
            get
            {
                return _rep;
            }
            set
            {
                if (!ValidReps.Contains(value))
                {
                    throw new ArgumentException("Representation must be one of the following values: " + String.Join(", ", ValidReps));
                }
                _rep = value;
            }
        }
    }
}
