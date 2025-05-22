using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _28._01ui.TicketWindowFolder
{
    public partial class tPage2 : Page
    {
		Entities entities = new Entities();
		DispatcherTimer timer = new DispatcherTimer();
		Ticket ticket;
        public tPage2(Ticket ticket1)
        {
			InitializeComponent();
			ticket = ticket1;
			timer.Interval = TimeSpan.FromSeconds(3);
			timer.Tick += (s, e) =>
			{
				timer.Stop();
				Manager.TicketFrame.Navigate(new tPage3(ticket));
			};
			timer.Start();
		}
		private void Button_Cancel(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			var window = Window.GetWindow(this);
			window?.Close();
		}

		private void Button_Next(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			Manager.TicketFrame.Navigate(new tPage3(ticket));
		}
	}

}
