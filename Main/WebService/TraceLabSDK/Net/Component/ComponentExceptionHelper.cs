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
using System.Security.Permissions;

namespace TraceLabSDK
{
    internal sealed class ComponentDomainExceptionHelper : MarshalByRefObject
    {
        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        ///   </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        internal ComponentDomainExceptionHelper()
        {
        }

        /// <summary>
        /// Catch unhandled exceptions in Component domains and rethrow them so that the main application hears about it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Rethrow whatever happened on that domain
            throw (Exception)e.ExceptionObject;
        }

        internal static System.Reflection.Assembly AppDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return LocateAssemblyInCurrentDomain(args.Name);
        }

        internal static System.Reflection.Assembly AppDomainTypeResolve(object sender, ResolveEventArgs args)
        {
            return LocateAssemblyInCurrentDomain(args.Name);
        }

        private static System.Reflection.Assembly LocateAssemblyInCurrentDomain(string name)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Resolving assembly: {0}", name));

            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (System.Reflection.Assembly assm in assemblies)
                {
                    if (assm.FullName == name)
                    {
                        System.Diagnostics.Debug.WriteLine("\tFound assembly already loaded.");
                        return assm;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("\tDid not find assembly.");
            return null;
        }
    }
}
