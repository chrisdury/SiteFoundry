using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class SecurityUserRole
	{
		private int id;
		private int userID;
		private int roleID;
		private ObjectHolder roleIDObject; // Strongly Type as SecurityRole if not Lazy-Loading
		private ObjectHolder userIDObject; // Strongly Type as SecurityUser if not Lazy-Loading

		public int Id
		{
			get { return this.id; }
		}

		public int UserID
		{
			get { return this.userID; }
			set { this.userID = value; }
		}

		public int RoleID
		{
			get { return this.roleID; }
			set { this.roleID = value; }
		}

		// Return the primary key property from the primary key object
		public SecurityRole RoleIDObject
		{
			get { return (SecurityRole)this.roleIDObject.InnerObject; }
		}

		// Return the primary key property from the primary key object
		public SecurityUser UserIDObject
		{
			get { return (SecurityUser)this.userIDObject.InnerObject; }
		}

	}
}
