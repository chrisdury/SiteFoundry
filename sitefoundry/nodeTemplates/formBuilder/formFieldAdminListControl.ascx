<%@ Control Language="c#" AutoEventWireup="false" Codebehind="formFieldAdminListControl.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.formFieldAdminListControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<li class="<%=IsSelected()%>">
	<asp:linkButton id="title" runat="server" />
	<span id="fl_<%=FormField.FieldID%>" style="visibility:hidden;" class="controls">
		<asp:linkButton id="upButton" runat="server" text="<img src=images/up.gif border=0 />" tooltip="move item up" />
		<asp:linkButton id="dnButton" runat="server" text="<img src=images/down.gif border=0 />" tooltip="move item down" />
		<asp:linkButton id="editButton" runat="server" text="<img src=images/smalledit.gif border=0 />" tooltip="edit item"/>
		<asp:linkButton id="deleteButton" runat="server" text="<img src=images/smalldelete.gif border=0 />" tooltip="delete item"/>
	</span>
</li>