using _28._01ui.Classes;
using Microsoft.Build.Logging;
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
using System.Windows.Shapes;

namespace _28._01ui.EditorWindows
{
    /// <summary>
    /// Логика взаимодействия для RegulationEditorWindow.xaml
    /// </summary>
    public partial class RegulationEditorWindow : Window
    {
		public string resultText { get; set; }
        public string resultDate { get; set; }
        Entities entities = new Entities();
		public RegulationEditorWindow(string regulation, string regulationDate)
        {
            InitializeComponent();
			resultText = regulation;
            resultDate = regulationDate;
            resultTextBox.Text = resultText;
            if (resultDate != null )
            {
                resultDatePicker.SelectedDate = Convert.ToDateTime(resultDate);
            }
        }

		private void buttonSave_Click(object sender, RoutedEventArgs e)
		{
            var selectedEvent = entities.Events.Find(DataHolder.SharedEventId);
            if (String.IsNullOrEmpty(resultTextBox.Text) || resultDatePicker.SelectedDate == default)
            {
                PopupManager.ShowMessage("Заполните описание и дату проведения техосмотра");
                return;
            }
            if (resultDatePicker.SelectedDate.Value < selectedEvent.EventDate.Value.Date.AddDays(-3))
            {
                PopupManager.ShowMessage("Дата проведения должна быть как не более чем за 3 суток до события");
                return;
            }
            if (resultDatePicker.SelectedDate.Value.Date >= selectedEvent.EventDate.Value.Date)
            {
                PopupManager.ShowMessage("Нельзя проводить техосмотр в день мероприятия или позже");
                return;
            }
            resultDate = resultDatePicker.SelectedDate.ToString();
			resultText = resultTextBox.Text;
			Close();
        }

		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
            Close();
		}
	}
}
