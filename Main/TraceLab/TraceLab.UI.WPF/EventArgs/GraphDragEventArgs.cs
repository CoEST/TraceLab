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

using System.Windows;
using TraceLab.UI.WPF.Views;
using TraceLab.UI.WPF.Controls;

namespace TraceLab.UI.WPF.EventArgs
{
    public class GraphDragEventArgs
    {
        public GraphDragEventArgs(NodeGraphLayout canvas, DragEventArgs args)
        {
            Point canvasPoint = args.GetPosition(canvas);
            canvasPoint = new Point(canvasPoint.X - canvas.Translation.X, canvasPoint.Y - canvas.Translation.Y);
            Position = canvasPoint;

            DragArguments = args;
        }

        public Point Position
        {
            get;
            private set;
        }

        public DragEventArgs DragArguments
        {
            get;
            private set;
        }
    }
}
