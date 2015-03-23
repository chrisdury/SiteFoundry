<%@ Control Language="c#" AutoEventWireup="false" Codebehind="editFieldPassword.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.editFieldPassword" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h4>password</h4>
<table border="0" cellpadding="2" cellspacing="0">
	<tr><td>title:</td><td><asp:textbox id="title" runat="server" maxlength="50" width="300"/></td></tr>
	<tr><td>description:</td><td><asp:textbox id="description" runat="server" maxlength="250" width="300" /></td></tr>
	<tr><td>width:</td><td><asp:textbox id="width" runat="server" width="30" maxlength="3" /></td></tr>
	<tr><td>max length:</td><td><asp:textbox id="maxLength" runat="server" width="40" maxlength="5"/></td></tr>
	<tr><td>options:</td><td><asp:checkBox id="isRequired" runat="server" text="is required" /><asp:checkBox id="isValidated" runat="server" text="is validated"/></td></tr>
	<tr><td>validation expression</td><td><asp:textbox id="validationExpression" runat="server" width="300"/></td></tr>
	<tr><td>&nbsp;</td><td><asp:button id="saveButton" runat="server" text="save" /><asp:button id="cancelButton" runat="server" text="cancel" /></td></tr>
</table>