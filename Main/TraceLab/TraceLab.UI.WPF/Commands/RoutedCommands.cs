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
using System.Windows.Input;

namespace TraceLab.UI.WPF.Commands
{
    /// <summary>
    /// Routed commands essentially allow executing command via 'route' - a path within
    /// visual tree. 
    /// 
    /// In other words it is like a bridge from any element in the visual tree to the command 
    /// that has been binded via command binding in another element. 
    /// 
    /// For example in xaml of DockableGraph and ScopeGraph there are command binding of static RoutedCommands.RemoveNodeCommand 
    /// to the handlers in GraphView base class. 
    /// 
    /// Since that routed command is static it allows easy way of accessing it from all buttons in application. 
    /// In this case, all buttons on node controls have 'trash' button to remove the node. For example you may find it in ComponentNodeControl or DecisionNodeControl, etc.
    /// 
    /// Good easy explanation of RoutedCommand can be found in the following article:
    /// http://joshsmithonwpf.wordpress.com/2008/03/18/understanding-routed-commands/
    /// </summary>
    public static class RoutedCommands
    {
        public static readonly RoutedCommand RemoveNodeCommand;
        public static readonly RoutedCommand RemoveSelectedNodesCommand;
        public static readonly RoutedCommand ToggleLogLevelCommand;
        public static readonly RoutedCommand ToggleNodeInfoCommand;
        public static readonly RoutedCommand CompileDecisionModuleCommand;
        public static readonly RoutedCommand AppendCodeTokenCommand;
        public static readonly RoutedCommand OpenComponentGraphCommand;
        public static readonly RoutedCommand CreateConnectionCommand;
        public static readonly RoutedCommand DefineCompositeComponentCommand;
        public static readonly RoutedCommand AddScopeToDecisionCommand;
        public static readonly RoutedCommand CaptureScreenShotCommand;

        /// <summary>
        /// Initializes all routed commands. 
        /// </summary>
        static RoutedCommands()
        {
            RemoveNodeCommand = new RoutedCommand("RemoveNode", typeof(RoutedCommands));
            RemoveSelectedNodesCommand = new RoutedCommand("RemoveSelectedNodes", typeof(RoutedCommand));
            CompileDecisionModuleCommand = new RoutedCommand("CompileDecisionModule", typeof(RoutedCommands));
            ToggleLogLevelCommand = new RoutedCommand("ToggleLogLevel", typeof(RoutedCommands));
            AppendCodeTokenCommand = new RoutedCommand("AppendCodeToken", typeof(RoutedCommands));
            ToggleNodeInfoCommand = new RoutedCommand("ToggleNodeInfo", typeof(RoutedCommands));
            OpenComponentGraphCommand = new RoutedCommand("OpenComponentGraph", typeof(RoutedCommand));
            CreateConnectionCommand = new RoutedCommand("CreateConnetion", typeof(RoutedCommand));
            DefineCompositeComponentCommand = new RoutedCommand("DefineCompositeComponent", typeof(RoutedCommand));
            AddScopeToDecisionCommand = new RoutedCommand("AddScopeToDecision", typeof(RoutedCommand));
            CaptureScreenShotCommand = new RoutedCommand("CaptureScreenShot", typeof(RoutedCommand));
        }
    }
}
