<%@ Control Language="c#" AutoEventWireup="false" Codebehind="cmsMenuAJAX.ascx.cs" Inherits="Dury.SiteFoundry.Admin.cmsMenuAJAX" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script language="javascript" type="text/javascript" src="includes/ajax_menu.js"></script>
<div id="ajaxMenu"></div>
<script language="javascript"><asp:literal id="menu" runat="server" /></script>	
<div id="addNodeLink"><a href="javascript:addNode();">add a node</a></div>
<div id="nodeMenu">
	<a href="#" id="upLink" !onmouseover="keepMenuOpen();"><img src="images/up.gif" border="0" alt="move this node up" /></a>
	<a href="#" id="downLink" !onmouseover="keepMenuOpen();"><img src="images/down.gif" border="0" alt="move this node down" /></a>
	<a href="#" id="editLink"><img src="images/smalledit.gif" border="0" alt="edit this node" /></a>
	<a href="#" id="deleteLink"><img src="images/smalldelete.gif" border="0" alt="delete this node" /></a>
</div>