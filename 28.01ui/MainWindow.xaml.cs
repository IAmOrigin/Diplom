using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using _28._01ui.Classes;
using _28._01ui.Pages;
using _28._01ui.Properties;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Input;
using Wpf.Ui.Markup;
using MessageBox = System.Windows.MessageBox;

namespace _28._01ui
{
	public partial class MainWindow : FluentWindow
	{
		Entities entities = new Entities(); 
        public MainWindow()
		{
			InitializeComponent();
			LoadWindowSettings();
			this.Closing += MainWindow_Closing;
			this.StateChanged += MainWindow_StateChanged;
			ApplyTheme();
			Pic.defaultpic = @"images\default.jpg";
			Pic.defaultprofilepic = @"images\defaultprofilepic.jpg";
			Manager.DialogOverlay = dialogOverlay;
			Manager.MainFrame = MainFrame;
			Manager.MainFrame.Navigated += OnNavigated;
			Manager.MainFrame.Navigate(new StartPage1());
			PopupManager.Initialize(popup1, confirmPopup);

			

		}

		private void OnNavigated(object sender, NavigationEventArgs e)
		{
			var page = MainFrame.Content as Page;
			if (page is StartPage)
			{
				//buttonback.Visibility = Visibility.Collapsed;
			}
			else
			{
				buttonback.Visibility = Visibility.Visible;
			}

			if (Settings.Default.loggedInUser != 0)
			{
				var entity = entities.Users.Find(Settings.Default.loggedInUser);
				DataHolder.SharedRoleId = (Int32)entity.IdRole;
				profilebtn.Visibility = Visibility.Visible;
				loginbtn.Visibility = Visibility.Collapsed;
			}
			else
			{
				DataHolder.SharedRoleId = 0;
				profilebtn.Visibility = Visibility.Collapsed;
				loginbtn.Visibility = Visibility.Visible;
			}
		}

		private void HomeClick(object sender, RoutedEventArgs e)
		{
			var page = MainFrame.Content as Page;
			if (page is StartPage)
			{
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
							Application.Current.Resources["AppDialogColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF141414"));
							Application.Current.Resources["AppTextColor"] = Brushes.White;
						}
						else
						{
							themesDictionary.Theme = Wpf.Ui.Appearance.ApplicationTheme.Light;
							Application.Current.Resources["AppBackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC1C0C0"));
							Application.Current.Resources["AppDialogColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC7C7C7"));
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

		private void MainWindow_StateChanged(object sender, EventArgs e)
		{
			SaveWindowSettings();
		}

		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			SaveWindowSettings();
		}

		private void SaveWindowSettings()
		{
			Settings.Default.windowState = this.WindowState;
			if (this.WindowState == WindowState.Normal)
			{
				Settings.Default.windowTop = this.Top;
				Settings.Default.windowLeft = this.Left;
				Settings.Default.windowHeight = this.Height;
				Settings.Default.windowWidth = this.Width;
			}
			Settings.Default.Save();
		}

		private void LoadWindowSettings()
		{
			if (Settings.Default.windowTop >= 0 &&
				Settings.Default.windowLeft >= 0)
			{
				this.Top = Settings.Default.windowTop;
				this.Left = Settings.Default.windowLeft;
			}
			if (Settings.Default.windowHeight > 0 &&
				Settings.Default.windowWidth > 0)
			{
				this.Height = Settings.Default.windowHeight;
				this.Width = Settings.Default.windowWidth;
			}
			if (Settings.Default.windowState != WindowState.Minimized)
			{
				this.WindowState = Settings.Default.windowState;
			}
		}

		private void ClosePopup(object sender, RoutedEventArgs e)
		{
			popup1.IsOpen= false;	
		}

		private void testButton_Click(object sender, RoutedEventArgs e)
		{
			//Manager.MainFrame.Navigate(new PilotViewer());
		}

		private void dialogOverLay_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
