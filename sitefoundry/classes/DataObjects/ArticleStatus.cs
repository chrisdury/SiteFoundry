using System;

namespace Dury.SiteFoundry
{
	public class ArticleStatus
	{
		private int id;
		private string name;

		public int Id
		{
			get { return this.id; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

	}
}
