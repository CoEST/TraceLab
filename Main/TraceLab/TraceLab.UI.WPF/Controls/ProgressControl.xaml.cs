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
using System.Windows.Controls;
using System.Windows.Media;
using TraceLab.UI.WPF.Utilities;
using TraceLabSDK;
using System.Windows;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ProgressControl.xaml
    /// </summary>
    public partial class ProgressControl : UserControl, IProgress
    {
        public static readonly DependencyProperty StatusVisibilityProperty = DependencyProperty.Register("StatusVisibility", typeof(Visibility), typeof(ProgressControl), new UIPropertyMetadata(Visibility.Visible));

        public ProgressControl()
        {
            InitializeComponent();
        }

        public Visibility StatusVisibility
        {
            get { return (Visibility)GetValue(StatusVisibilityProperty); }
            set { SetValue(StatusVisibilityProperty, value); }
        }

        private delegate object GetterDelegate();

        public bool IsIndeterminate
        {
            get
            {
                GetterDelegate getter = () => { return Progress.IsIndeterminate; };
                return (bool)this.EnsureThread(getter);
            }
            set
            {
                Action setter = () => { Progress.IsIndeterminate = value; };
                this.EnsureThread(setter, null);
            }
        }

        public double NumSteps
        {
            get
            {
                GetterDelegate getter = () => { return Progress.Maximum; };
                return (double)this.EnsureThread(getter);
            }
            set
            {
                Action setter = () => { Progress.Maximum = value; };
                this.EnsureThread(setter, null);
            }
        }

        public double CurrentStep
        {
            get
            {
                GetterDelegate getter = () => { return Progress.Value; };
                return (double)this.EnsureThread(getter);
            }
            set
            {
                Action setter = () => { Progress.Value = value; };
                this.EnsureThread(setter, null);
            }
        }

        public string CurrentStatus
        {
            get
            {
                GetterDelegate getter = () => { return StatusMessage.Content; };
                return this.EnsureThread(getter) as string;
            }
            set
            {
                Action setter = () => { StatusMessage.Content = value; };
                this.EnsureThread(setter, null);
            }
        }

        public void Reset()
        {
            Action resetter = () =>
                {
                    Progress.Maximum = 1;
                    Progress.Value = 0;
                    Progress.IsIndeterminate = false;
                    StatusMessage.Content = string.Empty;
                };

            this.EnsureThread(resetter, null);
        }

        public void Increment()
        {
            Action incrementer = () =>
            {
                Progress.Value += 1;
            };

            this.EnsureThread(incrementer, null);
        }

        public void SetError(bool hasError)
        {
            Action setter = () =>
            {
                if (hasError)
                    Progress.Background = new SolidColorBrush(Colors.Red);
                else
                    Progress.ClearValue(ProgressBar.BackgroundProperty);
            };

            this.EnsureThread(setter, null);
        }
    }
}
