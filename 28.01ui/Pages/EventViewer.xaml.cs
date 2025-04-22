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

namespace _28._01ui.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventViewer.xaml
    /// </summary>
    public partial class EventViewer : Page
    {
        Entities entities = new Entities();
        public EventViewer()
        {
            InitializeComponent();
            animGrid.InitSlideUp();
            LoadInfo();
            
        }
        private void LoadInfo()
        {
            try
            {
                var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
                bunner.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + selectedEvent.EventImg));
                textBlockName.Text = selectedEvent.EventName;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{

        }

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
