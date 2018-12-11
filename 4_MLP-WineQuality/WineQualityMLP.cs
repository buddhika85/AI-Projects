using System;
using System.IO;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.CSV.Segregate;
using Encog.App.Analyst.CSV.Shuffle;
using Encog.App.Analyst.Wizard;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Lma;
using Encog.Neural.Networks.Training.Propagation.Manhattan;
using Encog.Neural.Networks.Training.Propagation.Quick;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training.Propagation.SCG;
using Encog.Persist;
using Encog.Util.Arrayutil;
using Encog.Util.CSV;
using Encog.Util.Simple;

namespace _4_MLP_WineQuality
{
    public class WineQualityMlp
    {
        /// <summary>
        /// Shuffle the csv file
        /// </summary>
        /// <param name="source"></param>
        public void ShuffleOriginalCsv(FileInfo source, FileInfo destination)
        {
            try
            {
                var shuffle = new ShuffleCSV();
                shuffle.Analyze(source, true, CSVFormat.English);
                shuffle.ProduceOutputHeaders = true;
                shuffle.Process(destination);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void SegregateToTrainAndEvalSets(FileInfo shuffledBaseFile, FileInfo trainingFile, FileInfo testingFile, object trainingPercentage, object testingPercentage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Segregate source file into training and evaluation file
        /// </summary>
        /// <param name="shuffledSource"></param>
        /// <param name="trainingFile"></param>
        /// <param name="testingFile"></param>
        /// <param name="trainingPercentage"></param>
        /// <param name="testingPercentage"></param>
        public void SegregateToTrainAndEvalSets(FileInfo shuffledSource, FileInfo trainingFile, FileInfo testingFile, int trainingPercentage, int testingPercentage)
        {
            try
            {
                var seg = new SegregateCSV();
                seg.Targets.Add(new SegregateTargetPercent(trainingFile, trainingPercentage));
                seg.Targets.Add(new SegregateTargetPercent(testingFile, testingPercentage));
                seg.ProduceOutputHeaders = true;
                seg.Analyze(shuffledSource, true, CSVFormat.English);
                seg.Process();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Create normalised traning, testing files with encoh analist file
        /// </summary>
        /// <param name="baseFile"></param>
        /// <param name="trainingFile"></param>
        /// <param name="testingFile"></param>
        /// <param name="normalisedTrainingFile"></param>
        /// <param name="normalisedTestingFile"></param>
        /// <param name="analystFile"></param>
        public void Normalise(FileInfo baseFile, FileInfo trainingFile, FileInfo testingFile, FileInfo normalisedTrainingFile, FileInfo normalisedTestingFile,FileInfo analystFile)
        {
            try
            {
                //Analyst
                var analyst = new EncogAnalyst();

                //Wizard
                var wizard = new AnalystWizard(analyst);
                wizard.Wizard(baseFile, true, AnalystFileFormat.DecpntComma);

                // inputs
                // 1 - fixed acidity
                analyst.Script.Normalize.NormalizedFields[0].Action = NormalizationAction.Normalize; // contniues
                // 2 - volatile acidity
                analyst.Script.Normalize.NormalizedFields[1].Action = NormalizationAction.Normalize; // contniues
                // 3 - citric acid  
                analyst.Script.Normalize.NormalizedFields[2].Action = NormalizationAction.Normalize; // contniues
                // 4 - residual sugar
                analyst.Script.Normalize.NormalizedFields[3].Action = NormalizationAction.Normalize; // contniues
                // 5 - chlorides
                analyst.Script.Normalize.NormalizedFields[4].Action = NormalizationAction.Normalize; // contniues
                // 6 - free sulfur dioxide
                analyst.Script.Normalize.NormalizedFields[5].Action = NormalizationAction.Normalize; // discrete
                // 7 - total sulfur dioxide
                analyst.Script.Normalize.NormalizedFields[6].Action = NormalizationAction.Normalize; // discrete
                // 8 - density
                analyst.Script.Normalize.NormalizedFields[7].Action = NormalizationAction.Normalize; // contniues
                // 9 - pH
                analyst.Script.Normalize.NormalizedFields[8].Action = NormalizationAction.Normalize; // contniues
                // 10 - sulphates
                analyst.Script.Normalize.NormalizedFields[9].Action = NormalizationAction.Normalize; // contniues
                // 11 - alcohol
                analyst.Script.Normalize.NormalizedFields[10].Action = NormalizationAction.Normalize; // contniues
                // output
                // 12 - quality 
                analyst.Script.Normalize.NormalizedFields[11].Action = NormalizationAction.Equilateral; // discrete


                //Norm for Trainng
                var norm = new AnalystNormalizeCSV
                {
                    ProduceOutputHeaders = true
                };

                norm.Analyze(trainingFile, true, CSVFormat.English, analyst);
                norm.Normalize(normalisedTrainingFile);

                //Norm of evaluation
                norm.Analyze(Config.TestingFile, true, CSVFormat.English, analyst);
                norm.Normalize(normalisedTestingFile);

                //save the analyst file
                analyst.Save(analystFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CreateNetwork(FileInfo networkFile)
        {
            try
            {
                var network = new BasicNetwork();
                network.AddLayer(new BasicLayer(new ActivationLinear(), true, 11));        // all continues inputs - so 11
                network.AddLayer(new BasicLayer(new ActivationTANH(), true, 15));           // one hidden layer
                network.AddLayer(new BasicLayer(new ActivationTANH(), false, 5));           // 5 classes for the output
                network.Structure.FinalizeStructure();
                network.Reset();
                EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void TrainNetwork(FileInfo ann, FileInfo trainingFile)
        {
            try
            {
                var network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(ann);
                var trainingSet = EncogUtility.LoadCSV2Memory(trainingFile.ToString(),
                    network.InputCount, network.OutputCount, true, CSVFormat.English, false);
                var trainerAlgorithm = new ResilientPropagation(network, trainingSet);                  // 16617 ==> e 0.1
                //var trainerAlgorithm = new QuickPropagation(network, trainingSet, 2.0);
                //var trainerAlgorithm = new ManhattanPropagation(network, trainingSet, 0.001);           //   
                //var trainerAlgorithm = new ScaledConjugateGradient(network, trainingSet);                   // 73799  ==> e 0.1
                //var trainerAlgorithm = new LevenbergMarquardtTraining(network, trainingSet);          // 32750 ==> e 0.1
                var iteration = 1;
                do
                {
                    trainerAlgorithm.Iteration();
                    Console.WriteLine("Epoch : {0} Error : {1}", iteration, trainerAlgorithm.Error);
                    iteration++;
                } while (trainerAlgorithm.Error > 0.1);

                EncogDirectoryPersistence.SaveObject(ann, (BasicNetwork)network);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void EvaluateNetwork(FileInfo trainedNetwork, FileInfo analystFile, FileInfo normalisedTestFile, FileInfo finalResultsFile)
        {
            try
            {
                var network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(trainedNetwork);
                var analyst = new EncogAnalyst();
                analyst.Load(analystFile.ToString());
                var evaluationSet = EncogUtility.LoadCSV2Memory(normalisedTestFile.ToString(),
                    network.InputCount, network.OutputCount, true, CSVFormat.English, false);

                using (var file = new StreamWriter(finalResultsFile.ToString()))
                {
                    foreach (var item in evaluationSet)
                    {

                        var normalizedActualoutput = (BasicMLData)network.Compute(item.Input);
                        //var actualoutput = analyst.Script.Normalize.NormalizedFields[11].DeNormalize(normalizedActualoutput.Data[0]);
                        //var idealOutput = analyst.Script.Normalize.NormalizedFields[11].DeNormalize(item.Ideal[0]);

                        int classCount = analyst.Script.Normalize.NormalizedFields[11].Classes.Count;
                        double normalizationHigh = analyst.Script.Normalize.NormalizedFields[11].NormalizedHigh;
                        double normalizationLow = analyst.Script.Normalize.NormalizedFields[11].NormalizedLow;

                        var eq = new Encog.MathUtil.Equilateral(classCount, normalizationHigh, normalizationLow);
                        var predictedClassInt = eq.Decode(normalizedActualoutput);
                        var idealClassInt = eq.Decode(item.Ideal);

                        //Write to File
                        var resultLine = idealClassInt.ToString() + "," + predictedClassInt.ToString();
                        file.WriteLine(resultLine);
                        Console.WriteLine("Ideal : {0}, Actual : {1}", idealClassInt, predictedClassInt);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}