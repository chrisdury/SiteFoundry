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
using DuryTools.Data;

namespace Dury.SiteFoundry.admin.helpers
{
	/// <summary>
	/// Summary description for nodeEdit.
	/// </summary>
	public class nodeEdit : System.Web.UI.Page
	{
		protected Literal pageAction;	
		protected TextBox filename;
		protected DropDownList parentID;			
		protected DropDownList nodeTypeID;
		protected DropDownList pageTemplateID;
		protected CheckBox publish_check;
		protected CheckBox visible_check;
		protected CheckBox visibleMenu_check;
		protected CheckBox visibleSubMenu_check;
		protected DataGrid labelsGrid;
		protected Dury.SiteFoundry.Admin.permissionSelector permissionSelector;
		
		protected TextBox newLabel;
		protected TextBox newLabelLang;
		protected Button addLabel;
		protected RequiredFieldValidator nameBlankValidator;

		protected Button saveButton;
		protected Button saveAndCloseButton;
		protected Button closeButton;

		private int action = 0;
		private int nodeID = 0;

		private Node root;
		private Node currentNode;
		private UserPrincipal user;

		protected override void OnPreRender(EventArgs e)
		{
			saveButton.Attributes.Add("onClick","updateParent=false;");
			saveAndCloseButton.Attributes.Add("onClick","updateParent=false;");
			addLabel.Attributes.Add("onClick","updateParent=false;");
			base.OnPreRender (e);
		}


		private void Page_Load(object sender, System.EventArgs e)
		{
			root = (Node)Application["nodeRoot"];
			user = (UserPrincipal)Context.User;

			closeButton.Attributes.Add("onClick","updateParent=false;closeEditWindow(true);return false;");
			closeButton.CausesValidation = false;
			newLabelLang.Text = SFGlobal.DefaultLanguage;

		
			switch(Request.QueryString["action"])
			{
				case "add":
					action = 0;
					if (Request.QueryString["id"] != null) nodeID = int.Parse(Request.QueryString["id"]);
                    pageAction.Text = "add";
					saveButton.Text = "add another";
					saveButton.Click += new EventHandler(saveButton_Click);
					saveAndCloseButton.Text = "add and close";
					saveAndCloseButton.Click += new EventHandler(saveAndCloseButton_Click);
					addLabel.Click += new EventHandler(addLabel_Click);
					permissionSelector.Visible = false;
					break;
				case "edit":
					action = 1;
					pageAction.Text = "edit";
					nodeID = (Request.QueryString["id"] != null && Request.QueryString["id"] != "") ? int.Parse(Request.QueryString["id"]) : -1;
					saveButton.Click += new EventHandler(saveButton_Click);
					saveButton.Attributes.Add("onClick","updateParent=false;");
					saveAndCloseButton.Click += new EventHandler(saveAndCloseButton_Click);
					saveAndCloseButton.Attributes.Add("onClick","updateParent=false;");
					addLabel.Click += new EventHandler(addLabel_Click);
					addLabel.Attributes.Add("onClick","updateParent=false;");
					break;
				case "delete":
					action = 2;
					nameBlankValidator.Visible = false;
					saveButton.Text = "delete";
					saveButton.Click += new EventHandler(saveButtonDelete_click);
					saveButton.CssClass = "button red";
					saveButton.Attributes.Add("onClick","return deleteWarning('this node?');updateParent=false;");
					saveAndCloseButton.Visible = false;
					nameBlankValidator.Visible = false;
					pageAction.Text =  "delete";
					nodeID = (Request.QueryString["id"] != null && Request.QueryString["id"] != "") ? int.Parse(Request.QueryString["id"]) : -1;
					filename.Enabled = false;
					parentID.Enabled = false;
					nodeTypeID.Enabled = false;
					pageTemplateID.Enabled = false;
					publish_check.Enabled = false;
					visible_check.Enabled = false;
					addLabel.Enabled = false;
					break;
				default:
					Response.End();
					break;
			}
			if (!IsPostBack)
			{
				
				fillNodeTypes();
				fillTemplates();
				foreach(Node n in NodeFactory.UserNodes)
					fillParentSelect(n,0);
				if (SFGlobal.CurrentUser.IsUserAdmin()) parentID.Items.Insert(0,new ListItem("-no parent-","0"));
				loadNode();
				applySecurity();
				
			}
		
		}

