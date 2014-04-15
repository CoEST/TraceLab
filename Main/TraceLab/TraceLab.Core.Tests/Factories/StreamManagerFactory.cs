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

// <copyright file="StreamManagerFactory.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>

using System;
using Microsoft.Pex.Framework;
using TraceLab.Serialization.StreamSystem;

namespace TraceLab.Serialization.StreamSystem
{
    /// <summary>A factory for TraceLab.Serialization.StreamSystem.StreamManager instances</summary>
    public static partial class StreamManagerFactory
    {
        /// <summary>A factory for TraceLab.Serialization.StreamSystem.StreamManager instances</summary>
        [PexFactoryMethod(typeof(StreamManager))]
        public static object Create()
        {
            return StreamManager.CreateNewManager();
        }
    }
}
