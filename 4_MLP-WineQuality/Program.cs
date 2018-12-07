using System;

namespace _4_MLP_WineQuality
{

    // Ref - https://archive.ics.uci.edu/ml/machine-learning-databases/wine-quality/
    // Ref - https://archive.ics.uci.edu/ml/datasets/Wine+Quality 
    // Predict winde quality using 11 input atributes 
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var wineQuailityMlp = new WineQualityMlp();
                // 1 shuffle
                wineQuailityMlp.ShuffleOriginalCsv(Config.BaseFile, Config.ShuffledBaseFile);
                Console.WriteLine(
                    $"Step 1 - \nShuffling original file - {Config.BaseFile.Name}.\nShuffled data file - {Config.ShuffledBaseFile.Name}");
                DisplaySperatetor();

                // 2 segregate 
                wineQuailityMlp.SegregateToTrainAndEvalSets(Config.ShuffledBaseFile, Config.TrainingFile,
                    Config.TestingFile, Config.TrainingPercentage, Config.TestingPercentage);
                Console.WriteLine(
                    $"Step 2 - \nTraining and Testing files created - \n{Config.TrainingFile.Name}  \n{Config.TestingFile.Name}");
                DisplaySperatetor();

                // 3 normalise
                wineQuailityMlp.Normalise(Config.BaseFile, Config.TrainingFile, Config.TestingFile, Config.NormalizedTrainingFile, Config.NormalizedTestingFile, Config.AnalystFile);
                Console.WriteLine(
                    $"Step 3 - \nNormalised and Testing files created - \n{Config.TrainingFile.Name}  \n{Config.TestingFile.Name}");
                DisplaySperatetor();

                // 4 create network
                wineQuailityMlp.CreateNetwork(Config.TrainedNetworkFile);
                Console.WriteLine(
                    $"Step 4 - \nANN created - \n{Config.TrainedNetworkFile.Name}");
                DisplaySperatetor();

                // 5 train network
                wineQuailityMlp.TrainNetwork(Config.TrainedNetworkFile, Config.NormalizedTrainingFile);
                Console.WriteLine(
                    $"Step 5 - \nANN trained - \n{Config.TrainedNetworkFile.Name}");
                DisplaySperatetor();

                // 6 evaluate network
                wineQuailityMlp.EvaluateNetwork(Config.TrainedNetworkFile, Config.AnalystFile,
                    Config.NormalizedTestingFile, Config.ValidationResult);
                Console.WriteLine(
                    $"Step 6 - \nANN evaluated and results are in - \n{Config.ValidationResult.Name}");

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DisplaySperatetor()
        {
            Console.WriteLine("\n-------------- \n");
        }
    }
}
