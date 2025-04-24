using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using _28._01ui.Classes;
using _28._01ui.Properties;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Input;
using Wpf.Ui.Markup;

namespace _28._01ui
{
	public partial class MainWindow : FluentWindow
	{
		Entities entities = new Entities(); 
        public MainWindow()
		{
			InitializeComponent();
			ApplyTheme();
			Pic.defaultpic = @"images\default.jpg";
			Pic.defaultprofilepic = @"images\defaultprofilepic.jpg";
			Manager.MainFrame = MainFrame;
			Manager.MainFrame.Navigated += OnNavigated;
			Manager.MainFrame.Navigate(new StartPage());
			PopupManager.Initialize(popup1);
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
			var page = MainFrame.Content as Page;
			if (page is StartPage)
			{
				PopupManager.ShowMessage("Вы уже на главной странице!");
				return;
			}
			Manager.MainFrame.Navigate(new Uri("Pages/StartPage.xaml", UriKind.Relative));
		}

		private void ProfileClick(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/ProfilePage.xaml", UriKind.Relative));
		}

		private void LoginClick(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/LoginPage.xaml", UriKind.Relative));
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
			Settings.Default.darkTheme = !Settings.Default.darkTheme;
			ApplyTheme();
			Settings.Default.Save();
		}

		private void ApplyTheme()
		{
			try
			{
				var appResources = Application.Current.Resources;
				var mergedDictionaries = appResources.MergedDictionaries;

				foreach (var dictionary in mergedDictionaries)
				{
					if (dictionary is ThemesDictionary themesDictionary)
					{
						if (Settings.Default.darkTheme)
						{
							themesDictionary.Theme = Wpf.Ui.Appearance.ApplicationTheme.Dark;
							Application.Current.Resources["AppBackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1B1B1B"));
							Application.Current.Resources["AppTextColor"] = Brushes.White;
						}
						else
						{
							themesDictionary.Theme = Wpf.Ui.Appearance.ApplicationTheme.Light;
							Application.Current.Resources["AppBackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC1C0C0"));
							Application.Current.Resources["AppTextColor"] = Brushes.Black;
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show($"Ошибка при применении темы: {ex.Message}");
			}
		}

		private void ClosePopup(object sender, RoutedEventArgs e)
		{
			popup1.IsOpen= false;	
		}
	}
}
