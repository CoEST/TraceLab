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

namespace DictionaryIndexBuilder
{
	public class BasicIndexBuilder
	{
		/**
		 * input is a map of artifact.Id and processed text of each artifact
		 * 
		 */
		public static TLDictionaryIndex build(TLArtifactsCollection setOfProcessedDocuments, ComponentLogger logger)
		{
			// Variables
			TLTermEntry termEntry;
			TLPosting posting;
		   
			double vectorLength;
			// Stores the vector lenght of each document - this is used to normalize the term weights
			// The vector length is the square root of the sum of the squares of all the term weights.
			Dictionary<string, double> documentVectorLength = new Dictionary<string, double>();	
		
			// Creates the dictionary
			TLDictionaryIndex dict = new TLDictionaryIndex(); 
		
			// Iterates over all the documents
			foreach (TLArtifact artifact in setOfProcessedDocuments.Values)  
			{

				string[] terms = artifact.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			
				if (terms.Length == 0)
				{
					logger.Warn(String.Format("Artifact Id {0} is empty.", artifact.Id));
				}

				// Iterates over all the terms
				foreach (string t in terms) {

					// Checks if that term has already a posting
					if(!dict.ContainsTermEntry(t)) {
						// New term
						termEntry = dict.AddTermEntry(t, 1, 1, 1.0);
						posting = termEntry.AddPosting(artifact.Id, 1, 1.0);
					} else {
						// Existing term
						termEntry = dict.GetTermEntry(t);
						termEntry.TotalFrequencyAcrossArtifacts += 1;
						termEntry.Weight = 1.0;
					
						// Checks if there is already a posting for this document
						if (!termEntry.ContainsPosting(artifact.Id)) {
							// New posting
							termEntry.NumberOfArtifactsContainingTerm += 1;
							posting = termEntry.AddPosting(artifact.Id, 1, 1.0);
						
						} else {
							// Existing posting
							posting = termEntry.GetPosting(artifact.Id);
							posting.Frequency += 1;
							posting.Weight += 1.0;
						}
					}
				}
			}

			string artId;
			// Now that all the counts are in, it calculates the document vector weights
			foreach (TLTermEntry t in dict.TermEntries) {
				foreach (TLPosting p in t.Postings) {
					artId = p.ArtifactId;
					vectorLength = Math.Pow(p.Frequency, 2);
					if (documentVectorLength.ContainsKey(artId)) {
						// The document has other terms
						vectorLength += documentVectorLength[artId];
					}
					documentVectorLength[artId] = vectorLength;
				}
			}
			
			// Finally, we need to get the square root of all entries in the document vector length
			foreach (TLArtifact artifact in setOfProcessedDocuments.Values)  
			{
				if (documentVectorLength.ContainsKey(artifact.Id))
				{
					vectorLength = Math.Sqrt(documentVectorLength[artifact.Id]);
					// Here we update the document vector length of the dictionary - not the internal structure any more
					dict.SetDocumentVectorWeight(artifact.Id, vectorLength);
				}		
			}		
		
			return dict;
		}
	}
}
