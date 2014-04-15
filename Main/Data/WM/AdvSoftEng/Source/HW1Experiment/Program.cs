using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLabSDK.Types;
using AdvSoftEng.Types;
using AdvSoftEng.Importers;
using AdvSoftEng.Models;
using AdvSoftEng.Exporters;


namespace HW1Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> config = new Dictionary<string,string>();
            string relativeLocation = "..\\..\\..\\..\\";

            config.Add("idPath", System.IO.Path.Combine(relativeLocation, "Experiment\\Rhino\\RhinoCorpusMapping.txt"));
            config.Add("docPath", System.IO.Path.Combine(relativeLocation, "Experiment\\Rhino\\RhinoCorpus.txt"));
            config.Add("qidPath", System.IO.Path.Combine(relativeLocation, "Experiment\\Rhino\\RhinoListOfFeatures.txt"));
            config.Add("qdocPath", System.IO.Path.Combine(relativeLocation, "Experiment\\Rhino\\RhinoQueries.txt"));
            config.Add("goldSetDir", System.IO.Path.Combine(relativeLocation, "Experiment\\Rhino\\RhinoFeaturesToGoldSetMethodsMapping"));
            config.Add("effAllPath", System.IO.Path.Combine(relativeLocation, "Experiment\\EffectivenessAllMethods.txt"));
            config.Add("effBestPath", System.IO.Path.Combine(relativeLocation, "Experiment\\EffectivenessBestMethods.txt"));

            Console.WriteLine("Running experiment...");
            Console.WriteLine("Importing corpus...");
            TLArtifactsCollection corpusArtifacts = Corpus.Import(config["idPath"], config["docPath"]);
            Console.WriteLine("Computing corpus vectors...");
            Vectorizer corpusVectors = new Vectorizer(corpusArtifacts, "Ordinal");
            Console.WriteLine("Computing corpus tf, df...");
            Normalizer corpusTF = new Normalizer(corpusVectors.Vectors);
            Console.WriteLine("Computing corpus idf...");
            NormalizedVector corpusIDF = InverseDocumentFrequency.Compute(corpusVectors.Frequencies, corpusVectors.Vectors.Count);
            Console.WriteLine("Computing corpus tf-idf...");
            NormalizedVectorCollection corpusTFIDF = TFIDF.Compute(corpusTF.Vectors, corpusIDF);
            Console.WriteLine("Importing queries...");
            TLArtifactsCollection queryArtifacts = Corpus.Import(config["qidPath"], config["qdocPath"]);
            Console.WriteLine("Computing corpus vectors...");
            Vectorizer queryVectors = new Vectorizer(queryArtifacts, "Boolean");
            Console.WriteLine("Computing similarities...");
            TLSimilarityMatrix sims = CosineSimilarity.Compute(corpusTF.Vectors, corpusTF.Lengths, queryVectors.Vectors);
            Console.WriteLine("Importing gold set...");
            TLSimilarityMatrix goldset = AnswerMapping.Import(config["goldSetDir"]);
            Console.WriteLine("Calculating effectiveness measures...");
            Effectiveness.Export(queryArtifacts, sims, goldset, config["effAllPath"], config["effBestPath"]);
            Console.WriteLine("Effectiveness measures written to:\n\t" + config["effAllPath"] + "\n\t" + config["effBestPath"]);
            Console.WriteLine("Experiment complete.");

            Console.WriteLine("\nPress enter key to continue...");
            Console.ReadLine();
        }
    }
}
