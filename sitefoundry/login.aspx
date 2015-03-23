<%@ Page language="c#" Codebehind="login.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Login" %>
<html>
<head>
<title>please login</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<META http-equiv="Pragma" content="no-cache">

<style type="text/css">

body {
	font-family:arial,helvetica,sans-serif;
	font-size:11px;
	background-color:#eee;
	margin:0px;
}

input.loginBox {
	border:1px solid #ccc;
	width:150px;
}

input.loginButton  {
	margin-top:10px;
	margin-left:64px;
	margin-bottom:10px;
}


.loginTable {
	border-collapse:collapse;
	margin-top:10px;
}

.loginTable td {
	font-size:11px;
	padding:0px;
}


</style>
</head>
<body onLoad="document.forms[0].username.focus();">
    <form id="login" method="post" runat="server">
	<div align="center" style="margin-top:50px;">
		<div id="login" style="border:1px solid #999999;background-color:#F4FAFD;width:300px;">
			<span style="font-size:30px;font-weight:bold;">Please Sign In</span><BR>
			You must provide a valid login/password<br>to view the page requested.
			<div style="width:90%;margin-top:10px;color:#333333;font-size:12px;"><asp:label id="msg" runat="server"/></div>
			<table class="loginTable">
				<tr><td align="right">login :&nbsp;</td><td><asp:textbox id="username" runat="server" cssclass="loginBox"/></td></tr>
				<tr><td align="right">password :&nbsp;</td><td><asp:textbox id="password" TextMode="Password" runat="server" cssclass="loginBox"/></td></tr>
				<tr><td colspan="2">
					<asp:button id="submit" text=" login " runat="server" cssclass="button loginButton"/>
				</td></tr>
			</table>		
		</div>
	</div>
    </form>
  </body>
</html>