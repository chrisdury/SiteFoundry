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
using Dury.SiteFoundry.Security;
using Dury.SiteFoundry.Utils;

namespace Dury.SiteFoundry
{
	/// <summary>
	/// Main static class.
	/// All external references in SiteFoundry should pass through here.
	/// Contains references to Data Layer/Object Manager, web.config items and other global information
	/// 
	/// also contains main functions for user authenticiation/authorization, url rewriting, startup and teardown
	/// </summary>
	public class SFGlobal
	{
		private static int startNodeID = 1;

		public static int UserCount;
		public static string NodeTemplateLocation;
		public static string ResourceFileLocation;
		public static string BaseDirectory;
		public static string VirtualDirectory;
		public static string VirutalFileExtention;
		public static string SiteRoot;
		public static string DefaultLanguage;
		public static bool ShowLangInUrl;
		public static string EncryptionKey;
		public static string EncryptionMethod;
		public static byte[] EncryptionSalt;
		public static string PublicSuffix = "_public";
		public static string ContentMenuDisplay;

		public static string AdminstratorRoleName;
		public static string PublisherRoleName;
		public static string EditorRoleName;
		public static string ContributorRoleName;		

		public static Wilson.ORMapper.ObjectSpace ObjectManager;
		public static Wilson.ORMapper.ObjectSpace ObjectManagerPublic;
		public static DAL DAL;

		private static System.IO.FileSystemWatcher fsw;

		private static string appDirectory;

		public SFGlobal()
		{
            
		}


		/// <summary>
		/// start up.  get globals, load object mappings, init DAL, etc...
		/// </summary>
		public static void Start()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;

			// set globals
			NodeTemplateLocation = System.Configuration.ConfigurationSettings.AppSettings["nodeTemplateDirectory"];
			ResourceFileLocation = System.Configuration.ConfigurationSettings.AppSettings["resourceDirectory"];
			VirtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"];
			SiteRoot = "/" + System.Configuration.ConfigurationSettings.AppSettings["virtualDirName"];
			VirutalFileExtention = System.Configuration.ConfigurationSettings.AppSettings["virtualFileExtension"];
			DefaultLanguage = System.Configuration.ConfigurationSettings.AppSettings["defaultLanguage"];
			ShowLangInUrl = bool.Parse(System.Configuration.ConfigurationSettings.AppSettings["showLangInUrl"]);
			BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
			EncryptionKey = System.Configuration.ConfigurationSettings.AppSettings["encryptionKey"];
			EncryptionMethod = System.Configuration.ConfigurationSettings.AppSettings["encryptionMethod"];
			EncryptionSalt = Convert.FromBase64CharArray(EncryptionKey.ToCharArray(),0,EncryptionKey.Length);
			AdminstratorRoleName = System.Configuration.ConfigurationSettings.AppSettings["AdminstratorRoleName"];
			PublisherRoleName = System.Configuration.ConfigurationSettings.AppSettings["PublisherRoleName"];
			EditorRoleName = System.Configuration.ConfigurationSettings.AppSettings["EditorRoleName"];
			ContributorRoleName = System.Configuration.ConfigurationSettings.AppSettings["ContributorRoleName"];
			ContentMenuDisplay = System.Configuration.ConfigurationSettings.AppSettings["contentMenuDisplay"];

			appDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace("/bin","");

			//context.Application.Add("currentUsers",0);
			UserCount = 0;

			// start Object-Relational Mappings
			string connection = System.Configuration.ConfigurationSettings.AppSettings["connectionString"];
			string mappings = appDirectory + System.Configuration.ConfigurationSettings.AppSettings["objectMappingsFile"];
			ObjectManager = new Wilson.ORMapper.ObjectSpace(mappings,connection,Wilson.ORMapper.Provider.MsSql);
			mappings = appDirectory + System.Configuration.ConfigurationSettings.AppSettings["objectMappingsPublicFile"];
			ObjectManagerPublic = new Wilson.ORMapper.ObjectSpace(mappings,connection,Wilson.ORMapper.Provider.MsSql);

			// SQL Data Abstraction Layer
			DAL = new DAL();

			// read templates into memory
			TemplateLoader.Load(System.Configuration.ConfigurationSettings.AppSettings["nodeTemplateDefinitions"]);

			// set FSW on config files
			fsw = new System.IO.FileSystemWatcher(appDirectory,"*.config");
			fsw.EnableRaisingEvents = true;
			fsw.NotifyFilter = System.IO.NotifyFilters.LastWrite;
			fsw.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);


			UpdateNodes();
		}

		private static void fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			SFGlobal.Start();
		}


		public static UserPrincipal GetCurrentUser() 
		{
			if (System.Web.HttpContext.Current.User != null) 
				return (UserPrincipal)System.Web.HttpContext.Current.User;
			else
				return null;
		}

		public static CultureInfo CurrentCulture
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentCulture;
			}
		}

