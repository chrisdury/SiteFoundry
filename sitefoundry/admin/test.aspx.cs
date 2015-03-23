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
using Dury.SiteFoundry.Security;


namespace Dury.SiteFoundry.admin
{
	/// <summary>
	/// Summary description for test.
	/// </summary>
	public class test : System.Web.UI.Page
	{
		protected DataGrid grid;
		protected Literal msg;
		private UserPrincipal user;

		private void Page_Load(object sender, System.EventArgs e)
		{
			user = SFGlobal.CurrentUser;
			/*
			DuryTools.Data.DataContainer dc = new DuryTools.Data.DataContainer("ArticleBodies");
			DataRow dr = dc.GetRowByKey("id",35);
			msg.Text = dr["body"].ToString();
			*/
			int nodeID = (Request.QueryString["n"] != null) ? int.Parse(Request.QueryString["n"]) : 1;
            
			Node root = (Node)Application["nodeRoot"];
			Node n = root.Find(nodeID);

			msg.Text += n.Filename + "<BR>";

			foreach(int i in Enum.GetValues(typeof(Permission)))
			{
				string s = Enum.GetName(typeof(Permission),i);
				msg.Text += s + "=";
				msg.Text += user.CheckRolePermission(n,(Permission)i).ToString();
				msg.Text += "<BR>";
			}


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

	}
}
