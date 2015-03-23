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
	/// Summary description for documents.
	/// </summary>
	public class documents : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Repeater documentRepeater;
		public string virtualDirectory = SFGlobal.VirtualDirectory;
		public string fileDirectory = SFGlobal.BaseDirectory + SFGlobal.ResourceFileLocation;
		public string fileDirectoryRelative = SFGlobal.VirtualDirectory  + SFGlobal.ResourceFileLocation;

		private void Page_Load(object sender, System.EventArgs e)
		{
			string[] fileTypes = new string[] { "*.pdf", "*.doc", "*.zip", "*.ppt" };
			System.Collections.ArrayList docFiles = new ArrayList();
			foreach(string ext in fileTypes)
			{
				docFiles.AddRange(System.IO.Directory.GetFiles(fileDirectory,ext));                
			}
			DataTable dt = new DataTable("files");
			dt.Columns.Add("filename",typeof(string));
			dt.Columns.Add("size",typeof(int));
			dt.Columns.Add("type",typeof(string));
			dt.Columns.Add("dateModified",typeof(System.DateTime));

			for(int i=0; i<docFiles.Count; i++) 
			{
				System.IO.FileInfo fi = new System.IO.FileInfo((string)docFiles[i]);
				dt.Rows.Add(new object[] { fi.Name,(int)fi.Length,fi.Extension.Remove(0,1),fi.LastWriteTime});
			}
			dt.DefaultView.Sort = "filename";
			documentRepeater.DataSource = dt.DefaultView;
			documentRepeater.DataBind();
		}

		public string getFileSize(int fs)
		{
			//return fs.ToString("N0");
			float f = (float)fs / (float)1024;
			return f.ToString("N2");
		}

		public string trimName(string name)
		{
			int cut = 20;
			if (name.Length > cut)
			{
				return name.Substring(0,cut) + "...";
			}
			else
			{
				return name;
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
