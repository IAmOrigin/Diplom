using _28._01ui.Classes;
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
                EventContainer.Items.Add(selectedEvent);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AdminCheck()
        {
			if (DataHolder.SharedRole != "Admin")
			{
                adminPanel.Visibility = Visibility.Collapsed;
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("Pages/EventEditor.xaml", UriKind.Relative));
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			var deleteEvent = entities.Events.Find(DataHolder.SharedEventId);
			var result = MessageBox.Show("Вы точно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
				return;
			entities.Events.Remove(deleteEvent);
			entities.SaveChanges();
			Manager.MainFrame.GoBack();
		}
		private void ButtonBuyTicket(object sender, RoutedEventArgs e)
		{
			if (Settings.Default.loggedInUser == 0)
            {
                PopupManager.ShowMessage("Для покупки билета необходимо авторизоваться");
            }
		}
	}
}
