using _28._01ui.EditorWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace _28._01ui.Pages
{
    public partial class PilotViewer : Page
    {
		int pilotId;
		Entities entities = new Entities();
        public PilotViewer()
        {
            InitializeComponent();
			pilotId = 73;
			LoadData(pilotId);
			CarCheck();
			AdminCheck();
        }

		private void AdminCheck()
		{
			if (DataHolder.SharedRoleId != 1)
			{
				adminPanel.Visibility = Visibility.Collapsed;
				buttonAddCar.Visibility = Visibility.Collapsed;
				adminPanelEditCar.Visibility = Visibility.Collapsed;
			}
		}
		private void LoadData(int Id)
		{
			entities = new Entities();
			var pilot = entities.Pilots.Find(Id);
			var imageBrush = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + pilot.PilotImg)),
				Stretch = Stretch.UniformToFill
			};
			borderPilotImg.Background = imageBrush;
			namePilot.Text = pilot.PilotName;
			cityPilot.Text = "Город: " + pilot.City;
			rolePilot.Text = "Должность: " + pilot.PilotRoles.NameRole;
			bioPilot.Text = pilot.Bio;
			pilotId = Id;
			teamName.Text = pilot.Teams.TeamName;
			bunnerTeam.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + pilot.Teams.TeamBunner));
		}
		private void CarCheck()
		{
			entities = new Entities();
			var pilot = entities.Pilots.Find(pilotId);
			if (pilot.CarId != null)
			{
				var car = entities.PilotCar.Find(pilot.CarId);
				buttonAddCar.Visibility = Visibility.Collapsed;
				borderCarInfo.Visibility = Visibility.Visible;
				nameCar.Text = "Наименование: " + car.Name;
				nameEngine.Text = "Двигатель: " + car.Engine;
				engineVol.Text = "Рабочий объем двигателя: " + car.Volume.ToString()+" л";
				engineHP.Text = "Мощность: " + car.HorsePower.ToString()+" л.с.";
			}
			else
			{
				buttonAddCar.Visibility = Visibility.Visible;
				borderCarInfo.Visibility = Visibility.Collapsed;
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			PilotEditorWindow window = new PilotEditorWindow(pilotId);
			Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.Closed += Editor_Closed;
			window.ShowDialog();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void btnEditCar_Click(object sender, RoutedEventArgs e)
		{
			CarEditorWindow window = new CarEditorWindow(pilotId);
			Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.Closed += Editor_Closed;
			window.ShowDialog();
		}

		private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
		{
			var pilot = entities.Pilots.Find(pilotId);
			var car = entities.PilotCar.Find(pilot.CarId);
			entities.PilotCar.Remove(car);
			entities.SaveChanges();
			CarCheck();
		}

		private void buttonAddCar_Click(object sender, RoutedEventArgs e)
		{
			CarEditorWindow window = new CarEditorWindow(pilotId);
			Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.Closed += Editor_Closed;
			window.ShowDialog();
		}
		private void Editor_Closed(object sender, EventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
			CarCheck();
			LoadData(pilotId);
		}
	}
}
