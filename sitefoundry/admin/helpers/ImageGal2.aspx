<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="iso-8859-1" %>
<script runat="server">
private string siteID;
private string basePath;
int thumbWidth = 90;
string[] supportedExtensions = {"gif","jpg","bmp"};

private double spaceAllowed = 0;	//in Kb 0 for no limit
private double maxFileSize = 2048;	//in Kb

private void Page_Load(object sender, System.EventArgs e) {
	//base path could be set using querystring
	basePath = "/sitefoundry/files/";
	
	if (!IsPostBack){
		ListFiles();	
	}
}

private void DeleteImage(object sender, System.EventArgs e){
	if (ImageName.Value == ""){
		statusinfo.InnerHtml = "Please Select An Image To Delete First";
	}
	else{
		statusinfo.InnerHtml = "Delete: " + ImageName.Value;
		System.IO.File.Delete(Server.MapPath(basePath + ImageName.Value));
	}
	
	ListFiles();
}

private void UploadImage(object sender, System.EventArgs e){
	//Response.Redirect("UploadImage.aspx?" + Request.QueryString);
	ImagesHolder.Visible = false;
	ControlHolder.Visible = false;
	UploadAreaHolder.Visible = true;
	
	//Set Default Size
	_Size_Medium.Checked = true;
}
private void AdvancedUpload(object sender, System.EventArgs e){
	if (AdvancedUploadHolder.Visible == false){
		AdvancedUploadHolder.Visible = true;
	}
	else{
		AdvancedUploadHolder.Visible = false;
	}
}
private void DoUpload(object sender, System.EventArgs e){
	if (_FileName.Value == ""){
		statusinfo.InnerHtml = "Please Select A File To Upload";
		return;
	}
	
	if (_FileName.PostedFile == null){
		statusinfo.InnerHtml = "Please Select A Valid File";
		return;
	}
	
	bool allowedType = false;
	foreach (string extension in supportedExtensions){
		if (extension.ToLower() == System.IO.Path.GetExtension(_FileName.Value).TrimStart('.')){
			allowedType = true;
			break;
		}
	}
	
	if (!allowedType){
		statusinfo.InnerHtml = "Please Select A Valid Type";
		return;
	}
	
	//Check output Image Size For Upload NOT POSTED FILE
	/*
	if ((outputImage.Length / 1024) > maxFileSize){
		statusinfo.InnerHtml = "That file is too large";
		return;
	}
	
	Response.Write("File: " + _FileName.PostedFile.ContentLength + " used: " + spaceUsed + " Allowed: " + spaceAllowed);
	if (((_FileName.PostedFile.ContentLength / 1024) + spaceUsed)  > spaceAllowed){
		statusinfo.InnerHtml = "That file is too large you have used too much space already";
		return;
	}
	
	*/
	
	System.Drawing.Bitmap origBitmap = new System.Drawing.Bitmap(_FileName.PostedFile.InputStream);
	
	int reqHeight;
	int reqWidth;
	int newHeight;
	int newWidth;
	
	if (_Size_Icon.Checked){
		reqHeight = 50;
		reqWidth = 50;
	}
	else if (_Size_Medium.Checked){
		reqHeight = 150;
		reqWidth = 150;
	}
	else if (_Size_Large.Checked){
		reqHeight = 400;
		reqWidth = 400;
	}
	else{
		reqHeight = origBitmap.Height;
		reqWidth = origBitmap.Width;
	}	
	
	 if (origBitmap.Height >= origBitmap.Width){
		//Portrait
		//Response.Write("Portrait ");	
		newHeight = reqHeight;
		newWidth = (int)(((double)origBitmap.Width / (double)origBitmap.Height) * reqHeight);
	}
	else{
		//Landscape
		//Response.Write("Landscape ");
		newWidth = reqWidth;
		newHeight = (int)(((double)origBitmap.Height / (double)origBitmap.Width) * reqWidth);
	}
	
	System.Drawing.Bitmap outputImage = new System.Drawing.Bitmap(origBitmap, newWidth, newHeight);
	outputImage.Save(Server.MapPath(basePath + System.IO.Path.GetFileName(_FileName.Value)),System.Drawing.Imaging.ImageFormat.Jpeg);
	
	statusinfo.InnerHtml = "Uploaded " + Server.MapPath(basePath + System.IO.Path.GetFileName(_FileName.Value));

	ImagesHolder.Visible = true;
	ControlHolder.Visible = true;
	UploadAreaHolder.Visible = false;
	
	ListFiles();
}
private void ListFiles(){
	//Get List Of Files
    System.IO.FileInfo[] files = (new System.IO.DirectoryInfo(Server.MapPath(basePath))).GetFiles("*");
    int count = 0;
	foreach (System.IO.FileInfo fileInfo in files)
    {
       // Response.Write(fileInfo.Name + "<br>");
		
		bool supported = false;
		foreach (string extension in supportedExtensions){
			if ("." + extension.ToLower() == fileInfo.Extension.ToLower()){
				supported = true;
			}
		}
		
		if (supported){
			
			string fileName = fileInfo.Name;
			long fileSize = fileInfo.Length / 1024;
			int fileWidth = 0;
			int fileHeight = 0;		
			
			System.Drawing.Image imageFile = System.Drawing.Image.FromFile(fileInfo.FullName);
				fileWidth = imageFile.Width;
				fileHeight = imageFile.Height;
			imageFile.Dispose();
					
			Images.Controls.Add(new LiteralControl(String.Format("\t<div class=\"ImageBox\" id=\"imagebox{0}\">\n",count)));
			Images.Controls.Add(new LiteralControl(String.Format("\t\t<div class=\"Image\" id=\"image{0}\">",count)));
			
			HtmlImage currentImage = new HtmlImage();
			currentImage.Attributes["Src"] = "thumbnail.aspx?height=90&width=90&image=" + basePath + fileName;
			currentImage.Attributes["onclick"] = String.Format("SelectImage(\"image{0}\",\"{1}\",\"{2}\",\"{3}\")",count,fileName,fileWidth,fileHeight);
			currentImage.Attributes["ondblclick"] = "UseImage();";
			
			Images.Controls.Add(currentImage);
	
			Images.Controls.Add(new LiteralControl("</div>\n"));
			Images.Controls.Add(new LiteralControl("\t\t<div class=\"Caption\">"));
			Images.Controls.Add(new LiteralControl(String.Format("<p>{0}<br>{2} x {3} ({1}k)</p>",fileName,fileSize,fileWidth,fileHeight)));
			Images.Controls.Add(new LiteralControl("</div>\n"));
			Images.Controls.Add(new LiteralControl("\t</div>\n"));
			
			count++;
   		}
		
	}
	
	WriteSpaceUsed();
}
private void WriteSpaceUsed(){
	if (spaceAllowed != 0 && spaceUsed >= spaceAllowed){
		UploadButton.Visible = false;
	}
	else{
		UploadButton.Visible = true;
	}
	
	//Details
	if (spaceAllowed != 0){
		double percentUsed = (spaceUsed / spaceAllowed) * 100;
		usagestats.InnerHtml = System.String.Format("You Have Used {0}k of {1}k (<img src=\"percentbar.aspx?used={2}&amp;height=8&amp;width=60\"> {2}%)",((int)spaceUsed),((int)spaceAllowed),((int)percentUsed));
	}
	else{
		usagestats.InnerHtml = System.String.Format("You Have Used {0}k ",((int)spaceUsed));
	}
}

