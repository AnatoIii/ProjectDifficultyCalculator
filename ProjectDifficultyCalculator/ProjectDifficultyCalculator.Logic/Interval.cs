using System;

namespace ProjectDifficultyCalculator.Logic
{
    public struct Interval<T> where T : IComparable<T>
    {
        public readonly T From;
        public readonly T To;
        public readonly bool Including;

        public Interval(T from, T to, bool including = true)
        {
            if(from.CompareTo(to) > 0) throw new ArgumentException("Wrong interval endpoints.");
            From = from;
            To = to;
            Including = including;
        }

        public bool Contains(T value)
        {
            if (Including) return From.CompareTo(value) <= 0 && To.CompareTo(value) >= 0;
            return From.CompareTo(value) < 0 && To.CompareTo(value) > 0;
        }

        public override string ToString()
        {
            char firstSymbol;
            char lastSymbol;
            if (Including)
            {
                firstSymbol = '[';
                lastSymbol = ']';
            }
            else
            {
                firstSymbol = '(';
                lastSymbol = ')';
            }

            if (From.CompareTo(To) == 0) return firstSymbol + From.ToString() + lastSymbol;
            return firstSymbol + From.ToString() + ';' + To.ToString() + lastSymbol;
        }
    }
}
