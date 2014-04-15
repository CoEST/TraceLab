using System;
using System.Collections.Generic;
using System.IO;
using TraceLabSDK.Types;

/// SEMERU Component Library - TraceLab Component Plugin
/// Copyright © 2012-2013 SEMERU
/// 
/// This file is part of the SEMERU Component Library.
/// 
/// The SEMERU Component Library is free software: you can redistribute it and/or
/// modify it under the terms of the GNU General Public License as published by the
/// Free Software Foundation, either version 3 of the License, or (at your option)
/// any later version.
/// 
/// The SEMERU Component Library is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
/// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for
/// more details.
/// 
/// You should have received a copy of the GNU General Public License along with the
/// SEMERU Component Library.  If not, see <http://www.gnu.org/licenses/>.

namespace SEMERU.Core.Models
{
    public class TermDocumentMatrix
    {
        #region Private members
        private double[][] _matrix;
        private List<string> _termIndex;
        private List<string> _docIndex;
        private Dictionary<string, int> _termIndexLookup;
        private Dictionary<string, int> _docIndexLookup;
        #endregion

        #region Public accessors

        /// <summary>
        /// Allows direct access to the value at docindex, termindex
        /// </summary>
        /// <param name="docindex">Document index</param>
        /// <param name="termindex">Term index</param>
        /// <returns>Term value in document</returns>
        public double this[int docindex, int termindex]
        {
            get
            {
                return _matrix[docindex][termindex];
            }
            set
            {
                _matrix[docindex][termindex] = value;
            }
        }

        /// <summary>
        /// Raw term-by-document matrix
        /// Rows: documents
        /// Columns: terms
        /// </summary>
        public double[][] RawMatrix
        {
            get
            {
                return _matrix;
            }
        }

        /// <summary>
        /// Term index listing
        /// </summary>
        public List<string> TermMap
        {
            get
            {
                return _termIndex;
            }
        }

        /// <summary>
        /// Document index listing
        /// </summary>
        public List<string> DocMap
        {
            get
            {
                return _docIndex;
            }
        }

        /// <summary>
        /// Number of documents
        /// </summary>
        public int NumDocs
        {
            get
            {
                return _docIndex.Count;
            }
        }

