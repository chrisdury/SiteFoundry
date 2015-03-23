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
using System.Xml;
using XStandard.Library;
using Dury.SiteFoundry;


namespace Dury.SiteFoundry.ws
{
	/// <summary>
	/// Summary description for linklibrary.
	/// </summary>
	public class linklibrary : System.Web.UI.Page
	{

		private void Page_Load(object sender, System.EventArgs e)
		{
				
			/* -----------------------------------------------------------------------------------------
				-                                        IMPORTANT
				- 1. Put XStandard.Library.dll into a sub-folder called "bin".
				- 2. The folder containing this script must be made into an "Application" in IIS.
				-
				- To customize this service, search for "ADD CUSTOM CODE HERE" below.
				----------------------------------------------------------------------------------------- */



			/* -----------------------------------------------------------------------------------------
				-                             OPTIONAL - CHANGE THESE SETTINGS
				----------------------------------------------------------------------------------------- */
			string libraryName = "Link Library";

			//Log file for errors or debug information.  Can be a template like "%Y-%m-%d.log" where %Y is 4 digit year, %m is 2 digit month and %d is 2 digit day.  This will produce a log file like "2003-12-29.log".
			string logfile = Server.MapPath(".") + @"\x%Y-%m-%d.log"; // Or hardcode the path like "C:\XStandard.log"

			//Enable the ablity to browse this library
			bool libraryBrowseEnabled = true;

			//Enabled the ability to search this library. You'll need to customize the search feature to meet your CMS needs.
			bool librarySearchEnabled = true;

			//Used to turn debuging on or off.  This is a boolean value.
			bool debug = true;

			//An authorization code used to restrict access to this Web Service. You get this code from your account on the xstandard.com Web site.
			string authorizationCode = "";
			/* -----------------------------------------------------------------------------------------
				-                             OPTIONAL - CHANGE THESE SETTINGS
				----------------------------------------------------------------------------------------- */

			/* -----------------------------------------------------------------------------------------
				- Purpose:	LinkLibrary Service
				- 
				- Input:	SOAP
				- Output:	SOAP
				-
				- Copyright (c) 2002 Belus Technology Inc.  All rights reserved.
				----------------------------------------------------------------------------------------- */
			SOAPServer soap = new SOAPServer();
			soap.LogFile = logfile;
			soap.TempFolder = Server.MapPath(".");
			soap.LibraryFolder = Server.MapPath(".");
			soap.LibraryBaseURL = SFGlobal.SiteRoot + "files";
			soap.LibraryBrowseEnabled = libraryBrowseEnabled;
			soap.LibrarySearchEnabled = librarySearchEnabled;
			soap.LibraryUploadToRootContainerEnabled = false;
			soap.LibraryUploadToSubContainerEnabled = false;
			soap.Debug = debug;

			ArrayList attributes;
			ArrayList properties;


			//Process SOAP message
			if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
			{
				//Check authorization code
				if (authorizationCode != String.Empty)
				{
					if (Request.ServerVariables["HTTP_X_LICENSE_ID"] == null)
					{
						soap.RaiseError("No authorization code set. Please contact your System Administrator.");
					}
					else
					{
						if (Request.ServerVariables["HTTP_X_LICENSE_ID"] != authorizationCode)
						{
							soap.RaiseError("Invalid authorization code. Please contact your System Administrator.");
						}
					}
				}

				//Process request SOAP message
				soap.ProcessRequest(Request);
	
				//Depending on the request, pass data to the SOAP server
				if(soap.Action == "doLibraryDescribe")
				{
					soap.LibraryName = libraryName;
		
					if (librarySearchEnabled)
					{
						/*
							** -------------------------------------------------------
							** ADD CUSTOM CODE HERE
							**
							** ADD SEARCH FILTERS
							**
							** Use method:
							** soap.AddSearchFilter(string id, string label)
							** -------------------------------------------------------
							*/
						soap.AddSearchFilter("", "(no filter)");
						soap.AddSearchFilter("user", "User");
					}
				}
				else if(soap.Action == "doLibraryBrowse")
				{
					//Get properties sent by XStandard
					string path = soap.GetProperty("path");
					string lang = soap.GetProperty("lang");

					Node currentNode;
					if (path.Length == 0)
						currentNode = NodeFactory.RootNode;
					else 
					{
						currentNode = NodeFactory.RootNode.Find(int.Parse(path));						
					}

					
					foreach(Node n in currentNode.children) 
					{
						properties = new ArrayList();
						attributes = new ArrayList();
						attributes.Add(new XStandard.Library.Attribute("href", n.URL));

						if (currentNode.children.Count > 0) 
						{
							soap.AddContainer(n.Id.ToString(),"",n.getName(SFGlobal.DefaultLanguage),n.URL,false,"","",0);
						}
						else
							soap.AddObject(n.Id.ToString(),"",n.getName(SFGlobal.DefaultLanguage),attributes.ToArray(),properties.ToArray(),"document","",0);
					}
					properties = new ArrayList();
					attributes = new ArrayList();
					attributes.Add(new XStandard.Library.Attribute("href", currentNode.URL));

					soap.AddObject(currentNode.Id.ToString(),"",currentNode.getName(SFGlobal.DefaultLanguage),attributes.ToArray(),properties.ToArray(),"document","",0);
					
				}
				else if(soap.Action == "doLibrarySearch")
				{
					//Get properties sent by XStandard
					string searchFor = soap.GetProperty("searchFor");
					string filterBy = soap.GetProperty("filterBy");
					string lang = soap.GetProperty("lang");
	
					/*
						** -------------------------------------------------------
						** ADD CUSTOM CODE HERE
						**
						** SEARCH THE LIBRARY
						** (Simulated browse results below)
						** -------------------------------------------------------
						*/
					Hashtable results = new Hashtable();
				
				}
	
				//Process response SOAP message
				soap.ProcessResponse(Response);
			}
			else
			{
				Response.ContentType = "text/plain";
				Response.AddHeader("content-disposition", "inline; filename=xstandard.txt");
				if (soap.Test())
				{
					Response.Write("Status: Ready");
				}
				else
				{
					Response.Write("Status: Error - " + soap.ErrorMessage);
				}
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
