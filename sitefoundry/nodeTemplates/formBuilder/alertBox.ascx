<%@ Control Language="c#" AutoEventWireup="false" Codebehind="alertBox.ascx.cs" Inherits="Dury.SiteFoundry.nodeTemplates.formBuilder.alertBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h1 class="alert" id="alertMsg" runat="server" visible="false" />
<script language="javascript">
window.setTimeout('hideElement(\'<%=alertID%>\')',5000);
</script>