namespace ProjectDifficultyCalculator.Logic.COCOMO
{
    public struct CocomoProperties
    {
        public readonly double A;
        public readonly double B;
        public readonly FactorInfo<double>[] ScaleFactors;
        public readonly FactorInfo<double>[] CostDrivers;

        public CocomoProperties(double a, double b,
            FactorInfo<double>[] scaleFactors, FactorInfo<double>[] costDrivers)
        {
            A = a;
            B = b;
            ScaleFactors = scaleFactors;
            CostDrivers = costDrivers;
        }
    }
}
