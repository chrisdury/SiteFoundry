namespace Dury.SiteFoundry.articleTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for template1.
	/// </summary>
	public class template1 : Dury.SiteFoundry.BaseArticle
	{
		protected System.Web.UI.WebControls.Literal articleBody;
		protected System.Web.UI.WebControls.Literal articleTitle;
        
		private void Page_Load(object sender, System.EventArgs e)
		{
			articleBody.Text = ArticleBody;
			articleTitle.Text =ArticleTitle;
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
