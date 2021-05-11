using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDifficultyCalculator.Models
{
    public class LanguageSize
    {
        public string Language { get; set; }
        public uint LinesAmount { get; set; }
        public uint ChangeLines { get; set; }

        public LanguageSize(string language, uint linesAmount, uint changeLines = 0)
        {
            Language = language;
            LinesAmount = linesAmount;
            ChangeLines = changeLines;
        }
    }
}
