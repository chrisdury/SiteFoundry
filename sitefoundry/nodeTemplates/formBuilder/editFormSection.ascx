<%@ Control Language="c#" AutoEventWireup="false" Codebehind="editFormSection.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.editFormSection" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h4>form section</h4>
<table border="0" cellpadding="2" cellspacing="0">
	<tr><td>title:</td><td><asp:textbox id="title" runat="server" maxlength="50" width="300"/></td></tr>
	<tr><td>description:</td><td><asp:textbox id="description" runat="server" maxlength="250" width="300" /></td></tr>
	<tr><td>&nbsp;</td><td><asp:button id="saveButton" runat="server" text="save" /><asp:button id="cancelButton" runat="server" text="cancel" /></td></tr>
</table>