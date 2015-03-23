namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;
	using Dury.SiteFoundry.Utils;

	/// <summary>
	///		Summary description for editFieldRadioButtonList.
	/// </summary>
	public class editFieldRadioButtonList : Dury.SiteFoundry.FieldControl
	{

		protected TextBox title;
		protected TextBox description;
		protected CheckBox isRequired;
		protected TextBox items;
		protected RadioButtonList repeatDirection;
		protected Button saveButton;
		protected Button cancelButton;

		private void Page_Load(object sender, System.EventArgs e)
		{
			saveButton.Click += new EventHandler(saveButton_Click);
			cancelButton.Click += new EventHandler(cancelButton_Click);

			if (action == "delete")
			{
				saveButton.Text = "delete";
				title.Enabled = false;
				description.Enabled = false;
				isRequired.Enabled = false;
			}

			if (!IsPostBack) 
			{
				load();
			}
		}

		private void load() 
		{
			title.Text = FormField.Title;
			description.Text = FormField.Description;
			isRequired.Checked = FormField.IsRequired;
			if (FormField.Width == 1) repeatDirection.SelectedValue = "horizontal";
			else repeatDirection.SelectedValue = "vertical";
			items.Text = FormField.ListItems;
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

		private void saveButton_Click(object sender, EventArgs e)
		{
			Field ff = this.FormField;

			if (action == "edit") 
			{
			ff.Title = title.Text;
			ff.Description = description.Text;
			ff.IsRequired = isRequired.Checked;
			ff.ListItems = items.Text;
			ff.Width = (repeatDirection.SelectedValue == "horizontal") ? 1 : 0;
			FormBuilder.SaveField(ff);
			} 
			else if(action=="delete")
			{
				FormBuilder.RemoveField(ff);
			}
			finishEditing();
			
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			finishEditing();
		}

		private void finishEditing()
		{
			QueryString qs = new QueryString(Request.QueryString);
			qs.Remove("fieldID");
			qs.Remove("action");			
			Response.Redirect(Request.Path + "?" + qs.ToString());

		}
	}
}
