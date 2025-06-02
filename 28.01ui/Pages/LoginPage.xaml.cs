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
		private readonly Entities _entities = new Entities();
		private const string LoginFailedTitle = "Не удалось войти";

		public LoginPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
		}

		private void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (IsInputInvalid())
					return;

				var login = textboxLogin.Text;
				var user = FindUserByLogin(login);

				if (user == null)
				{
					ShowLoginError("Пользователь с таким логином не найден");
					return;
				}

				ProcessUserAuthentication(user);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private bool IsInputInvalid()
		{
			if (string.IsNullOrEmpty(textboxLogin.Text) ||
				string.IsNullOrEmpty(pasboxPas.Password))
			{
				ShowLoginError("Заполните все поля");
				return true;
			}
			return false;
		}

		private Users FindUserByLogin(string login)
		{
			return _entities.Users.FirstOrDefault(u => u.Login == login);
		}

		private void ProcessUserAuthentication(Users user)
		{
			if (!IsPasswordValid(user.Password))
			{
				ShowLoginError("Неверный пароль");
				return;
			}

			if (user.IdRole == 0)
			{
				ShowLoginError("Неизвестный пользователь");
				return;
			}

			CompleteSuccessfulLogin(user);
		}

		private bool IsPasswordValid(string storedPassword)
		{
			return PasswordHelper.VerifyPassword(pasboxPas.Password, storedPassword);
		}

		private void CompleteSuccessfulLogin(Users user)
		{
			Settings.Default.loggedInUser = user.Id;
			Settings.Default.Save();
			Manager.MainFrame.Navigate(new StartPage());
		}

		private void ShowLoginError(string message)
		{
			PopupManager.ShowMessage(message, LoginFailedTitle);
		}

		private void HandleException(Exception ex)
		{
			// В продакшн-коде лучше использовать логгер
			MessageBox.Show($"Произошла ошибка: {ex.Message}");
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new RegistrPage());
		}
	}
}