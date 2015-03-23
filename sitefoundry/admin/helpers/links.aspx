<%@ Page language="c#" Codebehind="links.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.helpers.links" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>Insert An Internal Link &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</title>
		<link rel="stylesheet" href="../includes/common.css" type="text/css" />
		<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
		<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
		<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
		<script language="javascript">
		<!--
		
		function pickLink(url,label) {
			f = document.forms[0];
			
			f['url'].value = url;
			f['label'].value = label;
		}
		
		
		
		function insertLink() {
			f = document.forms[0];
			t = '<%=Request.QueryString%>';
			/*
			s = '<a href="' + f['url'].value + '" ';
			s += (f["newwindow"].checked) ? 'target="_blank">' : '>';
			s += f['label'].value + '</a>';			
			*/
			start = '<a href="' + f['url'].value + '" ';
			start += (f["newwindow"].checked) ? 'target="_blank">' : '>';
			end = "</a>";
			if (window.opener) {
				//window.opener.FTB_AddLink(t,s);
				window.opener.FTB_AddLink(t,start,end);
			}
		}
		
		
		
		//-->
		</script>
	</head>
	<body class="gallery">
		<form id="Form1" method="post" runat="server">
			<input type="hidden" name="url" />
			<div !align="center" style="width:100%;height:50px;background-color:#9DA4AD;border-bottom:4px solid #ddd;">
				<table border="0" cellpadding="2" cellspacing="0" width="100%">
					<tr>
						<td>
							label:<asp:textbox id="label" runat="server" width="180px" cssclass="inputText" />
						</td>
						<td align="right">
							<input type="button" name="insert" value="insert link" class="button green" onClick="insertLink();" />
						</td>
					</tr>
					<tr>
						<td>
							<asp:checkbox id="newwindow" runat="server" text="open new window" cssclass="small" />
						</td>
						<td>
							&nbsp;
						</td>
					</tr>						
				</table>
			
			</div>
			<div id="tiles" style="width:100%;height:348px;overflow:auto;padding-top:10px;padding-left:20px;" class="scrollStyle" onClick="setActiveLink(null);">
				<asp:repeater id="linksRepeater" runat="server" >
					<itemTemplate>
						<a href="#" onClick="setActiveLink(this);event.cancelBubble=true;pickLink('<%# DataBinder.Eval(Container.DataItem, "href") %>','<%#escapeQuotes((string)DataBinder.Eval(Container.DataItem, "label"))%>')" 
						title="<%# DataBinder.Eval(Container.DataItem, "href") %>" 
						style="margin-left:<%#addMargin((int)DataBinder.Eval(Container.DataItem, "indent"))%>px;" 
						class="normal"				
						>
						<%# DataBinder.Eval(Container.DataItem, "label") %></a><br>
					</itemTemplate>
				</asp:repeater>
			</div>
		</form>
	</body>
</html>