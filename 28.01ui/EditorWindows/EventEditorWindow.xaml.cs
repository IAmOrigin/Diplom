using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using System.Windows.Shapes;

namespace _28._01ui.EditorWindows
{
    public partial class EventEditorWindow : Window
    {
        Entities entities = new Entities();
		int eventId;
        public EventEditorWindow(int id)
        {
            InitializeComponent();
			LoadData();
			eventId = id;
        }

		private void LoadData()
		{
			foreach (var type in entities.EventType)
			{
				comboType.Items.Add(type);
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
			if (ticketsRemain.Visibility == Visibility.Visible)
			{
				ticketsRemain.Visibility = Visibility.Collapsed;
				tiketPrice.Visibility = Visibility.Collapsed;
			}
			else
			{
				ticketsRemain.Visibility = Visibility.Visible;
				tiketPrice.Visibility = Visibility.Visible;
			}
		}

		private void buttonAddImg_Click(object sender, RoutedEventArgs e)
		{

		}

		private void buttonSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void buttonAddType_Click(object sender, RoutedEventArgs e)
		{
			EventTypeEditorWindow window = new EventTypeEditorWindow();
			window.ShowDialog();
		}
	}
}
