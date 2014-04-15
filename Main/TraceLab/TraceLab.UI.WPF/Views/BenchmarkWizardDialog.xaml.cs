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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TraceLab.UI.WPF.ViewModels;
using System.Windows.Media.Animation;
using TraceLab.UI.WPF.Utilities;
using TraceLab.Core.Experiments;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for BenchmarkWizard.xaml
    /// </summary>
    public partial class BenchmarkWizardDialog : Window
    {
        private double m_oldLeft;
        private double m_oldTop;
        private double m_oldHeight;
        private double m_oldWidth;
        private bool m_isShowingProcessArea;

        public static readonly DependencyProperty OpenComponentGraphCommandProperty = DependencyProperty.Register("OpenComponentGraphCommand", typeof(ICommand), typeof(BenchmarkWizardDialog));
        public static readonly DependencyProperty ShowProcessAreaProperty = DependencyProperty.Register("ShowProcessArea", typeof(bool), typeof(BenchmarkWizardDialog), new UIPropertyMetadata(OnShowProcessAreaChanged));
        public static readonly DependencyProperty ToggleInfoPaneForNodeProperty = DependencyProperty.Register("ToggleInfoPaneForNodeCommand", typeof(ICommand), typeof(BenchmarkWizardDialog));

        public static readonly DependencyProperty ListViewSelectBenchmarkCommand = BehaviourFactory.CreateCommandExecutionEventBehaviour(
                                                                                                      ListViewItem.MouseDoubleClickEvent, "ListViewSelectBenchmarkCommand", typeof(BenchmarkWizardDialog));

        public static void SetListViewSelectBenchmarkCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(ListViewSelectBenchmarkCommand, value);
        }

        public static ICommand GetListViewSelectBenchmarkCommand(DependencyObject o)
        {
            return o.GetValue(ListViewSelectBenchmarkCommand) as ICommand;
        }   

        static BenchmarkWizardDialog()
        {
        }

        public BenchmarkWizardDialog()
        {
            InitializeComponent();

            AddHandler(ComboBox.SelectionChangedEvent, new RoutedEventHandler(ComboBox_SelectionChanged));
        }

        public BenchmarkWizardDialog(Window parent)
            : this()
        {
            Owner = parent;
        }

        public ICommand ToggleInfoPaneForNodeCommand
        {
            get { return (ICommand)GetValue(ToggleInfoPaneForNodeProperty); }
            set { SetValue(ToggleInfoPaneForNodeProperty, value); }
        }

        public ICommand OpenComponentGraphCommand
        {
            get { return (ICommand)GetValue(OpenComponentGraphCommandProperty); }
            set { SetValue(OpenComponentGraphCommandProperty, value); }
        }

        private static void OnShowProcessAreaChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            BenchmarkWizardDialog wizard = (BenchmarkWizardDialog)sender;
            bool? value = args.NewValue as bool?;
            if (value.HasValue && wizard.m_isShowingProcessArea != value.Value)
            {
                wizard.m_isShowingProcessArea = value.Value;
                if (value.Value)
                {
                    wizard.m_oldLeft = wizard.Left;
                    wizard.m_oldTop = wizard.Top;
                    wizard.m_oldHeight = 75;
                    wizard.m_oldWidth = 195;

                    wizard.AnimateToSize(wizard.Owner.Left, wizard.Owner.Top, 0, 0);
                }
                else
                {
                    wizard.AnimateToSize(wizard.m_oldLeft, wizard.m_oldTop, wizard.m_oldHeight, wizard.m_oldWidth);
                }
            }
        }

        public bool ShowProcessArea
        {
            get { return (bool)GetValue(ShowProcessAreaProperty); }
            set
            {
                SetValue(ShowProcessAreaProperty, value);
            }
        }

        private void AnimateToSize(double left, double top, double height, double width)
        {
            var heightAnim = new GridLengthAnimation(headerRowDefinition.Height, new GridLength(height), new Duration(TimeSpan.FromMilliseconds(500)));
            var widthAnim = new GridLengthAnimation(keyBorderColumnDefinition.Width, new GridLength(width), new Duration(TimeSpan.FromMilliseconds(500)));

            Storyboard.SetTargetName(heightAnim, "headerRowDefinition");
            Storyboard.SetTargetProperty(heightAnim, new PropertyPath(RowDefinition.HeightProperty));

            Storyboard.SetTargetName(widthAnim, "keyBorderColumnDefinition");
            Storyboard.SetTargetProperty(widthAnim, new PropertyPath(ColumnDefinition.WidthProperty));

            var story2 = new Storyboard();
            story2.Children.Add(heightAnim);
            story2.Children.Add(widthAnim);
            story2.Begin(this);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //ComboBox combo = (ComboBox)sender;
            //object data = combo.DataContext;
        }

        private void InputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void executeBenchmarkProgress_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Open Component Graph Command

        private void ExecuteOpenComponentGraphCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                OpenComponentGraphCommand.Execute(e.Parameter);
            }
        }

        private void CanExecuteOpenComponentGraphCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (OpenComponentGraphCommand != null)
            {
                e.CanExecute = OpenComponentGraphCommand.CanExecute(e.Parameter);
            }
        }

        #endregion

        #region Execute Toggle Node Info

        private void ExecuteToggleNodeInfo(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleInfoPaneForNodeCommand.Execute(e.Parameter);
        }

        #endregion

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
