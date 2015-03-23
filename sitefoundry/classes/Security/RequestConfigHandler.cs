using System;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace Dury.SiteFoundry.Security
{
	/// <summary>
	/// This class acts as a factory for the configuration settings.
	/// </summary>
	public sealed class RequestConfigHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Create a new config section handler.  This is of type <see cref="Settings"/>
		/// </summary>
		object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode configSection) 
		{
			RequestConfigSettings settings;
			if(parent == null)
				settings = RequestConfigSettings.Default;
			else
				settings = (RequestConfigSettings)parent;
			settings.AddSettings(configSection);
			return settings;
		}
	}

}


