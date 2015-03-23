using System;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class NodeName
	{
		private int id;
		private int nodeID;
		private string lang;
		private string name;
		private ObjectHolder nodeIDObject; // Strongly Type as Node if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
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

		// Return the primary key property from the primary key object
		public Node NodeIDObject
		{
			get { return (Node)this.nodeIDObject.InnerObject; }
		}

	}
}
