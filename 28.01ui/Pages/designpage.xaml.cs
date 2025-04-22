using System;
using System.Windows;
using System.Windows.Controls;


namespace _28._01ui
{
    public partial class designpage : Page
    {
		Entities entities = new Entities();
		public designpage()
		{
			InitializeComponent();

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
			MessageBox.Show(event1.Id.ToString());
			DataHolder.SharedEventId = event1.Id;
			Manager.MainFrame.Navigate(new Uri("EventEditor.xaml", UriKind.Relative));
		}
	}
}
