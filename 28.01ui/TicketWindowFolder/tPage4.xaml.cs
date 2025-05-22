using _28._01ui.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace _28._01ui.TicketWindowFolder
{
    public partial class tPage4 : Page
    {
		Entities entities = new Entities();
		DispatcherTimer timer = new DispatcherTimer();
		public tPage4()
        {
            InitializeComponent();
			var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
			textBlockDateTime.Text = "Ждем Вас на мероприятии " + selectedEvent.EventDate.Value.ToString("d MMMM") + " в " + selectedEvent.EventDate.Value.ToString("HH:mm");
			PopupManager.ShowMessage("Билет добавлен в ваш профиль");
			timer.Interval = TimeSpan.FromSeconds(7);
			timer.Tick += (s, e) =>
			{
				timer.Stop();
				var window = Window.GetWindow(this);
				window.Close();
			};
			timer.Start();
		}

		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			var window = Window.GetWindow(this);
			window.Close();
		}
    }
}
