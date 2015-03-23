<%@ Control Language="c#" AutoEventWireup="false" Codebehind="formSectionAdminListControl.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.formSectionAdminListControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:linkButton id="title" runat="server" cssclass="sectionTitle"/>
<span id="sl_<%=Section.FormSectionID%>" style="visibility:hidden;">
	<asp:linkButton id="upButton" runat="server" text="<img src=images/up.gif border=0 />" tooltip="move item up"/>
	<asp:linkButton id="dnButton" runat="server" text="<img src=images/down.gif border=0 />" tooltip="move item down"/>
	<asp:linkButton id="editButton" runat="server" text="<img src=images/smalledit.gif border=0 />" tooltip="edit item"/>
	<asp:linkButton id="deleteButton" runat="server" text="<img src=images/smalldelete.gif border=0 />" tooltip="delete item"/>
</span>