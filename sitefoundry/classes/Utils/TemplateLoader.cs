using System;

using System.Xml;

namespace Dury.SiteFoundry.Utils
{
	/// <summary>
	/// Static methods to load node templates.
	/// </summary>
	public class TemplateLoader
	{
		public static NodeTemplateCollection Templates;
		
		public TemplateLoader(){}


		/// <summary>
		/// Read XML file containing definitions to in-memory object for later use by .GetTemplate()
		/// </summary>
		/// <param name="xmlFile">which file to load templates from</param>
		public static void Load(string xmlFile) 
		{
			if (xmlFile == null || xmlFile.Length < 1) throw new DuryTools.ErrorHandler("TemplateLoader: xmlFile is blank or null");
			Templates = new NodeTemplateCollection();
			xmlFile = SFGlobal.BaseDirectory + xmlFile;
			try
			{
				System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
				xd.Load(xmlFile);
				foreach (XmlNode x in xd["nodeTypes"]) 
				{
					NodeTemplate n = new NodeTemplate();
					n.ID = int.Parse(x.Attributes["id"].Value);
					n.Name = x.Attributes["name"].Value;
					n.SiteTemplate= x.Attributes["siteTemplate"].Value;
					n.AdminTemplate= x.Attributes["adminTemplate"].Value;
					n.PublishSP = (x.Attributes["publishSP"] != null) ? x.Attributes["publishSP"].Value : "";
					Templates.Add(n);
				}
			}
			catch (Exception e)
			{
				throw new DuryTools.ErrorHandler("Problem parsing XML file.. check xml syntax or file: " + xmlFile,e);
			}
	
		}

		/// <summary>
		/// Get the template associated with the specified node
		/// </summary>
		/// <param name="n">Node you want to load the template from</param>
		/// <returns>UserControl</returns>
		public static System.Web.UI.Control GetTemplate(Node n)
		{
			System.Web.UI.UserControl c = new System.Web.UI.UserControl();
			return c.LoadControl("~/" + SFGlobal.NodeTemplateLocation + "/" + Templates.getByID(n.TypeID).SiteTemplate + ".ascx");
		}

		/// <summary>
		/// Get the template associated with the specified node
		/// </summary>
		/// <param name="n">Node you want to load the template from</param>
		/// <returns>UserControl</returns>
		public static System.Web.UI.Control GetAdminTemplate(Node n)
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			System.Web.UI.UserControl c = new System.Web.UI.UserControl();
			if (n==null) context.Response.Redirect(context.Request.RawUrl.Substring(0,context.Request.RawUrl.IndexOf("?")));
			return c.LoadControl("~/" + SFGlobal.NodeTemplateLocation + "/" + Templates.getByID(n.TypeID).AdminTemplate + ".ascx");;
		}

		/// <summary>
		/// Gets the list of templates.  Used w/ webservice to pass template information to flash menu system
		/// </summary>
		/// <returns>xml (as string)</returns>
		public static string GetTemplateListXML()
		{
			System.Xml.XmlDocument xd = new XmlDocument();
			string xml = "<templates>";
			foreach(NodeTemplate nt in Templates)
			{
				xml += "<template id=\"" + nt.ID + "\" name=\"" + nt.Name + "\" />";
			}
			xml += "</templates>";
			return xml;
		}


	}


	/// <summary>
	/// Stores information regarding templates applied to Nodes.
	/// </summary>
	public struct NodeTemplate
	{
		public int ID;
		public string Name;
		public string SiteTemplate;
		public string AdminTemplate;
		public string PublishSP;

	}


	/// <summary>
	/// Strongly-typed collection of NodeTemplates
	/// </summary>
	/// 
	public class NodeTemplateCollection : System.Collections.CollectionBase
	{
		public NodeTemplateCollection() 
		{}

		public void Add(NodeTemplate n)
		{
			List.Add(n);
		}

		public NodeTemplate this[int index]
		{
			get
			{
				return (NodeTemplate)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public NodeTemplate getByID(int id) 
		{
			foreach(NodeTemplate n in List)
			{
				if (n.ID == id)
					return n;
			}
			throw new Exception("supplied ID not found");
		}


	}
	


}
