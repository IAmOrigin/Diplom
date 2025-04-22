using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace _28._01ui
{
	public partial class PilotEditor : Page
	{
		Entities entities = new Entities();
		bool newpilotbool;
		string newfile = null;
		string sourceFilePath = null;
		public PilotEditor()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			stackNew.Visibility = Visibility.Collapsed;
			stackTransfer.Visibility = Visibility.Collapsed;
			stackSaveCancel.Visibility = Visibility.Collapsed;
			foreach (var team in entities.Teams)
			{
				if (team.Id != DataHolder.SharedTeamId)
				{
					listviewTeams.Items.Add(team);
				}
			}
		}

		private void AddImg(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			newfile = null;
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				try
				{
					File.Copy(sourceFilePath, Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\ProfileImages\", Path.GetFileName(sourceFilePath)));
				}
				catch { }
				newfile = Path.Combine(@"images\ProfileImages\", Path.GetFileName(sourceFilePath));

				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + newfile)),
					Stretch = Stretch.UniformToFill
				};
				imgborder.Background = imageBrush;
			}
			else
			{
				Warn.SetTextWithDelay("Изображение не было выбрано");
			}
		}

		private void newPilot (object sender, RoutedEventArgs e)
		{
			newpilotbool = true;
			foreach (var roles in entities.PilotRoles)
			{
				comboRole.Items.Add(roles);
			}
			stackNew.Visibility = Visibility.Visible;
			stackSaveCancel.Visibility = Visibility.Visible;
			stackNewTransfer.Visibility = Visibility.Collapsed;
		}

		private void transferPilot (object sender, RoutedEventArgs e)
		{
			stackNewTransfer.Visibility = Visibility.Collapsed;
			stackTransfer.Visibility = Visibility.Visible;
			stackSaveCancel.Visibility = Visibility.Visible;
		}

		private void btnCancel(object sender, RoutedEventArgs e)
		{
			newpilotbool = false;
			stackNewTransfer.Visibility = Visibility.Visible;
			stackNew.Visibility = Visibility.Collapsed;
			stackTransfer.Visibility = Visibility.Collapsed;
			stackSaveCancel.Visibility = Visibility.Collapsed;
		}

		private void listviewTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedTeam = listviewTeams.SelectedItem as Teams;
			listviewPilots.Items.Clear();
			foreach (var pilot in entities.Pilots)
			{
				if (selectedTeam.Id == pilot.TeamId)
				{
					listviewPilots.Items.Add(pilot);
				}
			}
		}

		private void btnSave(object sender, RoutedEventArgs e)
		{
			if (newpilotbool == true)
			{
				if (tboxName.Text == null || dpickerBirthDate.SelectedDate == default || comboRole.SelectedIndex == -1)
				{
					Warn.SetTextWithDelay("Заполните все поля");
				}
				else
				{
					if (dpickerBirthDate.SelectedDate > DateTime.Today.AddYears(-18))
					{
						Warn.SetTextWithDelay("Участник должен быть старше 18 лет");
					}
					else
					{
						var newpilot = new Pilots();
						newpilot.PilotBirthDate = dpickerBirthDate.SelectedDate;
						entities.Pilots.Add(newpilot);
						newpilot.TeamId = DataHolder.SharedTeamId;
						newpilot.PilotName = tboxName.Text;
						newpilot.PilotRoles = comboRole.SelectedItem as PilotRoles;
						if (newfile != null)
						{
							newpilot.PilotImg = newfile;
						}
						else
						{
							newpilot.PilotImg = Pic.defaultprofilepic;
						}
						entities.SaveChanges();
						Manager.MainFrame.GoBack();
					}
				}
			}
			else
			{
				var transferpilot = listviewPilots.SelectedItem as Pilots;
				if (transferpilot != null)
				{
					transferpilot.TeamId = DataHolder.SharedTeamId;
					entities.SaveChanges();
					Manager.MainFrame.GoBack();
				}
				else
				{
					Warn.SetTextWithDelay("Выберите участника");
				}
			}
		}
	}
}
