<%@ Page language="C#" AutoEventWireup="false" ValidateRequest="false" Debug="false" %>
<%@Import namespace="XStandard.Library"%>
<%@Import namespace="System.IO"%>
<%@Import namespace="System.Xml"%>
<%@Import namespace="System.Collections"%>
<%

/* -----------------------------------------------------------------------------------------
-                                        IMPORTANT
- 1. Put XStandard.Library.dll into a sub-folder called "bin".
- 2. The folder containing this script must be made into an "Application" in IIS.
- 3. Set file permissions to Everyone on Temp and Library folders.
----------------------------------------------------------------------------------------- */




/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */
//The root folder where uploaded files should be saved.
string libraryFolder = Server.MapPath("../files");

//Base URL to create for files. Relative URLs are okay, for example: "docs/"
string baseURL = "http://" + Request.ServerVariables["SERVER_NAME"].ToString() + Request.ServerVariables["SCRIPT_NAME"].ToString().Substring(0, Request.ServerVariables["SCRIPT_NAME"].ToString().LastIndexOf("/") + 1);

//Log file for errors or debug information.  Can be a template like "%Y-%m-%d.log" where %Y is 4 digit year, %m is 2 digit month and %d is 2 digit day.  This will produce a log file like "2003-12-29.log".
string logfile = Server.MapPath("../files") + @"\x%Y-%m-%d.log"; // Or hardcode the path like "C:\XStandard.log"

//Path to config file
string configFile = Server.MapPath("attachmentlibrary.config");

//Temp folder to stored received packets.
string tempFolder = @"C:\Temp\";

//A list of accepted file extensions.
string acceptedFileTypes = "txt zip pdf doc rtf tar ppt xls xml xsl xslt swf gif jpeg jpg png bmp";

//Maximum file upload size
long maxUploadSize = 2048000;

//Provide the last modified date for files.  For large libraries, turning this off can improve performance.
bool getDateLastModified = true;

//Provide file size. For large libraries, turning this off can improve performance.
bool getFileSize = true;

//Enable the ablity to browse this library
bool libraryBrowseEnabled = true;

//Enabled the ability to search this library. You'll need to customize the search feature to meet your CMS needs.
bool librarySearchEnabled = false;

//Enable the ability to upload files to root folder.
bool libraryUploadToRootContainerEnabled = true;

//Enable the ability to upload files to a sub folder.
bool libraryUploadToSubContainerEnabled = false;

//Enable the ability to replace file when uploading.
bool libraryUploadReplaceEnabled = true;

//Used to turn debuging on or off.  This is a boolean value.
bool debug = false;

//Comma delimited list of hidden folders
string hiddenFolders = "CVS,_vti_cnf";

//Comma delimited list of hidden files
string hiddenFiles = "";

//An authorization code used to restrict access to this Web Service. You get this code from your account on the xstandard.com Web site.
string authorizationCode = "";
/* -----------------------------------------------------------------------------------------
-                             OPTIONAL - CHANGE THESE SETTINGS
----------------------------------------------------------------------------------------- */










/* -----------------------------------------------------------------------------------------
- Purpose:	AttachmentLibrary Service
- 
- Input:	SOAP
- Output:	SOAP
-
- Copyright (c) 2002 Belus Technology Inc.  All rights reserved.
----------------------------------------------------------------------------------------- */
SOAPServer soap = new SOAPServer();
soap.LogFile = logfile;
soap.TempFolder = tempFolder;
soap.LibraryFolder = libraryFolder;
soap.LibraryBaseURL = baseURL;
soap.MaxUploadSize = maxUploadSize;
soap.LibraryBrowseEnabled = libraryBrowseEnabled;
soap.LibrarySearchEnabled = librarySearchEnabled;
soap.LibraryUploadToRootContainerEnabled = libraryUploadToRootContainerEnabled;
soap.LibraryUploadToSubContainerEnabled = libraryUploadToSubContainerEnabled;
soap.LibraryUploadReplaceEnabled = libraryUploadReplaceEnabled;
soap.AddAcceptedObjectType("file", false, acceptedFileTypes);
soap.AddAcceptedObjectType("folder", true, "");
soap.Debug = debug;

