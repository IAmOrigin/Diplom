using _28._01ui.Properties;
using System;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using Page = System.Windows.Controls.Page;
using Border = System.Windows.Controls.Border;

namespace _28._01ui
{
	/// <summary>
	/// Логика взаимодействия для ProfilePage.xaml
	/// </summary>
	public partial class ProfilePage : Page
	{
		Entities entities = new Entities();
		public ProfilePage()
		{
			InitializeComponent();
				var loggedUser = entities.Users.Find(Settings.Default.loggedInUser);
			if (loggedUser.Role == "Admin")
			{
				profileimg.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"images\ProfileImages\admin.jpg"));
			}
			else
			{
				profileimg.Source = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + @"images\ProfileImages\user.jpg"));
			}
			TBlockName.Text = loggedUser.Name;
			TBlockRole.Text = loggedUser.Role;

			TicketContainer.Children.Clear();

			foreach (var ticket in entities.Ticket)
			{
				if (ticket.IdUser == Settings.Default.loggedInUser && ticket.Events.EventDate >= DateTime.Now)
				{
					var border = new Border
					{
						Width = 260,
						Height = 380,
						CornerRadius = new CornerRadius(8),
						Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF313131")),
						Margin = new Thickness(10)
					};

					var grid = new Grid();
					grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(115) });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

					var imageBorder = new Border
					{
						CornerRadius = new CornerRadius(8, 8, 0, 0),
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch
					};

					var imageBrush = new ImageBrush
					{

						ImageSource = new BitmapImage(new Uri(ProjectDirectory.GetProjectDirectory() + ticket.Events.EventImg)),
						Stretch = Stretch.UniformToFill
					};

					imageBorder.Background = imageBrush;
					Grid.SetRow(imageBorder, 0);
					grid.Children.Add(imageBorder);

					var eventNameTextBlock = new TextBlock
					{
						Text = ticket.Events.EventName,
						FontSize = 20,
						TextWrapping = TextWrapping.Wrap,
						FontWeight = FontWeights.Bold,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Center,
						Margin = new Thickness(5)
					};
					Grid.SetRow(eventNameTextBlock, 1);
					grid.Children.Add(eventNameTextBlock);

					var cityTextBlock = new TextBlock
					{
						Text = ticket.Events.Location,
						TextWrapping = TextWrapping.Wrap,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Left,
						Margin = new Thickness(5, 0, 5, 0)
					};
					Grid.SetRow(cityTextBlock, 2);
					grid.Children.Add(cityTextBlock);

					var dateTextBlock = new TextBlock
					{
						Text = ticket.Events.EventDate.ToString(),
						TextWrapping = TextWrapping.Wrap,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Left,
						Margin = new Thickness(5, 0, 5, 0)
					};
					Grid.SetRow(dateTextBlock, 3);
					grid.Children.Add(dateTextBlock);

					var holderTextBlock = new TextBlock
					{
						Text = ticket.Name,
						TextWrapping = TextWrapping.Wrap,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Left,
						Margin = new Thickness(5, 0, 5, 0)
					};
					Grid.SetRow(holderTextBlock, 4);
					grid.Children.Add(holderTextBlock);

					var phoneTextBlock = new TextBlock
					{
						Text = ticket.Phone,
						TextWrapping = TextWrapping.Wrap,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Left,
						Margin = new Thickness(5, 0, 5, 0)
					};
					Grid.SetRow(phoneTextBlock, 5);
					grid.Children.Add(phoneTextBlock);

					var priceTextBlock = new TextBlock
					{
						Text = ticket.Price.ToString() + " рублей",
						TextWrapping = TextWrapping.Wrap,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Bottom,
						Margin = new Thickness(5)
					};
					Grid.SetRow(priceTextBlock, 6);
					grid.Children.Add(priceTextBlock);

					border.Child = grid;
					TicketContainer.Children.Add(border);
				}
			}
		}
		private void LogOut(object sender, RoutedEventArgs e)
		{	
			Settings.Default.loggedInUser = 0;
			Settings.Default.Save();
			Manager.MainFrame.Navigate(new Uri("StartPage.xaml", UriKind.Relative));
		}

		private void btnBack(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new Uri("StartPage.xaml", UriKind.Relative));
		}

		private void Btndoc(object sender, RoutedEventArgs e)
		{
			ExportTicketsToWord(entities.Ticket);
		}

		public void ExportTicketsToWord(IEnumerable<Ticket> tickets)
		{
			// Создаем экземпляр приложения Word
			Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
			Document wordDoc = wordApp.Documents.Add();

			// Добавляем заголовок
			Microsoft.Office.Interop.Word.Paragraph title = wordDoc.Content.Paragraphs.Add();
			title.Range.Text = "Чек об оплате";
			title.Range.Font.Bold = 1;
			title.Range.Font.Size = 16;
			title.Range.InsertParagraphAfter();

			// Перебираем все билеты и добавляем их в документ
			foreach (var ticket in tickets)
			{
				if (ticket.IdUser == Settings.Default.loggedInUser && ticket.Events.EventDate >= DateTime.Now)
				{
					// Добавляем информацию о билете
					Microsoft.Office.Interop.Word.Paragraph ticketInfo = wordDoc.Content.Paragraphs.Add();

					ticketInfo.Range.Text = $"Уникальный идентификатор билета: {ticket.Id}";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Название мероприятия: {ticket.Events.EventName} ({ticket.IdEvent})";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Место проведения: {ticket.Events.Location}";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Дата мероприятия: {ticket.Events.EventDate}";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Аккаунт: {ticket.Users.Name} ({ticket.IdUser})";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Держатель билета: {ticket.Name}";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Номер телефона: {ticket.Phone}";
					ticketInfo.Range.InsertParagraphAfter();

					ticketInfo.Range.Text = $"Оплачено: {ticket.Price} рублей";
					ticketInfo.Range.InsertParagraphAfter();

					// Добавляем пустую строку для разделения билетов
					ticketInfo.Range.Text = "";
					ticketInfo.Range.InsertParagraphAfter();
				}
			}

			// Сохраняем документ
			string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Tickets.docx");
			wordDoc.SaveAs2(filePath);

			// Закрываем документ и приложение Word
			wordDoc.Close();
			wordApp.Quit();

			// Освобождаем ресурсы
			System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

			MessageBox.Show($"Документ успешно сохранен: {filePath}");
		}

	}
}
