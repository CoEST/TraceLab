using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using TraceLab.UI.WPF.Utilities;
using System.Windows.Controls;
using System.Windows;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Controls
{
    class RoutePointThumb : Thumb
    {
        #region Point
        /// <summary>
        /// Point Dependency Property
        /// </summary>
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
            "Point", 
            typeof(RoutePoint), 
            typeof(RoutePointThumb),
            new FrameworkPropertyMetadata(new RoutePoint()));

        /// <summary>
        /// Gets or sets the Point property
        /// </summary>
        public RoutePoint Point
        {
            get { return (RoutePoint)GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }
        #endregion

        static RoutePointThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoutePointThumb), new FrameworkPropertyMetadata(typeof(RoutePointThumb)));
        }

        public RoutePointThumb()
        {
            this.DragDelta += new DragDeltaEventHandler(this.OnDragDelta);
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            //this.Point = new Point(this.Point.X + e.HorizontalChange, this.Point.Y + e.VerticalChange);
            Point.X += e.HorizontalChange;
            Point.Y += e.VerticalChange;
        }
    }
}
