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
			Warn.Text = "";
			foreach (var events in entities.Events)
			{
				if (events.EventDate >= DateTime.Now)
				{
					LViewEvents.Items.Add(events);
				}
			}
			if (DataHolder.SharedRole != "Admin")
			{
				BtnNewName.Visibility = Visibility.Collapsed;
			}
		}

		private void LViewEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedevent = LViewEvents.SelectedItem as Events;
			if (selectedevent != null)
			{
				selectEvent.Text = "";
				eventName.Text = "Название мероприятия: " + selectedevent.EventName;
				eventDate.Text = "Дата проведения: " + selectedevent.EventDate.ToString();
				eventType.Text = "Дисциплина: " + selectedevent.EventType.ToString();
				eventLocation.Text = "Место проведения: " + selectedevent.Location;
				eventDesc.Text = selectedevent.EventDesc;
				if (selectedevent.EventImg == null)
				{
					eventImg.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"\\images\\default.jpg"));
				}
				else
				{
					try
					{
						eventImg.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedevent.EventImg));
					}
					catch
					{
						Warn.SetTextWithDelay("Не удалось открыть изображение");
					}
				}
			}
		}
		private void BtnEdit(object sender, RoutedEventArgs e)
		{
			if (DataHolder.SharedRole == "Admin")
			{
				var button = sender as System.Windows.Controls.Button;
			// Получаем DataContext кнопки (это объект, связанный с текущим элементом ListView)
			var selectedEvent = button.DataContext as Events;
			if (selectedEvent == null) return;
			// Выделяем элемент в ListView
			LViewEvents.SelectedItem = selectedEvent;
			//var idEvent = LViewEvents.SelectedItem as Events;
			DataHolder.SharedEventId = selectedEvent.Id;
			Manager.MainFrame.Navigate(new Uri("EventEditor.xaml", UriKind.Relative));
			}

			else
			{
				Warn.SetTextWithDelay("Необходимо быть администратором");
			}
		}

		private void BtnTicket(object sender, RoutedEventArgs e)
		{
			if (Settings.Default.loggedInUser == 0)
			{
				Warn.SetTextWithDelay("Необходимо авторизоваться");
			}
			else
			{
				var button = sender as System.Windows.Controls.Button;
				var selectedEvent = button.DataContext as Events;
				if (selectedEvent == null) return;
				LViewEvents.SelectedItem = selectedEvent;
				DataHolder.SharedEventId = selectedEvent.Id;
				Manager.MainFrame.Navigate(new Uri("TicketBuy.xaml", UriKind.Relative));
			}
		}

		private void BtnNew(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedEventId = 0;
			Manager.MainFrame.Navigate(new Uri("EventEditor.xaml", UriKind.Relative));
		}
		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchingText = SearchBox.Text.ToLower();
			LViewEvents.Items.Clear();
			var filtername = entities.Events.Where(s => s.EventName.ToLower() .Contains(searchingText)).ToList();
			var filterlocation = entities.Events.Where(s => s.Location.ToLower().Contains(searchingText)).ToList();
			var filtertype = entities.Events.Where(s => s.EventType.TypeName.ToLower().Contains(searchingText)).ToList();
			if(searchingText != "")
			{
				foreach (var filterItem in filtername)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						LViewEvents.Items.Add(filterItem);
					}
				}
				foreach (var filterItem in filtertype)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						LViewEvents.Items.Add(filterItem);
					}
				}
				foreach (var filterItem in filterlocation)
				{
					if (filterItem.EventDate >= DateTime.Now)
					{
						LViewEvents.Items.Add(filterItem);
					}
				}
			}
			else
			{
				LViewEvents.Items.Clear();
				foreach (var events in entities.Events)
				{
					if (events.EventDate >= DateTime.Now)
					{
						LViewEvents.Items.Add(events);
					}
				}
			}
			
		}

		private void BtnBack(object sender, RoutedEventArgs e)
		{
			try
			{
				Manager.MainFrame.GoBack();
			}
			catch
			{
				Warn.SetTextWithDelay("Невозможно вернуться, используйте боковое меню");
			}
		}
	}
}
