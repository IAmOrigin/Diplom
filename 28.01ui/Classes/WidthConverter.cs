using System;
using System.Globalization;
using System.Windows.Data;

namespace _28._01ui.Classes
{
	public class SubtractWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double width)
			{
				return Math.Max(0, width - 40); // Гарантирует, что ширина не станет отрицательной
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
