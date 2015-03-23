namespace Dury.SiteFoundry.nodeTemplates
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using DuryTools.Data;
	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Security;

	/// <summary>
	///		Summary description for rawHtmlAdmin.
	/// </summary>
	public class rawHtmlAdmin : DuryTools.UI.BaseControl
	{

		protected System.Web.UI.WebControls.TextBox title;
		protected System.Web.UI.WebControls.CheckBox templateCheck;
		protected System.Web.UI.WebControls.CheckBox publishCheck;
		protected System.Web.UI.WebControls.TextBox content;

		protected System.Web.UI.WebControls.Button saveButton;
		protected System.Web.UI.WebControls.Button previewButton;

		private DAL dal = SFGlobal.DAL;
		private Node currentNode;
		private UserPrincipal user;

		private void Page_Load(object sender, System.EventArgs e)
		{
			init();
			loadPage();
		}


		private void init() 
		{
			//Page.PageTitle += "Raw HTML";
			Node root = (Node)Application["nodeRoot"];
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = root.Find(nodeID);
			if (currentNode == null) throw new Exception("node not found, id=" + nodeID.ToString());
			user = (UserPrincipal)Context.User;
		}

		private void loadPage() 
		{
            DataSet ds = dal.execDataSet("SELECT * FROM SimpleArticles WHERE nodeID = " + currentNode.Id);
			if (ds.Tables[0].Rows.Count == 0)
			{
				//string sql = String.Format("INSERT INTO SimpleArticles (nodeID,title,body,dateCreated,dateModified,createdBy) VALUES ('{0}','{1}','{2}','{3}','{4}',{5})",currentNode.Id,"new page","new body",System.DateTime.Now,System.DateTime.Now,((UserIdentity)user.Identity).ID);
				//dal.execNonQuery(sql);

				DataContainer dc = new DataContainer("SimpleArticles");
				DataRow dr = dc.GetNewRow();
				dr["nodeID"] = currentNode.Id;
				dr["title"] = "new page";
				dr["body"] = "new body";
				dr["dateCreated"] = System.DateTime.Now;
				dr["dateModified"] = System.DateTime.Now;
				dr["createdBy"] = ((UserIdentity)user.Identity).ID;
				dc.AddNewRow(dr);
				dc.Update();

				Response.Redirect(Request.RawUrl);
			} 
			else 
			{
				DataRow dr = ds.Tables[0].Rows[0];
                title.Text = dr["title"].ToString();
				content.Text = dr["body"].ToString();
				templateCheck.Checked = (dr["showTemplate"] != System.DBNull.Value) ? (bool)dr["showTemplate"] : false;
				publishCheck.Checked = (dr["publish"] != System.DBNull.Value) ? (bool)dr["publish"] : false;
			}

		}


		private void savePage() 
		{
			/*
			string sql = "UPDATE SimpleArticles SET ";
			sql += "showTemplate = " + Convert.ToInt32(templateCheck.Checked) + ", ";
			sql += "title = '" + title.Text + "', ";
			sql += "body = '" + content.Text + "', ";
			sql += "publish = " + Convert.ToInt32(publishCheck.Checked) + ", ";
			sql += "dateModified = '" + System.DateTime.Now + "', ";
			sql += "editedBy = " + ((UserIdentity)user.Identity).ID.ToString();
			sql += " WHERE nodeID = " + currentNode.Id;
            dal.execNonQuery(sql);
			*/

			DataContainer dc = new DataContainer("SimpleArticles");
			DataRow dr = dc.GetRowByKey("nodeID",currentNode.Id);
			dr["nodeID"] = currentNode.Id;
			dr["showTemplate"] = templateCheck.Checked;
			dr["title"] = title.Text;
			dr["body"] = content.Text;
			dr["publish"] = publishCheck.Checked;
			dr["dateModified"] = System.DateTime.Now;
			dr["editedBy"] = ((UserIdentity)user.Identity).ID;
			dc.UpdateRow(dr);
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
			saveButton.Click +=new EventHandler(saveButton_Click);
			previewButton.Click +=new EventHandler(previewButton_Click);
		}
		#endregion

		#region event handlers

		private void saveButton_Click(object sender, EventArgs e)
		{
			savePage();
		}

		private void previewButton_Click(object sender, EventArgs e)
		{
			savePage();
			Response.Redirect(currentNode.URL);
		}

		#endregion
	}
}
