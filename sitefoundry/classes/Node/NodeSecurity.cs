using System;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// List objects for what roles belong to nodes.
	/// </summary>
	public class NodeRole
	{
		private int id;
		private string name;
		private int permissionLevel;

		public string Name
		{
			get { return name;}
		}

		public int Level
		{
			get { return permissionLevel;}
		}

		public NodeRole()
		{
		}
	}


	/// <summary>
	/// Represents bit values used for different levels of permissions
	/// </summary>
	public enum Permission
	{
		None = 0,
		View = 1,
		Add = 2,
		Edit = 4,
		Publish = 8,
		Delete = 16
	}

    


}
