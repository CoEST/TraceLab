﻿//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for Calendar Control
    /// </summary>
    public sealed class CalendarAutomationPeer : FrameworkElementAutomationPeer, IGridProvider, IMultipleViewProvider, ISelectionProvider, ITableProvider
    {
        /// <summary>
        /// Initializes a new instance of the CalendarAutomationPeer class.
        /// </summary>
        /// <param name="owner">Owning Calendar</param>
        public CalendarAutomationPeer(Calendar owner)
            : base(owner)
        {
        }

        #region Private Properties

        private Calendar OwningCalendar
        {
            get
            {
                return this.Owner as Calendar;
            }
        }

        private Grid OwningGrid
        {
            get
            {
                if (this.OwningCalendar != null && this.OwningCalendar.MonthControl != null)
                {
                    if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
                    {
                        return this.OwningCalendar.MonthControl.MonthView;
                    }
                    else
                    {
                        return this.OwningCalendar.MonthControl.YearView;
                    }
                }

                return null;
            }
        }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Gets the control pattern that is associated with the specified System.Windows.Automation.Peers.PatternInterface.
        /// </summary>
        /// <param name="patternInterface">A value from the System.Windows.Automation.Peers.PatternInterface enumeration.</param>
        /// <returns>The object that supports the specified pattern, or null if unsupported.</returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Grid:
                case PatternInterface.Table:
                case PatternInterface.MultipleView:
                case PatternInterface.Selection:
                    {
                        if (this.OwningGrid != null)
                        {
                            return this;
                        }

                        break;
                    }

                default: break;
            }

            return base.GetPattern(patternInterface);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Calendar;
        }

        /// <summary>
        /// Called by GetClassName that gets a human readable name that, in addition to AutomationControlType, 
        /// differentiates the control represented by this AutomationPeer.
        /// </summary>
        /// <returns>The string that contains the name.</returns>
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        #endregion Protected Methods

        #region InternalMethods

        internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
        {
            int numSelected = this.OwningCalendar.SelectedDates.Count;
            int numAdded = e.AddedItems.Count;

            if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && numSelected == 1 && numAdded == 1)
            {
                CalendarDayButton selectedButton = this.OwningCalendar.FindDayButtonFromDay((DateTime)e.AddedItems[0]);

                if (selectedButton != null)
                {
                    AutomationPeer peer = FrameworkElementAutomationPeer.FromElement(selectedButton);

                    if (peer != null)
                    {
                        peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
                    }
                }
            }
            else
            {
                if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
                {
                    foreach (DateTime date in e.AddedItems)
                    {
                        CalendarDayButton selectedButton = this.OwningCalendar.FindDayButtonFromDay(date);

                        if (selectedButton != null)
                        {
                            AutomationPeer peer = FrameworkElementAutomationPeer.FromElement(selectedButton);

                            if (peer != null)
                            {
                                peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
                            }
                        }
                    }
                }

                if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
                {
                    foreach (DateTime date in e.RemovedItems)
                    {
                        CalendarDayButton removedButton = this.OwningCalendar.FindDayButtonFromDay(date);

                        if (removedButton != null)
                        {
                            AutomationPeer peer = FrameworkElementAutomationPeer.FromElement(removedButton);

                            if (peer != null)
                            {
                                peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
                            }
                        }
                    }
                }
            }
        }

        #endregion InternalMethods

        #region IGridProvider

        int IGridProvider.ColumnCount
        {
            get
            {
                if (this.OwningGrid != null)
                {
                    return this.OwningGrid.ColumnDefinitions.Count;
                }

                return 0;
            }
        }

        int IGridProvider.RowCount
        {
            get
            {
                if (this.OwningGrid != null)
                {
                    if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
                    {
                        // In Month DisplayMode, since first row is DayTitles, we return the RowCount-1
                        return Math.Max(0, this.OwningGrid.RowDefinitions.Count - 1);
                    }
                    else
                    {
                        return this.OwningGrid.RowDefinitions.Count;
                    }
                }

                return 0;
            }
        }

        IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
        {
            if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
            {
                // In Month DisplayMode, since first row is DayTitles, we increment the row number by 1
                row++;
            }

            if (this.OwningGrid != null && row >= 0 && row < this.OwningGrid.RowDefinitions.Count && column >= 0 && column < this.OwningGrid.ColumnDefinitions.Count)
            {
                foreach (UIElement child in this.OwningGrid.Children)
                {
                    int childRow = (int)child.GetValue(Grid.RowProperty);
                    int childColumn = (int)child.GetValue(Grid.ColumnProperty);
                    if (childRow == row && childColumn == column)
                    {
                        AutomationPeer peer = CreatePeerForElement(child);
                        if (peer != null)
                        {
                            return ProviderFromPeer(peer);
                        }
                    }
                }
            }

            return null;
        }

        #endregion IGridProvider

        #region IMultipleViewProvider

        int IMultipleViewProvider.CurrentView 
        { 
            get 
            { 
                return (int)this.OwningCalendar.DisplayMode; 
            } 
        }

        int[] IMultipleViewProvider.GetSupportedViews()
        {
            int[] supportedViews = new int[3];

            supportedViews[0] = (int)CalendarMode.Month;
            supportedViews[1] = (int)CalendarMode.Year;
            supportedViews[2] = (int)CalendarMode.Decade;

            return supportedViews;
        }

        string IMultipleViewProvider.GetViewName(int viewId)
        {
            switch (viewId)
            {
                case 0:
                    {
                        return SR.Get(SRID.CalendarAutomationPeer_MonthMode);
                    }

                case 1:
                    {
                        return SR.Get(SRID.CalendarAutomationPeer_YearMode);
                    }

                case 2:
                    {
                        return SR.Get(SRID.CalendarAutomationPeer_DecadeMode);
                    }
            }

            // TODO: update when Jolt 23302 is fixed
            // throw new ArgumentOutOfRangeException("viewId", Resource.Calendar_OnDisplayModePropertyChanged_InvalidValue);
            return String.Empty;
        }

        void IMultipleViewProvider.SetCurrentView(int viewId)
        {
            this.OwningCalendar.DisplayMode = (CalendarMode)viewId;
        }

        #endregion IMultipleViewProvider

        #region ISelectionProvider

        bool ISelectionProvider.CanSelectMultiple
        {
            get
            {
                return this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleRange || this.OwningCalendar.SelectionMode == CalendarSelectionMode.MultipleRange;
            }
        }

        bool ISelectionProvider.IsSelectionRequired 
        { 
            get 
            { 
                return false; 
            } 
        }

        IRawElementProviderSimple[] ISelectionProvider.GetSelection()
        {
            List<IRawElementProviderSimple> providers = new List<IRawElementProviderSimple>();

            if (this.OwningGrid != null)
            {
                if (this.OwningCalendar.DisplayMode == CalendarMode.Month && this.OwningCalendar.SelectedDates != null && this.OwningCalendar.SelectedDates.Count != 0)
                {
                    // return selected DayButtons
                    CalendarDayButton dayButton;

                    foreach (UIElement child in this.OwningGrid.Children)
                    {
                        int childRow = (int)child.GetValue(Grid.RowProperty);

                        if (childRow != 0)
                        {
                            dayButton = child as CalendarDayButton;

                            if (dayButton != null && dayButton.IsSelected)
                            {
                                AutomationPeer peer = CreatePeerForElement(dayButton);

                                if (peer != null)
                                {
                                    providers.Add(ProviderFromPeer(peer));
                                }
                            }
                        }
                    }
                }
                else
                {
                    // return the CalendarButton which has focus
                    CalendarButton calendarButton;

                    foreach (UIElement child in this.OwningGrid.Children)
                    {
                        calendarButton = child as CalendarButton;

                        if (calendarButton != null && calendarButton.IsFocused)
                        {
                            AutomationPeer peer = CreatePeerForElement(calendarButton);

                            if (peer != null)
                            {
                                providers.Add(ProviderFromPeer(peer));
                            }

                            break;
                        }
                    }
                }

                if (providers.Count > 0)
                {
                    return providers.ToArray();
                }
            }

            return null;
        }

        #endregion ISelectionProvider

        #region ITableProvider

        RowOrColumnMajor ITableProvider.RowOrColumnMajor
        {
            get
            {
                return RowOrColumnMajor.RowMajor;
            }
        }

        IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
        {
            if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
            {
                List<IRawElementProviderSimple> providers = new List<IRawElementProviderSimple>();

                foreach (UIElement child in this.OwningGrid.Children)
                {
                    int childRow = (int)child.GetValue(Grid.RowProperty);

                    if (childRow == 0)
                    {
                        AutomationPeer peer = CreatePeerForElement(child);

                        if (peer != null)
                        {
                            providers.Add(ProviderFromPeer(peer));
                        }
                    }
                }

                if (providers.Count > 0)
                {
                    return providers.ToArray();
                }
            }

            return null;
        }

        // If WeekNumber functionality is supported by Calendar in the future,
        // this method should return weeknumbers
        IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
        {
            return null;
        }

        #endregion ITableProvider
    }
}
