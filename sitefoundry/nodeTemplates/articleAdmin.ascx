<%@ Control Language="c#" AutoEventWireup="false" Codebehind="articleAdmin.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.articleAdmin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="CTB" Namespace="Dury.SiteFoundry.UI" Assembly="SiteFoundry" %>
<script language="Javascript">
	function trapEnter() {
		if (document.all) {
			if (event.keyCode == 13) {
				event.returnValue=false;
				event.cancel=true;
				document.getElementById("_ctl2_savePage").click();			
			}
		}
	}
</script>



<table border="0" cellpadding="0" cellspacing="0" width="500" style="background-image:url(images/teal-bg.gif);" id="articleLangTable" runat="server">
	<tr>
		<td><img src="images/teal-left.gif"></td>
		<td style="height:30px;"><span class="title">Article</span></td>
		<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
		<td><span class="small">Language:</span></td>
		<td><asp:dropdownlist id="languageSelect" runat="server" cssclass="small" /></td>
		<td align="right">
			<asp:textbox id="newLang" runat="server" width="30px" maxlength="5" cssclass="inputText"/>
			<asp:button id="addLang" runat="server" text="add" cssclass="button green" />
			<asp:button id="deleteLang" runat="server" text="delete" cssclass="button red" />
		</td>
		<td><img src="images/teal-right.gif" border="0" hspace="0" align="right"></td>
		<td style="background:#fff;">&nbsp;&nbsp;&nbsp;</td>
		<td style="background:#fff;" class="small"><asp:literal id="msg" runat="server" /></td>		
		<td style="background:#fff;"><nobr><asp:linkbutton id="expandButton" text="large view" runat="server" Visible="false" /></nobr></td>
	</tr>
</table>
<table border="0" cellpadding="0" cellspacing="0" width="600" id="articleInsertHelpTable" runat="server" Visible="false">
	<tr>
		<td><br><br>
			<h1>You must add a language indentifier to this article</h1>
			<p>Add a language by specifying it's 2 or 4 character language code. (ie. 'xx' or 'xx-XX')</p>
			<p>examples</p>			
			<ul>
				<li>en</li>
				<li>fr</li>
				<li>en-CA</li>
				<li>fr-CA</li>
				<li>en-US</li>
			</ul>
		</td>
	</tr>
</table>


<table border="0" cellpadding="2" cellspacing="0" width="700" runat="server" id="articleMainTable" style="border:1px solid #ccc;background-color:#eeeeee;">
	<tr>
		<td>
			<!--
			<span class="label">Current Page:</span> <asp:dropdownlist id="pageSelect" runat="server" cssclass="small"/> of <asp:literal id="pageCount" runat="server" />
			&nbsp;
			<asp:button id="addPage" runat="server" text="add page" cssclass="button green"/>
			<asp:button id="deletePage" runat="server" text="delete" cssclass="button red" />
			-->
		</td>
		<td align="right" colspan="3">
			
			<asp:button id="savePage" runat="server" text="save" cssclass="button yellow" />
			<asp:button id="previewPage" runat="server" text="preview"  cssclass="button yellow"/>
			
		</td>
	</tr>
	<tr>
		<td><span class="label">Title:</span><br><asp:textbox id="title" runat="server" width="350px" cssclass="inputText" tabindex="1" /></td>
		<td><span class="label">Template:</span><br>
			<asp:dropdownlist id="articleTemplateID" runat="server" cssclass="small"/>
		</td>
		<td><!--<span class="label">Status:</span><br><asp:dropdownlist id="articleStatusID" runat="server" cssclass="small" />--></td>
		<td align="right" valign="bottom"><asp:checkbox id="publishCheck" runat="server" text="publish" /></td>
	</tr>
	<tr>
		<td>
			<span class="label">Summary:</span><BR><asp:textbox id="summary" runat="server" textmode="Multiline" width="350px" rows="2" cssclass="inputText" tabindex="2" />		
		</td>
		<td  colspan="3" align="left" valign="top">
			<span class="label">Keywords:</span><BR><asp:textbox id="keywords" runat="server" textmode="Multiline" width="340px" rows="2" cssclass="inputText" tabindex="3" />
		</td>
	</tr>
	<tr>
		<td colspan="5">
			<span class="label">Content:</span><BR>
			<CTB:CustomFreeTextBox tabindex="4" width="400" height="440" id="content" runat="Server"/>
		</td>
	</tr>
</table>




<table border="0" cellpadding="0" cellspacing="0" width="700" runat="server" id="addArticleTable" Visible="false">
	<tr>
		<td>
			<h1>Article Not Found!</h1>
			There is no article associated with this node.<br>Click the button below to create one now, or review your options below.<br>
			
			<asp:button id="addArticle" runat="server" text="add article now" class="button green" />
		
			<BR><BR><BR>
			other options:
			<ul>
				<li><a href="javascript:editNode(<%=Request.QueryString["nodeID"]%>);">change node type of this node</a></li>
				<li><a href="mailto:stefany@DuryTools.com">e-mail for support</a></li>
			</ul>
		
		</td>	
	</tr>
</table>



