/*

		public static void ProcessRequest()
		{
			//loadUserFromCookie();
			//applySecurity();
		}




		/// <summary>
		/// Handles processing of URL replacement and mapping to pages.aspx (page holder)
		/// </summary>
		public static void ProcessRequest() 
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string r = context.Request.RawUrl;
			if (r.IndexOf("login.aspx") < 0 && r.IndexOf("logout.aspx") < 0 && r.IndexOf("genimage") < 0 && r.IndexOf("asmx") < 0 && r.IndexOf("error.aspx") < 0 && r.IndexOf("insertuser.aspx") < 0) 
			{
				if (r.IndexOf("admin") < 0) Dury.SiteFoundry.UrlRewriter.RewritePaths();
				loadUserFromCookie();
				applySecurity();
			} 
			else 
			{
				context.Items.Add("currentNode",null);
			}
			
		}
		

		public static void SetLangSession()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			string r = context.Request.RawUrl;
			if (r.IndexOf("?") > 0 && r.IndexOf("=") > 0) 
			{
				string query = r.Substring(r.IndexOf("?"));
				DuryTools.Utils.CustomQueryString cqs = DuryTools.Utils.QueryStringUtils.QueryStringBuilder(query);
				if (cqs["lang"] != null)
				{
					context.Session["lang"] = SFGlobal.SqlCleanString(cqs["lang"]);
				}
			}
			if (context.Session["lang"] == null)
			{
				context.Session["lang"] = SFGlobal.DefaultLanguage;
			}
			CultureInfo culture;
			switch (context.Session["lang"].ToString())
			{
				case "en":
					culture = CultureInfo.CreateSpecificCulture("en-CA");
					break;
				case "fr":
					culture = CultureInfo.CreateSpecificCulture("fr-CA");
					break;
				default:
					culture = CultureInfo.CreateSpecificCulture("en-CA");
					break;
			}
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;
		}
*/


		public static void IncrementUserCount() 
		{
			UserCount++;
		}

		public static void DecrementUserCount()
		{
			UserCount--;
		}



		/// <summary>
		/// updates nodes in memory from datastore
		/// </summary>
		public static void UpdateNodes()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			context.Application.Lock();

			// check to see if site has been published, then populate application object with public nodes
			string sql = "SELECT id FROM sysobjects WHERE (id = OBJECT_ID(N'[dbo].[Nodes_public]')) AND (OBJECTPROPERTY(id, N'IsUserTable') = 1)";
			object o = SFGlobal.DAL.execScalar(sql);
			if (o != null)
			{
				if (context.Application["nodeRootPublic"] != null)
					context.Application.Add("nodeRootPublic",NodeFactory.GetAll(ObjectManagerPublic, startNodeID));
				else
					context.Application["nodeRootPublic"] = NodeFactory.GetAll(ObjectManagerPublic, startNodeID);
			}

			// read nodes into application object for CMS only
			if (context.Application["nodeRoot"] != null)
				context.Application.Add("nodeRoot",NodeFactory.GetAll(ObjectManager, startNodeID));
			else
				context.Application["nodeRoot"] = NodeFactory.GetAll(ObjectManager, startNodeID);

			context.Application.UnLock();
		}

		public static string GetUserLanguage()
		{
			return System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
		}



		/// <summary>
		/// gets a UserPrincipal object.
		/// if a forms-auth cookie exists, then it loads that.  if no cookie is present, then the user is assigned public access
		/// </summary>
		/// <returns></returns>
		public static UserPrincipal FetchUser()
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
			int userID;
			string username;
			ArrayList userGroups;
			
			// add default permissions for unauthenticated (ie. non-cookied 'public') user
			if(authCookie == null) 
			{
				userGroups = new ArrayList();
				userGroups.Add("Public");
				userID = 1;
				username = "PublicUser";
			} 
			else 
			{
				FormsAuthenticationTicket authTicket = null;
				try
				{
					authTicket = FormsAuthentication.Decrypt(authCookie.Value);
				}
				catch(Exception ex)
				{
					throw new DuryTools.ErrorHandler(ex.Message,ex);
				}
				if (null == authTicket)	SFGlobal.RedirectToLogin(); 
				// array of items from cookie has the userID as the first element (so we remove it).  all other elements are user roles
				userGroups = new ArrayList(authTicket.UserData.Split(new char[]{'|'}));
				userID = int.Parse(userGroups[0].ToString());
				username = authTicket.Name;
				userGroups.RemoveAt(0);
			}
			return SFGlobal.CreateUserPrincipal(userID,username,userGroups);
		}


		/// <summary>
		/// See if the current user is a CMS user
		/// </summary>
		/// <returns></returns>
		public static bool IsUserCMS()
		{
			return CurrentUser.IsUserCMS();
		}

		public static UserPrincipal CurrentUser
		{
			get 
			{
				System.Web.HttpContext context = System.Web.HttpContext.Current;
				return (UserPrincipal)context.User;		
			}
		}

		public static bool CheckUserNodePermission(Permission p)
		{
			Node currentNode = (Node)System.Web.HttpContext.Current.Items["currentNode"];
			return SFGlobal.CurrentUser.CheckRolePermission(currentNode,p);
		}

		public static UserPrincipal CreateUserPrincipal(int id, string username, ArrayList groups)
		{
			UserIdentity ui = new UserIdentity(id,username);
			UserPrincipal principal = new UserPrincipal(ui, groups); 
			return principal;  
		}

		public static UserPrincipal CreateUserPrincipal(string encryptedString)
		{
			Dury.SiteFoundry.Security.Cryptography.SymmetricEncryption se = new Dury.SiteFoundry.Security.Cryptography.SymmetricEncryption();
			try
			{
				ArrayList groups = new ArrayList(se.DecryptString(encryptedString,SFGlobal.EncryptionKey).Split('|'));
				int userID = int.Parse((string)groups[0]);
				string username = (string)groups[1];
				groups.RemoveRange(0,2);
				return CreateUserPrincipal(userID,username,groups);
			}
			catch(Exception e)
			{
				throw new DuryTools.ErrorHandler("user decrypt failed",e);
			}
		}



		public static void RedirectToLogin()
		{
			string loginPage = System.Configuration.ConfigurationSettings.AppSettings["loginPage"];
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			context.Response.Redirect("~/" + loginPage + "?RequestedUrl=" + context.Server.UrlEncode(context.Request.RawUrl));
		}

		/// <summary>
		/// Loads article template references into memory from XML file.
		/// </summary>
		/// <returns></returns>
		public static System.Collections.Specialized.ListDictionary LoadArticleTemplates()
		{
			string appKeyName = "sf_articleTemplates";
			System.Web.HttpContext context = System.Web.HttpContext.Current;

			if (context.Application[appKeyName] != null)
			{
				return (System.Collections.Specialized.ListDictionary)context.Application[appKeyName];
			}
			else
			{
				System.Collections.Specialized.ListDictionary at = new System.Collections.Specialized.ListDictionary();
				System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
				try
				{
					xd.Load(System.AppDomain.CurrentDomain.BaseDirectory + SFGlobal.NodeTemplateLocation + "\\" + System.Configuration.ConfigurationSettings.AppSettings["articleTemplateDefinitions"]);
					foreach (XmlNode xn in xd["templates"])
					{
						at.Add(int.Parse(xn.Attributes["id"].Value),new ArticleTemplateInfo(xn.Attributes["id"].Value,xn.Attributes["name"].Value,xn.Attributes["src"].Value));
					}
					context.Application.Add(appKeyName,at);
				}
				catch(Exception e)
				{
					throw new DuryTools.ErrorHandler("can't load articleTemplate XML...",e);
				}
				return at;
			}
		}
		public static ArticleTemplateInfo GetArticleTemplate(int id)
		{
			System.Collections.Specialized.ListDictionary at = LoadArticleTemplates();
			if (at[id] == null) 
			{
				throw new DuryTools.ErrorHandler("error loading template... check id {" + id.ToString() + "} or make sure articles are published");
			}
			return (ArticleTemplateInfo)at[id];
		}

		public static string SqlCleanString(string input)
		{
			//string[] restrictedWords = new string[] { "SELECT" , "INSERT", "DELETE", "DROP", "--", ";" };
			string[] restrictedWords = new string[] { "---"};
			foreach(string s in restrictedWords)
			{
				if (input.ToLower().IndexOf(s.ToLower()) > -1)
					throw new DuryTools.ErrorHandler("reserved word(s) or characters used in input text.");
			}
			return input.Replace("'","''");
		}

		public static void DeleteNode(Node n)
		{
			NodeFactory.DeleteNode(n);
		}


		public static Node GetNode(string nodePath) 
		{
			System.Web.HttpContext context = System.Web.HttpContext.Current;
			if (context.Application["nodeRoot"] != null)
			{
				try 
				{
					Node n;
					if (SFGlobal.IsUserCMS()) 
					{
						n = (Node)context.Application["nodeRoot"];
					} 
					else 
					{
						n = (Node)context.Application["nodeRootPublic"];
					}
					System.Collections.ArrayList s = new System.Collections.ArrayList(nodePath.Split('*'));
					return n.Find(s);
				}
				catch (Exception e)
				{
					throw new Exception("user isn't loaded.",e);
					//return null;
				}
			}
			else
			{
				return null;
			}
		}

		public static bool IsNumeric(string s)
		{
			try 
			{
				Int32.Parse(s);
			}
			catch 
			{
				return false;
			}
			return true;
		}

		public static string SerializeArray(ICollection array) 
		{
			string s = String.Empty;
			int i = 0;
			foreach(object o in array) 
			{
				s += i.ToString() + ": " + o.ToString() + "<br/>";
				i++;
			}
			return s;
		}

		public static string TruncateText(string input, int maxLength) 
		{
			if (input.Length > maxLength)
				return input.Substring(0,maxLength) + "...";
			return input;
		}

		public static void AlertUser(string text)
		{
			System.Web.HttpContext.Current.Session["alertMsg"] = text;
		}
		public static object GetUserAlert()
		{
			return System.Web.HttpContext.Current.Session["alertMsg"];
		}
		public static void ClearUserAlert()
		{
			System.Web.HttpContext.Current.Session.Remove("alertMsg");
		}
			

	}
}
