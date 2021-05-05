namespace ProjectDifficultyCalculator.Logic.COCOMO
{
    public struct CocomoProperties
    {
        public readonly double A;
        public readonly double B;
        public readonly FactorInfo[] ScaleFactors;
        public readonly FactorInfo[] CostDrivers;

        public CocomoProperties(double a, double b, FactorInfo[] scaleFactors, FactorInfo[] costDrivers)
        {
            A = a;
            B = b;
            ScaleFactors = scaleFactors;
            CostDrivers = costDrivers;
        }
    }
}
