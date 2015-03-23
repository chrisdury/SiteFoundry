<%@ Page language="C#" AutoEventWireup="false" ValidateRequest="false" Debug="false" %>
<%@Import namespace="XStandard.SpellChecker"%>
<%@Import namespace="System.IO"%>
<%@Import namespace="System.Xml"%>
<%

/* -----------------------------------------------------------------------------------------
-                                        IMPORTANT
- 1. Put aspell-15.dll, pspell-15.dll, XStandard.SpellChecker.dll and XStandard.Aspell.dll
-    into a sub-folder called "bin".
- 2. The folder containing this script must be made into an "Application" in IIS.
----------------------------------------------------------------------------------------- */




/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */
//Log file for errors or debug information.  Can be a template like "%Y-%m-%d.log" where %Y is 4 digit year, %m is 2 digit month and %d is 2 digit day.  This will produce a log file like "2003-12-29.log".
string logfile = Server.MapPath(".") + @"\x%Y-%m-%d.log"; // Or hardcode the path like "C:\XStandard.log"

//Path to config file
string configFile = Server.MapPath("spellchecker.config");

//Path to custom dictionary
string customDictionary;
if (Request.ServerVariables["HTTP_X_SPELL_CHECKER_LANG"] == null || Request.ServerVariables["HTTP_X_SPELL_CHECKER_LANG"] == String.Empty)
{
	customDictionary = Server.MapPath(".") + @"\custom.txt";
}
else
{
	customDictionary = Server.MapPath(".") + @"\custom." + Request.ServerVariables["HTTP_X_SPELL_CHECKER_LANG"] + ".txt";
}

//Enable the ablity to add words to custom dictionary
bool spellCheckerAddToCustomDictionaryEnabled = true;

//Max number of suggestions per mispelled word. Set to -1 for unlimited.
int suggestionCount = 10;

//Used to turn debuging on or off.  This is a boolean value.
bool debug = false;

//An authorization code used to restrict access to this Web Service. You get this code from your account on the xstandard.com Web site.
string authorizationCode = "";
/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */










/* -----------------------------------------------------------------------------------------
- Purpose:	SpellChecker Service
- 
- Input:	SOAP
- Output:	SOAP
-
- Copyright (c) 2002 Belus Technology Inc.  All rights reserved.
----------------------------------------------------------------------------------------- */

SOAPServer soap = new SOAPServer();
soap.LogFile = logfile;
soap.ConfigFile = configFile;
soap.CustomDictionary = customDictionary;
soap.SpellCheckerAddToCustomDictionaryEnabled = spellCheckerAddToCustomDictionaryEnabled;
soap.CurrentFolder = Server.MapPath(".");
soap.SuggestionCount = suggestionCount;
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
