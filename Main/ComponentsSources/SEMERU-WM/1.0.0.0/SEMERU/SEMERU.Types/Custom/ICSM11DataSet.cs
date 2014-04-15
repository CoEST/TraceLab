using System;
using System.Text;
using SEMERU.Types.Dataset;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Types.Custom
{
    [Serializable]
    public class ICSM11DataSet : DataSet
    {
        public string PrecomputedRTMSimilarities;

        public RTMSettings RTM;
        public PCASettings PCA;

        public override string ToOutputString()
        {
            StringBuilder txt = new StringBuilder();
            txt.Append("DataSet:         ");
            txt.AppendLine(Name);
            txt.Append("SourceArtifacts: ");
            txt.AppendLine(SourceArtifacts);
            txt.Append("TargetArtifacts: ");
            txt.AppendLine(TargetArtifacts);
            txt.Append("Oracle:          ");
            txt.AppendLine(Oracle);
            if (RTM != null)
                txt.Append(RTM.ToOutputString());
            if (PCA != null)
                txt.Append(PCA.ToOutputString());
            if (Metrics != null)
                txt.Append(Metrics.ToOutputString());
            return txt.ToString();
        }
    }

    [Serializable]
    public class RTMSettings
    {
        public double Alpha;
        public double Beta;
        public double Eta;
        public int NumTopics;

        public string ToOutputString()
        {
            StringBuilder txt = new StringBuilder("RTM Settings\n");
            txt.Append("    Alpha:  ");
            txt.AppendLine(Alpha.ToString());
            txt.Append("    Beta:   ");
            txt.AppendLine(Beta.ToString());
            txt.Append("    Eta:    ");
            txt.AppendLine(Eta.ToString());
            txt.Append("    Topics: ");
            txt.AppendLine(NumTopics.ToString());
            return txt.ToString();
        }
    }

    [Serializable]
    public class PCASettings
    {
        public double VSM_JS;
        public double VSM_RTM;
        public double JS_RTM;

        public string ToOutputString()
        {
            StringBuilder txt = new StringBuilder("PCA Settings\n");
            txt.Append("    VSM->JS:  ");
            txt.AppendLine(VSM_JS.ToString());
            txt.Append("    VSM->RTM: ");
            txt.AppendLine(VSM_RTM.ToString());
            txt.Append("    JS->RTM:  ");
            txt.AppendLine(JS_RTM.ToString());
            return txt.ToString();
        }
    }
}
