namespace Dury.SiteFoundry.nodeTemplates.articleContentEditors
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for FreeTextBox.
	/// </summary>
	public class FreeTextBox : System.Web.UI.UserControl, Dury.SiteFoundry.Article.IContentEditor
	{
		private string text;
		public string Text
		{
			get { return text; }
			set { text = value; }
		}
		private int height;
		public int Height
		{
			get { return height; }
			set { height = value; }
		}
		private int width;
		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		protected Dury.SiteFoundry.UI.CustomFreeTextBox content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			content.Height = Height;
			content.Width = Width;
			content.Text = Text;
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
