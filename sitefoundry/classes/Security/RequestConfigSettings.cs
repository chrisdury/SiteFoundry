using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace Dury.SiteFoundry.Security
{
	/// <summary>
	/// This class encapsulates the settings for SiteFoundry UrlRewrite
	/// </summary>
	public sealed class RequestConfigSettings 
	{
		private StringCollection permittedRoles;
		private string adminDirectory;

		/// <summary>
		/// Create an HttpCompressionModuleSettings from an XmlNode
		/// </summary>
		/// <param name="node">The XmlNode to configure from</param>
		public RequestConfigSettings(XmlNode node) : this() 
		{
			AddSettings(node);
		}

    
		private RequestConfigSettings() 
		{
			permittedRoles = new StringCollection();
			adminDirectory = "";
		}

		/// <summary>
		/// Suck in some more changes from an XmlNode.  Handy for config file parenting.
		/// </summary>
		/// <param name="node">The node to read from</param>
		public void AddSettings(XmlNode node) 
		{

			if(node == null)
				return;

			XmlAttribute ad = node.Attributes["adminDirectory"];
			if(ad != null) 
			{
				try 
				{
					adminDirectory = ad.Value;//(Algorithms)Enum.Parse(typeof(Algorithms), preferredAlgorithm.Value, true);
				}
				catch(ArgumentException) { }
			}
			ParseExcludedPaths(node.SelectSingleNode("allow"));
		}


		/// <summary>
		/// Get the current settings from the xml config file
		/// </summary>
		public static RequestConfigSettings GetSettings() 
		{
			RequestConfigSettings settings = (RequestConfigSettings)System.Configuration.ConfigurationSettings.GetConfig("SiteFoundry/requestSecurity");
			if(settings == null)
				return RequestConfigSettings.Default;
			else
				return settings;
		}

		/// <summary>
		/// The default settings.  Deflate + normal.
		/// </summary>
		public static RequestConfigSettings Default 
		{
			get { return new RequestConfigSettings(); }
		}

		
		public string AdminDirectory
		{
			get { return adminDirectory; }
		}

    
		/// <summary>
		/// Looks for a given path in the list of paths excluded from rewriting
		/// </summary>
		/// <param name="relUrl">the relative url to check</param>
		/// <returns>true if excluded, false if not</returns>
		public bool IsExcludedRole(string role) 
		{
			return this.permittedRoles.Contains(role.ToLower());
		}

/*
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

*/


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
							permittedRoles.Add(node.ChildNodes[i].Attributes[attributeName].Value.ToLower());
						break;
					case "delete":
						if(node.ChildNodes[i].Attributes[attributeName] != null)
							permittedRoles.Remove(node.ChildNodes[i].Attributes[attributeName].Value.ToLower());
						break;
				}
			}
		}

	}

}

