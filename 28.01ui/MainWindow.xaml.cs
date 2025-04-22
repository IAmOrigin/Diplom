using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using _28._01ui.Properties;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace _28._01ui
{
	public partial class MainWindow : FluentWindow
	{
		private bool _isDarkTheme = false;
		Entities entities = new Entities(); 
        public MainWindow()
		{
			InitializeComponent();
			Pic.defaultpic = @"images\default.jpg";
			Pic.defaultprofilepic = @"images\defaultprofilepic.jpg";
			Manager.MainFrame = MainFrame;
			Manager.MainFrame.Navigated += OnNavigated;
			Manager.MainFrame.Navigate(new Uri("StartPage.xaml", UriKind.Relative));
		}

		private void OnNavigated(object sender, NavigationEventArgs e)
		{
			var page = MainFrame.Content as Page;

			if (page is StartPage)
			{
				buttonback.Visibility = Visibility.Collapsed;
			}
			else
			{
				buttonback.Visibility = Visibility.Visible;
			}

			if (Settings.Default.loggedInUser != 0)
			{
				var entity = entities.Users.Find(Settings.Default.loggedInUser);
				DataHolder.SharedRole = entity.Role;
				profilebtn.Visibility = Visibility.Visible;
				loginbtn.Visibility = Visibility.Collapsed;
			}
			else
			{
				DataHolder.SharedRole = null;
				profilebtn.Visibility = Visibility.Collapsed;
				loginbtn.Visibility = Visibility.Visible;
			}
		}

		private void HomeClick(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("designpage.xaml", UriKind.Relative));
		}

		private void ProfileClick(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("ProfilePage.xaml", UriKind.Relative));
		}

		private void LoginClick(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
		}
		private void btnback(object sender, RoutedEventArgs e)
		{
			try
			{
				Manager.MainFrame.GoBack();
			}
			catch { }
		}

		private void themeButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Переключаем тему
				_isDarkTheme = !_isDarkTheme;

				// Находим ThemesDictionary в ресурсах приложения
				var appResources = Application.Current.Resources;
				var mergedDictionaries = appResources.MergedDictionaries;

				foreach (var dictionary in mergedDictionaries)
				{
					if (dictionary is ThemesDictionary themesDictionary)
					{
						// Меняем тему
						themesDictionary.Theme = _isDarkTheme
							? Wpf.Ui.Appearance.ApplicationTheme.Dark
							: Wpf.Ui.Appearance.ApplicationTheme.Light;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				// Обработка ошибок
				System.Windows.MessageBox.Show($"Ошибка при переключении темы: {ex.Message}");
			}
		}
	}
}
