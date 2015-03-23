using System;

using Dury.SiteFoundry;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Base article object to handle mulitple templates for individual articles
	/// </summary>
	public class BaseArticle : Dury.SiteFoundry.UI.BaseControl
	{
		public string ArticleTitle;
		public string ArticleBody;
		public string Summary;
		public string Keywords;
		public DateTime LastModified;
		public int TemplateID;
		public string Lang;
		public string[] ArticlePageTitles;

		public BaseArticle()
		{
		}

	}
}
