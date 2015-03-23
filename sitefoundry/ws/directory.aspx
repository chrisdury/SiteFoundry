<%@ Page language="C#" AutoEventWireup="false" ValidateRequest="false" Debug="false" %>
<%@Import namespace="XStandard.Directory"%>
<%@Import namespace="System.IO"%>
<%@Import namespace="System.Xml"%>
<%

/* -----------------------------------------------------------------------------------------
-                                        IMPORTANT
- 1. Put XStandard.Directory.dll into a sub-folder called "bin".
- 2. The folder containing this script must be made into an "Application" in IIS.
----------------------------------------------------------------------------------------- */




/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */
//Path to config file.
string configFile = Server.MapPath(@"directory.config");

//Directory name.
string directoryName = "Directory Service";

//Log file for errors or debug information.  Can be a template like "%Y-%m-%d.log" where %Y is 4 digit year, %m is 2 digit month and %d is 2 digit day.  This will produce a log file like "2003-12-29.log".
string logfile = Server.MapPath(".") + @"\x%Y-%m-%d.log"; // Or hardcode the path like "C:\XStandard.log"

//Enable the ablity to browse this Directory.
bool directoryBrowseEnabled = true;

//Enabled the ability to search this Directory. You'll need to customize the search feature to meet your CMS needs.
bool directorySearchEnabled = false;

//Used to turn debuging on or off.  This is a boolean value.
bool debug = false;

//An authorization code used to restrict access to this Web Service. You get this code from your account on the xstandard.com Web site.
string authorizationCode = "";
/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */










/* -----------------------------------------------------------------------------------------
- Purpose:	Directory Service
- 
- Input:	SOAP
- Output:	SOAP
-
- Copyright (c) 2002 Belus Technology Inc.  All rights reserved.
----------------------------------------------------------------------------------------- */

SOAPServer soap = new SOAPServer();
soap.ConfigFile = configFile;
soap.LogFile = logfile;
soap.DirectoryName = directoryName;
soap.DirectoryBrowseEnabled = directoryBrowseEnabled;
soap.DirectorySearchEnabled = directorySearchEnabled;
soap.CurrentFolder = Server.MapPath(".");
soap.Debug = debug;

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
	if(soap.Action == "doDirectoryDescribe")
	{
		if (directorySearchEnabled)
		{
			/*
			** -------------------------------------------------------
			** ADD CUSTOM CODE HERE
			**
			** Use method like:
			** soap.AddSearchFilter(string id, string label)
			** -------------------------------------------------------
			*/
			soap.AddSearchFilter("", "(no filter)");
			soap.AddSearchFilter("abc", "ABC");
			soap.AddSearchFilter("def", "DEF");
			soap.AddSearchFilter("ghi", "GHI");
		}
	}
	else if(soap.Action == "doDirectoryBrowse")
	{
		//Pass data to scripts in directory.config file
		soap.SharedAdd("DocumentID", Request.ServerVariables["HTTP_X_DOCUMENT_ID"]);
		soap.SharedAdd("UserID", Request.ServerVariables["HTTP_X_USER_ID"]);
		soap.SharedAdd("SessionID", Request.ServerVariables["HTTP_X_SESSION_ID"]);
		soap.SharedAdd("TransactionID", Request.ServerVariables["HTTP_X_TRANSACTION_ID"]);
		soap.SharedAdd("ClientID", Request.ServerVariables["HTTP_X_CLIENT_ID"]);
		soap.SharedAdd("InstanceID", Request.ServerVariables["HTTP_X_INSTANCE_ID"]);
		soap.SharedAdd("TagID", Request.ServerVariables["HTTP_X_TAG_ID"]);
		soap.SharedAdd("ZoneID", Request.ServerVariables["HTTP_X_ZONE_ID"]);
		soap.SharedAdd("ProjectID", Request.ServerVariables["HTTP_X_PROJECT_ID"]);
		soap.SharedAdd("AreaID", Request.ServerVariables["HTTP_X_AREA_ID"]);
		soap.SharedAdd("GroupID", Request.ServerVariables["HTTP_X_GROUP_ID"]);
		soap.SharedAdd("ParentID", Request.ServerVariables["HTTP_X_PARENT_ID"]);
		soap.SharedAdd("ContainerID", Request.ServerVariables["HTTP_X_CONTAINER_ID"]);
		soap.SharedAdd("ObjectID", Request.ServerVariables["HTTP_X_OBJECT_ID"]);
	}
	else if(soap.Action == "doDirectorySearch")
	{
		string searchFor = soap.GetProperty("searchFor");
		string filterBy = soap.GetProperty("filterBy");

		/*
		** -------------------------------------------------------
		** ADD CUSTOM CODE HERE
		**
		** Use method like:
		** soap.AddObject(string name, string data)
		** soap.AddObject(string name, string data, string icon)
		** -------------------------------------------------------
		*/
		soap.AddObject("Test 1", "<p>Hello World!</p>");
		soap.AddObject("Test 2", "<p>Hello World!</p>");
		soap.AddObject("Test 3", "<p>Hello World!</p>");
		soap.AddObject("Test 4", "<p>Hello World!</p>");
		soap.AddObject("Test 5", "<p>Hello World!</p>");
		soap.AddObject("Test 6", "<p>Hello World!</p>");
		soap.AddObject("Test 7", "<p>Hello World!</p>");
		soap.AddObject("Test 8", "<p>Hello World!</p>");
		soap.AddObject("Test 9", "<p>Hello World!</p>");
		soap.AddObject("Test 10", "<p>Hello World!</p>");
		soap.AddObject("Test 11", "<p>Hello World!</p>");
		soap.AddObject("Test 12", "<p>Hello World!</p>");
		soap.AddObject("Test 13", "<p>Hello World!</p>");
		soap.AddObject("Test 14", "<p>Hello World!</p>");
		soap.AddObject("Test 15", "<p>Hello World!</p>");
		soap.AddObject("Test 16", "<p>Hello World!</p>");
		soap.AddObject("Test 17", "<p>Hello World!</p>");
		soap.AddObject("Test 18", "<p>Hello World!</p>");
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
%>
