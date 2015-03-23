using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class SecurityNode
	{
		private int id;
		private int nodeID;
		private int roleID;
		private int permissionLevel;
		private ObjectHolder nodeIDObject; // Strongly Type as Node if not Lazy-Loading
		private ObjectHolder roleIDObject; // Strongly Type as SecurityRole if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
		}

		public int NodeID
		{
			get { return this.nodeID; }
			set { this.nodeID = value; }
		}

		public int RoleID
		{
			get { return this.roleID; }
			set { this.roleID = value; }
		}

		public int PermissionLevel
		{
			get { return this.permissionLevel; }
			set { this.permissionLevel = value; }
		}

		// Return the primary key property from the primary key object
		public Node NodeIDObject
		{
			get { return (Node)this.nodeIDObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public SecurityRole RoleIDObject
		{
			get { return (SecurityRole)this.roleIDObject.InnerObject; }
		}

	}
}
