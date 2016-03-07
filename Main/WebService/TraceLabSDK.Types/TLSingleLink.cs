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
using System.Xml.Serialization;
using TraceLabSDK.Types.Generics.Collections;

namespace TraceLabSDK.Types
{
    /// <summary>
    /// Represents the single link between two artifacts specified by source artifact id and target artifact id, as well as score.
    /// </summary>
    [Serializable]
    public class TLSingleLink : IComparable<TLSingleLink>, IRawSerializable
    {
        #region Members

        private string m_sourceArtifactId;
        /// <summary>
        /// Gets or sets source artifact id for the link
        /// </summary>
        public string SourceArtifactId
        {
            get { return this.m_sourceArtifactId; }
            set { this.m_sourceArtifactId = value; }
        }

        private string m_targetArtifactId;
        /// <summary>
        /// Gets or sets target artifact id for the link
        /// </summary>
        public string TargetArtifactId
        {
            get { return this.m_targetArtifactId; }
            set { this.m_targetArtifactId = value; }
        }

        private double m_score;
        /// <summary>
        /// Gets or sets score for the link.
        /// </summary>
        public double Score
        {
            get { return this.m_score; }
            set { this.m_score = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructs single link for the specified source and target artifacts with specified confidence score
        /// </summary>
        /// <param name="sourceArtifactId">source artifact id for the link</param>
        /// <param name="targetArtifactId">target artifact id for the link</param>
        /// <param name="score">confidence score</param>
        public TLSingleLink(string sourceArtifactId, string targetArtifactId, double score)
        {
            this.m_sourceArtifactId = sourceArtifactId;
            this.m_targetArtifactId = targetArtifactId;
            this.m_score = score;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TLSingleLink"/> class.
        /// </summary>
        internal TLSingleLink()
        { }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: 
        /// Less than zero - This instance precedes obj in the sort order.
        /// Zero - This instance occurs in the same position in the sort order as obj.
        /// Greater than zero - This instance follows obj in the sort order.
        /// </returns>
        public int CompareTo(TLSingleLink other)
        {
            return -Score.CompareTo(other.Score);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the Object.</returns>
        public override int GetHashCode()
        {
            int sourceArtifacyhash = SourceArtifactId.GetHashCode();
            int targetArtifactHash = TargetArtifactId.GetHashCode();
            int probabilityHash = Score.GetHashCode();

            int hash = sourceArtifacyhash ^ targetArtifactHash ^ probabilityHash;

            return hash;
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj">the other object</param>
        /// <returns>true if objects are equal</returns>
        public override bool Equals(object obj)
        {
            TLSingleLink other = obj as TLSingleLink;

            if (other != null)
            {
                return ((SourceArtifactId.Equals(other.SourceArtifactId) &&
                    TargetArtifactId.Equals(other.TargetArtifactId)) && Score.Equals(other.Score));
            }

            return false;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return SourceArtifactId + " " + TargetArtifactId + " " + Score.ToString();
        }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            this.m_sourceArtifactId = reader.ReadString();
            this.m_targetArtifactId = reader.ReadString();
            this.m_score = reader.ReadDouble();
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(this.m_sourceArtifactId);
            writer.Write(this.m_targetArtifactId);
            writer.Write(this.m_score);
        }

        #endregion
    }
}
