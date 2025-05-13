using _28._01ui.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _28._01ui.EditorWindows
{
    public partial class EventEditorWindow : Window
    {
        Entities entities = new Entities();
		int eventId;
		string newfile = null;
		string sourceFilePath = null;
		public EventEditorWindow()
        {
            InitializeComponent();
			eventId = DataHolder.SharedEventId;
			LoadData();
        }

		private void LoadData()
		{
			foreach (var type in entities.EventType)
			{
				comboType.Items.Add(type);
			}
			if (eventId != 0)
			{
				var selectedEvent = entities.Events.Find(eventId);
				var imageBrush = new ImageBrush()
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg)),
					Stretch = Stretch.UniformToFill
				};
				borderEventImg.Background = imageBrush;
				eventName.Text = selectedEvent.EventName;
				comboType.SelectedItem = selectedEvent.EventType;
				eventDesc.Text = selectedEvent.EventDesc;
				eventLocation.Text = selectedEvent.Location;
				eventDate.SelectedDate = selectedEvent.EventDate;
				DateTime dateTime = selectedEvent.EventDate.Value;
				eventHours.Value = dateTime.Hour;
				eventMinutes.Value = dateTime.Minute;
				if (selectedEvent.Price != 0)
				{
					ticketSwitch.IsChecked = false;
					ticketsRemain.Value = selectedEvent.TicketsRemain;
					ticketPrice.Value = selectedEvent.Price;
				}
				else
				{
					ticketSwitch.IsChecked = true;
					ticketSwitcher();
				}
			}
		}

		private void buttonNext_Click(object sender, RoutedEventArgs e)
		{
			buttonBack.Visibility = Visibility.Visible;
			buttonNext.Visibility = Visibility.Collapsed;
			buttonSave.Visibility = Visibility.Visible;
			page1.Visibility = Visibility.Collapsed;
			page2.Visibility = Visibility.Visible;
		}

		private void buttonBack_Click(object sender, RoutedEventArgs e)
		{
			buttonBack.Visibility = Visibility.Collapsed;
			buttonNext.Visibility = Visibility.Visible;
			buttonSave.Visibility = Visibility.Collapsed;
			page1.Visibility = Visibility.Visible;
			page2.Visibility = Visibility.Collapsed;
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
			this.Close();
		}

		private void ticketSwitch_Click(object sender, RoutedEventArgs e)
		{
			ticketSwitcher();
		}

		private void ticketSwitcher()
		{
			if (ticketsRemain.Visibility == Visibility.Visible)
			{
				ticketsRemain.Visibility = Visibility.Collapsed;
				ticketPrice.Visibility = Visibility.Collapsed;
			}
			else
			{
				ticketsRemain.Visibility = Visibility.Visible;
				ticketPrice.Visibility = Visibility.Visible;
			}
		}

		private void buttonAddImg_Click(object sender, RoutedEventArgs e)
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
				borderEventImg.Background = imageBrush;
			}
			else
			{
				PopupManager.ShowMessage("Изображение не было выбрано");
			}
		}

		private void buttonSave_Click(object sender, RoutedEventArgs e)
		{
			var selectedEvent = entities.Events.Find(eventId);
			if(String.IsNullOrEmpty(eventName.Text) ||
				comboType.SelectedIndex == -1 ||
				String.IsNullOrEmpty(eventDesc.Text) ||
				String.IsNullOrEmpty(eventLocation.Text) ||
				eventDate.SelectedDate == default ||
				eventHours.Text == "" ||
				eventMinutes.Text == "" ||
				(ticketSwitch.IsChecked == false && (ticketsRemain.Text == "" || ticketPrice.Text == "")))
			{
				PopupManager.ShowMessage("Заполните все поля");
				return;
			}
			if (selectedEvent == null)
			{
				selectedEvent = new Events();
				entities.Events.Add(selectedEvent);
			}
			selectedEvent.EventName = eventName.Text;
			selectedEvent.EventType = comboType.SelectedItem as EventType;
			selectedEvent.EventDesc = eventDesc.Text;
			selectedEvent.Location = eventLocation.Text;
			DateTime selectedDate = eventDate.SelectedDate.Value.Date;
			int hours = (int)eventHours.Value;
			int minutes = (int)eventMinutes.Value;
			DateTime combinedDateTime = new DateTime(
				selectedDate.Year,
				selectedDate.Month,
				selectedDate.Day,
				hours,
				minutes,
				0
			);
			selectedEvent.EventDate = combinedDateTime;
			if (ticketSwitch.IsChecked == true)
			{
				selectedEvent.TicketsRemain = 0;
				selectedEvent.Price = 0;
			}
			else
			{
				selectedEvent.TicketsRemain = (int)ticketsRemain.Value;
				selectedEvent.Price = (int)ticketPrice.Value;
			}
			try
			{
				if (sourceFilePath != null)
				{
					string targetDirectory = Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\EventImages\");
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
					newfile = Path.Combine(@"images\EventImages\", Path.GetFileName(sourceFilePath));
					selectedEvent.EventImg = newfile;
				}
				else
				{
					if (selectedEvent.EventImg == null)
					{
						selectedEvent.EventImg = Pic.defaultpic;
					}
				}
			}
			catch (Exception ex)
			{
				if (selectedEvent.EventImg == null)
				{
					selectedEvent.EventImg = Pic.defaultpic;
				}
				PopupManager.ShowMessage("Не удалось добавить изображение");
				MessageBox.Show(ex.ToString());
			}
			entities.SaveChanges();
			Close();
		}

		private void buttonAddType_Click(object sender, RoutedEventArgs e)
		{
			EventTypeEditorWindow window = new EventTypeEditorWindow();
			window.ShowDialog();
		}
	}
}
