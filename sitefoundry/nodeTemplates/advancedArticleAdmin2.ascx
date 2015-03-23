<%@ Control Language="c#" AutoEventWireup="false" Codebehind="advancedArticleAdmin2.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.advancedArticleAdmin2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="articleTools">
	<h2><span style="float:right"><a href="javascript:toggleTools();">&raquo;</a></span> tools </h2>
	<table border="1" cellpadding="0" cellspacing="0" width="99%" id="articleToolsTable">
		<tr>
			<td>
				<h3>languages</h3>
				<p>
					current: <asp:dropdownlist id="currentLanguageSelect" runat="server" />
					new language: <asp:textbox id="newLanguageText" runat="server" width="20" /> <asp:button id="newLanguageButton" runat="server" text="add" />
				</p>
			</td>
			<td>
				<h3>pages</h3>
				<p>
					current page: <asp:dropdownlist id="currentPageSelect" runat="server" />
					<asp:button id="addPageButton" runat="server" text="add" /> <asp:button id="deletePageButton" runat="server" text="delete" />
				</p>
			</td>
			<td>
				<h3>versions</h3>
				<p>
					current version: <asp:dropdownlist id="versionSelect" runat="server" />
					<asp:button id="deleteVersionButton" runat="server" text="x" />
				</p>
			</td>
		</tr>
		<tr>
			<td>
				<h3>options</h3>
				<p>	
					<asp:button id="publishNowButton" runat="server" text="publish now" />
					<asp:checkbox id="publishCheck" runat="server" text="publish" />
					<asp:checkbox id="activeCheck" runat="server" text="active" />
				</p>
			</td>
			<td>
				<h3>templates</h3>
				<p><asp:dropdownlist id="pageTemplateSelect" runat="server" /></p>
			</td>
			<td>
				<h3>content editor</h3>
				<p><asp:dropdownlist id="contentEditorList" runat="server" /></p>
			</td>
		</tr>
	</table>
	<div style="text-align:center;">
		<p>
			<asp:button id="saveButton" runat="server" text="save" />
			<asp:button id="saveNewVersionButton" runat="server" text="save new version" />
		</p>
	</div>
</div>
<div id="articleToolsHiddenLink">
	<a href="javascript:toggleTools();">&laquo;</a>
</div>



<div id="articleContent">
	<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td>
				<h3>title</h3>
				<asp:textbox id="title" runat="server" width="200" />
			</td>
			<td>
				<h3>summary</h3>
				<asp:textbox id="summary" runat="server" width="200" textmode="multiline" />
			</td>
			<td>
				<h3>keywords</h3>
				<asp:textbox id="keywords" runat="server" width="200" textmode="multiline" />
			</td>
		</tr>
		<tr>
			<td colspan="3">
				<asp:placeholder id="contentHolder" runat="server" />
			</td>
		</tr>
	</table>
</div>







