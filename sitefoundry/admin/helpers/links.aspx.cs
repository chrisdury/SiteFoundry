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

namespace Dury.SiteFoundry.Admin.helpers
{
	/// <summary>
	/// Summary description for links.
	/// </summary>
	public class links : System.Web.UI.Page
	{

		protected System.Web.UI.WebControls.Literal tree;
		protected System.Data.DataTable linksTable;
		protected Repeater linksRepeater;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Page.EnableViewState = false;
			drawTree();
		}

		private void drawTree()
		{
			linksTable = new DataTable();
			linksTable.Columns.Add("label",typeof(string));
			linksTable.Columns.Add("href",typeof(string));
			linksTable.Columns.Add("indent",typeof(int));

			fillLinksTable((Node)Application["nodeRoot"],0);

			linksRepeater.DataSource = linksTable;
			linksRepeater.DataBind();
		}

		public string escapeQuotes(string input) 
		{
			return input.Replace("'","\\'");
		}

		private void fillLinksTable(Node currentNode, int indent)
		{
			linksTable.Rows.Add(new object[] { currentNode.getName(SFGlobal.DefaultLanguage), currentNode.URL, indent} );
			foreach(Node n in currentNode.children)
			{
				fillLinksTable(n,indent+1);
			}

		}

		public string addMargin(int indent)
		{
			int i = indent * 20;
			return i.ToString();
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
