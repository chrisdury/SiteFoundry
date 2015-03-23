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

using DuryTools.Data;
using Dury.SiteFoundry;

namespace SiteFoundry
{
	/// <summary>
	/// Summary description for InsertUser.
	/// </summary>
	public class InsertUser : System.Web.UI.Page
	{

		protected TextBox username;
		protected TextBox password;
		protected Button save;

		private DAL dal = SFGlobal.DAL;


		private void Page_Load(object sender, System.EventArgs e)
		{
			save.Click += new EventHandler(save_Click);




			
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

		private void save_Click(object sender, EventArgs e)
		{
			string pw = Dury.SiteFoundry.Security.Cryptography.AsymmetricEncryption.ComputeHash(password.Text,SFGlobal.EncryptionMethod,SFGlobal.EncryptionSalt);
			dal.execNonQuery("INSERT INTO SecurityUsers (Username,Password) VALUES ('" + username.Text + "','" + pw + "')");
            
			DataRow dr = dal.execDataRow("SELECT id FROM SecurityUsers WHERE username='" + username.Text + "'");
			int userID = (int)dr["id"];
			dal.execNonQuery("INSERT INTO SecurityUserRoles (userID, roleID) VALUES (" + userID.ToString() + ",1)");

			username.Text = "";
			password.Text = "";
		}
	}
}
