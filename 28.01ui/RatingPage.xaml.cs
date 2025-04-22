using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace _28._01ui
{
	public partial class RatingPage : Page
	{
		Entities entities = new Entities();
		public RatingPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
			addResultButton.Visibility = Visibility.Collapsed;
			try
			{
				cboxevents.SelectedItem = entities.Events.Find(DataHolder.SharedResultEventId);
				if (cboxevents.SelectedItem != null)
				{
					selectHelper.Text = "";
				} 
			}
			catch { }
			foreach (var entity in entities.Events)
			{
				if (entity.EventDate < DateTime.Now)
				{
					cboxevents.Items.Add(entity);
				}
			}
		}

		private ObservableCollection<Results> _sortedResults;
		private void cboxevents_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectHelper.Text = "";
			if (DataHolder.SharedRole == "Admin")
			{
				addResultButton.Visibility = Visibility.Visible;
			}
			var selectedEvent = cboxevents.SelectedItem as Events;
			// Получаем данные из базы данных без использования неподдерживаемых методов
			var results = entities.Results
				.Where(entity => selectedEvent.Id == entity.EventId)
				.ToList(); // Материализуем запрос в память
			// Выполняем сортировку в памяти с помощью LINQ to Objects
			_sortedResults = new ObservableCollection<Results>(
				results
					.OrderByDescending(entity =>
						entity.Points.HasValue && entity.Points != 0 && entity.Time.HasValue // Если есть и очки, и время
							? (long)entity.Points // Приводим к long для сортировки
							: (entity.Points.HasValue && entity.Points != 0) // Если есть только очки
								? (long)entity.Points // Приводим к long для сортировки
								: (entity.Time.HasValue) // Если есть только время
									? -entity.Time.Value.Ticks // Сортировка по времени (чем меньше время, тем выше)
									: long.MinValue) // Если нет ни очков, ни времени
					.ThenBy(entity => entity.Time) // Дополнительная сортировка по времени, если очки равны
					.ThenBy(entity =>
						!entity.Points.HasValue && !entity.Time.HasValue ? 1 : 0) // Элементы с Points = null и Time = null в конце
			);
			var viewSource = new CollectionViewSource { Source = _sortedResults };
			listviewrating.ItemsSource = viewSource.View;
		}
		private void addResultButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedEvent = cboxevents.SelectedItem as Events;
			DataHolder.SharedResultId = 0;
			DataHolder.SharedResultEventId = selectedEvent.Id;
			Manager.MainFrame.Navigate(new Uri("RatingEditor.xaml", UriKind.Relative));
		}
		private void editResultClick(object sender, RoutedEventArgs e)
		{
			if (DataHolder.SharedRole == "Admin")
			{
				var selectedEvent = cboxevents.SelectedItem as Events;
				var button = sender as Button;
				var result = button.DataContext as Results;
				if (result == null) return;
				listviewrating.SelectedItem = result;
				DataHolder.SharedResultId = result.Id;
				DataHolder.SharedResultEventId = selectedEvent.Id;
				Manager.MainFrame.Navigate(new Uri("RatingEditor.xaml", UriKind.Relative));
			}
		}
	}
}
