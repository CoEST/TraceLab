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
using TraceLab.UI.WPF.Commands;
using TraceLab.Core.Settings;
using System.Collections.ObjectModel;
using TraceLab.UI.WPF.ViewModels;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();

            m_moveUp = new DelegateCommand(DoMoveUp, CanMoveUp);
            m_moveDown = new DelegateCommand(DoMoveDown, CanMoveDown);
            m_addNew = new DelegateCommand(DoAddNew, CanAddNew);
            m_editExisting = new DelegateCommand(DoEditExisting, CanEditExisting);
            m_delete = new DelegateCommand(DoDelete, CanDelete);
        }

        ListBox GetSource(object p)
        {
            SettingsPathType source = (SettingsPathType)p;

            if (source == SettingsPathType.Type)
                return TypeList;
            else
                return ComponentList;
        }

        private ICommand m_moveUp;
        public ICommand MoveUp
        {
            get { return m_moveUp; }
        }

        private void DoMoveUp(object p)
        {
            var source = GetSource(p);
            SettingsPathViewModel pathVM = source.SelectedItem as SettingsPathViewModel;
            SettingsViewModel settingsVM = DataContext as SettingsViewModel;

            settingsVM.MoveUp(pathVM);
        }

        private bool CanMoveUp(object p)
        {
            var source = GetSource(p);
            ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;



            bool canMove = false;
            int index = source.SelectedIndex;
            if (index != -1)
            {
                int prev = index - 1;
                if (0 <= prev)
                {
                    SettingsPathViewModel prevItem = sourceCollection[prev] as SettingsPathViewModel;
                    SettingsPathViewModel currentItem = sourceCollection[index] as SettingsPathViewModel;
                    canMove = prevItem.IsTemporary == false && currentItem.IsTemporary == false;
                }
            }

            return canMove;
        }

        private ICommand m_moveDown;
        public ICommand MoveDown
        {
            get { return m_moveDown; }
        }


        private void DoMoveDown(object p)
        {
            var source = GetSource(p);
            SettingsPathViewModel pathVM = source.SelectedItem as SettingsPathViewModel;
            SettingsViewModel settingsVM = DataContext as SettingsViewModel;

            settingsVM.MoveDown(pathVM);
        }

        private bool CanMoveDown(object p)
        {
            var source = GetSource(p);
            ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;

            bool canMove = false;
            int index = source.SelectedIndex;
            if (index != -1)
            {
                int next = index + 1;
                if (next < sourceCollection.Count)
                {
                    SettingsPathViewModel nextItem = sourceCollection[next] as SettingsPathViewModel;
                    SettingsPathViewModel currentItem = sourceCollection[index] as SettingsPathViewModel;
                    canMove = nextItem.IsTemporary == false && currentItem.IsTemporary == false;
                }
            }

            return canMove;
        }

        private ICommand m_addNew;
        public ICommand AddNew
        {
            get { return m_addNew; }
        }

        private void DoAddNew(object p)
        {
            SettingsViewModel vm = DataContext as SettingsViewModel;
            vm.BeginAddPath((SettingsPathType)p);
        }

        private bool CanAddNew(object p)
        {
            return true;
        }

        private ICommand m_editExisting;
        public ICommand EditExisting
        {
            get { return m_editExisting; }
        }

        private void DoEditExisting(object p)
        {
            var source = GetSource(p);
            ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;
            SettingsPathViewModel vm = sourceCollection[source.SelectedIndex];

            vm.IsEditing = true;
        }

        private bool CanEditExisting(object p)
        {
            var source = GetSource(p);
            bool canEdit = false;

            if (source.SelectedIndex != -1)
            {
                ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;
                SettingsPathViewModel vm = sourceCollection[source.SelectedIndex];

                if (vm.IsEditing == false && vm.IsTemporary == false)
                {
                    canEdit = true;
                }
            }
            
            return canEdit;
        }

        private ICommand m_delete;
        public ICommand Delete
        {
            get { return m_delete; }
        }

        private void DoDelete(object p)
        {
            var source = GetSource(p);
            ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;
            SettingsPathViewModel pathVM = sourceCollection[source.SelectedIndex];
            SettingsViewModel settingsVM = DataContext as SettingsViewModel;

            settingsVM.Delete(pathVM);
        }

        private bool CanDelete(object p)
        {
            var source = GetSource(p);
            bool canDelete = false;

            if (source.SelectedIndex != -1)
            {
                ObservableCollection<SettingsPathViewModel> sourceCollection = source.ItemsSource as ObservableCollection<SettingsPathViewModel>;
                SettingsPathViewModel vm = sourceCollection[source.SelectedIndex];

                if (vm.IsTemporary == false)
                {
                    canDelete = true;
                }
            }

            return canDelete;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DockPanel s = sender as DockPanel;
            if (!s.IsKeyboardFocusWithin)
            {
                SettingsPathViewModel vm = s.DataContext as SettingsPathViewModel;
                vm.IsEditing = false;

                if (vm.IsAdded == false)
                {
                    SettingsViewModel settingVM = DataContext as SettingsViewModel;
                    settingVM.FinalizeAddPath(vm);
                }
            }
        }

        private void TEMPLATE_PART_Text_Loaded(object sender, RoutedEventArgs e)
        {
            Action action = () =>
            {
                TextBox s = (TextBox)sender;
                if (s.IsVisible)
                {
                    s.Focus();
                    s.SelectAll();
                }
            };

            Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Background, null);
        }

        private void PART_Content_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ContentControl s = (ContentControl)sender;
            SettingsPathViewModel vm = s.DataContext as SettingsPathViewModel;

            if (!vm.IsTemporary)
            {
                vm.IsEditing = true;
            }
        }

        private void TEMPLATE_PART_Text_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Return || e.Key == Key.Enter)
            //{
            //    TextBox s = (TextBox)sender;
            //    SettingsPathViewModel vm = s.DataContext as SettingsPathViewModel;

            //    // Prepare to send focus back to our parent after editing is finished.
            //    var parent = s.GetParent<ListBoxItem>(this);
            //    if (parent != null)
            //    {
            //        Dispatcher.BeginInvoke(new Action(() => { parent.Focus(); }), System.Windows.Threading.DispatcherPriority.Background, null);
            //    }

            //    // Now finish editing.
            //    if (!vm.IsTemporary)
            //    {
            //        vm.IsEditing = false;
            //    }
            //}
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var window = this.GetParent<Window>(null);
            window.DialogResult = true;
        }

        private void ComponentList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                ListBox s = (ListBox)sender;
                SettingsPathViewModel vm = s.SelectedItem as SettingsPathViewModel;

                // Now finish editing.
                if (!vm.IsTemporary)
                {
                    vm.IsEditing = !vm.IsEditing;
                }

                if (!vm.IsEditing)
                {
                    Dispatcher.BeginInvoke(new Action(() => 
                    {
                        var item = s.ItemContainerGenerator.ContainerFromItem(vm) as ListBoxItem;
                        item.Focus(); 
                    }), System.Windows.Threading.DispatcherPriority.Background, null);
                }

                e.Handled = true;
            }
        }

        private void TEMPLATE_PART_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                var s = (Button)sender;
                SettingsPathViewModel vm = s.DataContext as SettingsPathViewModel;
                vm.Path = dialog.SelectedPath;
            }
        }

        private void DefaultExperiment_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFileDialog fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            fileDialog.CheckFileExists = true;
            fileDialog.Title = "Select a experiment";
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".teml";
            fileDialog.Filter = "Experimental Graph Files|*.teml";

            bool? success = fileDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                var s = (Button)sender;
                var vm = s.DataContext as SettingsViewModel;
                vm.DefaultExperiment = fileDialog.FileName;
            }
        }

        private void DefaultExperimentsDirectory_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            bool? success = folderDialog.ShowDialog();
            if (success.HasValue && success.Value)
            {
                var s = (Button)sender;
                var vm = s.DataContext as SettingsViewModel;
                vm.DefaultExperimentsDirectory = folderDialog.SelectedPath;
            }
        }
    }
}
