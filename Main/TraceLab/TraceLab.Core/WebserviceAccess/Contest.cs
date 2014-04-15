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

namespace TraceLab.Core.WebserviceAccess
{
    public class Contest
    {
        protected Contest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contest"/> class.
        /// </summary>
        /// <param name="contestGUID">The contest GUID.</param>
        /// <param name="name">The name.</param>
        /// <param name="author">The author.</param>
        /// <param name="contributors">The contributors.</param>
        /// <param name="shortDescription">The short description.</param>
        /// <param name="description">The description.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="metrics">The metrics.</param>
        /// <param name="datasets">The datasets. The list of datasets names</param>
        protected Contest(string contestGUID, string name, string author, string contributors,
                       string shortDescription, string description, DateTime deadline, List<MetricDefinition> metrics, List<string> datasets)
        {
            if (String.IsNullOrEmpty(contestGUID))
                throw new ArgumentNullException("contestGUID");

            ContestGUID = contestGUID;
            Name = name;
            Author = author;
            Contributors = contributors;
            ShortDescription = shortDescription;
            Description = description;
            Deadline = deadline.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.UniversalSortableDateTimePattern);

            Metrics = metrics;
            Datasets = datasets;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contest"/> class.
        /// The packageFile content is converted to base64 string. The contructor attempts to read all bytes from that file, 
        /// thus there are several related exception being thrown by this constructor.
        /// </summary>
        /// <param name="contestGUID">The contest GUID.</param>
        /// <param name="name">The name.</param>
        /// <param name="author">The author.</param>
        /// <param name="contributors">The contributors.</param>
        /// <param name="shortDescription">The short description.</param>
        /// <param name="description">The description.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="packageFilePath">The package file path.</param>
        /// <exception cref="System.IO.IOException">An I/O error occurred while opening the package file.</exception>
        /// <exception cref="System.UnauthorizedAccessExceptionn">packageFilePath specified a directory -or- The caller does not have the required permission.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission to the package file.</exception>
        /// <exception cref="System.ArgumentException">packageFilePath is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        /// -or- contestGUID is null or empty
        /// </exception>
        public Contest(string contestGUID, string name, string author, string contributors,
                       string shortDescription, string description, DateTime deadline, List<MetricDefinition> metrics, List<string> datasets, string packageFilePath) 
            : this(contestGUID, name, author, contributors, shortDescription, description, deadline, metrics, datasets)
        {
            if (String.IsNullOrEmpty(packageFilePath))
                throw new ArgumentNullException("packageFilePath");

            //Read all bytes of the package file and convert it to base64 string
            //throws several exceptions IOException, UnauthorizedAccessException, FileNotFoundException, SecurityException
            byte[] packageBytes = System.IO.File.ReadAllBytes(packageFilePath);
            PackageFileContent = Convert.ToBase64String(packageBytes);
            PackageFileName = System.IO.Path.GetFileName(packageFilePath);
            PackageFileSize = packageBytes.Length;
            PackageFileType = "application/xml";

        }

        public Contest(string contestGUID, string name, string author, string contributors,
                       string shortDescription, string description, DateTime deadline, List<MetricDefinition> metrics, List<string> datasets, byte[] packageBytes, string packageFileName, string packageFileType)
            : this(contestGUID, name, author, contributors, shortDescription, description, deadline, metrics, datasets)
        {
            PackageFileContent = Convert.ToBase64String(packageBytes);
            PackageFileName = packageFileName;
            PackageFileSize = packageBytes.Length;
            PackageFileType = packageFileType;
        }

        public string ContestIndex { get; set; }
        public string ContestGUID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Contributors { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public string PackageFileContent { get; set; }
        public string PackageFileName { get; set; }
        public int PackageFileSize { get; set; }
        public string PackageFileType { get; set; }

        /// <summary>
        /// Gets or sets the metrics.
        /// </summary>
        /// <value>
        /// The metrics.
        /// </value>
        private List<MetricDefinition> m_metrics;
        public List<MetricDefinition> Metrics {
            get { return m_metrics; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Metrics definition cannot be null");
                }
                m_metrics = value;
            }
        }


        /// <summary>
        /// Gets or sets the list of datasets names
        /// </summary>
        /// <value>
        /// The datasets.
        /// </value>
        public List<string> Datasets { get; set; }


        internal void SetDeadline(DateTime deadline)
        {
            Deadline = deadline.ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.UniversalSortableDateTimePattern);
        }

        private ContestResults m_baselineResults;

        public ContestResults BaselineResults
        {
            get { return m_baselineResults; }
            set { m_baselineResults = value; }
        }

    }
}
