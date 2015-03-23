using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;

namespace Dury.SiteFoundry.Admin
{
	/// <summary>
	/// Summary description for resources.
	/// </summary>
	public class resources :  DuryTools.UI.BasePage
	{
		private string searchPattern = "*.*";
		private string rootFileDirectory;
		public string absoluteUrl = string.Empty;
		public string relativeUrl = string.Empty;
		protected DropDownList directorySelect;
		protected DropDownList uploadFileDirectorySelect;
		protected DropDownList moveFilesDirectorySelect;
		protected TextBox newDirectoryName;
		protected Button newDirectoryButton;
		protected Label newDirectoryMsg;
		protected Button moveFilesButton;
		protected Label moveFilesMsg;
		protected Button deleteFilesButton;
		protected Label deleteMsg;
		protected Button deleteDirectoryButton;
		protected Repeater filesRepeater;
		protected DropDownList fileFilter;
		protected Label msg;
		protected HtmlInputFile fileToUpload;
		protected Button upload;
		protected PlaceHolder pagination;
		protected DropDownList paginationList;
		protected Literal stats;
		protected UserPrincipal user;

		private string currentDirectory;
		private int pageSize = 30;
		private int currentPage;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.PageTitle = "File Upload";
			this.rootFileDirectory = SFGlobal.BaseDirectory + SFGlobal.ResourceFileLocation;
			this.absoluteUrl = System.Configuration.ConfigurationSettings.AppSettings["/"];
			this.relativeUrl = "/" + System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] + SFGlobal.ResourceFileLocation;

			filesRepeater.ItemDataBound += new RepeaterItemEventHandler(filesRepeater_ItemDataBound);
			directorySelect.AutoPostBack = true;
			directorySelect.SelectedIndexChanged += new EventHandler(directorySelect_SelectedIndexChanged);

			newDirectoryButton.Click += new EventHandler(newDirectoryButton_Click);
			moveFilesButton.Click += new EventHandler(moveFilesButton_Click);
			deleteFilesButton.Click += new EventHandler(deleteFilesButton_Click);
			deleteFilesButton.Attributes.Add("onClick","return deleteConfirm('the selected file(s)?');");
			deleteDirectoryButton.Click += new EventHandler(deleteDirectoryButton_Click);
			deleteDirectoryButton.Attributes.Add("onClick","return deleteConfirm('the current directory?');");

			user = (UserPrincipal)Context.User;

			//currentPage = (Request.QueryString["page"] != null) ? int.Parse(Request.QueryString["page"]) : 0;
			paginationList.AutoPostBack = true;
			paginationList.SelectedIndexChanged += new EventHandler(paginationList_SelectedIndexChanged);


			deleteDirectoryButton.Enabled = false;
			moveFilesButton.Enabled = false;
			if (user.IsInRole(SFGlobal.AdminstratorRoleName) || user.IsInRole(SFGlobal.PublisherRoleName))
			{
				deleteDirectoryButton.Enabled = true;
				moveFilesButton.Enabled = true;
			}
	
