<%@ Control Language="c#" AutoEventWireup="false" Codebehind="rawHtmlAdmin.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.rawHtmlAdmin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<table border="0" cellpadding="1" cellspacing="0" width="700" class="newItemTable">
	<tr>
		<td colspan="3" class="header">Raw HTML</td>
	</tr>
	<tr>
		<td width="320">title:</td>
		<td>&nbsp;</td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td width="320"><asp:textbox id="title" runat="server" width="300px" cssclass="inputText" /></td>
		<td width="250">
			<asp:checkbox id="templateCheck" runat="server" text="show template" />&nbsp;&nbsp;&nbsp;
			<asp:checkbox id="publishCheck" runat="server" text="publish" />
		</td>
		<td width="130" align="right">
			<asp:button id="previewButton" runat="server" text="preview" cssclass="button yellow" />
			<asp:button id="saveButton" runat="server" text="save" cssclass="button yellow" />
		</td>
	</tr>
	<tr>
		<td colspan="3"><BR>content:</td>
	</tr>
	<tr>
		<td colspan="3"><asp:textbox id="content" runat="server" textmode="multiline" width="700px" rows="25" cssclass="inputTextFixedWidth" /></td>
	</tr>		
</table>