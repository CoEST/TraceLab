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
using System.Windows.Controls;
using System.Windows;
using TraceLab.Core.Experiments;
using System.Windows.Markup;

namespace TraceLab.UI.WPF.Controls
{
    public sealed class ExperimentBreadcrumb : Control
    {
        public static readonly DependencyProperty GathererProperty;
        public static readonly DependencyProperty CrumbsProperty;
        public static readonly DependencyProperty SourceProperty;

        static ExperimentBreadcrumb()
        {
            GathererProperty = DependencyProperty.Register("Gatherer", typeof(CrumbGatherer), typeof(ExperimentBreadcrumb), new UIPropertyMetadata(OnGathererChanged));
            SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ExperimentBreadcrumb), new UIPropertyMetadata(OnSourceChanged));
            CrumbsProperty = DependencyProperty.Register("Crumbs", typeof(Crumb[]), typeof(ExperimentBreadcrumb), new UIPropertyMetadata(null, OnCrumbsChanged, CoerceCrumbs));
        }

      
        private static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ExperimentBreadcrumb control = (ExperimentBreadcrumb)sender;
            control.CoerceValue(CrumbsProperty);
        }

        private static void OnCrumbsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ExperimentBreadcrumb control = (ExperimentBreadcrumb)sender;
            if (control.Crumbs != null && control.Crumbs.Length > 0)
            {
                control.Crumbs[control.Crumbs.Length - 1].IsCurrent = true;
                foreach (Crumb crumb in control.Crumbs)
                {
                    crumb.IsNested = true;
                }
                control.Crumbs[0].IsNested = false;
            }
        }

        private static object CoerceCrumbs(DependencyObject d, object value)
        {
            ExperimentBreadcrumb control = (ExperimentBreadcrumb)d;
            if(control.Gatherer != null)
            {
                return control.Gatherer.GatherCrumbs(control.Source);
            }

            return null;
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public Crumb[] Crumbs
        {
            get { return (Crumb[])GetValue(CrumbsProperty); }
            private set { SetValue(CrumbsProperty, value); }
        }
        
        public CrumbGatherer Gatherer
        {
            get { return (CrumbGatherer)GetValue(GathererProperty); }
            set { SetValue(GathererProperty, value); }
        }

        private static void OnGathererChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ExperimentBreadcrumb control = (ExperimentBreadcrumb)sender;
            control.CoerceValue(CrumbsProperty);
        }
    }

    public abstract class CrumbGatherer
    {
        public abstract Crumb[] GatherCrumbs(object source);
    }

    public class Crumb : DependencyObject
    {
        public static readonly DependencyProperty DisplayTextProperty;
        public static readonly DependencyProperty ValueProperty;
        public static readonly DependencyProperty IsCurrentProperty;
        public static readonly DependencyProperty IsNestedProperty;

        static Crumb()
        {
            DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(Crumb));
            ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(Crumb));
            IsCurrentProperty = DependencyProperty.Register("IsCurrent", typeof(bool), typeof(Crumb));
            IsNestedProperty = DependencyProperty.Register("IsNested", typeof(bool), typeof(Crumb));
        }

        public Crumb(string displayText, object value)
        {
            DisplayText = displayText;
            Value = value;
        }

        public Crumb()
        {
        }

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool IsCurrent
        {
            get { return (bool)GetValue(IsCurrentProperty); }
            internal set { SetValue(IsCurrentProperty, value); }
        }

        public bool IsNested
        {
            get { return (bool)GetValue(IsNestedProperty); }
            internal set { SetValue(IsNestedProperty, value); }
        }
    }
}
