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

namespace TraceLabSDK
{
    /// <summary>
    /// Represents an editor for a workspace unit.  This is to be applied to a UI element that will be used to edit
    /// the unit.
    /// </summary>
    /// <remarks>
    /// Editors for types in the workspace are searched for in the following manner:
    /// 
    /// 1) The data type of the workspace unit is determined, along with the assembly that defines this type
    /// 2) The directory where that assembly resides is searched for other assemblies that match the pattern
    /// 
    ///     [type's assembly-name].UI.[UI type].dll
    ///     WPF version: TraceLabSDK.UI.WPF.dll
    ///     Mono version: TraceLabSDK.UI.Mono.dll
    /// 
    ///     Currently supported UI types are: WPF and Mono.
    ///     At present, WPF does not support loading in the Mono editors - this may change.
    ///     Mono will never be able to load the WPF editors.
    /// 
    /// 3) If an assembly is found that matches, this assembly is searched for a class that 
    ///     a: implements IWorkspaceUnitEditor
    ///     b: has a typename that matches [workspace unit typename]Editor
    /// 
    ///     eg. TraceLabSDK.Types.TLArtifact 
    ///         will search for a class TraceLabSDK.Types.TLArtifactEditor that implements IWorkspaceUnitEditor
    /// 
    /// A few notes: 
    /// a) The editor class must have a default constructor.  This is what will be used to construct the editor by the framework.
    /// b) The editor must have the same namespace as the type it is editing.
    /// 
    /// </remarks>
    public interface IWorkspaceUnitEditor
    {
        /// <summary>
        /// The data of the Workspace Unit that is being edited.
        /// </summary>
        object Data { get; set; }
    }
}
