using _28._01ui.Properties;
using System.Globalization;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;
using System.Net.Sockets;

namespace _28._01ui
{
    
    public partial class designpage : Page
    {
		Entities entities = new Entities();
		public designpage()
		{
			InitializeComponent();
			foreach (var event1 in entities.Events)
			{
			var border = new Border
			{
				Margin = new Thickness(0, 0, 10, 20),
				CornerRadius = new CornerRadius(10),
				Height = 208,
				Width = 408
			};

			var stackPanel = new StackPanel
			{
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(5),
				Orientation = Orientation.Vertical
			};

			var dateTextBlock = new TextBlock
			{
				Text = event1.EventDate.ToString(),
				FontSize = 24,
				Foreground = Brushes.White
			};

			var nameTextBlock = new TextBlock
			{
				Text = event1.EventName,
				FontSize = 48,
				FontWeight = FontWeights.Bold,
				Foreground = Brushes.White,
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left,
				TextWrapping = TextWrapping.Wrap
			};

			stackPanel.Children.Add(dateTextBlock);
			stackPanel.Children.Add(nameTextBlock);

			border.Child = stackPanel;

			var imageBrush = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + event1.EventImg, UriKind.RelativeOrAbsolute)),
				Stretch = Stretch.UniformToFill
			};

			border.Background = imageBrush;

			EventContainer.Children.Add(border);
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
}
