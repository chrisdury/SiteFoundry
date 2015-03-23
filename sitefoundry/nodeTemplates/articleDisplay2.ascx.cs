namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for articleDisplay2.
	/// </summary>
	public class articleDisplay2 : Dury.SiteFoundry.UI.BaseControl
	{
		protected PlaceHolder holder;
		private Node currentNode;
		private BaseArticle article;
		private string publicSuffix = SFGlobal.PublicSuffix;
		private string lang = SFGlobal.GetUserLanguage();

		private void Page_Load(object sender, System.EventArgs e)
		{
			currentNode = (Node)Context.Items["currentNode"]; // get our current node from the context stack
			string sql = "";
			object id = null;

            
			// lets give the CMS user special treatment
			if (SFGlobal.IsUserCMS()) 
			{
				publicSuffix = "";
				// see if there is a preview assigned to this node
				//sql = "SELECT b.versionNumber, b.pageNumber, b.title, b.summary, b.body, b.dateModified, b.templateID, b.keywords FROM  sf_ArticlePages b INNER JOIN sf_Articles a ON b.articleID = a.id WHERE (a.nodeID = " + currentNode.Id + ") AND (a.lang = '" + lang + "') AND (b.preview = 1)";
				id = SFGlobal.DAL.execScalar(String.Format("SELECT b.id FROM sf_ArticlePages b INNER JOIN sf_Articles a ON b.articleID = a.id WHERE (a.nodeID = {0}) AND (b.preview = 1) AND (a.lang = '{1}')", currentNode.Id, this.lang));
				Response.Write("1=" + id);
                

				// otherwise show the article set for publish
				if (id == null) 
				{
					id = SFGlobal.DAL.execScalar(String.Format("SELECT b.id FROM sf_ArticlePages b INNER JOIN sf_Articles a ON b.articleID = a.id WHERE (a.nodeID = {0}) AND (a.lang = '{1}') AND (b.publish = 1)", currentNode.Id, this.lang));
					Response.Write("2=" + id);
				}


				// otherwise show the greatest version number
				if (id == null) 
				{
					id = SFGlobal.DAL.execScalar(String.Format("SELECT TOP 1 b.id FROM sf_ArticlePages b INNER JOIN sf_Articles a ON b.articleID = a.id WHERE (a.nodeID = {0}) AND (a.lang = '{1}') ORDER BY b.versionNumber DESC", currentNode.Id, this.lang));
					Response.Write("3=" + id);
				}

			} 
			else 
			{
				//id = (int)SFGlobal.DAL.execScalar(String.Format("SELECT TOP 1 b.id FROM sf_ArticlePages" + this.publicSuffix + " b INNER JOIN sf_Articles" + this.publicSuffix + " a ON b.articleID = a.id WHERE (a.nodeID = {0}) AND (a.lang = '{1}')", currentNode.Id, this.lang));
			}

			if (id == null) 
			{
				throw new DuryTools.ErrorHandler("can't find article for this node! help!");
			}
			else 
			{
                DataSet ds = SFGlobal.DAL.execDataSet("SELECT * FROM sf_ArticlePages" + this.publicSuffix + " WHERE id = " + id.ToString());
				if (ds.Tables[0].Rows.Count >= 1)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					ArticleTemplateInfo at = SFGlobal.GetArticleTemplate((int)dr["templateID"]);
					article = (BaseArticle)Page.LoadControl(SFGlobal.NodeTemplateLocation + "\\" + at.Src+ ".ascx");
					article.ArticleTitle = dr["title"].ToString();
					article.ArticleBody = dr["body"].ToString();
					article.LastModified = (dr["dateModified"] != DBNull.Value) ? (DateTime)dr["dateModified"] : System.DateTime.Now;
					article.Keywords = dr["keywords"].ToString();
					article.Summary = dr["summary"].ToString();
					//title = dr["title"].ToString();
					holder.Controls.Add(article);

					Response.Write(Page.CustomQueryString["page"]);

				}
				else
				{
					throw new DuryTools.ErrorHandler("Can't load article id: " + id.ToString());
				}
			}

			//DataSet ds = SFGlobal.DAL.execDataSet(sql);		
			//Response.Write(ds.Tables[0].Rows.Count);
			
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
