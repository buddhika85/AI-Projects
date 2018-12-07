using System.IO;
using Encog.Util.File;

namespace _4_MLP_WineQuality
{
    public class Config
    {
        public static FileInfo BasePath = new FileInfo(@"D:\PhD\IS - AI - ACBT\EncogCSharpProjects\4_MLP-WineQuality\Data");

        #region "Step1"

        public static FileInfo BaseFile = FileUtil.CombinePath(BasePath, "0_winequality-red.csv");
        public static FileInfo ShuffledBaseFile = FileUtil.CombinePath(BasePath, "1_winequality-red_Shuffled.csv");

        #endregion


        #region "Step2"
        public static FileInfo TrainingFile = FileUtil.CombinePath(BasePath, "2_winequality-red_Train.csv");
        public static FileInfo TestingFile = FileUtil.CombinePath(BasePath, "2_winequality-red_Eval.csv");
        public static int TestingPercentage = 25;
        public static int TrainingPercentage = 75;
        #endregion


        #region "Step3"

        public static FileInfo NormalizedTrainingFile = FileUtil.CombinePath(BasePath, "3_winequality-red_Train_Norm.csv");
        public static FileInfo NormalizedTestingFile = FileUtil.CombinePath(BasePath, "3_winequality-red_Eval_Norm.csv");
        public static FileInfo AnalystFile = FileUtil.CombinePath(BasePath, "3_winequality-red_Analyst.ega");

        #endregion


        #region "Step4"

        public static FileInfo TrainedNetworkFile = FileUtil.CombinePath(BasePath, "4_5_winequality-red_Train.eg");

        #endregion


        #region "Step5"
        #endregion

        #region "Step6"

        public static FileInfo ValidationResult = FileUtil.CombinePath(BasePath, "6_winequality-red_ValidationResult.csv");
        
        #endregion
    }
}
