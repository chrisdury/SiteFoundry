namespace Dury.SiteFoundry.Admin
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for permissionSelector.
	/// </summary>
	public class permissionSelector : System.Web.UI.UserControl
	{
		public Node CurrentNode 
		{
			get { 
				if (currentNode == null) 
				{
					if (Request.QueryString["id"] != null)
						currentNode = NodeFactory.GetNode(int.Parse(Request.QueryString["id"]));
					else
						currentNode = NodeFactory.CreateNewNode();
				}
				return currentNode; 				
			}
			set { currentNode = value; }
		}
		private Node currentNode;

		protected DataGrid rolesGrid;
		protected DropDownList rolesList;
		protected CheckBox delete;
		protected CheckBox publish;
		protected CheckBox edit;
		protected CheckBox add;
		protected CheckBox view;
		protected Button addButton;
		protected Literal msg;


		private void Page_Load(object sender, System.EventArgs e)
		{
			rolesGrid.ItemDataBound += new DataGridItemEventHandler(rolesGrid_ItemDataBound);
			rolesGrid.ItemCommand += new DataGridCommandEventHandler(rolesGrid_ItemCommand);
			addButton.Click += new EventHandler(addButton_Click);
			addButton.Attributes.Add("onClick","updateParent=false;");
			msg.Text = "";

			if (!IsPostBack)
			{
				fillRolesList();
				fillRoles();
			}			
		}

		private void fillRolesList() 
		{
			ArrayList al = Roles.GetAllRoles();
			foreach(SecurityRole sr in al)
			{
				//if (currentNode.getRolePermission(sr.Name) == 0)
					rolesList.Items.Add(new ListItem(sr.Name,sr.Id.ToString()));
			}
			if (rolesList.Items.Count==0) addButton.Enabled = false;
		}


		#region rolesGrid

		private void fillRoles()
		{
			rolesGrid.DataSource = CurrentNode.GetRoles();
			rolesGrid.DataBind();
		}
		private void rolesGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
			{
				SecurityNode sn = (SecurityNode)e.Item.DataItem;

				if (e.Item.FindControl("rolesList") != null)
				{
					DropDownList rl = (DropDownList)e.Item.FindControl("rolesList");
					rl.DataSource = Roles.GetAllRoles();
					rl.DataTextField = "Name";
					rl.DataValueField = "Id";
					rl.DataBind();
					rl.SelectedValue = sn.RoleID.ToString();
					if (sn.NodeID != currentNode.Id) rl.Enabled = false;
				}

				if (e.Item.FindControl("delete") != null)
				{
					CheckBox d = (CheckBox)e.Item.FindControl("delete");
					d.Checked = ((sn.PermissionLevel & (int)Permission.Delete) > 0);
					d.Enabled = (currentNode.Id == sn.NodeID);
				}
				if (e.Item.FindControl("publish") != null)
				{
					CheckBox d = (CheckBox)e.Item.FindControl("publish");
					d.Checked = ((sn.PermissionLevel & (int)Permission.Publish) > 0);
					d.Enabled = (currentNode.Id == sn.NodeID);
				}
				if (e.Item.FindControl("edit") != null)
				{
					CheckBox d = (CheckBox)e.Item.FindControl("edit");
					d.Checked = ((sn.PermissionLevel & (int)Permission.Edit) > 0);
					d.Enabled = (currentNode.Id == sn.NodeID);
				}
				if (e.Item.FindControl("add") != null)
				{
					CheckBox d = (CheckBox)e.Item.FindControl("add");
					d.Checked = ((sn.PermissionLevel & (int)Permission.Add) > 0);
					d.Enabled = (currentNode.Id == sn.NodeID);
				}
				if (e.Item.FindControl("view") != null)
				{
					CheckBox d = (CheckBox)e.Item.FindControl("view");
					d.Checked = ((sn.PermissionLevel & (int)Permission.View) > 0);
					d.Enabled = (currentNode.Id == sn.NodeID);
				}
		
				if (e.Item.FindControl("saveButton") != null)
				{
					LinkButton sb = (LinkButton)e.Item.FindControl("saveButton");
					if (sn.NodeID != currentNode.Id) sb.Enabled = false;
					sb.Attributes.Add("onClick","updateParent=false;");
				}

				if (e.Item.FindControl("deleteButton") != null)
				{
					LinkButton sb = (LinkButton)e.Item.FindControl("deleteButton");
					if (sn.NodeID != currentNode.Id) sb.Enabled = false;
					sb.Attributes.Add("onClick","updateParent=false;return deleteConfirm('this role?');");
				}

			}
		}

		private void rolesGrid_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Save") 
			{
				SecurityNode sn = Roles.GetSecurityNode(int.Parse(e.CommandArgument.ToString()));
				CheckBox delete = (CheckBox)e.Item.FindControl("delete");
				CheckBox publish = (CheckBox)e.Item.FindControl("publish");
				CheckBox edit = (CheckBox)e.Item.FindControl("edit");
				CheckBox add = (CheckBox)e.Item.FindControl("add");
				CheckBox view = (CheckBox)e.Item.FindControl("view");
				int i = 0;
				if (delete.Checked) i += (int)Permission.Delete;
				if (publish.Checked) i += (int)Permission.Publish;
				if (edit.Checked) i += (int)Permission.Edit;
				if (add.Checked) i += (int)Permission.Add;
				if (view.Checked) i += (int)Permission.View;
				sn.PermissionLevel = i;
				Roles.SaveSecurityNode(sn);
				SFGlobal.UpdateNodes();
				Response.Redirect(Request.RawUrl);
			}
			if (e.CommandName == "Delete") 
			{
				SecurityNode sn = Roles.GetSecurityNode(int.Parse(e.CommandArgument.ToString()));
				Roles.DeleteSecurityNode(sn);
				msg.Text = "role deleted";
				SFGlobal.UpdateNodes();
				Response.Redirect(Request.RawUrl);
			}
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

		private void addButton_Click(object sender, EventArgs e)
		{
			SecurityNode sn = Roles.GetSecurityNode();
			sn.NodeID = CurrentNode.Id;
			sn.RoleID = int.Parse(rolesList.SelectedValue);
			int i = 0;
			if (delete.Checked) i += (int)Permission.Delete;
			if (publish.Checked) i += (int)Permission.Publish;
			if (edit.Checked) i += (int)Permission.Edit;
			if (add.Checked) i += (int)Permission.Add;
			if (view.Checked) i += (int)Permission.View;
			sn.PermissionLevel = i;
            Roles.SaveSecurityNode(sn);
			msg.Text = "added role: " + rolesList.SelectedItem.Text + " level:" + i.ToString();
			SFGlobal.UpdateNodes();
			Response.Redirect(Request.RawUrl);
		}
	}
}
