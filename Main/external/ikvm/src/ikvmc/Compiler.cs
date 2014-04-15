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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Internal;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

sealed class FatalCompilerErrorException : Exception
{
	internal FatalCompilerErrorException(Message id, params object[] args)
		: base(string.Format("fatal error IKVMC{0}: {1}", (int)id, args.Length == 0 ? GetMessage(id) : string.Format(GetMessage(id), args)))
	{
	}

	private static string GetMessage(Message id)
	{
		switch (id)
		{
			case IKVM.Internal.Message.ResponseFileDepthExceeded:
				return "Response file nesting depth exceeded";
			case IKVM.Internal.Message.ErrorReadingFile:
				return "Unable to read file: {0}\n\t({1})";
			case IKVM.Internal.Message.NoTargetsFound:
				return "No targets found";
			case IKVM.Internal.Message.FileFormatLimitationExceeded:
				return "File format limitation exceeded: {0}";
			case IKVM.Internal.Message.CannotSpecifyBothKeyFileAndContainer:
				return "You cannot specify both a key file and container";
			case IKVM.Internal.Message.DelaySignRequiresKey:
				return "You cannot delay sign without a key file or container";
			case IKVM.Internal.Message.InvalidStrongNameKeyPair:
				return "Invalid key {0} specified.\n\t(\"{1}\")";
			case IKVM.Internal.Message.ReferenceNotFound:
				return "Reference not found: {0}";
			case IKVM.Internal.Message.OptionsMustPreceedChildLevels:
				return "You can only specify options before any child levels";
			case IKVM.Internal.Message.UnrecognizedTargetType:
				return "Invalid value '{0}' for -target option";
			case IKVM.Internal.Message.UnrecognizedPlatform:
				return "Invalid value '{0}' for -platform option";
			case IKVM.Internal.Message.UnrecognizedApartment:
				return "Invalid value '{0}' for -apartment option";
			case IKVM.Internal.Message.MissingFileSpecification:
				return "Missing file specification for '{0}' option";
			case IKVM.Internal.Message.PathTooLong:
				return "Path too long: {0}";
			case IKVM.Internal.Message.PathNotFound:
				return "Path not found: {0}";
			case IKVM.Internal.Message.InvalidPath:
				return "Invalid path: {0}";
			case IKVM.Internal.Message.InvalidOptionSyntax:
				return "Invalid option: {0}";
			case IKVM.Internal.Message.ExternalResourceNotFound:
				return "External resource file does not exist: {0}";
			case IKVM.Internal.Message.ExternalResourceNameInvalid:
				return "External resource file may not include path specification: {0}";
			case IKVM.Internal.Message.InvalidVersionFormat:
				return "Invalid version specified: {0}";
			case IKVM.Internal.Message.InvalidFileAlignment:
				return "Invalid value '{0}' for -filealign option";
			case IKVM.Internal.Message.ErrorWritingFile:
				return "Unable to write file: {0}\n\t({1})";
			case IKVM.Internal.Message.UnrecognizedOption:
				return "Unrecognized option: {0}";
			case IKVM.Internal.Message.NoOutputFileSpecified:
				return "No output file specified";
			case IKVM.Internal.Message.SharedClassLoaderCannotBeUsedOnModuleTarget:
				return "Incompatible options: -target:module and -sharedclassloader cannot be combined";
			case IKVM.Internal.Message.RuntimeNotFound:
				return "Unable to load runtime assembly";
			case IKVM.Internal.Message.MainClassRequiresExe:
				return "Main class cannot be specified for library or module";
			case IKVM.Internal.Message.ExeRequiresMainClass:
				return "No main method found";
			case IKVM.Internal.Message.PropertiesRequireExe:
				return "Properties cannot be specified for library or module";
			case IKVM.Internal.Message.ModuleCannotHaveClassLoader:
				return "Cannot specify assembly class loader for modules";
			case IKVM.Internal.Message.ErrorParsingMapFile:
				return "Unable to parse remap file: {0}\n\t({1})";
			case IKVM.Internal.Message.BootstrapClassesMissing:
				return "Bootstrap classes missing and core assembly not found";
			case IKVM.Internal.Message.StrongNameRequiresStrongNamedRefs:
				return "All referenced assemblies must be strong named, to be able to sign the output assembly";
			case IKVM.Internal.Message.MainClassNotFound:
				return "Main class not found";
			case IKVM.Internal.Message.MainMethodNotFound:
				return "Main method not found";
			case IKVM.Internal.Message.UnsupportedMainMethod:
				return "Redirected main method not supported";
			case IKVM.Internal.Message.ExternalMainNotAccessible:
				return "External main method must be public and in a public class";
			case IKVM.Internal.Message.ClassLoaderNotFound:
				return "Custom assembly class loader class not found";
			case IKVM.Internal.Message.ClassLoaderNotAccessible:
				return "Custom assembly class loader class is not accessible";
			case IKVM.Internal.Message.ClassLoaderIsAbstract:
				return "Custom assembly class loader class is abstract";
			case IKVM.Internal.Message.ClassLoaderNotClassLoader:
				return "Custom assembly class loader class does not extend java.lang.ClassLoader";
			case IKVM.Internal.Message.ClassLoaderConstructorMissing:
				return "Custom assembly class loader constructor is missing";
			case IKVM.Internal.Message.MapFileTypeNotFound:
				return "Type '{0}' referenced in remap file was not found";
			case IKVM.Internal.Message.MapFileClassNotFound:
				return "Class '{0}' referenced in remap file was not found";
			case IKVM.Internal.Message.MaximumErrorCountReached:
				return "Maximum error count reached";
			case IKVM.Internal.Message.LinkageError:
				return "Link error: {0}";
			case IKVM.Internal.Message.RuntimeMismatch:
				return "Referenced assembly {0} was compiled with an incompatible IKVM.Runtime version\n" +
					"\tCurrent runtime: {1}\n" +
					"\tReferenced assembly runtime: {2}";
			case IKVM.Internal.Message.CoreClassesMissing:
				return "Failed to find core classes in core library";
			case IKVM.Internal.Message.CriticalClassNotFound:
				return "Unable to load critical class '{0}'";
			case IKVM.Internal.Message.AssemblyContainsDuplicateClassNames:
				return "Type '{0}' and '{1}' both map to the same name '{2}'\n" +
					"\t({3})";
			case IKVM.Internal.Message.CallerIDRequiresHasCallerIDAnnotation:
				return "CallerID.getCallerID() requires a HasCallerID annotation";
			case IKVM.Internal.Message.UnableToResolveInterface:
				return "Unable to resolve interface '{0}' on type '{1}'";
			default:
				return "Missing Error Message. Please file a bug.";
		}
	}
}

