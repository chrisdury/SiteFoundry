<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin._default" %>
<form id="Form1" method="post" runat="server">

<h1>Welcome to Your Content and Site Management Tool!</h1>

<div style="float:right;width:300px;">
<h3>Buttons</h3>
<p>
Buttons used throught this site are colour-coded to represent the
volatility of the actions they will perform.
</p>
<table class="buttonExplanationTable" cellpadding="5">
	<tr>
		<td class="button"><input type="button" value="safe" class="button green" /></td>
		<td>buttons marked with green will not alter or delete any item in the database</td>
	</tr>
	<tr>
		<td class="button"><input type="button" value="warning" class="button yellow" /></td>
		<td>buttons marked with yellow will modify but not delete</td>
	</tr>
	<tr>
		<td class="button"><input type="button" value="danger" class="button red" /></td>
		<td>buttons marked red will delete and in most cases cannot be undone or reversed.  please use extreme caution when deleting items</td>
	</tr>
</table>


</div>


<div style="width:400px;">
<p>To begin, select an activity from the menu above.  Here's a detailed explanation of each item</p>
<ul>
	<li><h3>Content Management</h3>This is where you manage the content contained on the pages of your site</li>
	<li><h3>Resources</h3>This is where you can manage files and documents that are referenced or linked from pages in the CMS</li>
	<li><h3>Publish</h3>This is where you can publish the changes/updates to your site to be publically visible</li>
	<li><h3>Users</h3>Manage security accounts and roles to precisely delegate control to others</li>
</ul>
</div>
</form>
