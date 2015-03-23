namespace Dury.SiteFoundry.NodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;
	using DuryTools.Data;

	/// <summary>
	///		Advanced Article Administration system with multiple pages and multiple versions.s
	/// </summary>
	public class advancedArticleAdmin : DuryTools.UI.BaseControl
	{
		#region Properties

		protected TextBox articleName;
		protected TextBox title;
		protected CheckBox activeCheck;
		protected CheckBox publishCheck;
		protected CheckBox richEditCheck;
		protected DropDownList articleTemplateID;
		protected TextBox summary;
		protected TextBox keywords;
		protected Dury.SiteFoundry.UI.CustomFreeTextBox content;

		protected DropDownList languageSelect;
		protected Button addLanguage;
		protected TextBox newLanguage;
		protected Button deleteLanguage;

		protected DropDownList pageSelect;
		protected DropDownList versionSelect;
		protected Literal pageCount;
		protected Button addPage;
		protected Button savePage;
		protected Button removeArchivePage;
		protected Button archivePage;
		protected Button previewPage;
		protected Button deletePage;
		protected Button publishNowButton;
		protected Literal msg;

		protected System.Web.UI.HtmlControls.HtmlTable articleEditHelp;
		protected System.Web.UI.HtmlControls.HtmlContainerControl articleEditContainer;
		protected System.Web.UI.HtmlControls.HtmlContainerControl bodyEditContainer;
		protected System.Web.UI.HtmlControls.HtmlContainerControl addArticleContainer;

		public string Content;

		private Node currentNode;
		private int version;
		private int rank;
		private string lang;
		private DAL dal = SFGlobal.DAL;
		private string dbTable = "AdvancedArticles";

		#endregion

		#region page 
		private void Page_Load(object sender, System.EventArgs e)
		{
			init();
			applySecurity();
			if (!IsPostBack) this.oneTimeInit();

			if (languageSelect.Items.Count > 0) 
				this.lang = languageSelect.SelectedValue;
			if (versionSelect.Items.Count > 0)
				this.version = int.Parse(versionSelect.SelectedValue);
			if (pageSelect.Items.Count > 0)
				this.rank = int.Parse(pageSelect.SelectedValue);

			if (!IsPostBack) loadPage(1);		
		}

		private void applySecurity()
		{
			if (SFGlobal.GetCurrentUser().Roles.Contains(SFGlobal.AdminstratorRoleName) || SFGlobal.GetCurrentUser().Roles.Contains(SFGlobal.PublisherRoleName))
			{
				deleteLanguage.Enabled = true;
				deletePage.Enabled = true;
				publishNowButton.Enabled = true;
			}
		}

		private void init() 
		{
			//add event handlers
			pageSelect.SelectedIndexChanged += new EventHandler(pageSelect_SelectedIndexChanged);
			languageSelect.SelectedIndexChanged += new EventHandler(languageSelect_SelectedIndexChanged);
			versionSelect.SelectedIndexChanged += new EventHandler(versionSelect_SelectedIndexChanged);
			addPage.Click += new EventHandler(addPage_Click);
			addLanguage.Click += new EventHandler(addLanguage_Click);
			deleteLanguage.Click += new EventHandler(deleteLanguage_Click);
			savePage.Click += new EventHandler(savePage_Click);
			previewPage.Click += new EventHandler(previewPage_Click);
			deletePage.Click += new EventHandler(deletePage_Click);
			removeArchivePage.Click += new EventHandler(removeArchivePage_Click);
			archivePage.Click += new EventHandler(archivePage_Click);

			// get node info
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = NodeFactory.UserNodes.Find(nodeID);
			if (currentNode == null) throw new DuryTools.ErrorHandler("node not found, id=" + nodeID.ToString());

			pageSelect.AutoPostBack = true;
			languageSelect.AutoPostBack = true;
			versionSelect.AutoPostBack = true;

			removeArchivePage.Attributes.Add("onClick","return deleteWarning('this version');");

			deleteLanguage.Enabled = false;
			deleteLanguage.Attributes.Add("onClick","return deleteWarning('all pages for this language');");
			deletePage.Enabled = false;
			deletePage.Attributes.Add("onClick","return deleteWarning('this article page');");
			publishNowButton.Enabled = false;
			publishNowButton.Attributes.Add("onClick","return deleteWarning('to publish this page');");

			publishCheck.Enabled = false;
			//publishCheck.AutoPostBack = true;
			//publishCheck.CheckedChanged += new EventHandler(publishCheck_CheckedChanged);
			activeCheck.AutoPostBack = true;
			activeCheck.CheckedChanged += new EventHandler(activeCheck_CheckedChanged);

			// configure content area
			richEditCheck.AutoPostBack = true;
			richEditCheck.CheckedChanged += new EventHandler(richEditCheck_CheckedChanged);
			//if (content.RenderMode == FreeTextBoxControls.RenderMode.Rich) richEditCheck.Checked = true;
			//if (content.RenderMode == FreeTextBoxControls.RenderMode.Plain) content.TabIndex = 4;

			//content.DownLevelCols = 86;
			//content.DownLevelRows = 20;

			//Page.PageTitle += "Article : ";

		}


		private void oneTimeInit() 
		{
			fillLangSelect();
			fillTemplateSelect();
		}

		private void fillLangSelect() 
		{
			DataSet ds = dal.execDataSet("SELECT DISTINCT(lang) FROM " + this.dbTable + " WHERE nodeID = " + currentNode.Id.ToString());
			languageSelect.DataSource = ds;
			languageSelect.DataTextField = "lang";
			languageSelect.DataValueField = "lang";
			languageSelect.DataBind();
		}
		private void fillPageSelect() 
		{
			string sql = String.Format("SELECT DISTINCT(rank) FROM {0} WHERE preview = 0 AND nodeID = {1} AND lang = '{2}' ORDER BY rank ASC",dbTable,currentNode.Id,lang);
			DataSet ds = dal.execDataSet(sql);
			pageSelect.DataSource = ds;
			pageSelect.DataTextField = "rank";
			pageSelect.DataValueField = "rank";
			pageSelect.DataBind();
		}
		private void fillTemplateSelect() 
		{
			System.Collections.Specialized.ListDictionary at = SFGlobal.LoadArticleTemplates();
			foreach(System.Collections.DictionaryEntry de in at)
			{
				ArticleTemplateInfo ati = (ArticleTemplateInfo)de.Value;
				articleTemplateID.Items.Add(new ListItem(ati.Name,ati.ID));
			}            
		}

		private void fillVersionSelect(int rank) 
		{
			string sql = String.Format("SELECT a.version, a.dateModified, a.rank, a.active, a.userID, u.Username FROM {0} a INNER JOIN SecurityUsers u ON a.userID = u.id WHERE (a.nodeID = {1}) AND (a.rank = {2}) AND preview = 0 AND lang = '{3}' ORDER BY a.version DESC",dbTable,currentNode.Id,rank,lang);
			DataSet ds = dal.execDataSet(sql);
			versionSelect.Items.Clear();
			foreach(System.Data.DataRow dr in ds.Tables[0].Rows)
			{
				//string mark = ((bool)dr["publish"]) ? " •" : "";
				string mark = ((bool)dr["active"]) ? " •" : "";
				string name = String.Format("{0} - ({1} {2})"+mark,dr["version"].ToString(),((DateTime)dr["dateModified"]).ToString(),dr["username"].ToString());
				versionSelect.Items.Add(new ListItem(name,dr["version"].ToString()));
			}
			versionSelect.DataBind();
		}
		private void showPageCount() 
		{
			string sql = String.Format("SELECT COUNT(DISTINCT(rank)) FROM " + dbTable + " WHERE nodeID = {0} AND preview = 0",currentNode.Id);
			int p = (int)dal.execScalar(sql);
			pageCount.Text = p.ToString();
		}

		private int versionCount() 
		{
			string sql = String.Format("SELECT MAX(version) FROM {0} WHERE nodeID = {1} AND rank = {2} AND preview = 0",dbTable,currentNode.Id,int.Parse(pageSelect.SelectedValue));
			return (int)dal.execScalar(sql);
		}

		#endregion

		#region Event Handlers

		private void pageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedPage = int.Parse(pageSelect.SelectedValue);
			loadPage(selectedPage);
		}

		private void languageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			//fillPageSelect(); // can this be moved into loadPage? might interfere with onSelectIndexChanged event detection...
			this.lang = languageSelect.SelectedValue;
			loadPage(1);
		}

		private void versionSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			version = int.Parse(versionSelect.SelectedValue);
			loadPage(this.rank,version);
		}

		private void addPage_Click(object sender, EventArgs e)
		{
			int newrank = rank + 1;
			insertPage(); // bump up other pages
			createArticlePage(lang,newrank);
			loadPage(newrank);
		}

		private void deletePage_Click(object sender, EventArgs e)
		{
			deleteArticlePage();
			removePage();
			Response.Redirect(Request.RawUrl);
		}

		private void addLanguage_Click(object sender, EventArgs e)
		{
			createArticlePage(newLanguage.Text,1);
			Response.Redirect(Request.RawUrl);
		}

		private void deleteLanguage_Click(object sender, EventArgs e)
		{
			deleteLanguagePage();
		}

		private void savePage_Click(object sender, EventArgs e)
		{
			saveCurrentPage();
			loadPage();
		}
		private void previewPage_Click(object sender, EventArgs e)
		{
			createPreview();
			Response.Redirect(currentNode.URL);
		}
		private void removeArchivePage_Click(object sender, EventArgs e)
		{
			removeVersion();
		}

		// "save new"
		private void archivePage_Click(object sender, EventArgs e)
		{
			createNewVersion();
		}

		/*
		private void publishCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (publishCheck.Checked)
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 0 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND lang = '" + lang + "'");
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 1 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + this.version + " AND lang = '" + lang + "'");
			} 
			else 
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 0 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + this.version + " AND lang = '" + lang + "'");			
			}
			loadPage(rank,version);
		}
		*/

		private void activeCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (activeCheck.Checked)
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET active = 0 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND lang = '" + lang + "'");
				dal.execNonQuery("UPDATE " + dbTable + " SET active = 1 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + this.version + " AND lang = '" + lang + "'");
				publishCheck.Checked = true;
			} 
			else 
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET active = 0 WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + this.version + " AND lang = '" + lang + "'");			
			}
			loadPage(rank,version);
		}

		

		private void richEditCheck_CheckedChanged(object sender, EventArgs e)
		{
			//content.RenderMode = (richEditCheck.Checked)? FreeTextBoxControls.RenderMode.Rich : FreeTextBoxControls.RenderMode.Plain;
		}


		#endregion

		#region load and save
		private void deleteLanguagePage() 
		{
			//DataContainer dc = new DataContainer(dbTable);
			SFGlobal.DAL.execNonQuery("DELETE FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND lang = '" + lang + "'");
			Response.Redirect(Request.RawUrl.Substring(0,Request.RawUrl.IndexOf("?")));
		}

		private void deleteArticlePage() 
		{
			this.dal.execNonQuery(String.Format("DELETE FROM {0} WHERE nodeID = {1} AND lang = '{2}' AND rank = {3}",dbTable, currentNode.Id,lang,rank));
			loadPage();
		}

		private void createArticlePage(string language, int rank) 
		{
			DataContainer dc = new DataContainer(dbTable);
			DataRow dr = dc.GetNewRow();
			dr["nodeID"] = currentNode.Id;
			dr["templateID"] = 1;
			dr["lang"] = language;
			dr["version"] = 1;
			dr["rank"] = rank;
			dr["title"] = currentNode.getName(SFGlobal.DefaultLanguage);
			dr["summary"] = "new summary";
			dr["keywords"] = "new keywords";
			dr["body"] = "new body for " + currentNode.getName(SFGlobal.DefaultLanguage);
			dr["userID"] = SFGlobal.GetCurrentUser().ID;
			dr["publish"] = false;
			dr["preview"] = false;
			dr["active"] = false;
			dr["dateCreated"] = dr["dateModified"] = System.DateTime.Now;			
			dc.UpdateRow(dr);
		}

		private void loadPage()
		{
			loadPage(rank,version);
		}

		private void loadPage(int rank)
		{
			loadPage(rank,0);
		}

		private void loadPage(int rank, int version)
		{
			this.rank = rank;
			fillPageSelect();
			showPageCount();
			fillVersionSelect(rank);

			string sql = "SELECT * FROM {0} WHERE nodeID = {1} AND preview = 0 AND rank = {2} AND lang = '{3}' AND version = ";
			if (version == 0)
				sql += " (SELECT MAX(version) FROM {0} WHERE nodeID = {1} AND lang = '{3}' AND rank = {2})";
			else
				sql += version.ToString();

			sql = String.Format(sql,dbTable,currentNode.Id,rank,lang);

			DataSet ds = dal.execDataSet(sql);
			if (ds.Tables[0].Rows.Count > 0) 
			{
				DataRow dr = ds.Tables[0].Rows[0];
				title.Text = dr["title"].ToString();
				summary.Text = dr["summary"].ToString();
				keywords.Text = dr["keywords"].ToString();
				content.Text = dr["body"].ToString();
				//Content = dr["body"].ToString();
				pageSelect.SelectedValue = rank.ToString();
				versionSelect.SelectedValue = dr["version"].ToString();
				articleTemplateID.SelectedValue = dr["templateID"].ToString();
				publishCheck.Checked = (dr["publish"] != null) ? (bool)dr["publish"] : false;
				activeCheck.Checked = (dr["active"] != null) ? (bool)dr["active"] : false;
				showPageCount();
				bodyEditContainer.Visible = true;
				articleEditContainer.Visible = true;
				//Page.PageTitle += dr["title"].ToString();
				checkActive();
				if (activeCheck.Checked == true) publishCheck.Enabled = true;
			} 
			else
			{
				articleEditContainer.Visible = true;
				articleEditHelp.Visible = true;
				newLanguage.Text = SFGlobal.DefaultLanguage;
			}
			
		}


		/// <summary>
		/// Check and make sure a version is set to active.
		/// </summary>
		private void checkActive() 
		{
			//int o = (int)SFGlobal.DAL.execScalar("SELECT COUNT(id) FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND active = 1");
			int o = SFGlobal.ObjectManager.GetObjectCount(typeof(AdvancedArticle),"nodeID = " + currentNode.Id + " AND lang = '" + this.lang + "' AND rank = " + rank + " AND active = 1");
			if (o>=1) 
			{
				activeCheck.ForeColor = Color.Black;
				activeCheck.ToolTip = "";
			} 
			else 
			{
				activeCheck.ForeColor = Color.Red;
				activeCheck.ToolTip = "There is no active version of this article page. You must have an active version before publishing.";
			}
			//activeCheck.ForeColor = (o==1) ? Color.Black : Color.Red;
		}




		private void saveCurrentPage() 
		{
			this.removePreviewArticle();
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetRowByCustomSql("SELECT * FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + version);
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["userID"] = ((UserIdentity)SFGlobal.GetCurrentUser().Identity).ID;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = publishCheck.Checked;
			dr["active"] = activeCheck.Checked;
			dc.UpdateRow(dr);
		}
		private void createNewVersion() 
		{
			this.removePreviewArticle();
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetNewRow();
			dr["nodeID"] = currentNode.Id;
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["version"] = versionCount() + 1;
			dr["lang"] = lang;
			dr["rank"] = rank;
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["userID"] = ((UserIdentity)SFGlobal.GetCurrentUser().Identity).ID;
			dr["dateCreated"] = dr["dateModified"] = System.DateTime.Now;			
			dr["publish"] = false;
			dr["preview"] = false;
			dr["active"] = false;
			dc.UpdateRow(dr);
			loadPage(rank);
		}

		private void removeVersion()
		{
			removePreviewArticle();
			this.dal.execNonQuery("DELETE FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + version + " AND lang = '" + lang +  "'");
			loadPage(rank);            
		}

		private void createPreview()
		{
			// first delete any previews from before
			this.removePreviewArticle();

			// now put the new preview in
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetNewRow();
			dr["nodeID"] = currentNode.Id;
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["version"] = 0;
			dr["lang"] = lang;
			dr["rank"] = rank;
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["userID"] = ((UserIdentity)SFGlobal.GetCurrentUser().Identity).ID;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = false;
			dr["preview"] = true;
			dr["active"] = true;
			dc.UpdateRow(dr);
		}

		private void insertPage()
		{
			string sql = String.Format("UPDATE " + dbTable + " SET rank = rank + 1 WHERE (rank > {0}) AND nodeID = {1} AND lang = '{2}'",rank,currentNode.Id,lang);
			this.dal.execNonQuery(sql);
		}

		private void removePage()
		{
			string sql = String.Format("UPDATE " + dbTable + " SET rank = rank - 1 WHERE (rank > {0}) AND nodeID = {1} AND lang = '{2}'",rank,currentNode.Id,lang);
			this.dal.execNonQuery(sql);
		}

		private void removePreviewArticle() 
		{
			string sql = String.Format("DELETE FROM " + dbTable + " WHERE preview = 1 AND nodeID = {0} AND lang = '{1}' AND userID = {2}",currentNode.Id,lang,((UserIdentity)SFGlobal.GetCurrentUser().Identity).ID);
			dal.execNonQuery(sql);
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
