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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace TraceLab.UI.WPF.Controls
{
    /// <summary>
    /// Encapsulates methods for manipulation of the visual tree.
    /// </summary>
    public static class UIHelpers
    {
        #region Constructor

        static UIHelpers()
        {
            // Register the hyperlink click event so we could launch the browser.
            // NOTE: Some member of UIHelpers must be called for this static ctor to work.
            EventManager.RegisterClassHandler(typeof(Hyperlink), Hyperlink.ClickEvent, new RoutedEventHandler(OnHyperlinkClick));
        }

        #endregion

        #region Ensure Access

        delegate object InvokeMethodDelegate(object obj, object[] parameters);

        /// <summary>
        /// Ensures the calling thread is the thread associated with the <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        public static bool EnsureAccess(MethodBase method)
        {
            return EnsureAccess((Dispatcher)null, method, null);
        }

        /// <summary>
        /// Ensures the calling thread is the thread associated with the <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static bool EnsureAccess(MethodBase method, params object[] parameters)
        {
            return EnsureAccess(null, method, null, parameters);
        }

        /// <summary>
        /// Ensures the calling thread is the thread associated with the <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static bool EnsureAccess(MethodBase method, object obj)
        {
            return EnsureAccess(null, method, obj);
        }

        /// <summary>
        /// Ensures the calling thread is the thread associated with the <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static bool EnsureAccess(MethodBase method, object obj, params object[] parameters)
        {
            return EnsureAccess(null, method, obj, parameters);
        }

        /// <summary>
        /// Ensures the calling thread is the thread associated with the <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="method">The method.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static bool EnsureAccess(Dispatcher dispatcher, MethodBase method, object obj, params object[] parameters)
        {
            return EnsureAccess(DispatcherPriority.Normal, dispatcher, method, obj, parameters);
        }

        public static bool EnsureAccess(DispatcherPriority priority, Dispatcher dispatcher, MethodBase method, object obj, params object[] parameters)
        {
            if (dispatcher == null)
            {
                DispatcherObject dispatcherObj = obj as DispatcherObject;
                if (obj != null)
                {
                    dispatcher = dispatcherObj.Dispatcher;
                }
                else if (Application.Current != null)
                {
                    dispatcher = Application.Current.Dispatcher;
                }
                else
                {
                    throw new ArgumentException("Unable to find a Dispatcher.", "dispatcher");
                }
            }

            bool hasAccess = dispatcher.CheckAccess();

            if (!hasAccess)
            {
                dispatcher.Invoke(priority,
                    new InvokeMethodDelegate(method.Invoke), obj, new object[] { parameters });
            }

            return hasAccess;
        }

        #endregion

        #region Find Elements

        /// <summary>
        /// Finds the logical ancestor according to the predicate.
        /// </summary>
        /// <param name="startElement">The start element.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static DependencyObject FindLogicalAncestor(DependencyObject startElement, Predicate<DependencyObject> condition)
        {
            DependencyObject obj = startElement;
            while ((obj != null) && !condition(obj))
            {
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return obj;
        }

        /// <summary>
        /// Finds the logical ancestor by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">The start element.</param>
        /// <returns></returns>
        public static T FindLogicalAncestorByType<T>(DependencyObject startElement) where T : DependencyObject
        {
            return (T)FindLogicalAncestor(startElement, delegate(DependencyObject o) { return (o is T); });
        }

        /// <summary>
        /// Finds the logical root.
        /// </summary>
        /// <param name="startElement">The start element.</param>
        /// <returns></returns>
        public static DependencyObject FindLogicalRoot(DependencyObject startElement)
        {
            DependencyObject obj = null;
            while (startElement != null)
            {
                obj = startElement;
                startElement = LogicalTreeHelper.GetParent(startElement);
            }
            return obj;
        }

        /// <summary>
        /// Finds the visual ancestor according to the predicate.
        /// </summary>
        /// <param name="startElement">The start element.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static DependencyObject FindVisualAncestor(DependencyObject startElement, Predicate<DependencyObject> condition)
        {
            DependencyObject obj = startElement;
            while ((obj != null) && !condition(obj))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj;
        }

        /// <summary>
        /// Finds the visual ancestor by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">The start element.</param>
        /// <returns></returns>
        public static T FindVisualAncestorByType<T>(DependencyObject startElement) where T : DependencyObject
        {
            return (T)FindVisualAncestor(startElement, delegate(DependencyObject o) { return (o is T); });
        }

        /// <summary>
        /// Finds the visual descendant.
        /// </summary>
        /// <param name="startElement">The start element.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public static DependencyObject FindVisualDescendant(DependencyObject startElement, Predicate<DependencyObject> condition)
        {
            if (startElement != null)
            {
                if (condition(startElement))
                {
                    return startElement;
                }
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(startElement); i++)
                {
                    DependencyObject obj = FindVisualDescendant(VisualTreeHelper.GetChild(startElement, i), condition);
                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the visual descendant by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startElement">The start element.</param>
        /// <returns></returns>
        public static T FindVisualDescendantByType<T>(DependencyObject startElement) where T : DependencyObject
        {
            return (T)FindVisualDescendant(startElement, delegate(DependencyObject o) { return (o is T); });
        }

        /// <summary>
        /// Finds the visual root.
        /// </summary>
        /// <param name="startElement">The start element.</param>
        /// <returns></returns>
        public static DependencyObject FindVisualRoot(DependencyObject startElement)
        {
            return FindVisualAncestor(startElement, delegate(DependencyObject o) { return (VisualTreeHelper.GetParent(o) == null); });
        }

        /// <summary>
        /// Gets the visual children.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static IEnumerable<Visual> GetVisualChildren(Visual parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; ++i)
            {
                yield return (Visual)VisualTreeHelper.GetChild(parent, i);
            }
        }

        /// <summary>
        /// Gets children by type.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildrenByType<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildrenByType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion

        #region Launch Browser

        static int launchBrowserRequests = 0;
        const int MaxBrowserRequests = 3;

        /// <summary>
        /// Launches the browser.
        /// <remarks>Providers accidental click flood.</remarks>
        /// </summary>
        /// <param name="uri">The URI.</param>
        public static void LaunchBrowser(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
            {
                return;
            }

            if (Interlocked.Increment(ref launchBrowserRequests) >= MaxBrowserRequests)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(LaunchBrowserCallback, uri);
        }

        static void LaunchBrowserCallback(object state)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = (state as Uri).AbsoluteUri;

            Process.Start(startInfo);

            Interlocked.Decrement(ref launchBrowserRequests);
        }

        static void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            Uri uri = ((Hyperlink)e.Source).NavigateUri;
            if (uri != null)
            {
                LaunchBrowser(uri);
            }
        }

        #endregion
    }
}
