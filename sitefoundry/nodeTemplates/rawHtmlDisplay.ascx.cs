namespace Dury.SiteFoundry.nodeTemplates
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
	///		Summary description for rawHtmlDisplay.
	/// </summary>
	public class rawHtmlDisplay : Dury.SiteFoundry.UI.BaseControl
	{
		private Node currentNode;
		protected System.Web.UI.WebControls.Literal body;
		private DuryTools.Data.DAL dal = new DuryTools.Data.DAL();

		private string publicSuffix = "_public";

		private void Page_Load(object sender, System.EventArgs e)
		{
			currentNode = (Node)Context.Items["currentNode"];
            if (currentNode == null) throw new DuryTools.ErrorHandler("currentNode not found!");

			if (SFGlobal.IsUserCMS())
				publicSuffix = "";


			DataSet ds = dal.execDataSet("SELECT showTemplate,title,body FROM SimpleArticles " + publicSuffix + " WHERE nodeID = " + currentNode.Id);
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow dr = ds.Tables[0].Rows[0];
				Page.ShowTemplate = (dr["showTemplate"] != System.DBNull.Value) ? (bool)dr["showTemplate"] : false;
				Page.PageTitle = dr["title"].ToString();
				body.Text = dr["body"].ToString();
			}
			else
			{
				body.Text = "content not found in database.";

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
