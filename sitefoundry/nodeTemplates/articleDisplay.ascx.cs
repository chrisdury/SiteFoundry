namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;

	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;

	public class Article : Dury.SiteFoundry.UI.BaseControl
	{
		protected System.Web.UI.WebControls.PlaceHolder articleHolder;
		private string articleBodyTable = "ArticleBodies";
		private string articleContainerTable = "ArticleContainers";
		private string publicSuffix = "_public";
		private string lang;
		private string title;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// load the article control
			Node currentNode = (Node)Context.Items["currentNode"];
			if (SFGlobal.IsUserCMS())publicSuffix = "";            
			BaseArticle article;

			//SFGlobal.SetLangSession();
			
			if (Session["lang"] != null)
				lang = Session["lang"].ToString();//Context.Items["lang"];
			else
				lang = SFGlobal.DefaultLanguage;

			string sql = string.Format("SELECT b.title, b.summary, b.body, b.dateModified, b.templateID ,b.summary, b.keywords FROM " + this.articleBodyTable + this.publicSuffix + " b INNER JOIN " + this.articleContainerTable + this.publicSuffix + " a ON b.articleID = a.id WHERE (a.nodeID = {0}) AND (b.lang = '{1}')",currentNode.Id.ToString(),lang);
			DataSet ds = SFGlobal.DAL.execDataSet(sql);
			string title;
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow dr = ds.Tables[0].Rows[0];
				ArticleTemplateInfo at = SFGlobal.GetArticleTemplate((int)dr["templateID"]);
				article = (BaseArticle)Page.LoadControl(SFGlobal.NodeTemplateLocation + "\\" + at.Src+ ".ascx");
				article.ArticleTitle = dr["title"].ToString();
				article.ArticleBody = dr["body"].ToString();
				article.LastModified = (dr["dateModified"] != DBNull.Value) ? (DateTime)dr["dateModified"] : System.DateTime.Now;
				article.Keywords = dr["keywords"].ToString();
				article.Summary = dr["summary"].ToString();
				title = dr["title"].ToString();
			}
			else
			{
				ArticleTemplateInfo at = SFGlobal.GetArticleTemplate(1);
				article = (BaseArticle)Page.LoadControl(SFGlobal.NodeTemplateLocation + "\\" + at.Src + ".ascx");				
				article.ArticleTitle = "problem!";
				article.ArticleBody = "article doesn't exist for this node.";
				title = "BIG PROBLEM!";
			}
			articleHolder.Controls.Add(article);

			/*
			Node p = currentNode.parent;
			if (p.parent != null)
				while(p.parent != null) 
				{
					title = ": " + p.getName(SFGlobal.DefaultLanguage) + title;
					p = p.parent;
				}
			*/
			
		}
		/*
		protected override void OnPreRender(EventArgs e)
		{
			Page.PageTitle += ": " + title;
			base.OnPreRender (e);
		}
		*/








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
