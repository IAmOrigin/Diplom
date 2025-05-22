using _28._01ui.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _28._01ui
{
	public partial class RatingEditor : Page
	{
		Entities entities = new Entities();
		public RatingEditor()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			animGrid1.ContentSlideUp();
			var result = entities.Results.Find(DataHolder.SharedResultId);
			var resultEvent = entities.Events.Find(DataHolder.SharedResultEventId);
			textblockEventName.Text = resultEvent.EventName;
			ButtonDelete.Visibility = Visibility.Collapsed;
			tblockEvent.Text ="Событие: " + DataHolder.SharedResultEventId.ToString();
			if (resultEvent.EventImg != null)
			{
				eventBunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + resultEvent.EventImg));
			}
			if (DataHolder.SharedResultId != 0)
			{
				ButtonDelete.Visibility = Visibility.Visible;
				textblockNamePilot.Text = result.Pilots.PilotName;
				numberboxPoints.Value = result.Points;
				textboxTime.Text = result.Time.ToString();
				if (result.Pilots.PilotImg != null)
				{
					var imageBrush = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + result.Pilots.PilotImg)),
						Stretch = Stretch.UniformToFill
					};
					pilotBorderImage.Background = imageBrush;
				}
				else
				{
					var imageBrush = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + Pic.defaultprofilepic)),
						Stretch = Stretch.UniformToFill
					};
					pilotBorderImage.Background = imageBrush;
				}
				textblockNamePilot.Text = result.Pilots.PilotName;
				tblockEvent.Text += " Пилот: " + result.PilotId.ToString();
				enterhelper1.Visibility = Visibility.Collapsed;
				enterhelper2.Visibility = Visibility.Collapsed;
				listviewTeams.Visibility = Visibility.Collapsed;
				listviewPilots.Visibility = Visibility.Collapsed;
			}
			foreach(var team in entities.Teams)
			{
				listviewTeams.Items.Add(team);
			}
		}

		private void listviewTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedTeam = listviewTeams.SelectedItem as Teams;
			listviewPilots.Items.Clear();
			foreach(var pilot in entities.Pilots)
			{
				if (pilot.TeamId == selectedTeam.Id)
				{
					listviewPilots.Items.Add(pilot);
				}
			}
		}

		private void listviewPilots_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedPilot = listviewPilots.SelectedItem as Pilots;
			if (selectedPilot != null)
			{
				textblockNamePilot.Text = selectedPilot.PilotName;
				if (selectedPilot.PilotImg != null)
				{
					var imageBrush = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedPilot.PilotImg)),
						Stretch = Stretch.UniformToFill
					};
					pilotBorderImage.Background = imageBrush;
				}
				else
				{
					var imageBrush = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + Pic.defaultprofilepic)),
						Stretch = Stretch.UniformToFill
					};
					pilotBorderImage.Background = imageBrush;
				}
			}
			else
			{
				textblockNamePilot.Text = "";
				pilotBorderImage.Background = null;
			}
			
		}

		private void Save(object sender, RoutedEventArgs e)
		{
			if (DataHolder.SharedResultId != 0)
			{
				var savedresult = entities.Results.Find(DataHolder.SharedResultId);
				if (numberboxPoints.Value == null)
				{
					savedresult.Points = null;
				}
				else
				{
					savedresult.Points = Convert.ToInt32(numberboxPoints.Value);
				}
				if (textboxTime.Text == "")
				{
					savedresult.Time = null;
				}
				else
				{
					savedresult.Time = TimeSpan.Parse(textboxTime.Text);
				}
				try
				{
					entities.SaveChanges();
					Manager.MainFrame.GoBack();
				}
				catch
				{
					PopupManager.ShowMessage("Неверный формат времени");
					return;
				}
			}
			else if (listviewPilots.SelectedItem != null)
			{
				var selectedpilot = listviewPilots.SelectedItem as Pilots;

				var savedresult = new Results();
				
				savedresult.PilotId = selectedpilot.Id;
				savedresult.EventId = DataHolder.SharedResultEventId;
				if (numberboxPoints.Value == null)
				{
					savedresult.Points = null;
				}
				else
				{
					savedresult.Points = Convert.ToInt32(numberboxPoints.Value);
				}
				if (textboxTime.Text == "")
				{
					savedresult.Time = null;
				}
				else
				{
					TimeSpan timeResult;
					if (TimeSpan.TryParse(textboxTime.Text, out timeResult))
					{
						// Проверка, что время не превышает 24 часа
						if (timeResult >= TimeSpan.Zero && timeResult < TimeSpan.FromHours(24))
						{
							savedresult.Time = timeResult;
						}
						else
						{
							MessageBox.Show("Время должно быть в диапазоне от 00:00:00 до 23:59:59.");
							savedresult.Time = null;
						}
					}
					else
					{
						MessageBox.Show("Неверный формат времени. Пожалуйста, введите время в формате 'hh:mm:ss'.");
						savedresult.Time = null;
					}
				}
				entities.Results.Add(savedresult);
				entities.SaveChanges();
				Manager.MainFrame.GoBack();
				
			}
			else
			{
				PopupManager.ShowMessage("Выберите пилота");
			}
		}

		private void Delete(object sender, RoutedEventArgs e)
		{
			var result = entities.Results.Find(DataHolder.SharedResultId);
			entities.Results.Remove(result);
			entities.SaveChanges();
			Manager.MainFrame.GoBack();
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.GoBack();
		}
	}
}
