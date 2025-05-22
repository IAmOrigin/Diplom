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

namespace _28._01ui.Pages
{
    public partial class EventViewer : Page
    {
        Entities entities = new Entities();
        public EventViewer()
        {
            InitializeComponent();
            animGrid.InitSlideUp();
            LoadInfo();
            AdminCheck();
        }
        private void LoadInfo()
        {
            try
            {
                var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
                bunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg));
                eventName.Text = selectedEvent.EventName;
                eventDate.Text = selectedEvent.EventDate.Value.ToString("yyyy").ToLower();
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
	}
}
