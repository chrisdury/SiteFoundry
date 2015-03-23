using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Principal;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;

using Dury.SiteFoundry;
using Dury.SiteFoundry.Security;


namespace Dury.SiteFoundry.UrlRewrite
{
	/// <summary>
	/// Handles URL Rewriting for SiteFoundry
	/// </summary>
	public class UrlModule : IHttpModule 
	{

		private bool debug = true;
		private DateTime startTime;

        public void Init(System.Web.HttpApplication application) 
		{
			application.BeginRequest += new EventHandler(this.Application_BeginRequest);
			application.EndRequest += new EventHandler(application_EndRequest);
		}
        

		// url scheme:  http://mysite.com/en-CA/default.aspx
		//              http://mysite.com/en-CA/whoweare/default.aspx
		//              http://mysite.com/whoweare/gallery.aspx (uses default language)

		private void Application_BeginRequest(object sender, System.EventArgs e) 
		{

			HttpApplication app = ((HttpApplication)(sender));
			HttpContext context = app.Context;
			startTime = DateTime.Now;


			string r = context.Request.RawUrl;
			string virtualDir = (System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"] : "" ;
			string virtualFileExtension = (System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"] != null) ? System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"] : ".aspx";
			RewriteConfigSettings cs = RewriteConfigSettings.GetSettings();
			RequestConfigSettings rqcs = RequestConfigSettings.GetSettings();

			// apply exceptions
			if (!cs.IsExcluded(r) && r.IndexOf(rqcs.AdminDirectory) < 0)
			{

				if (r[r.Length-1] != '/' && virtualFileExtension == "" && r.IndexOf("?") == -1) context.Response.Redirect(r + "/",true);

				// lets cut the incoming url into the parts we're interested in
				string u = r.Substring(1);
				//context.Response.Write("1u=" + u + "<BR>");
				//context.Response.Write("1r=" + r + "<BR>");
				if (u.IndexOf("?") > 0)	u = u.Substring(0,u.IndexOf("?"));
				if (virtualDir.Length > 0) u = u.Replace(virtualDir,"");
				if (u.IndexOf(".aspx") > 0 && virtualDir.Length == 0) u.Replace(".aspx","");
				if (u != "" && virtualFileExtension != "" && virtualFileExtension.Length > 0) 
				{
					try {
						u = u.Substring(0,u.IndexOf(virtualFileExtension));//u.Replace(virtualFileExtension,"");
					}
					catch(Exception ex) 
					{
						throw new Exception("substring 3 - " + u +", "+r,ex);
					}                                        
				}
				// split it!
				System.Collections.Specialized.StringCollection url_parts = new System.Collections.Specialized.StringCollection();
				url_parts.AddRange(u.Split('/'));

				// find language identitfier -- if not found then set default
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("/[a-z]{2}-[a-zA-Z]{2}/|/[a-z]{2}/");
				if (regex.IsMatch(r) && regex.Match(r).Index == 0)
				{
					setCulture(url_parts[0]);
					url_parts.RemoveAt(0);
				} 
				else
				{
					setCulture(SFGlobal.DefaultLanguage);
				}


				// create pseudo-querystring
				string output = "";
				foreach(string s in url_parts)
				{
					if (s.Length > 0 && !SFGlobal.IsNumeric(s))
						output += s + "*";
				}

				output = (output.Length > 1) ? output.Substring(0,output.Length-1) : "default";

				// url rewriting

				// this needs to load the user before the currentNode is returned

				context.User = SFGlobal.FetchUser();
				context.Items.Add("currentNode",SFGlobal.GetNode(output));

				if (context.Items["currentNode"] == null || SFGlobal.GetNode(output) == null) 
				{
					context.Response.Write("currentNode is null:" + output);
					context.Response.End();
				}

				context.RewritePath(cs.PageHandler,cs.PageHandler,output);
			}
			else
			{
				//context.Response.Write("no rewrite");
			}
		}

		private void application_EndRequest(object sender, EventArgs e)
		{
			HttpApplication app = ((HttpApplication)(sender));
			HttpContext context = app.Context;
			string r = context.Request.RawUrl;
			RewriteConfigSettings cs = RewriteConfigSettings.GetSettings();
			RequestConfigSettings rqcs = RequestConfigSettings.GetSettings();
			Node currentNode = (Node)context.Items["currentNode"];

			if (debug && !cs.IsExcluded(r) && r.IndexOf(rqcs.AdminDirectory) < 0 && currentNode != null)
			{
				TimeSpan ts = System.DateTime.Now.Subtract(startTime);
                
				string n = System.Environment.NewLine;
				string s = n + n + "<!--" + n;
				s += "current node: " + currentNode.Filename + n;
				s += "localized as: " + System.Threading.Thread.CurrentThread.CurrentCulture.Name + n;
				s += "generated in: " + ts.TotalMilliseconds.ToString() + " ms" + n;
				s += "-->"+n;	
				context.Response.Write(s);
			}
		}






		/// <summary>
		/// sets the execution thread to the culture specified in the url
		/// </summary>
		/// <param name="lang">2 or 5 character (prefereably 5, ie. xx-XX) format ISO localization string</param>
		private void setCulture(string lang)
		{
			CultureInfo culture;
			switch (lang)
			{
				case "en":
					culture = CultureInfo.CreateSpecificCulture("en-CA");
					break;
				case "fr":
					culture = CultureInfo.CreateSpecificCulture("fr-CA");
					break;
				default:
					culture = CultureInfo.CreateSpecificCulture(lang);
					break;
			}
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;
		}


        
		public void Dispose() 
		{
		}


	}
}
