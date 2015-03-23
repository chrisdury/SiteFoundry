using System;
using System.Collections;
using Wilson.ORMapper;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Encapsulates security related functions
	/// </summary>
	public class Roles 
	{
		private static readonly Roles instance=new Roles();
		public static Roles GetInstance() { return instance; }
		static Roles() {}
		Roles()
		{
		}

		public static SecurityNode GetSecurityNode()
		{
			return (SecurityNode)SFGlobal.ObjectManager.GetObject(typeof(SecurityNode));
		}

		public static SecurityNode GetSecurityNode(int id)
		{
			return (SecurityNode)SFGlobal.ObjectManager.GetObject(typeof(SecurityNode),id);
		}

		public static void SaveSecurityNode(SecurityNode sn)
		{
			SFGlobal.ObjectManager.PersistChanges(sn);
		}
		public static void DeleteSecurityNode(SecurityNode sn)
		{
			SFGlobal.ObjectManager.MarkForDeletion(sn);
			SFGlobal.ObjectManager.PersistChanges(sn);
		}


		public static ArrayList GetAllRoles() 
		{
			return (ArrayList)SFGlobal.ObjectManager.GetCollection(typeof(ArrayList),new OPathQuery(typeof(SecurityRole),"",""));
		}            

	}
}
