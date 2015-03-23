using System;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace Dury.SiteFoundry.UrlRewrite
{
	/// <summary>
	/// This class acts as a factory for the configuration settings.
	/// </summary>
	public sealed class ConfigHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Create a new config section handler.  This is of type <see cref="Settings"/>
		/// </summary>
		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode configSection) 
		{
			RewriteConfigSettings settings;
			if(parent == null)
				settings = RewriteConfigSettings.Default;
			else
				settings = (RewriteConfigSettings)parent;
			settings.AddSettings(configSection);
			return settings;
		}
	}

}


