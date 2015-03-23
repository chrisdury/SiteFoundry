<%@ Control Language="c#" AutoEventWireup="false" Codebehind="header.ascx.cs" Inherits="Dury.SiteFoundry.Admin.modules.header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%
	string activeClass = String.Empty;

%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<asp:literal id="userToken" runat="server" />
<html>
	<head>
		<title><%=Page.MainTitle%> : <%=Page.PageTitle%></title>
		<link rel="stylesheet" href="includes/common.css" type="text/css" />
		<link rel="stylesheet" href="includes/admin.css" type="text/css" />
		<link rel="stylesheet" href="includes/forms.css" type="text/css" />
		<link rel="stylesheet" href="includes/ajax_menu.css" type="text/css" />
		<script language="javascript" type="text/javascript" src="includes/prototype.js"></script>
		<script language="javascript" type="text/javascript" src="includes/querystring.js"></script>
		<script language="javascript" type="text/javascript" src="includes/main.js"></script>
	</head>
	<body class="admin">
	<% if (Page.showMenu) { %>
		<div class="header"><div id="userInfo">logged in as: <asp:literal id="username" runat="server" /></div><a href="default.aspx" ><img src="images/admin_title.gif" border="0" /></a></div>
		<div class="mainmenu">
			<asp:repeater id="menuRepeater" runat="server">
				<itemTemplate>
					<a href="<%# DataBinder.Eval(Container.DataItem, "href")%>" <%# isActive((string)DataBinder.Eval(Container.DataItem, "href"))%>><%# DataBinder.Eval(Container.DataItem, "label") %></a>
				</itemTemplate>
			</asp:repeater>	
		</div>
	<% } %>
		<span class="sectionbody">