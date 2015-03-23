namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;
	using Wilson.ORMapper;
	using Dury.SiteFoundry.Security;
	using DuryTools.Data;

	public class advancedArticle2 : DuryTools.UI.BaseControl
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
		protected XStandard.WebForms.XHTMLEditor body1;

		protected DropDownList languageSelect;
		protected Button addLanguage;
		protected TextBox newLanguage;
		protected Button deleteLanguage;

		protected DropDownList pageSelect;
		protected DropDownList versionSelect;
		protected Literal pageCount;
		protected Button addPage;
		protected Button savePage;
		protected Button removeVersionPage;
		protected Button versionPage;
		protected Button previewPage;
		protected Button deletePage;
		protected Button publishNowButton;
		protected Literal msg;

		protected HtmlTable articleEditHelp;
		protected HtmlContainerControl articleEditContainer;
		protected HtmlContainerControl bodyEditContainer;
		protected HtmlContainerControl addArticleContainer;

		public string Content;

		private AdvancedArticle currentArticle;
		private Node currentNode;
		private int version;
		private int rank;
		private string lang;
		private DuryTools.Data.DAL dal = SFGlobal.DAL;
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
			//if (SFGlobal.GetCurrentUser().Roles.Contains(SFGlobal.AdminstratorRoleName) || SFGlobal.GetCurrentUser().Roles.Contains(SFGlobal.PublisherRoleName))
			if (SFGlobal.CurrentUser.IsUserAdmin())
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
			removeVersionPage.Click += new EventHandler(removeArchivePage_Click);
			versionPage.Click += new EventHandler(archivePage_Click);
			publishNowButton.Click += new EventHandler(publishNowButton_Click);

			// get node info
			/*
			Node root = (Node)Application["nodeRoot"];
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = root.Find(nodeID);
			*/
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = NodeFactory.UserNodes.Find(nodeID);
			if (currentNode == null) throw new DuryTools.ErrorHandler("node not found, id=" + nodeID.ToString());

			pageSelect.AutoPostBack = true;
			languageSelect.AutoPostBack = true;
			versionSelect.AutoPostBack = true;

			removeVersionPage.Attributes.Add("onClick","return deleteWarning('this version');");

			deleteLanguage.Enabled = false;
			deleteLanguage.Attributes.Add("onClick","return deleteWarning('all pages for this language');");
			deletePage.Enabled = false;
			deletePage.Attributes.Add("onClick","return deleteWarning('this article page');");
			publishNowButton.Enabled = false;
			publishNowButton.Attributes.Add("onClick","return confirm('Are you sure you want to publish this page immediately?');");

			//publishCheck.Enabled = false;
			//publishCheck.AutoPostBack = true;
			//publishCheck.CheckedChanged += new EventHandler(publishCheck_CheckedChanged);
			activeCheck.AutoPostBack = true;
			activeCheck.CheckedChanged += new EventHandler(activeCheck_CheckedChanged);
			//Page.PageTitle += "Article : ";

			string sl = "http://" + Request.ServerVariables["SERVER_NAME"] + SFGlobal.SiteRoot + "ws/";
			body1.AttachmentLibraryURL = sl + "attachmentlibrary.aspx";
			body1.DirectoryURL = sl + "directory.aspx";
			body1.ImageLibraryURL = sl + "imagelibrary.aspx";
			body1.LinkLibraryURL = sl + "linklibrary.aspx";
			body1.SpellCheckerURL = sl + "spellchecker.aspx";
			body1.EditorCSS = "./includes/xstandard.css";
			body1.CSS = "http://" + Request.ServerVariables["SERVER_NAME"] + SFGlobal.SiteRoot + "includes/xstandard.css";
			body1.License = System.Configuration.ConfigurationSettings.AppSettings["XStandardKeyLocation"];

			body1.Options = 16418;

			body1.Buttons = sl + "buttons.xml";
			body1.Icons = sl + "icons.xml";
			body1.BorderColor = Color.Thistle;
			body1.Toolbar = "numbering, bullets, , layout-table, data-table, image, line, hyperlink, attachment, dropdownlistlinker, spellchecker, undo, , wysiwyg, source, preview,,";
			body1.IndentOutput = true;
			body1.EnableTimestamp = false;
			Response.Filter = new XStandard.WebForms.ResponseFilter(Response.Filter, Response.Output.Encoding);
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

		private void publishNowButton_Click(object sender, EventArgs e)
		{
			AdvancedArticle publicArticle;
			int c = SFGlobal.ObjectManagerPublic.GetObjectCount(typeof(AdvancedArticle),"nodeID=" + currentNode.Id + " AND lang = '" + languageSelect.SelectedValue + "' AND rank=" + pageSelect.SelectedValue);
			if (c > 0)
				publicArticle = (AdvancedArticle)SFGlobal.ObjectManagerPublic.GetObject(new OPathQuery(typeof(AdvancedArticle),"NodeID=" + currentNode.Id + " && Lang='" + languageSelect.SelectedValue + "' && Rank=" + pageSelect.SelectedValue));
			else
				publicArticle = (AdvancedArticle)SFGlobal.ObjectManagerPublic.GetObject(typeof(AdvancedArticle));

			AdvancedArticle currentArticle = (AdvancedArticle)SFGlobal.ObjectManager.GetObject(new OPathQuery(typeof(AdvancedArticle),"NodeID=" + currentNode.Id + " && Lang='" + languageSelect.SelectedValue + "' && Rank=" + pageSelect.SelectedValue));

			publicArticle.Rank = currentArticle.Rank;
			publicArticle.Title = currentArticle.Title;
			publicArticle.Body = currentArticle.Body;
			publicArticle.Keyword = currentArticle.Keyword;
			publicArticle.Lang = currentArticle.Lang;
			publicArticle.NodeID = currentArticle.NodeID;
			publicArticle.Summary = currentArticle.Summary;
			publicArticle.TemplateID = currentArticle.TemplateID;
			publicArticle.UserID = currentArticle.UserID;
			publicArticle.DateCreated = currentArticle.DateCreated;
			publicArticle.DateModified= currentArticle.DateModified;

			SFGlobal.ObjectManagerPublic.PersistChanges(publicArticle);
			currentArticle.Publish = false;
			SFGlobal.ObjectManager.PersistChanges(currentArticle);
            publishCheck.Checked = false;
			msg.Text = "article published";

		}

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
			//loadPage();
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
			AdvancedArticle aa = (AdvancedArticle)SFGlobal.ObjectManager.GetObject(typeof(AdvancedArticle));
            aa.NodeID = currentNode.Id;
			aa.TemplateID = 1;
			aa.Lang = language;
			aa.Version = 1;
			aa.Rank = rank;
			aa.Title = currentNode.getName(SFGlobal.DefaultLanguage);
			aa.Body = "new body text";
			aa.UserID = SFGlobal.CurrentUser.ID;
			aa.Publish = false;
			aa.Preview = false;
			aa.Active = true;
			aa.DateCreated = aa.DateModified = DateTime.Now;
			SFGlobal.ObjectManager.PersistChanges(aa);
			msg.Text = "page created";
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

			//string whereClause = "NodeID == {0} && !Preview && Rank == {1} && Lang == '{2}' && Version == ";
			string whereClause = "NodeID = {0} AND Preview <> 1 AND Rank = {1} AND Lang = '{2}' AND Version = ";
			if (version == 0) 
			{
				try 
				{
					int i = (int)dal.execScalar(String.Format("SELECT MAX(version) FROM {0} WHERE nodeID = {1} AND lang = '{3}' AND rank = {2}",dbTable,currentNode.Id,rank,lang));
					whereClause += i.ToString();
				} 
				catch(System.InvalidCastException e)
				{
					whereClause += "1";
				}
			}
			else
				whereClause += version.ToString();
			
			whereClause = String.Format(whereClause,currentNode.Id,rank,lang);
			ObjectSet os = SFGlobal.ObjectManager.GetObjectSet(new OPathQuery(typeof(AdvancedArticle),whereClause,""));
			
			AdvancedArticle aa = null;
			
			if (os.Count > 0) aa = (AdvancedArticle)os[0];

			if (aa != null)
			{
				title.Text = aa.Title;
				summary.Text = aa.Summary;
				keywords.Text = aa.Keyword;
				body1.Value = aa.Body;
				pageSelect.SelectedValue = rank.ToString();
				versionSelect.SelectedValue = aa.Version.ToString();
				articleTemplateID.SelectedValue = aa.TemplateID.ToString();
				publishCheck.Checked = aa.Publish;
				activeCheck.Checked = aa.Active;
				showPageCount();
				bodyEditContainer.Visible = true;
				articleEditContainer.Visible = true;
				articleEditHelp.Visible = false;
				//Page.PageTitle += aa.Title;
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

		private void checkActive() 
		{
			//int o = (int)SFGlobal.DAL.execScalar("SELECT COUNT(id) FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND lang = '" + this.lang + "' AND rank = " + rank + " AND active = 1");
			int o = SFGlobal.ObjectManager.GetObjectCount(typeof(AdvancedArticle),"nodeID = " + currentNode.Id + " AND lang = '" + this.lang + "' AND rank = " + rank + " AND active = 1");


			if (o==1) 
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
			AdvancedArticle aa = (AdvancedArticle)SFGlobal.ObjectManager.GetObject(new OPathQuery(typeof(AdvancedArticle),String.Format("NodeID=={0} && Rank=={1} && Version=={2} && Lang='{3}'",currentNode.Id,rank,version,lang),""));
			aa.TemplateID = int.Parse(articleTemplateID.SelectedValue);
			aa.Title = title.Text;
			aa.Summary = summary.Text;
			aa.Keyword = keywords.Text;
			aa.Body = body1.Value;
			aa.UserID = SFGlobal.CurrentUser.ID;
			aa.DateModified = DateTime.Now;
			aa.Publish = publishCheck.Checked;
			aa.Active = activeCheck.Checked;
			SFGlobal.ObjectManager.PersistChanges(aa);
			msg.Text = "changes saved";

		}
		private void createNewVersion() 
		{
			this.removePreviewArticle();
			AdvancedArticle aa = (AdvancedArticle)SFGlobal.ObjectManager.GetObject(typeof(AdvancedArticle));
			aa.NodeID = currentNode.Id;
			aa.TemplateID = int.Parse(articleTemplateID.SelectedValue);
			aa.Version = versionCount() + 1;
			aa.Lang = lang;
			aa.Rank = rank;
			aa.Title = title.Text;
			aa.Summary = summary.Text;
			aa.Keyword = keywords.Text;
			aa.Body = body1.Value;
			aa.UserID = SFGlobal.CurrentUser.ID;
			aa.DateCreated = aa.DateModified = DateTime.Now;
			aa.Publish = false;
			aa.Active = true;
			aa.Preview = false;
			SFGlobal.ObjectManager.PersistChanges(aa);
			loadPage(rank);
			msg.Text = "new version created";
		}

		private void removeVersion()
		{
			removePreviewArticle();
			this.dal.execNonQuery("DELETE FROM " + dbTable + " WHERE nodeID = " + currentNode.Id + " AND rank = " + rank + " AND version = " + version + " AND lang = '" + lang +  "'");
			loadPage(rank);
			msg.Text = "version removed";
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
			dr["body"] = body1.Value;
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
			msg.Text = "page inserted";
		}

		private void removePage()
		{
			string sql = String.Format("UPDATE " + dbTable + " SET rank = rank - 1 WHERE (rank > {0}) AND nodeID = {1} AND lang = '{2}'",rank,currentNode.Id,lang);
			this.dal.execNonQuery(sql);
			msg.Text = "page removed";
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

