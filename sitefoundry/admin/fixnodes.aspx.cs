using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;

namespace Dury.SiteFoundry.admin
{
	/// <summary>
	/// Summary description for fixnodes.
	/// </summary>
	public class fixnodes : System.Web.UI.Page
	{
		protected Button fix;
		//private Node rootNode;

		private int rootNodeID = 1;


		private void Page_Load(object sender, System.EventArgs e)
		{
			fix.Click += new EventHandler(fix_Click);
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		private void fix_Click(object sender, EventArgs e)
		{
			fixNodes(rootNodeID);
		}


		private void fixNodes(int currentNodeID) 
		{
			Node currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node),currentNodeID);
			Wilson.ORMapper.ObjectReader or = SFGlobal.ObjectManager.GetObjectReader(typeof(Node),"parentID=" + currentNodeID);
			int i = 0;
			foreach(Node n in or) 
			{
				n.Rank = i;
				Response.Write(n.getName("en-CA") + " rank is:" + n.Rank.ToString() + "<BR>");
				SFGlobal.ObjectManager.PersistChanges(n);
				fixNodes(n.Id);
				i++;
			}
		}

	}
}
