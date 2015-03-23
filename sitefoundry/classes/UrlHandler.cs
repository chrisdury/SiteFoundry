using System;
using System.Web;
using System.Web.SessionState;


namespace HolmesAndLee.SiteFoundry 
{
	public class UrlHandler : IHttpHandler, IRequiresSessionState 
	{
        
		public bool IsReusable 
		{
			get 
			{
				// To enable pooling, return true here.
				// This keeps the handler in memory.
				return true;
			}
		}
        
		public void ProcessRequest(System.Web.HttpContext context) 
		{
			// do something?
			context.Response.Write("hello from the handler");

		}
	}
}
