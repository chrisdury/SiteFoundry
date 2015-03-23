<%@ Control Language="c#" AutoEventWireup="false" Codebehind="cmsMenuDHTML.ascx.cs" Inherits="Dury.SiteFoundry.admin.cmsMenuDHTML" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" src="includes/menu.js" type="text/javascript"></script>
<script language="javascript" src="includes/ajax_menu.js" type="text/javascript"></script>
<div align="right">
	<a href="javascript:addNode();">+ add a node</a>
</div>
<div class="menu" style="overflow:hidden;">
	<div style="width:1000px;"><asp:Literal ID="menu" Runat="server" /></div>
</div>