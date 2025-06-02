using _28._01ui.Classes;
using _28._01ui.EditorWindows;
using _28._01ui.TicketWindowFolder;
using _28._01ui.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace _28._01ui.Pages
{
    public partial class EventViewer : Page
    {
        Entities entities = new Entities();
        DispatcherTimer timer = new DispatcherTimer();
		List<string> galleryImages = new List<string>();
		int currentImageIndex = 0;
		public EventViewer()
        {
            InitializeComponent();
            animGrid.InitSlideUp();
            LoadInfo();
            AdminCheck();
			GalleryCheck();
        }
		private void GalleryCheck()
		{
			entities = new Entities();
			var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
			timer.Stop();
			galleryImages.Clear();
			foreach (var item in entities.EventGallery)
            {
                if (item.EventId == selectedEvent.Id)
                {
                    galleryImages.Add(item.Image);
                }
            }
			if (galleryImages.Count == 0)
			{
				galleryGrid.Visibility = Visibility.Collapsed;

				var binding = new Binding("ActualWidth")
				{
					ElementName = "grid", // или Source = grid, если есть ссылка на объект
					Mode = BindingMode.OneWay,
					Converter = (IValueConverter)FindResource("Sub20WidthConverter")
				};
				stackPanelInfo.SetBinding(FrameworkElement.WidthProperty, binding);
			}
			if (galleryImages.Count > 0)
			{
				galleryGrid.Visibility = Visibility.Visible;
				var binding = new Binding("ActualWidth")
				{
					ElementName = "grid", // или Source = grid, если есть ссылка на объект
					Mode = BindingMode.OneWay,
					Converter = (IValueConverter)FindResource("DivBy5Mul2Converter")
				};
				stackPanelInfo.SetBinding(FrameworkElement.WidthProperty, binding);
				ShowImageWithAnimation(currentImageIndex);
				InitializeTimer();
			}
		}
		private void ShowImageWithAnimation(int index)
		{
			if (galleryImages.Count == 0) return;

			var newImage = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + galleryImages[index])),
				Stretch = Stretch.UniformToFill,
				Opacity = 0
			};

			var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.7));
			var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.7));

			var oldImage = galleryBorder.Background as ImageBrush;

			galleryBorder.Background = newImage;
			newImage.BeginAnimation(Brush.OpacityProperty, fadeIn);

			if (oldImage != null)
			{
				oldImage.BeginAnimation(Brush.OpacityProperty, fadeOut);
			}

			currentImageIndex = index;
		}
		private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
		private void ShowNextImage()
		{
			if (galleryImages.Count == 0) return;

			int nextIndex = (currentImageIndex + 1) % galleryImages.Count;
			ShowImageWithAnimation(nextIndex);
		}
		private void ShowPreviousImage()
		{
			if (galleryImages.Count == 0) return;

			int prevIndex = (currentImageIndex - 1 + galleryImages.Count) % galleryImages.Count;
			ShowImageWithAnimation(prevIndex);
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			ShowNextImage();
		}
		private void LoadInfo()
        {
            try
            {
				entities = new Entities();
                var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
                bunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg));
                eventName.Text = selectedEvent.EventName;
                eventDate.Text = selectedEvent.EventDate.Value.ToString("yyyy");
				eventDateTime.Text = selectedEvent.EventDate.Value.ToString("dd MMMM HH:mm").ToLower();
				eventType.Text = selectedEvent.EventType.ToString();
                eventLocation.Text = selectedEvent.Location;
                eventDesc.Text = selectedEvent.EventDesc;
                
                if (selectedEvent.Price == 0)
                {
                    buttonBuy.Visibility = Visibility.Collapsed;
                    textBlockNoBuy.Visibility = Visibility.Visible;
                }
                else if (selectedEvent.TicketsRemain == 0)
                {
                    buttonBuy.Visibility = Visibility.Collapsed;
                    textBlockNoBuy.Text = "Билеты закончились";
                }
                if (selectedEvent.EventDate < DateTime.Now)
                {
                    buttonBuy.Visibility = Visibility.Collapsed;
                    textBlockNoBuy.Text = "Событие завершено";
                }
				GalleryCheck();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AdminCheck()
        {
			if (DataHolder.SharedRoleId != 1)
			{
                adminPanel.Visibility = Visibility.Collapsed;
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
            EventEditorWindow window = new EventEditorWindow();
			window.Closed += Editor_Closed;
            Manager.DialogOverlay.Visibility = Visibility.Visible;
			window.ShowDialog();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var deleteEvent = entities.Events.Find(DataHolder.SharedEventId);
            PopupManager.ShowConfirm("Вы точно хотите удалить это событие?", result =>
            {
                if (result)
                {
                    entities.Events.Remove(deleteEvent);
			        entities.SaveChanges();
			        Manager.MainFrame.GoBack();
                }
                else
                {
                    return;
                }
			});
		}
		private void ButtonBuyTicket(object sender, RoutedEventArgs e)
		{
			var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
			if (Settings.Default.loggedInUser == 0)
            {
                PopupManager.ShowMessage("Для покупки билета необходимо авторизоваться");
                return;
            }
            TicketWindow window = new TicketWindow();
            Manager.DialogOverlay.Visibility = Visibility.Visible;
            window.Closed += TicketWindow_Closed;
            window.ShowDialog();


		}
		private void Editor_Closed(object sender, EventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
			LoadInfo();
		}
		private void TicketWindow_Closed(object sender, EventArgs e)
		{
			Manager.DialogOverlay.Visibility = Visibility.Collapsed;
			LoadInfo();
		}

		private void Button_Prev(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			timer.Start();
			ShowPreviousImage();
		}

		private void Button_Next(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			timer.Start();
			ShowNextImage();
		}
	}
}
