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
using TraceLabSDK;
using NLog;
using System.Security.Permissions;

namespace TraceLabSDK
{
    /// <summary>
    /// An abstract base class that can be used when creating a component.
    /// </summary>
    [Component("B60234AB-F433-4E89-87D8-272AA242A0DB", "Default Component", "", "Nobody", "0.0", null)]
    public abstract class BaseComponent : MarshalByRefObject, IComponent
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseComponent"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used by this component.</param>
        protected BaseComponent(ComponentLogger logger)
        {
            m_logger = logger;
        }

        /// <summary>
        /// Performs any pre-compute setup that is needed.  This is run immediately prior to Compute.
        /// </summary>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        public virtual void PreCompute() { }

        /// <summary>
        /// Called when the component should do it's actual work.
        /// </summary>
        /// <exception cref="T:System.Exception"/>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        abstract public void Compute();

        /// <summary>
        /// Performs any post-compute shutdown or cleanup steps that are needed.  This is run immediately after Compute.
        /// </summary>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        public virtual void PostCompute() { }

        /**
         * Reference to the Workspace
         * 
         */
        private IWorkspace m_workspace;
        /// <summary>
        /// The component's own personal phone-line to the Workspace.
        /// </summary>
        public IWorkspace Workspace 
        {
            get 
            {
                return m_workspace;
            }
            set
            {
                m_workspace = value;
            }
        }

        private object m_configuration;
        /// <summary>
        /// An object that contains configuration properties for this component
        /// </summary>
        public object Configuration
        {
            get {
                return m_configuration;
            }
            protected set
            {
                m_configuration = value;
            }
        }

        private ComponentLogger m_logger;
        /// <summary>
        /// Gets the logger that is to be used for this component
        /// </summary>
        public ComponentLogger Logger
        {
            get
            {
                return m_logger;
            }
        }
    }


    /// <summary>
    /// The interface that must be implemented by a component
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Performs any pre-compute setup that is needed.  This is run immediately prior to <c>Compute</c>.
        /// </summary>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        void PreCompute();

        /// <summary>
        /// Called when the component should do it's actual work.  
        /// </summary>
        /// <remarks>Any exception thrown will cause <c>Compute</c> to be treated as failed.</remarks>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        void Compute();

        /// <summary>
        /// Performs any processing necessary to shutdown/release systems or memory that the component might be holding on to prior to unload.
        /// </summary>
        /// <remarks>It's important to be aware that <c>PostCompute</c> will be called regardless of whether <c>Compute</c> or <c>PreCompute</c> succeeds or not.</remarks>
        [IKVM.Attributes.Throws(typeof(ComponentException))]
        void PostCompute();

        /// <summary>
        /// The component's own personal phone-line to the Workspace.
        /// </summary>
        IWorkspace Workspace
        {
            get;
            set;
        }

        /// <summary>
        /// An object that contains configuration properties for this component
        /// </summary>
        object Configuration
        {
            get;
        }
    }

}
