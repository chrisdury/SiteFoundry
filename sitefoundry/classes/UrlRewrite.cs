using System;


namespace HolmesAndLee.SiteFoundry
{
	/// <summary>
	/// 
	/// THIS CLASS IS DEPRECATED!!
	/// 
	/// 
	/// Re-writes the url supplied from the client.
	/// This class allows dynamic sites (somepage.aspx?page=home) to appear as (home.aspx).  This increases readability of urls and
	/// search engine placement (keywords in urls count in google)
	/// </summary>
	public class UrlRewriter
	{
		public UrlRewriter()
		{}


		public static void RewritePaths()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string nodePath = String.Empty;
			string p = context.Request.Path;
			string fe = (System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"] : ".aspx";
			string pageHandler = (System.Configuration.ConfigurationSettings.AppSettings["pageHandler"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["pageHandler"] : "pages.aspx";
			string adminDirectory = (System.Configuration.ConfigurationSettings.AppSettings["adminDirectory"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["adminDirectory"] : "admin";
			string virtualDir = (System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] : "" ;
			if (p.IndexOf(fe) > -1 && p.IndexOf(adminDirectory) < 0)
			{
				// convert our querystringed node-path into a more friendly version (minus virtual dir and escapes)
				if (virtualDir.Length > 0)
					nodePath = p.ToLower().Substring(1,p.Length-1).Replace(virtualDir,"").Replace(fe,"").Replace("/","*");
				else
					nodePath = p.ToLower().Substring(1,p.Length-1).Replace(fe,"").Replace("/","*");
				context.RewritePath("~/" + pageHandler + "?" + nodePath);
				context.Items.Add("currentNode",getCurrentNode(nodePath));

			}
		}

		private static Node getCurrentNode(string nodePath) 
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			if (context.Application["nodeRoot"] != null)
			{
				Node n = (Node)context.Application["nodeRoot"];
				System.Collections.ArrayList s = new System.Collections.ArrayList(nodePath.Split('*'));
				return n.Find(s);
			}
			else
			{
				return null;
			}
		}

		/*
		 * might not be needed!
		private string[] parseQueryString(string qs)
		{
			qs = qs.Replace(System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"],"");
			qs = qs.Replace("/" + System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] + "/","");
			if (qs.IndexOf('/') == 0)
				qs = qs.Substring(1,qs.Length-1);
			if (qs.IndexOf("?") > -1)
				qs = qs.Substring(0,qs.IndexOf("?"));
			if (qs.IndexOf("/") > -1)
				return qs.Split('/');
			else
				return qs.Split(' ');
		}
		*/


		private static void newProcess() 
		{

			// http://mysite.com/index.aspx/who_we_are





		}








	}
}
