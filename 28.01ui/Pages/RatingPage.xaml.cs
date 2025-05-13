using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;

namespace _28._01ui
{
	public partial class RatingPage : Page
	{
		Entities entities = new Entities();
		public RatingPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			foreach (var item in entities.Results)
			{
				dataGridResults.Items.Add(item);
			}
		}


		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
