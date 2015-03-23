using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Dury.SiteFoundry;
using DuryTools.Data;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class Login : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox username;
		protected System.Web.UI.WebControls.TextBox password;
		protected System.Web.UI.WebControls.TextBox domain;
		protected System.Web.UI.WebControls.Label msg;
		protected System.Web.UI.WebControls.Button submit;

		private void Page_Load(object sender, System.EventArgs e)
		{
			submit.Click += new EventHandler(submit_Click);
            msg.Text = "";
		}


		public void submit_Click(object sender, System.EventArgs e)
		{
			string u = SFGlobal.SqlCleanString(username.Text);
			string p = Dury.SiteFoundry.Security.Cryptography.AsymmetricEncryption.ComputeHash(password.Text,SFGlobal.EncryptionMethod,SFGlobal.EncryptionSalt);
			//string p = SFGlobal.SqlCleanString(password.Text);

			string userGroup = this.getUserGroup(u,p);
			if (userGroup != null)
			{
				FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,username.Text,DateTime.Now,DateTime.Now.AddMinutes(30),false,userGroup);
				string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
				HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,encryptedTicket);
				Response.Cookies.Add(authCookie); 
				//if (Request.QueryString["RequestedUrl"] == null || Request.QueryString["RequestedUrl"] == "")
                    Response.Redirect("admin/default.aspx");
				//else
				//	Response.Redirect(Request.QueryString["RequestedUrl"]);
				//msg.Text = userGroup;
			}
			else
			{
				msg.Text = "login invalid";
			}

		}

		private string getUserGroup(string username, string password)
		{
			string sql = "SELECT sur.userID, r.name FROM SecurityUserRoles sur INNER JOIN SecurityRoles r ON sur.roleID = r.id WHERE (sur.userID = (SELECT id FROM SecurityUsers WHERE username = '" + username + "' AND password = '" + password + "' AND disabled <> 1))";
			DataSet ds = SFGlobal.DAL.execDataSet(sql);
            
			string s = "";
			if (ds.Tables[0].Rows.Count > 0)
			{
				s += ds.Tables[0].Rows[0]["userID"].ToString();
				for (int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					s += "|" + ds.Tables[0].Rows[i]["name"];
					//if (i<ds.Tables[0].Rows.Count-1) s+="|";
				}
				DataContainer dc = new DataContainer("SecurityUsers");
				DataRow dr = dc.GetRowByKey("id",ds.Tables[0].Rows[0]["userID"]);
				dr["LastLogin"] = DateTime.Now;
				dc.UpdateRow(dr);
				return s;
			} 
			else
			{
				return null;
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
