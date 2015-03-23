using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class Link
	{
		private int id;
		private int nodeID;
		private string uRL;
		private bool publish;
		private DateTime dateCreated;
		private DateTime dateModified;
		private int createdBy;
		private int editedBy;
		private ObjectHolder nodeIDObject; // Strongly Type as Node if not Lazy-Loading
		private ObjectHolder createdByObject; // Strongly Type as SecurityUser if not Lazy-Loading
		private ObjectHolder editedByObject; // Strongly Type as SecurityUser if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
		}

		public string URL
		{
			get { return this.uRL; }
			set { this.uRL = value; }
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

		public int CreatedBy
		{
			get { return this.createdBy; }
			set { this.createdBy = value; }
		}

		public int EditedBy
		{
			get { return this.editedBy; }
			set { this.editedBy = value; }
		}

		// Return the primary key property from the primary key object
		public Node NodeIDObject
		{
			get { return (Node)this.nodeIDObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public SecurityUser CreatedByObject
		{
			get { return (SecurityUser)this.createdByObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public SecurityUser EditedByObject
		{
			get { return (SecurityUser)this.editedByObject.InnerObject; }
		}

	}
}
