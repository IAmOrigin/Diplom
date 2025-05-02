using _28._01ui.Classes;
using _28._01ui.Properties;
using System;
using System.Linq;
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
					PopupManager.ShowMessage("Заполните все поля", "Не удалось войти");
					return;
				}

				string Login = textboxLogin.Text;
				string Password = pasboxPas.Password;

				var user = entities.Users.FirstOrDefault(u => u.Login == Login);

				if (user == null)
				{
					PopupManager.ShowMessage("Пользователь с таким логином не найден", "Не удалось войти");
				}
				else
				{
					bool isPasswordValid = PasswordHelper.VerifyPassword(Password, user.Password);

					if (!isPasswordValid)
					{
						PopupManager.ShowMessage("Неверный пароль", "Не удалось войти");
					}
					else
					{
						if (user.IdRole != 0)
						{
							Settings.Default.loggedInUser = user.Id;
							Settings.Default.Save();
							Manager.MainFrame.Navigate(new StartPage());
						}
						else
						{
							PopupManager.ShowMessage("Неизвестный пользователь", "Не удалось войти");
						}
					}
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