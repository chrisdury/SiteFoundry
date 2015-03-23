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
	///		Summary description for editFormSection.
	/// </summary>
	public class editFormSection : System.Web.UI.UserControl
	{
		protected TextBox title;
		protected TextBox description;
		protected Button saveButton;
		protected Button cancelButton;

		private FormSection fs;
		private string action;

		private void Page_Load(object sender, System.EventArgs e)
		{
			saveButton.Click += new EventHandler(saveButton_Click);
			cancelButton.Click += new EventHandler(cancelButton_Click);
			action = Request.QueryString["action"];
			fs = FormBuilder.GetFormSection(int.Parse(Request.QueryString["formSectionID"]));

			if (action == "delete") 
			{	
				saveButton.Text = "delete";
				title.Enabled =false;
				description.Enabled =false;
				saveButton.Attributes.Add("onClick","deleteWarning(' this section?');");
			}

			if (!IsPostBack) 
			{
				load();
			}

		}

		private void load() 
		{
			title.Text = fs.Title;
			description.Text = fs.Description;
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

			if (action == "edit") 
			{
				fs.Title = title.Text;
				fs.Description = description.Text;
				FormBuilder.SaveFormSection(fs);
			} 
			else if (action == "delete")
			{
				FormBuilder.RemoveSection(fs);
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
			qs.Remove("formSectionID");
			qs.Remove("action");			
			Response.Redirect(Request.Path + "?" + qs.ToString());

		}
	}
}
