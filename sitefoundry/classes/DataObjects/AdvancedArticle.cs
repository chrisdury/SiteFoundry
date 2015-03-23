using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class AdvancedArticle
	{
		private int id;
		private int nodeID;
		private int templateID;
		private string lang;
		private int version;
		private int rank;
		private string title;
		private string keyword;
		private string summary;
		private string body;
		private int userID;
		private bool active;
		private bool publish;
		private bool preview;
		private DateTime dateCreated;
		private DateTime dateModified;
		private ObjectHolder nodeIDObject; // Strongly Type as Node if not Lazy-Loading
		private ObjectHolder userIDObject; // Strongly Type as SecurityUser if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
		}

		public int TemplateID
		{
			get { return this.templateID; }
			set { this.templateID = value; }
		}

		public string Lang
		{
			get { return this.lang; }
			set { this.lang = value; }
		}

		public int Version
		{
			get { return this.version; }
			set { this.version = value; }
		}

		public int Rank
		{
			get { return this.rank; }
			set { this.rank = value; }
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public string Keyword
		{
			get { return this.keyword; }
			set { this.keyword = value; }
		}

		public string Summary
		{
			get { return this.summary; }
			set { this.summary = value; }
		}

		public string Body
		{
			get { return this.body; }
			set { this.body = value; }
		}

		public int UserID
		{
			get { return this.userID; }
			set { this.userID = value; }
		}

		public bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		public bool Publish
		{
			get { return this.publish; }
			set { this.publish = value; }
		}

		public bool Preview
		{
			get { return this.preview; }
			set { this.preview = value; }
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

		// Return the primary key property from the primary key object
		public Node NodeIDObject
		{
			get { return (Node)this.nodeIDObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public SecurityUser UserIDObject
		{
			get { return (SecurityUser)this.userIDObject.InnerObject; }
		}

	}
}
