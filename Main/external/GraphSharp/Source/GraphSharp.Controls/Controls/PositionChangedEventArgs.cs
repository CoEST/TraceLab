﻿using System.Diagnostics.Contracts;
using System.Windows;

namespace GraphSharp.Controls
{
    public class PositionChangedEventArgs : RoutedEventArgs
    {
        public double XChange { get; private set; }
        public double YChange { get; private set; }

        public PositionChangedEventArgs(RoutedEvent evt, object source, double xChange, double yChange)
            : base(evt, source)
        {
            Contract.Requires(source != null);

            XChange = xChange;
            YChange = yChange;
        }
    }
}
