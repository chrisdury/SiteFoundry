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
	///		Summary description for editFieldText.
	/// </summary>
	public class editFieldText : Dury.SiteFoundry.FieldControl
	{

		protected TextBox title;
		protected TextBox description;
		protected TextBox validationExpression;
		protected CheckBox isRequired;
		protected CheckBox isValidated;
		protected TextBox width;
		protected TextBox maxLength;
		protected Button saveButton;
		protected Button cancelButton;


		private void Page_Load(object sender, System.EventArgs e)
		{
			saveButton.Click += new EventHandler(saveButton_Click);
			cancelButton.Click += new EventHandler(cancelButton_Click);
			if (action == "delete") 
			{	
				saveButton.Text = "delete";
				title.Enabled =false;
				description.Enabled =false;
				isRequired.Enabled =false;
				isValidated.Enabled =false;
				validationExpression.Enabled =false;
				width.Enabled = false;
				maxLength.Enabled = false;
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
			isValidated.Checked = FormField.IsValidated;
			validationExpression.Text = FormField.ValidationExpression;
			if (FormField.Width > 0) width.Text = FormField.Width.ToString();
			if (FormField.MaxLength > 0) maxLength.Text = FormField.MaxLength.ToString();
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
				ff.IsValidated = isValidated.Checked;
				ff.ValidationExpression = validationExpression.Text;
				ff.MaxLength = (maxLength.Text != "") ? int.Parse(maxLength.Text) : 0;
				ff.Width = (width.Text != "") ? int.Parse(width.Text) : 0;
				FormBuilder.SaveField(ff);
			} 
			else if (action == "delete")
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