string rootFilePath = libraryFolder;
string rootFolderPath = libraryFolder;


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
		//Get the library name from config file
		XmlDocument config = new XmlDocument();	
		if (File.Exists(configFile))
		{
			string lang = soap.GetProperty("lang");
			config.Load(configFile);
			if (config.SelectSingleNode("/library/name[lang('" + lang + "') or count(@*) = 0]") != null)
			{
				soap.LibraryName = config.SelectSingleNode("/library/name[lang('" + lang + "') or count(@*) = 0]").InnerText.Trim();
			}
		}
		
		if (librarySearchEnabled)
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
			soap.AddSearchFilter("doc", "Word Documents");
			soap.AddSearchFilter("pdf", "PDF Documents");
		}
	}
	else if(soap.Action == "doLibraryBrowse")
	{
		//Get the current path
		string path = soap.GetProperty("path");
		bool usingNewBaseURL = false;
		
		//Process config file
		XmlDocument config = new XmlDocument();	
		Hashtable folderIndex = new Hashtable();
		Hashtable fileIndex = new Hashtable();
		if (File.Exists(configFile))
		{
			//Get library name from config file
			string lang = soap.GetProperty("lang");
			try
			{
				config.Load(configFile);
			}
			catch (Exception e)
			{
				soap.RaiseError("Failed to load config file.");
				soap.LogToFile("Failed to load config file: " + e.Message);
			}
			
			//Build folder index
			foreach (XmlElement node in config.SelectNodes("/library/folder"))
			{
				string temp = node.SelectSingleNode("path").InnerText.Trim().Replace(@"\", "/").ToLower();
				if (temp.EndsWith("/"))
				{
					temp = temp.Remove(temp.Length - 1, 1);
				}
				if (temp.StartsWith("/"))
				{
					temp = temp.Remove(0, 1);
				}
				if (soap.IsValidRelativePath(temp))
				{
					folderIndex.Add(temp.ToLower(), node);
				}
			}
			
			// build file index
			foreach (XmlElement node in config.SelectNodes("/library/attachment"))
			{
				string temp = node.SelectSingleNode("path").InnerText.Trim().Replace(@"\", "/").ToLower();
				if (temp.EndsWith("/"))
				{
					temp = temp.Remove(temp.Length - 1, 1);
				}
				if (temp.StartsWith("/"))
				{
					temp = temp.Remove(0, 1);
				}
				if (soap.IsValidRelativePath(temp))
				{
					fileIndex.Add(temp.ToLower(), node);
				}
			}
		}

		//Get the base URL for the current folder
		if (folderIndex.Contains(path.ToLower()))
		{
			XmlElement node = (XmlElement)folderIndex[path.ToLower()];
			if (node.SelectSingleNode("baseURL") != null)
			{
				if (node.SelectSingleNode("baseURL").InnerText.Trim() != String.Empty)
				{
					baseURL = node.SelectSingleNode("baseURL").InnerText.Trim();
					usingNewBaseURL = true;
				}
			}
		}
		
		//Get sub-folder to browse
		if (soap.IsValidRelativePath(path))
		{
			if (path == String.Empty)
			{
				rootFilePath = libraryFolder;
				rootFolderPath = libraryFolder;
			}
			else
			{
				rootFilePath = Path.Combine(libraryFolder, path);
				rootFolderPath = Path.Combine(libraryFolder, path);
			}
		
			DirectoryInfo root;
			if(Directory.Exists(rootFolderPath))
			{
				//Get folders
				root = new DirectoryInfo(rootFolderPath);
				foreach(DirectoryInfo folder in root.GetDirectories())
				{
					bool add = false;
					string temp = folder.Name;
					string label = folder.Name;
					bool empty = false;
					string icon = "";
					if (path != String.Empty)
					{
						temp = path + "/" + temp;
					}
					string url = baseURL;
					if (!url.EndsWith("/"))
					{
						url = url + "/";
					}
					url = url + soap.URLEncode(temp) + "/";
					if (folderIndex.Contains(temp.ToLower()))
					{
						XmlElement node = (XmlElement)folderIndex[temp.ToLower()];
						if (node.SelectSingleNode("hidden[text() = 'yes']") == null)
						{
							add = true;
							if (node.SelectSingleNode("label") != null)
							{
								label = node.SelectSingleNode("label").InnerText.Trim();
							}
							if (node.SelectSingleNode("icon") != null)
							{
								icon = node.SelectSingleNode("icon").InnerText.Trim();
							}
							if (node.SelectSingleNode("baseURL") != null)
							{
								if (node.SelectSingleNode("baseURL").InnerText.Trim() != String.Empty)
								{
									url = node.SelectSingleNode("baseURL").InnerText.Trim();
								}
							}
						}
					}
					else
					{
						add = true;
					}
	
					char[] hiddenFolderSeparator = new char[] {','};
					string[] hiddenFolderList = hiddenFolders.Split(hiddenFolderSeparator);
					for(int i = 0; i <= hiddenFolderList.Length - 1; i++)
					{
						if (folder.Name.ToLower() == hiddenFolderList[i].ToLower().Trim()) {
							add = false;
							break;
						}
					}
	
					if (add)
					{
						if (folder.GetFiles().Length == 0 && folder.GetDirectories().Length == 0)
						{
							empty = true;
						}
						soap.AddContainer(folder.Name, path, label, url, empty, icon, "", 0);
					}
				}
			}
			
			if(Directory.Exists(rootFilePath))
			{
				//Get files
				root = new DirectoryInfo(rootFilePath);
				ArrayList acceptedFileTypesList;
				acceptedFileTypesList = new ArrayList();
				char[] separator = new char[] {' ', ';', ','};
				foreach(string ext in acceptedFileTypes.ToLower().Split(separator))
				{
					if(ext != String.Empty)
					{
						acceptedFileTypesList.Add(ext);
					}
				}
				
				foreach(FileInfo file in root.GetFiles())
				{
					bool add = false;
					string temp = file.Name;
					string name = temp;
					string title = "";
					string cssClass = "";
					bool newWindow = false;
					string icon = "";
					if (path != String.Empty)
					{
						temp = path + "/" + temp;
					}
					acceptedFileTypesList.Sort();
					if(acceptedFileTypesList.BinarySearch("*") >= 0 || acceptedFileTypesList.BinarySearch(file.Extension.ToLower().Replace(".", "")) >= 0)
					{
						if (fileIndex.Contains(temp.ToLower()))
						{
							XmlElement node = (XmlElement)fileIndex[temp.ToLower()];
							if (node.SelectSingleNode("hidden[text() = 'yes']") == null)
							{
								add = true;
								if (node.SelectSingleNode("class") != null)
								{
									cssClass = node.SelectSingleNode("class").InnerText.Trim();
								}
								if (node.SelectSingleNode("newWindow[text() = 'yes']") != null)
								{
									newWindow = true;
								}
								if (node.SelectSingleNode("icon") != null)
								{
									icon = node.SelectSingleNode("icon").InnerText.Trim();
								}
							}
						}
						else
						{
							add = true;
						}
						
						char[] hiddenFileSeparator = new char[] {','};
						string[] hiddenFileList = hiddenFiles.Split(hiddenFileSeparator);
						for(int i = 0; i <= hiddenFileList.Length - 1; i++)
						{
							if (file.Name.ToLower() == hiddenFileList[i].ToLower().Trim()) {
								add = false;
								break;
							}
						}
	
						if (add)
						{
							string url = "";
							if (baseURL != String.Empty)
							{
								if (baseURL.EndsWith("/"))
								{
									url = baseURL;	
								}
								else
								{
									url = baseURL + "/";
								}	
							}
							
							if (!usingNewBaseURL)
							{
								url = url + soap.URLEncode(path);
							}
							
							if (url.EndsWith("/"))
							{
								url = url + soap.URLEncode(file.Name);
							}
							else
							{
								url = url + "/" + soap.URLEncode(file.Name);
							}
							
							ArrayList attributes = new ArrayList();
							attributes.Add(new XStandard.Library.Attribute("href", url));
							if (title != String.Empty)
							{
								attributes.Add(new XStandard.Library.Attribute("title", title));
							}
							if (cssClass != String.Empty)
							{
								attributes.Add(new XStandard.Library.Attribute("class", cssClass));
							}
							
							ArrayList properties = new ArrayList();
							if (getFileSize)
							{
								properties.Add(new XStandard.Library.Property("size", file.Length.ToString()));
							}
							if (getDateLastModified)
							{
								properties.Add(new XStandard.Library.Property("date", soap.ISODate(file.LastWriteTime) + " " + soap.ISOTime(file.LastWriteTime)));
							}
							if (newWindow)
							{
								properties.Add(new XStandard.Library.Property("newWindow", "true"));
							}
							soap.AddObject(file.Name, path, file.Name, attributes.ToArray(), properties.ToArray(), icon, "", 0);
						}
					}
				}
			}
		}
		else
		{
			soap.RaiseError("Invalid path.");
		}
	}
	else if(soap.Action == "doLibrarySearch")
	{
		ArrayList attributes;
		ArrayList properties;
		string searchFor = soap.GetProperty("searchFor");
		string filterBy = soap.GetProperty("filterBy");
		
		/*
		** -------------------------------------------------------
		** ADD CUSTOM CODE HERE
		**
		** Use method like:
		** soap.AddObject(string objectName, string path, string label, object[] attributes, object[] properties, string icon, string metadata, int options)
		** -------------------------------------------------------
		*/
		attributes = new ArrayList();
		properties = new ArrayList();
		attributes.Add(new XStandard.Library.Attribute("href", "http://xstandard.com/images/logo.gif"));
		soap.AddObject("logo.gif", "", "XStandard Web Site Logo", attributes.ToArray(), properties.ToArray(), "image", "", 0);
	}
	
	//Process response SOAP message
	soap.ProcessResponse(Response);

	if(soap.Action == "doLibraryUpload")
	{
		if (soap.TransactionComplete == true)
		{
			/*
			** -------------------------------------------------------
			** ADD CUSTOM CODE HERE
			** -------------------------------------------------------
			*/
		}
	}
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
