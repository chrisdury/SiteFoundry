using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Node
	{
		private int id;
		private int parentID;
		private string filename;
		private int typeID;
		private bool publish;
		private bool visible;
		private bool visibleMenu;
		private bool visibleSubMenu;
		private int rank;
		private DateTime dateCreated;
		private DateTime dateModified;
		private IList advancedArticleNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList linkNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList nodeNameNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList securityNodeNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList simpleArticleNodeIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList

		public int Id
		{
			get { return this.id; }
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

		public int TypeID
		{
			get { return this.typeID; }
			set { this.typeID = value; }
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

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
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

		public IList AdvancedArticleNodeIDs
		{
			get { return this.advancedArticleNodeIDs; }
		}

		public IList LinkNodeIDs
		{
			get { return this.linkNodeIDs; }
		}

		public IList NodeNameNodeIDs
		{
			get { return this.nodeNameNodeIDs; }
		}

		public IList SecurityNodeNodeIDs
		{
			get { return this.securityNodeNodeIDs; }
		}

		public IList SimpleArticleNodeIDs
		{
			get { return this.simpleArticleNodeIDs; }
		}

	}
}
