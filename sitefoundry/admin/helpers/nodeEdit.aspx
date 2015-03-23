<%@ Page language="c#" Codebehind="nodeEdit.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.admin.helpers.nodeEdit" %>
<%@ Register TagPrefix="sf" TagName="permissionSelector" src="~/admin/modules/permissionselector.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>Node Administration</title>
	<link rel="stylesheet" href="../includes/common.css" type="text/css" />
	<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
	<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
</head>
<body class="gallery" onBeforeUnload="if(updateParent){closeEditWindow(false);}">
	<form id="nodeEdit" method="post" runat="server">
		<table class="formTable" width="100%">
			<tr>
				<td>&nbsp;</td>
				<td><asp:literal id="pageAction" runat="server" /> node</td>
			</tr>
			<tr>
				<td class="label">Filename</td>
				<td><asp:textbox id="filename" runat="server" cssclass="inputText" width="200" maxlength="30" />.aspx 
					<asp:requiredFieldValidator id="nameBlankValidator" runat="server" controlToValidate="filename"  ErrorMessage="Cannot be blank" CssClass="validate" Display="Dynamic" />
					<asp:regularexpressionvalidator id="nameRegExpValidator" runat="server" ControlToValidate="filename" ErrorMessage="invalid name" ValidationExpression="^[a-zA-Z0-9_\-]+$" Display="Dynamic" />	
				</td>
			</tr>
			<tr>
				<td class="label">Child Of</td>
				<td><asp:DropDownList ID="parentID" Runat="server" csslcass="small" width="450" /></td>
			<tr>
				<td class="label">Type</td>
				<td><asp:dropdownlist id="nodeTypeID" runat="server" cssclass="small" /></td>
			</tr>
			<tr>
				<td class="label">Page Template</td>
				<td><asp:dropdownlist id="pageTemplateID" runat="server" cssclass="small" /></td>
			</tr>
			<tr>
				<td class="label">Publish</td>
				<td><asp:checkbox id="publish_check" runat="server" Checked="false" /></td>
			</tr>
			<tr>
				<td class="label">Visible</td>
				<td><asp:checkbox id="visible_check" runat="server" Checked="true" /></td>
			</tr>
			<tr>
				<td class="label">VisibleMenu</td>
				<td><asp:checkbox id="visibleMenu_check" runat="server" Checked="false" /></td>
			</tr>
			<tr>
				<td class="label">VisibleSubMenu</td>
				<td><asp:checkbox id="visibleSubMenu_check" runat="server" Checked="false" /></td>
			</tr>			
			<tr>
				<td class="label">Labels</td>
				<td>
					<div style="border:0px solid #ccc">
						<asp:datagrid id="labelsGrid" runat="server" 
						BackColor="#f6f6f6"
						cellpadding="3"
						cellspacing="0"
						width="240"
						border="1"
						GridLines="Horizontal"
						autogeneratecolumns="false"
						itemStyle-cssClass="tableitem"
						OnEditCommand="labelsGrid_Edit"
						OnUpdateCommand="labelsGrid_Update"
						OnCancelCommand="labelsGrid_Cancel"
						OnDeleteCommand="labelsGrid_Delete"
						OnItemDataBound="labelsGrid_OnItemDataBound"
						ShowHeader="true"
						cssclass="newItemTable"
						>
						<HeaderStyle cssclass="tableheader" />
						<AlternatingItemStyle BackColor="#ffffff" />
							<columns>
								<asp:BoundColumn DataField="id" Visible="false" readonly="true" />
							
								<asp:templateColumn headerText="label">
								<itemStyle cssclass="small" />
									<itemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "name")%>
									</itemTemplate>	
									<editItemTemplate>
										<asp:textbox id="name" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "name")%>' width="120" maxlength="200" cssclass="inputText" />
									</editItemTemplate>					
								</asp:templateColumn>
								
								<asp:templateColumn headerText="lang">
								<itemStyle cssclass="small center"/>
									<itemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "lang")%>
									</itemTemplate>	
									<editItemTemplate>
										<asp:textbox id="lang" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "lang")%>' width="30" maxlength="5" cssclass="inputText" />
									</editItemTemplate>					
								</asp:templateColumn>
					
								<asp:EditCommandColumn
										EditText="<img src='../images/smalledit.gif' border='0'alt='edit' onClick='updateParent=false;' />"
										CancelText="<img src='../images/smallcancel.gif' border='0' alt='cancel' onClick='updateParent=false;' />"
										UpdateText="<img src='../images/smallsave.gif' border='0' alt='save' onClick='updateParent=false;' />"
										HeaderText="edit">
								<itemStyle cssclass="center" />
								</asp:EditCommandColumn>
						        
								<asp:templatecolumn HeaderText="delete" >
								<itemStyle cssclass="small center" />
									<itemtemplate>
										<asp:linkbutton id="deleteButton" runat="server" text="<img src='../images/smalldelete.gif' border='0'/>" commandname="Delete" />
									</itemtemplate>
									<edititemTemplate>
									</edititemTemplate>
								</asp:templatecolumn>
							</columns>
						</asp:datagrid>
						<asp:textbox id="newLabel" runat="server" cssclass="inputText" text="new label" width="136"/>
						<asp:textbox id="newLabelLang" runat="server" cssclass="inputText" width="30" maxlength="2"/>
						<asp:button id="addLabel" runat="server" text="add label" cssclass="button" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="label">Permissions</td>
				<td><sf:permissionSelector id="permissionSelector" runat="server" /></td>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<asp:button id="saveButton" runat="server" text="save" cssclass="button yellow" />
					<asp:button id="saveAndCloseButton" runat="server" text="save and close" cssclass="button yellow" />
					<asp:button id="closeButton" runat="server" text="close" cssclass="button green" />
				</td>
			</tr>
		</table>
	</form>
</body>
</html>