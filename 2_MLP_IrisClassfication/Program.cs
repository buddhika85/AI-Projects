using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.CSV.Segregate;
using Encog.App.Analyst.CSV.Shuffle;
using Encog.App.Analyst.Wizard;
using Encog.Engine.Network.Activation;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;
using Encog.Util.CSV;
using Encog.Util.Simple;
using System;
using System.IO;

namespace _2_MLP_IrisClassfication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("###################### Step 1 #########################");
            //Step1();    // Shuffle Data

            //Console.WriteLine("###################### Step 2 #########################");
            //Step2();    // Segregate to Train and Eval

            //Console.WriteLine("###################### Step 3 #########################");
            //Step3();    // Normalise Data

            //Console.WriteLine("###################### Step 4 #########################");
            //Step4();    // Create network

            //Console.WriteLine("###################### Step 5 #########################");
            //Step5();       // Train

            Console.WriteLine("###################### Step 6 #########################");
            Step6();

            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
        }

        #region "Step1 : Shuffle"

        static void Step1()
        {
            Console.WriteLine("Step 1: Shuffle CSV Data File");
            Shuffle(Config.BaseFile);
        }

        static void Shuffle(FileInfo source)
        {
            //Shuffle the CSV File
            var shuffle = new ShuffleCSV();
            shuffle.Analyze(source, true, CSVFormat.English);
            shuffle.ProduceOutputHeaders = true;
            shuffle.Process(Config.ShuffledBaseFile);
        }


        #endregion

        #region "Step2 : Segregate"

        static void Step2()
        {
            Console.WriteLine("Step 2: Generate training and Evaluation  file");
            Segregate(Config.ShuffledBaseFile);

        }

        static void Segregate(FileInfo source)
        {
            //Segregate source file into training and evaluation file
            var seg = new SegregateCSV();
            seg.Targets.Add(new SegregateTargetPercent(Config.TrainingFile, 75));
            seg.Targets.Add(new SegregateTargetPercent(Config.EvaluateFile, 25));
            seg.ProduceOutputHeaders = true;
            seg.Analyze(source, true, CSVFormat.English);
            seg.Process();

        }

        #endregion

        #region "Step3 : Normalize"
        static void Step3()
        {
            Console.WriteLine("Step 3: Normalize Training and Evaluation Data");

            //Analyst
            var analyst = new EncogAnalyst();

            //Wizard
            var wizard = new AnalystWizard(analyst);
            wizard.Wizard(Config.BaseFile, true, AnalystFileFormat.DecpntComma);



            //Norm for Trainng
            var norm = new AnalystNormalizeCSV();
            norm.Analyze(Config.TrainingFile, true, CSVFormat.English, analyst);
            norm.ProduceOutputHeaders = true;
            norm.Normalize(Config.NormalizedTrainingFile);

            //Norm of evaluation
            norm.Analyze(Config.EvaluateFile, true, CSVFormat.English, analyst);
            norm.Normalize(Config.NormalizedEvaluateFile);

            //save the analyst file
            analyst.Save(Config.AnalystFile);
        }


        #endregion

        #region "Step4 : Create Network"

        static void Step4()
        {
            Console.WriteLine("Step 4: Create Neural Network");
            CreateNetwork(Config.TrainedNetworkFile);
        }

        static void CreateNetwork(FileInfo networkFile)
        {
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(), true, 4));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 6));
            network.AddLayer(new BasicLayer(new ActivationTANH(), false, 2));
            network.Structure.FinalizeStructure();
            network.Reset();
            EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);

        }

        #endregion

        #region "Step5 : Train Network"

        static void Step5()
        {
            Console.WriteLine("Step 5: Train Neural Network");
            TrainNetwork();
        }

        static void TrainNetwork()
        {
            var network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(Config.TrainedNetworkFile);
            var trainingSet = EncogUtility.LoadCSV2Memory(Config.NormalizedTrainingFile.ToString(),
                network.InputCount, network.OutputCount, true, CSVFormat.English, false);


            var train = new ResilientPropagation(network, trainingSet);
            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine("Epoch : {0} Error : {1}", epoch, train.Error);
                epoch++;
            } while (train.Error > 0.01);

            EncogDirectoryPersistence.SaveObject(Config.TrainedNetworkFile, (BasicNetwork)network);

        }

        #endregion


        #region "Step6 : Evaluate"
        static void Step6()
        {
            Console.WriteLine("Step 6: Evaluate Network");
            Evaluate();
        }


        static void Evaluate()
        {

            var network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(Config.TrainedNetworkFile);
            var analyst = new EncogAnalyst();
            analyst.Load(Config.AnalystFile.ToString());
            var evaluationSet = EncogUtility.LoadCSV2Memory(Config.NormalizedEvaluateFile.ToString(),
                network.InputCount, network.OutputCount, true, CSVFormat.English, false);

            int count = 0;
            int CorrectCount = 0;
            foreach (var item in evaluationSet)
            {
                count++;
                var output = network.Compute(item.Input);

                var sepal_l = analyst.Script.Normalize.NormalizedFields[0].DeNormalize(item.Input[0]);
                var sepal_w = analyst.Script.Normalize.NormalizedFields[1].DeNormalize(item.Input[1]);
                var petal_l = analyst.Script.Normalize.NormalizedFields[2].DeNormalize(item.Input[2]);
                var petal_w = analyst.Script.Normalize.NormalizedFields[3].DeNormalize(item.Input[3]);

                int classCount = analyst.Script.Normalize.NormalizedFields[4].Classes.Count;
                double normalizationHigh = analyst.Script.Normalize.NormalizedFields[4].NormalizedHigh;
                double normalizationLow = analyst.Script.Normalize.NormalizedFields[4].NormalizedLow;

                var eq = new Encog.MathUtil.Equilateral(classCount, normalizationHigh, normalizationLow);
                var predictedClassInt = eq.Decode(output);
                var predictedClass = analyst.Script.Normalize.NormalizedFields[4].Classes[predictedClassInt].Name;
                var idealClassInt = eq.Decode(item.Ideal);
                var idealClass = analyst.Script.Normalize.NormalizedFields[4].Classes[idealClassInt].Name;

                if (predictedClassInt == idealClassInt)
                {
                    CorrectCount++;
                }
                Console.WriteLine("Count :{0} Properties [{1},{2},{3},{4}] ,Ideal : {5} Predicted : {6} ",
                    count, sepal_l, sepal_w, petal_l, petal_w, idealClass, predictedClass);

            }

            Console.WriteLine("Total Test Count : {0}", count);
            Console.WriteLine("Total Correct Prediction Count : {0}", CorrectCount);
            Console.WriteLine("% Success : {0}", ((CorrectCount * 100.0) / count));
        }



        #endregion
    }
}