			if (!IsPostBack) 
			{
				this.currentDirectory = this.rootFileDirectory;
				fillDirectorySelects();
				initFileTypes();
				ViewState["sortby"] = null;
				ViewState["sortDirection"] = null;
				currentPage = 0;
				msg.Text = "";
				newDirectoryMsg.Text = "";
				getFiles();
			}
			
		}
		private void initFileTypes() 
		{
			System.Collections.ArrayList fileTypes = new System.Collections.ArrayList();
			fileTypes.Add("ALL");
			string[] files = System.IO.Directory.GetFiles(rootFileDirectory,searchPattern);
			for(int i=0; i<files.Length; i++) 
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(files[i]);
				if (fi.Extension == ".db") continue;
				if (!fileTypes.Contains(fi.Extension.ToLower()))
					fileTypes.Add(fi.Extension.ToLower());
			}
			fileFilter.DataSource = fileTypes;
			fileFilter.DataBind();
		}

		public void fileFilter_changed(object sender, System.EventArgs e)
		{
			this.currentPage = 0;
			getFiles();
		}

		private void setSearchPattern()
		{
			switch(fileFilter.SelectedItem.Text) 
			{
				case "ALL":
					searchPattern = "*.*";
					break;
				default:
					searchPattern = "*" + fileFilter.SelectedItem.Text;
					break;
			}
		}

		private void fillDirectorySelects() 
		{
			string[] dirs = getDirectories(this.rootFileDirectory).Split('|');
			object[] selects = {directorySelect,uploadFileDirectorySelect,moveFilesDirectorySelect};

			object[] lic = new object[] {};
			foreach(string d in dirs) 
			{
				if (d.Length > 0) 
				{
					foreach(DropDownList ddl in selects) 
					{
						//ddl.Items.Clear();
						if (d != this.rootFileDirectory) 
						{
							string name = d.Replace(this.rootFileDirectory,"");
							ListItem li = new ListItem(name,d.Replace(this.rootFileDirectory,""));
							ddl.Items.Add(li);
		
						} 
						else 
						{
							ddl.Items.Add(new ListItem("Uploaded Files - Root",""));
						}
						//ddl.SelectedValue = currentDirectory.Replace(this.rootFileDirectory,"");
					}
				}
			}

			
		}

		private string getDirectories(string currentDir)
		{
			string s = currentDir + "|";
			foreach(string d in System.IO.Directory.GetDirectories(currentDir)) 
			{
				s += getDirectories(d);
			}
			return s;			
		}
		
		private void getFiles() 
		{
			this.currentDirectory = this.getCurrentDirectory();
			int totalFileCount = System.IO.Directory.GetFiles(this.currentDirectory,"*").Length;
			setSearchPattern();
			string[] files = System.IO.Directory.GetFiles(this.currentDirectory,searchPattern);
			DataTable dt = new DataTable("files");
			dt.Columns.Add("filename",typeof(string));
			dt.Columns.Add("size",typeof(long));
			dt.Columns.Add("dateModified",typeof(System.DateTime));

			int startIndex;
			int endLength;

			if (currentPage == -1) 
			{
				startIndex = 0;
				endLength = files.Length;
			}
			else 
			{
				startIndex = currentPage * pageSize;
				endLength = ((startIndex + pageSize) > files.Length) ? startIndex + (files.Length - startIndex) : startIndex + pageSize;
			}
			createPaginator(files.Length);

			for(int i=startIndex; i<endLength; i++) 
			{
				System.IO.FileInfo fi = new System.IO.FileInfo(files[i]);
				if (fi.Name.ToLower() == "thumbs.db") continue;
				dt.Rows.Add(new object[] { fi.Name,fi.Length,fi.LastWriteTime });
			}
			stats.Text = "showing " + startIndex + " to " + (endLength) + " of " + files.Length + " (" + totalFileCount + " total) files";
			filesRepeater.DataSource = dt;
			filesRepeater.DataBind();
		}

		private void createPaginator(int total) 
		{
			paginationList.Items.Clear();
			for (int j=0;j<=total/pageSize;j++) 
			{
				ListItem li = new ListItem((j+1).ToString(),j.ToString());
				if (j==currentPage) li.Selected = true;
				paginationList.Items.Add(li);
			}
			paginationList.Items.Insert(0,new ListItem("all","-1"));
		}

		private void filesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.FindControl("previewHolder") != null)
			{
				DataRowView drv = (DataRowView)e.Item.DataItem;
				string fn = drv["filename"].ToString();
				string size = getKiloBytes((long)drv["size"]);
				string date = drv["dateModified"].ToString();
				string ex = System.IO.Path.GetExtension(fn).ToLower();
				PlaceHolder p = (PlaceHolder)e.Item.FindControl("previewHolder");
				HtmlImage i = new HtmlImage();
				string alt = "filename: {0} \nsize: {1} bytes \nuploaded on: {2}";
				i.Alt = String.Format(alt,fn,size,date);

				switch (ex)
				{
					case ".jpeg":
					case ".jpg":
					case ".gif":
						string f = (this.currentDirectory == this.rootFileDirectory) ? fn : this.currentDirectory.Replace(this.rootFileDirectory,"").Replace("\\","/") + fn;
						i.Src = "~/image.ashx?file=" + f + "&w=60&h=60";
						//i.Src = "/sitefoundry/files/" + f;
						i.Border = 0;
						i.Height = 60;
						i.Width = 60;
						p.Controls.Add(i);
						break;

					case ".zip":
						p.Controls.Add(new LiteralControl("ZIP"));
						break;

					case ".pdf":
						i.Src = "~/images/icon-pdf.gif";
						i.Border = 0;
						p.Controls.Add(i);
						break;

					case ".doc":
						i.Src = "~/images/icon-doc.gif";
						i.Border = 0;
						p.Controls.Add(i);
						break;
				}

			}

		}
		public string shortenName(string input) 
		{
			if (input.Length > 16)
				return input.Substring(0,16) + "...";
			else
				return input;
		}
		public string getKiloBytes(long filesize) 
		{
			//float kb = filesize/1024;
			return filesize.ToString("N0");
		}

		public string getRelativeUrl(string file)
		{
			return relativeUrl + "/" + file;
		}

		public string getAbsoluteUrl(string file)
		{
			return absoluteUrl + "/" + file;
		}

		private string getCurrentDirectory() 
		{
			string c = this.rootFileDirectory + directorySelect.SelectedValue.Replace("/","\\") + "\\";
			this.currentDirectory = c;
			return c;
		}
		protected void upload_click(object sender, System.EventArgs e)
		{
			for (int i=0; i<Request.Files.Count; i++)
			{
				if (Request.Files[i].ContentLength > 0)
				{
					string newFile = rootFileDirectory + "\\" + System.IO.Path.GetFileName(Request.Files[i].FileName);
					Request.Files[i].SaveAs(newFile);
					msg.Text += System.IO.Path.GetFileName(Request.Files[i].FileName) + " uploaded successfully<BR>";
				}
			}
			getCurrentDirectory();
			getFiles();
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

		private void directorySelect_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.currentDirectory = getCurrentDirectory();
			getFiles();
		}

		private void newDirectoryButton_Click(object sender, EventArgs e)
		{
			string d = getCurrentDirectory() + newDirectoryName.Text;
			newDirectoryName.Text = "";
			DuryTools.IO.CreateDirectory(d);
			this.currentDirectory = d;
			fillDirectorySelects();			
			getFiles();
			newDirectoryMsg.Text = "directory created";
		}

		private void moveFilesButton_Click(object sender, EventArgs e)
		{
			if (Request.Form["files"] != null) 
			{
				string sourceDir = getCurrentDirectory();
				string targetDir = this.rootFileDirectory + moveFilesDirectorySelect.SelectedValue.Replace("/","\\") + "\\";
				string[] files = Request.Form["files"].Split(',');
				foreach(string f in files) 
				{
					System.IO.File.Move(sourceDir + f,targetDir + f);
				}
				moveFilesMsg.Text = files.Length + " files moved";
				getFiles();
			}
		}

		private void deleteFilesButton_Click(object sender, EventArgs e)
		{
			if (Request.Form["files"] != null) 
			{
				string sourceDir = getCurrentDirectory();
				string[] files = Request.Form["files"].Split(',');
				foreach(string f in files) 
				{
					System.IO.File.Delete(sourceDir + f);
				}
				deleteMsg.Text = files.Length + " files deleted";
				getFiles();
			}


		}

		private void deleteDirectoryButton_Click(object sender, EventArgs e)
		{
			System.IO.Directory.Delete(getCurrentDirectory(),true);
			this.currentDirectory = this.rootFileDirectory;
			this.fillDirectorySelects();
			getFiles();
		}

		private void paginationLink_Command(object sender, CommandEventArgs e)
		{
			Response.Write("command: " + e.CommandArgument + "<br/>");
			this.currentPage = int.Parse(e.CommandArgument.ToString());
			getFiles();
		}

		private void paginationList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.currentPage = int.Parse(paginationList.SelectedValue);
			getFiles();
		}
	}
}

