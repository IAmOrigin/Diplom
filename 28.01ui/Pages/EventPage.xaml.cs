using _28._01ui.EditorWindows;
using _28._01ui.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace _28._01ui
{
	public partial class EventPage : Page
	{
		Entities entities = new Entities();
		public EventPage()
		{
			InitializeComponent();
			AdminCheck();
			LoadEvents(EventContainer);
			LoadArchive(ArchiveContainer);
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
		}

		private void LoadEvents(ItemsControl itemsControl)
		{
			itemsControl.Items.Clear();
			foreach (var entity in entities.Events)
			{
				if (entity.EventDate >= DateTime.Now)
				{
					itemsControl.Items.Add(entity);
				}
			}
		}
		private void LoadArchive(ItemsControl itemsControl)
		{
			itemsControl.Items.Clear();
			foreach (var entity in entities.Events)
			{
				if (entity.EventDate < DateTime.Now)
				{
					itemsControl.Items.Add(entity);
				}
			}
		}

		private void AdminCheck()
		{
			if (DataHolder.SharedRoleId != 1)
			{
				btnAdd.Visibility = Visibility.Collapsed;
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedEventId = 0;
			EventEditorWindow window = new EventEditorWindow();
			window.Closed += Editor_Closed;
			Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.ShowDialog();
        }

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchingText = SearchBox.Text.ToLower();
			EventContainer.Items.Clear();
			ArchiveContainer.Items.Clear();
			if (searchingText != "")
			{
				var filteredEvents = entities.Events
					.Where(s => s.EventName.ToLower().Contains(searchingText) ||
							   s.Location.ToLower().Contains(searchingText) ||
							   s.EventType.TypeName.ToLower().Contains(searchingText))
					.Where(s => s.EventDate >= DateTime.Now)
					.Distinct()
					.ToList();

				foreach (var eventItem in filteredEvents)
				{
					EventContainer.Items.Add(eventItem);
				}
				var filteredArchive = entities.Events
					.Where(s => s.EventName.ToLower().Contains(searchingText) ||
							   s.Location.ToLower().Contains(searchingText) ||
							   s.EventType.TypeName.ToLower().Contains(searchingText))
					.Where(s => s.EventDate < DateTime.Now)
					.Distinct()
					.ToList();

				foreach (var eventItem in filteredArchive)
				{
					ArchiveContainer.Items.Add(eventItem);
				}
			}
			else
			{
				ArchiveContainer.Items.Clear();
				LoadEvents(ArchiveContainer);
				EventContainer.Items.Clear();
				LoadEvents(EventContainer);
			}
		}
		private void EventClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var event1 = button.DataContext as Events;
			if (event1 == null) return;
			DataHolder.SharedEventId = event1.Id;
			Manager.MainFrame.Navigate(new Uri("Pages/EventViewer.xaml", UriKind.Relative));
		}
		private void Editor_Closed(object sender, EventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
			EventContainer.Items.Clear();
			ArchiveContainer.Items.Clear();
			LoadArchive(ArchiveContainer);
			LoadEvents(EventContainer);

		}
	}
}
