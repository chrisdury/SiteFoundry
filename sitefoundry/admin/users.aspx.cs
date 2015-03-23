using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;
using Wilson.ORMapper;

namespace Dury.SiteFoundry.Admin
{
	/// <summary>
	/// Summary description for users.
	/// </summary>
	public class users : DuryTools.UI.BasePage
	{

		protected System.Web.UI.WebControls.DataGrid rolesGrid;
		protected System.Web.UI.WebControls.DataGrid userGrid;
		
		protected System.Web.UI.HtmlControls.HtmlTable newRoleTable;
		protected System.Web.UI.WebControls.TextBox newRoleName;
		protected System.Web.UI.WebControls.Button addNewRole;

		protected System.Web.UI.HtmlControls.HtmlTable newUserTable;
		protected System.Web.UI.WebControls.Button addNewUserButton;
		protected System.Web.UI.WebControls.TextBox newUserName;
		protected System.Web.UI.WebControls.TextBox newUserPassword;
		protected System.Web.UI.WebControls.TextBox newUserFullName;
		protected System.Web.UI.WebControls.TextBox newUserEmail;
		protected System.Web.UI.WebControls.ListBox newUserRoles;

		protected System.Web.UI.WebControls.Literal msg;

		protected UserPrincipal user;


		private void Page_Load(object sender, System.EventArgs e)
		{
			this.PageTitle += "Users";
			unhideNewTables();
			user = (UserPrincipal)Context.User;
			rolesGrid.ItemDataBound += new DataGridItemEventHandler(rolesGrid_ItemDataBound);
			msg.Text = "";



			if (!IsPostBack) 
			{
				if (user.IsUserAdmin()) 
				{
					rolesBind();
					userGridBind();
					fillNewUserRoles();
				}
				else
				{
					hideNewTables();				
					msg.Text = "You don't have access to modify users or roles";
				}
			}

			
		}


		#region rolesgrid
		protected void rolesBind()
		{
            ObjectSet os = SFGlobal.ObjectManager.GetObjectSet(new ObjectQuery(typeof(SecurityRole),"",""));
			rolesGrid.DataSource = os;
			//rolesGrid.DataSource = SFGlobal.DAL.execDataSet("SELECT * FROM SecurityRoles");
			rolesGrid.DataBind();
		}

