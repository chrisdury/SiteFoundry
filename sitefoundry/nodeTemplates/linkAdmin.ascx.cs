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

	/// <summary>
	///		Summary description for linkAdmin.
	/// </summary>
	public class linkAdmin : DuryTools.UI.BaseControl
	{
		protected System.Web.UI.WebControls.TextBox url;
		protected System.Web.UI.WebControls.Button save;
		protected System.Web.UI.WebControls.CheckBox publish;

		private DuryTools.Data.DAL dal = new DuryTools.Data.DAL();
		private Node currentNode;
		private UserPrincipal user;

		private void Page_Load(object sender, System.EventArgs e)
		{
			init();
			loadPage();
		}


		private void init() 
		{
			//Page.PageTitle += "Link";
			Node root = (Node)Application["nodeRoot"];
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = root.Find(nodeID);
			if (currentNode == null) throw new Exception("node not found, id=" + nodeID.ToString());
			user = (UserPrincipal)Context.User;
		}

		private void loadPage() 
		{
			DataSet ds = dal.execDataSet("SELECT * FROM Links WHERE nodeID = " + currentNode.Id);
			if (ds.Tables[0].Rows.Count == 0)
			{
				//string sql = String.Format("INSERT INTO Links (nodeID,linkURL,publish,dateCreated,dateModified,createdBy,editedBy) VALUES ({0},'{1}',{2},'{3}','{4}',{5},{6})",currentNode.Id,"http://www.somewhere.com",1,System.DateTime.Now,System.DateTime.Now,((UserIdentity)user.Identity).ID,((UserIdentity)user.Identity).ID);
				//dal.execNonQuery(sql);

				DataContainer dc = new DataContainer("Links");
				DataRow dr = dc.GetNewRow();
				dr["nodeID"] = currentNode.Id;
				dr["linkURL"] = "http://www.somewhere.com";
				dr["publish"] = true;
				dr["dateCreated"] = System.DateTime.Now;
				dr["dateModified"] = System.DateTime.Now;
				dr["createdBy"] = ((UserIdentity)user.Identity).ID;
				dr["editedBy"] = ((UserIdentity)user.Identity).ID;
				dc.AddNewRow(dr);
				dc.Update();

				Response.Redirect(Request.RawUrl);
			} 
			else 
			{
				DataRow dr = ds.Tables[0].Rows[0];
				url.Text = dr["linkURL"].ToString();
				publish.Checked = (dr["publish"] != null) ? (bool)dr["publish"] : false;
			}

		}


		private void savePage() 
		{
			/*
			string sql = "UPDATE Links SET ";
			sql += "linkURL = '" + url.Text + "', ";
			sql += "publish = " + Convert.ToInt16(publish.Checked).ToString() + ", ";
			sql += "dateModified = '" + System.DateTime.Now + "', ";
			sql += "editedBy = " + ((UserIdentity)user.Identity).ID.ToString();
			sql += " WHERE nodeID = " + currentNode.Id;
			dal.execNonQuery(sql);
			*/

			DataContainer dc = new DataContainer("Links");
			DataRow dr = dc.GetRowByKey("nodeID",currentNode.Id);
			dr["linkURL"] = url.Text;
			dr["publish"] = publish.Checked;
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
			save.Click += new EventHandler(save_Click);
		}
		#endregion

		private void save_Click(object sender, EventArgs e)
		{
			savePage();
		}
	}
}
