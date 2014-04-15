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

namespace TraceLabSDK.Types.Contests
{
    /// <summary>
    /// Represents the point in LineSeries
    /// </summary>
    [Serializable]
    public class Point : IRawSerializable
    {
        #region Members

        private double m_x;
        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>
        /// The X coordinate.
        /// </value>
        public double X
        {
            get { return m_x; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The line series point cannot be set to Nan (Not a number) double values. ");
                m_x = value;
            }
        }

        private double m_y;
        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>
        /// The Y coordinate.
        /// </value>
        public double Y
        {
            get { return m_y; }
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("The line series point cannot accept Nan (Not a number) double values. ");

                m_y = value;
            }
        }

        #endregion Members

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        internal Point()
        {
            this.m_x = 0.0d;
            this.m_y = 0.0d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Point p = obj as Point;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (m_x == p.m_x) && (m_y == p.m_y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #endregion Methods

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            this.m_x = reader.ReadDouble();
            this.m_y = reader.ReadDouble();
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(this.m_x);
            writer.Write(this.m_y);
        }

        #endregion IRawSerializable Members
    }
}
