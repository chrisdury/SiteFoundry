using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;
using Dury.SiteFoundry.Utils;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for pages.
	/// </summary>
	public class Pages : Dury.SiteFoundry.UI.BasePage
	{
		protected System.Web.UI.WebControls.Literal menu;
		protected System.Web.UI.WebControls.PlaceHolder controlHolder;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.EnableViewState = false;
			Node n = (Node)Context.Items["currentNode"];
			Control c = TemplateLoader.GetTemplate(n);
			controlHolder.Controls.Add(c);
			Response.Expires = 0;
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
