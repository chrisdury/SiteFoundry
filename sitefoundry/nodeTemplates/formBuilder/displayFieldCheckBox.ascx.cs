namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for displayFieldCheckBox.
	/// </summary>
	public class displayFieldCheckBox : Dury.SiteFoundry.FieldControl
	{
		protected Literal title;
		protected Literal description;
		protected CheckBox answer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			title.Text = this.FormField.Title;
			answer.Text = FormField.Description;
			if (Mode == "Display") 
			{
				if (FormField.IsRequired) 
				{
					RequiredFieldValidator rfv = new RequiredFieldValidator();
					rfv.ControlToValidate = answer.ID;
					rfv.Display = ValidatorDisplay.Dynamic;
					rfv.Text = "required";
					rfv.CssClass = "validatorDisplay";
					//Controls.Add(rfv);
				}
			}

		}

		public override string GetValue()
		{
			if (Page.IsValid)
				return answer.Checked.ToString();
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
