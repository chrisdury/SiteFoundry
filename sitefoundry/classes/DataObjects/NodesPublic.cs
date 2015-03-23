using System;

namespace Dury.SiteFoundry
{
	public class NodesPublic
	{
		private int id;
		private int parentID;
		private string filename;
		private int nodeTypeID;
		private int rank;
		private bool publish;
		private bool visible;
		private bool visibleMenu;
		private bool visibleSubMenu;
		private DateTime dateCreated;
		private DateTime dateModified;

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public int ParentID
		{
			get { return this.parentID; }
			set { this.parentID = value; }
		}

		public string Filename
		{
			get { return this.filename; }
			set { this.filename = value; }
		}

		public int NodeTypeID
		{
			get { return this.nodeTypeID; }
			set { this.nodeTypeID = value; }
		}

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
		}

		public bool Publish
		{
			get { return this.publish; }
			set { this.publish = value; }
		}

		public bool Visible
		{
			get { return this.visible; }
			set { this.visible = value; }
		}

		public bool VisibleMenu
		{
			get { return this.visibleMenu; }
			set { this.visibleMenu = value; }
		}

		public bool VisibleSubMenu
		{
			get { return this.visibleSubMenu; }
			set { this.visibleSubMenu = value; }
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

	}
}