        /// <summary>
        /// Number of terms
        /// </summary>
        public int NumTerms
        {
            get
            {
                return _termIndex.Count;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a term-by-document matrix from a TLArtifactsCollection
        /// </summary>
        /// <param name="artifacts">Artifacts collection</param>
        public TermDocumentMatrix(TLArtifactsCollection artifacts)
        {
            _termIndex = new List<string>();
            _docIndex = new List<string>();
            _termIndexLookup = new Dictionary<string, int>();
            _docIndexLookup = new Dictionary<string, int>();

            // create temporary corpus to build matrix with
            Dictionary<string, Dictionary<string, double>> corpus = new Dictionary<string, Dictionary<string, double>>();
            foreach (TLArtifact artifact in artifacts.Values)
            {
                // update document maps
                _docIndex.Add(artifact.Id);
                _docIndexLookup.Add(artifact.Id, _docIndex.Count - 1);
                corpus.Add(artifact.Id, new Dictionary<string, double>());
                foreach (string term in artifact.Text.Split())
                {
                    if (!String.IsNullOrWhiteSpace(term))
                    {
                        // update term maps
                        if (!_termIndexLookup.ContainsKey(term))
                        {
                            _termIndex.Add(term);
                            _termIndexLookup.Add(term, _termIndex.Count - 1);
                        }
                        // update document counts
                        if (corpus[artifact.Id].ContainsKey(term))
                        {
                            corpus[artifact.Id][term]++;
                        }
                        else
                        {
                            corpus[artifact.Id].Add(term, 1);
                        }
                    }
                }
            }

            // build term-by-document matrix
            _matrix = new double[_docIndex.Count][];
            for (int i = 0; i < _docIndex.Count; i++)
            {
                _matrix[i] = new double[_termIndex.Count];
                for (int j = 0; j < _termIndex.Count; j++)
                {
                    corpus[_docIndex[i]].TryGetValue(_termIndex[j], out _matrix[i][j]);
                }
            }
        }

        /// <summary>
        /// Build your own matrix from scratch
        /// </summary>
        /// <param name="docs">Number of documents</param>
        /// <param name="terms">Number of terms</param>
        public TermDocumentMatrix(int docs, int terms)
        {
            _matrix = new double[docs][];
            _docIndex = new List<string>();
            _docIndexLookup = new Dictionary<string, int>();
            for (int i = 0; i < docs; i++)
            {
                _matrix[i] = new double[terms];
                _docIndex.Add("d" + i);
                _docIndexLookup.Add("d" + i, _docIndex.Count);
            }
            _termIndex = new List<string>();
            _termIndexLookup = new Dictionary<string, int>();
            for (int i = 0; i < terms; i++)
            {
                _termIndex.Add("t" + i);
                _termIndexLookup.Add("t" + i, _termIndex.Count);
            }
        }

        /// <summary>
        /// Private constructor for internal use
        /// </summary>
        private TermDocumentMatrix() { }

        /// <summary>
        /// Deep copy constructor
        /// </summary>
        /// <param name="matrix">Object to be copied</param>
        public TermDocumentMatrix(TermDocumentMatrix matrix)
        {
            _matrix = new double[matrix.NumDocs][];
            for (int i = 0; i < matrix.NumDocs; i++)
            {
                _matrix[i] = new double[matrix.NumTerms];
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    _matrix[i][j] = matrix[i,j];
                }
            }
            _docIndex = new List<string>(matrix._docIndex);
            _docIndexLookup = new Dictionary<string, int>(matrix._docIndexLookup);
            _termIndex = new List<string>(matrix._termIndex);
            _termIndexLookup = new Dictionary<string, int>(matrix._termIndexLookup);
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets a document array based on its index in the matrix
        /// </summary>
        /// <param name="index">Document index</param>
        /// <returns>Document array</returns>
        public double[] GetDocument(int index)
        {
            return _matrix[index];
        }

        /// <summary>
        /// Gets a document array based on its ID
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <returns>Document array</returns>
        public double[] GetDocument(string artifactID)
        {
            return GetDocument(_docIndexLookup[artifactID]);
        }

        /// <summary>
        /// Gets a value from the matrix based on the document and term indexes
        /// </summary>
        /// <param name="doc">Document index</param>
        /// <param name="term">Term index</param>
        /// <returns>Term value in document</returns>
        public double GetValue(int doc, int term)
        {
            return _matrix[doc][term];
        }

        /// <summary>
        /// Gets a value from the matrix based on the artifact's ID and the term
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <param name="term">Term</param>
        /// <returns>Term value in document</returns>
        public double GetValue(string artifactID, string term)
        {
            return GetValue(_docIndexLookup[artifactID], _termIndexLookup[term]);
        }

        /// <summary>
        /// Finds the index of the term in the matrix
        /// </summary>
        /// <param name="term">Term to find</param>
        /// <returns>Term index</returns>
        public int GetTermIndex(string term)
        {
            return _termIndexLookup[term];
        }

        /// <summary>
        /// Returns the term at index
        /// </summary>
        /// <param name="index">term index</param>
        /// <returns>term</returns>
        public string GetTermName(int index)
        {
            return _termIndex[index];
        }

        /// <summary>
        /// Finds the index of the document in the matrix
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <returns>Document index</returns>
        public int GetDocumentIndex(string artifactID)
        {
            return _docIndexLookup[artifactID];
        }

        /// <summary>
        /// Returns the name of the artifact at index
        /// </summary>
        /// <param name="index">artifact index</param>
        /// <returns>artifat name</returns>
        public string GetDocumentName(int index)
        {
            return _docIndex[index];
        }

        #endregion

        #region Setters

        /// <summary>
        /// Sets a new document array for the document at index
        /// </summary>
        /// <param name="index">Document index</param>
        /// <param name="doc">New document array</param>
        public void SetDocument(int index, double[] doc)
        {
            if (doc.Length != _matrix[index].Length)
                throw new ArgumentException("The array sizes do not match.");
            _matrix[index] = doc;
        }

        /// <summary>
        /// Sets a new document array for the document with the given ID
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <param name="doc">New document array</param>
        public void SetDocument(string artifactID, double[] doc)
        {
            SetDocument(_docIndexLookup[artifactID], doc);
        }

        /// <summary>
        /// Sets a value in the matrix based on the document and term indexes
        /// </summary>
        /// <param name="doc">Document index</param>
        /// <param name="term">Term index</param>
        /// <param name="value">Value to set</param>
        public void SetValue(int doc, int term, double value)
        {
            _matrix[doc][term] = value;
        }

        /// <summary>
        /// Sets a value in the matrix based on the term and artifact's ID
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <param name="term">Term</param>
        /// <param name="value">Value to set</param>
        public void SetValue(string artifactID, string term, double value)
        {
            SetValue(_docIndexLookup[artifactID], _termIndexLookup[term], value);
        }

        /// <summary>
        /// Sets a new matrix with the same dimensions
        /// </summary>
        /// <param name="matrix">New matrix</param>
        public void SetMatrix(double[][] matrix)
        {
            if (matrix.GetLength(0) != _matrix.GetLength(0))
                throw new ArgumentException("The matrix has the wrong number of rows.");
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                if (matrix[i].Length != _matrix[i].Length)
                    throw new ArgumentException("The matrix has the wrong number of columns in row " + i + ".");
            }
            _matrix = matrix;
        }

        #endregion

        #region Queries

        /// <summary>
        /// Queries the matrix for the existence of document
        /// </summary>
        /// <param name="artifactID">Artifact ID</param>
        /// <returns>True if document is in matrix, false otherwise</returns>
        public bool ContainsDocument(string artifactID)
        {
            return _docIndexLookup.ContainsKey(artifactID);
        }

        /// <summary>
        /// Queries the matrix for the existence of term
        /// </summary>
        /// <param name="term">Term</param>
        /// <returns>True if term is in matrix, false otherwise</returns>
        public bool ContainsTerm(string term)
        {
            return _termIndexLookup.ContainsKey(term);
        }

        #endregion

        #region Static utilities

        /// <summary>
        /// Recreates each matrix with documents containing missing terms.
        /// List[0] : matrix 1
        /// List[1] : matrix 2
        /// </summary>
        /// <param name="matrix1">First term-by-document matrix</param>
        /// <param name="matrix2">Second term-by-document matrix</param>
        /// <returns>Copies of original matrices with missing terms from each</returns>
        public static List<TermDocumentMatrix> Equalize(TermDocumentMatrix matrix1, TermDocumentMatrix matrix2)
        {
            // initialize matrices
            List<TermDocumentMatrix> matrices = new List<TermDocumentMatrix>();
            // matrix 1
            matrices.Add(new TermDocumentMatrix());
            matrices[0]._matrix = new double[matrix1.NumDocs][];
            matrices[0]._docIndex = new List<string>(matrix1._docIndex);
            matrices[0]._docIndexLookup = new Dictionary<string,int>(matrix1._docIndexLookup);
            // matrix 2
            matrices.Add(new TermDocumentMatrix());
            matrices[1]._matrix = new double[matrix2.NumDocs][];
            matrices[1]._docIndex = new List<string>(matrix2._docIndex);
            matrices[1]._docIndexLookup = new Dictionary<string,int>(matrix2._docIndexLookup);
            // compute term set
            List<string> termIndex = new List<string>();
            Dictionary<string, int> termIndexLookup = new Dictionary<string, int>();
            Dictionary<string, int> leftovers = new Dictionary<string,int>(matrix2._termIndexLookup);
            // get all terms in first matrix
            foreach (string term in matrix1._termIndex)
            {
                termIndex.Add(term);
                termIndexLookup.Add(term, termIndex.Count - 1);
                // remove duplicate terms
                if (matrix2._termIndexLookup.ContainsKey(term))
                {
                    leftovers.Remove(term);
                }
            }
            // add leftovers
            foreach (string term in leftovers.Keys)
            {
                termIndex.Add(term);
                termIndexLookup.Add(term, termIndex.Count - 1);
            }
            // create new term distributions for each document
            // matrix 1
            matrices[0]._termIndex = new List<string>(termIndex);
            matrices[0]._termIndexLookup = new Dictionary<string,int>(termIndexLookup);
            for (int i = 0; i < matrices[0].NumDocs; i++)
            {
                matrices[0]._matrix[i] = new double[termIndex.Count];
                // fill in original values
                for (int j = 0; j < matrix1.NumTerms; j++)
                {
                    matrices[0][i, j] = matrix1[i, j];
                }
                // fill in missing terms
                for (int j = matrix1.NumTerms; j < termIndex.Count; j++)
                {
                    matrices[0][i, j] = 0.0;
                }
            }
            // matrix 2
            matrices[1]._termIndex = new List<string>(termIndex);
            matrices[1]._termIndexLookup = new Dictionary<string,int>(termIndexLookup);
            for (int i = 0; i < matrices[1].NumDocs; i++)
            {
                matrices[1]._matrix[i] = new double[termIndex.Count];
                // fill in values
                for (int j = 0; j < termIndex.Count; j++)
                {
                    if (matrix2.ContainsTerm(termIndex[j]))
                    {
                        matrices[1][i, j] = matrix2.GetValue(matrix2.GetDocumentName(i), termIndex[j]);
                    }
                    else
                    {
                        matrices[1][i, j] = 0.0;
                    }
                }
            }
            // return
            return matrices;
        }

        /// <summary>
        /// Takes the two specified documents and creates two new document vectors with the missing terms from each.
        /// Row 0: document 1
        /// Row 1: document 2
        /// </summary>
        /// <param name="matrix1">document1 container</param>
        /// <param name="document1">document1 index</param>
        /// <param name="matrix2">document2 container</param>
        /// <param name="document2">document2 index</param>
        /// <returns>New term-by-document matrix containing the two documents and their term maps</returns>
        public static TermDocumentMatrix EqualizeDocuments(TermDocumentMatrix matrix1, int document1, TermDocumentMatrix matrix2, int document2)
        {
            // initialize new TermDocumentMatrix
            TermDocumentMatrix newmatrix = new TermDocumentMatrix();
            newmatrix._matrix = new double[2][];
            newmatrix._termIndex = new List<string>();
            newmatrix._termIndexLookup = new Dictionary<string, int>();
            newmatrix._docIndex = new List<string>();
            newmatrix._docIndexLookup = new Dictionary<string, int>();
            newmatrix._docIndex.Add(matrix1.GetDocumentName(document1));
            newmatrix._docIndexLookup.Add(matrix1.GetDocumentName(document1), newmatrix._docIndex.Count - 1);
            newmatrix._docIndex.Add(matrix2.GetDocumentName(document2));
            newmatrix._docIndexLookup.Add(matrix2.GetDocumentName(document2), newmatrix._docIndex.Count - 1);
            List<double> doc1 = new List<double>();
            List<double> doc2 = new List<double>();
            // compute total term set
            Dictionary<string, int> leftovers = new Dictionary<string,int>(matrix2._termIndexLookup);
            foreach (string term in matrix1._termIndex)
            {
                newmatrix._termIndex.Add(term);
                newmatrix._termIndexLookup.Add(term, newmatrix._termIndex.Count - 1);
                doc1.Add(matrix1.GetValue(document1, matrix1.GetTermIndex(term)));
                if (matrix2._termIndexLookup.ContainsKey(term))
                {
                    leftovers.Remove(term);
                    doc2.Add(matrix2.GetValue(document2, matrix2.GetTermIndex(term)));
                }
                else
                {
                    doc2.Add(0.0);
                }
            }
            foreach (string term in leftovers.Keys)
            {
                newmatrix._termIndex.Add(term);
                newmatrix._termIndexLookup.Add(term, newmatrix._termIndex.Count - 1);
                doc1.Add(0.0);
                doc2.Add(matrix2.GetValue(document2, matrix2.GetTermIndex(term)));
            }
            newmatrix._matrix[0] = doc1.ToArray();
            newmatrix._matrix[1] = doc2.ToArray();
            return newmatrix;
        }

        /// <summary>
        /// Takes the two specified documents and creates two new document vectors with the missing terms from each.
        /// </summary>
        /// <param name="matrix1">artifact1 container</param>
        /// <param name="artifact1">artifact1 ID</param>
        /// <param name="matrix2">artifact2 container</param>
        /// <param name="artifact2">artifact2 ID</param>
        /// <returns>New term-by-document matrix containing the two documents and their term maps</returns>
        public static TermDocumentMatrix EqualizeDocuments(TermDocumentMatrix matrix1, string artifact1, TermDocumentMatrix matrix2, string artifact2)
        {
            return EqualizeDocuments(matrix1, matrix1.GetDocumentIndex(artifact1), matrix2, matrix2.GetDocumentIndex(artifact2));
        }

        #endregion

        #region Static I/O

        /// <summary>
        /// Separates entries in the matrix when reading from or saving to disk
        /// </summary>
        private static string IODelimeter = " ";

        /// <summary>
        /// Saves matrix to file
        /// </summary>
        /// <param name="matrix">Term-by-document matrix</param>
        /// <param name="filename">File location</param>
        public static void Save(TermDocumentMatrix matrix, string filename)
        {
            // attempt to create file
            TextWriter tw = new StreamWriter(File.Open(filename, FileMode.Create));
            // print out term list
            foreach (string term in matrix.TermMap)
            {
                tw.Write("{0}{1}", TermDocumentMatrix.IODelimeter, term);
            }
            tw.WriteLine();
            // print out each document
            for (int i = 0; i < matrix.NumDocs; i++)
            {
                tw.Write(matrix.GetDocumentName(i));
                // print out each term
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    tw.Write("{0}{1}", TermDocumentMatrix.IODelimeter, matrix[i, j]);
                }
                tw.WriteLine();
            }
            // close file
            tw.Flush();
            tw.Close();
        }

