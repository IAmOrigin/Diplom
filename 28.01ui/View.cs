using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28._01ui
{
	public partial class Events 
	{
		public override string ToString()
		{
			return EventName;
		}
	}

	public partial class EventType 
	{
		public override string ToString()
		{
			return TypeName;
		}
	}

	public partial class Results 
	{
		public override string ToString()
		{
			return Pilots.PilotName + Pilots.Teams.TeamName + Points + Time + Pilots.Teams.TeamCountry;
		}
	}
	public partial class Teams 
	{
		public override string ToString()
		{
			return TeamName + TeamCountry + TeamLogo;
		}
	}
	public partial class Pilots 
	{
		public override string ToString()
		{
			return Id.ToString() + PilotRoles.NameRole + PilotName;
		}
	}

	public partial class PilotRoles
	{
		public override string ToString()
		{
			return NameRole;
		}
	}

	internal class Pic
	{
		public static String defaultpic { get; set; }
		public static String defaultprofilepic { get; set; }
	}
}
