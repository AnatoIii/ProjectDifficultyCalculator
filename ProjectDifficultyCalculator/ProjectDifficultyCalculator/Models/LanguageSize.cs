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

        public int LinesAmount { get; set; }

        public LanguageSize(string language, int linesAmount)
        {
            Language = language;
            LinesAmount = linesAmount;
        }
    }
}
