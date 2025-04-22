using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _28._01ui
{
	public partial class TeamEditor : Page
	{
		string newfilebunner = null;
		string newfilelogo = null;
		string sourceFilePath = null;
		Entities entities = new Entities();
		public TeamEditor()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			if (DataHolder.SharedTeamId != 0)
			{
				var team = entities.Teams.Find(DataHolder.SharedTeamId);
				tboxName.Text = team.TeamName;
				tboxCountry.Text = team.TeamCountry;
				if(team.TeamBunner != null)
				{
					try
					{
						var bunner = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + team.TeamBunner)),
							Stretch = Stretch.UniformToFill
						};
						newfilebunner = team.TeamBunner;
						bunnerimg.Background = bunner;
					}
					catch
					{
						var bunner = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + Pic.defaultpic)),
							Stretch = Stretch.UniformToFill
						};
						newfilebunner = team.TeamBunner;
						bunnerimg.Background = bunner;
					}
				}
				if(team.TeamLogo != null)
				try
				{
					var logo = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + team.TeamLogo)),
						Stretch = Stretch.UniformToFill
					};
					newfilelogo = team.TeamLogo;
					logoimg.Background = logo;
					}
				catch
				{
					Warn.SetTextWithDelay("Не удалось открыть изображение");
					{
						var logo = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + Pic.defaultpic)),
							Stretch = Stretch.UniformToFill
						};
						newfilelogo = team.TeamLogo;
						logoimg.Background = logo;
					}
				}
			}
		}

		private void ButtonSave(object sender, RoutedEventArgs e)
		{
			var team = entities.Teams.Find(DataHolder.SharedTeamId);
			if (tboxName.Text == "" || tboxCountry.Text == "")
			{
				Warn.SetTextWithDelay("Заполните поля");
			}
			else
			{
				if (team == null)
				{
					team = new Teams();
					entities.Teams.Add(team);
				}

				team.TeamName = tboxName.Text;
				team.TeamCountry = tboxCountry.Text;
				if (newfilebunner != null)
				{
					team.TeamBunner = newfilebunner;
				}
				else
				{
					team.TeamBunner = Pic.defaultpic;
				}
				if (newfilelogo != null)
				{
					team.TeamLogo = newfilelogo;
				}
				else
				{
					team.TeamLogo = Pic.defaultpic;
				}
				entities.SaveChanges();
				Manager.MainFrame.GoBack();
			}
		}

		private void ButtonCancel (object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.GoBack();
		}

		private void ButtonBunner(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				try
				{
					File.Copy(sourceFilePath, Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\TeamImages\Banners\", Path.GetFileName(sourceFilePath)));
				}
				catch { }
				newfilebunner = Path.Combine(@"images\TeamImages\Banners\", Path.GetFileName(sourceFilePath));

				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + newfilebunner)),
					Stretch = Stretch.UniformToFill
				};
				bunnerimg.Background = imageBrush;
			}
			else
			{
				Warn.SetTextWithDelay("Изображение не было выбрано");
			}
		}

		private void ButtonLogo(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				try
				{
					File.Copy(sourceFilePath, Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\TeamImages\", Path.GetFileName(sourceFilePath)));
				}
				catch { }
				newfilelogo = Path.Combine(@"images\TeamImages\", Path.GetFileName(sourceFilePath));

				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + newfilelogo)),
					Stretch = Stretch.UniformToFill
				};
				logoimg.Background = imageBrush;
			}
			else
			{
				Warn.SetTextWithDelay("Изображение не было выбрано");
			}
		}
	}
}
