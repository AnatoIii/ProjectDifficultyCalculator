using System;
using System.Linq;

namespace ProjectDifficultyCalculator.Logic.COCOMO
{
    public class CocomoCalculator
    {
        public readonly CocomoProperties Properties;

        public CocomoCalculator(CocomoProperties properties)
        {
            Properties = properties;
        }

        public double CalculateScaleCoefficientE(params double[] scaleFactors)
        {
            return Properties.B + scaleFactors.Aggregate(0.01, (current, sf) => current * sf);
        }

        public double CalculateCorrectionFactorM(params double[] costDrivers)
        {
            return costDrivers.Aggregate((current, driver) => current * driver);
        }

        public double CalculateRequirementsChangeCoefficientK(double percentage)
        {
            return 1.0 + percentage / 100.0;
        }

        public double CalculateComplexityPM(uint size, double reqPercentage, double[] scaleFactors, double[] costDrivers)
        {
            var k = CalculateRequirementsChangeCoefficientK(reqPercentage);
            var e = CalculateScaleCoefficientE(scaleFactors);
            var m = CalculateCorrectionFactorM(costDrivers);
            return Properties.A * k * Math.Pow(size, e) * m;
        }
    }
}
