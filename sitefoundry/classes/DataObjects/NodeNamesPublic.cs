using System;

namespace Dury.SiteFoundry
{
	public class NodeNamesPublic
	{
		private int id;
		private int nodeID;
		private string lang;
		private string name;

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
		}

		public string Lang
		{
			get { return this.lang; }
			set { this.lang = value; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

	}
}
