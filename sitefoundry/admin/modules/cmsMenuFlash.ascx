<%@ Control Language="c#" AutoEventWireup="false" Codebehind="cmsMenuFlash.ascx.cs" Inherits="Dury.SiteFoundry.admin.cmsMenuFlash" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0" 
		WIDTH="240" HEIGHT="600" 
		id="main" ALIGN="" VIEWASTEXT>
	<PARAM NAME=movie VALUE="menu02.swf?path=<%=Dury.SiteFoundry.Admin.SFGlobal.SiteRoot%>admin/data.asmx&keyString=<%=UserEncryptedString%>">
	<PARAM NAME="menu" VALUE="false">
	<PARAM NAME=quality VALUE=high>
	<PARAM NAME="WMODE" value="TRANSPARENT">
	<EMBED src="menu02.swf?path=<%=Dury.SiteFoundry.Admin.SFGlobal.SiteRoot%>admin/data.asmx&keyString=<%=UserEncryptedString%>" quality=high WIDTH="240" HEIGHT="600" NAME="main" ALIGN="" TYPE="application/x-shockwave-flash" PLUGINSPAGE="http://www.macromedia.com/go/getflashplayer" wmode="transparent"></EMBED>
</OBJECT>