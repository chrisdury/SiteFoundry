using System;

namespace Dury.SiteFoundry
{
	public class ArticleContainersPublic
	{
		private int id;
		private int nodeID;
		private int typeID;
		private string name;
		private bool publish;
		private DateTime dateCreated;
		private DateTime dateModified;
		private DateTime dateExpire;

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

		public int TypeID
		{
			get { return this.typeID; }
			set { this.typeID = value; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public bool Publish
		{
			get { return this.publish; }
			set { this.publish = value; }
		}

		public DateTime DateCreated
		{
			get { return this.dateCreated; }
			set { this.dateCreated = value; }
		}

		public DateTime DateModified
		{
			get { return this.dateModified; }
			set { this.dateModified = value; }
		}

		public DateTime DateExpire
		{
			get { return this.dateExpire; }
			set { this.dateExpire = value; }
		}

	}
}