class IkvmcCompiler
{
	private bool nonleaf;
	private string manifestMainClass;
	private Dictionary<string, ClassItem> classes = new Dictionary<string, ClassItem>();
	private Dictionary<string, List<ResourceItem>> resources = new Dictionary<string, List<ResourceItem>>();
	private string defaultAssemblyName;
	private List<string> classesToExclude = new List<string>();
	private static bool time;
	private static string runtimeAssembly;
	private static bool nostdlib;
	private static readonly List<string> libpaths = new List<string>();
	internal static readonly AssemblyResolver resolver = new AssemblyResolver();

	private static void AddArg(List<string> arglist, string s, int depth)
	{
		if (s.StartsWith("@"))
		{
			if (depth++ > 16)
			{
				throw new FatalCompilerErrorException(Message.ResponseFileDepthExceeded);
			}
			try
			{
				using (StreamReader sr = new StreamReader(s.Substring(1)))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						string arg = line.Trim();
						if (arg != "" && !arg.StartsWith("#"))
						{
							AddArg(arglist, arg, depth);
						}
					}
				}
			}
			catch (FatalCompilerErrorException)
			{
				throw;
			}
			catch (Exception x)
			{
				throw new FatalCompilerErrorException(Message.ErrorReadingFile, s.Substring(1), x.Message);
			}
		}
		else
		{
			arglist.Add(s);
		}
	}

	private static List<string> GetArgs(string[] args)
	{
		List<string> arglist = new List<string>();
		foreach(string s in args)
		{
			AddArg(arglist, s, 0);
		}
		return arglist;
	}

	static int Main(string[] args)
	{
		DateTime start = DateTime.Now;
		System.Threading.Thread.CurrentThread.Name = "compiler";
		Tracer.EnableTraceConsoleListener();
		Tracer.EnableTraceForDebug();
		try
		{
			return Compile(args);
		}
		catch (FatalCompilerErrorException x)
		{
			Console.Error.WriteLine(x.Message);
			return 1;
		}
		catch (Exception x)
		{
			Console.Error.WriteLine();
			Console.Error.WriteLine("*** INTERNAL COMPILER ERROR ***");
			Console.Error.WriteLine();
			Console.Error.WriteLine("PLEASE FILE A BUG REPORT FOR IKVM.NET WHEN YOU SEE THIS MESSAGE");
			Console.Error.WriteLine();
			Console.Error.WriteLine(x);
			return 2;
		}
		finally
		{
			if (time)
			{
				Console.WriteLine("Total cpu time: {0}", System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime);
				Console.WriteLine("User cpu time: {0}", System.Diagnostics.Process.GetCurrentProcess().UserProcessorTime);
				Console.WriteLine("Total wall clock time: {0}", DateTime.Now - start);
				Console.WriteLine("Peak virtual memory: {0}", System.Diagnostics.Process.GetCurrentProcess().PeakVirtualMemorySize64);
				for (int i = 0; i <= GC.MaxGeneration; i++)
				{
					Console.WriteLine("GC({0}) count: {1}", i, GC.CollectionCount(i));
				}
			}
		}
	}

	static int Compile(string[] args)
	{
		List<string> argList = GetArgs(args);
		if (argList.Count == 0 || argList.Contains("-?") || argList.Contains("-help"))
		{
			PrintHelp();
			return 0;
		}
		if (!argList.Contains("-nologo"))
		{
			PrintHeader();
		}
		IkvmcCompiler comp = new IkvmcCompiler();
		List<CompilerOptions> targets = new List<CompilerOptions>();
		CompilerOptions toplevel = new CompilerOptions();
		StaticCompiler.toplevel = toplevel;
		comp.ParseCommandLine(argList.GetEnumerator(), targets, toplevel);
		resolver.Warning += loader_Warning;
		resolver.Init(StaticCompiler.Universe, nostdlib, toplevel.unresolvedReferences, libpaths);
		ResolveReferences(targets);
		ResolveStrongNameKeys(targets);
		if (targets.Count == 0)
		{
			throw new FatalCompilerErrorException(Message.NoTargetsFound);
		}
		if (StaticCompiler.errorCount != 0)
		{
			return 1;
		}
		try
		{
			return CompilerClassLoader.Compile(runtimeAssembly, targets);
		}
		catch (FileFormatLimitationExceededException x)
		{
			throw new FatalCompilerErrorException(Message.FileFormatLimitationExceeded, x.Message);
		}
	}

	static void loader_Warning(AssemblyResolver.WarningId warning, string message, string[] parameters)
	{
		switch (warning)
		{
			case AssemblyResolver.WarningId.HigherVersion:
				StaticCompiler.IssueMessage(Message.AssumeAssemblyVersionMatch, parameters);
				break;
			case AssemblyResolver.WarningId.InvalidLibDirectoryOption:
				StaticCompiler.IssueMessage(Message.InvalidDirectoryInLibOptionPath, parameters);
				break;
			case AssemblyResolver.WarningId.InvalidLibDirectoryEnvironment:
				StaticCompiler.IssueMessage(Message.InvalidDirectoryInLibEnvironmentPath, parameters);
				break;
			case AssemblyResolver.WarningId.LegacySearchRule:
				StaticCompiler.IssueMessage(Message.LegacySearchRule, parameters);
				break;
			case AssemblyResolver.WarningId.LocationIgnored:
				StaticCompiler.IssueMessage(Message.AssemblyLocationIgnored, parameters);
				break;
			default:
				StaticCompiler.IssueMessage(Message.UnknownWarning, string.Format(message, parameters));
				break;
		}
	}

	static void ResolveStrongNameKeys(List<CompilerOptions> targets)
	{
		foreach (CompilerOptions options in targets)
		{
			if (options.keyfile != null && options.keycontainer != null)
			{
				throw new FatalCompilerErrorException(Message.CannotSpecifyBothKeyFileAndContainer);
			}
			if (options.keyfile == null && options.keycontainer == null && options.delaysign)
			{
				throw new FatalCompilerErrorException(Message.DelaySignRequiresKey);
			}
			if (options.keyfile != null)
			{
				if (options.delaysign)
				{
					byte[] buf = ReadAllBytes(options.keyfile);
					try
					{
						// maybe it is a key pair, if so we need to extract just the public key
						buf = new StrongNameKeyPair(buf).PublicKey;
					}
					catch { }
					options.publicKey = buf;
				}
				else
				{
					SetStrongNameKeyPair(ref options.keyPair, options.keyfile, true);
				}
			}
			else if (options.keycontainer != null)
			{
				StrongNameKeyPair keyPair = null;
				SetStrongNameKeyPair(ref keyPair, options.keycontainer, false);
				if (options.delaysign)
				{
					options.publicKey = keyPair.PublicKey;
				}
				else
				{
					options.keyPair = keyPair;
				}
			}
		}
	}

	internal static byte[] ReadAllBytes(string path)
	{
		try
		{
			return File.ReadAllBytes(path);
		}
		catch (Exception x)
		{
			throw new FatalCompilerErrorException(Message.ErrorReadingFile, path, x.Message);
		}
	}

	static string GetVersionAndCopyrightInfo()
	{
		System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
		object[] desc = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
		if (desc.Length == 1)
		{
			object[] copyright = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
			if (copyright.Length == 1)
			{
				return string.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
					((System.Reflection.AssemblyTitleAttribute)desc[0]).Title,
					asm.GetName().Version,
					Environment.NewLine,
					((System.Reflection.AssemblyCopyrightAttribute)copyright[0]).Copyright);
			}
		}
		return "";
	}

	private static void PrintHeader()
	{
		Console.Error.WriteLine(GetVersionAndCopyrightInfo());
		Console.Error.WriteLine();
	}

	private static void PrintHelp()
	{
		PrintHeader();
		Console.Error.WriteLine("Usage: ikvmc [-options] <classOrJar1> ... <classOrJarN>");
		Console.Error.WriteLine();
		Console.Error.WriteLine("Compiler Options:");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - OUTPUT FILES -");
		Console.Error.WriteLine("-out:<outputfile>              Specify the output filename");
		Console.Error.WriteLine("-assembly:<name>               Specify assembly name");
		Console.Error.WriteLine("-version:<M.m.b.r>             Specify assembly version");
		Console.Error.WriteLine("-target:exe                    Build a console executable");
		Console.Error.WriteLine("-target:winexe                 Build a windows executable");
		Console.Error.WriteLine("-target:library                Build a library");
		Console.Error.WriteLine("-target:module                 Build a module for use by the linker");
		Console.Error.WriteLine("-platform:<string>             Limit which platforms this code can run on:");
		Console.Error.WriteLine("                               x86, x64, arm, anycpu32bitpreferred, or");
		Console.Error.WriteLine("                               anycpu. The default is anycpu.");
		Console.Error.WriteLine("-keyfile:<keyfilename>         Use keyfile to sign the assembly");
		Console.Error.WriteLine("-key:<keycontainer>            Use keycontainer to sign the assembly");
		Console.Error.WriteLine("-delaysign                     Delay-sign the assembly");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - INPUT FILES -");
		Console.Error.WriteLine("-reference:<filespec>          Reference an assembly (short form -r:<filespec>)");
		Console.Error.WriteLine("-recurse:<filespec>            Recurse directory and include matching files");
		Console.Error.WriteLine("-exclude:<filename>            A file containing a list of classes to exclude");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - RESOURCES -");
		Console.Error.WriteLine("-fileversion:<version>         File version");
		Console.Error.WriteLine("-win32icon:<file>              Embed specified icon in output");
		Console.Error.WriteLine("-win32manifest:<file>          Specify a Win32 manifest file (.xml)");
		Console.Error.WriteLine("-resource:<name>=<path>        Include file as Java resource");
		Console.Error.WriteLine("-externalresource:<name>=<path>");
		Console.Error.WriteLine("                               Reference file as Java resource");
		Console.Error.WriteLine("-compressresources             Compress resources");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - CODE GENERATION -");
		Console.Error.WriteLine("-debug                         Generate debug info for the output file");
		Console.Error.WriteLine("                               (Note that this also causes the compiler to");
		Console.Error.WriteLine("                               generated somewhat less efficient CIL code.)");
		Console.Error.WriteLine("-noautoserialization           Disable automatic .NET serialization support");
		Console.Error.WriteLine("-noglobbing                    Don't glob the arguments passed to main");
		Console.Error.WriteLine("-nojni                         Do not generate JNI stub for native methods");
		Console.Error.WriteLine("-opt:fields                    Remove unused private fields");
		Console.Error.WriteLine("-removeassertions              Remove all assert statements");
		Console.Error.WriteLine("-strictfinalfieldsemantics     Don't allow final fields to be modified outside");
		Console.Error.WriteLine("                               of initializer methods");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - ERRORS AND WARNINGS -");
		Console.Error.WriteLine("-nowarn:<warning[:key]>        Suppress specified warnings");
		Console.Error.WriteLine("-warnaserror                   Treat all warnings as errors");
		Console.Error.WriteLine("-warnaserror:<warning[:key]>   Treat specified warnings as errors");
		Console.Error.WriteLine("-writeSuppressWarningsFile:<file>");
		Console.Error.WriteLine("                               Write response file with -nowarn:<warning[:key]>");
		Console.Error.WriteLine("                               options to suppress all encountered warnings");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - MISCELLANEOUS -");
		Console.Error.WriteLine("@<filename>                    Read more options from file");
		Console.Error.WriteLine("-help                          Display this usage message (Short form: -?)");
		Console.Error.WriteLine("-nologo                        Suppress compiler copyright message");
		Console.Error.WriteLine();
		Console.Error.WriteLine("                      - ADVANCED -");
		Console.Error.WriteLine("-main:<class>                  Specify the class containing the main method");
		Console.Error.WriteLine("-srcpath:<path>                Prepend path and package name to source file");
		Console.Error.WriteLine("-apartment:sta                 (default) Apply STAThreadAttribute to main");
		Console.Error.WriteLine("-apartment:mta                 Apply MTAThreadAttribute to main");
		Console.Error.WriteLine("-apartment:none                Don't apply STAThreadAttribute to main");
		Console.Error.WriteLine("-D<name>=<value>               Set system property (at runtime)");
		Console.Error.WriteLine("-ea[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("-enableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                               Set system property to enable assertions");
		Console.Error.WriteLine("-da[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("-disableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                               Set system property to disable assertions");
		Console.Error.WriteLine("-nostacktraceinfo              Don't create metadata to emit rich stack traces");
		Console.Error.WriteLine("-Xtrace:<string>               Displays all tracepoints with the given name");
		Console.Error.WriteLine("-Xmethodtrace:<string>         Build tracing into the specified output methods");
		Console.Error.WriteLine("-privatepackage:<prefix>       Mark all classes with a package name starting");
		Console.Error.WriteLine("                               with <prefix> as internal to the assembly");
		Console.Error.WriteLine("-time                          Display timing statistics");
		Console.Error.WriteLine("-classloader:<class>           Set custom class loader class for assembly");
		Console.Error.WriteLine("-sharedclassloader             All targets below this level share a common");
		Console.Error.WriteLine("                               class loader");
		Console.Error.WriteLine("-baseaddress:<address>         Base address for the library to be built");
		Console.Error.WriteLine("-filealign:<n>                 Specify the alignment used for output file");
		Console.Error.WriteLine("-nopeercrossreference          Do not automatically cross reference all peers");
		Console.Error.WriteLine("-nostdlib                      Do not reference standard libraries");
		Console.Error.WriteLine("-lib:<dir>                     Additional directories to search for references");
		Console.Error.WriteLine("-highentropyva                 Enable high entropy ASLR");
	}

	void ParseCommandLine(IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
	{
		options.target = PEFileKinds.ConsoleApplication;
		options.guessFileKind = true;
		options.version = new Version(0, 0, 0, 0);
		options.apartment = ApartmentState.STA;
		options.props = new Dictionary<string, string>();
		ContinueParseCommandLine(arglist, targets, options);
	}

	void ContinueParseCommandLine(IEnumerator<string> arglist, List<CompilerOptions> targets, CompilerOptions options)
	{
		List<string> fileNames = new List<string>();
		while(arglist.MoveNext())
		{
			string s = arglist.Current;
			if(s == "{")
			{
				if (!nonleaf)
				{
					ReadFiles(fileNames);
					nonleaf = true;
				}
				IkvmcCompiler nestedLevel = new IkvmcCompiler();
				nestedLevel.manifestMainClass = manifestMainClass;
				nestedLevel.classes = new Dictionary<string, ClassItem>(classes);
				nestedLevel.resources = CompilerOptions.Copy(resources);
				nestedLevel.defaultAssemblyName = defaultAssemblyName;
				nestedLevel.classesToExclude = new List<string>(classesToExclude);
				nestedLevel.ContinueParseCommandLine(arglist, targets, options.Copy());
			}
			else if(s == "}")
			{
				break;
			}
			else if(nonleaf)
			{
				throw new FatalCompilerErrorException(Message.OptionsMustPreceedChildLevels);
			}
			else if(s[0] == '-')
			{
				if(s.StartsWith("-out:"))
				{
					options.path = s.Substring(5);
				}
				else if(s.StartsWith("-Xtrace:"))
				{
					Tracer.SetTraceLevel(s.Substring(8));
				}
				else if(s.StartsWith("-Xmethodtrace:"))
				{
					Tracer.HandleMethodTrace(s.Substring(14));
				}
				else if(s.StartsWith("-assembly:"))
				{
					options.assembly = s.Substring(10);
				}
				else if(s.StartsWith("-target:"))
				{
					switch(s)
					{
						case "-target:exe":
							options.target = PEFileKinds.ConsoleApplication;
							options.guessFileKind = false;
							break;
						case "-target:winexe":
							options.target = PEFileKinds.WindowApplication;
							options.guessFileKind = false;
							break;
						case "-target:module":
							options.targetIsModule = true;
							options.target = PEFileKinds.Dll;
							options.guessFileKind = false;
							break;
						case "-target:library":
							options.target = PEFileKinds.Dll;
							options.guessFileKind = false;
							break;
						default:
							throw new FatalCompilerErrorException(Message.UnrecognizedTargetType, s.Substring(8));
					}
				}
				else if(s.StartsWith("-platform:"))
				{
					switch(s)
					{
						case "-platform:x86":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Required32Bit;
							options.imageFileMachine = ImageFileMachine.I386;
							break;
						case "-platform:x64":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.PE32Plus;
							options.imageFileMachine = ImageFileMachine.AMD64;
							break;
						case "-platform:arm":
							options.pekind = PortableExecutableKinds.ILOnly;
							options.imageFileMachine = ImageFileMachine.ARM;
							break;
						case "-platform:anycpu32bitpreferred":
							options.pekind = PortableExecutableKinds.ILOnly | PortableExecutableKinds.Preferred32Bit;
							options.imageFileMachine = ImageFileMachine.I386;
							break;
						case "-platform:anycpu":
							options.pekind = PortableExecutableKinds.ILOnly;
							options.imageFileMachine = ImageFileMachine.I386;
							break;
						default:
							throw new FatalCompilerErrorException(Message.UnrecognizedPlatform, s.Substring(10));
					}
				}
				else if(s.StartsWith("-apartment:"))
				{
					switch(s)
					{
						case "-apartment:sta":
							options.apartment = ApartmentState.STA;
							break;
						case "-apartment:mta":
							options.apartment = ApartmentState.MTA;
							break;
						case "-apartment:none":
							options.apartment = ApartmentState.Unknown;
							break;
						default:
							throw new FatalCompilerErrorException(Message.UnrecognizedApartment, s.Substring(11));
					}
				}
				else if(s == "-noglobbing")
				{
					options.noglobbing = true;
				}
				else if(s.StartsWith("-D"))
				{
					string[] keyvalue = s.Substring(2).Split('=');
					if(keyvalue.Length != 2)
					{
						keyvalue = new string[] { keyvalue[0], "" };
					}
					options.props[keyvalue[0]] = keyvalue[1];
				}
				else if(s == "-ea" || s == "-enableassertions")
				{
					options.props["ikvm.assert.default"] = "true";
				}
				else if(s == "-da" || s == "-disableassertions")
				{
					options.props["ikvm.assert.default"] = "false";
				}
				else if(s.StartsWith("-ea:") || s.StartsWith("-enableassertions:"))
				{
					options.props["ikvm.assert.enable"] = s.Substring(s.IndexOf(':') + 1);
				}
				else if(s.StartsWith("-da:") || s.StartsWith("-disableassertions:"))
				{
					options.props["ikvm.assert.disable"] = s.Substring(s.IndexOf(':') + 1);
				}
				else if(s == "-removeassertions")
				{
					options.codegenoptions |= CodeGenOptions.RemoveAsserts;
				}
				else if(s.StartsWith("-main:"))
				{
					options.mainClass = s.Substring(6);
				}
				else if(s.StartsWith("-reference:") || s.StartsWith("-r:"))
				{
					string r = s.Substring(s.IndexOf(':') + 1);
					if(r == "")
					{
						throw new FatalCompilerErrorException(Message.MissingFileSpecification, s);
					}
					ArrayAppend(ref options.unresolvedReferences, r);
				}
				else if(s.StartsWith("-recurse:"))
				{
					string spec = s.Substring(9);
					bool exists = false;
					// MONOBUG On Mono 1.0.2, Directory.Exists throws an exception if we pass an invalid directory name
					try
					{
						exists = Directory.Exists(spec);
					}
					catch(IOException)
					{
					}
					if(exists)
					{
						DirectoryInfo dir = new DirectoryInfo(spec);
						Recurse(dir, dir, "*");
					}
					else
					{
						try
						{
							DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(spec));
							if(dir.Exists)
							{
								Recurse(dir, dir, Path.GetFileName(spec));
							}
							else
							{
								RecurseJar(spec);
							}
						}
						catch(PathTooLongException)
						{
							throw new FatalCompilerErrorException(Message.PathTooLong, spec);
						}
						catch(DirectoryNotFoundException)
						{
							throw new FatalCompilerErrorException(Message.PathNotFound, spec);
						}
						catch(ArgumentException)
						{
							throw new FatalCompilerErrorException(Message.InvalidPath, spec);
						}
					}
				}
				else if(s.StartsWith("-resource:"))
				{
					string[] spec = s.Substring(10).Split('=');
					if(spec.Length != 2)
					{
						throw new FatalCompilerErrorException(Message.InvalidOptionSyntax, s);
					}
					AddResource(null, spec[0].TrimStart('/'), ReadAllBytes(spec[1]), null);
				}
				else if(s.StartsWith("-externalresource:"))
				{
					string[] spec = s.Substring(18).Split('=');
					if(spec.Length != 2)
					{
						throw new FatalCompilerErrorException(Message.InvalidOptionSyntax, s);
					}
					if(!File.Exists(spec[1]))
					{
						throw new FatalCompilerErrorException(Message.ExternalResourceNotFound, spec[1]);
					}
					if(Path.GetFileName(spec[1]) != spec[1])
					{
						throw new FatalCompilerErrorException(Message.ExternalResourceNameInvalid, spec[1]);
					}
					if(options.externalResources == null)
					{
						options.externalResources = new Dictionary<string, string>();
					}
					// TODO resource name clashes should be tested
					options.externalResources.Add(spec[0], spec[1]);
				}
				else if(s == "-nojni")
				{
					options.codegenoptions |= CodeGenOptions.NoJNI;
				}
				else if(s.StartsWith("-exclude:"))
				{
					ProcessExclusionFile(classesToExclude, s.Substring(9));
				}
				else if(s.StartsWith("-version:"))
				{
					string str = s.Substring(9);
					if(!TryParseVersion(s.Substring(9), out options.version))
					{
						throw new FatalCompilerErrorException(Message.InvalidVersionFormat, str);
					}
				}
				else if(s.StartsWith("-fileversion:"))
				{
					options.fileversion = s.Substring(13);
				}
				else if(s.StartsWith("-win32icon:"))
				{
					options.iconfile = s.Substring(11);
				}
				else if(s.StartsWith("-win32manifest:"))
				{
					options.manifestFile = s.Substring(15);
				}
				else if(s.StartsWith("-keyfile:"))
				{
					options.keyfile = s.Substring(9);
				}
				else if(s.StartsWith("-key:"))
				{
					options.keycontainer = s.Substring(5);
				}
				else if(s == "-delaysign")
				{
					options.delaysign = true;
				}
				else if(s == "-debug")
				{
					options.codegenoptions |= CodeGenOptions.Debug;
				}
				else if(s.StartsWith("-srcpath:"))
				{
					options.sourcepath = s.Substring(9);
				}
				else if(s.StartsWith("-remap:"))
				{
					options.remapfile = s.Substring(7);
				}
				else if(s == "-nostacktraceinfo")
				{
					options.codegenoptions |= CodeGenOptions.NoStackTraceInfo;
				}
				else if(s == "-opt:fields")
				{
					options.removeUnusedFields = true;
				}
				else if(s == "-compressresources")
				{
					options.compressedResources = true;
				}
				else if(s == "-strictfinalfieldsemantics")
				{
					options.codegenoptions |= CodeGenOptions.StrictFinalFieldSemantics;
				}
				else if(s.StartsWith("-privatepackage:"))
				{
					string prefix = s.Substring(16);
					ArrayAppend(ref options.privatePackages, prefix);
				}
				else if(s.StartsWith("-publicpackage:"))
				{
					string prefix = s.Substring(15);
					ArrayAppend(ref options.publicPackages, prefix);
				}
				else if(s.StartsWith("-nowarn:"))
				{
					foreach(string w in s.Substring(8).Split(','))
					{
						string ws = w;
						// lame way to chop off the leading zeroes
						while(ws.StartsWith("0"))
						{
							ws = ws.Substring(1);
						}
						options.suppressWarnings[ws] = ws;
					}
				}
				else if(s == "-warnaserror")
				{
					options.warnaserror = true;
				}
				else if(s.StartsWith("-warnaserror:"))
				{
					foreach(string w in s.Substring(13).Split(','))
					{
						string ws = w;
						// lame way to chop off the leading zeroes
						while(ws.StartsWith("0"))
						{
							ws = ws.Substring(1);
						}
						options.errorWarnings[ws] = ws;
					}
				}
				else if(s.StartsWith("-runtime:"))
				{
					// NOTE this is an undocumented option
					runtimeAssembly = s.Substring(9);
				}
				else if(s == "-time")
				{
					time = true;
				}
				else if(s.StartsWith("-classloader:"))
				{
					options.classLoader = s.Substring(13);
				}
				else if(s == "-sharedclassloader")
				{
					if(options.sharedclassloader == null)
					{
						options.sharedclassloader = new List<CompilerClassLoader>();
					}
				}
				else if(s.StartsWith("-baseaddress:"))
				{
					string baseAddress = s.Substring(13);
					ulong baseAddressParsed;
					if (baseAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
					{
						baseAddressParsed = UInt64.Parse(baseAddress.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
					}
					else
					{
						// note that unlike CSC we don't support octal
						baseAddressParsed = UInt64.Parse(baseAddress);
					}
					options.baseAddress = (long)(baseAddressParsed & 0xFFFFFFFFFFFF0000UL);
				}
				else if(s.StartsWith("-filealign:"))
				{
					int filealign;
					if (!Int32.TryParse(s.Substring(11), out filealign)
						|| filealign < 512
						|| filealign > 8192
						|| (filealign & (filealign - 1)) != 0)
					{
						throw new FatalCompilerErrorException(Message.InvalidFileAlignment, s.Substring(11));
					}
					options.fileAlignment = filealign;
				}
				else if(s == "-nopeercrossreference")
				{
					options.crossReferenceAllPeers = false;
				}
				else if(s=="-nostdlib")
				{
					// this is a global option
					nostdlib = true;
				}
				else if(s.StartsWith("-lib:"))
				{
					// this is a global option
					libpaths.Add(s.Substring(5));
				}
				else if(s == "-noautoserialization")
				{
					options.codegenoptions |= CodeGenOptions.NoAutomagicSerialization;
				}
				else if(s == "-highentropyva")
				{
					options.highentropyva = true;
				}
				else if(s.StartsWith("-writeSuppressWarningsFile:"))
				{
					options.writeSuppressWarningsFile = s.Substring(27);
					try
					{
						File.Delete(options.writeSuppressWarningsFile);
					}
					catch(Exception x)
					{
						throw new FatalCompilerErrorException(Message.ErrorWritingFile, options.writeSuppressWarningsFile, x.Message);
					}
				}
				else if(s.StartsWith("-proxy:")) // currently undocumented!
				{
					string proxy = s.Substring(7);
					if(options.proxies.Contains(proxy))
					{
						StaticCompiler.IssueMessage(Message.DuplicateProxy, proxy);
					}
					options.proxies.Add(proxy);
				}
				else if(s == "-nologo")
				{
					// Ignore. This is handled earlier.
				}
				else
				{
					throw new FatalCompilerErrorException(Message.UnrecognizedOption, s);
				}
			}
			else
			{
				fileNames.Add(s);
			}
			if(options.targetIsModule && options.sharedclassloader != null)
			{
				throw new FatalCompilerErrorException(Message.SharedClassLoaderCannotBeUsedOnModuleTarget);
			}
		}
		if(nonleaf)
		{
			return;
		}
		ReadFiles(fileNames);
		if(options.assembly == null)
		{
			string basename = options.path == null ? defaultAssemblyName : new FileInfo(options.path).Name;
			if(basename == null)
			{
				throw new FatalCompilerErrorException(Message.NoOutputFileSpecified);
			}
			int idx = basename.LastIndexOf('.');
			if(idx > 0)
			{
				options.assembly = basename.Substring(0, idx);
			}
			else
			{
				options.assembly = basename;
			}
		}
		if(options.path != null && options.guessFileKind)
		{
			if(options.path.ToLower().EndsWith(".dll"))
			{
				options.target = PEFileKinds.Dll;
			}
			options.guessFileKind = false;
		}
		if(options.mainClass == null && manifestMainClass != null && (options.guessFileKind || options.target != PEFileKinds.Dll))
		{
			StaticCompiler.IssueMessage(options, Message.MainMethodFromManifest, manifestMainClass);
			options.mainClass = manifestMainClass;
		}
		options.classes = classes;
		options.resources = resources;
		options.classesToExclude = classesToExclude.ToArray();
		targets.Add(options);
	}

	private void ReadFiles(List<string> fileNames)
	{
		foreach (string fileName in fileNames)
		{
			if (defaultAssemblyName == null)
			{
				try
				{
					defaultAssemblyName = new FileInfo(Path.GetFileName(fileName)).Name;
				}
				catch (ArgumentException)
				{
					// if the filename contains a wildcard (or any other invalid character), we ignore
					// it as a potential default assembly name
				}
			}
			string[] files = null;
			try
			{
				string path = Path.GetDirectoryName(fileName);
				files = Directory.GetFiles(path == "" ? "." : path, Path.GetFileName(fileName));
			}
			catch { }
			if (files == null || files.Length == 0)
			{
				StaticCompiler.IssueMessage(Message.InputFileNotFound, fileName);
			}
			else
			{
				foreach (string f in files)
				{
					ProcessFile(null, f);
				}
			}
		}
	}

	internal static bool TryParseVersion(string str, out Version version)
	{
		if (str.EndsWith(".*"))
		{
			str = str.Substring(0, str.Length - 1);
			int count = str.Split('.').Length;
			// NOTE this is the published algorithm for generating automatic build and revision numbers
			// (see AssemblyVersionAttribute constructor docs), but it turns out that the revision
			// number is off an hour (on my system)...
			DateTime now = DateTime.Now;
			int seconds = (int)(now.TimeOfDay.TotalSeconds / 2);
			int days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
			if (count == 3)
			{
				str += days + "." + seconds;
			}
			else if (count == 4)
			{
				str += seconds;
			}
			else
			{
				version = null;
				return false;
			}
		}
		try
		{
			version = new Version(str);
			return version.Major <= 65535 && version.Minor <= 65535 && version.Build <= 65535 && version.Revision <= 65535;
		}
		catch (ArgumentException) { }
		catch (FormatException) { }
		catch (OverflowException) { }
		version = null;
		return false;
	}

	static void SetStrongNameKeyPair(ref StrongNameKeyPair strongNameKeyPair, string fileNameOrKeyContainer, bool file)
	{
		try
		{
			if (file)
			{
				strongNameKeyPair = new StrongNameKeyPair(File.ReadAllBytes(fileNameOrKeyContainer));
			}
			else
			{
				strongNameKeyPair = new StrongNameKeyPair(fileNameOrKeyContainer);
			}
			// FXBUG we explicitly try to access the public key force a check (the StrongNameKeyPair constructor doesn't validate the key)
			if (strongNameKeyPair.PublicKey != null) { }
		}
		catch (Exception x)
		{
			throw new FatalCompilerErrorException(Message.InvalidStrongNameKeyPair, file ? "file" : "container", x.Message);
		}
	}

	static void ResolveReferences(List<CompilerOptions> targets)
	{
		Dictionary<string, Assembly> cache = new Dictionary<string, Assembly>();
		foreach (CompilerOptions target in targets)
		{
			if (target.unresolvedReferences != null)
			{
				foreach (string reference in target.unresolvedReferences)
				{
					foreach (CompilerOptions peer in targets)
					{
						if (peer.assembly.Equals(reference, StringComparison.InvariantCultureIgnoreCase))
						{
							ArrayAppend(ref target.peerReferences, peer.assembly);
							goto next_reference;
						}
					}
					if (!resolver.ResolveReference(cache, ref target.references, reference))
					{
						throw new FatalCompilerErrorException(Message.ReferenceNotFound, reference);
					}
				next_reference: ;
				}
			}
		}
	}

	private static void ArrayAppend<T>(ref T[] array, T element)
	{
		if (array == null)
		{
			array = new T[] { element };
		}
		else
		{
			T[] temp = new T[array.Length + 1];
			Array.Copy(array, 0, temp, 0, array.Length);
			temp[temp.Length - 1] = element;
			array = temp;
		}
	}

	private static byte[] ReadFromZip(ZipFile zf, ZipEntry ze)
	{
		byte[] buf = new byte[ze.Size];
		int pos = 0;
		Stream s = zf.GetInputStream(ze);
		while(pos < buf.Length)
		{
			pos += s.Read(buf, pos, buf.Length - pos);
		}
		return buf;
	}

	private void AddClassFile(ZipEntry zipEntry, string filename, byte[] buf, bool addResourceFallback, string jar)
	{
		try
		{
			string name = ClassFile.GetClassName(buf, 0, buf.Length);
			if(classes.ContainsKey(name))
			{
				StaticCompiler.IssueMessage(Message.DuplicateClassName, name);
			}
			else
			{
				ClassItem item;
				item.data = buf;
				item.path = zipEntry == null ? filename : null;
				classes.Add(name, item);
			}
		}
		catch(ClassFormatError x)
		{
			if(addResourceFallback)
			{
				// not a class file, so we include it as a resource
				// (IBM's db2os390/sqlj jars apparantly contain such files)
				StaticCompiler.IssueMessage(Message.NotAClassFile, filename, x.Message);
				AddResource(zipEntry, filename, buf, jar);
			}
			else
			{
				StaticCompiler.IssueMessage(Message.ClassFormatError, filename, x.Message);
			}
		}
	}

	private void ProcessZipFile(string file, Predicate<ZipEntry> filter)
	{
		string jar = Path.GetFileName(file);
		ZipFile zf = new ZipFile(file);
		try
		{
			foreach(ZipEntry ze in zf)
			{
				if(filter != null && !filter(ze))
				{
					// skip
				}
				else if(ze.IsDirectory)
				{
					AddResource(ze, ze.Name, null, jar);
				}
				else if(ze.Name.ToLower().EndsWith(".class"))
				{
					AddClassFile(ze, ze.Name, ReadFromZip(zf, ze), true, jar);
				}
				else
				{
					// if it's not a class, we treat it as a resource and the manifest
					// is examined to find the Main-Class
					if(ze.Name == "META-INF/MANIFEST.MF" && manifestMainClass == null)
					{
						// read main class from manifest
						// TODO find out if we can use other information from manifest
						StreamReader rdr = new StreamReader(zf.GetInputStream(ze));
						string line;
						while((line = rdr.ReadLine()) != null)
						{
							if(line.StartsWith("Main-Class: "))
							{
								line = line.Substring(12);
								string continuation;
								while((continuation = rdr.ReadLine()) != null
									&& continuation.StartsWith(" ", StringComparison.Ordinal))
								{
									line += continuation.Substring(1);
								}
								manifestMainClass = line.Replace('/', '.');
								break;
							}
						}
					}
					AddResource(ze, ze.Name, ReadFromZip(zf, ze), jar);
				}
			}
		}
		finally
		{
			zf.Close();
		}
	}

	private void AddResource(ZipEntry zipEntry, string name, byte[] buf, string jar)
	{
		List<ResourceItem> list;
		if (!resources.TryGetValue(name, out list))
		{
			list = new List<ResourceItem>();
			resources.Add(name, list);
		}
		ResourceItem item = new ResourceItem();
		item.zipEntry = zipEntry;
		item.data = buf;
		item.jar = jar ?? "resources.jar";
		list.Add(item);
	}

	private void ProcessFile(DirectoryInfo baseDir, string file)
	{
		switch(new FileInfo(file).Extension.ToLower())
		{
			case ".class":
				AddClassFile(null, file, ReadAllBytes(file), false, null);
				break;
			case ".jar":
			case ".zip":
				try
				{
					ProcessZipFile(file, null);
				}
				catch(ICSharpCode.SharpZipLib.SharpZipBaseException x)
				{
					throw new FatalCompilerErrorException(Message.ErrorReadingFile, file, x.Message);
				}
				break;
			default:
			{
				if(baseDir == null)
				{
					StaticCompiler.IssueMessage(Message.UnknownFileType, file);
				}
				else
				{
					// include as resource
					// extract the resource name by chopping off the base directory
					string name = file.Substring(baseDir.FullName.Length);
					name = name.TrimStart(Path.DirectorySeparatorChar).Replace('\\', '/');
					AddResource(null, name, ReadAllBytes(file), null);
				}
				break;
			}
		}
	}

	private void Recurse(DirectoryInfo baseDir, DirectoryInfo dir, string spec)
	{
		foreach(FileInfo file in dir.GetFiles(spec))
		{
			ProcessFile(baseDir, file.FullName);
		}
		foreach(DirectoryInfo sub in dir.GetDirectories())
		{
			Recurse(baseDir, sub, spec);
		}
	}

	private void RecurseJar(string path)
	{
		string file = "";
		for (; ; )
		{
			file = Path.Combine(Path.GetFileName(path), file);
			path = Path.GetDirectoryName(path);
			if (Directory.Exists(path))
			{
				throw new DirectoryNotFoundException();
			}
			else if (File.Exists(path))
			{
				string pathFilter = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar;
				string fileFilter = "^" + Regex.Escape(Path.GetFileName(file)).Replace("\\*", ".*").Replace("\\?", ".") + "$";
				ProcessZipFile(path, delegate(ZipEntry ze) {
					// MONOBUG Path.GetDirectoryName() doesn't normalize / to \ on Windows
					string name = ze.Name.Replace('/', Path.DirectorySeparatorChar);
					return (Path.GetDirectoryName(name) + Path.DirectorySeparatorChar).StartsWith(pathFilter)
						&& Regex.IsMatch(Path.GetFileName(ze.Name), fileFilter);
				});
				return;
			}
		}
	}

	//This processes an exclusion file with a single regular expression per line
	private static void ProcessExclusionFile(List<string> classesToExclude, String filename)
	{
		try 
		{
			using(StreamReader file = new StreamReader(filename))
			{
				String line;
				while((line = file.ReadLine()) != null)
				{
					line = line.Trim();
					if(!line.StartsWith("//") && line.Length != 0)
					{
						classesToExclude.Add(line);
					}
				}
			}
		} 
		catch(Exception x) 
		{
			throw new FatalCompilerErrorException(Message.ErrorReadingFile, filename, x.Message);
		}
	}
}
