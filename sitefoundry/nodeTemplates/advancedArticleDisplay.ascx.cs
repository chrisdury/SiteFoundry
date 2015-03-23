namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;

	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;


	/// <summary>
	///		Summary description for advancedArticleDisplay.
	/// </summary>
	public class advancedArticleDisplay : Dury.SiteFoundry.UI.BaseControl
	{
		protected PlaceHolder holder;
		private string dbTable = "AdvancedArticles";
		private Node currentNode;
		private string lang;
		private int rank; //page number! 
		private UserIdentity user = (UserIdentity)SFGlobal.CurrentUser.Identity;
		private BaseArticle article;

		private void Page_Load(object sender, System.EventArgs e)
		{
			currentNode = (Node)Context.Items["currentNode"];
			lang = SFGlobal.CurrentCulture.Name;
			rank = SFGlobal.IsNumeric(Page.CustomQueryString["page"]) ? int.Parse(Page.CustomQueryString["page"]) : 1;
			string sql = "";
			string columns = "TOP 1 templateID, title, summary, keywords, body, dateModified ";

			if (SFGlobal.IsUserCMS()) 
			{
				sql = "SELECT " + columns + " FROM " + dbTable + " WHERE nodeID={0} AND lang='{1}' AND rank = {2} ORDER BY preview DESC, publish DESC, dateModified DESC, version DESC";
			} 
			else 
			{
				dbTable += SFGlobal.PublicSuffix;
				sql = "SELECT " + columns + " FROM " + dbTable + " WHERE nodeID = {0}AND lang = '{1}' and rank = {2}";
			}
			sql = String.Format(sql, currentNode.Id, lang, rank);
			DataRow dr = SFGlobal.DAL.execDataRow(sql);
			if (dr != null) 
			{
				article = loadArticle((int)dr["templateID"]);
				article.ArticleTitle = dr["title"].ToString();
				article.ArticleBody = dr["body"].ToString();
				article.Keywords = dr["keywords"].ToString();
				article.Summary = dr["summary"].ToString();
				article.LastModified = (DateTime)dr["dateModified"];
			}
			else
			{
				article = loadArticle(1);
				article.ArticleBody = String.Format("<h2>Article not Found.</h2><p>There is no Article for this node yet. Publish a new article</p>");
				article.ArticleTitle = "Error!";
				article.Keywords="ERROR";
				article.Summary = "ERROR";
				article.LastModified = DateTime.Now;				
				//throw new DuryTools.ErrorHandler("Problem loading article. Possible causes: haven't published yet, no article for this language");
				}
			holder.Controls.Add(article);
		}

        /// <summary>
        /// Retrieves the control specified from the XML config file
        /// </summary>
        /// <param name="id">template id</param>
        /// <returns>Control (which inherits BaseArticle)</returns>
		private BaseArticle loadArticle(int id) 
		{
			ArticleTemplateInfo ati = getArticleTemplateInfo(id);
			return (BaseArticle)Page.LoadControl(SFGlobal.NodeTemplateLocation + "\\" + ati.Src+ ".ascx");
		}

		private System.Collections.Specialized.ListDictionary loadArticleTemplates()
		{
			string appKeyName = "sf_articleTemplates";
			System.Web.HttpContext context = System.Web.HttpContext.Current;

			if (context.Application[appKeyName] != null)
			{
				return (System.Collections.Specialized.ListDictionary)context.Application[appKeyName];
			}
			else
			{
				System.Collections.Specialized.ListDictionary at = new System.Collections.Specialized.ListDictionary();
				System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
				try
				{
					xd.Load(System.AppDomain.CurrentDomain.BaseDirectory + SFGlobal.NodeTemplateLocation + "\\" + System.Configuration.ConfigurationSettings.AppSettings["articleTemplateDefinitions"]);
					foreach (XmlNode xn in xd["templates"])
					{
						at.Add(int.Parse(xn.Attributes["id"].Value),new ArticleTemplateInfo(xn.Attributes["id"].Value,xn.Attributes["name"].Value,xn.Attributes["src"].Value));
					}
					context.Application.Add(appKeyName,at);
				}
				catch(Exception e)
				{
					throw new DuryTools.ErrorHandler("can't load articleTemplate XML...",e);
				}
				return at;
			}
		}
		private ArticleTemplateInfo getArticleTemplateInfo(int id)
		{
			System.Collections.Specialized.ListDictionary at = loadArticleTemplates();
			if (at[id] == null) 
			{
				throw new DuryTools.ErrorHandler("error loading template... check id {" + id.ToString() + "} or make sure articles are published");
			}
			return (ArticleTemplateInfo)at[id];
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
