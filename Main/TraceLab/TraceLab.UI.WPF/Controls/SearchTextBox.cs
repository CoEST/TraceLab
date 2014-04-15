﻿// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace TraceLab.UI.WPF.Controls
{
    public enum SearchMode
    {
        Instant,
        Delayed,
    }

    /// <summary>
    /// This control was originally written by David Owens II - from his blog at 
    /// http://davidowens.wordpress.com/2009/02/18/wpf-search-text-box/
    /// </summary>
    public class SearchTextBox : TextBox
    {
        public static DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox));

        public static DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox));

        public static DependencyProperty SearchModeProperty =
            DependencyProperty.Register(
                "SearchMode",
                typeof(SearchMode),
                typeof(SearchTextBox),
                new PropertyMetadata(SearchMode.Instant));

        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        private static DependencyPropertyKey IsMouseLeftButtonDownPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsMouseLeftButtonDown",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());
        public static DependencyProperty IsMouseLeftButtonDownProperty = IsMouseLeftButtonDownPropertyKey.DependencyProperty;

        public static DependencyProperty SearchEventTimeDelayProperty =
            DependencyProperty.Register(
                "SearchEventTimeDelay",
                typeof(Duration),
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(
                    new Duration(new TimeSpan(0, 0, 0, 0, 500)),
                    new PropertyChangedCallback(OnSearchEventTimeDelayChanged)));

        public static readonly RoutedEvent SearchEvent = 
            EventManager.RegisterRoutedEvent(
                "Search",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SearchTextBox));

        static SearchTextBox() {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }

        private DispatcherTimer searchEventDelayTimer;

        public SearchTextBox()
            : base() {
            searchEventDelayTimer = new DispatcherTimer();
            searchEventDelayTimer.Interval = SearchEventTimeDelay.TimeSpan;
            searchEventDelayTimer.Tick += new EventHandler(OnSeachEventDelayTimerTick);
        }

        void OnSeachEventDelayTimerTick(object o, System.EventArgs e) {
            searchEventDelayTimer.Stop();
            RaiseSearchEvent();
        }

        static void OnSearchEventTimeDelayChanged(
            DependencyObject o, DependencyPropertyChangedEventArgs e) {
            SearchTextBox stb = o as SearchTextBox;
            if (stb != null) {
                stb.searchEventDelayTimer.Interval = ((Duration)e.NewValue).TimeSpan;
                stb.searchEventDelayTimer.Stop();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e) {
            base.OnTextChanged(e);
            
            HasText = Text.Length != 0;

            if (SearchMode == SearchMode.Instant) {
                searchEventDelayTimer.Stop();
                searchEventDelayTimer.Start();
            }
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            Border iconBorder = GetTemplateChild("PART_SearchIconBorder") as Border;
            if (iconBorder != null) {
                iconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(IconBorder_MouseLeftButtonDown);
                iconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(IconBorder_MouseLeftButtonUp);
                iconBorder.MouseLeave += new MouseEventHandler(IconBorder_MouseLeave);
            }
        }

        private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e) {
            IsMouseLeftButtonDown = true;
        }

        private void IconBorder_MouseLeftButtonUp(object obj, MouseButtonEventArgs e) {
            if (!IsMouseLeftButtonDown) return;

            if (HasText && SearchMode == SearchMode.Instant) {
                this.Text = "";
            }

            if (HasText && SearchMode == SearchMode.Delayed) {
                RaiseSearchEvent();
            }

            IsMouseLeftButtonDown = false;
        }

        private void IconBorder_MouseLeave(object obj, MouseEventArgs e) {
            IsMouseLeftButtonDown = false;
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.Escape && SearchMode == SearchMode.Instant) {
                this.Text = "";
            }
            else if ((e.Key == Key.Return || e.Key == Key.Enter) && 
                SearchMode == SearchMode.Delayed) {
                RaiseSearchEvent();
            }
            else {
                base.OnKeyDown(e);
            }
        }

        private void RaiseSearchEvent() {
            RoutedEventArgs args = new RoutedEventArgs(SearchEvent);
            RaiseEvent(args);
        }

        public string LabelText {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public SearchMode SearchMode {
            get { return (SearchMode)GetValue(SearchModeProperty); }
            set { SetValue(SearchModeProperty, value); }
        }

        public bool HasText {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        public Duration SearchEventTimeDelay {
            get { return (Duration)GetValue(SearchEventTimeDelayProperty); }
            set { SetValue(SearchEventTimeDelayProperty, value); }
        }

        public bool IsMouseLeftButtonDown {
            get { return (bool)GetValue(IsMouseLeftButtonDownProperty); }
            private set { SetValue(IsMouseLeftButtonDownPropertyKey, value); }
        }

        public event RoutedEventHandler Search {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }
    }
}
