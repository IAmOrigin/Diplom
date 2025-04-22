using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _28._01ui
{
	public partial class ArchivePage : Page
	{
		Entities entities = new Entities();
		public ArchivePage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			Warn.Text = "";
			foreach (var events in entities.Events)
			{
				if (events.EventDate < DateTime.Now)
				{
					LViewEvents.Items.Add(events);
				}
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
				eventType.Text = "Вид автоспорта: " + selectedevent.EventType.ToString();
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

		private void BtnDelete(object sender, RoutedEventArgs e)
		{
			if (DataHolder.SharedRole == "Admin")
			{
				var button = sender as Wpf.Ui.Controls.Button;
				// Получаем DataContext кнопки (это объект, связанный с текущим элементом ListView)
				var deleteEvent = button.DataContext as Events;
				if (deleteEvent == null) return;
				// Выделяем элемент в ListView
				LViewEvents.SelectedItem = deleteEvent;
				var result = MessageBox.Show("Вы точно хотите удалить это?", "Удаление",
				MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.No)
					return;
				foreach (var ticket in entities.Ticket)
				{
					if(ticket.IdEvent == deleteEvent.Id)
					{
						entities.Ticket.Remove(ticket);
					}
				}
				foreach (var result1 in entities.Results)
				{
					if (result1.EventId == deleteEvent.Id)
					{
						entities.Results.Remove(result1);
					}
				}
				LViewEvents.Items.Remove(deleteEvent);
				LViewEvents.Items.Refresh();
				entities.Events.Remove(deleteEvent);
				entities.SaveChanges();
				eventName.Text = "";
				eventDate.Text = "";
				eventType.Text = "";
				eventLocation.Text = "";
				eventDesc.Text = "";
				eventImg.Source = null;
				Warn.SetTextWithDelay("Удалено");
			}
			else
			{
				Warn.SetTextWithDelay("Необходимо быть администратором");
			}
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchingText = SearchBox.Text.ToLower();
			LViewEvents.Items.Clear();
			var filtername = entities.Events.Where(s => s.EventName.ToLower().Contains(searchingText)).ToList();
			var filterlocation = entities.Events.Where(s => s.Location.ToLower().Contains(searchingText)).ToList();
			var filtertype = entities.Events.Where(s => s.EventType.TypeName.ToLower().Contains(searchingText)).ToList();
			if (searchingText != "")
			{
				foreach (var filterItem in filtername)
				{
					if (filterItem.EventDate < DateTime.Now)
					{
						LViewEvents.Items.Add(filterItem);
					}
				}
				foreach (var filterItem in filtertype)
				{
					if (filterItem.EventDate < DateTime.Now)
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
					if (events.EventDate < DateTime.Now)
					{
						LViewEvents.Items.Add(events);
					}
				}
			}
			
		}

		private void BtnBack(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedResultEventId = 0;
			Manager.MainFrame.GoBack();
		}

		private void results(object sender, RoutedEventArgs e)
		{
			var button = sender as Wpf.Ui.Controls.Button;
			var selectedEvent = button.DataContext as Events;
			if (selectedEvent == null) return;
			LViewEvents.SelectedItem = selectedEvent;
			DataHolder.SharedResultEventId = selectedEvent.Id;
			Manager.MainFrame.Navigate(new RatingPage());
		}
	}
}