        /// <summary>
        /// Loads a previously saved TermDocumentMatrix from disk.
        /// </summary>
        /// <param name="filename">File location</param>
        /// <returns>Term-by-document matrix</returns>
        public static TermDocumentMatrix Load(string filename)
        {
            TextReader tr = new StreamReader(File.OpenRead(filename));
            TermDocumentMatrix matrix = new TermDocumentMatrix();
            int lineNum = 1;
            string line = tr.ReadLine();
            string[] delimeter = new string[] { TermDocumentMatrix.IODelimeter };
            // read terms
            matrix._termIndex = new List<string>(line.Split(delimeter, StringSplitOptions.RemoveEmptyEntries));
            matrix._termIndexLookup = new Dictionary<string, int>();
            for (int i = 0; i < matrix._termIndex.Count; i++)
            {
                matrix._termIndexLookup.Add(matrix._termIndex[i], i);
            }
            // read documents
            matrix._docIndex = new List<string>();
            matrix._docIndexLookup = new Dictionary<string, int>();
            List<double[]> docs = new List<double[]>();
            while ((line = tr.ReadLine()) != null)
            {
                lineNum++;
                string[] document = line.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                if (document.Length != matrix.NumTerms + 1)
                {
                    tr.Close();
                    throw new InvalidDataException("Incorrect data format on line " + lineNum + " in file: " + filename);
                }
                matrix._docIndex.Add(document[0]);
                matrix._docIndexLookup.Add(document[0], matrix._docIndex.Count - 1);
                double[] doc = new double[matrix.NumTerms];
                for (int i = 1; i < document.Length; i++)
                {
                    doc[i - 1] = Convert.ToDouble(document[i]);
                }
                docs.Add(doc);
            }
            // add documents
            matrix._matrix = new double[matrix.NumDocs][];
            for (int i = 0; i < matrix.NumDocs; i++)
            {
                matrix._matrix[i] = new double[matrix.NumTerms];
                for (int j = 0; j < matrix.NumTerms; j++)
                {
                    matrix[i, j] = docs[i][j];
                }
            }
            // cleanup
            tr.Close();
            return matrix;
        }

        #endregion

    }
}
