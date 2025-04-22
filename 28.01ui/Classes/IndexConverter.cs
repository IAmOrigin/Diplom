using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace _28._01ui
{
	public class IndexConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Получаем ListViewItem
			if (value is ListViewItem item)
			{
				// Получаем ItemsControl (ListView)
				var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
				if (listView != null)
				{
					// Получаем индекс элемента
					int index = listView.ItemContainerGenerator.IndexFromContainer(item);
					// Возвращаем индекс + 1 (чтобы начать с 1 вместо 0)
					return (index + 1).ToString();
				}
			}
			return "0"; // Если что-то пошло не так
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
