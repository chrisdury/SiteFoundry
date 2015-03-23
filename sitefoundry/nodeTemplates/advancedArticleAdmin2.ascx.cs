namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Article;

	/// <summary>
	///		Summary description for advancedArticleAdmin2.
	/// </summary>
	public class advancedArticleAdmin2 : System.Web.UI.UserControl
	{
		protected PlaceHolder contentHolder;
		protected DropDownList contentEditorList;

		private void Page_Load(object sender, System.EventArgs e)
		{
			contentEditorList.AutoPostBack = true;
			contentEditorList.SelectedIndexChanged += new EventHandler(contentEditorList_SelectedIndexChanged);

			if (!IsPostBack) 
			{
				fillEditorList();


			}
			loadEditor();
		}

		#region content editor
		private void contentEditorList_SelectedIndexChanged(object sender, EventArgs e)
		{
			loadEditor();
		}
		private void fillEditorList()
		{
			foreach(ContentEditorInfo cei in Dury.SiteFoundry.Article.ContentEditors.GetContentEditorList())
			{
				contentEditorList.Items.Add(new ListItem(cei.Name,cei.ID.ToString()));
			}
			contentEditorList.DataBind();			
		}
		private void loadEditor() 
		{
			int editorID = int.Parse(contentEditorList.SelectedValue);
			ContentEditorInfo cei = Dury.SiteFoundry.Article.ContentEditors.GetContentEditorInfo(editorID);
            IContentEditor ic = (IContentEditor)Page.LoadControl("~/nodeTemplates/articleContentEditors/" + cei.Src);
			ic.Height = cei.Height;
			ic.Width = cei.Width;
			ic.Text = "hello";
			contentHolder.Controls.Clear();
			contentHolder.Controls.Add((System.Web.UI.UserControl)ic);
		}
		#endregion




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
