<%@ Control Language="c#" AutoEventWireup="false" Codebehind="XStandard.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.articleContentEditors.XStandardEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="xs" Namespace="XStandard.WebForms" Assembly="XStandard.WebForms" %>
<script for="content" event="ButtonClick(sButton, sState)" language="JavaScript">
alert('button clicked: ' + sButton);
</script>

<xs:XHTMLEditor id="content" runat="server" EscapeUnicode="False"></xs:XHTMLEditor>