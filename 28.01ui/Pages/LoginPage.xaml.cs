using _28._01ui.Properties;
using System;
using System.Windows;
using System.Windows.Controls;

namespace _28._01ui
{
	public partial class LoginPage : Page
	{
		Entities entities = new Entities();
		public LoginPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
		}

		private void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(textboxLogin.Text) || string.IsNullOrEmpty(pasboxPas.Password))
				{
					Warn.SetTextWithDelay("Заполните все поля");
					return;
				}
				string Login = textboxLogin.Text;
				string Password = pasboxPas.Password;
				bool flag = false;
				foreach (var user in entities.Users)
				{
					if (Login == user.Login && Password == user.Password)
					{
						flag = true;
						Settings.Default.loggedInUser = user.Id;

						if (user.Role != null)
						{
							Settings.Default.loggedInUser = user.Id;
							Settings.Default.Save();
							Manager.MainFrame.Navigate(new StartPage());
						}
						else
						{
							Warn.SetTextWithDelay("Неизвестный пользователь");
						}
					}
				}
				if (!flag)
				{
					Warn.SetTextWithDelay("Неверный логин или пароль");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new RegistrPage());
		}
	}
}
