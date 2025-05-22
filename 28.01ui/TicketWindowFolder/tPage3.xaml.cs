using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using _28._01ui.Classes;
using MailKit.Net.Smtp;
using MimeKit;

namespace _28._01ui.TicketWindowFolder
{
    public partial class tPage3 : Page
    {
		Entities entities = new Entities();
		Ticket ticket;
		DispatcherTimer timer = new DispatcherTimer();
		TimeSpan timeLeft = TimeSpan.FromMinutes(3);
		bool timeOut = false;
		public tPage3(Ticket ticket1)
        {
            InitializeComponent();
			ticket = ticket1;
			InitializeTimer();
		}

		private void InitializeTimer()
		{
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			timeLeft = timeLeft.Add(TimeSpan.FromSeconds(-1));
			textBlockTimer.Text = timeLeft.ToString(@"m\:ss");
			if (timeLeft <= TimeSpan.Zero)
			{
				timer.Stop();
				textBlockTimer.Text = "0:00";
				ConfirmCode.LastGeneratedCode = null;
				timeOut = true;
				textBlockTimer.Visibility = Visibility.Collapsed;
				newCodeButton.Visibility = Visibility.Visible;
			}
		}

		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			window?.Close();
		}

		private void Button_Next(object sender, RoutedEventArgs e)
		{
			if (timeOut == true)
			{
				PopupManager.ShowMessage("Время действия кода подтверждения вышло.");
				return;
			}
			if (textBoxCode.Text != ConfirmCode.LastGeneratedCode)
			{
				PopupManager.ShowMessage("Неверный код подтверждения.");
				return;
			}
			var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
			selectedEvent.TicketsRemain -= 1;
			entities.Ticket.Add(ticket);
			entities.SaveChanges();
			Manager.TicketFrame.Navigate(new tPage4());
		}

		private void Button_NoCode(object sender, RoutedEventArgs e)
		{
			PopupManager.ShowMessage("Ваш код подтверждения: " + ConfirmCode.LastGeneratedCode);
		}

		private async void Button_NewCode(object sender, RoutedEventArgs e)
		{
			newCodeButton.Visibility = Visibility.Collapsed;
			textBlockTimer.Visibility = Visibility.Visible;
			textBlockTimer.Text = "3:00";
			timeOut = false;
			timeLeft = TimeSpan.FromMinutes(3);
			timer.Start();
			try
			{
				ConfirmCode.GenerateConfirmationCode();
				await ConfirmCode.SendConfirmationEmail("krupin_2099@mail.ru", ConfirmCode.LastGeneratedCode);
			}
			catch
			{
				PopupManager.ShowMessage("Не удалось отправить сообщение на электронную почту. Пожалуйста попробуйте позже.");
				var window = Window.GetWindow(this);
				window?.Close();
			}
		}
	}
}
