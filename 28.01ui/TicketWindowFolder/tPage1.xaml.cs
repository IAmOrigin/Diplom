using _28._01ui.Classes;
using _28._01ui.Properties;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace _28._01ui.TicketWindowFolder
{
    /// <summary>
    /// Логика взаимодействия для tPage1.xaml
    /// </summary>
    public partial class tPage1 : Page
    {
		Entities entities = new Entities();
        public tPage1()
        {
            InitializeComponent();
			var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
			textBlockPrice.Text = "К оплате " + selectedEvent.Price + " рублей";
        }

		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
			var window = Window.GetWindow(this);
			window?.Close();
		}

		private async void Button_Next(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(textBoxName.Text) ||
					String.IsNullOrEmpty(textBoxPhone.Text) ||
					String.IsNullOrEmpty(textBoxEmail.Text))
			{
				PopupManager.ShowMessage("Заполните все поля");
				return;
			}
			Regex fioRegex = new Regex(@"^[А-ЯЁа-яё-]+\s[А-ЯЁа-яё-]+(?:\s[А-ЯЁа-яё-]+)?$");
			Regex phoneRegex = new Regex(@"^(\+7|8)[\s-]?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$");
			Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
			if (!fioRegex.IsMatch(textBoxName.Text))
			{
				PopupManager.ShowMessage("Заполните поле ФИО");
				return;
			}
			if (!phoneRegex.IsMatch(textBoxPhone.Text))
			{
				PopupManager.ShowMessage("Номер телефона должен быть в формате: +7 (XXX) XXX-XX-XX или 8XXXXXXXXXX");
				return;
			}
			if (!emailRegex.IsMatch(textBoxEmail.Text))
			{
				PopupManager.ShowMessage("Недопустимый адрес электронной почты");
				return;
			}
			var ticket = new Ticket();
			var ticketEvent = entities.Events.Find(DataHolder.SharedEventId);
			ticket.IdEvent = DataHolder.SharedEventId;
			ticket.IdUser = Settings.Default.loggedInUser;
			ticket.Price = ticketEvent.Price;
			ticket.Name = textBoxName.Text;
			ticket.Phone = NormalizePhoneNumber(textBoxPhone.Text);
			ticket.BuyDate = DateTime.Now;
			try
			{
				ConfirmCode.GenerateConfirmationCode();
				string code = ConfirmCode.LastGeneratedCode;
				string email = textBoxEmail.Text;
				await ConfirmCode.SendConfirmationEmail(email, code);
				Manager.TicketFrame.Navigate(new tPage2(ticket));
			}
			catch 
			{
				PopupManager.ShowMessage("Не удалось отправить сообщение на электронную почту. Пожалуйста попробуйте позже.");
				var window = Window.GetWindow(this);
				window?.Close();
			}
			
		}
		private string NormalizePhoneNumber(string phone)
		{
			string digitsOnly = Regex.Replace(phone, @"[^\d]", "");
			if (digitsOnly.StartsWith("7") && digitsOnly.Length == 11)
			{
				digitsOnly = "8" + digitsOnly.Substring(1);
			}
			else if (digitsOnly.Length < 11)
			{
				PopupManager.ShowMessage("Некорректная длина номера");
			}
			return digitsOnly;
		}


	}
}
