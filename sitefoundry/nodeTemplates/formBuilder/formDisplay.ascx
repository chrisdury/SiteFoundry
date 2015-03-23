<%@ Control Language="c#" AutoEventWireup="false" Codebehind="formDisplay.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.formDisplay" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="mbcf" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.CustomForm" %>
<style type="text/css">
.validatorDisplay 
{
	font-family:Arial,helvetica,sans-serif;
	font-size:10px;
	color:#00ff00;
}

</style>


<mbcf:CustomForm runat="server" id="form">
	<asp:placeHolder id="formHolder" runat="server" />
	<p><asp:button id="submitButton" runat="server" text="submit" /></p>
</mbcf:CustomForm>