		private void applySecurity()
		{
			if (user.CanUserModify(currentNode)) 
			{
				return;
			}
				/*

				if (user.IsInRole(SFGlobal.AdminstratorRoleName) || user.IsInRole(SFGlobal.PublisherRoleName) || user.IsInRole(SFGlobal.EditorRoleName))
				{
					return;
				}
				*/
			else
			{
				if (action == 0) return;
				Response.Write("not authorized to add or modify nodes");
				Response.End();
			}
		}


		private void fillParentSelect(Node n,int indent) 
		{
			string p = "";
			for (int i=0;i<indent*2;i++) { p += (char)160; }
			//if (SFGlobal.CurrentUser.IsUserAdmin()) 
			parentID.Items.Add(new ListItem(p + n.getName(SFGlobal.DefaultLanguage),n.Id.ToString()));

			foreach(Node c in n.children) 
			{
				fillParentSelect(c,indent+1);
			}
		}

		private void fillNodeTypes()
		{
			if (Dury.SiteFoundry.Utils.TemplateLoader.Templates != null)
			{
				foreach(Dury.SiteFoundry.Utils.NodeTemplate nt in Dury.SiteFoundry.Utils.TemplateLoader.Templates)
				{
					nodeTypeID.Items.Add(new ListItem(nt.Name,nt.ID.ToString()));
				}
				nodeTypeID.DataBind();
			}
			else 
			{
				throw new DuryTools.ErrorHandler("problem loading templates from TemplateLoader...");
			}
		}

		private void fillTemplates() 
		{
			foreach(NodePageTemplate npt in Dury.SiteFoundry.PageTemplates.PageTemplatesList) 
			{
				ListItem li = new ListItem(npt.Name,npt.TemplateID.ToString());
				pageTemplateID.Items.Add(li);
			}
			pageTemplateID.Items.Insert(0,new ListItem("inherit","-1"));
			//pageTemplateID.DataBind();
		}

