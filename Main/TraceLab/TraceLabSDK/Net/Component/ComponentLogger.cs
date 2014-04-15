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
using System.ComponentModel;
using System.Security.Permissions;

namespace TraceLabSDK
{
    /// <summary>
    /// Allows component to log different messages to the output window
    /// </summary>
    public interface ComponentLogger
    {
        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        ///   </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        object InitializeLifetimeService();

        #region Info
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Info(object value);

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Info(string message);

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Info<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Info(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Info<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Info(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Info(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Info(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Info(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Info(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Info level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void InfoException(string message, Exception exception);

        #endregion Info

        #region Trace
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Trace(object value);

        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Trace(string message);

        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Trace<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Trace(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Trace<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Trace(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Trace(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Trace(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Trace(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Trace(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Trace level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void TraceException(string message, Exception exception);

        #endregion Trace

        #region Debug
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Debug(object value);

        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Debug(string message);

        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Debug<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Debug(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Debug<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Debug(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Debug(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Debug(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Debug(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Debug level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void DebugException(string message, Exception exception);

        #endregion Debug

        #region Warn
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Warn(object value);

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Warn(string message);

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Warn<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Warn(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Warn<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Warn(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Warn(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Warn(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Warn(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Warn(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Warn level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void WarnException(string message, Exception exception);

        #endregion Warn

        #region Error
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Error(object value);

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Error(string message);

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Error<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Error(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Error<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Error(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Error(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Error(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Error(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Error(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Error level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void ErrorException(string message, Exception exception);

        #endregion Error

        #region Fatal
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        void Fatal(object value);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="message">Log message.</param>
        void Fatal(string message);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        void Fatal<T>(T value);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        void Fatal(IFormatProvider formatProvider, object value);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        void Fatal<T>(IFormatProvider formatProvider, T value);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Fatal(string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal<TArgument>(string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, bool argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, byte argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, char argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, decimal argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, double argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, float argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, int argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, long argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, object argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        void Fatal(IFormatProvider formatProvider, string message, params object[] args);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>

        void Fatal(IFormatProvider formatProvider, string message, string argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        void Fatal(string message, object arg1, object arg2);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        void Fatal(string message, object arg1, object arg2, object arg3);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        /// <summary>
        /// Writes the diagnostic message and exception at the Fatal level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        void FatalException(string message, Exception exception);

        #endregion Fatal
    }

    /// <summary>
    /// ComponentLoggerImplementation is used to write the logging info, debug, error, warning messages from a component - each component will be given one ComponentLogger.
    /// This implementation is using NLog.LogManager.
    /// </summary>
    public class ComponentLoggerImplementation : MarshalByRefObject, TraceLabSDK.ComponentLogger
    {
        private NLog.Logger m_logger;

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">
        /// The immediate caller does not have infrastructure permission.
        ///   </exception>
        ///   
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        ///   </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public ComponentLoggerImplementation(string name)
        {
            m_logger = NLog.LogManager.GetLogger(name);
            m_name = name;
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
        }

        #region Info
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(object value)
        {
            m_logger.Info(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Info(string message)
        {
            m_logger.Info(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Info<T>(T value)
        {
            m_logger.Info(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, object value)
        {
            m_logger.Info(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Info(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, bool argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, byte argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, char argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, decimal argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, double argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, float argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, int argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, long argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, object argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Info(string message, params object[] args)
        {
            m_logger.Info(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, string argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Info<TArgument>(string message, TArgument argument)
        {
            m_logger.Info(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Info(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Info(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, object arg1, object arg2)
        {
            m_logger.Info(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Info(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Info(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Info(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Info(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Info(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Info level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Info(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Info level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void InfoException(string message, Exception exception)
        {
            m_logger.InfoException(message, exception);
        }
        #endregion

        #region Trace
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(object value)
        {
            m_logger.Trace(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Trace(string message)
        {
            m_logger.Trace(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Trace<T>(T value)
        {
            m_logger.Trace(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, object value)
        {
            m_logger.Trace(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Trace<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Trace(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, bool argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, byte argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, char argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, decimal argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, double argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, float argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, int argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, long argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, object argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Trace(string message, params object[] args)
        {
            m_logger.Trace(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, string argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Trace<TArgument>(string message, TArgument argument)
        {
            m_logger.Trace(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Trace(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Trace(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Trace(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, object arg1, object arg2)
        {
            m_logger.Trace(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Trace(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Trace(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Trace(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Trace(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Trace(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Trace level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Trace(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Trace level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void TraceException(string message, Exception exception)
        {
            m_logger.TraceException(message, exception);
        }
        #endregion

        #region Debug
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(object value)
        {
            m_logger.Debug(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Debug(string message)
        {
            m_logger.Debug(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Debug<T>(T value)
        {
            m_logger.Debug(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, object value)
        {
            m_logger.Debug(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Debug(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, bool argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, byte argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, char argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, decimal argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, double argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, float argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, int argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, long argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, object argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Debug(string message, params object[] args)
        {
            m_logger.Debug(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, string argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Debug<TArgument>(string message, TArgument argument)
        {
            m_logger.Debug(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Debug(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Debug(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, object arg1, object arg2)
        {
            m_logger.Debug(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Debug(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Debug(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Debug(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Debug(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Debug(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Debug level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Debug(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Debug level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void DebugException(string message, Exception exception)
        {
            m_logger.DebugException(message, exception);
        }
        #endregion

        #region Warning
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(object value)
        {
            m_logger.Warn(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Warn(string message)
        {
            m_logger.Warn(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Warn<T>(T value)
        {
            m_logger.Warn(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, object value)
        {
            m_logger.Warn(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Warn(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, bool argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, byte argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, char argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, decimal argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, double argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, float argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, int argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, long argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, object argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Warn(string message, params object[] args)
        {
            m_logger.Warn(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, string argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Warn<TArgument>(string message, TArgument argument)
        {
            m_logger.Warn(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Warn(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Warn(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, object arg1, object arg2)
        {
            m_logger.Warn(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Warn(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Warn(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Warn(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Warn(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Warn(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Warn level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Warn(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Warn level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void WarnException(string message, Exception exception)
        {
            m_logger.WarnException(message, exception);
        }
        #endregion

        #region Error
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(object value)
        {
            m_logger.Error(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Error(string message)
        {
            m_logger.Error(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Error<T>(T value)
        {
            m_logger.Error(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, object value)
        {
            m_logger.Error(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Error(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, bool argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, byte argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, char argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, decimal argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, double argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, float argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, int argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, long argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, object argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Error(string message, params object[] args)
        {
            m_logger.Error(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, string argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Error<TArgument>(string message, TArgument argument)
        {
            m_logger.Error(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Error(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Error(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, object arg1, object arg2)
        {
            m_logger.Error(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Error(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Error(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Error(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Error(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Error(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Error level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Error(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Error level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void ErrorException(string message, Exception exception)
        {
            m_logger.ErrorException(message, exception);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(object value)
        {
            m_logger.Fatal(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Fatal(string message)
        {
            m_logger.Fatal(message);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value to be written.</param>
        public void Fatal<T>(T value)
        {
            m_logger.Fatal(value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The object to be written.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, object value)
        {
            m_logger.Fatal(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to be written.</param>
        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            m_logger.Fatal(formatProvider, value);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, bool argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, byte argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, char argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, decimal argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, double argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, float argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, int argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, long argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, object argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Fatal(string message, params object[] args)
        {
            m_logger.Fatal(message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter.
        /// </summary>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, string argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameter
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Fatal<TArgument>(string message, TArgument argument)
        {
            m_logger.Fatal(message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, bool argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, byte argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, char argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, decimal argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, double argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, float argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, int argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, long argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, object argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// and formatting them with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing format items.</param>
        /// <param name="args">The arguments to format.</param>
        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            m_logger.Fatal(formatProvider, message, args);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified value
        /// as a parameter and formatting it with the supplied format provider.
        /// </summary>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(IFormatProvider formatProvider, string message, string argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameter
        /// and formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument">The argument to format.</param>
        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            m_logger.Fatal(formatProvider, message, argument);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, object arg1, object arg2)
        {
            m_logger.Fatal(message, arg1, arg2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Fatal(message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            m_logger.Fatal(formatProvider, message, argument1, argument2);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters.
        /// </summary>
        /// <param name="message">A string containing format items.</param>
        /// <param name="arg1">The first argument to format.</param>
        /// <param name="arg2">The second argument to format.</param>
        /// <param name="arg3">The third argument to format.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Fatal(string message, object arg1, object arg2, object arg3)
        {
            m_logger.Fatal(message, arg1, arg2, arg3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified parameters.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Fatal(message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message at the Fatal level using the specified arguments
        /// formatting it with the supplied format provider.
        /// </summary>
        /// <typeparam name="TArgument1">The type of the first argument.</typeparam>
        /// <typeparam name="TArgument2">The type of the second argument.</typeparam>
        /// <typeparam name="TArgument3">The type of the third argument.</typeparam>
        /// <param name="formatProvider">An IFormatProvider that supplies culture-specific formatting information.</param>
        /// <param name="message">A string containing one format item.</param>
        /// <param name="argument1">The first argument to format.</param>
        /// <param name="argument2">The second argument to format.</param>
        /// <param name="argument3">The third argument to format.</param>
        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            m_logger.Fatal(formatProvider, message, argument1, argument2, argument3);
        }
        /// <summary>
        /// Writes the diagnostic message and exception at the Fatal level.
        /// </summary>
        /// <param name="message">A string to be written.</param>
        /// <param name="exception">An exception to be logged.</param>
        public void FatalException(string message, Exception exception)
        {
            m_logger.FatalException(message, exception);
        }
        #endregion
    }
}
