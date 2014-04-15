/*
  Copyright (C) 2011 Jeroen Frijters

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
using System.Collections.Generic;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;

namespace IKVM.Internal
{
	static class ProxyGenerator
	{
		private static readonly TypeWrapper proxyClass;
		private static readonly TypeWrapper errorClass;
		private static readonly TypeWrapper runtimeExceptionClass;
		private static readonly MethodWrapper undeclaredThrowableExceptionConstructor;
		private static readonly FieldWrapper invocationHandlerField;
		private static readonly TypeWrapper javaLangReflectMethod;
		private static readonly TypeWrapper javaLangNoSuchMethodException;
		private static readonly MethodWrapper javaLangNoClassDefFoundErrorConstructor;
		private static readonly MethodWrapper javaLangThrowable_getMessage;
		private static readonly MethodWrapper javaLangClass_getMethod;
		private static readonly TypeWrapper invocationHandlerClass;
		private static readonly MethodWrapper invokeMethod;
		private static readonly MethodWrapper proxyConstructor;
		private static readonly MethodWrapper hashCodeMethod;
		private static readonly MethodWrapper equalsMethod;
		private static readonly MethodWrapper toStringMethod;

		static ProxyGenerator()
		{
			ClassLoaderWrapper bootClassLoader = ClassLoaderWrapper.GetBootstrapClassLoader();
			proxyClass = bootClassLoader.LoadClassByDottedNameFast("java.lang.reflect.Proxy");
			errorClass = bootClassLoader.LoadClassByDottedNameFast("java.lang.Error");
			runtimeExceptionClass = bootClassLoader.LoadClassByDottedNameFast("java.lang.RuntimeException");
			undeclaredThrowableExceptionConstructor = bootClassLoader.LoadClassByDottedNameFast("java.lang.reflect.UndeclaredThrowableException").GetMethodWrapper("<init>", "(Ljava.lang.Throwable;)V", false);
			undeclaredThrowableExceptionConstructor.Link();
			invocationHandlerField = proxyClass.GetFieldWrapper("h", "Ljava.lang.reflect.InvocationHandler;");
			invocationHandlerField.Link();
			javaLangReflectMethod = bootClassLoader.LoadClassByDottedNameFast("java.lang.reflect.Method");
			javaLangNoSuchMethodException = bootClassLoader.LoadClassByDottedNameFast("java.lang.NoSuchMethodException");
			javaLangNoClassDefFoundErrorConstructor = bootClassLoader.LoadClassByDottedNameFast("java.lang.NoClassDefFoundError").GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
			javaLangNoClassDefFoundErrorConstructor.Link();
			javaLangThrowable_getMessage = bootClassLoader.LoadClassByDottedNameFast("java.lang.Throwable").GetMethodWrapper("getMessage", "()Ljava.lang.String;", false);
			javaLangThrowable_getMessage.Link();
			javaLangClass_getMethod = CoreClasses.java.lang.Class.Wrapper.GetMethodWrapper("getMethod", "(Ljava.lang.String;[Ljava.lang.Class;)Ljava.lang.reflect.Method;", false);
			javaLangClass_getMethod.Link();
			invocationHandlerClass = bootClassLoader.LoadClassByDottedNameFast("java.lang.reflect.InvocationHandler");
			invokeMethod = invocationHandlerClass.GetMethodWrapper("invoke", "(Ljava.lang.Object;Ljava.lang.reflect.Method;[Ljava.lang.Object;)Ljava.lang.Object;", false);
			proxyConstructor = proxyClass.GetMethodWrapper("<init>", "(Ljava.lang.reflect.InvocationHandler;)V", false);
			proxyConstructor.Link();
			hashCodeMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("hashCode", "()I", false);
			equalsMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("equals", "(Ljava.lang.Object;)Z", false);
			toStringMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("toString", "()Ljava.lang.String;", false);
		}

		internal static void Create(CompilerClassLoader loader, string proxy)
		{
			string[] interfaces = proxy.Split(',');
			TypeWrapper[] wrappers = new TypeWrapper[interfaces.Length];
			for (int i = 0; i < interfaces.Length; i++)
			{
				try
				{
					wrappers[i] = loader.LoadClassByDottedNameFast(interfaces[i]);
				}
				catch (RetargetableJavaException)
				{
				}
				if (wrappers[i] == null)
				{
					StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, "unable to load interface " + interfaces[i]);
					return;
				}
			}
			Create(loader, proxy, wrappers);
		}

		private static void Create(CompilerClassLoader loader, string proxy, TypeWrapper[] interfaces)
		{
			List<ProxyMethod> methods;
			try
			{
				methods = CheckAndCollect(loader, interfaces);
			}
			catch (RetargetableJavaException x)
			{
				StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, x.Message);
				return;
			}
			catch (ProxyException x)
			{
				StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, x.Message);
				return;
			}
			CreateNoFail(loader, interfaces, methods);
		}

		private static List<ProxyMethod> CheckAndCollect(CompilerClassLoader loader, TypeWrapper[] interfaces)
		{
			List<MethodWrapper> methods = new List<MethodWrapper>();

			// The java.lang.Object methods precede any interface methods.
			methods.Add(equalsMethod);
			methods.Add(hashCodeMethod);
			methods.Add(toStringMethod);

			// Add the interfaces methods in order.
			foreach (TypeWrapper tw in interfaces)
			{
				if (!tw.IsInterface)
				{
					throw new ProxyException(tw.Name + " is not an interface");
				}
				if (tw.IsRemapped)
				{
					// TODO handle java.lang.Comparable
					throw new ProxyException(tw.Name + " is a remapped interface (not currently supported)");
				}
				foreach (MethodWrapper mw in GetInterfaceMethods(tw))
				{
					// Check for duplicates
					if (!MethodExists(methods, mw))
					{
						methods.Add(mw);
					}
				}
			}

			// TODO verify restrictions

			// Collect declared exceptions.
			Dictionary<string, TypeWrapper[]> exceptions = new Dictionary<string, TypeWrapper[]>();
			foreach (MethodWrapper mw in methods)
			{
				Add(loader, exceptions, mw);
			}

			// Build the definitive proxy method list.
			List<ProxyMethod> proxyMethods = new List<ProxyMethod>();
			foreach (MethodWrapper mw in methods)
			{
				proxyMethods.Add(new ProxyMethod(mw, exceptions[mw.Signature]));
			}
			return proxyMethods;
		}

		private static bool MethodExists(List<MethodWrapper> methods, MethodWrapper mw)
		{
			foreach (MethodWrapper mw1 in methods)
			{
				// TODO what do we do with differing return types?
				if (mw1.Name == mw.Name && mw1.Signature == mw.Signature)
				{
					return true;
				}
			}
			return false;
		}

		private static void Add(CompilerClassLoader loader, Dictionary<string, TypeWrapper[]> exceptions, MethodWrapper mw)
		{
			string signature = mw.Signature;
			TypeWrapper[] newExceptionTypes = LoadTypes(loader, mw.GetDeclaredExceptions());
			TypeWrapper[] curExceptionTypes;
			if (exceptions.TryGetValue(signature, out curExceptionTypes))
			{
				exceptions[signature] = Merge(newExceptionTypes, curExceptionTypes);
			}
			else
			{
				exceptions.Add(signature, newExceptionTypes);
			}
		}

		private static TypeWrapper[] Merge(TypeWrapper[] newExceptionTypes, TypeWrapper[] curExceptionTypes)
		{
			List<TypeWrapper> list = new List<TypeWrapper>();
			foreach (TypeWrapper twNew in newExceptionTypes)
			{
				TypeWrapper match = null;
				foreach (TypeWrapper twCur in curExceptionTypes)
				{
					if (twNew.IsAssignableTo(twCur))
					{
						if (match == null || twCur.IsAssignableTo(match))
						{
							match = twCur;
						}
					}
				}
				if (match != null && !list.Contains(match))
				{
					list.Add(match);
				}
			}
			return list.ToArray();
		}

		private static void CreateNoFail(CompilerClassLoader loader, TypeWrapper[] interfaces, List<ProxyMethod> methods)
		{
			DynamicClassLoader factory = (DynamicClassLoader)loader.GetTypeWrapperFactory();
			TypeBuilder tb = factory.DefineProxy(proxyClass, interfaces);
			AttributeHelper.SetImplementsAttribute(tb, interfaces);
			CreateConstructor(tb);
			for (int i = 0; i < methods.Count; i++)
			{
				methods[i].fb = tb.DefineField("m" + i, javaLangReflectMethod.TypeAsSignatureType, FieldAttributes.Private | FieldAttributes.Static);
			}
			foreach (ProxyMethod method in methods)
			{
				CreateMethod(loader, tb, method);
			}
			CreateStaticInitializer(tb, methods);
		}

		private static void CreateConstructor(TypeBuilder tb)
		{
			ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { invocationHandlerClass.TypeAsSignatureType });
			CodeEmitter ilgen = CodeEmitter.Create(cb);
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldarg_1);
			proxyConstructor.EmitCall(ilgen);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
		}

		private static void CreateMethod(CompilerClassLoader loader, TypeBuilder tb, ProxyMethod pm)
		{
			MethodBuilder mb = pm.mw.GetDefineMethodHelper().DefineMethod(loader.GetTypeWrapperFactory(), tb, pm.mw.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final);
			List<string> exceptions = new List<string>();
			foreach (TypeWrapper tw in pm.exceptions)
			{
				exceptions.Add(tw.Name);
			}
			AttributeHelper.SetThrowsAttribute(mb, exceptions.ToArray());
			CodeEmitter ilgen = CodeEmitter.Create(mb);
			ilgen.BeginExceptionBlock();
			ilgen.Emit(OpCodes.Ldarg_0);
			invocationHandlerField.EmitGet(ilgen);
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldsfld, pm.fb);
			TypeWrapper[] parameters = pm.mw.GetParameters();
			if (parameters.Length == 0)
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
			else
			{
				ilgen.Emit_Ldc_I4(parameters.Length);
				ilgen.Emit(OpCodes.Newarr, Types.Object);
				for (int i = 0; i < parameters.Length; i++)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit_Ldc_I4(i);
					ilgen.Emit(OpCodes.Ldarg, (short)i);
					if (parameters[i].IsNonPrimitiveValueType)
					{
						parameters[i].EmitBox(ilgen);
					}
					else if (parameters[i].IsPrimitive)
					{
						Boxer.EmitBox(ilgen, parameters[i]);
					}
					ilgen.Emit(OpCodes.Stelem_Ref);
				}
			}
			invokeMethod.EmitCallvirt(ilgen);
			TypeWrapper returnType = pm.mw.ReturnType;
			CodeEmitterLocal returnValue = null;
			if (returnType != PrimitiveTypeWrapper.VOID)
			{
				returnValue = ilgen.DeclareLocal(returnType.TypeAsSignatureType);
				if (returnType.IsNonPrimitiveValueType)
				{
					returnType.EmitUnbox(ilgen);
				}
				else if (returnType.IsPrimitive)
				{
					Boxer.EmitUnbox(ilgen, returnType);
				}
				else if (returnType != CoreClasses.java.lang.Object.Wrapper)
				{
					ilgen.EmitCastclass(returnType.TypeAsSignatureType);
				}
				ilgen.Emit(OpCodes.Stloc, returnValue);
			}
			CodeEmitterLabel returnLabel = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Leave, returnLabel);
			// TODO consider using a filter here (but we would need to add filter support to CodeEmitter)
			ilgen.BeginCatchBlock(Types.Exception);
			ilgen.Emit_Ldc_I4(0);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
			CodeEmitterLocal exception = ilgen.DeclareLocal(Types.Exception);
			ilgen.Emit(OpCodes.Stloc, exception);
			CodeEmitterLabel rethrow = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Ldloc, exception);
			errorClass.EmitInstanceOf(null, ilgen);
			ilgen.Emit(OpCodes.Brtrue, rethrow);
			ilgen.Emit(OpCodes.Ldloc, exception);
			runtimeExceptionClass.EmitInstanceOf(null, ilgen);
			ilgen.Emit(OpCodes.Brtrue, rethrow);
			foreach (TypeWrapper tw in pm.exceptions)
			{
				ilgen.Emit(OpCodes.Ldloc, exception);
				tw.EmitInstanceOf(null, ilgen);
				ilgen.Emit(OpCodes.Brtrue, rethrow);
			}
			ilgen.Emit(OpCodes.Ldloc, exception);
			undeclaredThrowableExceptionConstructor.EmitNewobj(ilgen);
			ilgen.Emit(OpCodes.Throw);
			ilgen.MarkLabel(rethrow);
			ilgen.Emit(OpCodes.Rethrow);
			ilgen.EndExceptionBlock();
			ilgen.MarkLabel(returnLabel);
			if (returnValue != null)
			{
				ilgen.Emit(OpCodes.Ldloc, returnValue);
			}
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
		}

		private static void CreateStaticInitializer(TypeBuilder tb, List<ProxyMethod> methods)
		{
			ConstructorBuilder cb = tb.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
			CodeEmitter ilgen = CodeEmitter.Create(cb);
			CodeEmitterLocal callerID = ilgen.DeclareLocal(CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType);
			TypeBuilder tbCallerID = DynamicTypeWrapper.FinishContext.EmitCreateCallerID(tb, ilgen);
			ilgen.Emit(OpCodes.Stloc, callerID);
			// HACK we shouldn't create the nested type here (the outer type must be created first)
			tbCallerID.CreateType();
			ilgen.BeginExceptionBlock();
			foreach (ProxyMethod method in methods)
			{
				method.mw.DeclaringType.EmitClassLiteral(ilgen);
				ilgen.Emit(OpCodes.Ldstr, method.mw.Name);
				TypeWrapper[] parameters = method.mw.GetParameters();
				ilgen.Emit(OpCodes.Ldc_I4, parameters.Length);
				ilgen.Emit(OpCodes.Newarr, CoreClasses.java.lang.Class.Wrapper.TypeAsArrayType);
				for (int i = 0; i < parameters.Length; i++)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldc_I4, i);
					parameters[i].EmitClassLiteral(ilgen);
					ilgen.Emit(OpCodes.Stelem_Ref);
				}
				if (javaLangClass_getMethod.HasCallerID)
				{
					ilgen.Emit(OpCodes.Ldloc, callerID);
				}
				javaLangClass_getMethod.EmitCallvirt(ilgen);
				ilgen.Emit(OpCodes.Stsfld, method.fb);
			}
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Leave_S, label);
			ilgen.BeginCatchBlock(javaLangNoSuchMethodException.TypeAsExceptionType);
			javaLangThrowable_getMessage.EmitCallvirt(ilgen);
			javaLangNoClassDefFoundErrorConstructor.EmitNewobj(ilgen);
			ilgen.Emit(OpCodes.Throw);
			ilgen.EndExceptionBlock();
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
		}

		private sealed class ProxyMethod
		{
			internal readonly MethodWrapper mw;
			internal readonly TypeWrapper[] exceptions;
			internal FieldBuilder fb;

			internal ProxyMethod(MethodWrapper mw, TypeWrapper[] exceptions)
			{
				this.mw = mw;
				this.exceptions = exceptions;
			}
		}

		private static IEnumerable<MethodWrapper> GetInterfaceMethods(TypeWrapper tw)
		{
			Dictionary<string, MethodWrapper> methods = new Dictionary<string, MethodWrapper>();
			foreach (MethodWrapper mw in tw.GetMethods())
			{
				methods.Add(mw.Name + mw.Signature, mw);
			}
			foreach (TypeWrapper iface in tw.Interfaces)
			{
				foreach (MethodWrapper mw in GetInterfaceMethods(iface))
				{
					if (!methods.ContainsKey(mw.Name + mw.Signature))
					{
						methods.Add(mw.Name + mw.Signature, mw);
					}
				}
			}
			return methods.Values;
		}

		private static TypeWrapper[] LoadTypes(ClassLoaderWrapper loader, string[] classes)
		{
			if (classes == null || classes.Length == 0)
			{
				return TypeWrapper.EmptyArray;
			}
			TypeWrapper[] tw = new TypeWrapper[classes.Length];
			for (int i = 0; i < tw.Length; i++)
			{
				tw[i] = loader.LoadClassByDottedName(classes[i]);
			}
			return tw;
		}

		private sealed class ProxyException : Exception
		{
			internal ProxyException(string msg)
				: base(msg)
			{
			}
		}
	}

	static class Boxer
	{
		private static readonly TypeWrapper javaLangByte;
		private static readonly MethodWrapper byteValue;
		private static readonly MethodWrapper valueOfByte;
		private static readonly TypeWrapper javaLangBoolean;
		private static readonly MethodWrapper booleanValue;
		private static readonly MethodWrapper valueOfBoolean;
		private static readonly TypeWrapper javaLangShort;
		private static readonly MethodWrapper shortValue;
		private static readonly MethodWrapper valueOfShort;
		private static readonly TypeWrapper javaLangCharacter;
		private static readonly MethodWrapper charValue;
		private static readonly MethodWrapper valueOfCharacter;
		private static readonly TypeWrapper javaLangInteger;
		private static readonly MethodWrapper intValue;
		private static readonly MethodWrapper valueOfInteger;
		private static readonly TypeWrapper javaLangFloat;
		private static readonly MethodWrapper floatValue;
		private static readonly MethodWrapper valueOfFloat;
		private static readonly TypeWrapper javaLangLong;
		private static readonly MethodWrapper longValue;
		private static readonly MethodWrapper valueOfLong;
		private static readonly TypeWrapper javaLangDouble;
		private static readonly MethodWrapper doubleValue;
		private static readonly MethodWrapper valueOfDouble;

		static Boxer()
		{
			ClassLoaderWrapper bootClassLoader = ClassLoaderWrapper.GetBootstrapClassLoader();
			javaLangByte = bootClassLoader.LoadClassByDottedNameFast("java.lang.Byte");
			byteValue = javaLangByte.GetMethodWrapper("byteValue", "()B", false);
			byteValue.Link();
			valueOfByte = javaLangByte.GetMethodWrapper("valueOf", "(B)Ljava.lang.Byte;", false);
			valueOfByte.Link();
			javaLangBoolean = bootClassLoader.LoadClassByDottedNameFast("java.lang.Boolean");
			booleanValue = javaLangBoolean.GetMethodWrapper("booleanValue", "()Z", false);
			booleanValue.Link();
			valueOfBoolean = javaLangBoolean.GetMethodWrapper("valueOf", "(Z)Ljava.lang.Boolean;", false);
			valueOfBoolean.Link();
			javaLangShort = bootClassLoader.LoadClassByDottedNameFast("java.lang.Short");
			shortValue = javaLangShort.GetMethodWrapper("shortValue", "()S", false);
			shortValue.Link();
			valueOfShort = javaLangShort.GetMethodWrapper("valueOf", "(S)Ljava.lang.Short;", false);
			valueOfShort.Link();
			javaLangCharacter = bootClassLoader.LoadClassByDottedNameFast("java.lang.Character");
			charValue = javaLangCharacter.GetMethodWrapper("charValue", "()C", false);
			charValue.Link();
			valueOfCharacter = javaLangCharacter.GetMethodWrapper("valueOf", "(C)Ljava.lang.Character;", false);
			valueOfCharacter.Link();
			javaLangInteger = bootClassLoader.LoadClassByDottedNameFast("java.lang.Integer");
			intValue = javaLangInteger.GetMethodWrapper("intValue", "()I", false);
			intValue.Link();
			valueOfInteger = javaLangInteger.GetMethodWrapper("valueOf", "(I)Ljava.lang.Integer;", false);
			valueOfInteger.Link();
			javaLangFloat = bootClassLoader.LoadClassByDottedNameFast("java.lang.Float");
			floatValue = javaLangFloat.GetMethodWrapper("floatValue", "()F", false);
			floatValue.Link();
			valueOfFloat = javaLangFloat.GetMethodWrapper("valueOf", "(F)Ljava.lang.Float;", false);
			valueOfFloat.Link();
			javaLangLong = bootClassLoader.LoadClassByDottedNameFast("java.lang.Long");
			longValue = javaLangLong.GetMethodWrapper("longValue", "()J", false);
			longValue.Link();
			valueOfLong = javaLangLong.GetMethodWrapper("valueOf", "(J)Ljava.lang.Long;", false);
			valueOfLong.Link();
			javaLangDouble = bootClassLoader.LoadClassByDottedNameFast("java.lang.Double");
			doubleValue = javaLangDouble.GetMethodWrapper("doubleValue", "()D", false);
			doubleValue.Link();
			valueOfDouble = javaLangDouble.GetMethodWrapper("valueOf", "(D)Ljava.lang.Double;", false);
			valueOfDouble.Link();
		}

		internal static void EmitUnbox(CodeEmitter ilgen, TypeWrapper tw)
		{
			if (tw == PrimitiveTypeWrapper.BYTE)
			{
				javaLangByte.EmitCheckcast(null, ilgen);
				byteValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				javaLangBoolean.EmitCheckcast(null, ilgen);
				booleanValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				javaLangShort.EmitCheckcast(null, ilgen);
				shortValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				javaLangCharacter.EmitCheckcast(null, ilgen);
				charValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				javaLangInteger.EmitCheckcast(null, ilgen);
				intValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				javaLangFloat.EmitCheckcast(null, ilgen);
				floatValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				javaLangLong.EmitCheckcast(null, ilgen);
				longValue.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
			{
				javaLangDouble.EmitCheckcast(null, ilgen);
				doubleValue.EmitCall(ilgen);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		internal static void EmitBox(CodeEmitter ilgen, TypeWrapper tw)
		{
			if (tw == PrimitiveTypeWrapper.BYTE)
			{
				valueOfByte.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				valueOfBoolean.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.SHORT)
			{
				valueOfShort.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.CHAR)
			{
				valueOfCharacter.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.INT)
			{
				valueOfInteger.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.FLOAT)
			{
				valueOfFloat.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.LONG)
			{
				valueOfLong.EmitCall(ilgen);
			}
			else if (tw == PrimitiveTypeWrapper.DOUBLE)
			{
				valueOfDouble.EmitCall(ilgen);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
	}
}
