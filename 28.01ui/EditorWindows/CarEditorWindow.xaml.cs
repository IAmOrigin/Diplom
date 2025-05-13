using _28._01ui.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace _28._01ui.EditorWindows
{
    public partial class CarEditorWindow : Window
    {
        Entities entities = new Entities();
        int pilotId = 0;
        public CarEditorWindow(int Id)
        {
            InitializeComponent();
            pilotId = Id;
			LoadData(Id);
			
		}

		private void LoadData(int Id)
		{
			var pilot = entities.Pilots.Find(pilotId);
			var car = entities.PilotCar.Find(pilot.CarId);
			if (car == null)
			{
				return;
			}
			nameCar.Text = car.Name;
			nameEngine.Text = car.Engine;
			engineVol.Text = car.Volume.ToString();
			engineHP.Text = car.HorsePower.ToString();
		}

		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
            this.Close();
		}

		private void Button_Save(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(nameCar.Text) ||
				string.IsNullOrEmpty(nameEngine.Text) ||
				string.IsNullOrEmpty(engineVol.Text) ||
				string.IsNullOrEmpty(engineHP.Text))
			{
				PopupManager.ShowMessage("Заполните все поля");
				return;
			}
			Regex decimalRegex = new Regex(@"^(?!0$)(0|[1-9]\d*)([.,]\d+)?$");
			Regex hpRegex = new Regex(@"^[1-9]\d*$");
			if (!decimalRegex.IsMatch(engineVol.Text))
			{
				PopupManager.ShowMessage("Поле Объем двигателя должно быть заполено в формате: л или л.мл");
				return;
			}
			string volumeText = engineVol.Text.Replace(".", ",");
			decimal volume = Convert.ToDecimal(volumeText, CultureInfo.GetCultureInfo("ru-RU"));
			if (!hpRegex.IsMatch(engineHP.Text))
			{
				PopupManager.ShowMessage("Поле Мощность должно содержать целое число");
				return;
			}
			var pilot = entities.Pilots.Find(pilotId);
			var car = entities.PilotCar.Find(pilot.CarId);
			if (car == null)
			{
				car = new PilotCar();
				entities.PilotCar.Add(car);
				pilot.CarId = car.Id;
				
			}
			car.Name = nameCar.Text;
			car.Engine = nameEngine.Text;
			car.Volume = volume;
			car.HorsePower = Convert.ToInt32(engineHP.Text);
			entities.SaveChanges();
			this.Close();
		}
	}
}
