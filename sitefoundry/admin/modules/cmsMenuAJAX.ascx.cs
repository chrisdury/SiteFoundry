namespace Dury.SiteFoundry.Admin
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;
	using Dury.SiteFoundry.Utils;
	using AjaxPro.JSON;

	/// <summary>
	///		Summary description for cmsMenuAJAX.
	/// </summary>
	public class cmsMenuAJAX : System.Web.UI.UserControl
	{
		//private UserPrincipal user;
		protected Literal menu;
		private NodeCollection userNodes;
		Node currentNode;
		int currentNodeID;
		private void Page_Load(object sender, System.EventArgs e)
		{
			AjaxPro.Utility.RegisterTypeForAjax(typeof(cmsMenuAJAX));

			userNodes = NodeFactory.UserNodes;
			Session["nodeRoot"] = userNodes;

			if (Request.QueryString["nodeID"] != null) 
			{
				currentNodeID = int.Parse(Request.QueryString["nodeID"]);
				currentNode = userNodes.Find(currentNodeID);//rootNode.Find(currentNodeID);
			}
			else 
			{
				currentNodeID = 0;
			}


			if (!IsPostBack) 
			{
				foreach(Node n in userNodes)
				{
					menu.Text += "drawMenu(" + n.Id + ");";
				}
			}

			if (Request.QueryString["dir"] != null) processNodeOrderChange();
		}
		public void changeNodeRank(Node currentNode, string direction) 
		{
			Node p = currentNode.parent;
			int i = currentNode.GetIndexInParentCollection();
			if (i >= 0 && i < p.children.Count) 
			{
				int oldRank = currentNode.Rank;
				int dir = (direction=="dn") ? 1 : -1;

				if (dir == -1 && i == 0) return;
				if (dir == 1 && i == p.children.Count-1) return;

				//Response.Write("changing " + currentNode.Id + ":" + currentNode.Rank + " to " + p.children[i+dir].Id + ":" + p.children[i+dir].Rank);

				currentNode.Rank = p.children[i+dir].Rank;
				p.children[i+dir].Rank = oldRank;
				currentNode.Publish=true;
				p.children[i+dir].Publish = true;
				SFGlobal.ObjectManager.PersistChanges(currentNode);
				SFGlobal.ObjectManager.PersistChanges(p.children[i+dir]);
				SFGlobal.UpdateNodes();
			} 
			else 
			{
				//System.Web.HttpContext.Current.Response.Write("ignoring order change");
			}
		}

		private void processNodeOrderChange() 
		{
			changeNodeRank(currentNode,Request.QueryString["dir"]);
			Response.Redirect(Request.Path);
		}



		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GetNode(int nodeID)
		{
			//Node n = (Node)Session["nodeRoot"];
			//Node s = n.Find(nodeID);

			NodeCollection nc = (NodeCollection)Session["nodeRoot"];
			Node s = nc.Find(nodeID);
			AjaxPro.JSON.JSONParser json = new JSONParser();
			return json.GetJSONString(s.GetSimple(0));	

		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GetNodeChildren(int nodeID)
		{
			/*
			Node n = (Node)Session["nodeRoot"];
			Node s = n.Find(nodeID);
			*/
			NodeCollection nc = (NodeCollection)Session["nodeRoot"];
			Node s = nc.Find(nodeID);
			AjaxPro.JSON.JSONParser json = new JSONParser();
			return json.GetJSONString(s.GetSimpleChildren());
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
