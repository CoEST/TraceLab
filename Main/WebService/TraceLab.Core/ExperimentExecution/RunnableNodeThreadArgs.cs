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
using System.Threading;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents the argument for the node thread creation. 
    /// </summary>
    public struct RunnableNodeThreadArgs
    {
        private IExperimentRunner m_experimentRunner;
        internal IExperimentRunner ExperimentRunner
        {
            get { return m_experimentRunner; }
            set { m_experimentRunner = value; }
        }

        private RunnableNode m_node;
        internal RunnableNode Node
        {
            get { return m_node; }
            set { m_node = value; }
        }

        public override int GetHashCode()
        {
            return ExperimentRunner.GetHashCode() ^ Node.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            if (obj is RunnableNodeThreadArgs)
                return Equals((RunnableNodeThreadArgs)obj);

            return base.Equals(obj);
        }

        public bool Equals(RunnableNodeThreadArgs otherArgs)
        {
            if (otherArgs == null)
                throw new ArgumentNullException("otherArgs");

            bool areEqual = ExperimentRunner == otherArgs.ExperimentRunner;
            areEqual &= Node == otherArgs.Node;
            return areEqual;
        }

        public static bool operator ==(RunnableNodeThreadArgs firstArgs, RunnableNodeThreadArgs otherArgs)
        {
            if (firstArgs == null || otherArgs == null)
                return false;

            return firstArgs.Equals(otherArgs);
        }

        public static bool operator !=(RunnableNodeThreadArgs firstArgs, RunnableNodeThreadArgs otherArgs)
        {
            if (firstArgs == null || otherArgs == null)
                return false;

            return !(firstArgs.Equals(otherArgs));
        }
    }
}
