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
using TraceLab.Core.WebserviceAccess.Metrics;
using System.Xml.Serialization;
using System.IO;

namespace TraceLab.Core.WebserviceAccess
{
    public class ContestResults
    {
        private ContestResults() { }

        public ContestResults(string contestGuid, string techniqueName, string techniqueDescription, List<DatasetResultsDTO> results, double score, IXmlSerializable baseData)
        {
            ContestGUID = contestGuid;
            TechniqueName = techniqueName;
            TechniqueDescription = techniqueDescription;
            Results = results;
            Score = score;

            //
            var serializer = TraceLab.Core.Serialization.XmlSerializerFactory.GetSerializer(baseData.GetType(), null);
            using (MemoryStream memorystream = new MemoryStream())
            {
                serializer.Serialize(memorystream, baseData);
                string xmlObject = Encoding.Default.GetString(memorystream.ToArray());
                BaseData = xmlObject;
            }
        }

        public string ContestGUID { get; set; }
        public string TechniqueName { get; set; }
        public string TechniqueDescription { get; set; }
        public List<DatasetResultsDTO> Results { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// The score represents position in the leader board ranking.
        /// The score can be computed by any method in the component defined by the user.
        /// It has to be independent from other results, and cannot change over time. It is a const score
        /// computed for the technique. This value is going to be uploaded to the server when publishing results
        /// and will determine the position in the ranking.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public double Score { get; set; }


        /// <summary>
        /// Gets or sets the base data. The base data is the object from which all metrics has been calculated.
        /// For example, if metrix were calculated from Similarity Matrix, then the similarity matrix is a base data.
        /// It does not have to include answer matrix... only similarity matrix results.
        /// The string represents the xml serialized object.
        /// </summary>
        /// <value>
        /// The base data.
        /// </value>
        public string BaseData { get; set; }
    }
}
