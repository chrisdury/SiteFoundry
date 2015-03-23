<%@ Page language="c#" Codebehind="users.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.users" %>
<form id="users" method="post" runat="server">

	<span class="leftcolumn">
		<div class="folderMenu">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>Roles</td>
					<td align="right">
						
					</td>
				</tr>
			</table>
		</div>
		<asp:datagrid id="rolesGrid" runat="server"
			BackColor="#f6f6f6"
			cellpadding="2"
			cellspacing="0"
			width="200"
			border="1"
			GridLines="Horizontal"
			autogeneratecolumns="false"
			ItemStyle-VerticalAlign="Middle"
			itemStyle-cssClass="tableitem"
			OnEditCommand="rolesGrid_Edit"
			OnUpdateCommand="rolesGrid_Update"
			OnCancelCommand="rolesGrid_Cancel"
			OnDeleteCommand="rolesGrid_Delete"
			OnItemDataBound="rolesGrid_OnItemDataBound"
			>
			<PagerStyle backColor="#92A5B0" ForeColor="#ffffff" Mode="NumericPages" PageButtonCount="10" cssclass="pager" horizontalAlign="right" NextPageText="next" PrevPageText="prev"/>
			<HeaderStyle cssclass="tableheader" BackColor="#9DA4AD" ForeColor="#333"/>
			<AlternatingItemStyle BackColor="#ffffff" />
			<EditItemStyle backcolor="#F7CECE" />
			<Columns>
				<asp:BoundColumn DataField="id" HeaderText="id" Visible="true" readonly="true" SortExpression="id">
				<itemStyle cssclass="small"/>
				</asp:boundcolumn>
			
				<asp:templateColumn headerText="Roles" sortexpression="type">
				<itemStyle cssclass="small" width="99%" />
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Name")%>
					</itemTemplate>	
					<editItemTemplate>
						<asp:textbox id="name" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Name")%>' width="100" cssclass="textbox" />
					</editItemTemplate>					
				</asp:templateColumn>
				
				<asp:templateColumn headerText="P" sortexpression="publish">
				<itemStyle cssclass="small" />
					<itemTemplate>
						<asp:placeHolder id="publishViewHolder" runat="server" />
					</itemTemplate>	
					<editItemTemplate>
						<asp:checkbox id="publishCheck" runat="server" />
					</editItemTemplate>					
				</asp:templateColumn>
				
				<asp:EditCommandColumn
						EditText="<img src='images/smalledit.gif' border='0'alt='edit'/>"
						CancelText="<img src='images/smallcancel.gif' border='0' alt='cancel'/>"
						UpdateText="<img src='images/smallsave.gif' border='0' alt='save' />"
						HeaderText="edit">
				<itemStyle cssclass="center" />
				</asp:EditCommandColumn>
		        
				<asp:templatecolumn HeaderText="delete" >
				<itemStyle cssclass="small center" />
					<itemtemplate>
						<asp:linkbutton id="deleteButton" runat="server" text="<img src='images/smalldelete.gif' border='0'/>" commandname="Delete" />
					</itemtemplate>
					<edititemTemplate>
					</edititemTemplate>
				</asp:templatecolumn>
			</Columns>		
		</asp:datagrid>		
		<br>
		<table border="0" width="200" cellpadding="0" cellspacing="0" class="newItemTable" runat="server" id="newRoleTable">
			<tr>
				<td colspan="1" class="header">New Role</td>
			</tr>
			<tr>
				<td>
					<asp:textbox id="newRoleName" runat="server" width="80" cssclass="textbox" />
					<asp:button id="addNewRole" runat="server" text="add new" cssclass="button green" />				
				</td>
			</tr>
		</table>
	</span>
	
	
	<span class="rightcolumn" !style="border:1px solid black;">
		<div class="articleMenu">
			<table border="0" cellpadding="0" cellspacing="0" width="700">
				<tr>
					<td>Users</td>
				</tr>	
			</table>				
		</div>
		<asp:datagrid id="userGrid" runat="server"
			BackColor="#f6f6f6"
			cellpadding="4"
			cellspacing="0"
			width="700"
			border="1"
			GridLines="Horizontal"
			autogeneratecolumns="false"
			ItemStyle-VerticalAlign="Middle"
			itemStyle-cssClass="tableitem"
			OnEditCommand="userGrid_Edit"
			OnUpdateCommand="userGrid_Update"
			OnCancelCommand="userGrid_Cancel"
			OnDeleteCommand="userGrid_Delete"
			OnItemDataBound="userGrid_OnItemDataBound"
			>
			<PagerStyle backColor="#92A5B0" ForeColor="#ffffff" Mode="NumericPages" PageButtonCount="10" cssclass="pager" horizontalAlign="right" NextPageText="next" PrevPageText="prev"/>
			<HeaderStyle cssclass="tableheader" />
			<AlternatingItemStyle BackColor="#ffffff" />
			<EditItemStyle backcolor="#F7CECE" />
			<Columns>
				<asp:BoundColumn DataField="id" HeaderText="id" Visible="false" readonly="true" SortExpression="id">
				<itemStyle cssclass="small"/>
				</asp:boundcolumn>

				<asp:templateColumn headerText="username" sortexpression="username">
				<itemStyle  cssclass="small"/>
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "username")%>
					</itemTemplate>	
					<editItemTemplate>
						username<br><asp:textbox id="username" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "username")%>' width="80" cssclass="textbox" />
						<br>
						password<br><asp:textbox id="password" runat="server" text="" width="80"  cssclass="textbox" />
						</editItemTemplate>					
				</asp:templateColumn>
				
				
				<asp:templateColumn headerText="fullname" sortexpression="fullname">
				<itemStyle  cssclass="small"/>
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "fullname")%>
					</itemTemplate>	
					<editItemTemplate>
						full name<br><asp:textbox id="fullname" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "fullname")%>' width="100"  cssclass="textbox" />
					</editItemTemplate>					
				</asp:templateColumn>
				
				<asp:templateColumn headerText="email" sortexpression="email">
				<itemStyle  cssclass="small"/>
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "email")%>
					</itemTemplate>	
					<editItemTemplate>
						email address<br><asp:textbox id="email" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "email")%>' width="120"  cssclass="textbox" />
					</editItemTemplate>					
				</asp:templateColumn>
				
				<asp:templateColumn headerText="roles" sortexpression="">
				<itemStyle  cssclass="small"/>
					<itemTemplate>
						<asp:dropdownlist id="rolesDisplay" runat="server" cssclass="small" width="150"/>
					</itemTemplate>
					<editItemTemplate>
						roles<br>
						<asp:listbox id="rolesList" runat="server" rows="4" selectionmode="multiple"/>
					</editItemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headerText="disabled" sortexpression="disabled">
				<itemStyle cssclass="small"/>
					<itemTemplate>
						<asp:literal id="disabledText" runat="server" />
					</itemTemplate>
					<editItemTemplate>
						<asp:checkbox id="disabledCheck" runat="server" text="disabled" />
					</editItemTemplate>
				</asp:templateColumn>
				
				
				
				<asp:templateColumn headerText="last login" sortexpression="lastlogin" visible="false">
				<itemStyle  cssclass="small"/>
					<itemTemplate>
						<%# getHowLongAgo((DateTime)DataBinder.Eval(Container.DataItem, "lastlogin"))%>
					</itemTemplate>	
				</asp:templateColumn>
			
				
				<asp:EditCommandColumn
						EditText="<img src='images/edit.gif' border='0'alt='edit'/>"
						CancelText="<img src='images/cancel.gif' border='0' alt='cancel'/>"
						UpdateText="<img src='images/save.gif' border='0' alt='save' />"
						HeaderText="edit">
				<itemStyle cssclass="center" />
				</asp:EditCommandColumn>
		        
				<asp:templatecolumn HeaderText="delete" visible="true" >
				<itemStyle cssclass="small center" />
					<itemtemplate>
						<asp:linkbutton id="deleteButton" runat="server" text="<img src='images/delete.gif' border='0'/>" commandname="Delete" />
					</itemtemplate>
					<edititemTemplate>
					</edititemTemplate>
				</asp:templatecolumn>
			</Columns>		
		</asp:datagrid>
		<br>
		<table border="0" cellpadding="0" cellspacing="0" width="700" class="newItemTable" runat="server" id="newUserTable">
			<tr>
				<td colspan="4" class="header">New User</td>
			</tr>
			<tr>
				<td>username</td>
				<td>password</td>
				<td>roles</td>
				<td rowspan="4" style="vertical-align:middle;"><asp:button id="addNewUserButton" runat="server" text="add new user" cssclass="button green" /></td>
			</tr>
			<tr>
				<td><asp:textbox id="newUserName" runat="server" width="120"  cssclass="textbox" /></td>
				<td><asp:textbox id="newUserPassword" runat="server" width="120"  cssclass="textbox" /></td>				
				<td rowspan="3"><asp:listbox id="newUserRoles" runat="server" rows="4" selectionmode="multiple"/></td>
			</tr>
			<tr>
				<td>fullname</td>
				<td>email</td>
			</tr>
			<tr>
				<td><asp:textbox id="newUserFullName" runat="server" width="120"  cssclass="textbox" /></td>
				<td><asp:textbox id="newUserEmail" runat="server" width="160" cssclass="textbox" /></td>
			</tr>
		</table>
		<br>
		<asp:literal id="msg" runat="server" />			
	</span>
</form>
