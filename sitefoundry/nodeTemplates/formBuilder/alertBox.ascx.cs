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
	///		Summary description for alertBox.
	/// </summary>
	public class alertBox : System.Web.UI.UserControl
	{
		protected HtmlGenericControl alertMsg;
		public string alertID;
		private void Page_Load(object sender, System.EventArgs e)
		{
			alertID = alertMsg.UniqueID.Replace(":","_");
						
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			//Response.Write(SFGlobal.GetUserAlert());
			if (SFGlobal.GetUserAlert() != null)
			{
				alertMsg.InnerText = SFGlobal.GetUserAlert().ToString();
				alertMsg.Visible = true;
				SFGlobal.ClearUserAlert();
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
