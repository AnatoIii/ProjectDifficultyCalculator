using System;

namespace ProjectDifficultyCalculator.Logic
{
    public struct IntervalsTable<TRowInterval, TColumnInterval, TCell>
        where TRowInterval : IComparable<TRowInterval>
        where TColumnInterval : IComparable<TColumnInterval>
    {
        public readonly Interval<TRowInterval>[] RowsIntervals;
        public readonly Interval<TColumnInterval>[] ColumnsIntervals;
        public readonly TCell[,] Cells;

        public IntervalsTable(Interval<TRowInterval>[] rowsIntervals,
            Interval<TColumnInterval>[] columnsIntervals, TCell[,] cells)
        {
            if(cells.GetLength(0) != rowsIntervals.Length)
                throw new ArgumentException("Wrong table rows count.");
            if(cells.GetLength(1) != columnsIntervals.Length)
                throw new ArgumentException("Wrong table columns count.");

            RowsIntervals = rowsIntervals;
            ColumnsIntervals = columnsIntervals;
            Cells = cells;
        }
    }
}
