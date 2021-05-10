using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectDifficultyCalculator
{
    class FontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is CurrentStep state))
			{
				throw new ArgumentException("Incorrect enum instance");
			}

			return (state, parameter as string) switch
			{
				(CurrentStep.CostDriverCalculation, "costDriver") => FontWeights.Bold,
				(CurrentStep.ScaleFactorsCalculation, "scaleFactors") => FontWeights.Bold,
				(CurrentStep.SizeCalculation, "size") => FontWeights.Bold,
				(CurrentStep.Results, "results") => FontWeights.Bold,
				_ => FontWeights.Thin
			};
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
