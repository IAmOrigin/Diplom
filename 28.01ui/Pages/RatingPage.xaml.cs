using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using _28._01ui.EditorWindows;

namespace _28._01ui
{
	public partial class RatingPage : Page
	{
		Entities entities = new Entities();
		public RatingPage()
		{
			InitializeComponent();
			ratingBorder.Visibility = Visibility.Collapsed;
			animGrid.InitSlideUp();
			foreach (var item in entities.Events)
			{
				if (item.EventDate < DateTime.Now)
				{
					eventButtonsContainer.Items.Add(item);
				}
			}
		}


		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			RatingEditorWindow window = new RatingEditorWindow();
			window.Closed += Editor_Closed;
			Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.ShowDialog();
		}

		private void event_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var selectedevent = button.DataContext as Events;
			dataGridResults.Items.Clear();
			var sortedResults = entities.Results
				.Where(i => i.EventId == selectedevent.Id)
				.Where(r => r.Points.HasValue || r.Time.HasValue)
				.OrderByDescending(r => r.Points.HasValue)
				.ThenByDescending(r => r.Points)
				.ThenBy(r => r.Time)
				.ToList();
			if (!sortedResults.Any())
			{
				textBlockInfo.Text = "У этого события пока нет результатов";
				ratingBorder.Visibility = Visibility.Collapsed;
				return;
			}
			else
			{
				ratingBorder.Visibility = Visibility.Visible;
				textBlockInfo.Text = "";
			}
				int position = 1;
			foreach (var item1 in sortedResults)
			{
				var itemWithPosition = new
				{
					Position = position++,
					Pilots = item1.Pilots,
					Time = item1.Time,
					Points = item1.Points
				};
				dataGridResults.Items.Add(itemWithPosition);
			}
		}
		private void Editor_Closed(object sender, EventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
		}
	}
}