private double spaceUsed{

	get{
		System.IO.FileInfo[] files = (new System.IO.DirectoryInfo(Server.MapPath(basePath))).GetFiles("*");
		double spaceUsed = 0;
		foreach (System.IO.FileInfo fileInfo in files){
			spaceUsed += fileInfo.Length / 1024;
		}
		
		return spaceUsed;
	}
}
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>Image Gallery</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<style type="text/css">
<!--
body {
	margin: 0px;
	padding: 0px;
	font-family: Arial, Helvetica, sans-serif;
	background-color:#99CCFF;
}
#Header {
	margin: 0px;
	padding: 20px;
	height: 50px;
	background-color:#99CCFF;
}
#Header h1{
	margin: 0px;
	padding: 0px;
}
#Header p{
	margin: 0px;
	padding: 0px;
	font-size:12px;
}
#Images{
	margin: 0px;
	padding: 10px 0px;
	height: 280px;
	background-color:#FFFFFF;
	overflow: auto;
}
#UploadArea{
	margin: 0px;
	padding: 10px 10px;
	height: 300px;
	background-color:#FFFFFF;
	overflow: auto;
}
#Images .ImageBox{
	height: <% Response.Write(thumbWidth + 30 + "px"); %>;
	width: <% Response.Write(thumbWidth + "px"); %>;
	margin: 5px;
	padding: 0px;
	float: left;
}
#Images .Image{
	padding: 2px 2px;
	margin: 0px;
	height: <% Response.Write(thumbWidth + "px"); %>;
	width: <% Response.Write(thumbWidth + "px"); %>;
	border: 2px solid #FFFFFF;
	text-align: center;
}
#Images .ImageBox .Caption{
	padding: 0px;
	margin: 0px;
	text-align: center;
}
#Images .ImageBox .Caption P{
	padding: 0px 5px;
	margin: 0px;
	font-size: 10px;
	width: <% Response.Write(thumbWidth + "px"); %>;
	height: 30px;
	overflow: hidden;
	white-space: nowrap;
}
#Controls{
	margin: 0px;
	padding: 10px 20px 0px 20px;
	font-size:12px;
	background-color:#99CCFF;
}
#statusinfo{
	margin: 0px;
	padding: 10px 20px 0px 20px;
	font-size:12px;
	background-color:#99CCFF;
}
#Controls p {
	margin: 0px;
	padding: 0px;
}
-->
</style>
<script language="javascript">
selected = "none";
selectedimage = "none";

