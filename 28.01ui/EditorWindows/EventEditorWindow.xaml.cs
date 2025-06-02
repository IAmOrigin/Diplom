using _28._01ui.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
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
		string regulation = null;
		string regulationDate = null;
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
				//regulationDate = selectedEvent.RegulationDate.ToString();
				//regulation = selectedEvent.Regulation;
				if (regulation == null)
				{
					regulationTextBlock.Text = "Добавьте регламент";
				}
				else
				{
					regulationTextBlock.Text = "Регламент";
				}
				foreach (var item in entities.EventGallery)
				{
					if (selectedEvent.Id == item.EventId)
					{
						itemsControlGallery.Items.Add(item);
					}
				}
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
			if (String.IsNullOrEmpty(eventName.Text) ||
				comboType.SelectedIndex == -1 ||
				String.IsNullOrEmpty(eventDesc.Text) ||
				String.IsNullOrEmpty(eventLocation.Text) ||
				eventDate.SelectedDate == default ||
				eventHours.Text == "" ||
				eventMinutes.Text == "" ||
				(regulationSwitch.IsChecked == true && (String.IsNullOrEmpty(regulation) || String.IsNullOrEmpty(regulationDate))) ||
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
			if (regulationSwitch.IsChecked == false)
			{
				//selectedEvent.Regulation = null
				//selectedEvent.Regulationdate = null
			}
			else
			{
				//selectedEvent.Regulation = regulation
				//selectedEvent.Regulationdate = regulationDate
			}
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

		private void Button_Gallery(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				string targetDirectory = Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\EventGallery\");
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
				newfile = Path.Combine(@"images\EventGallery\", Path.GetFileName(sourceFilePath));
				sourceFilePath = null;
				EventGallery photo = new EventGallery();
				photo.EventId = eventId;
				photo.Image = newfile;
				entities.EventGallery.Add(photo);
				itemsControlGallery.Items.Add(photo);
				
			}
			else
			{
				PopupManager.ShowMessage("Изображение не было выбрано");
			}
		}

		private void Button_RemoveGalleryItem(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var galleryItem = button.DataContext as EventGallery;
			entities.EventGallery.Remove(galleryItem);
			itemsControlGallery.Items.Remove(galleryItem);
		}

		private void regulationSwitch_Click(object sender, RoutedEventArgs e)
		{
			regulationSwitcher();
		}
		private void regulationSwitcher()
		{
			if (regulationButton.Visibility == Visibility.Visible)
			{
				regulationButton.Visibility = Visibility.Collapsed;
				regulationTextBlock.Visibility = Visibility.Collapsed;
			}
			else
			{
				regulationButton.Visibility = Visibility.Visible;
				regulationTextBlock.Visibility = Visibility.Visible;
			}
		}

		private void regulationButton_Click(object sender, RoutedEventArgs e)
		{
			RegulationEditorWindow window = new RegulationEditorWindow(regulation, regulationDate);
			window.Closed += RegulationEditor_Closed;
			window.ShowDialog();
			regulation = window.resultText;
			regulationDate = window.resultDate;
		}
		private void RegulationEditor_Closed(object sender, EventArgs e)
		{
			if (regulation == null)
			{
				regulationTextBlock.Text = "Добавьте регламент";
			}
			else
			{
				regulationTextBlock.Text = "Регламент";
			}
		}
	}
}
