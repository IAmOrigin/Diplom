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
			LoadEvents();
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
		}

		private void LoadEvents()
		{
			foreach (var entity in entities.Events)
			{
				if (entity.EventDate >= DateTime.Now)
				{
					EventContainer.Items.Add(entity);
				}
			}
		}

		private void AdminCheck()
		{
			if (DataHolder.SharedRole != "Admin")
			{
				btnAdd.Visibility = Visibility.Collapsed;
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedEventId = 0;
			Manager.MainFrame.Navigate(new Uri("Pages/EventEditor.xaml", UriKind.Relative));
        }

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchingText = SearchBox.Text.ToLower();
			EventContainer.Items.Clear();
			var filtername = entities.Events.Where(s => s.EventName.ToLower().Contains(searchingText)).ToList();
			var filterlocation = entities.Events.Where(s => s.Location.ToLower().Contains(searchingText)).ToList();
			var filtertype = entities.Events.Where(s => s.EventType.TypeName.ToLower().Contains(searchingText)).ToList();
			if (searchingText != "")
			{
				foreach (var filterItem in filtername)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						EventContainer.Items.Add(filterItem);
					}
				}
				foreach (var filterItem in filtertype)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						EventContainer.Items.Add(filterItem);
					}
				}
				foreach (var filterItem in filterlocation)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						EventContainer.Items.Add(filterItem);
					}
				}
			}
			else
			{
				EventContainer.Items.Clear();
				LoadEvents();
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
	}
}
