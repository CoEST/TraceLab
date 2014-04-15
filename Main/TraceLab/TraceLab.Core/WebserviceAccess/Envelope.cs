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
    /// The class allows sending any object content to the webservice, that has to be verified by ticket and username.
    /// </summary>
    public class Envelope
    {
        private Envelope() { }

        public Envelope(string ticket, string username, object content)
        {
            Ticket = ticket;
            Username = username;
            Content = content;
        }

        /// <summary>
        /// Gets or sets the ticket. The ticket is required to verify whether the envelope is valid.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the username. The username is required to verify whether envelope is valid.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the content. The content is any object to send to webservice. The object depends on which method is being invoked.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public object Content { get; set; }
    }
}
