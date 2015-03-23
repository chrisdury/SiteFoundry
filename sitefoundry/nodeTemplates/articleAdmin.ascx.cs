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
	using DuryTools.Data;

	using FreeTextBoxControls;


	/// <summary>
	///		Administrator tool for articles
	/// </summary>
	public class articleAdmin : DuryTools.UI.BaseControl
	{

		protected System.Web.UI.HtmlControls.HtmlTable articleMainTable;
		protected System.Web.UI.HtmlControls.HtmlTable addArticleTable;
		protected System.Web.UI.HtmlControls.HtmlTable articleLangTable;
		protected System.Web.UI.HtmlControls.HtmlTable articleInsertHelpTable;

		protected System.Web.UI.WebControls.TextBox articleName;
		protected System.Web.UI.WebControls.TextBox title;
		protected System.Web.UI.WebControls.CheckBox publishCheck;
		protected System.Web.UI.WebControls.DropDownList articleStatusID;
		protected System.Web.UI.WebControls.DropDownList articleTemplateID;
		protected System.Web.UI.WebControls.TextBox summary;
		protected System.Web.UI.WebControls.TextBox keywords;
		//protected FreeTextBoxControls.FreeTextBox content;
		protected Dury.SiteFoundry.UI.CustomFreeTextBox content;

		protected System.Web.UI.WebControls.DropDownList languageSelect;
		protected System.Web.UI.WebControls.Button addLang;
		protected System.Web.UI.WebControls.TextBox newLang;
		protected System.Web.UI.WebControls.Button deleteLang;

		protected System.Web.UI.WebControls.DropDownList pageSelect;
		protected System.Web.UI.WebControls.Literal pageCount;
		protected System.Web.UI.WebControls.Button addPage;
		protected System.Web.UI.WebControls.Button savePage;
		protected System.Web.UI.WebControls.Button previewPage;
		protected System.Web.UI.WebControls.Button deletePage;
		protected System.Web.UI.WebControls.Button addArticle;
		protected System.Web.UI.WebControls.Literal msg;

		private UserPrincipal user;
		private Node currentNode;
		private int articleID;
		private DAL dal = SFGlobal.DAL;
		private string dbTable = "ArticleBodies";

		protected LinkButton expandButton;
		private Unit smallContentWidth = Unit.Pixel(700);
		private Unit smallContentHeight = Unit.Pixel(330);
		private Unit largeContentWidth = Unit.Pixel(950);
		private Unit largeContentHeight = Unit.Pixel(600);
		private bool useLargeContentView = false;

		private void Page_Load(object sender, System.EventArgs e)
		{
			init();
			applySecurity();
			if (!IsPostBack) this.oneTimeInit();
			if (!IsPostBack) loadPage(1);			
		}

		private void applySecurity()
		{
			if (user.Roles.Contains(SFGlobal.AdminstratorRoleName) || user.Roles.Contains(SFGlobal.PublisherRoleName))
			{
				articleStatusID.Enabled = true;
				deleteLang.Enabled = true;
				deletePage.Enabled = true;
			}
		}

		private void init()
		{

			//add event handlers
			pageSelect.SelectedIndexChanged += new EventHandler(pageSelect_SelectedIndexChanged);
			languageSelect.SelectedIndexChanged += new EventHandler(languageSelect_SelectedIndexChanged);
			addPage.Click += new EventHandler(addPage_Click);
			addLang.Click += new EventHandler(addLang_Click);
			deleteLang.Click += new EventHandler(deleteLang_Click);
			savePage.Click += new EventHandler(savePage_Click);
			previewPage.Click += new EventHandler(previewPage_Click);
			deletePage.Click += new EventHandler(deletePage_Click);
			addArticle.Click += new EventHandler(addArticle_Click);
			expandButton.Click +=new EventHandler(expandButton_Click);


			// get node info
			Node root = (Node)Application["nodeRoot"];
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = root.Find(nodeID);
			if (currentNode == null) throw new DuryTools.ErrorHandler("node not found, id=" + nodeID.ToString());

			//get environment
            checkForArticle();

			// other config...
			content.FormatHtmlTagsToXhtml = true;
			pageSelect.AutoPostBack = true;
			languageSelect.AutoPostBack = true;
			articleStatusID.Enabled = false;
			deleteLang.Enabled = false;
			deleteLang.Attributes.Add("onClick","return deleteWarning('all pages for this language');");
			deletePage.Enabled = false;
			deletePage.Attributes.Add("onClick","return deleteWarning('this article page');");
			/*
			content.GutterBackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd");
			content.BackColor = System.Drawing.ColorTranslator.FromHtml("#eeeeee");
			content.SupportFolder = "~/admin/freetextbox/";
			content.ButtonFolder = "custom";
			content.DesignModeCss = "../includes/main.css";
			*/

			user = (UserPrincipal)Context.User;
			//Page.PageTitle += "Article";

			//get user's content size preference
			if (Session["useLargeContentView"] != null)
				this.useLargeContentView = (bool)Session["useLargeContentView"];
			else
				Session["useLargeContentView"] = false;


			applyEnterKeyTrap();

		}

		private void oneTimeInit() 
		{
			fillLangSelect();
			fillPageSelect();
			fillTemplateSelect();
			fillStatusSelect();
			/*

			FreeTextBoxControls.Toolbar tb = new Toolbar();

			ToolbarButton addImageButton = new ToolbarButton("Insert an Image","FTB_OpenImageWindow","insertimage");
			addImageButton.ScriptBlock = "function FTB_OpenImageWindow(ftbName) { openWin('helpers/gallery.aspx',400,400,ftbName); }" + System.Environment.NewLine;
			addImageButton.ScriptBlock += @"function FTB_AddImage(ftbName,src,h,w){ FTB_InsertText(ftbName,'<img src=""' + src + '"" height=""' + h + '"" border=""0"" />');}";

			ToolbarButton addDocumentButton = new ToolbarButton("Insert a Document","FTB_OpenDocumentWindow","insertdocument");
			addDocumentButton.ScriptBlock = "function FTB_OpenDocumentWindow(ftbName) { openWin('helpers/documents.aspx',400,400,ftbName); }" + System.Environment.NewLine;
			addDocumentButton.ScriptBlock += @"function FTB_AddDocument(ftbName,text){ FTB_InsertText(ftbName,text);}";

			ToolbarButton addLinkButton = new ToolbarButton("Insert an Internal Link","FTB_OpenLinkWindow","insertlink");
			addLinkButton.ScriptBlock = "function FTB_OpenLinkWindow(ftbName) { openWin('helpers/links.aspx',400,400,ftbName); }" + System.Environment.NewLine;
			//addLinkButton.ScriptBlock += @"function FTB_AddLink(ftbName,text){ FTB_InsertText(ftbName,text);}";
			addLinkButton.ScriptBlock += @"function FTB_AddLink(ftbName,start,end){ FTB_SurroundText(ftbName,start,end);}";

			content.Toolbars[content.Toolbars.Count-1].Items.Add(addImageButton);
			content.Toolbars[content.Toolbars.Count-1].Items.Add(addDocumentButton);
			content.Toolbars[content.Toolbars.Count-1].Items.Add(addLinkButton);
			*/
			content.Height = (this.useLargeContentView) ? this.largeContentHeight : this.smallContentHeight;
			content.Width = (this.useLargeContentView) ? this.largeContentWidth : this.smallContentWidth;
			if (this.useLargeContentView) 
				expandButton.Text = "small view";


		}
		private void fillLangSelect() 
		{
			DataSet ds = dal.execDataSet("SELECT DISTINCT(lang) FROM ArticleBodies WHERE articleID = " + this.articleID.ToString());
			languageSelect.DataSource = ds;
			languageSelect.DataTextField = "lang";
			languageSelect.DataValueField = "lang";
			languageSelect.DataBind();
		}
		private void fillPageSelect() 
		{
			string sql = String.Format("SELECT pageNumber FROM ArticleBodies WHERE articleID = {0} AND lang = '{1}' ORDER BY pageNumber ASC",this.articleID,languageSelect.SelectedValue);
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
		private void fillStatusSelect() 
		{
			DataSet ds = dal.execDataSet("SELECT id,name FROM ArticleStatus");
			articleStatusID.DataSource = ds;
			articleStatusID.DataValueField = "id";
			articleStatusID.DataTextField = "name";
			articleStatusID.DataBind();
		}


		private void checkForArticle() 
		{
            object i = dal.execScalar("SELECT id FROM ArticleContainers WHERE nodeID = " + currentNode.Id);
            if (i==null) showAddArticle();
			else articleID = (int)i;
		}
		private void showAddArticle()
		{
			articleMainTable.Visible = false;
			articleLangTable.Visible = false;
			addArticleTable.Visible = true;	
		}

		private void showPageCount() 
		{
			string sql = String.Format("SELECT COUNT(id) FROM ArticleBodies WHERE articleID = {0} AND lang = '{1}'",articleID,languageSelect.SelectedValue);
			int p = (int)dal.execScalar(sql);
			pageCount.Text = p.ToString();

		}

		private void loadPage(int pageNumber)
		{
			DataSet ds = dal.execDataSet("SELECT * FROM ArticleBodies WHERE articleID = " + this.articleID + " AND lang = '" + languageSelect.SelectedValue + "' AND pageNumber = " + pageNumber);
			if (ds.Tables[0].Rows.Count > 0) 
			{
				DataRow dr = ds.Tables[0].Rows[0];
				title.Text = dr["title"].ToString();
				summary.Text = dr["summary"].ToString();
				keywords.Text = dr["keywords"].ToString();
				content.Text = dr["body"].ToString();
				pageSelect.SelectedValue = pageNumber.ToString();
				articleTemplateID.SelectedValue = dr["templateID"].ToString();
				articleStatusID.SelectedValue = dr["statusID"].ToString();
				publishCheck.Checked = (dr["publish"] != null) ? (bool)dr["publish"] : false;
				showPageCount();
				expandButton.Visible = true;
			} 
			else
			{
				if (articleLangTable.Visible) 
				{
					articleInsertHelpTable.Visible = true;
                    newLang.Text = SFGlobal.DefaultLanguage;		
				}
				articleMainTable.Visible = false;
			}


		}

		private void insertPage(int articleID, int pageNumber, string lang)
		{
			string sql = String.Format("UPDATE ArticleBodies SET pageNumber = pageNumber + 1 WHERE (pageNumber > {0}) AND articleID = {1} AND lang = '{2}'",pageNumber,articleID,lang);
			this.dal.execNonQuery(sql);
		}

		private void removePage(int articleID, int pageNumber, string lang)
		{
			string sql = String.Format("UPDATE ArticleBodies SET pageNumber = pageNumber - 1 WHERE (pageNumber > {0}) AND articleID = {1} AND lang = '{2}'",pageNumber,articleID,lang);
			this.dal.execNonQuery(sql);
		}


		private void saveCurrentPage() 
		{
			DataContainer dc = new DataContainer(this.dbTable);
			DataRow dr = dc.GetRowByCustomSql("SELECT * FROM ArticleBodies WHERE articleID = " + articleID.ToString() + " AND pageNumber = " + pageSelect.SelectedValue + " AND lang = '" + languageSelect.SelectedValue + "'");
			dr["templateID"] = int.Parse(articleTemplateID.SelectedValue);
			dr["title"] = title.Text;
			dr["summary"] = summary.Text;
			dr["keywords"] = keywords.Text;
			dr["body"] = content.Text;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["statusID"] = 1; //articleStatusID.SelectedValue;
			dr["dateModified"] = System.DateTime.Now;
			dr["publish"] = publishCheck.Checked;
			dc.UpdateRow(dr);
		}

		private void applyEnterKeyTrap()
		{
			this.title.Attributes.Add("onKeyDown","trapEnter();");
			this.summary.Attributes.Add("onKeyDown","trapEnter();");
			this.keywords.Attributes.Add("onKeyDown","trapEnter();");
		}		

		#region event handlers
		private void pageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedPage = int.Parse(pageSelect.SelectedValue);
			loadPage(selectedPage);
		}

		private void languageSelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			fillPageSelect(); // can this be moved into loadPage? might interfere with onSelectIndexChanged event detection...
			loadPage(1);
		}

		private void addPage_Click(object sender, EventArgs e)
		{
			int currentPage = int.Parse(pageSelect.SelectedValue);
			int newPageNumber = currentPage + 1;

			// bump up other pages
			insertPage(this.articleID,currentPage,languageSelect.SelectedValue);

			DataContainer dc = new DataContainer(dbTable);
			DataRow dr = dc.GetNewRow();
			dr["articleID"] = articleID;
			dr["templateID"] = 1;
			dr["lang"] = languageSelect.SelectedValue;
			dr["pageNumber"] = newPageNumber;
			dr["title"] = "new article title";
			dr["summary"] = "new summary";
			dr["body"] = "new body";
			dr["createdBy"] = ((UserIdentity)user.Identity).ID;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["statusID"] = 1;
			dr["publish"] = false;
			dr["dateCreated"] = System.DateTime.Now;
			dr["dateModified"] = System.DateTime.Now;
			dc.AddNewRow(dr);
			dc.Update();

			//string sql = String.Format("INSERT INTO ArticleBodies (articleID, templateID, lang, pageNumber, title, summary, body, createdBy, editedBy, statusID, publish, dateCreated, dateModified) VALUES ( {0}, {1},'{2}',{3},'{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}')",
			//this.dal.execNonQuery(sql);

			fillPageSelect();
			loadPage(newPageNumber);
			msg.Text = "<nobr>new page added</nobr>";
		}

		private void deletePage_Click(object sender, EventArgs e)
		{
			int currentPage = int.Parse(pageSelect.SelectedValue);
			//int newPageNumber = (currentPage > 2) ? currentPage - 1 : 1;
			string sql = String.Format("DELETE FROM ArticleBodies WHERE articleID = {0} AND pageNumber = {1} AND lang = '{2}'",articleID,currentPage,languageSelect.SelectedValue);
			this.dal.execNonQuery(sql);
			removePage(articleID,currentPage,languageSelect.SelectedValue);
			//fillPageSelect();
			//loadPage(1);
			Response.Redirect(Request.RawUrl);
		}

		private void addLang_Click(object sender, EventArgs e)
		{
			//string sql = String.Format("INSERT INTO ArticleBodies (articleID, templateID, lang, pageNumber, title, summary, body, createdBy, editedBy, statusID, publish, dateCreated, dateModified) VALUES ( {0},{1},'{2}',{3},'{4}','{5}','{6}',{7},{8},{9},{10},'{11}','{12}')",articleID,1,newLang.Text.Trim(),1,"new article title","new summary","new body",((UserIdentity)user.Identity).ID,((UserIdentity)user.Identity).ID,1,1,System.DateTime.Now,System.DateTime.Now);
            //this.dal.execNonQuery(sql);
			DataContainer dc = new DataContainer("ArticleBodies");
			DataRow dr = dc.GetNewRow();
			dr["articleID"] = articleID;
			dr["templateID"] = 1;
			dr["lang"] = newLang.Text.Trim();
			dr["pageNumber"] = 1;
			dr["title"] = "new article title";
			dr["summary"] = "";
			dr["keywords"] = "";
			dr["body"] = "new body";
			dr["createdBy"] = dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dr["statusID"] = 1;
			dr["publish"] = false;
			dr["dateCreated"] = dr["dateModified"] = System.DateTime.Now;
            dc.UpdateRow(dr);
			Response.Redirect(Request.RawUrl);
		}

		private void deleteLang_Click(object sender, EventArgs e)
		{
			string sql = String.Format("DELETE FROM ArticleBodies WHERE articleID = {0} AND lang = '{1}'",this.articleID,languageSelect.SelectedValue);
			this.dal.execNonQuery(sql);
			Response.Redirect(Request.RawUrl);
		}

		private void savePage_Click(object sender, EventArgs e)
		{
			saveCurrentPage();
			msg.Text = "page saved";

		}

		private void previewPage_Click(object sender, EventArgs e)
		{
			saveCurrentPage();
			Response.Redirect(currentNode.URL + "?lang=" + languageSelect.SelectedValue);
		}

		private void addArticle_Click(object sender, EventArgs e)
		{
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			//string sql = String.Format("INSERT INTO ArticleContainers (nodeID, typeID, name, publish, dateCreated, dateModified, dateExpires) values ( {0}, {1}, '{2}', {3}, '{4}', '{5}', '{6}' )",nodeID,0,"Unnamed Article",0,System.DateTime.Now,System.DateTime.Now,System.DateTime.Now);
			//dal.execNonQuery(sql);
			DataContainer dc = new DataContainer("ArticleContainers");
			DataRow dr = dc.GetNewRow();
            dr["nodeID"] = nodeID;
			dr["typeID"] = 0;
			dr["name"] = "Unnamed Article";
			dr["publish"] = 0;
			dr["dateExpires"] = dr["dateCreated"] = dr["dateModified"] = System.DateTime.Now;
			dc.UpdateRow(dr);
			Response.Redirect(Request.Path + "?nodeID=" + nodeID.ToString());
		}


		private void expandButton_Click(object sender, EventArgs e)
		{
			/*
			if (content.Height.Value == this.defaultContentHeight)
			{
				content.Width = Unit.Pixel(900);
				content.Height = Unit.Pixel(600);
				expandButton.Text = "small view";
			} 
			else
			{
				content.Width = Unit.Pixel(this.defaultContentWidth);
				content.Height = Unit.Pixel(this.defaultContentHeight);
				expandButton.Text = "large view";
			}
			*/

			if (Session["useLargeContentView"] == null || (bool)Session["useLargeContentView"] == true)
			{
				Session["useLargeContentView"] = false;
			} 
			else
			{
				Session["useLargeContentView"] = true;
			}
			Response.Redirect(Request.RawUrl);

		}


		#endregion

		#region Web Form Designer generated code & custom events
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
