using System;
using Wilson.ORMapper;
using DuryTools.Data;
using Dury.SiteFoundry.Security;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Retrieves nodes from an ObjectManager
	/// </summary>
	public class NodeFactory
	{

		public static Node RootNode
		{
			get 
			{
				return (Node)System.Web.HttpContext.Current.Application["nodeRoot"];
			}
			set 
			{
			}
		}
		
		public static NodeCollection UserNodes
		{
			get 
			{
				return getUserNodes(SFGlobal.CurrentUser,NodeFactory.RootNode);
			}
		}

		private static readonly NodeFactory instance=new NodeFactory();
		public static NodeFactory GetInstance() { return instance; }
		static NodeFactory() {}
		NodeFactory()
		{
		}

		public static Node GetNode(int id)
		{
			return RootNode.Find(id);
		}

		private static Node getAllNodes(ObjectSpace ObjectManager, int currentNodeID, Node parent)
		{
			try 
			{
				Node n = (Node)ObjectManager.GetObject(typeof(Node),currentNodeID);
				n.parent = parent;
				ObjectQuery oq = new ObjectQuery(typeof(Node),"parentID=" + currentNodeID, "rank ASC");
				//ObjectReader or = ObjectManager.GetObjectReader(typeof(Node),"parentID=" + currentNodeID);
				ObjectReader or = ObjectManager.GetObjectReader(oq);
				NodeCollection nc = new NodeCollection();
				foreach (Node childNode in or)
				{
					nc.Add(getAllNodes(ObjectManager,childNode.Id,n));
				}
				n.children = nc;
				return n;
			} 
			catch (Exception e)
			{
				throw new Exception("problem! currentNodeID:" + currentNodeID,e);
			}			           
		}
		public static Node GetAll(ObjectSpace ObjectManager, int startNodeID)
		{
			return getAllNodes(ObjectManager,startNodeID,null);
		}

		public static Node CreateNewNode()
		{
			return (Node)SFGlobal.ObjectManager.GetObject(typeof(Node));
		}

		private static NodeCollection getUserNodes(UserPrincipal user, Node currentNode)
		{
			NodeCollection nc = new NodeCollection();
			if (user.CheckRolePermission(currentNode,Permission.Add) || user.CheckRolePermission(currentNode,Permission.Edit) || user.CheckRolePermission(currentNode,Permission.Publish) || user.CheckRolePermission(currentNode,Permission.Delete) ) 
			{
				nc.Add(currentNode);
				return nc;
			}
			foreach(Node nn in currentNode.children)
			{
				nc.AddRange(getUserNodes(user,nn));
			}
			return nc;
		}




		/// <summary>
		/// Deletes the supplied Node and all it's children
		/// </summary>
		/// <param name="n">Node you wish to delete</param>
		public static void DeleteNode(Node currentNode)
		{
            SFGlobal.DAL.execNonQuery("DELETE FROM NodeNames WHERE nodeID = " + currentNode.Id);

			// delete form(s)
            FormBuilder.DeleteForm(currentNode);

			foreach(Node n in currentNode.children)
			{
				NodeFactory.DeleteNode(n);
			}
			SFGlobal.ObjectManager.MarkForDeletion(currentNode);
			SFGlobal.ObjectManager.PersistChanges(currentNode);
		}

	}
}
