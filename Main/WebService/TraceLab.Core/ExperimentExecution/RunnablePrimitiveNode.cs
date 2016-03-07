using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraceLab.Core.ExperimentExecution
{
    /// <summary>
    /// Represents START node in the template graph.
    /// </summary>
    [Serializable]
    internal class RunnablePrimitiveNode : RunnableNode
    {
        public RunnablePrimitiveNode(String id, bool waitForAllPredecessors) : base(id, id, new RunnableNodeCollection(), new RunnableNodeCollection(), null, waitForAllPredecessors) { }

        public override void RunInternal()
        {
            System.Diagnostics.Trace.WriteLine(Id);
        }
    }
}