		private void rolesGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer && e.Item.ItemType != ListItemType.Pager && e.Item.ItemType != ListItemType.Separator) 
			{
				SecurityRole sr = (SecurityRole)e.Item.DataItem;
				if (e.Item.FindControl("publishViewHolder") != null) 
				{
					PlaceHolder p = (PlaceHolder)e.Item.FindControl("publishViewHolder");
					if (sr.Publish) p.Controls.Add(new LiteralControl("y"));
					else p.Controls.Add(new LiteralControl("n"));
				}
				if (e.Item.FindControl("publishCheck") != null) 
				{
					CheckBox c = (CheckBox)e.Item.FindControl("publishCheck");
					c.Checked = sr.Publish;
				}				
			}
		}

		protected void rolesGrid_Edit( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			rolesGrid.EditItemIndex = e.Item.ItemIndex;
			rolesBind();
			hideNewTables();
		}
	
		protected void rolesGrid_Update( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);
			SecurityRole sr = (SecurityRole)SFGlobal.ObjectManager.GetObject(typeof(SecurityRole),itemID);
			sr.Name = ((TextBox)e.Item.FindControl("name")).Text;
			sr.Publish = ((CheckBox)e.Item.FindControl("publishCheck")).Checked;
			SFGlobal.ObjectManager.PersistChanges(sr);
			SFGlobal.ObjectManager.Resync(sr);
            //SFGlobal.DAL.execNonQuery("UPDATE SecurityRoles SET name = '" + SFGlobal.SqlCleanString(s) + "' WHERE id = " + itemID.ToString());
			rolesGrid.EditItemIndex = -1;
			rolesBind();
		}

		protected void rolesGrid_Delete( System.Object sender, DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityUserRoles WHERE roleID = " + itemID.ToString());
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityNodes WHERE roleID = " + itemID.ToString());
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityRoles WHERE id = " + itemID.ToString());
			//rolesBind();
			Response.Redirect(Request.RawUrl);
		}

		protected void rolesGrid_Cancel(Object sender, DataGridCommandEventArgs e) 
		{
			rolesGrid.EditItemIndex = -1;
			rolesBind();
		}

		protected void rolesGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.FindControl("deleteButton") != null)
			{
				((LinkButton) e.Item.FindControl("deleteButton")).Attributes.Add("onClick", "return deleteWarning('this Security Role?\\nAll Security Entries involving this role will also be deleted!');");
			}
		}

		#endregion

		#region usergrid
		
		protected void userGridBind()
		{
			userGrid.DataSource = SFGlobal.DAL.execDataSet("SELECT * FROM SecurityUsers");
			userGrid.DataBind();
		}



		protected void userGrid_Edit( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			userGrid.EditItemIndex = e.Item.ItemIndex;
			userGridBind();
			hideNewTables();
		}
	
		protected void userGrid_Update( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);

			// insert user data
			string username = SFGlobal.SqlCleanString(((TextBox)e.Item.FindControl("username")).Text);
			string password = ((TextBox)e.Item.FindControl("password")).Text;
			string fullname = SFGlobal.SqlCleanString(((TextBox)e.Item.FindControl("fullname")).Text);
			string email = SFGlobal.SqlCleanString(((TextBox)e.Item.FindControl("email")).Text);
			string disabled = ((CheckBox)e.Item.FindControl("disabledCheck")).Checked ? "1" : "0";
			if (password != null && password != "")
			{
				password = Dury.SiteFoundry.Security.Cryptography.AsymmetricEncryption.ComputeHash(password,SFGlobal.EncryptionMethod,SFGlobal.EncryptionSalt);
				SFGlobal.DAL.execNonQuery("UPDATE SecurityUsers SET username = '" + username + "'" + ", password='" + password + "', fullname='" + fullname + "', email='" + email + "' , disabled=" + disabled + " WHERE id = " + itemID.ToString());
			} 
			else 
			{
				SFGlobal.DAL.execNonQuery("UPDATE SecurityUsers SET username = '" + username + "', fullname='" + fullname + "', email='" + email + "', disabled=" + disabled + " WHERE id = " + itemID.ToString());
			}


			// insert roles
			ListBox cbx = (ListBox)e.Item.FindControl("rolesList");
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityUserRoles WHERE userID = " + itemID.ToString());
			foreach(ListItem li in cbx.Items)
			{
				if (li.Selected)
				{
					SFGlobal.DAL.execNonQuery("INSERT INTO SecurityUserRoles (userID,roleID) VALUES (" + itemID.ToString() + "," + li.Value + ")");
				}
			}
			userGrid.EditItemIndex = -1;
			userGridBind();
			SFGlobal.UpdateNodes();
			msg.Text = "User: " + username + " updated ok";

		}

		protected void userGrid_Delete( System.Object sender, DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);
			SFGlobal.DAL.execNonQuery("ReAssignArticleOwnership",itemID,SFGlobal.CurrentUser.ID);
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityUserRoles WHERE userID = " + itemID.ToString());
			SFGlobal.DAL.execNonQuery("DELETE FROM SecurityUsers WHERE id = " + itemID.ToString());
			userGridBind();
		}

		protected void userGrid_Cancel(Object sender, DataGridCommandEventArgs e) 
		{
			userGrid.EditItemIndex = -1;
			userGridBind();
		}

		protected void userGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
		{
			string userRoles = "SELECT sur.userID, r.name FROM SecurityUserRoles sur INNER JOIN SecurityRoles r ON sur.roleID = r.id WHERE (sur.userID = " + e.Item.Cells[0].Text + ")";
			if (e.Item.FindControl("rolesDisplay") != null)
			{
				DropDownList ddl = (DropDownList)e.Item.FindControl("rolesDisplay");
				DataSet ds = SFGlobal.DAL.execDataSet(userRoles);
				foreach(DataRow dr in ds.Tables[0].Rows)
				{
					ddl.Items.Add(new ListItem(dr["name"].ToString()));
				}
				ddl.DataBind();
			}


			if (e.Item.FindControl("rolesList") != null)
			{
				ListBox cbx = (ListBox)e.Item.FindControl("rolesList");
				DataSet userDS = SFGlobal.DAL.execDataSet(userRoles);
				DataSet rolesDS = SFGlobal.DAL.execDataSet("SELECT * FROM SecurityRoles");
                
				foreach(DataRow dr in rolesDS.Tables[0].Rows)
				{
					cbx.Items.Add(new ListItem(dr["name"].ToString(),dr["id"].ToString()));
				}
				foreach(DataRow dr in userDS.Tables[0].Rows)
				{
					if (cbx.Items.FindByText(dr["name"].ToString()) != null)
					{
						cbx.Items.FindByText(dr["name"].ToString()).Selected = true;
					}
				}
			}

			if (e.Item.FindControl("disabledText") != null)
			{
				Literal l = (Literal)e.Item.FindControl("disabledText");
				SecurityUser su = (SecurityUser)SFGlobal.ObjectManager.GetObject(typeof(SecurityUser),int.Parse(e.Item.Cells[0].Text));
				if (su.Disabled) l.Text = "yes";
				else l.Text = "no";
			}


			if (e.Item.FindControl("disabledCheck") != null)
			{
				CheckBox cb = (CheckBox)e.Item.FindControl("disabledCheck");
				SecurityUser su = (SecurityUser)SFGlobal.ObjectManager.GetObject(typeof(SecurityUser),int.Parse(e.Item.Cells[0].Text));
				cb.Checked = (su.Disabled) ? true : false;
			}


			if (e.Item.FindControl("deleteButton") != null)
			{
				((LinkButton) e.Item.FindControl("deleteButton")).Attributes.Add("onClick", "return deleteWarning('delete this user?\\nAll Security entries regarding this user will also be deleted and all ownership will be re-assigned to your login.');");
			}
		}




		#endregion

		#region utilites

		public string getHowLongAgo(DateTime dt)
		{
			TimeSpan ts = System.DateTime.Now.Subtract(dt);
            return String.Format("{0} min. ago",ts.TotalMinutes.ToString("N0"));
		}


		public void fillNewUserRoles()
		{
			DataSet ds = SFGlobal.DAL.execDataSet("SELECT * FROM SecurityRoles");
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
                newUserRoles.Items.Add(new ListItem(dr["name"].ToString(),dr["id"].ToString()));
			}
			//newUserRoles.DataBind();
		}

		private void hideNewTables()
		{
			newUserTable.Visible = false;
			newRoleTable.Visible = false;
		}

		private void unhideNewTables()
		{
			newUserTable.Visible = true;
			newRoleTable.Visible = true;;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
			addNewRole.Click += new EventHandler(addNewRole_Click);
			addNewUserButton.Click += new EventHandler(addNewUserButton_Click);
		}
		#endregion

		private void addNewRole_Click(object sender, EventArgs e)
		{
			SFGlobal.DAL.execNonQuery("INSERT INTO SecurityRoles (name) VALUES ('" + SFGlobal.SqlCleanString(newRoleName.Text) + "')");
			Response.Redirect(Request.RawUrl);
		}

		private void addNewUserButton_Click(object sender, EventArgs e)
		{
			string username = SFGlobal.SqlCleanString(newUserName.Text);
			string password = Dury.SiteFoundry.Security.Cryptography.AsymmetricEncryption.ComputeHash(newUserPassword.Text,SFGlobal.EncryptionMethod,SFGlobal.EncryptionSalt);
			string fullname = SFGlobal.SqlCleanString(newUserFullName.Text);
			string email = SFGlobal.SqlCleanString(newUserEmail.Text);
			string sql = String.Format("INSERT INTO SecurityUsers (username,password,disabled,fullname,email,lastlogin,datecreated,datemodified) VALUES ('{0}','{1}',0, '{2}','{3}','{4}','{5}','{6}')",username,password,fullname,email,System.DateTime.Now,System.DateTime.Now,System.DateTime.Now);
            SFGlobal.DAL.execNonQuery(sql);

			int userID = (int)SFGlobal.DAL.execScalar("SELECT id FROM SecurityUsers WHERE username = '" + username + "'");

			foreach(ListItem li in newUserRoles.Items)
			{
				if (li.Selected)
				{
					SFGlobal.DAL.execNonQuery("INSERT INTO SecurityUserRoles (userID,roleID) VALUES (" + userID.ToString() + "," + li.Value + ")");
				}
				li.Selected = false;
			}
			newUserName.Text = "";
			newUserPassword.Text = "";
			newUserFullName.Text = "";
            newUserEmail.Text = "";

			userGridBind();
			msg.Text = "user added";
		}

		
	}
}
