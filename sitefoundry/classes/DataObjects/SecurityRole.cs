using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class SecurityRole
	{
		private int id;
		private string name;
		private bool publish;
		private IList securityNodeRoleIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList securityUserRoleRoleIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList

		public int Id
		{
			get { return this.id; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public bool Publish
		{
			get { return this.publish; }
			set { this.publish = value; }
		}

		public IList SecurityNodeRoleIDs
		{
			get { return this.securityNodeRoleIDs; }
		}

		public IList SecurityUserRoleRoleIDs
		{
			get { return this.securityUserRoleRoleIDs; }
		}

	}
}
