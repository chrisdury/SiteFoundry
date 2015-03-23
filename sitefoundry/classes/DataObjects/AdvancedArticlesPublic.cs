using System;

namespace Dury.SiteFoundry
{
	public class AdvancedArticlesPublic
	{
		private int id;
		private int nodeID;
		private int templateID;
		private string lang;
		private int rank;
		private string title;
		private string keyword;
		private string summary;
		private string body;
		private int userID;
		private DateTime dateCreated;
		private DateTime dateModified;

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
