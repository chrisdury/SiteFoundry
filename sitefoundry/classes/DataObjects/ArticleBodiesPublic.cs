using System;

namespace Dury.SiteFoundry
{
	public class ArticleBodiesPublic
	{
		private int id;
		private int articleID;
		private int templateID;
		private string lang;
		private int pageNumber;
		private string title;
		private string summary;
		private string keyword;
		private string body;
		private int createdBy;
		private int editedBy;
		private int statusID;
		private bool publish;
		private DateTime dateCreated;
		private DateTime dateModified;

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public int ArticleID
		{
			get { return this.articleID; }
			set { this.articleID = value; }
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

		public int PageNumber
		{
			get { return this.pageNumber; }
			set { this.pageNumber = value; }
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		public string Summary
		{
			get { return this.summary; }
			set { this.summary = value; }
		}

		public string Keyword
		{
			get { return this.keyword; }
			set { this.keyword = value; }
		}

		public string Body
		{
			get { return this.body; }
			set { this.body = value; }
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

		public int StatusID
		{
			get { return this.statusID; }
			set { this.statusID = value; }
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

	}
}
