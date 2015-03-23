using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace Dury.SiteFoundry.UrlRewrite
{
	/// <summary>
	/// This class encapsulates the settings for SiteFoundry UrlRewrite
	/// </summary>
	public sealed class RewriteConfigSettings 
	{

		private StringCollection excludedItems;
		private string pageHandler;

		/// <summary>
		/// Create an HttpCompressionModuleSettings from an XmlNode
		/// </summary>
		/// <param name="node">The XmlNode to configure from</param>
		public RewriteConfigSettings(XmlNode node) : this() 
		{
			AddSettings(node);
		}

    
		private RewriteConfigSettings() 
		{
			excludedItems = new StringCollection();
			pageHandler = "";
		}

		/// <summary>
		/// Suck in some more changes from an XmlNode.  Handy for config file parenting.
		/// </summary>
		/// <param name="node">The node to read from</param>
		public void AddSettings(XmlNode node) 
		{

			if(node == null)
				return;

			XmlAttribute ph = node.Attributes["pageHandler"];
			if(ph != null) 
			{
				try 
				{
					pageHandler = ph.Value;//(Algorithms)Enum.Parse(typeof(Algorithms), preferredAlgorithm.Value, true);
				}
				catch(ArgumentException) { }
			}
			ParseExcludedPaths(node.SelectSingleNode("excludedItems"));
		}


		/// <summary>
		/// Get the current settings from the xml config file
		/// </summary>
		public static RewriteConfigSettings GetSettings() 
		{
			RewriteConfigSettings settings = (RewriteConfigSettings)System.Configuration.ConfigurationSettings.GetConfig("SiteFoundry/urlRewrite");
			if(settings == null)
				return RewriteConfigSettings.Default;
			else
				return settings;
		}

		/// <summary>
		/// The default settings.  Deflate + normal.
		/// </summary>
		public static RewriteConfigSettings Default 
		{
			get { return new RewriteConfigSettings(); }
		}

		
		/// <summary>
		/// The preferred algorithm to use for compression
		/// </summary>
		public string PageHandler
		{
			get { return pageHandler; }
		}

    
		/// <summary>
		/// Looks for a given path in the list of paths excluded from rewriting
		/// </summary>
		/// <param name="relUrl">the relative url to check</param>
		/// <returns>true if excluded, false if not</returns>
		public bool IsExcludedItem(string relUrl) 
		{
			return this.excludedItems.Contains(relUrl.ToLower());
		}


		/// <summary>
		/// Looks for the given string in any of the excluded items
		/// </summary>
		/// <param name="input">string to check</param>
		/// <returns>true if excluded, false if not</returns>
		public bool IsExcluded(string input)
		{
			foreach(string s in this.excludedItems)
			{
				if (input.IndexOf(s) > 0)
					return true;
			}
			return false;
		}




		private void ParseExcludedPaths(XmlNode node) 
		{
			if(node == null) return;
			string attributeName = "value";

			for(int i = 0; i < node.ChildNodes.Count; ++i) 
			{
				switch(node.ChildNodes[i].LocalName) 
				{
					case "add":
						if(node.ChildNodes[i].Attributes[attributeName] != null) 
							excludedItems.Add(node.ChildNodes[i].Attributes[attributeName].Value.ToLower());
						break;
					case "delete":
						if(node.ChildNodes[i].Attributes[attributeName] != null)
							excludedItems.Remove(node.ChildNodes[i].Attributes[attributeName].Value.ToLower());
						break;
				}
			}
		}

	}

}

