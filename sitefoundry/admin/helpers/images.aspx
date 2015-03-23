<%@ Page language="c#" Codebehind="images.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.admin.helpers.images" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>Insert An Image </title>
		<link rel="stylesheet" href="../includes/common.css" type="text/css" />
		<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
		<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
		
		<style type="text/css">
		
		.headline {
			font-size:14px;
			color:#fff;
			font-weight:bold;
		}
		
		#header {
			height:30px;
			background-color:#9DA4AD;
			border-bottom:4px solid #ddd;
			padding:5px;		
		}
		#filelist {
			width:400px;
			height:450px;
			overflow:auto
		}
		#upload {
			position:absolute;
			top:480px;
			width:400px;
			height:60px;
			text-align:center;
			padding:15px;
			background-color:#eee;
			font-size:10px;
		}
		#imageinfo {
			position:absolute;
			left:400px;
			top:30px;
			height:450px;
			width:200px;
			text-align:center;
			background-color:#eee;
			padding:2px;		
		}
		#actions {
			position:absolute;
			left:400px;
			top:480px;
			width:200px;
			height:60px;
			text-align:center;
			vertical-align:middle;
			padding:15px;
			background-color:#eee;
		}
		table.imagePreview td {
			text-align:center;
			vertical-align:middle;
		
		}
		input.imgPadding {
			font-size:10px;
			width:25px;
		}
		fieldset {
			margin:0px 2px 5px 2px;
			font-size:10px;
		}
		fieldset td {
			font-size:10px;
		}
		fieldset legend {
			font-size:10px;
		}
		</style>	
		
		<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
		<script language="javascript">
		
		var maxWidth = 120;
		var maxHeight = 100;
		var siteRoot = '<%=Dury.SiteFoundry.SFGlobal.SiteRoot%>';
		var resourcesDir = '<%=Dury.SiteFoundry.SFGlobal.ResourceFileLocation%>';
	
		function pickImage(filename,width,height,size) {
			i = document.getElementById("image");
			if (width>height) {
				i.width = maxWidth;
				i.height = height / (width/maxWidth);
			} else {
				i.height = maxHeight;
				i.width = width / (height/maxHeight);
			} 
			i.src = siteRoot + resourcesDir + "/" + filename;
			
			document.forms[0]['filename'].value = filename + "|" + width + "|" + height;
			
		}
		
		function generateImageHtml() {
			var s = "";
			var f = document.forms[0];
			var generateLink = false;
			t = f['filename'].value.split('|');
			var imageFilename = t[0];
			var imageWidth = t[1];
			var imageHeight = t[2];
			var thumbImage = f['thumbsList'].options[f['thumbsList'].selectedIndex].value;
			var thumbWidth = (f['thumbWidth'].value.length > 0 && f['thumbWidth'].value % 1 == 0) ? parseInt(f['thumbWidth'].value) : 0;
			var thumbHeight = (f['thumbHeight'].value.length > 0 && f['thumbHeight'].value % 1 == 0) ? parseInt(f['thumbHeight'].value) : 0;
			var thumbPercent = (f['thumbPercent'].value.length > 0 && f['thumbPercent'].value % 1 == 0) ? parseInt(f['thumbPercent'].value) : 0;
			
			if (thumbImage == '' && thumbWidth == 0 && thumbHeight == 0 && thumbPercent == 0) {
				imageFilename = siteRoot + resourcesDir + '/' + imageFilename;
					
			} else {
				if (thumbImage != '') {
					ti = thumbImage.split('|');
					imageFilename = siteRoot + resourcesDir + '/' + ti[0];
					imageWidth = ti[1];
					imageHeight = ti[2];
				} else {
					if (thumbPercent != 0) {
						thumbPercent = thumbPercent / 100;
						imageWidth = thumbWidth = Math.floor(imageWidth * thumbPercent);
						imageHeight = thumbHeight = Math.floor(imageHeight * thumbPercent);
					} else {
						if (thumbWidth != 0) {
							imageHeight = Math.floor(imageHeight * (thumbWidth / imageWidth));
							imageWidth = thumbWidth;
						} else {
							imageWidth = Math.floor(imageWidth * (thumbHeight / imageHeight));
							imageHeight = thumbHeight;
						}			
					}
					imageFilename = formatString(siteRoot + "image.ashx/{0}/{1}/{2}",imageFilename,imageWidth,imageHeight);
				}
			}
			
			
			var src = '<div style="' + getImagePaddingStyle() + getImageAlignStyle() + getImageBackgroundColor() + '">' + getImageLinkHtml(true) + '<img src="{0}" width="{1}" height="{2}" border="0" alt="" />' + getImageLinkHtml(false) + '</div>';
			src = formatString(src,imageFilename,imageWidth,imageHeight);
			
			q = '<%=Request.QueryString%>';
			if (window.opener)
				window.opener.FTB_InsertText(q,src);		
			else
				alert("main window closed!");			
		}
		
		
		
		

		function getImageLinkHtml(openTag) {
			var f = document.forms[0];
			var url;
			
			if (f['openLargeImage'].checked == true) {
				t = f['filename'].value.split('|');					
				url = formatString('javascript:openWindow(\'<%=Dury.SiteFoundry.SFGlobal.SiteRoot%><%=Dury.SiteFoundry.SFGlobal.ResourceFileLocation%>/{0}\',{1},{2})',t[0],t[1],t[2]);
			} else if (f['openURL'].value.length > 0) {
				url = f['openURL'].value;				
			} else if (f['openPage'].selectedIndex != 0) {
				url = f['openPage'].options[f['openPage'].selectedIndex].value;
			}
			
			if (url != null) {
				if (openTag) { return '<a href="' + url + '">'; }
				else { return '</a>'; }
			} else { return ''; }
		}	
		
		
		function getImageBackgroundColor() {
			var f = document.forms[0];
			if (f['bgcolor'].value != '') {
				return 'background-color:' + f['bgcolor'].value + ';';
			} else {
				return '';
			}		
		}
		
		function getImageAlignStyle() {
			var f = document.forms[0];
			var alignArray = new Array('','width:100%;text-align:center;', 'float:left;', 'float:right;');
			return alignArray[f['alignment'].selectedIndex];	
		}		
		
		
		function getImagePaddingStyle() {
			var paddingArray = new Array( 'img_padding_top', 'img_padding_right', 'img_padding_bottom', 'img_padding_left');
			var style = "padding:";
			var f = document.forms[0];
			for(i=0; i<paddingArray.length; i++) {
				if (f[paddingArray[i]].value == 0 || f[paddingArray[i]].value == "") {
					style += "0px ";
				} else {
					style += f[paddingArray[i]].value + "px ";
				}
			}
			style += ";";
			return style;		
		}
		
		function validate() {
			var f = document.forms[0];
			
			//check to see if image selected
			if (f['filename'].value.length <= 0) {
				alert("please select an image");
				return false;
			}
			
			// padding(s) integer only
			paddingArray = new Array( 'img_padding_top', 'img_padding_right', 'img_padding_bottom', 'img_padding_left');
			for (i=0;i<paddingArray.length;i++) {
				re0 = new RegExp('^[0-9]*$');
				if (f[paddingArray[i]].value.length > 0 && !f[paddingArray[i]].value.match(re0)) {
					f[paddingArray[i]].select();
					f[paddingArray[i]].focus();
					alert('padding can only be positive numbers');
					return false;
				}
			}
		
		
			// background color is html
			r1 = '^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$';
			re1 = new RegExp(r1);
			if (f['bgcolor'].value.length > 0 && !f['bgcolor'].value.match(re1)) {
				alert('invalid color code used.');
				f['bgcolor'].select();
				f['bgcolor'].focus();
				return false;
			}	
			
			// no .gifs can be resized (yet);
			if (( f['thumbsList'].selectedIndex != 0 || f['thumbPercent'].value.length > 0 || f['thumbWidth'].value.length > 0 || f['thumbHeight'].value.length > 0 ) && f['filename'].value.indexOf('.gif') != -1) {
				alert("only JPGs can be resized. sorry!");
				return false;			
			}
			return true;
		}

		
		</script>
	</head>
	<body class="gallery">
		<form id="images" method="post" runat="server" encType="multipart/form-data">
			<input type="hidden" name="filename" value="" />
			<div id="header">
				<span class="headline">Images</span>
			</div>
			<div id="filelist" class="scrollStyle">
				<asp:repeater id="imageRepeater" runat="server">
					<itemTemplate>
						<span class="imgTile normal" onClick="setActiveTile(this);pickImage('<%#DataBinder.Eval(Container.DataItem,"filename") %>',<%#DataBinder.Eval(Container.DataItem,"width") %>,<%#DataBinder.Eval(Container.DataItem,"height") %>,<%#DataBinder.Eval(Container.DataItem,"size") %>)">
							<table border="0" cellpadding="0" cellspacing="0">
								<tr>
									<td valign="middle"><img src="<%=Dury.SiteFoundry.SFGlobal.SiteRoot+Dury.SiteFoundry.SFGlobal.ResourceFileLocation%>/<%#DataBinder.Eval(Container.DataItem,"filename") %>" height="50" width="50" style="margin:5px;" alt='image uploaded on: <%#DataBinder.Eval(Container.DataItem,"dateModified") %>' /></td>
									<td valign="top">
										<span class="small">
											<%#DataBinder.Eval(Container.DataItem,"filename") %><br />
											<span style="color:#666;"><%#DataBinder.Eval(Container.DataItem,"width") %>x<%#DataBinder.Eval(Container.DataItem,"height") %> (<%# getFileSize((int)DataBinder.Eval(Container.DataItem,"size")) %> KB)</span><br />
											<a href="<%=Dury.SiteFoundry.SFGlobal.SiteRoot+Dury.SiteFoundry.SFGlobal.ResourceFileLocation%>/<%#DataBinder.Eval(Container.DataItem,"filename") %>" target="_new"><img src="../images/zoom.gif" border="0"></a>
											<asp:imageButton id="deleteButton" src="../images/delete.gif" runat="server" onCommand="deleteImage" commandArgument='<%#DataBinder.Eval(Container.DataItem,"filename") %>' alternatetext="click to delete image"/>
										</span>
									</td>
								</tr>
							</table>
						</span>
					</itemTemplate>
				</asp:repeater>
			</div>
			
			<div id="upload">
				New Image:
				<input type="file" runat="server" id="fileToUpload" class="" NAME="fileToUpload" style="width:230px;" cssclass="small"/>
				<asp:button id="uploadButton" runat="server" text="upload" class="button green" />
			</div>
			
			
			<div id="imageinfo">
				<fieldset>
					<legend>Padding</legend>
					<table border="0" cellpadding="2" cellspacing="0" !width="180" class="imagePreview">
						<tr>
							<td colspan="3"><asp:textbox id="img_padding_top" runat="server" maxlength="3" cssclass="imgPadding" /></td>
						</tr>
						<tr>
							<td>
								<asp:textbox id="img_padding_left" runat="server" maxlength="3" cssclass="imgPadding" />
							</td>
							<td>
								<img src="../images/selectanimage.gif" width="120" id="image" />
							</td>
							<td>
								<asp:textbox id="img_padding_right" runat="server" maxlength="3" cssclass="imgPadding" />
							</td>
						</tr>
						<tr>
							<td colspan="3">
								<asp:textbox id="img_padding_bottom" runat="server" maxlength="3" cssclass="imgPadding" />
							</td>
						</tr>
					</table>
				</fieldset>
				<fieldset>
					<legend>Background & Alignment</legend>
					<table border="0" cellpadding="2" cellspacing="0">
						<tr>	
							<td>Colour:</td>
							<td><asp:textbox id="bgcolor" runat="server" runat="server" maxlength="7" width="60px" cssclass="small" /></td>
						</tr>
						<tr>
							<td>Align:</td>
							<td>
								<asp:dropdownlist id="alignment" runat="server" cssclass="small">
									<asp:listitem>none</asp:listitem>
									<asp:listitem>centre</asp:listitem>
									<asp:listitem>left</asp:listitem>
									<asp:listitem>right</asp:listitem>
								</asp:dropdownlist>
							</td>
						</tr>
					</table>
				</fieldset>
				
				<fieldset>
					<legend>Thumbnail</legend>
					<asp:dropdownlist id="thumbsList" runat="server" cssclass="small"/>
					<hr size="1" noshade>
					<asp:textbox id="thumbPercent" runat="server" maxlength="2" width="20px" cssclass="small" /> % /
					width:<asp:textbox id="thumbWidth" runat="server" maxlength="3" width="25" cssclass="small" />
					height:<asp:textbox id="thumbHeight" runat="server" maxlength="3" width="25" cssclass="small" />
				</fieldset>
				
				<fieldset>
					<legend>Click Behaviour</legend>
					<table border="0" cellpadding="1" cellspacing="0">
						<tr><td>&nbsp;</td><td><asp:checkbox id="openLargeImage" runat="server" text="open large image" /></td></tr>
						<tr><td>url:</td><td><asp:textbox id="openURL" runat="server" width="140px" cssclass="small" text="" /></td></tr>
						<tr><td>page:</td><td><asp:dropdownlist id="openPage" runat="server" cssclass="small" /></td></tr>
					</table>
				</fieldset>
			</div>
			<div id="actions">
				<!--
				<asp:button id="insertButton" runat="server" text="insert" cssclass="button green" />
				<asp:button id="insertAndCloseButton" runat="server" text="insert & close" cssclass="button green" />
				-->
				<input type="button" id="insert" onclick="if (validate()) generateImageHtml();" value="insert" class="button green" />
				<input type="button" id="insertAndClose" onclick="if (validate()) generateImageHtml(); if (validate()) window.close();" value="insert and close" class="button green" />				
			</div>
		</form>
	</body>
</html>
