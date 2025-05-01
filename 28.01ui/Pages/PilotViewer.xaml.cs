using _28._01ui.EditorWindows;
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
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace _28._01ui.Pages
{
    public partial class PilotViewer : Page
    {
		bool car = false;
		Entities entities = new Entities();
        public PilotViewer()
        {
            InitializeComponent();
			CarCheck();
			AdminCheck();
        }

		private void AdminCheck()
		{
			if (DataHolder.SharedRole != "Admin")
			{
				adminPanel.Visibility = Visibility.Collapsed;
				buttonAddCar.Visibility = Visibility.Collapsed;
				adminPanelEditCar.Visibility = Visibility.Collapsed;
			}
		}
		private void LoadData()
		{
			
		}
		private void CarCheck()
		{
			if (car)
			{
				buttonAddCar.Visibility = Visibility.Collapsed;
				borderCarInfo.Visibility = Visibility.Visible;
			}
			else
			{
				buttonAddCar.Visibility = Visibility.Visible;
				borderCarInfo.Visibility = Visibility.Collapsed;
			}
		}

		private void btnEdit_Click(object sender, RoutedEventArgs e)
		{
			PilotEditorWindow window = new PilotEditorWindow();
			window.Closed += Editor_Closed;
			window.ShowDialog();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnEditCar_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnDeleteCar_Click(object sender, RoutedEventArgs e)
		{
			car = false;
			CarCheck();
		}

		private void buttonAddCar_Click(object sender, RoutedEventArgs e)
		{
			CarEditorWindow window = new CarEditorWindow();
			window.Closed += Editor_Closed;
			car = true;
			window.ShowDialog();
		}
		private void Editor_Closed(object sender, EventArgs e)
		{
			CarCheck();
		}
	}
}
