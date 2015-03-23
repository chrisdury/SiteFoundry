<%@ Control Language="c#" AutoEventWireup="false" Codebehind="formAdmin.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.formAdmin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="sf" TagName="alertbox" src="~/nodeTemplates/formBuilder/alertBox.ascx" %>
<script language="javascript">

//alert(window.innerHeight);
var idealHeight = 450;
var togglePanels = new Array('formPropertiesPanel','_ctl3_addPanel','_ctl3_editFieldPanel');
var fieldsPanels = new Array('fieldListPanel','previewPanel');

function toggle(el) {
	if ($(el)==null) { alert('You must save or close the question you are currently editing before you can perform this action.');return; }
	Element.toggle(el);
	//setPanelHeights();
}

function setPanelHeights() {
	//alert('setting heights');
	var cumulativeHeights = 0;
	for(i=0;i<togglePanels.length;i++) {
		if ($(togglePanels[i]) != null) cumulativeHeights += Element.getHeight(togglePanels[i]);
	}
	for(j=0;j<fieldsPanels.length;j++) {
		if ($(fieldsPanels[j]) != null) $(fieldsPanels[j]).style.height = idealHeight - cumulativeHeights + 'px';
	}
}

window.setTimeout("setPanelHeights()",50);

var currentControlID;
function showControls(id) {
	if (currentControlID != null) toggleVisibility(currentControlID);
	currentControlID = id;
	toggleVisibility(currentControlID);
}

function toggleVisibility(el) {
	$(el).style.visibility = ($(el).style.visibility == 'visible') ? 'hidden' : 'visible';
}

function showQuestions() {
	toggle(togglePanels[1]);
	setPanelHeights();
	return true;
}

function showFormProps() {
	toggle('_ctl3_formPropertiesTable');
	toggle('_ctl3_formTitleHeader');
	setPanelHeights();
	
	if (Element.visible('_ctl3_formTitleHeader')) 
		$('showFormPropsButton').src = 'images/change_form_details.gif';
	else
		$('showFormPropsButton').src = 'images/hide_form_details.gif';
	//return false;
}

function showhelp() {
	openWin('fb_help.aspx',600,400);
}


</script>

<div class="titleBar">
	<div style="float:right;">
		<input type="image" src="images/add_question.gif" onClick="showQuestions();return false;" value="add question or section" title="click to show a panel that will allow you to add a new question or section."/>
		<input type="image" src="images/change_form_details.gif"  onClick="showFormProps();return false;" value="show form properties" id="showFormPropsButton" title="click to show form details panel"/>
		<a href="javascript:showhelp();" title="click to view brief instructions"><img src="images/fb_help.gif" border="0" alt="help button" /></a>
	</div>
	<h1>Form Builder</h1>
</div>

<sf:alertbox id="ab" runat="server" />

<div class="panel" id="formPropertiesPanel" runat="server">
	<h2 id="formTitleHeader" runat="server" />
	<table border="0" cellpadding="5" cellspacing="0" runat="server" id="formPropertiesTable">
		<tr>
			<td>
				<h3>title:</h3>
				<asp:textbox id="formTitle" runat="server" width="400" maxlength="200" />
			</td>
			<td style="text-align:right;">
				<div style="float:right;white-space:nowrap;">
					<asp:checkbox id="formDisplaySectionCheck" runat="server" text="display sections" />
					<asp:checkbox id="formActiveCheck" runat="server" text="active" />
					&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:imagebutton id="saveFormButton" runat="server" imageurl="~/admin/images/save_form.gif" />
				</div>
				<!--div class="floatPanel">
					display sections as:
					<asp:dropdownlist id="displaySectionsList" runat="server">
						<asp:listitem>fieldsets</asp:listitem>
						<asp:listitem>tabs</asp:listitem>
					</asp:dropdownlist>
				</div-->				
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<h3>description:</h3>
				<asp:textbox id="description" runat="server" width="700" textmode="multiline" rows="4" />
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<h3>after submission:</h3>
				<asp:textbox id="successText" runat="server" width="700" textmode="multiline" rows="4" />
			</td>
		</tr>
	</table>
</div>
<div class="panel editPanel" id="addPanel" runat="server" style="text-align:center;display:none;">
	<table border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="padding-right:20px;text-align:left;">
				<h2>New Section</h2>
				<table border="0" cellpadding="2" cellspacing="0">
					<tr>
						<td>
							<h3>section title:</h3>
							<p><asp:textbox id="sectionTitle" runat="server" width="150" /></p>
						</td>
						<td rowspan="2">
							<h3>description:</h3>
							<p><asp:textbox id="sectionDescription" runat="server" width="150" textmode="multiline" rows="3" /></p>
						</td>
					</tr>
					<tr>
						<td style="vertical-align:bottom;"><p><asp:button id="addSectionButton" runat="server" text="add section" /></p></td>
					</tr>
				</table>
			</td>
			<td style="width:50%;padding-left:20px;border-left:1px dotted #ccc;text-align:left;">
				<h2>New Field</h2>
				<table border="0" cellpadding="2" cellspacing="0">
					<tr>
						<td>
							<h3>field title:</h3>
							<p><asp:textbox id="newFieldTitle" runat="server" /></p>
						</td>
						<td>
							<h3>section:</h3>
							<p><asp:dropdownlist id="newFieldSectionList" runat="server" /></p>
						</td>
					</tr>
					<tr>
						<td>
							<h3>type:</h3>
							<p><asp:dropdownlist id="newFieldTypeList" runat="server" /></p>
						</td>
						<td style="vertical-align:bottom;">							
							<p><asp:button id="addFieldButton" runat="server" text="add question" /></p>
						</td>
					</tr>
				</table>				
			</td>
		</tr>
	</table>
</div>

<div id="editFieldPanel" class="panel editPanel" runat="server">
	<h2>Edit Field</h2>
	<asp:placeHolder id="editFieldHolder" runat="server" />
</div>

<table border="0" cellpadding="0" cellspacing="0" id="fieldPanelsTable" runat="server">
	<tr><td>
		<div id="fieldListPanel" class="panel">
			<h2>Current Fields in the Form</h2>
			<asp:placeHolder id="fieldsHolder" runat="server" />
		</div>
	</td>
	<td style="padding-left:10px;">
		<div id="previewPanel" class="panel">
			<h2>Preview</h2>
			<asp:placeHolder id="previewHolder" runat="server" />
		</div>
	</td></tr>
</table>