function SelectImage(imageid,imagename,imagewidth,imageheight){
	
	if (selected == imageid){
		UseImage();
		return;
	}
	
	if (selected != "none")
	{
		document.getElementById(selected).style.border = "2px solid #FFFFFF";
	}

	document.getElementById(imageid).style.border = "2px solid #316AC5";
	document.getElementById("ImageName").value = imagename;
	document.getElementById("ImageHeight").value = imageheight;
	document.getElementById("ImageWidth").value = imagewidth;
	selected = imageid;
}

function UseImage(){
	if (document.getElementById("ImageName").value != null){
		imageLink = "<img src=\"<%Response.Write(basePath);%>" + document.getElementById("ImageName").value + "\" />";
		window.opener.FTB_InsertText("<% Response.Write(Request.QueryString["textboxname"]); %>",imageLink);
		this.focus();
		
		//if you want the window to close after one useage enable this
		//this.close();
	}
}
</script>
</head>
<body>
<form id="form1" runat="server">
<div id="Header">
	<h1>Image Upload</h1>
	<p id="usagestats" runat="server">There Is No Space Limit Set</p>
</div>
<asp:PlaceHolder ID="ImagesHolder" runat="server">
<div id="Images" runat="server">
</div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="UploadAreaHolder" runat="server" Visible="false">
<div id="UploadArea" runat="server">
	<p>Please select an image from your hard drive to upload to the store:</p>
	<input type="file" id="_FileName" runat="server" />
	<input type="button" id="_DoUpload" runat="server" OnServerClick="DoUpload" value="Upload Image"/>
	<p><asp:LinkButton ID="_AdvancedUpload" OnClick="AdvancedUpload" runat="server">Advanced >></asp:LinkButton></p>
	<asp:PlaceHolder ID="AdvancedUploadHolder" runat="server" Visible="false">
		<p>Advanced Options:</p>
		<p>Please Choose An Image Size</p>
		<P>
			Icon(50x50):<input type="radio" id="_Size_Icon" value="Icon" runat="server">
			Medium(150x150):<input type="radio" id="_Size_Medium" value="Medium" runat="server">
			Large(400x400):<input type="radio" id="_Size_Large" value="Large" runat="server">
			Unchanged:<input type="radio" id="_Size_Full" value="Full" runat="server">
		</P>
	</asp:PlaceHolder>
</div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="ControlHolder" runat="server">
<div id="Controls">
<p>
	<a href="javascript: UseImage();">Use Selected Image</a> | 
	<asp:PlaceHolder ID="UploadButton" runat="server"><asp:LinkButton ID="_Upload" OnClick="UploadImage" runat="server">Upload New Image</asp:LinkButton> | </asp:PlaceHolder>
	<asp:LinkButton ID="_Delete" OnClick="DeleteImage" runat="server">Delete</asp:LinkButton> |  
	<a href="javascript: void(window.close());">Close Window </a>
</p>
</div>
</asp:PlaceHolder>
<p id="statusinfo" style="color:#990000" runat="server">Please Choose An Image</p>
<input type="hidden" id="ImageName" runat="server" />
<input type="hidden" id="ImageHeight" runat="server" />
<input type="hidden" id="ImageWidth" runat="server" />
</form>
</body>
</html>