using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	public class SecurityUser
	{
		private int id;
		private bool disabled;
		private string username;
		private string password;
		private string fullName;
		private string email;
		private DateTime lastLogin;
		private DateTime dateCreated;
		private DateTime dateModified;
		private IList advancedArticleUserIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList linkCreatedBies; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList linkEditedBies; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList securityUserRoleUserIDs; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList simpleArticleCreatedBies; // Supports both ObjectSet and Lazy-Loaded ObjectList
		private IList simpleArticleEditedBies; // Supports both ObjectSet and Lazy-Loaded ObjectList

		public int Id
		{
			get { return this.id; }
		}

		public bool Disabled
		{
			get { return this.disabled; }
			set { this.disabled = value; }
		}

		public string Username
		{
			get { return this.username; }
			set { this.username = value; }
		}

		public string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}

		public string FullName
		{
			get { return this.fullName; }
			set { this.fullName = value; }
		}

		public string Email
		{
			get { return this.email; }
			set { this.email = value; }
		}

		public DateTime LastLogin
		{
			get { return this.lastLogin; }
			set { this.lastLogin = value; }
		}

		public DateTime DateCreated
		{
			get { return this.dateCreated; }
			set { this.dateCreated = value; }
		}

		public DateTime DateModified
		{
			get { return this.dateModified; }
			set { this.dateModified = value; }
		}

		public IList AdvancedArticleUserIDs
		{
			get { return this.advancedArticleUserIDs; }
		}

		public IList LinkCreatedBies
		{
			get { return this.linkCreatedBies; }
		}

		public IList LinkEditedBies
		{
			get { return this.linkEditedBies; }
		}

		public IList SecurityUserRoleUserIDs
		{
			get { return this.securityUserRoleUserIDs; }
		}

		public IList SimpleArticleCreatedBies
		{
			get { return this.simpleArticleCreatedBies; }
		}

		public IList SimpleArticleEditedBies
		{
			get { return this.simpleArticleEditedBies; }
		}

	}
}
