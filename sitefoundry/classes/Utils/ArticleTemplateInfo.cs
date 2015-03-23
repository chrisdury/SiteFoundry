using System;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Holder for Article template information
	/// </summary>
	public struct ArticleTemplateInfo
	{
		public string ID;
		public string Name;
		public string Src;
		public ArticleTemplateInfo(string i,string n, string s)
		{
			ID = i;
			Name = n;
			Src = s;
		}


	}
}
