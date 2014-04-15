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
	public class TFIDFIndexBuilder : BasicIndexBuilder
	{
		
		/**
		 * input is a map of artifact.Id and processed text of each artifact
		 * 
		 */ 
		public static new TLDictionaryIndex build(TLArtifactsCollection setOfProcessedDocuments, ComponentLogger logger)
		{
			// Variables
			int N = setOfProcessedDocuments.Count; // Total Number of Documents
			double idf;
			String docId; 
			double vectorLength;
			// Stores the vector lenght of each document - this is used to normalize the term weights
			// The vector length is the square root of the sum of the squares of all the term weights.
			Dictionary<string, double> documentVectorLength = new Dictionary<string, double>();

			// It starts off by calling the parent method, which will calculate the basic frequencies
			TLDictionaryIndex dict = BasicIndexBuilder.build(setOfProcessedDocuments, logger);
		
			// Iterates over all the terms
			foreach (TLTermEntry term in dict.TermEntries) {
				
				// Calculates the idf for each term - and stores this in the weight of the term - for weighing queries later
				idf = Math.Log10(N/((double) term.NumberOfArtifactsContainingTerm));
				term.Weight = idf;
			
				// Iterates over all the postings
				foreach (TLPosting posting in term.Postings) {
					
					// Multiplies each term weight by the idf
					double newWeight = posting.Frequency * idf;
					posting.Weight = newWeight;
			
					// Updates the document vector length
					docId = posting.ArtifactId;

					vectorLength = Math.Pow(newWeight, 2);
					if (documentVectorLength.ContainsKey(docId)) {
						// The document has other terms
						vectorLength += documentVectorLength[docId];
					}
					documentVectorLength[docId] = vectorLength;
				}
			}
		
			// Now, we need to get the square root of all entries in the document vector length
			foreach (TLArtifact d in setOfProcessedDocuments.Values) {
				docId = d.Id;
				if(documentVectorLength.ContainsKey(docId)) 
				{ 
				vectorLength = Math.Sqrt(documentVectorLength[docId]);
				documentVectorLength[docId] = vectorLength;
				// Here we update the document vector length of the dictionary - not the internal structure anymore
				dict.SetDocumentVectorWeight(docId,vectorLength);
			}
			}
		
			// Lastly, we normalize all the term weights
			foreach (TLTermEntry term in dict.TermEntries) {
				foreach (TLPosting posting in term.Postings) {
					vectorLength = documentVectorLength[posting.ArtifactId];
					posting.Weight = (posting.Weight / vectorLength);
				}
			}
			
			return dict;
		}
	}
}
