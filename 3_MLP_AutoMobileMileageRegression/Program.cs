using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.CSV.Segregate;
using Encog.App.Analyst.CSV.Shuffle;
using Encog.App.Analyst.Wizard;
using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;
using Encog.Util.CSV;
using Encog.Util.Simple;
using System;
using System.IO;

namespace _3_MLP_AutoMobileMileageRegression
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("###################### Step 1 #########################");
            Step1();    // shuffle

            Console.WriteLine("###################### Step 2 #########################");
            Step2();    // segregate

            Console.WriteLine("###################### Step 3 #########################");
            Step3();    // normalise

            Console.WriteLine("###################### Step 4 #########################");
            Step4();    // create network

            Console.WriteLine("###################### Step 5 #########################");
            Step5();    // train

            Console.WriteLine("###################### Step 6 #########################");
            Step6();    // evaluate

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
            //Cylinders  
            analyst.Script.Normalize.NormalizedFields[0].Action = Encog.Util.Arrayutil.NormalizationAction.Equilateral;     // multivalued discreste field type
            //displacement
            analyst.Script.Normalize.NormalizedFields[1].Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;       // continues
            //HorsePower
            analyst.Script.Normalize.NormalizedFields[2].Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;
            //weight
            analyst.Script.Normalize.NormalizedFields[3].Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;
            //Acceleration
            analyst.Script.Normalize.NormalizedFields[4].Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;
            //year
            analyst.Script.Normalize.NormalizedFields[5].Action = Encog.Util.Arrayutil.NormalizationAction.Equilateral;     // multivalued discreste field type
            //Origin
            analyst.Script.Normalize.NormalizedFields[6].Action = Encog.Util.Arrayutil.NormalizationAction.Equilateral;     // multivalued discreste field type
            //Name
            analyst.Script.Normalize.NormalizedFields[7].Action = Encog.Util.Arrayutil.NormalizationAction.Ignore;          // will not use
            //mpg
            analyst.Script.Normalize.NormalizedFields[8].Action = Encog.Util.Arrayutil.NormalizationAction.Normalize;       // contniues



            //Norm for Trainng
            var norm = new AnalystNormalizeCSV();
            norm.ProduceOutputHeaders = true;

            norm.Analyze(Config.TrainingFile, true, CSVFormat.English, analyst);
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
            network.AddLayer(new BasicLayer(new ActivationLinear(), true, 22));
            network.AddLayer(new BasicLayer(new ActivationTANH(), true, 6));
            network.AddLayer(new BasicLayer(new ActivationTANH(), false, 1));
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

            using (var file = new System.IO.StreamWriter(Config.ValidationResult.ToString()))
            {
                foreach (var item in evaluationSet)
                {

                    var NormalizedActualoutput = (BasicMLData)network.Compute(item.Input);
                    var Actualoutput = analyst.Script.Normalize.NormalizedFields[8].DeNormalize(NormalizedActualoutput.Data[0]);
                    var IdealOutput = analyst.Script.Normalize.NormalizedFields[8].DeNormalize(item.Ideal[0]);

                    //Write to File
                    var resultLine = IdealOutput.ToString() + "," + Actualoutput.ToString();
                    file.WriteLine(resultLine);
                    Console.WriteLine("Ideal : {0}, Actual : {1}", IdealOutput, Actualoutput);

                }

            }








        }



        #endregion

    }
}
