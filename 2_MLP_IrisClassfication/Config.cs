using Encog.Util.File;
using System.IO;

namespace _2_MLP_IrisClassfication
{
    public static class Config
 {
     public static FileInfo BasePath = new FileInfo(@"D:\PhD\IS - AI - ACBT\EncogCSharpProjects\2_MLP_IrisClassfication\Data\");
   
     #region "Step1"

     public static FileInfo BaseFile = FileUtil.CombinePath(BasePath, "IrisData.csv");
     public static FileInfo ShuffledBaseFile = FileUtil.CombinePath(BasePath, "Iris_Shuffled.csv");
   
     #endregion
          
     #region "Step2"

     public static FileInfo TrainingFile = FileUtil.CombinePath(BasePath, "Iris_Train.csv");
     public static FileInfo EvaluateFile = FileUtil.CombinePath(BasePath, "Iris_Eval.csv");

     #endregion


     #region "Step3"

     public static FileInfo NormalizedTrainingFile = FileUtil.CombinePath(BasePath, "Iris_Train_Norm.csv");
     public static FileInfo NormalizedEvaluateFile = FileUtil.CombinePath(BasePath, "Iris_Eval_Norm.csv");
     public static FileInfo AnalystFile = FileUtil.CombinePath(BasePath, "Iris_Analyst.ega");
   
     #endregion

     
     #region "Step4"

     public static FileInfo TrainedNetworkFile = FileUtil.CombinePath(BasePath, "Iris_Train.eg");
   
     #endregion

        
     #region "Step5"

     #endregion

     #region "Step6"

     
     #endregion

   }
}
