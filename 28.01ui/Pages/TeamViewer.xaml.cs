using _28._01ui.EditorWindows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using TextBlock = System.Windows.Controls.TextBlock;

namespace _28._01ui
{
	public partial class TeamViewer : Page
	{
		Entities entities = new Entities();
		public TeamViewer()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
			if (DataHolder.SharedRole != "Admin")
			{
				btnEdit.Visibility = Visibility.Collapsed;
				btnDelete.Visibility = Visibility.Collapsed;
			}
			var team = entities.Teams.Find(DataHolder.SharedTeamId);
			if (team.TeamBunner != null)
			{
				bunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + team.TeamBunner));
			}
			else
			{
				bunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + Pic.defaultpic));
			}
			textblockteamname.Text = team.TeamName;
			pilotscontainer.Children.Clear();
			foreach (var pilot in entities.Pilots)
			{
				if (pilot.TeamId == DataHolder.SharedTeamId)
				{
					var border = new Border
					{
						Width = 260,
						Height = 360,
						CornerRadius = new CornerRadius(8),
						Margin = new Thickness(0, 0, 10, 20)
					};
					var stackpanel = new StackPanel
					{
						Orientation = Orientation.Vertical,
					};
					var imageBorder = new Border
					{
						CornerRadius = new CornerRadius(500),
						Width = 240,
						Height = 240,
						Margin = new Thickness(10)
					};
					if (pilot.PilotImg != null)
					{
						var imageBrush = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + pilot.PilotImg)),
							Stretch = Stretch.UniformToFill
						};
						imageBorder.Background = imageBrush;
					}
					else
					{
						var imageBrush = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"/images/ProfileImages/p3.jpg")),
							Stretch = Stretch.UniformToFill
						};
						imageBorder.Background = imageBrush;
					}
						var roleTextBlock = new TextBlock
					{
						Text = pilot.PilotRoles.NameRole,
						HorizontalAlignment = HorizontalAlignment.Center,
						Foreground = Brushes.White,
						FontWeight = FontWeights.Medium,
						FontSize = 28,
					};
					var nameTextBlock = new TextBlock
					{
						Text = pilot.PilotName,
						HorizontalAlignment = HorizontalAlignment.Center,
						Foreground = Brushes.White,
						FontSize = 18,
					};
					border.Child = stackpanel;
					stackpanel.Children.Add(imageBorder);
					stackpanel.Children.Add(roleTextBlock);
					stackpanel.Children.Add(nameTextBlock);
					pilotscontainer.Children.Add(border);
					if (DataHolder.SharedRole == "Admin")
					{
						var delbutton = new Wpf.Ui.Controls.Button
						{
							HorizontalAlignment = HorizontalAlignment.Center,
							Appearance = ControlAppearance.Transparent,
							Foreground = Brushes.Coral,
							Tag = pilot.Id,
							Content = "Удалить",
							Name = "deletepilot",
							BorderThickness = new Thickness(0),
						};
						delbutton.Click += (sender, e) =>
						{
							var button = sender as Wpf.Ui.Controls.Button;
							if (button != null)
							{
								int pilotId = (int)button.Tag;
								var pilot1 = entities.Pilots.Find(pilotId);
								entities.Pilots.Remove(pilot1);
								pilotscontainer.Children.Remove(border);
								entities.SaveChanges();
							}
						};
						stackpanel.Children.Add(delbutton);
					}
				}
			}

			if (DataHolder.SharedRole == "Admin")
			{
				var uiButton = new Wpf.Ui.Controls.Button
			{
				Margin = new Thickness(10),
				CornerRadius = new CornerRadius(500),
				Height = 240,
				Width = 240,
				BorderThickness = new Thickness(0),
				Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"))
			};
			var tblockadd = new TextBlock
			{
				Margin = new Thickness(0, 0, 2, 12),
				Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#33FFFFFF")),
				Text = "+",
				FontSize = 48,
			};
			uiButton.Content = tblockadd;
			var roleTextBlock1 = new TextBlock
			{
				FontSize = 28,
			};
			var nameTextBlock1 = new TextBlock
			{
				FontSize = 18
			};
			var stackpanel1 = new StackPanel
			{
				Orientation = Orientation.Vertical,
			};
				uiButton.Click += btnAdd_Click;
				stackpanel1.Children.Add(uiButton);
				stackpanel1.Children.Add(roleTextBlock1);
				stackpanel1.Children.Add(nameTextBlock1);
			pilotscontainer.Children.Add(stackpanel1);
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			PilotAddChanger window = new PilotAddChanger();
			window.ShowDialog();
			//Manager.MainFrame.Navigate(new PilotEditor());
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new TeamEditor());
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var team = entities.Teams.Find(DataHolder.SharedTeamId);
			foreach (var pilot in entities.Pilots)
			{
				if (pilot.TeamId == team.Id)
				{
					foreach (var result in entities.Results)
					{
						if (result.PilotId == pilot.Id)
						{
							entities.Results.Remove(result);
						}
					}
					entities.Pilots.Remove(pilot);
				}
			}
			entities.Teams.Remove(team);
			entities.SaveChanges();
			Manager.MainFrame.GoBack();
		}
	}
}
