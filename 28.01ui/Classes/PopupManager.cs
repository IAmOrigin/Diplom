using System.Windows;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Controls;
using TextBlock = System.Windows.Controls.TextBlock;

namespace _28._01ui.Classes
{
	public static class PopupManager
	{
		public static Popup WarnPopup { get; private set; }

		public static void Initialize(Popup popup)
		{
			WarnPopup = popup;
		}

		public static void ShowMessage(string message, string title = "Уведомление", int autoCloseDelay = 5)
		{
			if (WarnPopup == null) return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				// Находим элементы по именам
				var headerTextBlock = WarnPopup.FindName("popupHeaderTextBlock") as TextBlock;
				var messageTextBlock = WarnPopup.FindName("popupTextBlock") as TextBlock;
				// Обновляем содержимое
				if (headerTextBlock != null) headerTextBlock.Text = title;
				if (messageTextBlock != null) messageTextBlock.Text = message;

				WarnPopup.IsOpen = true;
			});
		}
	}
}
