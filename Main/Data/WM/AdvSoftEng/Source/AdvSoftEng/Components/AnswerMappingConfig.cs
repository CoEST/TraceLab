using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Component.Config;
using System.IO;

namespace AdvSoftEng.Components
{
    class AnswerMappingConfig
    {
        private String _dir;
        public String Directory
        {
            get
            {
                return _dir;
            }
            set
            {
                if (!System.IO.Directory.Exists(value))
                {
                    throw new ArgumentException("Directory does not exist:\n" + value);
                }

                _dir = value;

                if (value[value.Length - 1] != '\\')
                {
                    _dir += "\\";
                }
            }
        }
    }
}
