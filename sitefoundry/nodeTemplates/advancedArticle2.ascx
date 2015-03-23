<%@ Control Language="c#" AutoEventWireup="false" Codebehind="advancedArticle2.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.advancedArticle2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="xs" Namespace="XStandard.WebForms" Assembly="XStandard.WebForms" %>


<style type="text/css">

table.articleAdminHeader {
	background-image: url(images/teal-bg.gif);
	color:#fff;
	
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
	<table border="0" class="articleAdminHeader" cellpadding="4" cellspacing="0" !width="600">
		<tr>
			<td class="title" width="140">Article (XStandard)</td>
			<td>lang:<asp:dropdownlist id="languageSelect" runat="server" cssclass="small" /></td>
			<td align="right">
				new language:
				<asp:textbox id="newLanguage" runat="server" width="40px" cssclass="inputText" />
				<asp:button id="addLanguage" runat="server" text="add"  cssclass="button" />
				<asp:button id="deleteLanguage" runat="server" text="delete"  cssclass="button"/>
			</td>
			<td style="background-color:#fff;">&nbsp;</td>
			<!--td !width="200"><asp:checkbox id="richEditCheck" runat="server" text="rich edit" textalign="right"/></td-->
			<td style="background-color:#fff;"><span style="font-weight:bold;color:red;"><asp:literal id="msg" runat="server" /></span></td>
		</tr>
	</table>
	<table border="0" cellpadding="0" cellspacing="0" width="600" runat="server" id="articleEditHelp" visible="false">
		<tr>
			<td><br><br>
				<h1>You must add a new language for this Article.</h1>
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
		<tr !style="background-color:#e6e6e6;height:32px;">
			<td colspan="4" style="text-align:center;">
				<span style="float:left;">
					<span class="label">Page:</span> <asp:dropdownlist id="pageSelect" runat="server" cssclass="small"/> of <asp:literal id="pageCount" runat="server" />
					&nbsp;
					<asp:button id="addPage" runat="server" text="add" cssclass="button green"/>
					<asp:button id="deletePage" runat="server" text="delete" cssclass="button red" />
				</span>
				<span style="margin-left:30px;float:left;">
					<span class="label" style="padding-left:5px;">Version: </span><asp:dropdownlist id="versionSelect" runat="server" cssclass="small" />
					<asp:button id="removeVersionPage" runat="server" text="x" cssclass="button red" />
				</span>
				<span style="float:right;">
					<asp:button id="versionPage" runat="server" text="save new" cssclass="button green" />
					<asp:button id="savePage" runat="server" text="save" cssclass="button yellow" />
					<asp:button id="previewPage" runat="server" text="preview"  cssclass="button green"/>
				</span>
			</td>
		</tr>
		<tr>
			<td colspan="2"><span class="label">Title:</span><br><asp:textbox id="title" runat="server" width="350px" cssclass="inputText" tabindex="1" /></td>
			<td colspan="2">
				<span style="float:right;margin-right:10px;">
					<asp:button id="publishNowButton" runat="server" text="publish now" cssclass="button yellow" />
				</span>
				<span style="float:right;margin-right:10px;">
					<asp:checkbox id="activeCheck" runat="server" text="active" /><br/>
					<asp:checkbox id="publishCheck" runat="server" text="publish later" />
				</span>
				
				<span class="label">Template:<br/></span><asp:dropdownlist id="articleTemplateID" runat="server" cssclass="small"/>
				
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<span class="label">Summary:</span><BR><asp:textbox id="summary" runat="server" textmode="Multiline" width="350px" rows="1" cssclass="inputText" tabindex="2" />		
			</td>
			<td colspan="2" align="left" valign="top">
				<span class="label">Keywords:</span><BR><asp:textbox id="keywords" runat="server" textmode="Multiline" width="340px" rows="1" cssclass="inputText" tabindex="3" />
			</td>
		</tr>
		<tr>
			<td colspan="4">
				<span class="label">Content:</span><BR>
				<xs:XHTMLEditor id="body1" runat="server" Width="710" Height="370" EscapeUnicode="False"></xs:XHTMLEditor>
			</td>
		</tr>
	</table>
</div>
<script language="javascript">
var XSEditor = 'editor_ctl3:body1';

function insertMarkup(html) {
	//var x = $(XSEditor).Fix(html);
	$(XSEditor).InsertXML(html);
}

</script>


<script for="editor_ctl3:body1" event="ButtonClick(sButton, sState)" language="JavaScript">
	openWindow('helpers/dropdownlistlinker.aspx',620,350);
</script>