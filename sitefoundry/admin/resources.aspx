<%@ Page language="c#" Codebehind="resources.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.resources" %>
<form id="resources" method="post" runat="server">
	<span class="leftcolumn">
		<h2>Tools</h2>
		<div class="toolPanel">
			<div class="title">Upload File(s)</div>
			<div class="content">
				<p><asp:dropdownlist id="uploadFileDirectorySelect" runat="server" cssclass="resourcesFileSelect" /></p>
				<p><input type="file" runat="server" id="fileToUpload1" class="" NAME="fileToUpload" style="width:200px;" /></p>
				<p><input type="file" runat="server" id="File1" class="" NAME="fileToUpload" style="width:200px;" /></p>
				<p><input type="file" runat="server" id="File2" class="" NAME="fileToUpload" style="width:200px;" /></p>
				<p><input type="file" runat="server" id="File3" class="" NAME="fileToUpload" style="width:200px;" /></p>
				<p><asp:button id="upload" runat="server" text="Upload File(s)" onClick="upload_click" /></p>
			</div>
		</div>
		
		<div class="toolPanel">
			<div class="title">Create Directory</div>
			<div class="content">
				<p>new directory:<asp:textbox id="newDirectoryName" runat="server" width="120" cssclass="small" /></p>
				<p><asp:button id="newDirectoryButton" runat="server" text="create" /></p>
				<asp:label id="newDirectoryMsg" runat="server" />
			</div>
		</div>
		
		<div class="toolPanel">
			<div class="title">Move Selected File(s)</div>
			<div class="content">
				<p><asp:dropdownlist id="moveFilesDirectorySelect" runat="server"  cssclass="resourcesFileSelect"></asp:dropdownlist></p>
				<p><asp:button id="moveFilesButton" runat="server" text="move files" /></p>
				<asp:label id="moveFilesMsg" runat="server" />
			</div>
		</div>
		
		<div class="toolPanel">
			<div class="title">Delete Selected File(s)</div>
			<div class="content">
				<p><asp:button id="deleteFilesButton" runat="server" text="delete files" /></p>
				<asp:label id="deleteMsg" runat="server" />
			</div>
		</div>
		
		<div class="toolPanel">
			<div class="title">Delete Directory</div>
			<div class="content">
				<p><asp:button id="deleteDirectoryButton" runat="server" text="delete directory" /></p>
			</div>
		</div>
		
		<asp:label id="msg" runat="server" cssclass="small" />
	</span>
	
	<span class="rightcolumn">
		<div class="articleMenu">
			<table border="0" cellpadding="0" cellspacing="0" width="700">
				<tr>
					<td><h2>Current Directory &raquo; <asp:dropdownlist id="directorySelect" runat="server" cssclass="small" autopostback="true" /></h2></td>
					<td><strong>select</strong> <a href="javascript:selectCheckboxes('all','files');">all</a> | <a href="javascript:selectCheckboxes('none','files');">none</a> | <a href="javascript:selectCheckboxes('invert','files');">invert</a></td>
					<td align="right">filter by file type: <asp:dropdownlist id="fileFilter" runat="server" cssclass="small" autopostback="true" onselectedindexchanged="fileFilter_changed"/></td>
				</tr>	
			</table>				
		</div>
		<div class="resourcesFileDisplay">
			<asp:repeater id="filesRepeater" runat="server">
				<headerTemplate>
					<div class="resourcesFileContainer">
				</headerTemplate>
				
				<itemTemplate>
					<div class="resourcesFileItem">
						<div class="checkbox"><input type="checkbox" id="<%# DataBinder.Eval(Container.DataItem,"filename") %>" value="<%# DataBinder.Eval(Container.DataItem,"filename") %>" name="files" ></div>
						<table><tr><td>
							<a href="<%# getRelativeUrl((string)DataBinder.Eval(Container.DataItem,"filename")) %>" target="_blank" ><asp:placeHolder id="previewHolder" runat="server" /></a>
						</td><tr></table>
						<p><a href="<%# getRelativeUrl((string)DataBinder.Eval(Container.DataItem,"filename")) %>" target="_blank" title="<%# DataBinder.Eval(Container.DataItem,"filename")%>" ><%# shortenName((string)DataBinder.Eval(Container.DataItem,"filename")) %></a></p>
					</div>
				</itemTemplate>
				
				<footerTemplate>
					</div>
				</footerTemplate>			
			</asp:repeater>			
		</div>
		<div class="resourcesPagination">
			<div style="float:right;">page: <asp:dropdownlist id="paginationList" runat="server" cssclass="small" /></div>
			<asp:literal id="stats" runat="server" />
		</div>
	</span>
</form>
