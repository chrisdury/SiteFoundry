<%@ Control Language="c#" AutoEventWireup="false" Codebehind="articleAdmin2.ascx.cs" Inherits="sitefoundry.nodeTemplates.articleAdmin2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="CTB" Namespace="HolmesAndLee.SiteFoundry.UI" Assembly="sitefoundry" %>

<!--
PROCESSING RULES:

- load article with publish bit set or greatest version number if no article selected for publish

- when user clicks preview, a new entry is made in database, marked as preview and tied to the user account
- when user saves, version is incremented

//
- user rules are enabling content flexibility, not security -- frees users from cumbersome interfaces to make changes
needed and move on

- users can save over other users work, if their role has permission
//


contexts:

1. Creating new article
	- add article container + page
	
2. Editing existing page
	- preview creates temp entry in db
	- save creates new entry w/ incremented versionNumber

3. Add new page
	- create new entry in db w/ new version
	
4. delete page
	- delete page and all versions
	
5. delete article 
	- delete everything


-->




<style type="text/css">

table.articleAdminHeader {
	background-color:#ddd;
}
table.articleAdminHeader td {
	font-size:11px;
}

table.articleAdminHeader td.title {
	font-size:14px;
	font-weight:bold;
}

table.articleAdmin {
	background-color:#eee;
}

table.articleAdmin td {
	/*padding:2px;*/
}



</style>
<div id="articleEditContainer" runat="server" visible="false">
	<table border="0" class="articleAdminHeader" cellpadding="4" cellspacing="0">
		<tr>
			<td class="title" width="120">Article</td>
			<td>lang:<asp:dropdownlist id="languageSelect" runat="server" cssclass="small" /></td>
			<td align="right">
				new language:
				<asp:textbox id="newLanguage" runat="server" width="40px" cssclass="inputText" />
				<asp:button id="addLanguage" runat="server" text="add"  cssclass="button" />
				<asp:button id="deleteLanguage" runat="server" text="delete"  cssclass="button"/>
			</td>
			<td><a href="#">?</a></td>
		</tr>
	</table>
	<table border="0" cellpadding="0" cellspacing="0" width="600" runat="server" id="articleEditHelp" visible="false">
		<tr>
			<td><br><br>
				<h1>You must add a new Article.</h1>
				<p>Add a language by specifying it's 4 character language code. (ie. 'xx-XX')</p>
				<p>examples:</p>			
				<ul>
					<li>en-CA</li>
					<li>fr-CA</li>
					<li>en-US</li>
				</ul>
			</td>
		</tr>
	</table>
</div>
<div id="bodyEditContainer" runat="server" visible="false">
	<table border="0" class="articleAdmin" width="720" cellpadding="3" cellspacing="0">
		<tr style="background-color:#e6e6e6;height:32px;">
			<td width="200">
				<span class="label">Page:</span> <asp:dropdownlist id="pageSelect" runat="server" cssclass="small"/> of <asp:literal id="pageCount" runat="server" />
				&nbsp;
				<asp:button id="addPage" runat="server" text="add" cssclass="button green"/>
				<asp:button id="deletePage" runat="server" text="delete" cssclass="button red" />
			</td>
			<td align="left" colspan="2" width="320">
				<span class="label" style="padding-left:5px;">Version: </span><asp:dropdownlist id="versionSelect" runat="server" cssclass="small" />
				<asp:button id="removeArchivePage" runat="server" text="x" cssclass="button red" />
			</td>
			<td align="right" colspan="1" width="190">
				<asp:button id="archivePage" runat="server" text="save new" cssclass="button green" />
				<asp:button id="savePage" runat="server" text="save" cssclass="button yellow" />
				<asp:button id="previewPage" runat="server" text="preview"  cssclass="button green"/>
			</td>
		</tr>
		<tr>
			<td colspan="2"><span class="label">Title:</span><br><asp:textbox id="title" runat="server" width="350px" cssclass="inputText" tabindex="1" /></td>
			<td colspan="2">
				<span style="float:right;margin-top:10px;margin-right:10px;"><asp:checkbox id="richEditCheck" runat="server" text="rich edit" />&nbsp;&nbsp;<asp:checkbox id="publishCheck" runat="server" text="publish" /></span>
				<span class="label">Template:<br/></span><asp:dropdownlist id="articleTemplateID" runat="server" cssclass="small"/>
				
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<span class="label">Summary:</span><BR><asp:textbox id="summary" runat="server" textmode="Multiline" width="350px" rows="2" cssclass="inputText" tabindex="2" />		
			</td>
			<td colspan="2" align="left" valign="top">
				<span class="label">Keywords:</span><BR><asp:textbox id="keywords" runat="server" textmode="Multiline" width="340px" rows="2" cssclass="inputText" tabindex="3" />
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<span class="label">Content:</span><BR>
				<CTB:CustomFreeTextBox tabindex="4" width="710" id="content" runat="Server" />
			</td>
		</tr>
	</table>
</div>