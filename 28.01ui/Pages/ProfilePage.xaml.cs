using _28._01ui.Properties;
using Microsoft.Office.Interop.Word;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using Border = System.Windows.Controls.Border;
using MessageBox = System.Windows.MessageBox;
using Page = System.Windows.Controls.Page;
using TextBlock = System.Windows.Controls.TextBlock;

namespace _28._01ui
{
	public partial class ProfilePage : Page
	{
		Entities entities = new Entities();
		public ProfilePage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			var loggedUser = entities.Users.Find(Settings.Default.loggedInUser);
			if (loggedUser.IdRole == 1)
			{
				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"images\ProfileImages\admin.jpg")),
					Stretch = Stretch.UniformToFill
				};
				avaborder.Background = imageBrush;
				TBlockRole.Text = "Администратор";
			}
			else
			{
				var imageBrush = new ImageBrush
				{
					ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"images\ProfileImages\user.jpg")),
					Stretch = Stretch.UniformToFill
				};
				avaborder.Background = imageBrush;
				TBlockRole.Text = "Пользователь";
			}
			TBlockName.Text = loggedUser.Name;

			bool hasTickets = entities.Ticket.Any(ticket =>
				ticket.IdUser == Settings.Default.loggedInUser &&
				ticket.Events.EventDate >= DateTime.Now);
			if (hasTickets)
			{
				noneticket.Text = "";
				ticketsinfo.Text = "Билеты";
				animGrid1.ContentSlideUp();
			}
			else
			{
				noneticket.Text = "У вас нет билетов";
				ticketsinfo.Text = "";
			}
			foreach(var ticket in entities.Ticket)
			{
				if (ticket.IdUser == Settings.Default.loggedInUser && ticket.Events.EventDate >= DateTime.Now)
				{
					ticketContainer.Items.Add(ticket);
				}
			}
		}
		

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			
		}
		private void ExportButton_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void btnLogOut_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.loggedInUser = 0;
			Settings.Default.Save();
			Manager.MainFrame.Navigate(new StartPage());
		}

		public void ExportTicketToWord(Ticket ticket)
		{
			if (ticket.IdUser == Settings.Default.loggedInUser && ticket.Events.EventDate >= DateTime.Now)
			{
				// Создаем экземпляр приложения Word
				Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
				Document wordDoc = wordApp.Documents.Add();

				// Создаем таблицу для чека
				Microsoft.Office.Interop.Word.Table table = wordDoc.Tables.Add(wordDoc.Content, 7, 2); // 7 строк, 2 столбца
				table.Borders.Enable = 1; // Включаем границы таблицы

				// Заполняем таблицу данными
				table.Cell(1, 1).Range.Text = "FastLane Manager";
				table.Cell(1, 1).Range.Font.Bold = 1;
				table.Cell(1, 1).Range.Font.Size = 14;
				table.Cell(1, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
				table.Rows[1].Cells.Merge(); // Объединяем ячейки для заголовка

				table.Cell(2, 1).Range.Text = $"Номер чека: {ticket.Id}";
				table.Cell(3, 1).Range.Text = $"Профиль: {ticket.IdUser} ({ticket.Users.Name})";
				table.Cell(4, 1).Range.Text = $"Дата: {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}";

				table.Cell(5, 1).Range.Text = $"Мероприятие: {ticket.Events.EventName} ({ticket.IdEvent})";
				table.Rows[5].Cells.Merge(); // Объединяем ячейки для названия мероприятия

				table.Cell(6, 1).Range.Text = "Итого:";
				table.Cell(6, 2).Range.Text = $"{ticket.Price} ₽";
				table.Cell(6, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

				table.Cell(7, 1).Range.Text = "Спасибо за покупку! Ждем Вас на мероприятии!";
				table.Rows[7].Cells.Merge(); // Объединяем ячейки для финального сообщения
				table.Cell(7, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

				// Сохраняем документ
				string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket.docx");
				wordDoc.SaveAs2(filePath);

				// Закрываем документ и приложение Word
				wordDoc.Close();
				wordApp.Quit();

				// Освобождаем ресурсы
				System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
				System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

				// Открываем файл после сохранения
				try
				{
					System.Diagnostics.Process.Start(filePath);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
				}
			}
			else
			{
				MessageBox.Show("Билет не подходит для экспорта.");
			}
		}

		private void ButtonEdit(object sender, RoutedEventArgs e)
		{

        }
    }
}
