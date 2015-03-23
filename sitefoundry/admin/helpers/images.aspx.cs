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
using DuryTools.Data;


namespace Dury.SiteFoundry.admin.helpers
{
	/// <summary>
	/// Summary description for images.
	/// </summary>
	public class images : System.Web.UI.Page
	{
		protected TextBox img_padding_top;
		protected TextBox img_padding_left;
		protected TextBox img_padding_right;
		protected TextBox img_padding_bottom;
		protected TextBox bgcolor;
		protected DropDownList alignment;
		protected DropDownList thumbsList;
		protected TextBox thumbPercent;
		protected TextBox thumbWidth;
		protected TextBox thumbHeight;
		protected CheckBox openLargeImage;
		protected TextBox openURL;
		protected DropDownList openPage;
		protected Button uploadButton;
		protected Button insertButton;
		protected Button insertAndCloseButton;
		protected Repeater imageRepeater;
		protected HtmlInputFile fileToUpload;
		protected RequiredFieldValidator rfv_upload;

		private string[] fileTypes = new string[] { "*.jpg","*.jpeg","*.gif" };
		public string fileDirectory = SFGlobal.SiteRoot + SFGlobal.ResourceFileLocation;

		private DataTable dt_images;

		private void Page_Load(object sender, System.EventArgs e)
		{
			uploadButton.Click += new EventHandler(uploadButton_Click);
			imageRepeater.ItemDataBound += new RepeaterItemEventHandler(imageRepeater_ItemDataBound);
			dt_images = new DataTable("files");
			if (!IsPostBack) 
			{
				fillFilesRepeater();
				fillThumbsList();
				fillPageList();
			}
			
		}

		private void fillThumbsList() 
		{
			if (dt_images.Rows.Count > 0) 
			{
				for(int i=0;i<dt_images.Rows.Count; i++)
				{
					ListItem li = new ListItem();
					li.Text = dt_images.Rows[i]["filename"].ToString();
					li.Value = dt_images.Rows[i]["filename"].ToString() + "|" + dt_images.Rows[i]["width"].ToString() + "|" + dt_images.Rows[i]["height"].ToString();
					thumbsList.Items.Add(li);
				}
				thumbsList.Items.Insert(0,new ListItem("select image or size",""));
			}
		}


		private void fillFilesRepeater() 
		{
			System.Collections.ArrayList imageFiles = new ArrayList();
			foreach(string ext in fileTypes)
			{
				imageFiles.AddRange(System.IO.Directory.GetFiles(Server.MapPath(this.fileDirectory),ext));                
			}
			//DataTable dt = new DataTable("files");
			dt_images.Columns.Add("filename",typeof(string));
			dt_images.Columns.Add("size",typeof(int));
			dt_images.Columns.Add("height",typeof(int));
			dt_images.Columns.Add("width",typeof(int));
			dt_images.Columns.Add("dateModified",typeof(System.DateTime));

			for(int i=0; i<imageFiles.Count; i++) 
			{
				System.IO.FileInfo fi = new System.IO.FileInfo((string)imageFiles[i]);
				try 
				{
					System.Drawing.Bitmap b = new Bitmap((string)imageFiles[i]);
					dt_images.Rows.Add(new object[] { fi.Name,(int)fi.Length,b.Height,b.Width,fi.LastWriteTime });
					b.Dispose();
				}		
				catch(Exception ex) 
				{
					throw new Exception("problem reading: " + imageFiles[i].ToString() + " :" + ex.Message,ex);
				}
			

			}
			dt_images.DefaultView.Sort = "filename";
			imageRepeater.DataSource = dt_images.DefaultView;
			imageRepeater.DataBind();
		}


		private void fillPageList() 
		{
			Node root = (Node)Application["nodeRoot"];
			fillLinksTable(root,0);
			openPage.Items.Insert(0,"select page");
		}

		private void fillLinksTable(Node currentNode, int indent)
		{
			//linksTable.Rows.Add(new object[] { currentNode.getName(SFGlobal.DefaultLanguage), currentNode.URL, indent} );
			ListItem li = new ListItem();
			string p = "";
			for(int i=0;i<indent*2;i++) p+= ((char)160).ToString();

			string name = currentNode.getName(SFGlobal.DefaultLanguage);
			if (name.Length > 15) name = name.Substring(0,15) + "...";
			li.Text = p + name;
			li.Value = currentNode.URL;
			openPage.Items.Add(li);
			
			foreach(Node n in currentNode.children)
			{
				fillLinksTable(n,indent+1);
			}
		}


		private void imageRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.FindControl("deleteButton") != null)
			{
				ImageButton delete = (ImageButton)e.Item.FindControl("deleteButton");
				delete.Attributes.Add("onClick","return deleteWarning('this image?')");
			}
		}

		private void uploadButton_Click(object sender, EventArgs e)
		{
			if (fileToUpload.PostedFile != null) 
			{
				string filename = System.IO.Path.GetFileName(fileToUpload.PostedFile.FileName.ToLower());
				fileToUpload.PostedFile.SaveAs(Server.MapPath(this.fileDirectory) + "\\" + filename);
				fillFilesRepeater();
			}
		}

		public void deleteImage(object sender, CommandEventArgs e) 
		{
			string filename = e.CommandArgument.ToString();
			System.IO.File.Delete(Server.MapPath(this.fileDirectory) + "\\" + filename);
			fillFilesRepeater();
		} 

		public string getFileSize(int fs)
		{
			//return fs.ToString("N0");
			float f = (float)fs / (float)1024;
			return f.ToString("N2");
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
