namespace Dury.SiteFoundry.siteTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;

	/// <summary>
	///		Summary description for defaultHeader.
	/// </summary>
	public class defaultHeader : Dury.SiteFoundry.UI.BaseControl
	{

		protected System.Web.UI.WebControls.Literal menu;
		protected System.Web.UI.WebControls.Literal userstring;

		public string siteRoot;

		private void Page_Load(object sender, System.EventArgs e)
		{
			UserPrincipal u = (UserPrincipal)Context.User;
			Node root;
			//if (u.IsInRole("Administrators") || u.IsInRole("Publishers") || u.IsInRole("Contributors") || u.IsInRole("Editors"))
			if (u.IsUserCMS())
				root = (Node)Application["nodeRoot"];
			else
				root = (Node)Application["nodeRootPublic"];
			menu.Text = root.ToHTML(0);
			//if (u.CheckRole("Administrators") || u.CheckRole("Publishers") || u.CheckRole("Contributors") || u.CheckRole("Editors"))
			if (u.IsUserCMS())
			{
				menu.Text += "<BR><a href=\"/SiteFoundry/logout.aspx\">logout</a>";
				menu.Text += "<BR><a href=\"admin\">admin</a>";
			}
					

			siteRoot = "/" + System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"];
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
