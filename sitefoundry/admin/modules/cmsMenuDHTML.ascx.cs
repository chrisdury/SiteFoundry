namespace Dury.SiteFoundry.admin
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for cmsMenuDHTML.
	/// </summary>
	public class cmsMenuDHTML : System.Web.UI.UserControl
	{
		protected Literal menu;
		private int currentNodeID;
		private Node currentNode;
		private Node rootNode;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.EnableViewState = false;
			rootNode = (Node)Application["nodeRoot"];//LBNodeFactory.RootNode;
			if (Request.QueryString["nodeID"] != null) 
			{
				currentNodeID = int.Parse(Request.QueryString["nodeID"]);
				currentNode = rootNode.Find(currentNodeID);
			}
			else 
			{
				currentNodeID = 0;
			}

			if (Request.QueryString["dir"] != null) processNodeOrderChange();	
			drawMenu();
		}


		private void drawMenu()
		{
			menu.Text = "<ul !class=\"menu\">" + formatMenuItem(0,rootNode) + "</ul>";
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

				Response.Write("changing " + currentNode.Id + ":" + currentNode.Rank + " to " + p.children[i+dir].Id + ":" + p.children[i+dir].Rank);

				currentNode.Rank = p.children[i+dir].Rank;
				//p.children[i+dir].Rank = currentNode.Rank;
				p.children[i+dir].Rank = oldRank;
				currentNode.Publish=true;
				p.children[i+dir].Publish = true;
				//currentNode.Rank = (oldRank+dir >= 0) ? oldRank+dir : 0;
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
			//Response.Redirect(Request.Path);
		}

		private string formatMenuItem(int indent,Node n) 
		{
			string itemClass = (currentNodeID == n.Id) ? "class=\"active\" " : "";
			string s = "";
			s = "<li>";
			if (n.children.Count>0) { s += "<img id=\"mo_menu_{0}\" src=\"images/closed.gif\" class=\"open\" onClick=\"toggleMenu('menu_{0}');\">";}//-</span>"; }
			//s += "<a name=\"{0}\" href=\"javascript:document.all('nodeDisplay').location.href='nodeDisplay.aspx?nodeID={0}';\" {2} onmouseover=\"toggleNodeMenu(this,'{0}');\">{1}</a>";
			s += "<a name=\"{0}\" href=\"javascript:sn({0});\" {2} onmouseover=\"toggleNodeMenu(this,'{0}');\">{1}</a>";
			s += "<span id=\"LB_MENU_{0}\" class=\"menuItemUtils\"> ";
			s += "<span class=\"up\" onClick=\"g('?nodeID={0}&dir=up');\">+</span>";
			s += "<span class=\"dn\" onClick=\"g('?nodeID={0}&dir=dn');\">-</span>";
			s += "<span class=\"edit\" onClick=\"editNode({0});\">edit</span>";
			s += "<span class=\"del\" onClick=\"deleteNode({0});\">del</span>";
			s += "</span>";
			s = String.Format(s,n.Id,n.getName(SFGlobal.DefaultLanguage),itemClass);
			if (n.children.Count>0) 
			{
				if (indent < 1) s += "<ul id=\"menu_" + n.Id + "\" class=\"menu\">";
				else s += "<ul id=\"menu_" + n.Id + "\" class=\"menu\" style=\"display:none;\">";
			} 
			foreach(Node child in n.children) 
			{
				s += formatMenuItem(indent + 1,child);
			}
			if (n.children.Count>0) s += "</ul>";
			return s + "</li>";
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
