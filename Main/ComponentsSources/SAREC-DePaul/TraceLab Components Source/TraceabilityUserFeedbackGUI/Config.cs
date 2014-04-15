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
using System.ComponentModel;
using TraceLabSDK.Component.Config;

namespace TraceabilityUserFeedbackGUI
{
    public class Config
    {
        [DisplayName("Weight Threshold")]
        [Description("The minimum value links weights have to have to be displayed (inclusive).")]
        public double WeightsThreshold
        {
            get;
            set;
        }

        [DisplayName("Output save path")]
        [Description("Specify the path to the location where you'd like your work to be saved. You can use the importer to retrive this file afterwords.")]
        public FilePath OutSavePath
        {
            get;
            set;
        }
    }
}
