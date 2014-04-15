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
using System.Windows.Threading;
using System.Timers;

namespace TraceLab.UI.WPF.Controls
{
    public partial class EditableTextBlock : UserControl
    {
        #region Constructor

        static EditableTextBlock()
        {
            int doubleClickTime = System.Windows.Forms.SystemInformation.DoubleClickTime + 1;
            m_delayEditTimerMilliseconds = doubleClickTime;
            m_delayEditableChangeMilliseconds = doubleClickTime;
        }

        public EditableTextBlock()
        {
            InitializeComponent();
            base.Focusable = true;
            base.FocusVisualStyle = null;

            InitDelayEditTimer();

            InitDelayEditableChangeTimer();
        }

        #endregion Constructor

        #region Member Variables

        // We keep the old text when we go into editmode
        // in case the user aborts with the escape key
        private string oldText;

        #endregion Member Variables

        #region Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlock), new UIPropertyMetadata());

        public bool IsInEditMode
        {
            get
            {
                if (IsEditable)
                    return (bool)GetValue(IsInEditModeProperty);
                else
                    return false;
            }
            set
            {
                if (value) oldText = Text;
                SetValue(IsInEditModeProperty, value);
            }
        }

        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false));

        #region IsEditable Property

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableTextBlock), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnEditableChanged)));

        /// <summary>
        /// Gets or sets a value indicating whether editing is enabled with slight delayed.
        /// It allows preventing immidiate editing with double click on text block when node was not selected. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabling editing is slightly delayed; otherwise, <c>false</c>.
        /// </value>
        public bool DelayEditableChange
        {
            get { return (bool)GetValue(DelayEditableChangeProperty); }
            set { SetValue(DelayEditableChangeProperty, value); }
        }

        public static readonly DependencyProperty DelayEditableChangeProperty =
            DependencyProperty.Register("DelayEditableChange", typeof(bool), typeof(EditableTextBlock), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Called when IsEditable property is changed. If DelayEditableChange is set to true it will postpone setting private field allowEdit to true.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        static void OnEditableChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = o as EditableTextBlock;
            if (textBlock != null)
            {
                if ((bool)e.NewValue == true)
                {
                    if (textBlock.DelayEditableChange)
                    {
                        //set m_allowEdit to true after short delay
                        textBlock.m_delayEditableChangeTimer.Start();
                    }
                    else
                    {
                        textBlock.m_allowEdit = true;
                    }
                }
                else
                {
                    textBlock.m_allowEdit = false;
                }
            }
        }

        #endregion IsEditable Property

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Loaded event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Give the TextBox input focus
            txt.Focus();

            txt.SelectAll();
        }

        /// <summary>
        /// Handles the LostFocus event of the TextBox control.
        /// Exit edit mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsInEditMode = false;
        }

        /// <summary>
        /// Handles the KeyDown event of the TextBox control.
        /// Invoked when the user edits the annotation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //don't allow empty text
                if(String.IsNullOrWhiteSpace(Text)) 
                {
                    Text = oldText;
                }
                this.IsInEditMode = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                this.IsInEditMode = false;
                Text = oldText;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the TextBlock control.
        /// Stop the edit timer, if not yet entered to EditMode. Prevents showing edit mode if mouse leaves the box before timer tick.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if(m_delayEditTimer.IsEnabled)
                m_delayEditTimer.Stop();
        }

        /// <summary>
        /// Handles the MouseMove event of the TextBlock control.
        /// Stop the edit timer, if not yet entered to EditMode. Prevents showing edit mode if user executed dragging with mouse pressed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_delayEditTimer.IsEnabled)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    m_delayEditTimer.Stop();
                }
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the TextBlock control. 
        /// Enters EditMode if editing is allowed and with slight delay (milisecond longer than double mouse click). 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (m_allowEdit)
            {
                if (m_delayEditTimer.IsEnabled)
                {
                    //prevents action on double click, when node is already selected
                    m_delayEditTimer.Stop();
                }
                else
                {
                    //change IsInEditMode = true after short delay
                    m_delayEditTimer.Start();
                }
            }
        }

        #endregion Event Handlers

        #region Delay Timers

        private bool m_allowEdit = false;
        private DispatcherTimer m_delayEditTimer;
        private static int m_delayEditTimerMilliseconds;
        private DispatcherTimer m_delayEditableChangeTimer;
        private static int m_delayEditableChangeMilliseconds;

        /// <summary>
        /// Inits the delay editable change timer.
        /// Timer responsible for delaying allowing editing. (useful when editable is dependent whether node is selected or not)
        /// Prevents immidiate editing when user double clicks unselected node.
        /// </summary>
        private void InitDelayEditableChangeTimer()
        {
            m_delayEditableChangeTimer = new DispatcherTimer();
            m_delayEditableChangeTimer.Interval = TimeSpan.FromMilliseconds(m_delayEditTimerMilliseconds);
            
            //delays changing allow edit to true
            m_delayEditableChangeTimer.Tick +=
                (sender, eventArgs) =>
                {
                    m_allowEdit = true;
                    m_delayEditableChangeTimer.Stop();
                };
        }

        /// <summary>
        /// Inits the delay edit timer.
        /// Timer responsible for delaying changing into edit mode. It delays it just so that it prevents executing single click and double click
        /// at the same time. (when done on selected node)
        /// </summary>
        private void InitDelayEditTimer()
        {
            m_delayEditTimer = new DispatcherTimer();
            m_delayEditTimer.Interval = TimeSpan.FromMilliseconds(m_delayEditTimerMilliseconds);
            
            //delays changing edit mode to true
            m_delayEditTimer.Tick +=
                (sender, eventArgs) =>
                {
                    IsInEditMode = true;
                    m_delayEditTimer.Stop();
                };
        }

        #endregion
    }
}
