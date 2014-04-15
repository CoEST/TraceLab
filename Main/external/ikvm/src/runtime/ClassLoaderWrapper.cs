/*
  Copyright (C) 2002-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using IKVM.Attributes;

namespace IKVM.Internal
{
	[Flags]
	enum CodeGenOptions
	{
		None = 0,
		Debug = 1,
		NoStackTraceInfo = 2,
		StrictFinalFieldSemantics = 4,
		NoJNI = 8,
		RemoveAsserts = 16,
		NoAutomagicSerialization = 32,
	}

#if !STUB_GENERATOR
	abstract class TypeWrapperFactory
	{
		internal abstract ModuleBuilder ModuleBuilder { get; }
		internal abstract TypeWrapper DefineClassImpl(Dictionary<string, TypeWrapper> types, ClassFile f, ClassLoaderWrapper classLoader, object protectionDomain);
		internal abstract bool ReserveName(string name);
		internal abstract string AllocMangledName(DynamicTypeWrapper tw);
		internal abstract Type DefineUnloadable(string name);
#if CLASSGC
		internal abstract void AddInternalsVisibleTo(Assembly friend);
#endif
	}
#endif // !STUB_GENERATOR

	class ClassLoaderWrapper
	{
		private static readonly object wrapperLock = new object();
		private static readonly Dictionary<Type, TypeWrapper> globalTypeToTypeWrapper = new Dictionary<Type, TypeWrapper>();
#if STATIC_COMPILER || STUB_GENERATOR
		private static ClassLoaderWrapper bootstrapClassLoader;
#else
		private static AssemblyClassLoader bootstrapClassLoader;
#endif
		private static List<GenericClassLoader> genericClassLoaders;
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		protected java.lang.ClassLoader javaClassLoader;
#endif
#if !STUB_GENERATOR
		private TypeWrapperFactory factory;
#endif // !STUB_GENERATOR
		private Dictionary<string, TypeWrapper> types = new Dictionary<string, TypeWrapper>();
		private readonly Dictionary<string, Thread> defineClassInProgress = new Dictionary<string, Thread>();
		private List<IntPtr> nativeLibraries;
		private CodeGenOptions codegenoptions;
#if CLASSGC
		private Dictionary<Type, TypeWrapper> typeToTypeWrapper;
		private static ConditionalWeakTable<Assembly, ClassLoaderWrapper> dynamicAssemblies;
#endif
		private static Dictionary<Type, string> remappedTypes = new Dictionary<Type, string>();

#if STATIC_COMPILER || STUB_GENERATOR
		// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
		// when we're compiling the core class libraries and by ikvmstub with the -bootstrap option
		internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
		{
			Debug.Assert(ClassLoaderWrapper.bootstrapClassLoader == null);

			ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
		}
#endif

		static ClassLoaderWrapper()
		{
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.BOOLEAN.TypeAsTBD] = PrimitiveTypeWrapper.BOOLEAN;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.BYTE.TypeAsTBD] = PrimitiveTypeWrapper.BYTE;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.CHAR.TypeAsTBD] = PrimitiveTypeWrapper.CHAR;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.DOUBLE.TypeAsTBD] = PrimitiveTypeWrapper.DOUBLE;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.FLOAT.TypeAsTBD] = PrimitiveTypeWrapper.FLOAT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.INT.TypeAsTBD] = PrimitiveTypeWrapper.INT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.LONG.TypeAsTBD] = PrimitiveTypeWrapper.LONG;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.SHORT.TypeAsTBD] = PrimitiveTypeWrapper.SHORT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.VOID.TypeAsTBD] = PrimitiveTypeWrapper.VOID;
			LoadRemappedTypes();
		}

		internal static void LoadRemappedTypes()
		{
			// if we're compiling the core, coreAssembly will be null
			Assembly coreAssembly = JVM.CoreAssembly;
			if(coreAssembly != null && remappedTypes.Count ==0)
			{
				RemappedClassAttribute[] remapped = AttributeHelper.GetRemappedClasses(coreAssembly);
				if(remapped.Length > 0)
				{
					foreach(RemappedClassAttribute r in remapped)
					{
						remappedTypes.Add(r.RemappedType, r.Name);
					}
				}
				else
				{
#if STATIC_COMPILER
					throw new FatalCompilerErrorException(Message.CoreClassesMissing);
#else
					JVM.CriticalFailure("Failed to find core classes in core library", null);
#endif
				}
			}
		}

		internal ClassLoaderWrapper(CodeGenOptions codegenoptions, object javaClassLoader)
		{
			this.codegenoptions = codegenoptions;
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			this.javaClassLoader = (java.lang.ClassLoader)javaClassLoader;
#endif
		}

		internal static bool IsRemappedType(Type type)
		{
			return remappedTypes.ContainsKey(type);
		}

#if STATIC_COMPILER || STUB_GENERATOR
		internal void SetRemappedType(Type type, TypeWrapper tw)
		{
			lock(types)
			{
				types.Add(tw.Name, tw);
			}
			lock(globalTypeToTypeWrapper)
			{
				globalTypeToTypeWrapper.Add(type, tw);
			}
			remappedTypes.Add(type, tw.Name);
		}
#endif

		// return the TypeWrapper if it is already loaded, this exists for DynamicTypeWrapper.SetupGhosts
		// and ClassLoader.findLoadedClass()
		internal virtual TypeWrapper GetLoadedClass(string name)
		{
			lock(types)
			{
				TypeWrapper tw;
				types.TryGetValue(name, out tw);
				return tw;
			}
		}

		internal TypeWrapper RegisterInitiatingLoader(TypeWrapper tw)
		{
			Debug.Assert(tw != null);
			Debug.Assert(!tw.IsUnloadable);
			Debug.Assert(!tw.IsPrimitive);

			try
			{
				// critical code in the finally block to avoid Thread.Abort interrupting the thread
			}
			finally
			{
				tw = RegisterInitiatingLoaderCritical(tw);
			}
			return tw;
		}

		private TypeWrapper RegisterInitiatingLoaderCritical(TypeWrapper tw)
		{
			lock(types)
			{
				TypeWrapper existing;
				types.TryGetValue(tw.Name, out existing);
				if(existing != tw)
				{
					if(existing != null)
					{
						// another thread beat us to it, discard the new TypeWrapper and
						// return the previous one
						return existing;
					}
					// NOTE if types.ContainsKey(tw.Name) is true (i.e. the value is null),
					// we currently have a DefineClass in progress on another thread and we've
					// beaten that thread to the punch by loading the class from a parent class
					// loader instead. This is ok as DefineClass will throw a LinkageError when
					// it is done.
					types[tw.Name] = tw;
				}
			}
			return tw;
		}

		internal bool EmitDebugInfo
		{
			get
			{
				return (codegenoptions & CodeGenOptions.Debug) != 0;
			}
		}

		internal bool EmitStackTraceInfo
		{
			get
			{
				// NOTE we're negating the flag here!
				return (codegenoptions & CodeGenOptions.NoStackTraceInfo) == 0;
			}
		}

		internal bool StrictFinalFieldSemantics
		{
			get
			{
				return (codegenoptions & CodeGenOptions.StrictFinalFieldSemantics) != 0;
			}
		}

		internal bool NoJNI
		{
			get
			{
				return (codegenoptions & CodeGenOptions.NoJNI) != 0;
			}
		}

		internal bool RemoveAsserts
		{
			get
			{
				return (codegenoptions & CodeGenOptions.RemoveAsserts) != 0;
			}
		}

		internal bool NoAutomagicSerialization
		{
			get
			{
				return (codegenoptions & CodeGenOptions.NoAutomagicSerialization) != 0;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal bool RelaxedClassNameValidation
		{
			get
			{
#if FIRST_PASS
				return true;
#else
				return JVM.relaxedVerification && (javaClassLoader == null || java.lang.ClassLoader.isTrustedLoader(javaClassLoader));
#endif
			}
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		protected virtual void CheckDefineClassAllowed(string className)
		{
			// this hook exists so that AssemblyClassLoader can prevent DefineClass when the name is already present in the assembly
		}

#if !STUB_GENERATOR
		internal TypeWrapper DefineClass(ClassFile f, object protectionDomain)
		{
			string dotnetAssembly = f.IKVMAssemblyAttribute;
			if(dotnetAssembly != null)
			{
				// It's a stub class generated by ikvmstub (or generated by the runtime when getResource was
				// called on a statically compiled class).
				ClassLoaderWrapper loader;
				try
				{
					loader = ClassLoaderWrapper.GetAssemblyClassLoaderByName(dotnetAssembly);
				}
				catch(Exception x)
				{
					// TODO don't catch all exceptions here
					throw new NoClassDefFoundError(f.Name + " (" + x.Message + ")");
				}
				TypeWrapper tw = loader.LoadClassByDottedNameFast(f.Name);
				if(tw == null)
				{
					throw new NoClassDefFoundError(f.Name + " (type not found in " + dotnetAssembly + ")");
				}
				return RegisterInitiatingLoader(tw);
			}
			CheckDefineClassAllowed(f.Name);
			TypeWrapper def;
			try
			{
				// critical code in the finally block to avoid Thread.Abort interrupting the thread
			}
			finally
			{
				def = DefineClassCritical(f, protectionDomain);
			}
			return def;
		}

		private TypeWrapper DefineClassCritical(ClassFile f, object protectionDomain)
		{
			lock(types)
			{
				if(types.ContainsKey(f.Name))
				{
					throw new LinkageError("duplicate class definition: " + f.Name);
				}
				// mark the type as "loading in progress", so that we can detect circular dependencies.
				types.Add(f.Name, null);
				defineClassInProgress.Add(f.Name, Thread.CurrentThread);
			}
			try
			{
				return GetTypeWrapperFactory().DefineClassImpl(types, f, this, protectionDomain);
			}
			finally
			{
				lock(types)
				{
					if(types[f.Name] == null)
					{
						// if loading the class fails, we remove the indicator that we're busy loading the class,
						// because otherwise we get a ClassCircularityError if we try to load the class again.
						types.Remove(f.Name);
					}
					defineClassInProgress.Remove(f.Name);
					Monitor.PulseAll(types);
				}
			}
		}

		internal TypeWrapperFactory GetTypeWrapperFactory()
		{
			if(factory == null)
			{
				lock(this)
				{
					try
					{
						// critical code in the finally block to avoid Thread.Abort interrupting the thread
					}
					finally
					{
						if(factory == null)
						{
#if CLASSGC
							if(dynamicAssemblies == null)
							{
								Interlocked.CompareExchange(ref dynamicAssemblies, new ConditionalWeakTable<Assembly, ClassLoaderWrapper>(), null);
							}
							typeToTypeWrapper = new Dictionary<Type, TypeWrapper>();
							DynamicClassLoader instance = DynamicClassLoader.Get(this);
							dynamicAssemblies.Add(instance.ModuleBuilder.Assembly.ManifestModule.Assembly, this);
							this.factory = instance;
#else
							factory = DynamicClassLoader.Get(this);
#endif
						}
					}
				}
			}
			return factory;
		}
#endif // !STUB_GENERATOR

		internal TypeWrapper LoadClassByDottedName(string name)
		{
			TypeWrapper type = LoadClassByDottedNameFastImpl(name, true);
			if(type != null)
			{
				return RegisterInitiatingLoader(type);
			}
			throw new ClassNotFoundException(name);
		}

		internal TypeWrapper LoadClassByDottedNameFast(string name)
		{
			TypeWrapper type = LoadClassByDottedNameFastImpl(name, false);
			if(type != null)
			{
				return RegisterInitiatingLoader(type);
			}
			return null;
		}

		private TypeWrapper LoadClassByDottedNameFastImpl(string name, bool throwClassNotFoundException)
		{
			Profiler.Enter("LoadClassByDottedName");
			try
			{
				TypeWrapper type;
				lock(types)
				{
					if(types.TryGetValue(name, out type) && type == null)
					{
						Thread defineThread;
						if(defineClassInProgress.TryGetValue(name, out defineThread))
						{
							if(Thread.CurrentThread == defineThread)
							{
								throw new ClassCircularityError(name);
							}
							// the requested class is currently being defined by another thread,
							// so we have to wait on that
							while(defineClassInProgress.ContainsKey(name))
							{
								Monitor.Wait(types);
							}
							// the defineClass may have failed, so we need to use TryGetValue
							types.TryGetValue(name, out type);
						}
					}
				}
				if(type != null)
				{
					return type;
				}
				if(name.Length > 1 && name[0] == '[')
				{
					return LoadArrayClass(name);
				}
				return LoadClassImpl(name, throwClassNotFoundException);
			}
			finally
			{
				Profiler.Leave("LoadClassByDottedName");
			}
		}

		private TypeWrapper LoadArrayClass(string name)
		{
			int dims = 1;
			while(name[dims] == '[')
			{
				dims++;
				if(dims == name.Length)
				{
					// malformed class name
					return null;
				}
			}
			if(name[dims] == 'L')
			{
				if(!name.EndsWith(";") || name.Length <= dims + 2 || name[dims + 1] == '[')
				{
					// malformed class name
					return null;
				}
				string elemClass = name.Substring(dims + 1, name.Length - dims - 2);
				// NOTE it's important that we're registered as the initiating loader
				// for the element type here
				TypeWrapper type = LoadClassByDottedNameFast(elemClass);
				if(type != null)
				{
					type = type.GetClassLoader().CreateArrayType(name, type, dims);
				}
				return type;
			}
			if(name.Length != dims + 1)
			{
				// malformed class name
				return null;
			}
			switch(name[dims])
			{
				case 'B':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BYTE, dims);
				case 'C':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.CHAR, dims);
				case 'D':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.DOUBLE, dims);
				case 'F':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.FLOAT, dims);
				case 'I':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.INT, dims);
				case 'J':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.LONG, dims);
				case 'S':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.SHORT, dims);
				case 'Z':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BOOLEAN, dims);
				default:
					return null;
			}
		}

		internal TypeWrapper LoadGenericClass(string name)
		{
			// we need to handle delegate methods here (for generic delegates)
			// (note that other types with manufactured inner classes such as Attribute and Enum can't be generic)
			if (name.EndsWith(DotNetTypeWrapper.DelegateInterfaceSuffix))
			{
				TypeWrapper outer = LoadGenericClass(name.Substring(0, name.Length - DotNetTypeWrapper.DelegateInterfaceSuffix.Length));
				if (outer != null && outer.IsFakeTypeContainer)
				{
					foreach (TypeWrapper tw in outer.InnerClasses)
					{
						if (tw.Name == name)
						{
							return tw;
						}
					}
				}
			}
			// generic class name grammar:
			//
			// mangled(open_generic_type_name) "_$$$_" M(parameter_class_name) ( "_$$_" M(parameter_class_name) )* "_$$$$_"
			//
			// mangled() is the normal name mangling algorithm
			// M() is a replacement of "__" with "$$005F$$005F" followed by a replace of "." with "__"
			//
			int pos = name.IndexOf("_$$$_");
			if(pos <= 0 || !name.EndsWith("_$$$$_"))
			{
				return null;
			}
			Type type = GetGenericTypeDefinition(DotNetTypeWrapper.DemangleTypeName(name.Substring(0, pos)));
			if(type == null)
			{
				return null;
			}
			List<string> typeParamNames = new List<string>();
			pos += 5;
			int start = pos;
			int nest = 0;
			for(;;)
			{
				pos = name.IndexOf("_$$", pos);
				if(pos == -1)
				{
					return null;
				}
				if(name.IndexOf("_$$_", pos, 4) == pos)
				{
					if(nest == 0)
					{
						typeParamNames.Add(name.Substring(start, pos - start));
						start = pos + 4;
					}
					pos += 4;
				}
				else if(name.IndexOf("_$$$_", pos, 5) == pos)
				{
					nest++;
					pos += 5;
				}
				else if(name.IndexOf("_$$$$_", pos, 6) == pos)
				{
					if(nest == 0)
					{
						if(pos + 6 != name.Length)
						{
							return null;
						}
						typeParamNames.Add(name.Substring(start, pos - start));
						break;
					}
					nest--;
					pos += 6;
				}
				else
				{
					pos += 3;
				}
			}
			Type[] typeArguments = new Type[typeParamNames.Count];
			for(int i = 0; i < typeArguments.Length; i++)
			{
				string s = (string)typeParamNames[i];
				// only do the unmangling for non-generic types (because we don't want to convert
				// the double underscores in two adjacent _$$$_ or _$$$$_ markers)
				if(s.IndexOf("_$$$_") == -1)
				{
					s = s.Replace("__", ".");
					s = s.Replace("$$005F$$005F", "__");
				}
				int dims = 0;
				while(s.Length > dims && s[dims] == 'A')
				{
					dims++;
				}
				if(s.Length == dims)
				{
					return null;
				}
				TypeWrapper tw;
				switch(s[dims])
				{
					case 'L':
						tw = LoadClassByDottedNameFast(s.Substring(dims + 1));
						if(tw == null)
						{
							return null;
						}
						tw.Finish();
						break;
					case 'Z':
						tw = PrimitiveTypeWrapper.BOOLEAN;
						break;
					case 'B':
						tw = PrimitiveTypeWrapper.BYTE;
						break;
					case 'S':
						tw = PrimitiveTypeWrapper.SHORT;
						break;
					case 'C':
						tw = PrimitiveTypeWrapper.CHAR;
						break;
					case 'I':
						tw = PrimitiveTypeWrapper.INT;
						break;
					case 'F':
						tw = PrimitiveTypeWrapper.FLOAT;
						break;
					case 'J':
						tw = PrimitiveTypeWrapper.LONG;
						break;
					case 'D':
						tw = PrimitiveTypeWrapper.DOUBLE;
						break;
					default:
						return null;
				}
				if(dims > 0)
				{
					tw = tw.MakeArrayType(dims);
				}
				typeArguments[i] = tw.TypeAsSignatureType;
			}
			try
			{
				type = type.MakeGenericType(typeArguments);
			}
			catch(ArgumentException)
			{
				// one of the typeArguments failed to meet the constraints
				return null;
			}
			TypeWrapper wrapper = GetWrapperFromType(type);
			if(wrapper != null && wrapper.Name != name)
			{
				// the name specified was not in canonical form
				return null;
			}
			return wrapper;
		}

		protected virtual TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = LoadGenericClass(name);
			if(tw != null)
			{
				return tw;
			}
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			Profiler.Enter("ClassLoader.loadClass");
			try
			{
				java.lang.Class c = ((java.lang.ClassLoader)GetJavaClassLoader()).loadClassInternal(name);
				if(c == null)
				{
					return null;
				}
				TypeWrapper type = TypeWrapper.FromClass(c);
				if(type.Name != name)
				{
					// the class loader is trying to trick us
					return null;
				}
				return type;
			}
			catch(java.lang.ClassNotFoundException x)
			{
				if(throwClassNotFoundException)
				{
					throw new ClassLoadingException(ikvm.runtime.Util.mapException(x));
				}
				return null;
			}
			catch(Exception x)
			{
				throw new ClassLoadingException(ikvm.runtime.Util.mapException(x));
			}
			finally
			{
				Profiler.Leave("ClassLoader.loadClass");
			}
#else
			return null;
#endif
		}

		private TypeWrapper CreateArrayType(string name, TypeWrapper elementTypeWrapper, int dims)
		{
			Debug.Assert(new String('[', dims) + elementTypeWrapper.SigName == name);
			Debug.Assert(!elementTypeWrapper.IsUnloadable && !elementTypeWrapper.IsVerifierType && !elementTypeWrapper.IsArray);
			Debug.Assert(dims >= 1);
			return RegisterInitiatingLoader(new ArrayTypeWrapper(elementTypeWrapper, name));
		}

		internal virtual object GetJavaClassLoader()
		{
#if FIRST_PASS || STATIC_COMPILER || STUB_GENERATOR
			return null;
#else
			return javaClassLoader;
#endif
		}

		internal TypeWrapper ExpressionTypeWrapper(string type)
		{
			Debug.Assert(!type.StartsWith("Lret;"));
			Debug.Assert(type != "Lnull");

			int index = 0;
			return SigDecoderWrapper(ref index, type, false);
		}

		// NOTE this exposes potentially unfinished types
		internal Type[] ArgTypeListFromSig(string sig)
		{
			if(sig[1] == ')')
			{
				return Type.EmptyTypes;
			}
			TypeWrapper[] wrappers = ArgTypeWrapperListFromSig(sig);
			Type[] types = new Type[wrappers.Length];
			for(int i = 0; i < wrappers.Length; i++)
			{
				types[i] = wrappers[i].TypeAsSignatureType;
			}
			return types;
		}

		private TypeWrapper SigDecoderLoadClass(string name, bool nothrow)
		{
			return nothrow ? LoadClassNoThrow(this, name) : LoadClassByDottedName(name);
		}

		// NOTE: this will ignore anything following the sig marker (so that it can be used to decode method signatures)
		private TypeWrapper SigDecoderWrapper(ref int index, string sig, bool nothrow)
		{
			switch(sig[index++])
			{
				case 'B':
					return PrimitiveTypeWrapper.BYTE;
				case 'C':
					return PrimitiveTypeWrapper.CHAR;
				case 'D':
					return PrimitiveTypeWrapper.DOUBLE;
				case 'F':
					return PrimitiveTypeWrapper.FLOAT;
				case 'I':
					return PrimitiveTypeWrapper.INT;
				case 'J':
					return PrimitiveTypeWrapper.LONG;
				case 'L':
				{
					int pos = index;
					index = sig.IndexOf(';', index) + 1;
					return SigDecoderLoadClass(sig.Substring(pos, index - pos - 1), nothrow);
				}
				case 'S':
					return PrimitiveTypeWrapper.SHORT;
				case 'Z':
					return PrimitiveTypeWrapper.BOOLEAN;
				case 'V':
					return PrimitiveTypeWrapper.VOID;
				case '[':
				{
					// TODO this can be optimized
					string array = "[";
					while(sig[index] == '[')
					{
						index++;
						array += "[";
					}
					switch(sig[index])
					{
						case 'L':
						{
							int pos = index;
							index = sig.IndexOf(';', index) + 1;
							return SigDecoderLoadClass(array + sig.Substring(pos, index - pos), nothrow);
						}
						case 'B':
						case 'C':
						case 'D':
						case 'F':
						case 'I':
						case 'J':
						case 'S':
						case 'Z':
							return SigDecoderLoadClass(array + sig[index++], nothrow);
						default:
							throw new InvalidOperationException(sig.Substring(index));
					}
				}
				default:
					throw new InvalidOperationException(sig.Substring(index));
			}
		}

		internal TypeWrapper FieldTypeWrapperFromSig(string sig)
		{
			int index = 0;
			return SigDecoderWrapper(ref index, sig, false);
		}

		internal TypeWrapper FieldTypeWrapperFromSigNoThrow(string sig)
		{
			int index = 0;
			return SigDecoderWrapper(ref index, sig, true);
		}

		internal TypeWrapper RetTypeWrapperFromSig(string sig)
		{
			int index = sig.IndexOf(')') + 1;
			return SigDecoderWrapper(ref index, sig, false);
		}

		internal TypeWrapper RetTypeWrapperFromSigNoThrow(string sig)
		{
			int index = sig.IndexOf(')') + 1;
			return SigDecoderWrapper(ref index, sig, true);
		}

		internal TypeWrapper[] ArgTypeWrapperListFromSig(string sig)
		{
			if(sig[1] == ')')
			{
				return TypeWrapper.EmptyArray;
			}
			List<TypeWrapper> list = new List<TypeWrapper>();
			for(int i = 1; sig[i] != ')';)
			{
				list.Add(SigDecoderWrapper(ref i, sig, false));
			}
			return list.ToArray();
		}

		internal TypeWrapper[] ArgTypeWrapperListFromSigNoThrow(string sig)
		{
			if (sig[1] == ')')
			{
				return TypeWrapper.EmptyArray;
			}
			List<TypeWrapper> list = new List<TypeWrapper>();
			for (int i = 1; sig[i] != ')'; )
			{
				list.Add(SigDecoderWrapper(ref i, sig, true));
			}
			return list.ToArray();
		}

#if STATIC_COMPILER || STUB_GENERATOR
		internal static ClassLoaderWrapper GetBootstrapClassLoader()
#else
		internal static AssemblyClassLoader GetBootstrapClassLoader()
#endif
		{
			lock(wrapperLock)
			{
				if(bootstrapClassLoader == null)
				{
					bootstrapClassLoader = new BootstrapClassLoader();
				}
				return bootstrapClassLoader;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static ClassLoaderWrapper GetClassLoaderWrapper(object javaClassLoader)
		{
			if(javaClassLoader == null)
			{
				return GetBootstrapClassLoader();
			}
			lock(wrapperLock)
			{
#if FIRST_PASS
				ClassLoaderWrapper wrapper = null;
#else
				ClassLoaderWrapper wrapper = 
#if __MonoCS__
					// MONOBUG the redundant cast to ClassLoaderWrapper is to workaround an mcs bug
					(ClassLoaderWrapper)(object)
#endif
					((java.lang.ClassLoader)javaClassLoader).wrapper;
#endif
				if(wrapper == null)
				{
					CodeGenOptions opt = CodeGenOptions.None;
					if(JVM.EmitSymbols)
					{
						opt |= CodeGenOptions.Debug;
					}
#if NET_4_0
					if (!AppDomain.CurrentDomain.IsFullyTrusted)
					{
						opt |= CodeGenOptions.NoAutomagicSerialization;
					}
#endif
					wrapper = new ClassLoaderWrapper(opt, javaClassLoader);
					SetWrapperForClassLoader(javaClassLoader, wrapper);
				}
				return wrapper;
			}
		}
#endif

#if CLASSGC
		internal static ClassLoaderWrapper GetClassLoaderForDynamicJavaAssembly(Assembly asm)
		{
			ClassLoaderWrapper loader;
			dynamicAssemblies.TryGetValue(asm, out loader);
			return loader;
		}
#endif // CLASSGC

		internal static TypeWrapper GetWrapperFromType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromType: {0}", type.AssemblyQualifiedName);
#if !STATIC_COMPILER
			TypeWrapper.AssertFinished(type);
#endif
			Debug.Assert(!type.IsPointer);
			Debug.Assert(!type.IsByRef);
			TypeWrapper wrapper;
			lock(globalTypeToTypeWrapper)
			{
				globalTypeToTypeWrapper.TryGetValue(type, out wrapper);
			}
			if(wrapper != null)
			{
				return wrapper;
			}
#if STUB_GENERATOR
			if(type.__IsMissing || type.__ContainsMissingType)
			{
				wrapper = new UnloadableTypeWrapper("Missing/" + type.Assembly.FullName);
				globalTypeToTypeWrapper.Add(type, wrapper);
				return wrapper;
			}
#endif
			string remapped;
			if(remappedTypes.TryGetValue(type, out remapped))
			{
				wrapper = LoadClassCritical(remapped);
			}
			else if(ReflectUtil.IsVector(type))
			{
				// it might be an array of a dynamically compiled Java type
				int rank = 1;
				Type elem = type.GetElementType();
				while(ReflectUtil.IsVector(elem))
				{
					rank++;
					elem = elem.GetElementType();
				}
				wrapper = GetWrapperFromType(elem).MakeArrayType(rank);
			}
			else
			{
				Assembly asm = type.Assembly;
#if CLASSGC
				ClassLoaderWrapper loader;
				if(dynamicAssemblies != null && dynamicAssemblies.TryGetValue(asm, out loader))
				{
					lock(loader.typeToTypeWrapper)
					{
						return loader.typeToTypeWrapper[type];
					}
				}
#endif
#if !STATIC_COMPILER && !STUB_GENERATOR
				if(ReflectUtil.IsReflectionOnly(type))
				{
					// historically we've always returned null for types that don't have a corresponding TypeWrapper (or java.lang.Class)
					return null;
				}
#endif
				// if the wrapper doesn't already exist, that must mean that the type
				// is a .NET type (or a pre-compiled Java class), which means that it
				// was "loaded" by an assembly classloader
				wrapper = AssemblyClassLoader.FromAssembly(asm).GetWrapperFromAssemblyType(type);
			}
#if CLASSGC
			if(type.Assembly.IsDynamic)
			{
				// don't cache types in dynamic assemblies, because they might live in a RunAndCollect assembly
				// TODO we also shouldn't cache generic type instances that have a GCable type parameter
				return wrapper;
			}
#endif
			lock(globalTypeToTypeWrapper)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					globalTypeToTypeWrapper[type] = wrapper;
				}
			}
			return wrapper;
		}

		internal virtual Type GetGenericTypeDefinition(string name)
		{
			return null;
		}

		internal static ClassLoaderWrapper GetGenericClassLoader(TypeWrapper wrapper)
		{
			Type type = wrapper.TypeAsTBD;
			Debug.Assert(type.IsGenericType);
			Debug.Assert(!type.ContainsGenericParameters);

			List<ClassLoaderWrapper> list = new List<ClassLoaderWrapper>();
			list.Add(AssemblyClassLoader.FromAssembly(type.Assembly));
			foreach(Type arg in type.GetGenericArguments())
			{
				ClassLoaderWrapper loader = GetWrapperFromType(arg).GetClassLoader();
				if(!list.Contains(loader) && loader != bootstrapClassLoader)
				{
					list.Add(loader);
				}
			}
			ClassLoaderWrapper[] key = list.ToArray();
			ClassLoaderWrapper matchingLoader = GetGenericClassLoaderByKey(key);
			matchingLoader.RegisterInitiatingLoader(wrapper);
			return matchingLoader;
		}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		internal static object DoPrivileged(java.security.PrivilegedAction action)
		{
			return java.security.AccessController.doPrivileged(action, ikvm.@internal.CallerID.create(typeof(java.lang.ClassLoader).TypeHandle));
		}
#endif

		private static ClassLoaderWrapper GetGenericClassLoaderByKey(ClassLoaderWrapper[] key)
		{
			lock(wrapperLock)
			{
				if(genericClassLoaders == null)
				{
					genericClassLoaders = new List<GenericClassLoader>();
				}
				foreach(GenericClassLoader loader in genericClassLoaders)
				{
					if(loader.Matches(key))
					{
						return loader;
					}
				}
				object javaClassLoader = null;
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				javaClassLoader = DoPrivileged(new AssemblyClassLoader.CreateAssemblyClassLoader(null));
#endif
				GenericClassLoader newLoader = new GenericClassLoader(key, javaClassLoader);
				SetWrapperForClassLoader(javaClassLoader, newLoader);
				genericClassLoaders.Add(newLoader);
				return newLoader;
			}
		}

		protected static void SetWrapperForClassLoader(object javaClassLoader, ClassLoaderWrapper wrapper)
		{
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
#if __MonoCS__
			typeof(java.lang.ClassLoader).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(javaClassLoader, wrapper);
#else
			((java.lang.ClassLoader)javaClassLoader).wrapper = wrapper;
#endif
#endif
		}

		internal static ClassLoaderWrapper GetGenericClassLoaderByName(string name)
		{
			Debug.Assert(name.StartsWith("[[") && name.EndsWith("]]"));
			Stack<List<ClassLoaderWrapper>> stack = new Stack<List<ClassLoaderWrapper>>();
			List<ClassLoaderWrapper> list = null;
			for(int i = 0; i < name.Length; i++)
			{
				if(name[i] == '[')
				{
					if(name[i + 1] == '[')
					{
						stack.Push(list);
						list = new List<ClassLoaderWrapper>();
						if(name[i + 2] == '[')
						{
							i++;
						}
					}
					else
					{
						int start = i + 1;
						i = name.IndexOf(']', i);
						list.Add(ClassLoaderWrapper.GetAssemblyClassLoaderByName(name.Substring(start, i - start)));
					}
				}
				else if(name[i] == ']')
				{
					ClassLoaderWrapper loader = GetGenericClassLoaderByKey(list.ToArray());
					list = stack.Pop();
					if(list == null)
					{
						return loader;
					}
					list.Add(loader);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			throw new InvalidOperationException();
		}

		internal static ClassLoaderWrapper GetAssemblyClassLoaderByName(string name)
		{
			if(name.StartsWith("[["))
			{
				return GetGenericClassLoaderByName(name);
			}
#if STATIC_COMPILER || STUB_GENERATOR
			return AssemblyClassLoader.FromAssembly(StaticCompiler.Load(name));
#else
			return AssemblyClassLoader.FromAssembly(Assembly.Load(name));
#endif
		}

		internal static int GetGenericClassLoaderId(ClassLoaderWrapper wrapper)
		{
			lock(wrapperLock)
			{
				return genericClassLoaders.IndexOf(wrapper as GenericClassLoader);
			}
		}

		internal static ClassLoaderWrapper GetGenericClassLoaderById(int id)
		{
			lock(wrapperLock)
			{
				return genericClassLoaders[id];
			}
		}

		internal void SetWrapperForType(Type type, TypeWrapper wrapper)
		{
#if !STATIC_COMPILER
			TypeWrapper.AssertFinished(type);
#endif
			Dictionary<Type, TypeWrapper> dict;
#if CLASSGC
			dict = typeToTypeWrapper ?? globalTypeToTypeWrapper;
#else
			dict = globalTypeToTypeWrapper;
#endif
			lock (dict)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					dict.Add(type, wrapper);
				}
			}
		}

		internal static TypeWrapper LoadClassCritical(string name)
		{
#if STATIC_COMPILER
			TypeWrapper wrapper = GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
			if (wrapper == null)
			{
				throw new FatalCompilerErrorException(Message.CriticalClassNotFound, name);
			}
			return wrapper;
#else
			try
			{
				return GetBootstrapClassLoader().LoadClassByDottedName(name);
			}
			catch(Exception x)
			{
				JVM.CriticalFailure("Loading of critical class failed", x);
				return null;
			}
#endif
		}

		internal void RegisterNativeLibrary(IntPtr p)
		{
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					if(nativeLibraries == null)
					{
						nativeLibraries = new List<IntPtr>();
					}
					nativeLibraries.Add(p);
				}
			}
		}

		internal void UnregisterNativeLibrary(IntPtr p)
		{
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					nativeLibraries.Remove(p);
				}
			}
		}

		internal IntPtr[] GetNativeLibraries()
		{
			lock(this)
			{
				if(nativeLibraries ==  null)
				{
					return new IntPtr[0];
				}
				return nativeLibraries.ToArray();
			}
		}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		public override string ToString()
		{
			object javaClassLoader = GetJavaClassLoader();
			if(javaClassLoader == null)
			{
				return "null";
			}
			return String.Format("{0}@{1:X}", GetWrapperFromType(javaClassLoader.GetType()).Name, javaClassLoader.GetHashCode());
		}
#endif

		internal virtual bool InternalsVisibleToImpl(TypeWrapper wrapper, TypeWrapper friend)
		{
			Debug.Assert(wrapper.GetClassLoader() == this);
			return this == friend.GetClassLoader();
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		// this method is used by IKVM.Runtime.JNI
		internal static ClassLoaderWrapper FromCallerID(ikvm.@internal.CallerID callerID)
		{
#if FIRST_PASS
			return null;
#else
			return GetClassLoaderWrapper(callerID.getCallerClassLoader());
#endif
		}
#endif

		internal static TypeWrapper LoadClassNoThrow(ClassLoaderWrapper classLoader, string name)
		{
			try
			{
				TypeWrapper wrapper = classLoader.LoadClassByDottedNameFast(name);
				if (wrapper == null)
				{
					string elementTypeName = name;
					if (elementTypeName.StartsWith("["))
					{
						int skip = 1;
						while (elementTypeName[skip++] == '[') ;
						elementTypeName = elementTypeName.Substring(skip, elementTypeName.Length - skip - 1);
					}
#if STATIC_COMPILER
					classLoader.IssueMessage(Message.ClassNotFound, elementTypeName);
#else
					Tracer.Error(Tracer.ClassLoading, "Class not found: {0}", elementTypeName);
#endif
					wrapper = new UnloadableTypeWrapper(name);
				}
				return wrapper;
			}
			catch (RetargetableJavaException x)
			{
				// HACK keep the compiler from warning about unused local
				GC.KeepAlive(x);
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				if(x.ToJava() is java.lang.ThreadDeath)
				{
					throw x.ToJava();
				}
				if(Tracer.ClassLoading.TraceError)
				{
					java.lang.ClassLoader cl = (java.lang.ClassLoader)classLoader.GetJavaClassLoader();
					if(cl != null)
					{
						System.Text.StringBuilder sb = new System.Text.StringBuilder();
						string sep = "";
						while(cl != null)
						{
							sb.Append(sep).Append(cl);
							sep = " -> ";
							cl = cl.getParent();
						}
						Tracer.Error(Tracer.ClassLoading, "ClassLoader chain: {0}", sb);
					}
					Exception m = ikvm.runtime.Util.mapException(x.ToJava());
					Tracer.Error(Tracer.ClassLoading, m.ToString() + Environment.NewLine + m.StackTrace);
				}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
				return new UnloadableTypeWrapper(name);
			}
		}

#if STATIC_COMPILER
		internal virtual void IssueMessage(Message msgId, params string[] values)
		{
			// it's not ideal when we end up here (because it means we're emitting a warning that is not associated with a specific output target),
			// but it happens when we're decoding something in a referenced assembly that either doesn't make sense or contains an unloadable type
			StaticCompiler.IssueMessage(msgId, values);
		}
#endif
	}

	class GenericClassLoader : ClassLoaderWrapper
	{
		private ClassLoaderWrapper[] delegates;

		internal GenericClassLoader(ClassLoaderWrapper[] delegates, object javaClassLoader)
			: base(CodeGenOptions.None, javaClassLoader)
		{
			this.delegates = delegates;
		}

		internal bool Matches(ClassLoaderWrapper[] key)
		{
			if(key.Length == delegates.Length)
			{
				for(int i = 0; i < key.Length; i++)
				{
					if(key[i] != delegates[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		internal override Type GetGenericTypeDefinition(string name)
		{
			foreach(ClassLoaderWrapper loader in delegates)
			{
				Type t = loader.GetGenericTypeDefinition(name);
				if(t != null)
				{
					return t;
				}
			}
			return null;
		}

		protected override TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = LoadGenericClass(name);
			if(tw != null)
			{
				return tw;
			}
			foreach(ClassLoaderWrapper loader in delegates)
			{
				tw = loader.LoadClassByDottedNameFast(name);
				if(tw != null)
				{
					return tw;
				}
			}
			return null;
		}

		internal string GetName()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('[');
			foreach(ClassLoaderWrapper loader in delegates)
			{
				sb.Append('[');
				GenericClassLoader gcl = loader as GenericClassLoader;
				if(gcl != null)
				{
					sb.Append(gcl.GetName());
				}
				else
				{
					sb.Append(((AssemblyClassLoader)loader).MainAssembly.FullName);
				}
				sb.Append(']');
			}
			sb.Append(']');
			return sb.ToString();
		}
	}
}
