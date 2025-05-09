using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace _28._01ui
{
	public partial class RatingPage : Page
	{
		Entities entities = new Entities();
		public RatingPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			
		}


		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
