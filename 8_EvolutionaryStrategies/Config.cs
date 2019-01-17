namespace _8_EvolutionaryStrategies
{
    public static class Config
    {
        public static int NumberOfParentsMiu = 30;
        public static int NumberOfChildrenLambda = NumberOfParentsMiu * 6;
        public static int NumberOfGenerationsM = 100;
        public static double OneOverFiveSigmaRuleConstant = 0.85;
        public static double InitialX1 = 0;
        public static double InitialX2 = 8;
        public static double InitialSigmaX1 = 1.25;
        public static double InitialSigmaX2 = 1;
        public static double MinX1 = -5;
        public static double MaxX1 = 10;
        public static double MinX2 = 0;
        public static double MaxX2 = 15;
    }
}