using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Controls;
using TextBlock = System.Windows.Controls.TextBlock;

namespace _28._01ui.Classes
{
	public static class PopupManager
	{
		public static Popup WarnPopup { get; private set; }
		public static Popup ConfirmPopup { get; private set; }

		public static void Initialize(Popup warnPopup, Popup confirmPopup)
		{
			WarnPopup = warnPopup;
			ConfirmPopup = confirmPopup;
		}

		public static void ShowMessage(string message, string title = "Уведомление", int autoCloseDelay = 5)
		{
			if (WarnPopup == null) return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				var headerTextBlock = WarnPopup.FindName("popupHeaderTextBlock") as TextBlock;
				var messageTextBlock = WarnPopup.FindName("popupTextBlock") as TextBlock;

				if (headerTextBlock != null) headerTextBlock.Text = title;
				if (messageTextBlock != null) messageTextBlock.Text = message;

				WarnPopup.IsOpen = true;
			});
		}

		public static void ShowConfirm(string message, Action<bool> callback, string yesText = "Да", string noText = "Нет")
		{
			if (ConfirmPopup == null) return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				var textQuestion = ConfirmPopup.FindName("textQuestion") as TextBlock;
				var yesButton = ConfirmPopup.FindName("yesButton") as Button;
				var noButton = ConfirmPopup.FindName("noButton") as Button;

				if (textQuestion != null) textQuestion.Text = message;

				if (yesButton != null)
				{
					yesButton.Content = yesText;
					yesButton.Click -= OnYesClicked;
					yesButton.Click += OnYesClicked;
				}

				if (noButton != null)
				{
					noButton.Content = noText;
					noButton.Click -= OnNoClicked;
					noButton.Click += OnNoClicked;
				}

				void OnYesClicked(object sender, RoutedEventArgs e)
				{
					ConfirmPopup.IsOpen = false;
					callback?.Invoke(true);
					CleanupEvents();
				}

				void OnNoClicked(object sender, RoutedEventArgs e)
				{
					ConfirmPopup.IsOpen = false;
					callback?.Invoke(false);
					CleanupEvents();
				}

				void CleanupEvents()
				{
					if (yesButton != null) yesButton.Click -= OnYesClicked;
					if (noButton != null) noButton.Click -= OnNoClicked;
				}

				ConfirmPopup.IsOpen = true;

			//	PopupManager.ShowConfirm("Вы действительно хотите удалить этот элемент?", result =>
			//	{
			//		if (result)
			//		{
			//			// Действие при подтверждении
			//			MessageBox.Show("Подтверждение");
			//		}
			//		else
			//		{
			//			// Действие при отмене
			//			MessageBox.Show("Отмена");
			//		}
			//	},
			//	"Да", "Нет");
			//}

			});
		}
	}
}

