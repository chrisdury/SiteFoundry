<%@ Control Language="c#" AutoEventWireup="false" Codebehind="editFieldCheckBox.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.editFieldCheckBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h4>check box</h4>
<table border="0" cellpadding="2" cellspacing="0">
	<tr><td>title:</td><td><asp:textbox id="title" runat="server" maxlength="50" width="300"/></td></tr>
	<tr><td>description:</td><td><asp:textbox id="description" runat="server" maxlength="250" width="300" /></td></tr>
	<tr><td>options:</td><td><asp:checkBox id="isRequired" runat="server" text="is required" /></td></tr>
	<tr><td>&nbsp;</td><td><asp:button id="saveButton" runat="server" text="save" /><asp:button id="cancelButton" runat="server" text="cancel" /></td></tr>
</table>