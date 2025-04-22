using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace _28._01ui
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isVisible && isVisible)
			{
				return Visibility.Visible; // Кнопка видима
			}
			return Visibility.Collapsed; // Кнопка скрыта
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
