<%@ Control Language="c#" AutoEventWireup="false" Codebehind="defaultHeader.ascx.cs" Inherits="Dury.SiteFoundry.siteTemplates.defaultHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
  <head>
    <title><%=Page.MainTitle%> : <%=Page.PageTitle%></title>
	<link rel="stylesheet" href="<%=siteRoot%>includes/main.css" type="text/css" />
  </head>
<body>
<table border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top" width="160">
			<asp:literal id="menu" runat="server" />
		</td>
		<td valign="top">