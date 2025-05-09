using _28._01ui.Classes;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace _28._01ui.EditorWindows
{
    public partial class PilotEditorWindow : Window
    {
        Entities entities = new Entities();
        int pilotId;
		string newfile = null;
		string sourceFilePath = null;
		public PilotEditorWindow(int id)
        {
            InitializeComponent();
            pilotId = id;
            LoadData();
        }

        private void LoadData()
        {
			foreach(var roles in entities.PilotRoles)
			{
				comboRole.Items.Add(roles);
			}
            var pilot = entities.Pilots.Find(pilotId);
			if (pilot == null)
			{
				header.Text = "Добавление участника";
				return;
			}
			header.Text = "Редактирование участника";
            var imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + pilot.PilotImg)),
                Stretch = Stretch.UniformToFill,
            };
            borderPilotImg.Background = imageBrush;
            namePilot.Text = pilot.PilotName;
			birthDate.SelectedDate = pilot.PilotBirthDate;
            cityPilot.Text = pilot.City;
            bioPilot.Text = pilot.Bio;
			comboRole.SelectedItem = pilot.PilotRoles;
        }

		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
            this.Close();
		}

		private void Button_EditImg(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(sourceFilePath)),
					Stretch = Stretch.UniformToFill
				};
				borderPilotImg.Background = imageBrush;
			}
			else
			{
				PopupManager.ShowMessage("Изображение не было выбрано");
			}
		}

		private void Button_Save(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(namePilot.Text) ||
				String.IsNullOrEmpty(cityPilot.Text) ||
				String.IsNullOrEmpty(bioPilot.Text) ||
				birthDate.SelectedDate == default ||
				comboRole.SelectedIndex == -1)
			{
				PopupManager.ShowMessage("Заполните все поля");
				return;
			}
			if (birthDate.SelectedDate > DateTime.Today.AddYears(-18))
			{
				PopupManager.ShowMessage("Участник должен быть страше 18 лет");
				return;
			}
			var pilot = entities.Pilots.Find(pilotId);
			if (pilot == null)
			{
				pilot = new Pilots();
				entities.Pilots.Add(pilot);
			}
			pilot.PilotName = namePilot.Text;
			pilot.PilotBirthDate = birthDate.SelectedDate;
			pilot.City = cityPilot.Text;
			pilot.Bio = bioPilot.Text;
			pilot.PilotRoles = comboRole.SelectedItem as PilotRoles;
			try
			{
				if (sourceFilePath != null)
				{
					string targetDirectory = Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\ProfileImages\");
					string targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(sourceFilePath));
					if (File.Exists(targetFilePath))
					{
						string fileNameWithoutExt = Path.GetFileNameWithoutExtension(sourceFilePath);
						string fileExt = Path.GetExtension(sourceFilePath);
						int counter = 1;
						do
						{
							targetFilePath = Path.Combine(targetDirectory, $"{fileNameWithoutExt}{counter}{fileExt}");
							counter++;
						} while (File.Exists(targetFilePath));
					}
					File.Copy(sourceFilePath, targetFilePath);
					newfile = Path.Combine(@"images\ProfileImages\", Path.GetFileName(sourceFilePath));
					pilot.PilotImg = newfile;
				}
			}
			catch(Exception ex) 
			{
				if(pilot.PilotImg == null)
				{
					pilot.PilotImg = Pic.defaultpic;
				}
				PopupManager.ShowMessage("Не удалось добавить изображение");
				MessageBox.Show(ex.ToString());
			}
			entities.SaveChanges();
			this.Close();
		}

		private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (birthDate.SelectedDate != null)
			{
				datePickerPlaceHolder.Text = "";
			}
			else
			{
				datePickerPlaceHolder.Text = "Дата рождения";
			}
		}
	}
}
