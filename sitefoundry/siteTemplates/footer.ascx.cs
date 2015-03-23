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
	///		Summary description for footer.
	/// </summary>
	public class footer : Dury.SiteFoundry.UI.BaseControl
	{
		protected System.Web.UI.WebControls.Literal stats;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Node n = (Node)Context.Items["currentNode"];
			stats.Text = "filename=" + n.Filename + ", username=" + ((UserIdentity)Context.User.Identity).Name + ", nodeTemplateID=" + n.TypeID;

			TimeSpan ts = System.DateTime.Now.Subtract(Page.PageTime);
			stats.Text += String.Format(", page generated in {0} ms",ts.Milliseconds.ToString());
			stats.Text += ", users=" + SFGlobal.UserCount.ToString();
			
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
