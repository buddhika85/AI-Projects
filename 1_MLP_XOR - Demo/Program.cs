using Encog;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using System;

namespace _1_MLP_XOR___Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // input data
            double[][] xorInput =
            {
                new[] {0.0, 0.0},
                new[] {1.0, 0.0},
                new[] {0.0, 1.0},
                new[] {1.0, 1.0}
            };

            // expected/ideal data
            double[][] xorIdeal =
            {
                new[] {0.0},
                new[] {1.0},
                new[] {1.0},
                new[] {0.0}
            };

            // create training data set
            var trainingSet = new BasicMLDataSet(xorInput, xorIdeal);

            // create evaluation data set
            var evaluationSet = trainingSet;    // for this example train data === evaluation data

            // create initial ANN
            BasicNetwork network = CreateBasicNetwork();

            // train ANN
            network = TrainBasicNetwork(network, trainingSet);

            // evaluate ANN
            EvaluateTrainedNetwork(network, trainingSet);

            EncogFramework.Instance.Shutdown();
            Console.ReadKey();
        }

        private static void EvaluateTrainedNetwork(BasicNetwork network, BasicMLDataSet evalutionDataSet)
        {
            foreach (var evaluationData in evalutionDataSet)
            {
                var predictedOutput = network.Compute(evaluationData.Input);
                Console.WriteLine(
                    $"Input : {evaluationData.Input[0]}  {evaluationData.Input[1]} \t Ideal : {evaluationData.Ideal[0]} \t Actual : {predictedOutput[0]}");
            }
        }

        private static BasicNetwork  TrainBasicNetwork(BasicNetwork network, BasicMLDataSet trainingSet)
        {
            var trainerAlgorithm = new Backpropagation(network, trainingSet, 0.7, 0.2);
            //var trainerAlgorithm = new ResilientPropagation(network, trainingSet);
            //var trainerAlgorithm = new ManhattanPropagation(network, trainingSet, 0.001)
            //var trainerAlgorithm = new ScaledConjugateGradient(network, trainingSet);
            //var trainerAlgorithm = new LevenbergMarquardtTraining(network, trainingSet);          //
            //var trainerAlgorithm = new QuickPropagation(network, trainingSet, 2.0);                 //

            var iteration = 1;
            do
            {
                trainerAlgorithm.Iteration();
                iteration++;
                Console.WriteLine($"Iteration Num : {iteration}, Error : {trainerAlgorithm.Error}");
            } while (trainerAlgorithm.Error > 0.001);
            trainerAlgorithm.FinishTraining();

            return network;
        }

        private static BasicNetwork CreateBasicNetwork()
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 2));                            // input layer
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));         // hidden layer
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));        // output layer
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }
    }
}
