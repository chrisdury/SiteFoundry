namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for displayFieldCheckBoxList.
	/// </summary>
	public class displayFieldCheckBoxList : Dury.SiteFoundry.FieldControl
	{

		protected Literal title;
		protected Literal description;
		protected CheckBoxList answer;

		private void Page_Load(object sender, System.EventArgs e)
		{
			title.Text = this.FormField.Title;
			description.Text = FormField.Description;

			if (FormField.Width == 1) answer.RepeatDirection = RepeatDirection.Horizontal;

			loadItems();

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
			{
				string s = "";
				foreach(ListItem li in answer.Items)
				{
					if (li.Selected)
						s += li.Value + "|";
				}
				if (s.Length > 1)
					return s.Substring(0,s.Length-1);
				else
					return s;
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
