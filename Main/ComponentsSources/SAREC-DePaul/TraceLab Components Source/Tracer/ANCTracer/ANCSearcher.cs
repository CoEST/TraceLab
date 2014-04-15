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

namespace Tracer
{
    class ANCSearcher
    {
	    SimilarityMetric _similarityMetric;
	
	    // Constructor
        public ANCSearcher(SimilarityMetric similarityMetric)
        {
		    _similarityMetric = similarityMetric;
	    }


        public List<Result> search(String query, TLDictionaryIndex dict, Dictionary<string, double> ancTermsWeights)
        {
		   
            // Variables
		    List<Result> results;
		    Dictionary<string, double> intermediateResults;
		    Dictionary<string, int> queryTermFrequency;
		    Dictionary<string, double> queryTermWeight;
		    double queryVectorLength;
		
		    // Initializes the data structures
		    results = new List<Result>();						// Result array
		    intermediateResults = new Dictionary<string, double>();	// Where the intermediate results of the query are kept.
		    queryTermFrequency = new Dictionary<string, int>();	// Keeps track of term frequencies
		    queryTermWeight = new Dictionary<string, double>();		// Keeps track of term weights
		    queryVectorLength = 0.0;								// The document vector length of the query
		
		    // The query is broken down into tokens
            string[] queryTerms = query.Split(new char[] { ' ' } );
		
		    // Iterates over each query term to compute the term frequency 
		    foreach (string qterm in queryTerms) {

			    // It only cares about those words that are in the dictionary
			    if (dict.ContainsTermEntry(qterm)) {
				    if (!queryTermFrequency.ContainsKey(qterm)) {
					    // First time the query word is encountered
					    queryTermFrequency.Add(qterm, 1);
				    } else {
					    // The query word is already there, so the frequency gets increased
					    queryTermFrequency[qterm] += 1;
				    }
			    }
		    }
		
		    // Iterates over the resulting query terms to compute their weights and the dot product of the query terms x the documents terms
		    foreach (string qterm in queryTermFrequency.Keys) {

			    // Gets the Term from the dictionary
			    TLTermEntry term = dict.GetTermEntry(qterm);
			
			    // It computes the weight of a term -  IE the frequency TIMES the term's specificity.
			    // Note: the specifity of the term is stored in the weight.
			    // 		For the basic dictionary this is just 1
			    //		For the tf-idf dictionary this is the idf
			    // 		For the signal-noise this is the signal
			    
                //double weight = queryTermFrequency[qterm] * term.Weight;
                double ancWeight;
                if(ancTermsWeights.TryGetValue(qterm, out ancWeight) == false) ancWeight = 1.0;
                

                double weight = queryTermFrequency[qterm] * ancWeight;
			    queryTermWeight.Add(qterm, weight);
			
			    // Updates the document vector length of the query
			    queryVectorLength += Math.Pow(weight, 2);

			    // It now iterates over all the postings that have this term
			    foreach (TLPosting posting in term.Postings) {
				    
                    string docId = posting.ArtifactId;
		
				    // Calculates the product of the query times the posting for this particular term
				    double r = queryTermWeight[qterm] * posting.Weight;
				    if (intermediateResults.ContainsKey(docId)) {
					    intermediateResults[docId] += r;
				    } else {
				        intermediateResults.Add(docId, r);
                    }
			    }
		    }

		    // The document vector lenght for the query is the square root of the sum of the squares of the term weights
		    queryVectorLength = Math.Sqrt(queryVectorLength);
		
		
		    // It iterates over the intermediate results to create the final array that is returned to the user
		    foreach (string docId in intermediateResults.Keys) {
			    // Result r = new ResultObj(docId, intermediateResults.get(docId));
			    double similarity = _similarityMetric.ComputeSimilarity(intermediateResults[docId], queryVectorLength, dict.GetDocumentVectorWeight(docId));
			    Result r = new Result(docId, similarity);
			    results.Add(r);
		    }
		
		    // Sorts the results
		    results.Sort();
		    return results;
	    }

    }
}
