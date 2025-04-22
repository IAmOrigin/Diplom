using _28._01ui.Properties;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _28._01ui
{
	public partial class TicketBuy : Page
	{
		Entities entities = new Entities();
		public TicketBuy()
		{
			InitializeComponent();
			var selectedEvent = entities.Events.Find(Convert.ToInt32(DataHolder.SharedEventId));
			var bunner = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg)),
				Stretch = Stretch.UniformToFill
			};
			imgBorder.Background = bunner;

			textblockeventname.Text = selectedEvent.EventName;
			locationTextBlock.Text = selectedEvent.Location;
			textblockDate.Text = selectedEvent.EventDate?.ToString("dd MMMM", new CultureInfo("ru-RU"));
			textblockTime.Text = selectedEvent.EventDate?.ToString("HH:mm");
			if (selectedEvent.Price == 0)
			{
				textblockPrice.Text = "";
				buyButton.Content = "Получить";
			}
			else
			{
				textblockPrice.Text = "К оплате " + selectedEvent.Price + " рублей";
			}
			
		}

		private void Btnbuy_Click(object sender, RoutedEventArgs e)
		{
			var selectedEvent = entities.Events.Find(Convert.ToInt32(DataHolder.SharedEventId));
			string pattern = @"^89\d{9}$"; // Формат 89xxxxxxxxx		
			if (textboxHolder.Text == "" || textboxPhone.Text == "")
			{
				Warn.SetTextWithDelay("Заполните контактные данные");
			}
			else
			{
				if (!Regex.IsMatch(textboxPhone.Text, pattern))
				{
					Warn.SetTextWithDelay("Номер телефона должен быть в формате 89XXXXXXXXX");
					textboxPhone.Focus();
					return;
				}
				var ticket = new Ticket();
				entities.Ticket.Add(ticket);
				ticket.Name = textboxHolder.Text;
				ticket.Phone = textboxPhone.Text;
				ticket.Price = selectedEvent.Price;
				ticket.IdEvent = selectedEvent.Id;
				ticket.BuyDate = DateTime.Now;
				ticket.IdUser = Settings.Default.loggedInUser;
				if (selectedEvent.Price != 0)
				{
					selectedEvent.TicketsRemain -= 1;
				}
				entities.SaveChanges();
				Manager.MainFrame.GoBack();
			}
		}

		private void textboxHolder_TextChanged(object sender, TextChangedEventArgs e)
		{
			holderTextBlock.Text = textboxHolder.Text;
		}

		private void textboxPhone_TextChanged(object sender, TextChangedEventArgs e)
		{
			phoneTextBloxk.Text = textboxPhone.Text;
		}
	}
}
