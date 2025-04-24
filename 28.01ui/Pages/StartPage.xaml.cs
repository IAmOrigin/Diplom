using _28._01ui.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace _28._01ui
{
    
    public partial class StartPage : Page
    {
		private List<BitmapImage> _images = new List<BitmapImage>();
		private int _currentImageIndex = 0;
		private DispatcherTimer _timer;
		public StartPage()
        {
            InitializeComponent();
			LoadImages();
			InitializeTimer();
			SetCurrentImage();
			animGrid.InitSlideUp();
		}
		private void LoadImages()
		{
			for (int i = 1; i <= 9; i++)
			{
				string imagePath = ProjectDirectory.GetProjectDirectory() + $"/images/ImageSlider/{i}.jpg";
				_images.Add(new BitmapImage(new Uri(imagePath)));
			}
			for (int i = 1; i <= 20; i++)
			{
				string imagePath = ProjectDirectory.GetProjectDirectory() + $"/images/BackImages/{i}.jpg";
				_images.Add(new BitmapImage(new Uri(imagePath)));
			}
		}

		private void InitializeTimer()
		{
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromSeconds(4.2); // Интервал - 4.20 секунд
			_timer.Tick += TimerTick;
			_timer.Start();
		}

		private void SetCurrentImage()
		{
			imageDisplay.Source = _images[_currentImageIndex];
			// Сброс анимации, чтобы она начиналась каждый раз сначала.
			imageDisplay.Opacity = 1.0;
		}

		private void TimerTick(object sender, EventArgs e)
		{
			_currentImageIndex = (_currentImageIndex + 1) % _images.Count; // Переход к следующей картинке

			// Создаем анимацию для затухания
			DoubleAnimation fadeOutAnimation = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(0.65)); // Анимация затухания
			fadeOutAnimation.Completed += (s, args) =>
			{
				// После затухания ставим новое изображение и делаем плавное появление
				SetCurrentImage();
				DoubleAnimation fadeInAnimation = new DoubleAnimation(0.0, 1.0, TimeSpan.FromSeconds(0.65)); // Анимация плавного появления
				imageDisplay.BeginAnimation(OpacityProperty, fadeInAnimation);
			};
			imageDisplay.BeginAnimation(OpacityProperty, fadeOutAnimation); // Начинаем анимацию затухания

		}

		private void RaceBtn(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/EventPage.xaml", UriKind.Relative));
		}

		private void ArchiveBtn(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/ArchivePage.xaml", UriKind.Relative));
		}

		private void RatingBtn(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedResultEventId = 0;
			Manager.MainFrame.Navigate(new Uri("Pages/RatingPage.xaml", UriKind.Relative));
		}

		private void TeamsBtn(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/TeamsPage.xaml", UriKind.Relative));
		}
    }
}
	

