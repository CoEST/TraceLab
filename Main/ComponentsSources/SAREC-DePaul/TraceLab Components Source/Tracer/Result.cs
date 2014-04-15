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

namespace Tracer
{
    internal sealed class Result : IComparable<Result>
    {
        // Variables
        string _artifactId;
        double _ranking;

        // Constructor - Package Private
        public Result(string artifactId, double ranking)
        {
            // Sets the local variables
            _artifactId = artifactId;
            _ranking = ranking;
        }

        public string ArtifactId
        {
            get
            {
                return _artifactId;
            }
        }
        public double Ranking
        {
            get
            {
                return _ranking;
            }
        }

        // The comparable interface allows an array list of results to be ordered
        public int CompareTo(Result other)
        {
            if (_ranking > other.Ranking)
            {
                return -1;
            }
            else if (_ranking == other.Ranking)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
