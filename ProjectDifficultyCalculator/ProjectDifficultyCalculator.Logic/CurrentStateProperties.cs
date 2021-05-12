using System.Collections.Generic;
using ProjectDifficultyCalculator.Logic.COCOMO;
using ProjectDifficultyCalculator.Logic.UFP;

namespace ProjectDifficultyCalculator.Logic
{
    public struct CurrentStateProperties
    {
        public uint Sloc;
        public double Brak;
        public int[] ScaleFactors;
        public int[] CostDrivers;
        public Dictionary<string, uint[][]> SlocFp;
        public Dictionary<string, uint[][]> BrakFp;
        public UfpProperties UfpProperties;
        public CocomoProperties CocomoProperties;

        public CurrentStateProperties(uint sloc, double brak, int[] scaleFactors, int[] costDrivers, Dictionary<string, uint[][]> slocFp, Dictionary<string, uint[][]> brakFp, UfpProperties ufpProperties, CocomoProperties cocomoProperties)
        {
            Sloc = sloc;
            Brak = brak;
            ScaleFactors = scaleFactors;
            CostDrivers = costDrivers;
            SlocFp = slocFp;
            BrakFp = brakFp;
            UfpProperties = ufpProperties;
            CocomoProperties = cocomoProperties;
        }

        public void Deconstruct(out uint sloc, out double brak, out int[] scaleFactors, out int[] costDrivers, out Dictionary<string, uint[][]> slocFp, out Dictionary<string, uint[][]> brakFp, out UfpProperties ufpProperties, out CocomoProperties cocomoProperties)
        {
            sloc = Sloc;
            brak = Brak;
            scaleFactors = ScaleFactors;
            costDrivers = CostDrivers;
            slocFp = SlocFp;
            brakFp = BrakFp;
            ufpProperties = UfpProperties;
            cocomoProperties = CocomoProperties;
        }
    }
}