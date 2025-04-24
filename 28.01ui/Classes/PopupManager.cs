using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace _28._01ui.Classes
{
    public static class PopupManager
    {
		public static Popup WarnPopup { get; private set; }

		public static void Initialize(Popup popup)
		{
			WarnPopup = popup;
		}

		public static void ShowMessage(string text)
		{
			if (WarnPopup?.Child is Border border &&
				border.Child is Grid grid &&
				grid.Children.OfType<TextBlock>().FirstOrDefault() is TextBlock textBlock)
			{
				textBlock.Text = text;
				WarnPopup.IsOpen = true;
			}
		}
	}
}
