using System;
using System.Collections;
using System.Xml;


namespace Dury.SiteFoundry.Article
{
	/// <summary>
	/// Summary description for ContentEditors.
	/// </summary>
	public class ContentEditors
	{
		private static readonly ContentEditors instance=new ContentEditors();
		public static ContentEditors GetInstance() { return instance; }
		static ContentEditors() {}
		ContentEditors()
		{
		}
		private static string file = SFGlobal.BaseDirectory + "nodeTemplates/articleContentEditors/editors.xml";

		private static ArrayList contentEditors;

		public static ArrayList GetContentEditorList()
		{
			if (contentEditors != null) return contentEditors;
			contentEditors = new ArrayList();
            XmlDocument xd = new XmlDocument();
			xd.Load(file);
			foreach(XmlNode x in xd["editors"])
			{
				ContentEditorInfo cei = new ContentEditorInfo(int.Parse(x.Attributes["id"].Value),x.Attributes["name"].Value,x.Attributes["src"].Value,int.Parse(x.Attributes["width"].Value),int.Parse(x.Attributes["height"].Value));
				contentEditors.Add(cei);
			}
			return contentEditors;
		}

		public static ContentEditorInfo GetContentEditorInfo(int id) 
		{
			foreach(ContentEditorInfo cei in contentEditors)
				if (cei.ID == id) return cei;
			throw new Exception("supplied ID not found in " + file);
		}
	}

	public struct ContentEditorInfo
	{
		public string Name;
		public int ID;
		public string Src;
		public int Height;
		public int Width;

		public ContentEditorInfo(int id, string name, string src, int width, int height)
		{
			ID = id;
			Name = name;
			Src = src;
			Width = width;
			Height = height;
		}

	}
}
