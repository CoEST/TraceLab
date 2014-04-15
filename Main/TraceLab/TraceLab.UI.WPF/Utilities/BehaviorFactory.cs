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
using System.Windows.Input;

namespace TraceLab.UI.WPF.Utilities
{

    /// <summary>
    /// This code was written by Samuel Jack, taken from http://archive.msdn.microsoft.com/eventbehaviourfactor
    /// </summary>
    public static class BehaviourFactory
    {
        public static DependencyProperty CreateCommandExecutionEventBehaviour(RoutedEvent routedEvent, string propertyName, Type ownerType)
        {
            DependencyProperty property = DependencyProperty.RegisterAttached(propertyName, typeof(ICommand), ownerType,
                                                               new PropertyMetadata(null,
                                                                   new ExecuteCommandOnRoutedEventBehaviour(routedEvent).PropertyChangedHandler));

            return property;
        }

        /// <summary>
        /// An internal class to handle listening for an event and executing a command,
        /// when a Command is assigned to a particular DependencyProperty
        /// </summary>
        private class ExecuteCommandOnRoutedEventBehaviour : ExecuteCommandBehaviour
        {
            private readonly RoutedEvent _routedEvent;

            public ExecuteCommandOnRoutedEventBehaviour(RoutedEvent routedEvent)
            {
                _routedEvent = routedEvent;
            }

            /// <summary>
            /// Handles attaching or Detaching Event handlers when a Command is assigned or unassigned
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue)
            {
                UIElement element = sender as UIElement;
                if (element == null) { return; }

                if (oldValue != null)
                {
                    element.RemoveHandler(_routedEvent, new RoutedEventHandler(EventHandler));
                }

                if (newValue != null)
                {
                    element.AddHandler(_routedEvent, new RoutedEventHandler(EventHandler));
                }
            }

            protected void EventHandler(object sender, RoutedEventArgs e)
            {
                HandleEvent(sender, e);
            }
        }

        internal abstract class ExecuteCommandBehaviour
        {
            protected DependencyProperty _property;
            protected abstract void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue);

            protected void HandleEvent(object sender, System.EventArgs e)
            {
                DependencyObject dp = sender as DependencyObject;
                if (dp == null)
                {
                    return;
                }

                ICommand command = dp.GetValue(_property) as ICommand;

                if (command == null)
                {
                    return;
                }

                if (command.CanExecute(e))
                {
                    command.Execute(e);
                }
            }

            /// <summary>
            /// Listens for a change in the DependencyProperty that we are assigned to, and
            /// adjusts the EventHandlers accordingly
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void PropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                // the first time the property changes,
                // make a note of which property we are supposed
                // to be watching
                if (_property == null)
                {
                    _property = e.Property;
                }

                object oldValue = e.OldValue;
                object newValue = e.NewValue;

                AdjustEventHandlers(sender, oldValue, newValue);
            }
        }
    }
}
