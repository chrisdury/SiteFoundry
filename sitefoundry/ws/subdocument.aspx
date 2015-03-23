<%@ Page language="C#" AutoEventWireup="false" ValidateRequest="false" Debug="false" %>
<%@Import namespace="XStandard.Subdocument"%>
<%@Import namespace="System.IO"%>
<%@Import namespace="System.Xml"%>
<%@Import namespace="System.Collections"%>
<%

/* -----------------------------------------------------------------------------------------
-                                        IMPORTANT
- 1. Put XStandard.Subdocument.dll into a sub-folder called "bin".
- 2. The folder containing this script must be made into an "Application" in IIS.
-
- To customize this service, search for "ADD CUSTOM CODE HERE" below.
----------------------------------------------------------------------------------------- */




/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */
//Log file for errors or debug information.  Can be a template like "%Y-%m-%d.log" where %Y is 4 digit year, %m is 2 digit month and %d is 2 digit day.  This will produce a log file like "2003-12-29.log".
string logfile = Server.MapPath(".") + @"\x%Y-%m-%d.log"; // Or hardcode the path like "C:\XStandard.log"

//Used to turn debuging on or off.  This is a boolean value.
bool debug = false;

//An authorization code used to restrict access to this Web Service. You get this code from your account on the xstandard.com Web site.
string authorizationCode = "";
/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */










/* -----------------------------------------------------------------------------------------
- Purpose:	Subdocument Service
- 
- Input:	SOAP
- Output:	SOAP
-
- Copyright (c) 2002 Belus Technology Inc.  All rights reserved.
----------------------------------------------------------------------------------------- */
SOAPServer soap = new SOAPServer();
soap.LogFile = logfile;
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
	if(soap.Action == "doSubdocumentDescribe")
	{
		/*
		** -------------------------------------------------------
		** ADD CUSTOM CODE HERE
		**
		** Use method like:
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute)
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute, string option01Attribute)
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute, string option01Attribute, string option02Attribute)
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute, string option01Attribute, string option02Attribute, string option03Attribute)
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute, string option01Attribute, string option02Attribute, string option03Attribute, string option04Attribute)
		** soap.AddSubdocumentDefinition(string elementName, string idAttribute, string option01Attribute, string option02Attribute, string option03Attribute, string option04Attribute, string option05Attribute)
		** -------------------------------------------------------
		*/
		soap.AddSubdocumentDefinition("include", "doc");
	}
	else if(soap.Action == "doSubdocumentDownload")
	{
		//Get request info
		string type = soap.GetProperty("type"); //custom element name
		string id = soap.GetProperty("id"); //id
		string option01 = soap.GetProperty("option01"); //additional info
		string option02 = soap.GetProperty("option02"); //additional info
		string option03 = soap.GetProperty("option03"); //additional info
		string option04 = soap.GetProperty("option04"); //additional info
		string option05 = soap.GetProperty("option05"); //additional info
		string option06 = soap.GetProperty("option06"); //additional info
		string option07 = soap.GetProperty("option07"); //additional info
		string option08 = soap.GetProperty("option08"); //additional info
		string option09 = soap.GetProperty("option09"); //additional info
		string option10 = soap.GetProperty("option10"); //additional info
		
		/*
		** -------------------------------------------------------
		** ADD CUSTOM CODE HERE
		**
		** Use method like:
		** soap.SetSubdocument(string content)
		**
		** Note, only call this method when the subdocument exists.
		** -------------------------------------------------------
		*/
		if (id == "A") {
			soap.SetSubdocument(soap.ReadFromFile(Server.MapPath("subdocument-example-A.txt")));
		} else if (id == "B") {
			soap.SetSubdocument(soap.ReadFromFile(Server.MapPath("subdocument-example-B.txt")));
		} else if (id == "C") {
			soap.SetSubdocument(soap.ReadFromFile(Server.MapPath("subdocument-example-C.txt")));
		} else if (id == "D") {
			soap.SetSubdocument(soap.ReadFromFile(Server.MapPath("subdocument-example-D.txt")));
		} else if (id == "E") {
			soap.SetSubdocument(soap.ReadFromFile(Server.MapPath("subdocument-example-E.txt")));
		}
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
