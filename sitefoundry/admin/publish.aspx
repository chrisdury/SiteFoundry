<%@ Page language="c#" Codebehind="publish.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.Admin.publish" %>
<form id="publish" method="post" runat="server">
<span class="leftcolumn">
	<div class="folderMenu">
		<table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td>Control<!--<img src="images/publish.gif" />--></td>
				<td align="right">
					
				</td>
			</tr>
		</table>
	</div>
	Check the node types you wish to publish:
	<asp:checkboxlist id="nodesToPublish" runat="server" />
	<BR>
	<asp:button id="publishButton" runat="server" text="publish" cssclass="button yellow" />
	<BR><BR>
	<span class="small"><asp:literal id="results" runat="server" /></span>

</span>
<span class="rightcolumn">
	<div class="articleMenu">
		<table border="0" cellpadding="0" cellspacing="0" width="700">
			<tr>
				<td>items marked for publication</td>
				<td align="right"></td>
			</tr>	
		</table>				
	</div>
	<span class="folder">
		<asp:placeHolder id="itemsHolder" runat="server" />
		<!--
		AllowSorting="true"
		AllowPaging="True"
		PageSize="10"
		OnSortCommand="sortGrid"
		OnPageIndexChanged="changePage"
		onItemDataBound="itemsGrid_onItemDataBound"
			-->
		<asp:DataGrid
			BackColor="#f6f6f6"
			cellpadding="3"
			cellspacing="0"

			border="1"
			id="itemsGrid" 
			runat="server" 
			GridLines="Horizontal"
			autogeneratecolumns="false"
			ItemStyle-VerticalAlign="Middle"
			itemStyle-cssClass="tableitem"
			
			
			>
			<PagerStyle backColor="#92A5B0" ForeColor="#ffffff" Mode="NumericPages" PageButtonCount="10" cssclass="pager" horizontalAlign="right" NextPageText="next" PrevPageText="prev"/>
			<HeaderStyle cssclass="tableheader"/>
			<AlternatingItemStyle BackColor="#ffffff" />
			<Columns>
				<asp:templateColumn headerText="Type" sortexpression="type">
				<itemStyle cssclass="small center" />
					<itemTemplate>
						<nobr><%# DataBinder.Eval(Container.DataItem, "type")%></nobr>
					</itemTemplate>						
				</asp:templateColumn>
				
				<asp:templateColumn headertext="Filename" sortexpression="filename">
				<itemStyle cssclass="small center" />
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "filename")%>
					</itemTemplate>						
				</asp:templateColumn>
				
				<asp:templateColumn HeaderText="Title" SortExpression="Title">
				<ItemStyle width="99%"></ItemStyle>
					<itemTemplate>
						<a href="cms.aspx?nodeID=<%# DataBinder.Eval(Container.DataItem, "nodeID")%>"><%# DataBinder.Eval(Container.DataItem, "title")%></a>
					</itemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headerText="<nobr>Modified By</nobr>" sortExpression="LastUser">
					<itemStyle cssclass="small center" />
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "modifiedBy")%>
					</itemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headerText="Last Modified" sortExpression="DateModified">
					<itemStyle cssclass="small" />
					<itemTemplate>
						<nobr><%# DataBinder.Eval(Container.DataItem, "dateModified")%></nobr>
					</itemTemplate>
				</asp:templateColumn>
			</Columns>
			</asp:DataGrid>
		<span class="small"><asp:literal id="info" runat="server" /></span>
		<BR><BR>
		</span>
</form>
