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
    /// Encapsulates Source Artifacts, Target Artifacts, and corresponding answer set from one to the other.
    /// </summary>
    [Serializable]
    [WorkspaceType]
    public class TLDataset : IRawSerializable
    {
        #region Members

        private static int version = 1;

        private string m_name = String.Empty;
        /// <summary>
        /// Gets or sets the name of dataset.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// Gets or sets the source artifacts.
        /// </summary>
        /// <value>
        /// The source artifacts.
        /// </value>
        public TLArtifactsCollection SourceArtifacts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target artifacts.
        /// </summary>
        /// <value>
        /// The target artifacts.
        /// </value>
        public TLArtifactsCollection TargetArtifacts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the answer set.
        /// </summary>
        /// <value>
        /// The answer set.
        /// </value>
        public TLSimilarityMatrix AnswerSet
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public TLDataset() { }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public TLDataset(string name)
        {
            this.m_name = name;
        }

        #endregion

        #region IRawSerializable Members

        /// <summary>
        /// Reads the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadData(System.IO.BinaryReader reader)
        {
            int dataversion = reader.ReadInt32();
            if (dataversion != TLDataset.version)
            {
                throw new InvalidOperationException("Binary reader did not read correct data version. Data corrupted. Potentially IRawSerializable not implemented correctly");
            }
            else
            {
                this.m_name = reader.ReadString();

                bool isMemberPresent = reader.ReadBoolean();
                if (isMemberPresent)
                {
                    TLArtifactsCollection artifacts = new TLArtifactsCollection();
                    artifacts.ReadData(reader);
                    this.SourceArtifacts = artifacts;
                }

                isMemberPresent = reader.ReadBoolean();
                if (isMemberPresent)
                {
                    TLArtifactsCollection artifacts = new TLArtifactsCollection();
                    artifacts.ReadData(reader);
                    this.TargetArtifacts = artifacts;
                }

                isMemberPresent = reader.ReadBoolean();
                if (isMemberPresent)
                {
                    TLSimilarityMatrix matrix = new TLSimilarityMatrix();
                    matrix.ReadData(reader);
                    this.AnswerSet = matrix;
                }
            }
        }

        /// <summary>
        /// Writes the data. (allows faster custom serialization for better performance in TraceLab)
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteData(System.IO.BinaryWriter writer)
        {
            writer.Write(TLDataset.version);

            writer.Write(this.m_name);

            if (this.SourceArtifacts != null)
            {
                writer.Write(true);
                this.SourceArtifacts.WriteData(writer);
            }
            else
            {
                writer.Write(false);
            }

            if (this.TargetArtifacts != null)
            {
                writer.Write(true);
                this.TargetArtifacts.WriteData(writer);
            }
            else
            {
                writer.Write(false);
            }

            if (this.AnswerSet != null)
            {
                writer.Write(true);
                this.AnswerSet.WriteData(writer);
            }
            else
            {
                writer.Write(false);
            }
        }

        #endregion
    }
}
