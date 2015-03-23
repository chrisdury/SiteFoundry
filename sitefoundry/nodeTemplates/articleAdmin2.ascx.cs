namespace sitefoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using HolmesAndLee.SiteFoundry;
	using HolmesAndLee.SiteFoundry.Security;
	using HolmesAndLee.Data;

	/// <summary>
	///		Summary description for articleAdmin2.
	/// </summary>
	public class articleAdmin2 : HolmesAndLee.UI.BaseControl
	{
		#region properties

		protected System.Web.UI.WebControls.TextBox articleName;
		protected System.Web.UI.WebControls.TextBox title;
		protected System.Web.UI.WebControls.CheckBox publishCheck;
		protected System.Web.UI.WebControls.CheckBox richEditCheck;
		protected System.Web.UI.WebControls.DropDownList articleTemplateID;
		protected System.Web.UI.WebControls.TextBox summary;
		protected System.Web.UI.WebControls.TextBox keywords;
		//protected FreeTextBoxControls.FreeTextBox content;
		protected HolmesAndLee.SiteFoundry.UI.CustomFreeTextBox content;

		protected System.Web.UI.WebControls.DropDownList languageSelect;
		protected System.Web.UI.WebControls.Button addLanguage;
		protected System.Web.UI.WebControls.TextBox newLanguage;
		protected System.Web.UI.WebControls.Button deleteLanguage;

		protected System.Web.UI.WebControls.DropDownList pageSelect;
		protected System.Web.UI.WebControls.DropDownList versionSelect;
		protected System.Web.UI.WebControls.Literal pageCount;
		protected System.Web.UI.WebControls.Button addPage;
		protected System.Web.UI.WebControls.Button savePage;
		protected System.Web.UI.WebControls.Button removeArchivePage;
		protected System.Web.UI.WebControls.Button archivePage;
		protected System.Web.UI.WebControls.Button previewPage;
		protected System.Web.UI.WebControls.Button deletePage;
		protected System.Web.UI.WebControls.Literal msg;

		protected System.Web.UI.HtmlControls.HtmlTable articleEditHelp;
		protected System.Web.UI.HtmlControls.HtmlContainerControl articleEditContainer;
		protected System.Web.UI.HtmlControls.HtmlContainerControl bodyEditContainer;
		protected System.Web.UI.HtmlControls.HtmlContainerControl addArticleContainer;

		private UserPrincipal user;
		private Node currentNode;
		private int articleID;
		private int versionNumber;
		private int pageNumber;
		private DAL dal = SFGlobal.DAL;
		private string dbTable = "sf_ArticlePages";

		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			init();
			applySecurity();
			if (!IsPostBack) this.oneTimeInit();

			if (languageSelect.Items.Count > 0) 
				this.articleID = int.Parse(languageSelect.SelectedValue);
			if (versionSelect.Items.Count > 0)
				this.versionNumber = int.Parse(versionSelect.SelectedValue);
			if (pageSelect.Items.Count > 0)
				this.pageNumber = int.Parse(pageSelect.SelectedValue);

			if (!IsPostBack) loadPage(1);		
		}

		private void applySecurity()
		{
			if (user.Roles.Contains(SFGlobal.AdminstratorRoleName) || user.Roles.Contains(SFGlobal.PublisherRoleName))
			{
				deleteLanguage.Enabled = true;
				deletePage.Enabled = true;
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
			Node root = (Node)Application["nodeRoot"];
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = root.Find(nodeID);
			if (currentNode == null) throw new HolmesAndLee.ErrorHandler("node not found, id=" + nodeID.ToString());

			pageSelect.AutoPostBack = true;
			languageSelect.AutoPostBack = true;
			versionSelect.AutoPostBack = true;

			removeArchivePage.Attributes.Add("onClick","return deleteWarning('this version');");

			deleteLanguage.Enabled = false;
			deleteLanguage.Attributes.Add("onClick","return deleteWarning('all pages for this language');");
			deletePage.Enabled = false;
			deletePage.Attributes.Add("onClick","return deleteWarning('this article page');");

			publishCheck.AutoPostBack = true;
			publishCheck.CheckedChanged += new EventHandler(publishCheck_CheckedChanged);

			// configure content area
			richEditCheck.AutoPostBack = true;
			richEditCheck.CheckedChanged += new EventHandler(richEditCheck_CheckedChanged);
			if (content.RenderMode == FreeTextBoxControls.RenderMode.Rich) richEditCheck.Checked = true;
			if (content.RenderMode == FreeTextBoxControls.RenderMode.Plain) content.TabIndex = 4;

			content.DownLevelCols = 86;
			content.DownLevelRows = 20;

			user = (UserPrincipal)Context.User;
			Page.PageTitle += "Article : ";

		}

		private void oneTimeInit() 
		{
			fillLangSelect();
			//fillPageSelect();
			fillTemplateSelect();
		}

		private void fillLangSelect() 
		{
			DataSet ds = dal.execDataSet("SELECT DISTINCT(lang), id FROM sf_Articles WHERE nodeID = " + currentNode.Id.ToString());
			languageSelect.DataSource = ds;
			languageSelect.DataTextField = "lang";
			languageSelect.DataValueField = "id";
			languageSelect.DataBind();
		}
		private void fillPageSelect() 
		{
			string sql = String.Format("SELECT DISTINCT(pageNumber) FROM " + dbTable + " WHERE articleID = {0}  AND preview = 0 ORDER BY pageNumber ASC",this.articleID);
			DataSet ds = dal.execDataSet(sql);
			pageSelect.DataSource = ds;
			pageSelect.DataTextField = "pageNumber";
			pageSelect.DataValueField = "pageNumber";
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

		private void fillVersionSelect(int pageNumber) 
		{
			string sql = String.Format("SELECT a.versionNumber, a.dateModified, a.pageNumber, a.publish, a.editedBy, u.Username FROM sf_ArticlePages a INNER JOIN SecurityUsers u ON a.editedBy = u.id WHERE (a.articleID = " + this.articleID + ") AND (a.pageNumber = {0}) AND preview = 0 ORDER BY a.versionNumber DESC",pageNumber);
			DataSet ds = dal.execDataSet(sql);
			versionSelect.Items.Clear();
			foreach(System.Data.DataRow dr in ds.Tables[0].Rows)
			{
				string mark = ((bool)dr["publish"]) ? " •" : "";
				string name = String.Format("{0} - ({1} {2})"+mark,dr["versionNumber"].ToString(),((DateTime)dr["dateModified"]).ToString(),dr["username"].ToString());
				versionSelect.Items.Add(new ListItem(name,dr["versionNumber"].ToString()));
			}
			versionSelect.DataBind();
		}
		private void showPageCount() 
		{
			string sql = String.Format("SELECT COUNT(DISTINCT(pageNumber)) FROM " + dbTable + " WHERE articleID = {0} AND preview = 0",articleID);
			int p = (int)dal.execScalar(sql);
			pageCount.Text = p.ToString();
		}

		private int versionCount() 
		{
			string sql = String.Format("SELECT MAX(versionNumber) FROM " + dbTable + " WHERE articleID = {0} AND pageNumber = {1} AND preview = 0",articleID,int.Parse(pageSelect.SelectedValue));
			return (int)dal.execScalar(sql);
		}


		#region event handlers
		private void pageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedPage = int.Parse(pageSelect.SelectedValue);
			loadPage(selectedPage);
		}

		private void languageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			//fillPageSelect(); // can this be moved into loadPage? might interfere with onSelectIndexChanged event detection...
			this.articleID = int.Parse(languageSelect.SelectedValue);
			loadPage(1);
		}

		private void versionSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			versionNumber = int.Parse(versionSelect.SelectedValue);
			loadPage(int.Parse(pageSelect.SelectedValue),versionNumber);

		}

		private void addPage_Click(object sender, EventArgs e)
		{
			int currentPage = int.Parse(pageSelect.SelectedValue);
			int newPageNumber = currentPage + 1;

			// bump up other pages
			insertPage(this.articleID,currentPage,languageSelect.SelectedValue);
			createArticlePage(newPageNumber);
			//fillPageSelect();
			loadPage(newPageNumber);
			//msg.Text = "<nobr>new page added</nobr>";
		}

		private void deletePage_Click(object sender, EventArgs e)
		{
			int currentPage = int.Parse(pageSelect.SelectedValue);
			//int newPageNumber = (currentPage > 2) ? currentPage - 1 : 1;
			string sql = String.Format("DELETE FROM " + dbTable + " WHERE articleID = {0} AND pageNumber = {1}",articleID,currentPage);
			this.dal.execNonQuery(sql);
			removePage(articleID,currentPage,languageSelect.SelectedValue);
			//fillPageSelect();
			//loadPage(1);
			Response.Redirect(Request.RawUrl);
		}

		private void addLanguage_Click(object sender, EventArgs e)
		{
			//string sql = String.Format("INSERT INTO " + dbTable + " (articleID, templateID, pageNumber, title, summary, body, createdBy, editedBy, publish, dateCreated, dateModified) VALUES ( {0},{1},'{2}',{3},'{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}')",articleID,1,newLanguage.Text.Trim(),1,"new article title","new summary","new body",((UserIdentity)user.Identity).ID,((UserIdentity)user.Identity).ID,1,1,System.DateTime.Now,System.DateTime.Now);
			//this.dal.execNonQuery(sql);
			//addArticlePage(1);
			createArticleMain();
			Response.Redirect(Request.RawUrl);
		}

		private void deleteLanguage_Click(object sender, EventArgs e)
		{
			string sql = String.Format("DELETE FROM " + dbTable + " WHERE articleID = {0}",this.articleID,languageSelect.SelectedValue);
			this.dal.execNonQuery(sql);
			Response.Redirect(Request.RawUrl);
		}

		private void savePage_Click(object sender, EventArgs e)
		{
			saveCurrentPage();
			loadPage();
			//msg.Text = "page saved";
		}
		private void previewPage_Click(object sender, EventArgs e)
		{
			createPreview();
			//saveCurrentPage();
			//Response.Redirect(currentNode.URL + "?lang=" + languageSelect.SelectedValue);
			Response.Redirect(currentNode.URL);
		}
		private void removeArchivePage_Click(object sender, EventArgs e)
		{
			removeVersion();
		}

		private void archivePage_Click(object sender, EventArgs e)
		{
			createNewVersion();
		}

		private void publishCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (publishCheck.Checked)
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 0 WHERE articleID = " + this.articleID + " AND pageNumber = " + pageNumber);
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 1 WHERE articleID = " + this.articleID + " AND pageNumber = " + pageNumber + " AND versionNumber = " + this.versionNumber);
			} 
			else 
			{
				dal.execNonQuery("UPDATE " + dbTable + " SET publish = 0 WHERE articleID = " + this.articleID + " AND pageNumber = " + pageNumber + " AND versionNumber = " + this.versionNumber);			
			}
			loadPage(pageNumber,versionNumber);
		}

		private void richEditCheck_CheckedChanged(object sender, EventArgs e)
		{
			content.RenderMode = (richEditCheck.Checked)? FreeTextBoxControls.RenderMode.Rich : FreeTextBoxControls.RenderMode.Plain;
		}



		#endregion

		#region load and save
		private void createArticleMain()
		{
			DataContainer dc = new DataContainer("sf_Articles");
			DataRow dr = dc.GetNewRow();
			dr["nodeID"] = currentNode.Id;
			dr["statusID"] = 1;
			dr["lang"] = newLanguage.Text;
			dr["createdBy"] = ((UserIdentity)user.Identity).ID;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dc.AddNewRow(dr);
			dc.Update();
			DataRow dr1 = dc.GetRowByCustomSql("SELECT id FROM sf_Articles WHERE lang = '" + newLanguage.Text + "' AND nodeID = " + currentNode.Id);
			this.articleID = (int)dr1["id"];
			createArticlePage(1);
		}

		private void createArticlePage(int pageNumber) 
		{
			DataContainer dc = new DataContainer(dbTable);
			DataRow dr = dc.GetNewRow();
			dr["articleID"] = articleID;
			dr["templateID"] = 1;
			dr["versionNumber"] = 1;
			dr["pageNumber"] = pageNumber;
			dr["title"] = "new article page title";
			dr["summary"] = "new summary";
			dr["keywords"] = "new keywords";
			dr["body"] = "new body";
			dr["createdBy"] = ((UserIdentity)user.Identity).ID;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["publish"] = false;
			dr["preview"] = false;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dc.AddNewRow(dr);
			dc.Update();
		}

		private void loadPage()
		{
			loadPage(pageNumber,versionNumber);
		}

		private void loadPage(int pageNumber)
		{
            loadPage(pageNumber,0);
		}

		private void loadPage(int pageNumber, int version)
		{
			fillPageSelect();
			fillVersionSelect(pageNumber);

			// get highest version number if not specified
			string sql = "SELECT * FROM " + dbTable + " WHERE articleID = " + this.articleID + "  AND preview = 0 AND pageNumber = " + pageNumber + " AND versionNumber =";
			if (version==0) 
				sql += "(SELECT MAX(versionNumber) FROM " + dbTable + " WHERE articleID = " + this.articleID + " AND pageNumber = " + pageNumber + ")";
			else 
				sql += versionNumber;

			DataSet ds = dal.execDataSet(sql);
			if (ds.Tables[0].Rows.Count > 0) 
			{
				DataRow dr = ds.Tables[0].Rows[0];
				title.Text = dr["title"].ToString();
				summary.Text = dr["summary"].ToString();
				keywords.Text = dr["keywords"].ToString();
				content.Text = dr["body"].ToString();
				pageSelect.SelectedValue = pageNumber.ToString();
				versionSelect.SelectedValue = dr["versionNumber"].ToString();
				articleTemplateID.SelectedValue = dr["templateID"].ToString();
				publishCheck.Checked = (dr["publish"] != null) ? (bool)dr["publish"] : false;
				showPageCount();
				bodyEditContainer.Visible = true;
				articleEditContainer.Visible = true;
				Page.PageTitle += dr["title"].ToString();
			} 
			else
			{
				articleEditContainer.Visible = true;
				articleEditHelp.Visible = true;
				newLanguage.Text = SFGlobal.DefaultLanguage;
			}
		}

		private void saveCurrentPage() 
		{
			/*
			this.articleID = int.Parse(languageSelect.SelectedValue);
			this.versionNumber = int.Parse(versionSelect.SelectedValue);
			*/
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetRowByCustomSql("SELECT * FROM " + dbTable + " WHERE articleID = " + articleID.ToString() + " AND pageNumber = " + pageSelect.SelectedValue + " AND versionNumber = " + versionNumber);
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = publishCheck.Checked;
			dc.UpdateRow(dr);
			this.removePreviewArticle();
		}
		private void createNewVersion() 
		{
			//DataRow dr = dc.g("SELECT * FROM " + dbTable + " WHERE articleID = " + articleID.ToString() + " AND pageNumber = " + pageSelect.SelectedValue + " AND versionNumber = " + versionNumber);

			/*
			this.articleID = int.Parse(languageSelect.SelectedValue);
			this.versionNumber = int.Parse(versionSelect.SelectedValue);
			int pageNumber = int.Parse(pageSelect.SelectedValue);
			*/
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetNewRow();
			dr["articleID"] = articleID;
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["versionNumber"] = versionCount() + 1;
			dr["pageNumber"] = pageNumber;
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["createdBy"] = ((UserIdentity)user.Identity).ID;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = false;
			dr["preview"] = false;
			dc.UpdateRow(dr);

			loadPage(pageNumber);
			/*
			fillVersionSelect(pageNumber);
			versionSelect.SelectedValue = versionCount();
			*/
		}

		private void removeVersion()
		{
			/*
			this.articleID = int.Parse(languageSelect.SelectedValue);
			this.versionNumber = int.Parse(versionSelect.SelectedValue);
			this.pageNumber = int.Parse(pageSelect.SelectedValue);
			*/
			
			this.dal.execNonQuery("DELETE FROM " + dbTable + " WHERE articleID = " + articleID + " AND pageNumber = " + pageNumber + " AND versionNumber = " + versionNumber);
			loadPage(pageNumber);            
		}
        
		private void createPreview()
		{
			/*
			this.articleID = int.Parse(languageSelect.SelectedValue);
			this.versionNumber = int.Parse(versionSelect.SelectedValue);
			int pageNumber = int.Parse(pageSelect.SelectedValue);
			*/

			// first delete any previews from before
            this.removePreviewArticle();

			// now put the new preview in
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetNewRow();
			dr["articleID"] = articleID;
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["versionNumber"] = 0;
			dr["pageNumber"] = pageNumber;
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["createdBy"] = ((UserIdentity)user.Identity).ID;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = false;
			dr["preview"] = true;
			dc.UpdateRow(dr);
			//Response.Write("Preview saved, but article loader not ready yet");
			//Response.End();
			//Response.Redirect(currentNode.URL);
		}

		private void insertPage(int articleID, int pageNumber, string lang)
		{
			string sql = String.Format("UPDATE " + dbTable + " SET pageNumber = pageNumber + 1 WHERE (pageNumber > {0}) AND articleID = {1}",pageNumber,articleID);
			this.dal.execNonQuery(sql);
		}

		private void removePage(int articleID, int pageNumber, string lang)
		{
			string sql = String.Format("UPDATE " + dbTable + " SET pageNumber = pageNumber - 1 WHERE (pageNumber > {0}) AND articleID = {1}",pageNumber,articleID);
			this.dal.execNonQuery(sql);
		}


		private void removePreviewArticle() 
		{
			string sql = String.Format("DELETE FROM " + dbTable + " WHERE preview = 1 AND articleID = {0}",articleID);
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
