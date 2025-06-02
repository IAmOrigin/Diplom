using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _28._01ui.Classes
{
    class DivBy5Mul3Converter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double width)
			{
				double mul = 3;
				double sub = 20;
				if (width < 900)
				{
					mul = 5;	
				}
				return Math.Max(0, (width / 5 * mul)-sub);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
