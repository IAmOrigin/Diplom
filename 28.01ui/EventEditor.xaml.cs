using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using File = System.IO.File;
using Path = System.IO.Path;

namespace _28._01ui
{
	public partial class EventEditor : Page
	{
		bool addtype = false;
		string newfile = null;
		string sourceFilePath = null;
		Entities entities = new Entities();
		public EventEditor()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			TBoxEventType.Visibility = Visibility.Collapsed;
			foreach (var types in entities.EventType)
			{
				CBoxEventType.Items.Add(types);
			}
			var selectedEvent = entities.Events.Find(Convert.ToInt32(DataHolder.SharedEventId));
			if (selectedEvent != null)
			{
				TBoxNameEvent.Text = selectedEvent.EventName;
				DPickerEvent.SelectedDate = selectedEvent.EventDate;
				CBoxEventType.SelectedItem = selectedEvent.EventType;
				TBoxLocate.Text = selectedEvent.Location;
				TBoxDesc.Text = selectedEvent.EventDesc;
				NBoxRemain.Value = selectedEvent.TicketsRemain;
				NBoxPrice.Value = selectedEvent.Price;
				newfile = selectedEvent.EventImg;
				if (selectedEvent.EventImg != null)
				{
					var bunner = new ImageBrush
					{
						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg)),
						Stretch = Stretch.UniformToFill
					};
					newfile = selectedEvent.EventImg;
					imgborder.Background = bunner;
				}
			}
			
		}

		private void Save(object sender, RoutedEventArgs e)
		{
			var savedEvent = entities.Events.Find(Convert.ToInt32(DataHolder.SharedEventId));
			
			if (TBoxNameEvent.Text == "" ||
				DPickerEvent.SelectedDate == default ||
				CBoxEventType.SelectedIndex == -1 ||
				TBoxLocate.Text == "" || TBoxDesc.Text == "")
			{
				Warn.SetTextWithDelay("Заполните все поля");
			}
			else
			{
				if (savedEvent == null)
				{
					savedEvent = new Events();
					entities.Events.Add(savedEvent);
				}
				savedEvent.EventName = TBoxNameEvent.Text;
				savedEvent.EventDate = DPickerEvent.SelectedDate;
				savedEvent.EventType = CBoxEventType.SelectedItem as EventType;
				savedEvent.Location = TBoxLocate.Text;
				savedEvent.EventDesc = TBoxDesc.Text;
				savedEvent.Price = Convert.ToInt32(NBoxPrice.Value);			
				savedEvent.TicketsRemain = Convert.ToInt32(NBoxRemain.Value);
				
				if (newfile !=  null)
				{
					savedEvent.EventImg = newfile;
				}
				else
				{
					savedEvent.EventImg = @"\images\default.jpg";
				}

				if (savedEvent.EventDate < DateTime.Now)
				{
					Warn.Text = "Сохранено и помещено в архив";
				}
				else
				{
					Warn.Text = "Сохранено";
				}

				entities.SaveChanges();
				Manager.MainFrame.GoBack();
			}
		}

		private void Delete(object sender, RoutedEventArgs e)
		{
			var deleteEvent = entities.Events.Find(Convert.ToInt32(DataHolder.SharedEventId));

			var result = MessageBox.Show("Вы точно хотите удалить?", "Удаление",
			MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
				return;
			entities.Events.Remove(deleteEvent);
			entities.SaveChanges();
			Manager.MainFrame.GoBack();
		}

		private void BtnImg(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; ;
			if (openFileDialog.ShowDialog() == true)
			{
				sourceFilePath = openFileDialog.FileName;
				try
				{
					File.Copy(sourceFilePath, Path.Combine(ProjectDirectory.GetProjectDirectory(), @"images\EventImages\", Path.GetFileName(sourceFilePath)));
				}
				catch { }
				newfile = Path.Combine(@"images\EventImages\", Path.GetFileName(sourceFilePath));

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

		private void Cancel(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.GoBack();
		}

		private void AddType(object sender, RoutedEventArgs e)
		{
			if (addtype == false)
			{
				CBoxEventType.Visibility = Visibility.Collapsed;
				TBoxEventType.Visibility = Visibility.Visible;
				addtype = true;
			}
			else
			{
				
				var type = new EventType();
				try
				{
					if (TBoxEventType.Text != "")
					{
						entities.EventType.Add(type);
						type.TypeName = TBoxEventType.Text;
						entities.SaveChanges();
						CBoxEventType.Items.Add(type);
						CBoxEventType.Items.Refresh();
						CBoxEventType.SelectedItem = type;
					}
				}
				catch
				{
					CBoxEventType.Visibility = Visibility.Visible;
					TBoxEventType.Visibility = Visibility.Collapsed;
					addtype = false;
				}
				CBoxEventType.Visibility = Visibility.Visible;
				TBoxEventType.Visibility = Visibility.Collapsed;
				addtype = false;
			}
			
		}

		private void DeleteType(object sender, RoutedEventArgs e)
		{
			var selectedtype = CBoxEventType.SelectedItem as EventType;
			if (selectedtype != null)
			{
				foreach (var events in entities.Events)
				{
					if (events.IdEventType == selectedtype.Id)
					{
						entities.Events.Remove(events);
					}
				}
				entities.EventType.Remove(selectedtype);
				entities.SaveChanges();
				CBoxEventType.Items.Remove(selectedtype);
				CBoxEventType.Items.Refresh();
			}
		}
	}
}
