using System;
using System.Collections;
using System.Security.Principal;
using Wilson.ORMapper;

namespace Dury.SiteFoundry.Security
{
	/// <summary>
	/// Principal User object
	/// </summary>
	public class UserPrincipal: IPrincipal
	{
		private IIdentity identity;
		private ArrayList roles;

		public UserPrincipal(IIdentity id, ArrayList rolesArray)
		{
			identity = id;
			roles = rolesArray;
		}

		/// <summary>
		/// Checks to see if user belongs to supplied role
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			return roles.Contains(role);
		}


		public bool IsRoleCMS(string role, Node n)
		{
			if (this.CheckRolePermission(n,Permission.Add) || this.CheckRolePermission(n,Permission.Edit) || this.CheckRolePermission(n,Permission.Publish) || this.CheckRolePermission(n,Permission.Delete) ) return true;
			foreach(Node nn in n.children)
			{
				if (this.IsRoleCMS(role,nn)) return true;
			}
			return false;
		}

		public bool CanUserModify(Node n)
		{
			if (this.CheckRolePermission(n,Permission.Delete) || this.CheckRolePermission(n,Permission.Edit))
				return true;
			else
				return false;
		}

		public bool IsUserAdmin() 
		{
			ObjectReader or = SFGlobal.ObjectManager.GetObjectReader(new OPathQuery(typeof(SecurityRole),"",""));
			foreach(SecurityRole sr in or) 
			{
				foreach(SecurityUserRole sur in sr.SecurityUserRoleRoleIDs) 
				{
					if (sur.UserID == this.ID && sur.RoleIDObject.Publish) return true;
					//if (sur.UserID == this.ID) return true;
					//System.Web.HttpContext.Current.Response.Write(sur.RoleIDObject.Name + "-" + sur.RoleIDObject.Publish + "<br>");
					//if (sur.RoleIDObject.Publish) return true;
				}
			}
			return false;
		}

		public bool IsUserCMS()
		{
            if (this.Roles.Contains("public")) return false;
			else return true;
		}

		public int ID
		{
			get { return ((UserIdentity)identity).ID; }
		}



		public IIdentity Identity
		{
			get { return identity; }
			set { identity = value; }
		}

		/// <summary>
		/// list of roles that the user belongs to
		/// </summary>
		public ArrayList Roles
		{
			get 
			{
				return roles;
			}
		}

		/// <summary>
		/// Check the supplied node to see if it the user has permissions on it
		/// </summary>
		/// <param name="n">What node to look to</param>
		/// <param name="perm">What permission to look up.  </param>
		/// <returns>true if user belongs to a role which has the permission requested.</returns>
		public bool CheckRolePermission(Node n, Permission perm)
		{
			foreach(string s in this.roles)
			{
				int p = n.getRolePermission(s);
				int q = (int)perm;
				//System.Web.HttpContext.Current.Response.Write("p=" + p.ToString() + ", q=" + q.ToString() + ", p&q=" + (p & q));
				// if ((p & q) == 0) return true;
				if ((p & q) >= 1) return true;
			}
			return false;
		}


		/// <summary>
		/// Generates symmetric encrypted string representing the user.  This method is supposed to be used to authenticate
		/// web service requests or other 'out-of-context' requests
		/// </summary>
		/// <returns>encrypted representation of user</returns>
		public string EncryptPrincipal()
		{
			string s = ((UserIdentity)this.identity).ID + "|" + this.identity.Name;
			foreach(string r in this.roles) s += "|" + r;
			//return Dury.SiteFoundry.Security.Cryptography.Symmetric.EncryptData(SFGlobal.EncryptionKey,s);
			Dury.SiteFoundry.Security.Cryptography.SymmetricEncryption se = new Dury.SiteFoundry.Security.Cryptography.SymmetricEncryption();
			return se.EncryptString(s,SFGlobal.EncryptionKey);
		}

	}


	/// <summary>
	/// Represents role information for tie-in w/ database
	/// (might be a struct?)
	/// </summary>
	public class Role
	{
		private int id;
		public int ID
		{
			get	{ return id;  }
		}

		private string name;
		public string Name
		{
			get { return name; }
			set { name=value; }
		}
		public Role()
		{
		}
	}


	/// <summary>
	/// Holds user specific informatio
	/// </summary>
    public class UserIdentity : IIdentity
	{
		private int id;
		private string username;
		private string password;
		private string fullName;
		private string email;
		private string language;
		private IList roles;
		private bool login = true;

		public UserIdentity()
		{
		}

		public int ID 
		{
			get { return this.id; }
		}

		public UserIdentity(string u)
		{
			this.username = u;
		}
		public UserIdentity(int userID, string u)
		{
			this.id = userID;
			this.username = u;
		}

		public string AuthenticationType 
		{
			get { return "Custom"; }
		}

		public bool IsAuthenticated  
		{
			get { return login; }
			set { login = value; }
		}
		public string Name 
		{
			get { return username; }
			set { username = value; }
		}

		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		public string UserEmail
		{
			get { return email; }
			set { email = value; }
		}

		public string Language
		{
			get { return language; }
			set { language = value; }
		}

		public string Password
		{
			get { return password; }
			set { password = value; }
		}


	}


}
