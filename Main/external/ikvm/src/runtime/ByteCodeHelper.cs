/*
  Copyright (C) 2002-2012 Jeroen Frijters

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
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using IKVM.Attributes;
using IKVM.Internal;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{
	static class GhostTag
	{
		private static volatile PassiveWeakDictionary<object, TypeWrapper> dict;

		internal static void SetTag(object obj, RuntimeTypeHandle typeHandle)
		{
			SetTag(obj, ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(typeHandle)));
		}

		internal static void SetTag(object obj, TypeWrapper wrapper)
		{
			if(dict == null)
			{
				PassiveWeakDictionary<object, TypeWrapper> newDict = new PassiveWeakDictionary<object, TypeWrapper>();
#pragma warning disable 0420 // don't whine about CompareExchange not respecting 'volatile'
				if(Interlocked.CompareExchange(ref dict, newDict, null) != null)
#pragma warning restore
				{
					newDict.Dispose();
				}
			}
			dict.Add(obj, wrapper);
		}

		internal static TypeWrapper GetTag(object obj)
		{
			if(dict != null)
			{
				TypeWrapper tw;
				dict.TryGetValue(obj, out tw);
				return tw;
			}
			return null;
		}

		// this method is called from <GhostType>.IsInstanceArray()
		internal static bool IsGhostArrayInstance(object obj, RuntimeTypeHandle typeHandle, int rank)
		{
			TypeWrapper tw1 = GhostTag.GetTag(obj);
			if(tw1 != null)
			{
				TypeWrapper tw2 = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(typeHandle)).MakeArrayType(rank);
				return tw1.IsAssignableTo(tw2);
			}
			return false;
		}

		// this method is called from <GhostType>.CastArray()
		[HideFromJava]
		internal static void ThrowClassCastException(object obj, RuntimeTypeHandle typeHandle, int rank)
		{
#if !FIRST_PASS
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(ikvm.runtime.Util.getClassFromObject(obj).getName()).Append(" cannot be cast to ")
				.Append('[', rank).Append('L').Append(ikvm.runtime.Util.getClassFromTypeHandle(typeHandle).getName()).Append(';');
			throw new java.lang.ClassCastException(sb.ToString());
#endif
		}
	}

	public static class ByteCodeHelper
	{
		[DebuggerStepThroughAttribute]
		public static object multianewarray(RuntimeTypeHandle typeHandle, int[] lengths)
		{
			for(int i = 0; i < lengths.Length; i++)
			{
				if(lengths[i] < 0)
				{
#if !FIRST_PASS
					throw new java.lang.NegativeArraySizeException();
#endif
				}
			}
			return MultianewarrayHelper(Type.GetTypeFromHandle(typeHandle).GetElementType(), lengths, 0);
		}

		private static object MultianewarrayHelper(Type elemType, int[] lengths, int index)
		{
			object o = Array.CreateInstance(elemType, lengths[index++]);
			if(index < lengths.Length)
			{
				elemType = elemType.GetElementType();
				object[] a = (object[])o;
				for(int i = 0; i < a.Length; i++)
				{
					a[i] = MultianewarrayHelper(elemType, lengths, index);
				}
			}
			return o;
		}

		[DebuggerStepThroughAttribute]
		public static object multianewarray_ghost(RuntimeTypeHandle typeHandle, int[] lengths)
		{
			Type type = Type.GetTypeFromHandle(typeHandle);
			int rank = 0;
			while(ReflectUtil.IsVector(type))
			{
				rank++;
				type = type.GetElementType();
			}
			object obj = multianewarray(ArrayTypeWrapper.MakeArrayType(typeof(object), rank).TypeHandle, lengths);
			GhostTag.SetTag(obj, typeHandle);
			return obj;
		}

		[DebuggerStepThroughAttribute]
		public static T[] anewarray_ghost<T>(int length, RuntimeTypeHandle typeHandle)
		{
			T[] obj = new T[length];
			GhostTag.SetTag(obj, typeHandle);
			return obj;
		}

#if !FIRST_PASS
		[DebuggerStepThroughAttribute]
		public static object DynamicMultianewarray(RuntimeTypeHandle type, string clazz, int[] lengths)
		{
			Profiler.Count("DynamicMultianewarray");
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			return multianewarray(wrapper.TypeAsArrayType.TypeHandle, lengths);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicNewarray(int length, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicNewarray");
			if(length < 0)
			{
				throw new java.lang.NegativeArraySizeException();
			}
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			return Array.CreateInstance(wrapper.TypeAsArrayType, length);
		}

		[DebuggerStepThroughAttribute]
		public static void DynamicAastore(object arrayref, int index, object val, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicAastore");
			// TODO do we need to load the type here?
			((Array)arrayref).SetValue(val, index);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicAaload(object arrayref, int index, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicAaload");
			// TODO do we need to load the type here?
			return ((Array)arrayref).GetValue(index);
		}

		private static FieldWrapper GetFieldWrapper(object thisObj, RuntimeTypeHandle type, string clazz, string name, string sig, bool isStatic)
		{
			TypeWrapper caller = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			FieldWrapper field = wrapper.GetFieldWrapper(name, sig);
			if(field == null)
			{
				throw new java.lang.NoSuchFieldError(clazz + "." + name);
			}
			// TODO check loader constraints
			if(field.IsStatic != isStatic)
			{
				throw new java.lang.IncompatibleClassChangeError(clazz + "." + name);
			}
			TypeWrapper objType = null;
			if(thisObj != null)
			{
				objType = ClassLoaderWrapper.GetWrapperFromType(thisObj.GetType());
			}
			if(field.IsAccessibleFrom(wrapper, caller, objType))
			{
				return field;
			}
			throw new java.lang.IllegalAccessError(field.DeclaringType.Name + "." + name);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicGetfield(object obj, string name, string sig, RuntimeTypeHandle type, string clazz, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicGetfield");
			FieldWrapper fw = GetFieldWrapper(obj, type, clazz, name, sig, false);
			java.lang.reflect.Field field = (java.lang.reflect.Field)fw.ToField(false);
			object val = field.get(obj, callerID);
			if(fw.FieldTypeWrapper.IsPrimitive)
			{
				val = JVM.Unbox(val);
			}
			return val;
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicGetstatic(string name, string sig, RuntimeTypeHandle type, string clazz, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicGetstatic");
			FieldWrapper fw = GetFieldWrapper(null, type, clazz, name, sig, true);
			java.lang.reflect.Field field = (java.lang.reflect.Field)fw.ToField(false);
			object val = field.get(null, callerID);
			if(fw.FieldTypeWrapper.IsPrimitive)
			{
				val = JVM.Unbox(val);
			}
			return val;
		}

		[DebuggerStepThroughAttribute]
		public static void DynamicPutfield(object obj, object val, string name, string sig, RuntimeTypeHandle type, string clazz, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicPutfield");
			FieldWrapper fw = GetFieldWrapper(obj, type, clazz, name, sig, false);
			if(fw.IsFinal)
			{
				throw new java.lang.IllegalAccessError("Field " + fw.DeclaringType.Name + "." + fw.Name + " is final");
			}
			java.lang.reflect.Field field = (java.lang.reflect.Field)fw.ToField(false);
			if(fw.FieldTypeWrapper.IsPrimitive)
			{
				val = JVM.Box(val);
			}
			field.set(obj, val, callerID);
		}

		[DebuggerStepThroughAttribute]
		public static void DynamicPutstatic(object val, string name, string sig, RuntimeTypeHandle type, string clazz, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicPutstatic");
			FieldWrapper fw = GetFieldWrapper(null, type, clazz, name, sig, true);
			if(fw.IsFinal)
			{
				throw new java.lang.IllegalAccessError("Field " + fw.DeclaringType.Name + "." + fw.Name + " is final");
			}
			java.lang.reflect.Field field = (java.lang.reflect.Field)fw.ToField(false);
			if(fw.FieldTypeWrapper.IsPrimitive)
			{
				val = JVM.Box(val);
			}
			field.set(null, val, callerID);
		}

		// the sole purpose of this method is to check whether the clazz can be instantiated (but not to actually do it)
		[DebuggerStepThroughAttribute]
		public static void DynamicNewCheckOnly(RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicNewCheckOnly");
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			if(wrapper.IsAbstract)
			{
				throw new java.lang.InstantiationError(clazz);
			}
			wrapper.RunClassInit();
		}

		private static TypeWrapper LoadTypeWrapper(RuntimeTypeHandle type, string clazz)
		{
			try
			{
				TypeWrapper context = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
				TypeWrapper wrapper = context.GetClassLoader().LoadClassByDottedNameFast(clazz);
				if(wrapper == null)
				{
					throw new java.lang.NoClassDefFoundError(clazz);
				}
				if(!wrapper.IsAccessibleFrom(context))
				{
					throw new java.lang.IllegalAccessError("Try to access class " + wrapper.Name + " from class " + context.Name);
				}
				wrapper.Finish();
				return wrapper;
			}
			catch(RetargetableJavaException x)
			{
				throw x.ToJava();
			}
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicClassLiteral(RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicClassLiteral");
			return LoadTypeWrapper(type, clazz).ClassObject;
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicCast(object obj, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicCast");
			// NOTE it's important that we don't try to load the class if obj == null
			// (to be compatible with Sun)
			if(obj != null && !DynamicInstanceOf(obj, type, clazz))
			{
				throw new java.lang.ClassCastException(NativeCode.ikvm.runtime.Util.GetTypeWrapperFromObject(obj).Name);
			}
			return obj;
		}

		[DebuggerStepThroughAttribute]
		public static bool DynamicInstanceOf(object obj, RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicInstanceOf");
			// NOTE it's important that we don't try to load the class if obj == null
			// (to be compatible with Sun)
			if(obj == null)
			{
				return false;
			}
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			return wrapper.IsInstance(obj);
		}

		private static MethodWrapper GetMethodWrapper(object thisObj, RuntimeTypeHandle type, string clazz, string name, string sig, bool isStatic)
		{
			TypeWrapper caller = ClassLoaderWrapper.GetWrapperFromType(Type.GetTypeFromHandle(type));
			TypeWrapper wrapper = LoadTypeWrapper(type, clazz);
			MethodWrapper mw = wrapper.GetMethodWrapper(name, sig, false);
			if(mw == null)
			{
				throw new java.lang.NoSuchMethodError(clazz + "." + name + sig);
			}
			// TODO check loader constraints
			if(mw.IsStatic != isStatic)
			{
				throw new java.lang.IncompatibleClassChangeError(clazz + "." + name);
			}
			TypeWrapper objType = null;
			if(thisObj != null)
			{
				objType = ClassLoaderWrapper.GetWrapperFromType(thisObj.GetType());
			}
			if(mw.IsAccessibleFrom(wrapper, caller, objType))
			{
				return mw;
			}
			throw new java.lang.IllegalAccessError(clazz + "." + name + sig);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokeSpecialNew(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicInvokeSpecialNew");
			MethodWrapper mw = GetMethodWrapper(null, type, clazz, name, sig, false);
			java.lang.reflect.Constructor cons = (java.lang.reflect.Constructor)mw.ToMethodOrConstructor(false);
			return cons.newInstance(BoxArgs(mw, args), callerID);
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokestatic(RuntimeTypeHandle type, string clazz, string name, string sig, object[] args, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicInvokestatic");
			MethodWrapper mw = GetMethodWrapper(null, type, clazz, name, sig, true);
			java.lang.reflect.Method m = (java.lang.reflect.Method)mw.ToMethodOrConstructor(false);
			object val = m.invoke(null, BoxArgs(mw, args), callerID);
			if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
			{
				val = JVM.Unbox(val);
			}
			return val;
		}

		[DebuggerStepThroughAttribute]
		public static object DynamicInvokevirtual(object obj, RuntimeTypeHandle type, string clazz, string name, string sig, object[] args, ikvm.@internal.CallerID callerID)
		{
			Profiler.Count("DynamicInvokevirtual");
			MethodWrapper mw = GetMethodWrapper(obj, type, clazz, name, sig, false);
			java.lang.reflect.Method m = (java.lang.reflect.Method)mw.ToMethodOrConstructor(false);
			object val = m.invoke(obj, BoxArgs(mw, args), callerID);
			if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
			{
				val = JVM.Unbox(val);
			}
			return val;
		}

		private static object[] BoxArgs(MethodWrapper mw, object[] args)
		{
			TypeWrapper[] paramTypes = mw.GetParameters();
			for (int i = 0; i < paramTypes.Length; i++)
			{
				if (paramTypes[i].IsPrimitive)
				{
					args[i] = JVM.Box(args[i]);
				}
			}
			return args;
		}

		[DebuggerStepThroughAttribute]
		public static Type DynamicGetTypeAsExceptionType(RuntimeTypeHandle type, string clazz)
		{
			Profiler.Count("DynamicGetTypeAsExceptionType");
			return LoadTypeWrapper(type, clazz).TypeAsExceptionType;
		}

		[DebuggerStepThrough]
		public static Delegate DynamicCreateDelegate(object obj, Type delegateType, string name, string sig)
		{
			TypeWrapper tw = TypeWrapper.FromClass(ikvm.runtime.Util.getClassFromObject(obj));
			MethodWrapper mw = tw.GetMethodWrapper(name, sig, true);
			if (mw == null || mw.IsStatic || !mw.IsPublic)
			{
				MethodInfo invoke = delegateType.GetMethod("Invoke");
				ParameterInfo[] parameters = invoke.GetParameters();
				Type[] parameterTypes = new Type[parameters.Length + 1];
				parameterTypes[0] = typeof(object);
				for (int i = 0; i < parameters.Length; i++)
				{
					parameterTypes[i + 1] = parameters[i].ParameterType;
				}
				System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod("Invoke", invoke.ReturnType, parameterTypes);
				CodeEmitter ilgen = CodeEmitter.Create(dm);
				ilgen.Emit(System.Reflection.Emit.OpCodes.Ldstr, tw.Name + ".Invoke" + sig);
				ClassLoaderWrapper.GetBootstrapClassLoader()
					.LoadClassByDottedName(mw == null || mw.IsStatic ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError")
					.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false)
					.EmitNewobj(ilgen);
				ilgen.Emit(System.Reflection.Emit.OpCodes.Throw);
				ilgen.DoEmit();
				return dm.CreateDelegate(delegateType, obj);
			}
			else
			{
				mw.ResolveMethod();
				return Delegate.CreateDelegate(delegateType, obj, (MethodInfo)mw.GetMethod());
			}
		}
#else
		[DebuggerStepThroughAttribute]
		public static object DynamicCast(object obj, RuntimeTypeHandle type, string clazz)
		{
			return null;
		}

		[DebuggerStepThroughAttribute]
		public static bool DynamicInstanceOf(object obj, RuntimeTypeHandle type, string clazz)
		{
			return false;
		}

		[DebuggerStepThroughAttribute]
		public static Type DynamicGetTypeAsExceptionType(RuntimeTypeHandle type, string clazz)
		{
			return null;
		}

		[DebuggerStepThrough]
		public static Delegate DynamicCreateDelegate(object obj, Type delegateType)
		{
			return null;
		}
#endif //!FIRST_PASS

		[DebuggerStepThroughAttribute]
		public static int f2i(float f)
		{
			if(f <= int.MinValue)
			{
				return int.MinValue;
			}
			if(f >= int.MaxValue)
			{
				return int.MaxValue;
			}
			if(float.IsNaN(f))
			{
				return 0;
			}
			return (int)f;
		}

		[DebuggerStepThroughAttribute]
		public static long f2l(float f)
		{
			if(f <= long.MinValue)
			{
				return long.MinValue;
			}
			if(f >= long.MaxValue)
			{
				return long.MaxValue;
			}
			if(float.IsNaN(f))
			{
				return 0;
			}
			return (long)f;
		}

		[DebuggerStepThroughAttribute]
		public static int d2i(double d)
		{
			if(d <= int.MinValue)
			{
				return int.MinValue;
			}
			if(d >= int.MaxValue)
			{
				return int.MaxValue;
			}
			if(double.IsNaN(d))
			{
				return 0;
			}
			return (int)d;
		}

		[DebuggerStepThroughAttribute]
		public static long d2l(double d)
		{
			if(d <= long.MinValue)
			{
				return long.MinValue;
			}
			if(d >= long.MaxValue)
			{
				return long.MaxValue;
			}
			if(double.IsNaN(d))
			{
				return 0;
			}
			return (long)d;
		}

		// This is used by static JNI and synchronized methods that need a class object
		[DebuggerStepThroughAttribute]
		public static object GetClassFromTypeHandle(RuntimeTypeHandle typeHandle)
		{
			return NativeCode.ikvm.runtime.Util.getClassFromTypeHandle(typeHandle);
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
		{
#if !FIRST_PASS
			// If the two arrays are the same, we can use the fast path, but we're also required to do so,
			// to get the required memmove semantics.
			if(src == dest)
			{
				try
				{
					arraycopy_fast((Array)src, srcStart, (Array)dest, destStart, len);
					return;
				}
				catch(InvalidCastException)
				{
					throw new java.lang.ArrayStoreException();
				}
			}
			else if(src == null || dest == null)
			{
				throw new java.lang.NullPointerException();
			}
			else if(len < 0)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			else
			{
				object[] src1 = src as object[];
				object[] dst1 = dest as object[];
				if(src1 != null && dst1 != null)
				{
					// for small copies, don't bother comparing the types as this is relatively expensive
					if(len > 50 && src.GetType() == dest.GetType())
					{
						arraycopy_fast(src1, srcStart, dst1, destStart, len);
						return;
					}
					else
					{
						for(; len > 0; len--)
						{
							// NOTE we don't need to catch ArrayTypeMismatchException & IndexOutOfRangeException, because
							// they automatically get converted to the Java equivalents anyway.
							dst1[destStart++] = src1[srcStart++];
						}
						return;
					}
				}
				else if(src.GetType() != dest.GetType() &&
						(IsPrimitiveArrayType(src.GetType()) || IsPrimitiveArrayType(dest.GetType())))
				{
					// we don't want to allow copying a primitive into an object array!
					throw new java.lang.ArrayStoreException();
				}
				else
				{
					try
					{
						arraycopy_fast((Array)src, srcStart, (Array)dest, destStart, len);
						return;
					}
					catch(InvalidCastException)
					{
						throw new java.lang.ArrayStoreException();
					}
				}
			}
#endif // !FIRST_PASS
		}

		private static bool IsPrimitiveArrayType(Type type)
		{
			return type.IsArray && ClassLoaderWrapper.GetWrapperFromType(type.GetElementType()).IsPrimitive;
		}
		
		[DebuggerStepThroughAttribute]
		public static void arraycopy_fast(Array src, int srcStart, Array dest, int destStart, int len)
		{
#if !FIRST_PASS
			try 
			{
				Array.Copy(src, srcStart, dest, destStart, len);
			}
			catch(ArgumentNullException)
			{
				throw new java.lang.NullPointerException();
			}
			catch(ArgumentException) 
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
#endif // !FIRST_PASS
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_8(Array src, int srcStart, Array dest, int destStart, int len)
		{
#if !FIRST_PASS
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart * 8, dest, destStart * 8, len * 8);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw new java.lang.NullPointerException();
			}
			catch(OverflowException)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
#endif // !FIRST_PASS
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_4(Array src, int srcStart, Array dest, int destStart, int len)
		{
#if !FIRST_PASS
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart * 4, dest, destStart * 4, len * 4);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw new java.lang.NullPointerException();
			}
			catch(OverflowException)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
#endif // !FIRST_PASS
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_2(Array src, int srcStart, Array dest, int destStart, int len)
		{
#if !FIRST_PASS
			try 
			{
				checked
				{
					Buffer.BlockCopy(src, srcStart * 2, dest, destStart * 2, len * 2);
					return;
				}
			}
			catch(ArgumentNullException)
			{
				throw new java.lang.NullPointerException();
			}
			catch(OverflowException)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
#endif // !FIRST_PASS
		}

		[DebuggerStepThroughAttribute]
		public static void arraycopy_primitive_1(Array src, int srcStart, Array dest, int destStart, int len)
		{
#if !FIRST_PASS
			try 
			{
				Buffer.BlockCopy(src, srcStart, dest, destStart, len);
				return;
			}
			catch(ArgumentNullException)
			{
				throw new java.lang.NullPointerException();
			}
			catch(OverflowException)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			catch(ArgumentException) 
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
#endif // !FIRST_PASS
		}

		[HideFromJava]
		public static void VerboseCastFailure(RuntimeTypeHandle typeHandle, object obj)
		{
#if !FIRST_PASS
			Type t1 = obj.GetType();
			Type t2 = Type.GetTypeFromHandle(typeHandle);
			string msg;
			if(t1.Assembly.FullName == t2.Assembly.FullName && t1.Assembly.Location != t2.Assembly.Location)
			{
				string l1 = t1.Assembly.Location;
				string l2 = t2.Assembly.Location;
				if(l1 == "")
				{
					l1 = "unknown location";
				}
				if(l2 == "")
				{
					l2 = "unknown location";
				}
				msg = String.Format("Object of type \"{0}\" loaded from {1} cannot be cast to \"{2}\" loaded from {3}", t1.AssemblyQualifiedName, l1, t2.AssemblyQualifiedName, l2);
			}
			else
			{
				msg = String.Format("Object of type \"{0}\" cannot be cast to \"{1}\"", t1.AssemblyQualifiedName, t2.AssemblyQualifiedName);
			}
			throw new java.lang.ClassCastException(msg);
#endif // !FIRST_PASS
		}

		public static bool SkipFinalizer()
		{
#if FIRST_PASS
			return false;
#else
			return Environment.HasShutdownStarted && !java.lang.Shutdown.runFinalizersOnExit;
#endif
		}

		public static long VolatileRead(ref long v)
		{
			return Interlocked.Read(ref v);
		}

		public static void VolatileWrite(ref long v, long newValue)
		{
			Interlocked.Exchange(ref v, newValue);
		}

		public static double VolatileRead(ref double v)
		{
			return Interlocked.CompareExchange(ref v, 0.0, 0.0);
		}

		public static void VolatileWrite(ref double v, double newValue)
		{
			Interlocked.Exchange(ref v, newValue);
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		public static void InitializeModule(Module module)
		{
			Assembly asm = Assembly.GetCallingAssembly();
			if (module.Assembly != asm)
			{
				throw new ArgumentOutOfRangeException();
			}
			object classLoader = AssemblyClassLoader.FromAssembly(asm).GetJavaClassLoader();
			Action<Module> init = (Action<Module>)Delegate.CreateDelegate(typeof(Action<Module>), classLoader, "InitializeModule", false, false);
			if (init != null)
			{
				init(module);
			}
		}

		public static T GetDotNetEnumField<T>(string name)
#if !FIRST_PASS
			where T : java.lang.Enum
#endif
		{
#if FIRST_PASS
			return default(T);
#else
			try
			{
				return (T)java.lang.Enum.valueOf(ikvm.@internal.ClassLiteral<T>.Value, name);
			}
			catch (java.lang.IllegalArgumentException)
			{
				throw new java.lang.NoSuchFieldError(ikvm.@internal.ClassLiteral<T>.Value.getName() + "." + name);
			}
#endif
		}

		[Flags]
		public enum MapFlags
		{
			None = 0,
			NoRemapping = 1,
			Unused = 2,
		}

		[HideFromJava]
		public static T MapException<T>(Exception x, MapFlags mode) where T : Exception
		{
			return ExceptionHelper.MapException<T>(x, (mode & MapFlags.NoRemapping) == 0, (mode & MapFlags.Unused) != 0);
		}

		public static T GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandle h)
			where T : class
		{
#if FIRST_PASS
			return null;
#else
			T del = h.vmtarget as T;
			if (del == null)
			{
				throw new global::java.lang.invoke.WrongMethodTypeException();
			}
			return del;
#endif
		}

		public static T GetDelegateForInvoke<T>(global::java.lang.invoke.MethodHandle h, ref InvokeCache<T> cache)
			where T : class
		{
#if FIRST_PASS
			return null;
#else
			if (cache.type == h.type() && cache.del != null)
			{
				return cache.del;
			}
			T del = h.vmtarget as T;
			if (del == null)
			{
				global::java.lang.invoke.MethodHandle adapter = global::java.lang.invoke.MethodHandles.exactInvoker(h.type());
				if (h.isVarargsCollector())
				{
					adapter = adapter.asVarargsCollector(h.type().parameterType(h.type().parameterCount() - 1));
				}
				del = (T)adapter.asType(MethodHandleUtil.GetDelegateMethodType(typeof(T))).vmtarget;
				if (Interlocked.CompareExchange(ref cache.type, h.type(), null) == null)
				{
					cache.del = del;
				}
			}
			return del;
#endif
		}

		public static java.lang.invoke.MethodType LoadMethodType<T>()
			where T : class // Delegate
		{
#if FIRST_PASS
			return null;
#else
			return MethodHandleUtil.GetDelegateMethodType(typeof(T));
#endif
		}

#if !FIRST_PASS
		sealed class ConstantMethodHandle : java.lang.invoke.MethodHandle
		{
			internal ConstantMethodHandle(Delegate del)
				: base(MethodHandleUtil.GetDelegateMethodType(del.GetType()))
			{
				this.vmtarget = del;
			}
		}
#endif

		public static java.lang.invoke.MethodHandle MethodHandleFromDelegate(Delegate del)
		{
#if FIRST_PASS
			return null;
#else
			return new ConstantMethodHandle(del);
#endif
		}

		[HideFromJava]
		public static void LinkIndyCallSite<T>(ref IndyCallSite<T> site, java.lang.invoke.CallSite cs, Exception x)
			where T : class // Delegate
		{
#if !FIRST_PASS
			IndyCallSite<T> ics;
			if (x != null || cs == null || (ics = cs.ics as IndyCallSite<T>) == null)
			{
				x = MapException<Exception>(x ?? (cs == null
					? (Exception)new java.lang.ClassCastException("bootstrap method failed to produce a CallSite")
					: new java.lang.invoke.WrongMethodTypeException()), MapFlags.None);
				java.lang.invoke.MethodType type = LoadMethodType<T>();
				ics = new IndyCallSite<T>((T)
					java.lang.invoke.MethodHandles.dropArguments(
						java.lang.invoke.MethodHandles.foldArguments(
							java.lang.invoke.MethodHandles.throwException(type.returnType(), typeof(java.lang.BootstrapMethodError)),
								new ConstantMethodHandle((MH<Exception, java.lang.BootstrapMethodError>)CreateBootstrapException).bindTo(x)),
						0, type.parameterArray())
					.vmtarget,
					false);
			}
			IndyCallSite<T> curr = site;
			if (curr.IsBootstrap)
			{
				Interlocked.CompareExchange(ref site, ics, curr);
			}
#endif
		}

#if !FIRST_PASS
		[HideFromJava]
		private static java.lang.BootstrapMethodError CreateBootstrapException(Exception x)
		{
			if (x is java.lang.BootstrapMethodError)
			{
				return (java.lang.BootstrapMethodError)x;
			}
			return new java.lang.BootstrapMethodError("call site initialization exception", x);
		}
#endif
	}

	public sealed class IndyCallSite<T>
#if !FIRST_PASS
		: java.lang.invoke.CallSite.IndyCallSite
#endif
		where T : class // Delegate
	{
		internal readonly bool IsBootstrap;
		private volatile T target;

		internal IndyCallSite()
		{
		}

		internal IndyCallSite(T target, bool bootstrap)
		{
			this.IsBootstrap = bootstrap;
			this.target = target;
		}

#if !FIRST_PASS
		void java.lang.invoke.CallSite.IndyCallSite.setTarget(object target)
		{
			this.target = (T)target;
		}
#endif

		public static IndyCallSite<T> CreateBootstrap(T bootstrap)
		{
			return new IndyCallSite<T>(bootstrap, true);
		}

		public T GetTarget()
		{
			return target;
		}
	}

	public struct InvokeCache<T>
		where T : class
	{
		internal global::java.lang.invoke.MethodType type;
		internal T del;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct DoubleConverter
	{
		[FieldOffset(0)]
		private double d;
		[FieldOffset(0)]
		private long l;

		public static long ToLong(double value, ref DoubleConverter converter)
		{
			converter.d = value;
			return converter.l;
		}

		public static double ToDouble(long value, ref DoubleConverter converter)
		{
			converter.l = value;
			return converter.d;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct FloatConverter
	{
		[FieldOffset(0)]
		private float f;
		[FieldOffset(0)]
		private int i;

		public static int ToInt(float value, ref FloatConverter converter)
		{
			converter.f = value;
			return converter.i;
		}

		public static float ToFloat(int value, ref FloatConverter converter)
		{
			converter.i = value;
			return converter.f;
		}
	}

	public struct MHA<T1, T2, T3, T4, T5, T6, T7, T8>
	{
		public T1 t1;
		public T2 t2;
		public T3 t3;
		public T4 t4;
		public T5 t5;
		public T6 t6;
		public T7 t7;
		public T8 t8;

		public MHA(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
		{
			this.t1 = t1;
			this.t2 = t2;
			this.t3 = t3;
			this.t4 = t4;
			this.t5 = t5;
			this.t6 = t6;
			this.t7 = t7;
			this.t8 = t8;
		}
	}

	public delegate void MHV();
	public delegate void MHV<T1>(T1 t1);
	public delegate void MHV<T1, T2>(T1 t1, T2 t2);
	public delegate void MHV<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
	public delegate void MHV<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
	public delegate void MHV<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
	public delegate void MHV<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
	public delegate void MHV<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
	public delegate void MHV<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

	public delegate TResult MH<TResult>();
	public delegate TResult MH<T1, TResult>(T1 t1);
	public delegate TResult MH<T1, T2, TResult>(T1 t1, T2 t2);
	public delegate TResult MH<T1, T2, T3, TResult>(T1 t1, T2 t2, T3 t3);
	public delegate TResult MH<T1, T2, T3, T4, TResult>(T1 t1, T2 t2, T3 t3, T4 t4);
	public delegate TResult MH<T1, T2, T3, T4, T5, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
	public delegate TResult MH<T1, T2, T3, T4, T5, T6, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
	public delegate TResult MH<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
	public delegate TResult MH<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
}
