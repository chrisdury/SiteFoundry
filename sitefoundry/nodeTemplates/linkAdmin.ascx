<%@ Control Language="c#" AutoEventWireup="false" Codebehind="linkAdmin.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.linkAdmin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table border="0" cellpadding="2" cellspacing="0" class="newItemTable">
	<tr>
		<td colspan="4" class="header">Link</td>
	</tr>
	<tr>
		<td style="vertical-align:middle;">URL:</td>
		<td><asp:textbox id="url" runat="server" width="400px" text="http://" class="inputText" /></td>
		<td><asp:checkbox id="publish" runat="server" text="publish" /></td>
		<td><asp:button id="save" runat="server" text="save" cssclass="button yellow" /></td>
	</tr>
</table>