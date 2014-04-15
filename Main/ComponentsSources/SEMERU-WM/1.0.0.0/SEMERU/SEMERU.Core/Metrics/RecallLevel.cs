using System;

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

namespace SEMERU.Core.Metrics
{
        public enum RecallLevel
        {
            Recall100,
            Recall90,
            Recall80,
            Recall70,
            Recall60,
            Recall50,
            Recall40,
            Recall30,
            Recall20,
            Recall10,
        }

        public static class RecallLevelUtil
        {
            public static string RecallString(RecallLevel level)
            {
                switch (level)
                {
                    case RecallLevel.Recall10:
                        return "Recall 10%";
                    case RecallLevel.Recall20:
                        return "Recall 20%";
                    case RecallLevel.Recall30:
                        return "Recall 30%";
                    case RecallLevel.Recall40:
                        return "Recall 40%";
                    case RecallLevel.Recall50:
                        return "Recall 50%";
                    case RecallLevel.Recall60:
                        return "Recall 60%";
                    case RecallLevel.Recall70:
                        return "Recall 70%";
                    case RecallLevel.Recall80:
                        return "Recall 80%";
                    case RecallLevel.Recall90:
                        return "Recall 90%";
                    case RecallLevel.Recall100:
                        return "Recall 100%";
                    default:
                        return "Unknown";
                }
            }

            public static string ShortRecallString(RecallLevel level)
            {
                switch (level)
                {
                    case RecallLevel.Recall10:
                        return "R10";
                    case RecallLevel.Recall20:
                        return "R20";
                    case RecallLevel.Recall30:
                        return "R30";
                    case RecallLevel.Recall40:
                        return "R40";
                    case RecallLevel.Recall50:
                        return "R50";
                    case RecallLevel.Recall60:
                        return "R60";
                    case RecallLevel.Recall70:
                        return "R70";
                    case RecallLevel.Recall80:
                        return "R80";
                    case RecallLevel.Recall90:
                        return "R90";
                    case RecallLevel.Recall100:
                        return "R100";
                    default:
                        return "Unknown";
                }
            }

            public static double RecallValue(RecallLevel level)
            {
                switch (level)
                {
                    case RecallLevel.Recall10:
                        return 0.1;
                    case RecallLevel.Recall20:
                        return 0.2;
                    case RecallLevel.Recall30:
                        return 0.3;
                    case RecallLevel.Recall40:
                        return 0.4;
                    case RecallLevel.Recall50:
                        return 0.5;
                    case RecallLevel.Recall60:
                        return 0.6;
                    case RecallLevel.Recall70:
                        return 0.7;
                    case RecallLevel.Recall80:
                        return 0.8;
                    case RecallLevel.Recall90:
                        return 0.9;
                    case RecallLevel.Recall100:
                        return 1.0;
                    default:
                        return 0.0;
                }
            }
        }
}
