<%@ Page language="c#" Codebehind="cms.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.cms" %>
<form id="cms" method="post" runat="server">
	<span class="leftcolumn">
		<asp:placeholder id="menuHolder" runat="server" />
	</span>
	<span class="rightcolumn" !style="border:1px solid black;" !onmouseover="hideNodeMenu();">
		<asp:placeHolder id="nodeTemplateHolder" runat="server" />
		<!--iframe id="nodeTemplateHolder" ru nat="server" scrolling="auto" frameborder="0" marginwidth="0" marginheight="0" leftmargin="0" topmargin="0"></iframe-->
	</span>
</form>

