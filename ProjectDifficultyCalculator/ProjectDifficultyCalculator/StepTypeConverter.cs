using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProjectDifficultyCalculator
{
	class StepTypeConverter : IValueConverter
	{
		public StepTypeConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is CurrentStep state))
			{
				throw new ArgumentException("Incorrect enum instance");
			}

			return (state, parameter as string) switch
			{
				(CurrentStep.CostDriverCalculation, "costDriver") => CurrentStep.CostDriverCalculation,
				(CurrentStep.ScaleFactorsCalculation, "scaleFactors") => CurrentStep.ScaleFactorsCalculation,
				(CurrentStep.SizeCalculation, "size") => CurrentStep.SizeCalculation,
				(CurrentStep.Results, "results") => CurrentStep.Results,
				_ => Visibility.Hidden
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
