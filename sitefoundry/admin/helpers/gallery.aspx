<%@ Page language="c#" Codebehind="gallery.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.helpers.imageGallery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>Insert An Image &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</title>
		<link rel="stylesheet" href="../includes/common.css" type="text/css" />
		<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
		<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
		<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
		<script language="javascript">
		<!--
		
		function pickImage(src,w,h) {
			f = document.forms[0];
			f['origWidth'].value = w;
			f['origHeight'].value = h;
			f['filename'].value = src;
			
			f['height'].value = h;
			f['width'].value = w;
		}
		function updateTextFields(formItem) {
			f = document.forms[0];
			if (f['width'].value != null && f['height'].value != null && f['useAspectRatio'].checked) {
				r = parseFloat(parseInt(f['origWidth'].value) / parseInt(f['origHeight'].value));
				if (formItem.name=='width') {
					f['height'].value = parseInt(formItem.value * (1/r));
				} else {
					f['width'].value = parseInt(formItem.value * (r));
				}
			}
		}	
		function applyRatio() {
			f = document.forms[0];
			if (f['useAspectRatio'].checked) {
				updateTextFields(f['width']);
			}	
		}
		function insertImage() {
			f = document.forms[0];
			if (window.opener) {
				baseDir = '/<%=fileDirectoryRelative%>';
				t = '<%=Request.QueryString%>';
				n = f['filename'].value;
				w = f['width'].value;
				h = f['height'].value;
				bg = f['bg'].value;
				if (f['useThumbnail'].checked) {
					window.opener.FTB_AddImage(t,'/SiteFoundry/genimage.aspx?f=' + n + '&w=' + w + '&h=' + h + '&bg=' + bg,h,w);
				} else {
					window.opener.FTB_AddImage(t,baseDir + '/' +n,h,w);
				}
			}
		}
		
		//-->
		</script>
	</head>
	<body class="gallery">
		<form id="Form1" method="post" runat="server">
			<input type="hidden" name="origHeight" />
			<input type="hidden" name="origWidth" />			
			<div align="center" style="width:100%;height:80px;background-color:#9DA4AD;border-bottom:4px solid #ddd;">
				<table border="0" cellpadding="2" cellspacing="0" width="100%">
					<tr>
						<td>
							<asp:checkbox id="useThumbnail" runat="server" text="Generate Thumbnail" Checked="false" cssclass="small" /><br>
							<asp:checkbox id="useAspectRatio" runat="server" text="Maintain Aspect Ratio" Checked="true" cssclass="small" onClick="applyRatio" />
						</td>
						<td class="small">File:<br><input type="text" name="filename" value="" style="width:100px;" /></td>
						<td class="small">Width:<br><input type="text" name="width" value="" maxlength="4" style="width:30px;" onChange="updateTextFields(this);" !onFocus="updateTextFields(this);" !onBlur="updateTextFields(this);" /></td>
						<td class="small">Height:<br><input type="text" name="height" value="" maxlength="4" style="width:30px;"  onChange="updateTextFields(this);" !onFocus="updateTextFields(this);" onBlur="updateTextFields(this);" /></td>
						<td class="small">BG:<br>#<input type="text" name="bg" value="000000" maxlength="6" style="width:50px;" /></td>
					</tr>
					<tr>
						<td class="small" align="center"><!--sort by: <a href="#">name</a> | <a href="#">size</a> | <a href="#">date</a>--></td>
						<td colspan="4" align="right">
							<input name="insert" type="button" value="insert image" onClick="insertImage();" class="button green" />
						</td>
					</tr>
				</table>
			
			</div>
			<div id="tiles" style="width:100%;height:320px;overflow:auto;" class="scrollStyle">
			
			<asp:repeater id="imageRepeater" runat="server">
				<headerTemplate>
					
				</headerTemplate>
				
				<itemTemplate>
					<span class="imgTile normal" !onMouseOver="this.className='imgTile selected';" onClick="setActiveTile(this);pickImage('<%#DataBinder.Eval(Container.DataItem,"filename") %>',<%#DataBinder.Eval(Container.DataItem,"width") %>,<%#DataBinder.Eval(Container.DataItem,"height") %>)">
						<table border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td valign="middle"><img src="<%=Dury.SiteFoundry.SFGlobal.SiteRoot+Dury.SiteFoundry.SFGlobal.ResourceFileLocation%>/<%#DataBinder.Eval(Container.DataItem,"filename") %>" height="50" width="50" style="margin:5px;"/></td>
								<td valign="top">
									<span class="small">
										<%#DataBinder.Eval(Container.DataItem,"filename") %><br>
										<%#DataBinder.Eval(Container.DataItem,"width") %>x<%#DataBinder.Eval(Container.DataItem,"height") %> (<%# getFileSize((int)DataBinder.Eval(Container.DataItem,"size")) %> KB)<br>
										<%#DataBinder.Eval(Container.DataItem,"dateModified") %>
										
									</span>
									
									
								</td>
							</tr>
						</table>
					</span>
				</itemTemplate>
				
				
				<footerTemplate>
				</footerTemplate>
			</asp:repeater>
			</div>
		</form>
	</body>
</html>
