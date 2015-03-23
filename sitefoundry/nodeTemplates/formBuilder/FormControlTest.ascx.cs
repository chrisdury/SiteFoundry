namespace SiteFoundry.nodeTemplates.formBuilder
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Dury.SiteFoundry;

	/// <summary>
	///		Summary description for FormControlTest.
	/// </summary>
	public class FormControlTest : System.Web.UI.UserControl
	{
		protected FormDisplayControl test;

		private void Page_Load(object sender, System.EventArgs e)
		{
			test.CurrentNode = (Node)Context.Items["currentNode"];
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
