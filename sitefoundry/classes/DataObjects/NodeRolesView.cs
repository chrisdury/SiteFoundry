using System;

namespace Dury.SiteFoundry
{
	public class NodeRolesView
	{
		private int id;
		private string name;
		private int permissionLevel;
		private int nodeID;

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public int PermissionLevel
		{
			get { return this.permissionLevel; }
			set { this.permissionLevel = value; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
		}

	}
}
