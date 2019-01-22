namespace _9_ParticleSwarmOptimisation
{
    public static class Config
    {
        public static int SwarmSize = 5;
        public static int NumberOfIterations = 100;
        
        public static double MinX1 = -5;
        public static double MaxX1 = 10;
        public static double MinX2 = 0;
        public static double MaxX2 = 15;

        public static double WInertiaWeight = 0.729;
        public static double C1CognitiveLocalWeight = 1.49445;  // Personal acceleration coefficient
        public static double C2SocialGlobalWeight = 1.49445;    // Social acceleration coefficient 
    }
}