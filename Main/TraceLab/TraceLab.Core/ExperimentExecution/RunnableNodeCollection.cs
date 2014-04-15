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
using System.Collections;
using System.Collections.ObjectModel;

namespace TraceLab.Core.ExperimentExecution
{
    [Serializable]
    public class RunnableNodeCollection : KeyedCollection<string, IRunnableNode>
    {
        public RunnableNodeCollection() : base() { }

        public IRunnableNode FindById(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (Contains(id))
                return this[id];

            return null;
        }

        public void AddRange(IEnumerable<IRunnableNode> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (IRunnableNode node in collection)
            {
                Add(node);
            }
        }

        protected override string GetKeyForItem(IRunnableNode item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return item.Id;
        }
    }

}
