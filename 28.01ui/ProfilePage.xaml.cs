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
			if (loggedUser.Role == "Admin")
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
			TicketContainer.Children.Clear();
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
			foreach (var ticket in entities.Ticket)
			{
				if (ticket.IdUser == Settings.Default.loggedInUser && ticket.Events.EventDate >= DateTime.Now)
				{
					Border border = new Border
					{
						Height = 270,
						Width = 600,
						VerticalAlignment = VerticalAlignment.Top,
						HorizontalAlignment = HorizontalAlignment.Left,
						Margin = new Thickness(0,0,15,15),
						Background = new ImageBrush
						{
							ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + ticket.Events.EventImg, UriKind.RelativeOrAbsolute)),
							Stretch = Stretch.UniformToFill
						}
					};
					// Создаем Grid для размещения текстовых блоков
					Grid grid = new Grid
					{
						Width = 280,
						HorizontalAlignment = HorizontalAlignment.Left,
						Background = new SolidColorBrush(Color.FromArgb(175, 0, 0, 0)) // #AF000000
					};
					// Добавляем строки в Grid
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
					// Текстовый блок для названия события
					TextBlock eventNameTextBlock = new TextBlock
					{
						Margin = new Thickness(10, 0, 0, 0),
						TextWrapping = TextWrapping.Wrap,
						FontSize = 32,
						FontWeight = FontWeights.Bold,
						Text = ticket.Events.EventName,
						Foreground = Brushes.White
					};
					Grid.SetRow(eventNameTextBlock, 0);
					grid.Children.Add(eventNameTextBlock);
					TextBlock locationTextBlock = new TextBlock
					{
						Margin = new Thickness(10, 10, 0, 0),
						Text = ticket.Events.Location,
						FontWeight = FontWeights.Medium,
						FontSize = 16,
						Foreground = Brushes.White
					};
					Grid.SetRow(locationTextBlock, 1);
					grid.Children.Add(locationTextBlock);
					// Текстовый блок для "Держатель билета"
					TextBlock holderTextBlock = new TextBlock
					{
						Margin = new Thickness(10, 5, 0, 0),
						Text = ticket.Name,
						FontSize = 16,
						Foreground = Brushes.White
					};
					Grid.SetRow(holderTextBlock, 2);
					grid.Children.Add(holderTextBlock);
					// Текстовый блок для номера билета
					TextBlock ticketNumberTextBlock = new TextBlock
					{
						Margin = new Thickness(10, 5, 0, 0),
						FontSize = 16,
						Text = ticket.Phone,
						Foreground = Brushes.White
					};
					Grid.SetRow(ticketNumberTextBlock, 3);
					grid.Children.Add(ticketNumberTextBlock);
					// StackPanel для даты и времени
					StackPanel stackPanel = new StackPanel
					{
						Orientation = Orientation.Vertical,
						Margin = new Thickness(0, 0, 10, 10),
						VerticalAlignment = VerticalAlignment.Bottom,
						HorizontalAlignment = HorizontalAlignment.Right
					};
					// Текстовый блок для даты
					TextBlock dateTextBlock = new TextBlock
					{
						HorizontalAlignment = HorizontalAlignment.Right,
						FontSize = 18,
						FontWeight = FontWeights.SemiBold,
						Text = ticket.Events.EventDate?.ToString("dd MMMM", new CultureInfo("ru-RU")),
						Foreground = Brushes.White
					};
					stackPanel.Children.Add(dateTextBlock);
					// Текстовый блок для времени
					TextBlock timeTextBlock = new TextBlock
					{
						HorizontalAlignment = HorizontalAlignment.Center,
						FontSize = 26,
						FontWeight = FontWeights.Bold,
						Text = ticket.Events.EventDate?.ToString("HH:mm"),
						Foreground = Brushes.White
					};
					stackPanel.Children.Add(timeTextBlock);
					TextBlock yearTextBlock = new TextBlock
					{
						HorizontalAlignment = HorizontalAlignment.Center,
						FontSize = 12,
						FontWeight = FontWeights.Light,
						Text = ticket.Events.EventDate?.ToString("yyyy"),
						Foreground = Brushes.White
					};
					stackPanel.Children.Add(yearTextBlock);
					StackPanel stackPanelButtons = new StackPanel
					{
						Orientation = Orientation.Horizontal,
						Margin = new Thickness(0, 0, 10, 10),
						HorizontalAlignment = HorizontalAlignment.Left,
						VerticalAlignment = VerticalAlignment.Bottom,
					};
					var deletebutton = new Wpf.Ui.Controls.Button
					{
						Appearance = ControlAppearance.Transparent,
						Tag = ticket.Id,
						Content = "Возврат",
						Name = "delete",
						BorderThickness = new Thickness(0),
						Margin = new Thickness(10,0,0,0),
					};
					deletebutton.Click += (sender, e) =>
					{
						var button = sender as Wpf.Ui.Controls.Button;
						if (button != null)
						{
							int ticketId = (int)button.Tag;
							var delticket = entities.Ticket.Find(ticketId);
							var ticketEvent = entities.Events.Find(delticket.IdEvent);
							ticketEvent.TicketsRemain += 1;
							entities.Ticket.Remove(delticket);
							TicketContainer.Children.Remove(border);
							entities.SaveChanges();
						}
					};
					var exportbutton = new Wpf.Ui.Controls.Button
					{
						Appearance = ControlAppearance.Transparent,
						Tag = ticket.Id,
						Content = "Чек",
						Name = "export",
						BorderThickness = new Thickness(0),
					};
					exportbutton.Click += (sender, e) =>
					{
						var button = sender as Wpf.Ui.Controls.Button;
						if (button != null)
						{
							int ticketId = (int)button.Tag;
							if (ticket != null)
							{
								// Экспортируем чек в Word
								ExportTicketToWord(ticket);
							}
							else
							{
								MessageBox.Show("Билет не найден.");
							}
						}
					};
					stackPanelButtons.Children.Add(deletebutton);
					stackPanelButtons.Children.Add(exportbutton);
					Grid.SetRowSpan(stackPanel, 5);
					Grid.SetRowSpan(stackPanelButtons, 5);
					grid.Children.Add(stackPanel);
					grid.Children.Add(stackPanelButtons);
					border.Child = grid;
					TicketContainer.Children.Add(border);
				}
			}
		}

		private void btnLogOut_Click(object sender, RoutedEventArgs e)
		{
			Settings.Default.loggedInUser = 0;
			Settings.Default.Save();
			Manager.MainFrame.Navigate(new Uri("StartPage.xaml", UriKind.Relative));
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
	}
}
