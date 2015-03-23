using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml;

using DuryTools.Data;
using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;
using Wilson.ORMapper;


namespace Dury.SiteFoundry.Admin
{
	/// <summary>
	/// Summary description for data.
	/// </summary>
	[WebService(Namespace="http://www.DuryTools.com/SiteFoundry/")]
	public class data : System.Web.Services.WebService
	{
		private DAL dal = SFGlobal.DAL;

		public data()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}


		[WebMethod]
		public XmlDocument GetMenu(int nodeID) 
		{
			Node root = (Node)Application["nodeRoot"];
			XmlDocument xd = new XmlDocument();
			string s = "";
			try 
			{
				Node n = root.Find(nodeID);
				s += n.ToSelfXML(1);
				xd.LoadXml(s);
			}
			catch (Exception e)
			{
				xd.LoadXml("<error>" + e.Message + "</error>");
			}
			return xd;
		}

		[WebMethod]
		public XmlDocument GetTopMenuForUser() 
		{
			XmlDocument xd = new XmlDocument();
			xd.LoadXml("<error>not implemented</error>");
			return xd;
		}


		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region flash menu
		/*
		[WebMethod]
		public System.Xml.XmlDocument getMenu(string key)
		{
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			string s = String.Empty;
			try
			{
				UserPrincipal up = SFGlobal.CreateUserPrincipal(key);
				Node root = (Node)Application["nodeRoot"];
				s = root.ToXML();
			}
			catch(Exception e)
			{
				s = "<error>" + e.Message + "</error>";
			}
			xd.LoadXml(s);			
			return xd;
		}


		[WebMethod]
		public System.Xml.XmlDocument updateNodes(string key, string update)
		{
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			string s = "";
			try
			{
				UserPrincipal up = SFGlobal.CreateUserPrincipal(key);
				if (!up.IsInRole(SFGlobal.AdminstratorRoleName)) throw new DuryTools.ErrorHandler("user doesn't have permission");
                s = processNodesUpdate(update);
				SFGlobal.UpdateNodes();
			}
			catch(Exception e)
			{
				s = "<error>" + e.Message + "</error>";
			}
			xd.LoadXml(s);
			return xd;
		}

		// id:parentID,rank#;
		private string processNodesUpdate(string complexNodes)
		{
			string[] nodes = complexNodes.Split(';');
			if (nodes.Length > 0)
			{
				System.Collections.ArrayList nodesList = new ArrayList(nodes.Length);
				foreach(string s in nodes)
				{
					if (s.Length > 0) 
					{
						string[] n = s.Split('|');
						string[] i = n[1].Split(',');
						NodeUpdateInfo ni = new NodeUpdateInfo(int.Parse(n[0]),int.Parse(i[0]),int.Parse(i[1]));
						nodesList.Add(ni);
					}
				}
				foreach(NodeUpdateInfo ni in nodesList)
				{
					dal.execNonQuery("UPDATE Nodes SET publish=1,parentID=" + ni.ParentID + ",rank=" + ni.Rank + " WHERE id=" + ni.ID);
				}
				return "<status>" + nodesList.Count.ToString() + " nodes updated</status>";
			}
			else
			{
				return "<error>nodes length is 0</error>";
			}
		}


		private struct NodeUpdateInfo
		{
			public int ID;
			public int ParentID;
			public int Rank;
			public NodeUpdateInfo(int id, int parentID, int rank)
			{
				this.ID = id;
				this.ParentID = parentID;
				this.Rank = rank;
			}
		}



		[WebMethod]
		public System.Xml.XmlDocument addNode(string key)
		{
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			string xml = String.Empty;
			try
			{
				UserPrincipal up = SFGlobal.CreateUserPrincipal(key);
				if (!up.IsInRole(SFGlobal.AdminstratorRoleName)) throw new DuryTools.ErrorHandler("user doesn't have permission");
				Node currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node));
				currentNode.ParentID = 1;
				currentNode.TypeID = 0;
				currentNode.Visible = true;
				currentNode.Publish = true;
				currentNode.Rank = 10000;
				currentNode.Filename = "newnode";
				currentNode.DateCreated = System.DateTime.Now;
				currentNode.DateModified = System.DateTime.Now;
				SFGlobal.ObjectManager.PersistChanges(currentNode);
				SFGlobal.UpdateNodes();
				xml = "<status>" + currentNode.Id +"</status>";
			}
			catch(Exception e)
			{
				xml = "<error>" + e.Message + "</error>";
			}
			xd.LoadXml(xml);
			return xd;
		}


		[WebMethod]
		public System.Xml.XmlDocument removeNode(string key, int nodeID)
		{
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			string xml = String.Empty;
			try
			{
				UserPrincipal up = SFGlobal.CreateUserPrincipal(key);
				if (!up.IsInRole(SFGlobal.AdminstratorRoleName)) throw new DuryTools.ErrorHandler("user doesn't have permission");
				Node currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node),nodeID);
				NodeFactory.DeleteNode(currentNode);
				SFGlobal.UpdateNodes();
				xml = "<status>ok</status>";
			}
			catch(Exception e)
			{
				xml = "<error>" + e.Message + "</error>";
			}
			xd.LoadXml(xml);
			return xd;
		}


		*/
		#endregion

		#region utils


		private void updateRoles(int nodeID, string complexRoles)
		{
			deleteRoles(nodeID);
			string[] roles = complexRoles.Split(',');
			foreach(string s in roles)
			{
				string[] r = s.Split(':');
				SFGlobal.DAL.execNonQuery("INSERT INTO SecurityNodes (nodeID,roleID,permissionLevel) VALUES ({0},{1},{2}",nodeID.ToString(),r[0],r[1]);
			}            
		}


		private void deleteRoles(int nodeID)
		{
            SFGlobal.DAL.execNonQuery("DELETE FROM SecurityNodes WHERE nodeID = " + nodeID.ToString());
		}
		

		#endregion
	
	}
}
