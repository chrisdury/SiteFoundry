namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for displayFieldDropDownList.
	/// </summary>
	public class displayFieldDropDownList : Dury.SiteFoundry.FieldControl
	{

		protected Literal title;
		protected Literal description;
		protected DropDownList answer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			title.Text = this.FormField.Title;
			description.Text = FormField.Description;

			if (FormField.Width > 0) answer.Width = FormField.Width;

			if (FormField.ListItems != null) loadItems();
			if (Mode == "Display") 
			{
				if (FormField.IsRequired) 
				{
                    CompareValidator cv = new CompareValidator();
					cv.ControlToValidate = answer.ID;
					cv.Display = ValidatorDisplay.Dynamic;
					cv.Text = "required";
					cv.CssClass = "validatorDisplay";
					cv.Operator = ValidationCompareOperator.NotEqual;
					cv.ValueToCompare = "-!-";
					Controls.Add(cv);
				}
			}

		}

		public override string GetValue()
		{
			if (Page.IsValid) 
			{
				return answer.SelectedValue;
			}
			else
				throw new Exception("control isn't valid :" + this.ID);
		}

		private void loadItems() 
		{
			string[] items = FormField.ListItems.Split(',');
			foreach(string i in items)
			{
				ListItem li = new ListItem();
				try 
				{
					if (i.IndexOf("|") > 0) 
					{
						string[] ii = i.Split('|');
						li.Text = ii[0];
						li.Value = ii[1];
					} 
					else 
					{
						li.Text = li.Value = i;
					}
				}
				catch (Exception e)
				{
					li.Text = "!!problem!!";
				}
				answer.Items.Add(li);
			}
			answer.Items.Insert(0,new ListItem("choose...","-!-"));
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
