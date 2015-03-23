using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Principal;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;

using DuryTools.Data;
using Dury.SiteFoundry;
using Dury.SiteFoundry.UrlRewrite;
using Dury.SiteFoundry.Security;
    
namespace Dury.SiteFoundry.Security 
{
   
	/// <summary>
	/// Initializes our Request environment
	/// - reads cookie 
	/// - verifies security permissions for the current Node.
	/// - creates user object in Context
	/// </summary>
	public class RequestModule : IHttpModule 
	{
		private RewriteConfigSettings cs;
		private RequestConfigSettings rqcs;

		/// In the Init method register for HttpApplication events by adding your handlers
		public void Init(System.Web.HttpApplication application) 
		{
			application.BeginRequest += new EventHandler(this.Application_BeginRequest);
			//application.Error += new EventHandler(this.Application_Error);

			cs = RewriteConfigSettings.GetSettings();
			rqcs = RequestConfigSettings.GetSettings();
		}
        
		private void Application_BeginRequest(object sender, System.EventArgs e) 
		{
			HttpApplication app = ((HttpApplication)(sender));
			HttpContext context = app.Context;
			string r = context.Request.RawUrl;

			// get the user
			System.Web.HttpContext.Current.User = SFGlobal.FetchUser();

			if (!cs.IsExcluded(r)) // page isn't excluded
			{

				if (r.IndexOf(rqcs.AdminDirectory) > 0) // is in the 'admin' directory
				{
					bool b = false;
					foreach(string role in SFGlobal.CurrentUser.Roles)
					{
						if (SFGlobal.CurrentUser.IsRoleCMS(role,NodeFactory.RootNode)) b = true;
					}

					if (!b) SFGlobal.RedirectToLogin();

					/*
					if (!SFGlobal.CurrentUser.IsUserCMS()) 
					{
						SFGlobal.RedirectToLogin();
					}
					*/					
				} 
				else // process normally
				{

					if (context.Items["currentNode"] == null) 
					{

                        
						throw new Exception("currentNode is null and page isn't excluded from url processing:");
						//context.Response.End();
					}

					if (!SFGlobal.CheckUserNodePermission(Permission.View)) // check to see if the user has view permissions
					{
						//context.Response.Write("process");
						//context.Response.End();
						SFGlobal.RedirectToLogin();                        
					}
				}
			}
		}
        
		private void Application_Error(object sender, System.EventArgs e) 
		{
			HttpApplication app = ((HttpApplication)(sender));
			HttpContext context = app.Context;
		}

	
/*


		private void applySecurity()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string r = context.Request.RawUrl;
			UserPrincipal up = (UserPrincipal)context.User;
			if (r.IndexOf(rqcs.AdminDirectory) < 0) 
			{
				Node currentNode = (Node)context.Items["currentNode"];
				if (!up.CheckRolePermission(currentNode,Permission.View))
				{
					//context.Response.Write("user has no permission for this public node");
					SFGlobal.redirectToLogin();
				} 
				else 
				{
					//context.Response.Write("user has permission");
				}
			} 
			else 
			{
				if (up.IsUserCMS()) 
				{
					//context.Response.Write("user is cms");
					return;
				}
				else
				{
					//context.Response.Write("user is not authenticated");
					SFGlobal.redirectToLogin();
				}
			}
		}


		private void readUserCookie()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
			string r = context.Request.RawUrl;
			

			// add default permissions for unauthenticated (ie. non-cookied) user
			if(authCookie == null) 
			{
				// if request isn't an excepted page, then add a virtual user (public)
				if (!cs.IsExcluded(r))
				{
					ArrayList al = new ArrayList();
					al.Add("Public");
					context.User = SFGlobal.CreateUserPrincipal(1,"PublicUser",al);
					return;
				}
				else
				{
					//context.Response.Write("page is excepted");
					//SFGlobal.redirectToLogin();
					return;
				}
			}
		
			// now try to load proper permissions
			FormsAuthenticationTicket authTicket = null;
			try
			{
				authTicket = FormsAuthentication.Decrypt(authCookie.Value);
			}
			catch(Exception ex)
			{
				throw new DuryTools.ErrorHandler(ex.Message,ex);
			}

			if (null == authTicket)	SFGlobal.redirectToLogin(); 
			// array of items from cookie has the userID as the first element (so we remove it).  all other elements are user roles
			ArrayList groups = new ArrayList(authTicket.UserData.Split(new char[]{'|'}));
			int userID = int.Parse(groups[0].ToString());
			groups.RemoveAt(0);
			context.User = SFGlobal.CreateUserPrincipal(userID,authTicket.Name, groups);

		}

*/
        
		public void Dispose() 
		{
		}
	}
}
