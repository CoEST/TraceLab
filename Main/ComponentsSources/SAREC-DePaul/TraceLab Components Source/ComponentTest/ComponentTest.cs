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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLabSDK;
using System.Reflection;

namespace ComponentTest
{
    [TestClass]
    public abstract class ComponentTest
    {
        public BaseComponent TestComponent
        {
            get;
            set;
        }

        public DummyWorkspace Workspace
        {
            get;
            set;
        }

        [TestInitialize]
        public void TestSetup()
        {
            CreateWorkspace();
            CreateImporter(CreateLogger());
            InitImporter();
        }

        protected abstract void CreateImporter(ComponentLogger logger);

        protected ComponentLogger CreateLogger()
        {
            ComponentLogger logger = new ComponentLoggerImplementation("test");
            return logger;
        }

        protected virtual void InitImporter()
        {
            TestComponent.Workspace = Workspace;
        }

        protected virtual void CreateWorkspace()
        {
            Workspace = new DummyWorkspace();
        }
    }
}
