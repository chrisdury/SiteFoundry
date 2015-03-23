using System;

namespace Dury.SiteFoundry.UI
{
	/// <summary>
	/// Extended System.Web.UI.Page for our main site template
	/// </summary>
	public class BasePage : System.Web.UI.Page
	{
		private string mainTitle;
		public string MainTitle 
		{
			get { return mainTitle; }
			set { mainTitle = value; }
		}

		private string pageTitle;
		public string PageTitle 
		{
			get { return pageTitle; }
			set { pageTitle = value; }
		}

		public string MetaKeywords = String.Empty;
		public string MetaDescription = String.Empty;
		public DateTime PageTime;
		public bool ShowTemplate = true;
		private string headerFile;
		private string footerFile;

		private DuryTools.Utils.CustomQueryString customQueryString;
		public DuryTools.Utils.CustomQueryString CustomQueryString
		{
			get { return customQueryString; }
		}

		protected override void OnInit(System.EventArgs e) 
		{
			PageTime = System.DateTime.Now;
			headerFile = (System.Configuration.ConfigurationSettings.AppSettings["siteTemplateHeader"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["siteTemplateHeader"] : "siteTemplates/header.ascx";
			footerFile = (System.Configuration.ConfigurationSettings.AppSettings["siteTemplateFooter"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["siteTemplateFooter"] : "siteTemplates/footer.ascx";
			mainTitle = (System.Configuration.ConfigurationSettings.AppSettings["siteMainTitle"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["siteMainTitle"] : "MainTitle";
			//SFGlobal.SetLangSession();
			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Node n = (Node)Context.Items["currentNode"];
			if (ShowTemplate) Controls.AddAt(0, LoadControl(n.Template.HeaderFile));
			base.OnPreRender (e);
			if (ShowTemplate) Controls.Add(LoadControl(n.Template.FooterFile));
		}

		protected override void OnLoad(EventArgs e)
		{
			customQueryString = DuryTools.Utils.QueryStringUtils.QueryStringBuilder(Context.Request.RawUrl);
			base.OnLoad (e);
		}




	}






	public class BaseControl : System.Web.UI.UserControl
	{
		public new BasePage Page 
		{
			get { return (BasePage)base.Page; }
		}

	}
}
