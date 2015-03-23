<%@ Control Language="c#" AutoEventWireup="false" Codebehind="permissionSelector.ascx.cs" Inherits="Dury.SiteFoundry.Admin.permissionSelector" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:datagrid id="rolesGrid" runat="server" 
	BackColor="#f6f6f6"
	cellpadding="3"
	cellspacing="0"
	width="450"
	border="1"
	GridLines="Horizontal"
	autogeneratecolumns="false"
	itemStyle-cssClass="tableitem"
	ShowHeader="true"
	cssclass="newItemTable"
	>
	<HeaderStyle cssclass="tableheader" />
	<AlternatingItemStyle BackColor="#ffffff" />
	<columns>
		<asp:BoundColumn DataField="id" Visible="false" readonly="true" />
	
		<asp:templateColumn headerText="role">
		<itemStyle cssclass="small" />
			<itemTemplate>
				<asp:dropdownlist id="rolesList" runat="server" />
			</itemTemplate>	
		</asp:templateColumn>
		
		<asp:templateColumn headerText="permissions">
		<itemStyle cssclass="small center"/>
			<itemTemplate>
				<asp:checkbox id="delete" runat="server" text="delete"  />
				<asp:checkbox id="publish" runat="server" text="publish" />
				<asp:checkbox id="edit" runat="server" text="edit" />
				<asp:checkbox id="add" runat="server" text="add" />
				<asp:checkbox id="view" runat="server" text="view" />
			</itemTemplate>	
		</asp:templateColumn>
							
		<asp:templatecolumn HeaderText="save">
		<itemStyle cssclass="small center" />
			<itemtemplate>
				<asp:linkbutton id="saveButton" runat="server" text='<img src="../images/smallsave.gif" border="0" />' commandName="Save" commandArgument='<%# DataBinder.Eval(Container.DataItem,"Id")%>'/>
			</itemtemplate>
		</asp:templatecolumn>
		
		<asp:templatecolumn HeaderText="delete">
		<itemStyle cssclass="small center" />
			<itemtemplate>
				<asp:linkbutton id="deleteButton" runat="server" text='<img src="../images/smalldelete.gif" border="0" />' commandName="Delete" commandArgument='<%# DataBinder.Eval(Container.DataItem,"Id")%>' /></td>
			</itemtemplate>
		</asp:templatecolumn>
	</columns>
</asp:datagrid>
<table border="0" cellpadding="2" cellspacing="0" width="450">
	<tr>
		<td colspan="3"><h3>add new role</h3></td>
	</tr>
	<tr>
		<td><asp:dropdownlist id="rolesList" runat="server" /></td>
		<td>
			<asp:checkbox id="delete" runat="server" text="delete"  />
			<asp:checkbox id="publish" runat="server" text="publish" />
			<asp:checkbox id="edit" runat="server" text="edit" />
			<asp:checkbox id="add" runat="server" text="add" />
			<asp:checkbox id="view" runat="server" text="view" />
		</td>
		<td><asp:button id="addButton" runat="server" text="add role" cssclass="button green" /></td>
	</tr>
	<tr>
		<td colspan="3"><asp:literal id="msg" runat="server" /></td>
	</tr>
</table>
