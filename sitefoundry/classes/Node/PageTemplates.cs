using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for PageTemplates.
	/// </summary>
	public class PageTemplates 
	{
		private static readonly PageTemplates instance=new PageTemplates();
		public static PageTemplates GetInstance() { return instance; }
		static PageTemplates() {}
		PageTemplates() {}


		private static ArrayList pageTemplatesList;
		public static ArrayList PageTemplatesList
		{
			get 
			{
				if (pageTemplatesList == null) 
				{
					XmlDocument xd = new XmlDocument();
					xd.Load(SFGlobal.BaseDirectory + "siteTemplates.config");
					System.Collections.ArrayList al = new System.Collections.ArrayList();
					foreach (XmlNode x in xd["siteTemplates"]) 
					{
						al.Add(new NodePageTemplate(int.Parse(x.Attributes["id"].Value),x.Attributes["name"].Value,x.Attributes["headerFile"].Value,x.Attributes["footerFile"].Value));
					}
					pageTemplatesList = al;
				}
				return pageTemplatesList;
			}
		}

		public static NodePageTemplate GetPageTemplate(int templateID) 
		{
			foreach(NodePageTemplate npt in PageTemplatesList) 
			{
				if (npt.TemplateID == templateID)
					return npt;
			}
			throw new DuryTools.ErrorHandler("cannot find supplied id {" + templateID + "} in siteTemplates.config");
		}

	}


	public struct NodePageTemplate 
	{
		public int TemplateID;
		public string Name;
		public string HeaderFile;
		public string FooterFile;
		public NodePageTemplate(int tID,string name,string hf, string ff)
		{
			TemplateID = tID;
			Name = name;
			HeaderFile = hf;
			FooterFile = ff;
		}
	}
}