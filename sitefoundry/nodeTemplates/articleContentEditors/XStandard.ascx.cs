namespace Dury.SiteFoundry.nodeTemplates.articleContentEditors
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using XStandard.WebForms;

	/// <summary>
	///		Summary description for XStandard.
	/// </summary>
	public class XStandardEditor : System.Web.UI.UserControl, Dury.SiteFoundry.Article.IContentEditor
	{
		protected XHTMLEditor content;
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

		private void Page_Load(object sender, System.EventArgs e)
		{
			content.Height = Height;
			content.Width = Width;
			content.Value = Text;

			string sl = "http://" + Request.ServerVariables["SERVER_NAME"] + SFGlobal.SiteRoot + "ws/";
			content.AttachmentLibraryURL = sl + "attachmentlibrary.aspx";
			content.DirectoryURL = sl + "directory.aspx";
			content.ImageLibraryURL = sl + "imagelibrary.aspx";
			content.LinkLibraryURL = sl + "linklibrary.aspx";
			content.SpellCheckerURL = sl + "spellchecker.aspx";
			content.EditorCSS = "./includes/xstandard.css";
			content.CSS = "http://" + Request.ServerVariables["SERVER_NAME"] + SFGlobal.SiteRoot + "includes/xstandard.css";
			content.License = System.Configuration.ConfigurationSettings.AppSettings["XStandardKeyLocation"];
			
			content.Options = 16418;


			//content.Buttons = sl + "buttons.xml";
			//content.Icons = sl + "icons.xml";
			content.BorderColor = Color.Thistle;
			content.Toolbar = "dropdownlistlinker,numbering, bullets, , layout-table, data-table, image, line, hyperlink, attachment, directory, spellchecker, undo, , wysiwyg, source, preview, screen-reader";
			content.IndentOutput = true;
			content.EnableTimestamp = false;
			Response.Filter = new XStandard.WebForms.ResponseFilter(Response.Filter, Response.Output.Encoding);
		
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
