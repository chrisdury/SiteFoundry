<%@ Page language="c#" Codebehind="InsertUser.aspx.cs" AutoEventWireup="false" Inherits="SiteFoundry.InsertUser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
  <head>
    <title>InsertUser</title>
  </head>
  <body>
    <form id="Form1" method="post" runat="server">
		UserName:<asp:textbox id="username" runat="server" /><br>
		Password:<asp:textbox id="password" runat="server" /><br>
		<asp:button id="save" runat="Server" text="ok" />
     </form>
  </body>
</html>
