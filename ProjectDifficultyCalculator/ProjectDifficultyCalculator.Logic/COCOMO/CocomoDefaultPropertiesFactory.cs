namespace ProjectDifficultyCalculator.Logic.COCOMO
{
    public static class CocomoDefaultPropertiesFactory
    {
        public static CocomoProperties Create()
        {
            const double a = 2.94;
            const double b = 0.91;

            var scaleFactors = new[]
            {
                new FactorInfo<double>("PREC", "Precedentedness", 4.05, 3.24, 2.43, 1.62, 0.81, 0.0),
                new FactorInfo<double>("FLEX", "Development Flexibility", 6.07, 4.86, 3.64, 2.43, 1.21, 0.0),
                new FactorInfo<double>("RESL", "Architecture / Risk Resolution", 4.22, 3.38, 2.53, 1.69, 0.84, 0.0),
                new FactorInfo<double>("TEAM", "Team Cohesion", 4.94, 3.95, 2.97, 1.98, 0.99, 0.0),
                new FactorInfo<double>("PMAT", "Process Maturity", 4.54, 3.64, 2.73, 1.82, 0.91, 0.0)
            };

            var costDrivers = new[]
            {
                new FactorInfo<double>("RELY", "Required Software Reliability", 0.75, 0.88, 1.0, 1.15, 1.39, double.NaN),
                new FactorInfo<double>("DATA", "Data Base Size", double.NaN, 0.93, 1.0, 1.09, 1.19, double.NaN),
                new FactorInfo<double>("CPLX", "Product Complexity", 0.75, 0.88, 1.0, 1.15, 1.30, 1.66),
                new FactorInfo<double>("RUSE", "Developed for Reusability", double.NaN, 0.91, 1.0, 1.14, 1.29, 1.49),
                new FactorInfo<double>("DOCU", "Documentation Match to Lifecycle Needs", 0.89, 0.95, 1.0, 1.06, 1.13, double.NaN),
                new FactorInfo<double>("TIME", "Time Constraint", double.NaN, double.NaN, 1.0, 1.11, 1.31, 1.67),
                new FactorInfo<double>("STOR", "Storage Constraint", double.NaN, double.NaN, 1.0, 1.06, 1.21, 1.57),
                new FactorInfo<double>("PVOL", "Platform Volatility", double.NaN, 0.87, 1.0, 1.15, 1.30, double.NaN),
                new FactorInfo<double>("ACAP", "Analyst Capability", 1.50, 1.22, 1.0, 0.83, 0.67, double.NaN),
                new FactorInfo<double>("PCAP", "Programmer Capability", 1.37, 1.16, 1.0, 0.87, 0.74, double.NaN),
                new FactorInfo<double>("PCON", "Personnel Continuity", 1.24, 1.10, 1.0, 0.92, 0.84, double.NaN),
                new FactorInfo<double>("AEXP", "Application Experience", 1.22, 1.10, 1.0, 0.89, 0.81, double.NaN),
                new FactorInfo<double>("PEXP", "Platform Experience", 1.25, 1.12, 1.0, 0.88, 0.81, double.NaN),
                new FactorInfo<double>("LTEX", "Language and Toolset Experience", 1.22, 1.10, 1.0, 0.91, 0.84, double.NaN),
                new FactorInfo<double>("TOOL", "Use of Software Tools", 1.24, 1.12, 1.0, 0.86, 0.72, double.NaN),
                new FactorInfo<double>("SITE", "Multisite Development", 1.25, 1.10, 1.0, 0.92, 0.84, 0.78),
                new FactorInfo<double>("SCED", "Required Development Schedule", 1.29, 1.10, 1.0, 1.0, 1.0, double.NaN),

                new FactorInfo<double>("SECR", "Required Software Security", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("ERGO", "Required Software Ergonomics", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("TRSL", "Software Text Translation", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("TCAP", "Testers Capability", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("DCAP", "Documentalists Capability", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("SCAP", "Software Designers Capability", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("CCNG", "Cost Change Probability", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("RCON", "Resources Constraints", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("DEXT", "Divisions External Influence", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("MEXT", "Market External Influence", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("CEXT", "Customer External Influence", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
                new FactorInfo<double>("GEXT", "Government External Influence", double.NaN, double.NaN, 1.0, double.NaN, double.NaN, double.NaN),
            };

            return new CocomoProperties(a, b, scaleFactors, costDrivers);
        }
    }
}
