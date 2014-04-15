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
using TraceLabSDK;
using TraceLabSDK.Types;
using System.IO;
using TraceLabSDK.Component.Config;

namespace Importer
{
    [Component(GuidIDString = "E95DBB36-1DB8-11E0-8F08-2EE7DFD72085", //GuidIDString is retained for backwards experiment that may refer to component by old guid
       Name = "AnswerMatrixImporter",
       DefaultLabel = "Answer Matrix Txt Importer",
       Author = "DePaul RE Lab Team",
       Version = "1.0",
       Description = "Imports answer matrix from text file. Each trace link should be in new line with artifacts ids separated by seperator. " +
                     "For example for comma separator ',' link would be: AC,CCHIT0212. You may define multiple separators one after another, " + 
                     "for example ',.' will use both comma and dot as split delimiter. \n" +
                     "Sample file can be found in Package 'Importers Sample Files', file 'Answer Matrix Importer Format.txt'", 
       ConfigurationType = typeof(AnswerMatrixImporterConfig))]
    [IOSpec(IOSpecType.Output, "answerMatrix", typeof(TraceLabSDK.Types.TLSimilarityMatrix))]
    [Tag("Importers.TLSimilarityMatrix.From TXT")]
    public class AnswerMatrixImporter : BaseComponent
    {
        public AnswerMatrixImporter(ComponentLogger log) : base(log)
        {
            config = new AnswerMatrixImporterConfig();
            this.Configuration = config;
        }

        private AnswerMatrixImporterConfig config;

        public override void Compute()
        {
            if (config.Separator == null)
            {
                throw new ComponentException("Seperator has not been specified");
            }

            TLSimilarityMatrix answerMatrix = ReadAnswerMatrix(config.Path, config.Separator.ToCharArray());

            Workspace.Store("answerMatrix", answerMatrix);

            Logger.Info("Matrix has been imported from " + config.Path);
        }


        /// <summary>
        /// Method reads answer matrix from text file. Each line represents one link from source artifact id to target artifact id, seperated by separator.
        /// Such link is added to the sparse matrix with score 1.0, as fully relevant link.
        /// </summary>
        /// <param name="answersFile">text file</param>
        /// <param name="separator">separator that seperates source artifact id to target artifact id in each line of the file</param>
        /// <returns></returns>
        public static TLSimilarityMatrix ReadAnswerMatrix(String answersFile, char[] separators)
        {
            if (answersFile == null)
            {
                throw new ComponentException("Path has not been specified.");
            }
            if (!File.Exists(answersFile))
            {
                throw new ComponentException("File does not exist.");
            }

            //this makes matrix with zeros
            TLSimilarityMatrix answerMatrix = new TLSimilarityMatrix();

            TextReader tr = new StreamReader(answersFile);

            String line;
            while ((line = tr.ReadLine()) != null)
            {

                String concatanateID = line.Trim();
                String[] ids = line.Split(separators);  //check if the format is correct

                if (ids.Length != 2)
                {
                    throw new ArgumentException(String.Format("Couldn't read ids. Each line in file should have two id values seperate by any of given seperators"));

                }
                else
                {
                    //update value to 1.0 in matrix for source and target relation
                    answerMatrix.AddLink(ids[0], ids[1], 1.0);
                }

            }

            return answerMatrix;
        }   
    }


    public class AnswerMatrixImporterConfig
    {
        private FilePath m_path;
        public FilePath Path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        private string m_separator;
        public string Separator
        {
            get
            {
                return m_separator;
            }
            set
            {    
                m_separator = value;
            }
        }

    }
}
