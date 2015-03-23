<%@ Page language="c#" Codebehind="documents.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.helpers.documents" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>Insert A Document &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</title>
		<link rel="stylesheet" href="../includes/common.css" type="text/css" />
		<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
		<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
		<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
		<script language="javascript">
		<!--
		
		function pickDocument(filename,size,ext) {
			f = document.forms[0];
			f["filename"].value = filename;
			f["size"].value = size;
			f["ext"].value = ext;
			f["label"].value = filename.substring(0,10);
		}
		
		function insertDocument() {
			f = document.forms[0];
			t = '<%=Request.QueryString%>';
			s = '<a href="/<%=fileDirectoryRelative%>/' + f["filename"].value + '" ';
			s += (f["newwindow"].checked) ? 'target="_blank">' : '>';
			s += (f["icon"].checked) ? getIcon(f["ext"].value) : '' ;
			s += f["label"].value;
			s += '</a> ';
			s += (f["filesize"].checked) ? getSize(f["size"].value) : '';
			
			window.opener.FTB_AddDocument(t,s);
		}


		function getSize(size) {
			return '[' + size + ' KB]';		
		}
		
		
		function getIcon(ext) {
			return '<img src="/<%=virtualDirectory%>images/icon-' + ext + '.gif" border="0" />';		
		}
		
		
		//-->
		</script>
	</head>
	<body class="gallery">
		<form id="Form1" method="post" runat="server">
			<div align="center" style="width:100%;height:60px;background-color:#9DA4AD;border-bottom:4px solid #ddd;">
				<input type="hidden" value="" name="filename" />
				<input type="hidden" value="" name="size" />
				<input type="hidden" value="" name="ext" />				
				<table border="0" cellpadding="2" cellspacing="0" width="100%">
					<tr>
						<td>
							label:<asp:textbox id="label" runat="server" width="180px" cssclass="inputText" />
						</td>
						<td align="right">
							<input type="button" name="insert" value="insert document" class="button green" onClick="insertDocument();" />
						</td>
					</tr>
					<tr>
						<td>
							<asp:checkbox id="icon" runat="server" text="use icon" cssclass="small" />
							<asp:checkbox id="newwindow" runat="server" text="open new window" cssclass="small" />
							<asp:checkbox id="filesize" runat="server" text="show filesize" cssclass="small" />
						</td>
						<td>
							&nbsp;
						</td>
					</tr>						
				</table>
			
			</div>
			<div id="tiles" style="width:100%;height:320px;overflow:auto;" class="scrollStyle">
			
			<asp:repeater id="documentRepeater" runat="server">
				<headerTemplate>
					
				</headerTemplate>
				
				<itemTemplate>
					<span class="imgTile normal" onClick="setActiveTile(this);pickDocument('<%#DataBinder.Eval(Container.DataItem,"filename") %>',<%#getFileSize((int)DataBinder.Eval(Container.DataItem,"size"))%>,'<%#DataBinder.Eval(Container.DataItem,"type") %>');">
						<table border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td><img src="../../images/icon-<%#DataBinder.Eval(Container.DataItem,"type") %>.gif" style="margin:5px;" /></td>
								<td valign="top">
									<span class="small">
										<%# trimName((string)DataBinder.Eval(Container.DataItem,"filename")) %><br>
										<%# DataBinder.Eval(Container.DataItem,"dateModified") %><br>
										<%# getFileSize((int)DataBinder.Eval(Container.DataItem,"size")) %> KB
										
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
