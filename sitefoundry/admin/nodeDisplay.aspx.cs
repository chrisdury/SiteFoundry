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
using Dury.SiteFoundry.Utils;


namespace Dury.SiteFoundry.admin
{
	/// <summary>
	/// Summary description for nodeDisplay.
	/// </summary>
	public class nodeDisplay : System.Web.UI.Page
	{

		protected System.Web.UI.WebControls.PlaceHolder nodeTemplateHolder;
		private UserPrincipal user;
		public string PageTitle;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Node root = (Node)Application["nodeRoot"];
			user = (UserPrincipal)Context.User;
			//this.showMenu = false;


			if (Request.QueryString["nodeID"] != null) 
			{
				int nodeID = int.Parse(Request.QueryString["nodeID"]);
				Node currentNode = root.Find(nodeID);

				nodeTemplateHolder.Controls.Add(TemplateLoader.GetAdminTemplate(currentNode));
			}
			else
			{
				nodeTemplateHolder.Controls.Add(new LiteralControl("Please select a page from the menu to the left"));
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
