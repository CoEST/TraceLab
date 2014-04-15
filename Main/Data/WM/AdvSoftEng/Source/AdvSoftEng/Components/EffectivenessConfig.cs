using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Component.Config;

namespace AdvSoftEng.Components
{
    class EffectivenessConfig
    {
        private String _all;
        public String AllMethodsFile
        {
            get
            {
                return _all;
            }
            set
            {
                _all = value;
            }
        }

        private String _best;
        public String BestMethodsFile
        {
            get
            {
                return _best;
            }
            set
            {
                _best = value;
            }
        }
    }
}
