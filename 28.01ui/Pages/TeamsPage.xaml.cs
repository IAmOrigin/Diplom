using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _28._01ui
{
	public partial class TeamsPage : Page
	{
		Entities entities = new Entities();
		public TeamsPage()
		{
			InitializeComponent();
			animGrid.InitSlideUp();
			foreach (var team in entities.Teams)
			{ 
				listviewteams.Items.Add(team);
			}
			if (DataHolder.SharedRole != "Admin")
			{
				btnAdd.Visibility = Visibility.Collapsed;
			}
		}

		private void TeamClick(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var team = button.DataContext as Teams;
			if (team == null) return;
			listviewteams.SelectedItem = team;
			DataHolder.SharedTeamId = team.Id;
			Manager.MainFrame.Navigate(new TeamViewer());
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			DataHolder.SharedTeamId = 0;
			Manager.MainFrame.Navigate(new TeamEditor());
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			string searchingText = SearchBox.Text.ToLower();
			listviewteams.Items.Clear();
			var filtername = entities.Teams.Where(s => s.TeamName.ToLower().Contains(searchingText)).ToList();
			foreach (var filterItem in filtername)
			{
				listviewteams.Items.Add(filterItem);
			}
			var filtercountry = entities.Teams.Where(s => s.TeamCountry.ToLower().Contains(searchingText)).ToList();
			foreach (var filterItem in filtercountry)
			{
				listviewteams.Items.Add(filterItem);
			}
		}
	}
}
