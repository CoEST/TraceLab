using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TraceLabSDK")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("TraceLabSDK")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b34f2e9c-95b6-428b-ba61-0c637cdf032b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.5.2.0")]
[assembly: AssemblyFileVersion("0.5.2.0")]

// Let TraceLab.Core see the privates of the SDK - Mainly for the Serializer cache.
[assembly: InternalsVisibleTo("TraceLab.Core, PublicKey=00240000048000009400000006020000002400005253413100040000010001006f7af96714d93af0694fa11cc58486da3fa0e1b647bd1ab690d7fffa640b9fe28c2e6d27f59a251c56627ea5911b632e219ae5bc23cad3bf4064df578c9abdcb3938969bffb12daded4c05a125bf38afdcfd5763a28aaad66cc5b7fe535da002c1b8ad2f839060f2aabae3dc6f0bd33f5476bc0d2b0d77d66f75c70ebabc3ec2")]

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("TraceLab.Core.Test, PublicKey=00240000048000009400000006020000002400005253413100040000010001006f7af96714d93af0694fa11cc58486da3fa0e1b647bd1ab690d7fffa640b9fe28c2e6d27f59a251c56627ea5911b632e219ae5bc23cad3bf4064df578c9abdcb3938969bffb12daded4c05a125bf38afdcfd5763a28aaad66cc5b7fe535da002c1b8ad2f839060f2aabae3dc6f0bd33f5476bc0d2b0d77d66f75c70ebabc3ec2")]