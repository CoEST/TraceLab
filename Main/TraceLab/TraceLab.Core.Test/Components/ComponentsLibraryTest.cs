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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraceLab.Core.Exceptions;
using TraceLab.Core.Test.Setup;
using TraceLab.Core.Workspaces;
using TraceLabSDK;
using System.Collections.Generic;
using TraceLab.Core.Settings;

namespace TraceLab.Core.Components.Test
{
    [Serializable]
    class MockComponentGenerator
    {
        public MockComponentGenerator(string path, string name)
        {
            Path = path;
            Name = name;
            AssemblyPath = System.IO.Path.Combine(Path, Name + ".dll");
        }

        public void Generate()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.ApplicationName = "Generation";
            AppDomain generationDomain = AppDomain.CreateDomain("GenerationDomain", null, setup);
            generationDomain.Load(Assembly.GetExecutingAssembly().GetName());

            generationDomain.DoCallBack(DoGenerate);

            AppDomain.Unload(generationDomain);
        }

        private void DoGenerate()
        {
            GenerateMockAssembly(Name, Path);
        }

        private string Path
        {
            get;
            set;
        }

        private string Name
        {
            get;
            set;
        }

        public string AssemblyPath
        {
            get;
            private set;
        }

        #region MockComponent generation

        private void GenerateMockAssembly(string name, string path, AppDomain currentDomain)
        {
            string filename = name + ".dll";

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = name;

            // Generate the mock component: 
            AssemblyBuilder builder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save, path);
            ModuleBuilder module = builder.DefineDynamicModule(name, filename, true);
            GenerateMockType(module);

