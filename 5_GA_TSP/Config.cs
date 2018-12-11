namespace _5_GA_TSP
{
    public static class Config
    {
        public const int CityCount = 20;
        public const int PopulationSize = 1000;

        public const double CrossOverProbabality = 0.9;
        public const double MutationProbabality = 0.1;

        public const int MaxXCordinate = 256;
        public const int MaxYCordinate = 256;

        public const int MaxNumIterationsSameSolution = 25;     // maximum number of iterations which GA could run on a same error rate
    }
}
