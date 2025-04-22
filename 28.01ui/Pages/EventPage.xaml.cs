using _28._01ui.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _28._01ui
{
	public partial class EventPage : Page
	{
		Entities entities = new Entities();
		public EventPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
			foreach (var entity in entities.Events)
			{
				EventContainer.Items.Add(entity);
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

        }

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{

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