		private void loadNode()
		{
			if (nodeID > 0)
			{
				currentNode = root.Find(nodeID);
				filename.Text = currentNode.Filename;

				if (parentID.Items.FindByValue(currentNode.ParentID.ToString()) == null) 
				{
					parentID.Items.Clear();
					
                    Node parent = NodeFactory.RootNode.Find(currentNode.ParentID);
					parentID.Items.Add(new ListItem(parent.getName(SFGlobal.DefaultLanguage),parent.Id.ToString()));

					parentID.Enabled = false;
				}
				else
					parentID.SelectedValue = currentNode.ParentID.ToString();

				nodeTypeID.SelectedValue = currentNode.TypeID.ToString();
				if (currentNode.PageTemplateID > 0)
					pageTemplateID.SelectedValue = currentNode.PageTemplateID.ToString();
				else
					pageTemplateID.SelectedValue = "-1";
				publish_check.Checked = currentNode.Publish;
				visible_check.Checked = currentNode.Visible;
				visibleMenu_check.Checked = currentNode.VisibleMenu;
				visibleSubMenu_check.Checked = currentNode.VisibleSubMenu;
				labelsGridBind();
				permissionSelector.CurrentNode = currentNode;
			} 
			else 
			{
				currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node));
			}
		}
    
		private void saveNode()
		{
			if (nodeID > 0) //update
			{
				currentNode = root.Find(nodeID);
			}	
			else//insert
			{
				currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node));
				currentNode.DateCreated = currentNode.DateModified = DateTime.Now;
			}
			currentNode.Filename = filename.Text;
			//if (nodeID <= 0) currentNode.Rank = 0;
	
			Node parent = root.Find(int.Parse(parentID.SelectedValue));
			if (nodeID == 0) 
			{
				if (parent.children.Count>0)
					currentNode.Rank = parent.children[parent.children.Count-1].Rank + 1;
				else
					currentNode.Rank = 0;
			}
			currentNode.ParentID = int.Parse(parentID.SelectedValue);
			currentNode.TypeID = int.Parse(nodeTypeID.SelectedValue);
			currentNode.PageTemplateID = int.Parse(pageTemplateID.SelectedValue);
			currentNode.Publish = publish_check.Checked;
			currentNode.Visible = visible_check.Checked;
			currentNode.VisibleMenu = visibleMenu_check.Checked;
			currentNode.VisibleSubMenu = visibleSubMenu_check.Checked;
			currentNode.DateModified = DateTime.Now;
			SFGlobal.ObjectManager.PersistChanges(currentNode);
			SFGlobal.UpdateNodes();

			// hack! -- skip the redirect if "add & close" was clicked
			if (nodeID > 0 && action == 0) 
			{
				Response.Redirect(Request.Path + "?action=add");
			}

		}

		private void deleteNode()
		{
			currentNode = (Node)SFGlobal.ObjectManager.GetObject(typeof(Node),nodeID);
			NodeFactory.DeleteNode(currentNode);
			SFGlobal.UpdateNodes();
		}


		#region namesGrid


		protected void labelsGridBind()
		{
			labelsGrid.DataSource = SFGlobal.DAL.execDataSet("SELECT * FROM NodeNames WHERE nodeID="+nodeID);
			labelsGrid.DataBind();
		}



		protected void labelsGrid_Edit( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			labelsGrid.EditItemIndex = e.Item.ItemIndex;
			labelsGridBind();
		}

		protected void labelsGrid_Update( System.Object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);
			string lang = ((TextBox)e.Item.FindControl("lang")).Text;
			string name = ((TextBox)e.Item.FindControl("name")).Text;
			DataContainer dc = new DataContainer("NodeNames");
			DataRow dr = dc.GetRowByKey("id",itemID);
			dr["lang"] = lang;
			dr["name"] = name;
			dc.UpdateRow(dr);
			//SFGlobal.DAL.execNonQuery("UPDATE NodeNames SET lang='" + lang + "', name='" + name + "' WHERE id=" + itemID);
			labelsGrid.EditItemIndex = -1;
			labelsGridBind();
		}

		protected void labelsGrid_Delete( System.Object sender, DataGridCommandEventArgs e ) 
		{
			int itemID = int.Parse(e.Item.Cells[0].Text);
			SFGlobal.DAL.execNonQuery("DELETE FROM NodeNames WHERE id = " + itemID.ToString());
			labelsGridBind();			
		}

		protected void labelsGrid_Cancel(Object sender, DataGridCommandEventArgs e) 
		{
			labelsGrid.EditItemIndex = -1;
			labelsGridBind();
		}

		protected void labelsGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.FindControl("deleteButton") != null)
			{
				((LinkButton) e.Item.FindControl("deleteButton")).Attributes.Add("onClick", "updateParent=false;return deleteWarning('delete this label?');");
			}

		}
		#endregion

		#region event handlers


		private void saveButton_Click(object sender, EventArgs e)
		{
			saveNode();
		}

		private void saveAndCloseButton_Click(object sender, EventArgs e)
		{
			// hack! -- set action flag to skip redirect if add & close was clicked.
			action = 1;
			saveNode();
			close(sender,e,false);
		}

		private void updateParentWin()
		{
			//Response.Write("<script language=\"javascript\">alert(window.opener.document.forms[0]);//window.opener.document.forms[0].submit();</script>");
			//Response.Write("<script language=\"javascript\">window.opener.document.location.href = window.opener.document.location.href;</script>");
			//Response.Write("<script language=\"javascript\">closeEditWindow();</script>");			
		}

		private void saveButtonDelete_click(object sender, EventArgs e)
		{
			deleteNode();
			close(sender,e,true);
		}

		public void close(object sender, System.EventArgs e, bool isDelete)
		{
			//updateParentWin();
			if (isDelete)
				Response.Write("<script language=\"javascript\">window.onload = function() { updateParent=true;window.close(); }</script>");
			else
				Response.Write("<script language=\"javascript\">window.onload = function() { updateParent=true;window.close(); }</script>");
			//Response.Write("<script language=\"javascript\">window.onload = function() { closeEditWindow(); }</script>");
		}

		private void addLabel_Click(object sender, EventArgs e)
		{
			string lang = newLabelLang.Text;
			string name = newLabel.Text;

			if (currentNode == null) 
			{
				saveNode();
			} 
			else if (nodeID != 0)
			{
				currentNode = root.Find(nodeID);
			}
			DataContainer dc = new DataContainer("NodeNames");
			DataRow dr = dc.GetNewRow();
			dr["nodeID"] = currentNode.Id;
			dr["lang"] = lang;
			dr["name"] = name;
			dc.UpdateRow(dr);

			DuryTools.Utils.CustomQueryString cqs = DuryTools.Utils.QueryStringUtils.QueryStringBuilder(Request.RawUrl);
			cqs["action"] = "add";
			cqs["id"] = currentNode.Id.ToString();
			Response.Redirect(Request.Path + "?" + cqs.ToString());

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
		}
		#endregion


	}
}