            builder.Save(filename);
        }

        private void GenerateMockAssembly(string name, string path)
        {
            GenerateMockAssembly(name, path, AppDomain.CurrentDomain);
        }

        private void GenerateMockType(ModuleBuilder module)
        {
            TypeBuilder mockTypeBuilder = module.DefineType("MockComponents.MockComponent", 
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit, 
                typeof(BaseComponent));

            //Let us apply the required attribute
            Type[] constructorParameters = new Type[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(Type) };

            // Add the Component attribute to the mock component
            ConstructorInfo constructorInfo = typeof(ComponentAttribute).GetConstructor(constructorParameters);
            CustomAttributeBuilder classLevelAttributeBuilder =
                new CustomAttributeBuilder(constructorInfo, new object[] { Guid.NewGuid().ToString(), "MockComponent", "Empty", "None", "1.0", null });

            mockTypeBuilder.SetCustomAttribute(classLevelAttributeBuilder);

            // Add the Tag attribute (Tag: Mock) to the mock component
            constructorParameters = new Type[] { typeof(string) };
            constructorInfo = typeof(TagAttribute).GetConstructor(constructorParameters);
            classLevelAttributeBuilder = new CustomAttributeBuilder(constructorInfo, new object[] { "Mock" });
            mockTypeBuilder.SetCustomAttribute(classLevelAttributeBuilder);

            // Add the Tag attribute (Tag: DoesNothing) to the mock component
            constructorParameters = new Type[] { typeof(string) };
            constructorInfo = typeof(TagAttribute).GetConstructor(constructorParameters);
            classLevelAttributeBuilder = new CustomAttributeBuilder(constructorInfo, new object[] { "DoesNothing" });
            mockTypeBuilder.SetCustomAttribute(classLevelAttributeBuilder);


            GenerateConstructor(mockTypeBuilder);
            GenerateComputeMethod(mockTypeBuilder);

            mockTypeBuilder.CreateType();
        }

        private void GenerateConstructor(TypeBuilder mockTypeBuilder)
        {
            Type[] componentConstructorParams = new Type[] { typeof(ComponentLogger) };

            ConstructorBuilder constructBuilder = mockTypeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig, CallingConventions.Standard, componentConstructorParams);

            //call constructor of base object
            ConstructorInfo conObj = typeof(BaseComponent).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, componentConstructorParams, null);

            ILGenerator il = constructBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, conObj);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ret);
        }

        private void GenerateComputeMethod(TypeBuilder mockTypeBuilder)
        {
            MethodBuilder methodBuilder = mockTypeBuilder.DefineMethod("Compute",
                  MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                  null,
                  new Type[] { } );

            ILGenerator generator = methodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ret);
        }

        #endregion
    }

    [TestClass]
    public class ComponentsLibraryTest
    {
        private static string AssemblyPath
        {
            get;
            set;
        }

        public ComponentsLibrary Library
        {
            get;
            set;
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        internal TraceLabTestApplication AppContext
        {
            get;
            set;
        }

        public ComponentsLibraryTest()
        {
        }

        [ClassInitialize]
        public static void SetupComponentTests(TestContext context)
        {
            NLog.LogManager.Configuration = new NLog.Config.LoggingConfiguration();

            string name = "TraceLab.Core.Components.Test.MockComponent";
            string assmDir = System.IO.Path.Combine(System.Environment.CurrentDirectory, Guid.NewGuid().ToString());
            System.IO.Directory.CreateDirectory(assmDir);

            //AssemblyPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "TraceLab.Core.Components.Test.MockComponent.DLL");
            MockComponentGenerator gen = new MockComponentGenerator(assmDir, name);
            gen.Generate();
            AssemblyPath = gen.AssemblyPath;

            // Ensure that the assembly is not already loaded
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    //Dynamic libraries generated dynamically in the process cannot access location - it would throw NotSupportedException
                    //for example runtime assemblies Microsoft.GeneratedCode of xml serializers
                    if (assm.IsDynamic == false)
                    {
                        Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
                    }
                }
                catch (NotSupportedException)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                    throw;
                }
            }

            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var types = System.IO.Path.Combine(dir, "Types");
            System.IO.Directory.CreateDirectory(types);
            List<string> typesDir = new List<string>();
            typesDir.Add(types);
            TraceLab.Core.Workspaces.WorkspaceManager.InitWorkspace(typesDir, System.IO.Path.Combine(dir, "workspace"), System.IO.Path.Combine(dir, "cache"));
        }

        [ClassCleanup]
        public static void TeardownComponentTests()
        {
            // Ensure that the assembly did not get loaded into the current application domain.
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
            }
            // We don't even want it in the reflection-only context
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
            {
                Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
            }

            System.IO.File.Delete(AssemblyPath);
            System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(AssemblyPath), true);

            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var types = System.IO.Path.Combine(dir, "Types");
            System.IO.Directory.Delete(types, true);
        }


        [TestInitialize]
        public void TestSetup()
        {
            AppContext = new TraceLabTestApplication(TestContext);
            this.Library = AppContext.Components;
        }

        [TestCleanup]
        public void TestTeardown()
        {
            Library.Clear();
        }

        private IWorkspaceInternal CreateWorkspaceWrapper(ComponentMetadata metadata)
        {
            return WorkspaceWrapperFactory.CreateWorkspaceWrapper(metadata, WorkspaceManager.WorkspaceInternalInstance);
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentsLibraryException))]
        public void TestAddAssemblyNotExists()
        {
            //add a mock components
            string id = "fakeID";
            string assembly = System.IO.Path.Combine(System.Environment.CurrentDirectory, "fake.dll"); //does not exists
            string classname = "fakeClassname";
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, assembly, classname);

            Library.Add(compMetaDef); //throws exception, because dll does not exists
        }


        [TestMethod]
        public void TestAddAssemblyExists()
        {
            string id = "MockComponent";
            string classname = "MockComponents.MockComponent";
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, AssemblyPath, classname);

            Library.Add(compMetaDef); //should pass, dll should exist
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentsLibraryException))]
        public void LoadComponentClassNotLoaded()
        {
            string id = "MockComponent";
            string classname = "FakeClass"; //class does not exist and could not be loaded
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, AssemblyPath, classname);
            ComponentMetadata compMetadata = new ComponentMetadata(compMetaDef, AppContext.BaseTestDirectory);

            Library.Add(compMetaDef);

            AppDomain testComponentsAppDomain = (new TraceLab.Core.Components.LibraryHelper(AppContext.WorkspaceInstance.TypeDirectories)).CreateDomain(id, true);
            try
            {
                Library.LoadComponent(compMetadata, CreateWorkspaceWrapper(compMetadata), null, testComponentsAppDomain);
            }
            finally
            {
                LibraryHelper.DestroyDomain(testComponentsAppDomain);
            }
        }

        [TestMethod]
        public void LoadComponentClassLoaded()
        {
            string id = "MockComponent";
            string classname = "MockComponents.MockComponent"; //class exists and should be loaded
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, AssemblyPath, classname);
            ComponentMetadata compMetadata = new ComponentMetadata(compMetaDef, AppContext.BaseTestDirectory);

            Library.Add(compMetaDef);

            AppDomain testComponentsAppDomain = (new TraceLab.Core.Components.LibraryHelper(AppContext.WorkspaceInstance.TypeDirectories)).CreateDomain(id, true);
            try
            {
                Library.LoadComponent(compMetadata, CreateWorkspaceWrapper(compMetadata), null, testComponentsAppDomain);
            }
            finally
            {
                LibraryHelper.DestroyDomain(testComponentsAppDomain);
            }
        }

        [TestMethod]
        public void LoadComponentMetadataDirectoryScanner()
        {
            Library.Clear();
            Assert.AreEqual(0, Library.Components.Count());

            //set new directory path
            string path = System.IO.Path.GetDirectoryName(AssemblyPath);
            AppContext.Settings.ComponentPaths.Clear();
            AppContext.Settings.ComponentPaths.Add(new SettingsPath(true, path));

            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(Library));
            libraryAccessor.LoadComponentsDefinitions(AppContext.WorkspaceInstance.TypeDirectories);
            Assert.AreEqual(1, Library.Components.Count());

            ComponentMetadataDefinition def = Library.Components.ElementAt(0) as ComponentMetadataDefinition;
            Assert.AreEqual("MockComponents.MockComponent", def.Classname);
            Assert.AreEqual(AssemblyPath.ToLower(), def.Assembly.ToLower());
        }

        [TestMethod]
        public void ComponentDefinitionsHaveTagsApplied()
        {
            Library.Clear();
            string path = System.IO.Path.GetDirectoryName(AssemblyPath);
            AppContext.Settings.ComponentPaths.Clear();
            AppContext.Settings.ComponentPaths.Add(new SettingsPath(true, path));

            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(Library));
            libraryAccessor.LoadComponentsDefinitions(AppContext.WorkspaceInstance.TypeDirectories);

            ComponentMetadataDefinition def = Library.Components.ElementAt(0) as ComponentMetadataDefinition;
            Assert.AreEqual("MockComponents.MockComponent", def.Classname);

            Assert.IsNotNull(def.Tags);
            List<string> tags = new List<string>(def.Tags.Values);
            Assert.AreEqual(2, tags.Count);
            Assert.IsTrue(tags.Contains("Mock"));
            Assert.IsTrue(tags.Contains("DoesNothing"));
        }

        [TestMethod]
        public void LoadAssemblyNotInCurrentDomain()
        {
            // Ensure that the assembly is not already loaded
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                //Dynamic libraries generated dynamically in the process cannot access location - it would throw NotSupportedException
                //for example runtime assemblies Microsoft.GeneratedCode of xml serializers
                if (assm.IsDynamic == false)
                {
                    Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
                }
            }
            // We don't even want it in the reflection-only context
            foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
            {
                Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
            }

            string id = "MockComponent";
            string classname = "MockComponents.MockComponent"; //class exists and should be loaded
            ComponentMetadataDefinition compMetaDef = new ComponentMetadataDefinition(id, AssemblyPath, classname);
            ComponentMetadata compMetadata = new ComponentMetadata(compMetaDef, AppContext.BaseTestDirectory);

            Library.Add(compMetaDef);

            AppDomain testComponentsAppDomain = (new TraceLab.Core.Components.LibraryHelper(AppContext.WorkspaceInstance.TypeDirectories)).CreateDomain(id, true);
            try
            {
                Library.LoadComponent(compMetadata, CreateWorkspaceWrapper(compMetadata), null, testComponentsAppDomain);

                // Ensure that the assembly did not get loaded into the current application domain.
                foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    //Dynamic libraries generated dynamically in the process cannot access location - it would throw NotSupportedException
                    //for example runtime assemblies Microsoft.GeneratedCode of xml serializers
                    if (assm.IsDynamic == false)
                    {
                        Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
                    }
                }
                // We don't even want it in the reflection-only context
                foreach (System.Reflection.Assembly assm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
                {
                    Assert.IsFalse(assm.Location.Equals(AssemblyPath, StringComparison.CurrentCultureIgnoreCase));
                }
            }
            finally
            {
                LibraryHelper.DestroyDomain(testComponentsAppDomain);
            }
        }

        /// <summary>
        /// The test is designed to test loading composite components, where components have dependency on one another.
        /// If they are loaded in a wrong order the application crashes.
        /// 
        /// The test tries to load components from TestLoadCompositeComponents directory, which consists of 12 composite components definition files:
        /// * composite_component.tcml -> dependency on MockComponents.dll
        /// * complex_composite_component.tcml -> dependency on composite_components.tcml, and thus on MockComponents.dll
        /// * complex_composite_component-Copy.tcml -> exact copy of the above file, same component (it should not be loaded to the library twice, nor should crash application)
        /// * unresolved_composite_component.tcml -> dependency on non existing composite component (it should not be loaded to the library)
        /// * and files: 2,3,5,7,8,9,10,11 (refer to the TestLoadCompositeComponents\dependency_graph_of_components.png to see the dependency)
        /// 
        /// Typically, files would be loaded alphabetically, but that would cause test to crash. 
        /// (complex_composite_component.tcml would be loaded before composite_component.tcml
        /// Thus the Library has to determine dependency and load files in correct order.
        /// 
        /// Note, the test ignores original settings from the library and 
        /// First, we compile Mock Components to be stored in the library.
        /// Secondly, the test tries to Load Definitions,
        /// and checks if all composite components are in the library with their Component Graphs not null
        /// </summary>
        [TestMethod]
        public void TestLoadCompositeComponents()
        {
            Library.Clear();
            Assert.AreEqual(0, Library.Components.Count());

            //set library components to new directory
            string testComponentsPath = System.IO.Path.Combine(AppContext.BaseTestDirectory, "TestLoadCompositeComponents");
            AppContext.Settings.ComponentPaths.Clear();
            AppContext.Settings.ComponentPaths.Add(new SettingsPath(true, testComponentsPath));

            string outputAssemblyPath = System.IO.Path.Combine(testComponentsPath, "MockComponents.dll");

            //compile Mock Components to the test directory
            TraceLabTestApplication.CompileMockComponents(outputAssemblyPath);

            //try to load
            var libraryAccessor = new ComponentsLibrary_Accessor(new PrivateObject(Library));
            libraryAccessor.LoadComponentsDefinitions(AppContext.WorkspaceInstance.TypeDirectories);

            //if successful there should be two composite components loaded in the library
            int counter = 0;
            foreach (MetadataDefinition definition in Library.Components)
            {
                CompositeComponentMetadataDefinition compositeCompDefinition = definition as CompositeComponentMetadataDefinition;
                if (compositeCompDefinition != null)
                {
                    //assert its graph is not empty
                    Assert.IsNotNull(compositeCompDefinition.ComponentGraph);
                    counter++;
                }
            }

            Assert.AreEqual(10, counter);
            Assert.AreEqual(15, Library.Components.Count()); //10 composite components + 5 primitive components from MockComponents.dll

            Library.Clear();
        }
    }
}
