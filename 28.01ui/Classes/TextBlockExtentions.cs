using System.Threading.Tasks;
using System.Windows.Controls;


namespace _28._01ui
{
	public static class TextBlockExtentions
	{
		public static async void SetTextWithDelay(this TextBlock textBlock, string text, int delayMilliseconds = 5000)
		{
			textBlock.Text = text;
			await Task.Delay(delayMilliseconds);
			textBlock.Text = string.Empty;
		}
	}
}
