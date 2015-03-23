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
using AjaxPro.JSON;


namespace Dury.SiteFoundry.Admin
{
	/// <summary>
	/// Summary description for cms.
	/// </summary>
	
	public class cms : DuryTools.UI.BasePage
	{
		protected System.Web.UI.WebControls.PlaceHolder nodeTemplateHolder;
		protected PlaceHolder menuHolder;
		//protected HtmlGenericControl nodeTemplateHolder;
		public string UserEncryptedString = "";

		private void Page_Load(object sender, System.EventArgs e)
		{
			AjaxPro.Utility.RegisterTypeForAjax(typeof(cms));
			this.PageTitle = "Content Management : ";
			//nodeTemplateHolder.Attributes.Add("src","nodeDisplay.aspx");
			loadMenu();
			loadNodeTemplate();
		}
		private void loadMenu() 
		{
			Control c;
			switch(SFGlobal.ContentMenuDisplay.ToLower())
			{
				case "flash":
					c = Page.LoadControl("modules/cmsMenuDHTML.ascx");
					break;
				case "dhtml":
					c = Page.LoadControl("modules/cmsMenuDHTML.ascx");
					break;
				case "ajax":
				default:
					c = Page.LoadControl("modules/cmsMenuAJAX.ascx");
					break;
			}
			menuHolder.Controls.Add(c);
		}


		private void loadNodeTemplate() 
		{
			if (Request.QueryString["nodeID"] != null) 
			{
				int nodeID = int.Parse(Request.QueryString["nodeID"]);
				Node currentNode = NodeFactory.RootNode.Find(nodeID);
				nodeTemplateHolder.Controls.Add(TemplateLoader.GetAdminTemplate(currentNode));
			}
			else
			{
				nodeTemplateHolder.Controls.Add(new LiteralControl("Please select a page from the menu to the left"));
			}

		}


		// try to move these into cmsMenuAJAX.ascx.cs
		// can't right now due to library incompatibility putting into .ascx files

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GetNode(int nodeID)
		{
			NodeCollection nc = (NodeCollection)Session["nodeRoot"];
			Node s = nc.Find(nodeID);
			AjaxPro.JSON.JSONParser json = new JSONParser();
			return json.GetJSONString(s.GetSimple(0));	

		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GetNodeChildren(int nodeID)
		{
			NodeCollection nc = (NodeCollection)Session["nodeRoot"];
			Node s = nc.Find(nodeID);
			AjaxPro.JSON.JSONParser json = new JSONParser();
			return json.GetJSONString(s.GetSimpleChildren());
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
