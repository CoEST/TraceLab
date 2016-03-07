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

namespace TraceLab.Core.WebserviceAccess
{
    /// <summary>
    /// The delegate defining the event handler events for the callback event on completed. Client caller method would need to implement 
    /// the delegate and attach to the CallCompleted event on the callback object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="responseArgs">The <see cref="TraceLab.Core.WebserviceAccess.CallCompletedEventArgs&lt;T&gt;"/> instance containing the event data. Includes the response from the webservice</param>
    public delegate void CallCompletedEventHandler<T>(object sender, CallCompletedEventArgs<T> responseArgs) where T : Response, new();
}
