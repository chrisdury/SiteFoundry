namespace Dury.SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for formFieldAdminListControl.
	/// </summary>
	public class formFieldAdminListControl : System.Web.UI.UserControl
	{
		protected LinkButton title;
		protected LinkButton upButton;
		protected LinkButton dnButton;
		protected LinkButton editButton;
		protected LinkButton deleteButton;
		public Field FormField;

		public string name;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (FormField.Title == "")
				title.Text = "new question";
			else
				title.Text = SFGlobal.TruncateText(FormField.Title,30);
			title.Command += new CommandEventHandler(button_Command);
			title.CommandName = "edit";
			title.Attributes.Add("onMouseOver","showControls('fl_" + FormField.FieldID.ToString() + "');");
			upButton.Command += new CommandEventHandler(button_Command);
			upButton.CommandName = "up";
			dnButton.Command += new CommandEventHandler(button_Command);
			dnButton.CommandName = "dn";
			editButton.Command += new CommandEventHandler(button_Command);
			editButton.CommandName = "edit";
			deleteButton.Command += new CommandEventHandler(button_Command);
			deleteButton.CommandName = "delete";
		}

		public string IsSelected() 
		{
			if (Request.QueryString["fieldID"] != null && int.Parse(Request.QueryString["fieldID"]) == FormField.FieldID)
				return "selected";
			return "";
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

		private void button_Command(object sender, CommandEventArgs e)
		{
			Dury.SiteFoundry.nodeTemplates.formBuilder.formAdmin parent = (Dury.SiteFoundry.nodeTemplates.formBuilder.formAdmin)this.Parent.Parent.Parent.Parent.Parent;
			switch(e.CommandName) 
			{
				case "up":
					parent.changeFieldRank(FormField,-1);
					break;
				case "dn":
					parent.changeFieldRank(FormField,1);
					break;
				case "edit":
					parent.EditField(FormField);
					break;
				case "delete":
					parent.DeleteField(FormField);
					break;
			}
		}
	}
}
