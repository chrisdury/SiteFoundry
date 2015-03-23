namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for displayFieldPassword.
	/// </summary>
	public class displayFieldPassword : Dury.SiteFoundry.FieldControl
	{
		protected Literal title;
		protected Literal description;
		protected TextBox answer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			title.Text = this.FormField.Title;
			description.Text = FormField.Description;
			if (FormField.Width > 0) answer.Width = FormField.Width;
			if (FormField.MaxLength > 0) answer.MaxLength = FormField.MaxLength;
			if (Mode == "Display") 
			{
				if (FormField.IsRequired) 
				{
					RequiredFieldValidator rfv = new RequiredFieldValidator();
					rfv.ControlToValidate = answer.ID;
					rfv.Display = ValidatorDisplay.Dynamic;
					rfv.Text = "required";
					rfv.CssClass = "validatorDisplay";
					Controls.Add(rfv);
				}
				if (FormField.IsValidated) 
				{
					RegularExpressionValidator rev = new RegularExpressionValidator();
					rev.ControlToValidate = answer.ID;
					rev.Display = ValidatorDisplay.Dynamic;
					rev.Text = "invalid";
					rev.CssClass = "validatorDisplay";
					rev.ValidationExpression = FormField.ValidationExpression;
					Controls.Add(rev);
				}
			}

		}

		public override string GetValue()
		{
			if (Page.IsValid)
				return answer.Text;
			else
				throw new Exception("control isn't valid :" + this.ID);
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
	}
}
