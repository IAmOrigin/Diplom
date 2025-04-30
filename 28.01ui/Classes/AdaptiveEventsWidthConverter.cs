using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _28._01ui.Classes
{
	public class AdaptiveEventsWidth : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double width)
			{
				double divisor = 1; // По умолчанию делим на 1 (если <= 600)

				if (width > 650 && width <= 1000)
					divisor = 2;    // От 600 до 1000 → делим на 2
				else if (width > 1000)
					divisor = 3;    // Больше 1000 → делим на 3

				return Math.Max(0, (width / divisor) - 20); // (width/divisor) - 20, но не меньше 0
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
