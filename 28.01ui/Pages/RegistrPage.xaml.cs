using _28._01ui.Properties;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _28._01ui
{
	public partial class RegistrPage : Page
	{
		Entities entities = new Entities();
		public RegistrPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
		}

		private void registrBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(textboxLogin.Text) || string.IsNullOrEmpty(pasboxPas.Password) ||
					string.IsNullOrEmpty(pasboxPas2.Password) || string.IsNullOrEmpty(textboxName.Text))
				{
					Warn.SetTextWithDelay("Заполните поля");
					return;
				}
				string Login = textboxLogin.Text;
				string Password = pasboxPas.Password;
				string Password2 = pasboxPas2.Password;
				string Name = textboxName.Text;
				if (entities.Users.Any(u => u.Login == Login))
				{
					Warn.SetTextWithDelay("Пользователь с таким логином уже существует");
					return;
				}
				if (Password != Password2)
				{
					Warn.SetTextWithDelay("Пароли не совпадают");
					return;
				}
				var newUser = new Users
				{
					Login = Login,
					Password = Password,
					Role = "User",
					Name = Name
				};
				entities.Users.Add(newUser);
				entities.SaveChanges();
				Settings.Default.loggedInUser = newUser.Id;
				Manager.MainFrame.Navigate(new StartPage());
			}
			catch (Exception ex)
			{
				Warn.SetTextWithDelay(ex.ToString());
			}
		}
    }
}
