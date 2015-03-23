namespace Dury.SiteFoundry.Admin.modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;

	using Dury.SiteFoundry.Security;

	/// <summary>
	///		Summary description for header.
	/// </summary>
	public class header : DuryTools.UI.BaseControl
	{

		private System.Collections.ArrayList menuItems = new System.Collections.ArrayList();
		private System.Collections.Specialized.ListDictionary fileEntries = new System.Collections.Specialized.ListDictionary();
		
		protected System.Web.UI.WebControls.Repeater menuRepeater;

		protected System.Web.UI.WebControls.Literal userstring;
		protected System.Web.UI.WebControls.Literal userToken;
		protected Literal username;


		private void Page_Load(object sender, System.EventArgs e)
		{
			drawMenu();
			username.Text = SFGlobal.CurrentUser.Identity.Name;
		}

		private void drawMenu()
		{
			//string activeCss,linkActiveCss = String.Empty;
			string xmlFile = "admin.config";
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
			System.Data.DataTable dt = new DataTable();
			dt.Columns.Add("label",typeof(string));
			dt.Columns.Add("href",typeof(string));
			dt.Columns.Add("icon",typeof(string));
			dt.Columns.Add("width",typeof(double));

			xd.Load(Server.MapPath(xmlFile));

			int count = xd["menu"].ChildNodes.Count;
			foreach (XmlNode x in xd["menu"]) 
			{
				if (x.Attributes["protected"].Value == "true" && SFGlobal.CurrentUser.IsUserAdmin()) 
				{
					dt.Rows.Add(new object[] { x.Attributes["label"].Value, x.Attributes["href"].Value, x.Attributes["icon"].Value, Math.Ceiling((double)100/(double)count) } );
				} 
				if (x.Attributes["protected"].Value == "false") 
				{
					dt.Rows.Add(new object[] { x.Attributes["label"].Value, x.Attributes["href"].Value, x.Attributes["icon"].Value, Math.Ceiling((double)100/(double)count) } );
				}
			}

			menuRepeater.DataSource = dt;
			menuRepeater.DataBind();
		}


		public string isActive(string href)
		{
			if (System.IO.Path.GetFileName(Request.Path) == href)
				return "class=\"active\"";
			else
				return "";
		}

		public string showImage(string icon)
		{
			return icon;

		}


		public string drawTabsStart(string href)
		{
			if (System.IO.Path.GetFileName(Request.Path) == href)
			{
				return "<td valign=\"top\" class=\"active\" style=\"width:10px\"><img src=\"images/menu-left.gif\" hspace=\"0\"></td>";
				
			}
			else
			{
				return "";
			}

		}
		public string drawTabsEnd(string href)
		{
			if (System.IO.Path.GetFileName(Request.Path) == href)
			{
				return "<td valign=\"top\" class=\"active\" style=\"width:10px\"><img src=\"images/menu-right.gif\" hspace=\"0\"></td>";
				
			}
			else
			{
				return "";
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
