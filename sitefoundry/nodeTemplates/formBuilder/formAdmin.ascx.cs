namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Utils;

	/// <summary>
	///		Summary description for formBuilder.
	/// </summary>
	public class formAdmin : DuryTools.UI.BaseControl
	{
		protected Node currentNode;
		protected Form currentForm;

		protected HtmlGenericControl formTitleHeader;
		protected HtmlGenericControl formPropertiesPanel;
		protected HtmlTable formPropertiesTable;
		protected TextBox formTitle;
		protected TextBox description;
		protected TextBox successText;
		protected CheckBox formActiveCheck;
		protected CheckBox formDisplaySectionCheck;
		protected ImageButton saveFormButton;
		protected DropDownList displaySectionsList;
		protected HtmlGenericControl addPanel;
		protected TextBox sectionTitle;
		protected TextBox sectionDescription;
		protected TextBox newFieldTitle;
		protected DropDownList newFieldTypeList;
		protected DropDownList newFieldSectionList;
		protected Button addFieldButton;
		protected Button addSectionButton;
		protected HtmlGenericControl editFieldPanel;
		protected HtmlTable fieldPanelsTable;
		protected PlaceHolder fieldsHolder;
		protected PlaceHolder previewHolder;
		protected PlaceHolder editFieldHolder;

		private void Page_Load(object sender, System.EventArgs e)
		{
           // saveFormButton.Click += new EventHandler(saveFormButton_Click);
			saveFormButton.Click += new ImageClickEventHandler(saveFormButton_Click);
			addFieldButton.Click += new EventHandler(addFieldButton_Click);
			addSectionButton.Click += new EventHandler(addSectionButton_Click);

			addPanel.Visible = true;
			formPropertiesPanel.Visible = true;
			editFieldPanel.Visible= false;
			fieldPanelsTable.Visible = false;

			LoadForm();
			if (!IsPostBack) 
			{
				
				fillFieldTypesList();
				fillSectionList();
			}
		}

		protected override void CreateChildControls()
		{
			//base.CreateChildControls ();


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

		#region LOAD AND SAVE
		public void LoadForm() 
		{
			int nodeID = int.Parse(Request.QueryString["nodeID"]);
			currentNode = NodeFactory.UserNodes.Find(nodeID);
			if (currentNode == null) throw new DuryTools.ErrorHandler("node not found, id=" + nodeID.ToString());
			currentForm = FormBuilder.GetForm(currentNode);

			if (currentNode.FormNodeIDs.Count >= 1)
			{
				formTitleHeader.InnerText = currentForm.Title;
				formPropertiesTable.Attributes.Add("style","display:none;");
				formTitle.Text = currentForm.Title;
				formActiveCheck.Checked = currentForm.IsActive;
				formDisplaySectionCheck.Checked = currentForm.DisplaySections;
				description.Text = currentForm.Description;
				successText.Text = currentForm.SuccessText;
				drawFields();
				drawPreview();
				drawEditBox();
				fieldPanelsTable.Visible = true;
			}
			else 
			{
				//saveFormButton.Text = "Create Form";
				saveFormButton.ImageUrl = "~/admin/images/createform.gif";
			}

		}

		private void fillSectionList()
		{
			foreach(FormSection fs in currentForm.FormSections)
			{
				newFieldSectionList.Items.Add(new ListItem(fs.Title,fs.FormSectionID.ToString()));
			}
		}
		private void fillFieldTypesList() 
		{
			foreach(FieldTypeInfo fti in FormBuilder.FieldTypesList)
			{
				newFieldTypeList.Items.Add(new ListItem(fti.Name,fti.ID.ToString()));
			}
		}

		#endregion

		private void togglePanel(HtmlGenericControl panel)
		{
			Page.RegisterClientScriptBlock("togglePanel","<script language=\"javascript\">window.setTimeout(\"showQuestions()\",250);</script>");
		}

		private void drawFields()
		{
			if (currentForm.FormSections.Count == 0) 
			{
				togglePanel(this.addPanel);
				addFieldButton.Enabled = false;
			}

			fieldsHolder.Controls.Clear();
			foreach(FormSection fs in currentForm.FormSections)
			{
				formBuilder.formSectionAdminListControl fsalc = (formBuilder.formSectionAdminListControl)Page.LoadControl("~/nodeTemplates/formBuilder/formSectionAdminListControl.ascx");
				fsalc.Section = fs;
				fieldsHolder.Controls.Add(fsalc);
				fieldsHolder.Controls.Add(new LiteralControl("<ol>"));
				if (fs.FieldSectionIDs.Count == 0) togglePanel(this.addPanel);
				foreach(Field f in fs.FieldSectionIDs)
				{
					formBuilder.formFieldAdminListControl ffalc = (formFieldAdminListControl)Page.LoadControl("~/nodeTemplates/formBuilder/formFieldAdminListControl.ascx");
					ffalc.FormField = f;
					fieldsHolder.Controls.Add(ffalc);
				}
				fieldsHolder.Controls.Add(new LiteralControl("</ol>"));
			}
		}

		private void drawPreview()
		{
			previewHolder.Controls.Clear();
			foreach(FormSection fs in currentForm.FormSections)
			{
				string s = "<fieldset><legend>" + fs.Title + "</legend>";
				if (currentForm.DisplaySections) previewHolder.Controls.Add(new LiteralControl(s));
				foreach(Field f in fs.FieldSectionIDs)
				{
					//FieldControl fc = (FieldControl)ge
					previewHolder.Controls.Add(getFieldDisplayControl(f));
				}
				if (currentForm.DisplaySections) previewHolder.Controls.Add(new LiteralControl("</fieldset>"));
			}
		}



		private void drawEditBox() 
		{
			if (Request.QueryString["fieldID"] != null)
			{
				editFieldPanel.Visible = true;
				addPanel.Visible = false;
				formPropertiesPanel.Visible = false;
				Field f = FormBuilder.GetField(int.Parse(Request.QueryString["fieldID"]));
                editFieldHolder.Controls.Add(getFieldEditControl(f));
			} 
			else if (Request.QueryString["formSectionID"] != null)
			{
				editFieldPanel.Visible = true;
				addPanel.Visible = false;
				formPropertiesPanel.Visible = false;
				editFieldHolder.Controls.Add(Page.LoadControl("~/nodeTemplates/formBuilder/editFormSection.ascx"));
			}
		}

		private FieldControl getFieldControl(Field f, bool isDisplayControl)
		{
			FieldTypeInfo fti = FormBuilder.GetFieldTypeInfo(f.FieldTypeID);
			string s = (isDisplayControl) ? fti.DisplaySrc : fti.EditSrc;
			FieldControl fc = (FieldControl)Page.LoadControl("~/nodeTemplates/formBuilder/" + s);
			fc.FormField = f;
			fc.Mode = (Request.QueryString["action"] != null) ? Request.QueryString["action"] : "";
			return fc;
		}
		private FieldControl getFieldEditControl(Field f)
		{
			return getFieldControl(f,false);
		}
		private FieldControl getFieldDisplayControl(Field f)
		{
			FieldControl fc = getFieldControl(f,true);
			//fc.Mode = "Display";
			return fc;
		}



		#region EVENTS
		private void saveFormButton_Click(object sender, ImageClickEventArgs e)
		{
			//currentForm = FormBuilder.GetForm(currentNode);
            currentForm.Title = formTitle.Text;
			currentForm.Description = description.Text;
			currentForm.SuccessText = successText.Text;
			currentForm.IsActive = formActiveCheck.Checked;
			currentForm.DisplaySections = formDisplaySectionCheck.Checked;
			FormBuilder.SaveForm(currentForm);
			//SFGlobal.UpdateNodes();
			Response.Redirect(Request.Path + "?" + Request.QueryString);

		}

		private void addFieldButton_Click(object sender, EventArgs e)
		{
			FormSection fs = FormBuilder.GetFormSection(int.Parse(newFieldSectionList.SelectedValue));
			Field f = FormBuilder.AddFieldToSection(fs,int.Parse(newFieldTypeList.SelectedValue),newFieldTitle.Text);
			newFieldTitle.Text = "";
			LoadForm();

		}

		private void addSectionButton_Click(object sender, EventArgs e)
		{
			FormSection fs = FormBuilder.AddSection(currentForm,sectionTitle.Text,sectionDescription.Text);
			sectionTitle.Text = "";
			sectionDescription.Text = "";
			fillSectionList();
			LoadForm();
			QueryString qs = new QueryString(Request.QueryString);
			Response.Redirect(Request.Path + "?" + qs.ToString());
		}

		public void EditField(Field f)
		{
			//showAlert("editing: " + f.Title);
			//editFieldHolder.Controls.Add(this.getFieldEditControl(f));
			QueryString qs = new QueryString(Request.QueryString);
			qs.Set("fieldID",f.FieldID.ToString());
			qs.Set("action","edit");
			qs.Remove("formSectionID");
			Response.Redirect(Request.Path + "?" + qs.ToString());
		}

		public void DeleteField(Field f)
		{
			QueryString qs = new QueryString(Request.QueryString);
			qs.Set("fieldID",f.FieldID.ToString());
			qs.Set("action","delete");
			qs.Remove("formSectionID");
			Response.Redirect(Request.Path + "?" + qs.ToString());

		}

		public void EditSection(FormSection fs)
		{
			QueryString qs = new QueryString(Request.QueryString);
			qs.Set("formSectionID",fs.FormSectionID.ToString());
			qs.Set("action","edit");
			qs.Remove("fieldID");
			Response.Redirect(Request.Path + "?" + qs.ToString());

		}
		public void DeleteSection(FormSection fs) 
		{
			QueryString qs = new QueryString(Request.QueryString);
			qs.Set("formSectionID",fs.FormSectionID.ToString());
			qs.Set("action","delete");
			qs.Remove("fieldID");
			Response.Redirect(Request.Path + "?" + qs.ToString());
		}

		public void changeSectionRank(FormSection fs, int dir)
		{
			FormBuilder.ChangeSectionRank(fs,dir);
			QueryString qs = new QueryString(Request.QueryString);
			Response.Redirect(Request.Path + "?" + qs.ToString());
		}
		public void changeFieldRank(Field f, int dir)
		{
			FormBuilder.ChangeFieldRank(f,dir);
			QueryString qs = new QueryString(Request.QueryString);
			Response.Redirect(Request.Path + "?" + qs.ToString());
		}


		#endregion

	}
}
