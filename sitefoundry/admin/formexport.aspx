<%@ Page language="c#" Codebehind="formexport.aspx.cs" AutoEventWireup="false" Inherits="Dury.SiteFoundry.admin.formexport" %>
<%@ Register TagPrefix="sf" TagName="alertbox" src="~/nodeTemplates/formBuilder/alertBox.ascx" %>
<form id="formexport" method="post" runat="server">
	<span class="leftcolumn">
		<h2>Data Export</h2>
		<div class="toolPanel">
			<div class="title">Information</div>
			<div class="content">
				
			</div>
		</div>
	</span>
	<span class="rightcolumn">
		<sf:alertbox id="ab" runat="server" />
		<h2>Forms</h2>
		<asp:datagrid id="formsGrid" runat="server"
			cellpadding="4"
			cellspacing="0"
			border="1"
			width="730"
			GridLines="horizontal"
			autogeneratecolumns="false"
			AllowSorting="true"
			AllowPaging="True"
			PageSize="14"
			cssClass="itemTable"
			>
			<PagerStyle cssclass="itemTablePager" Mode="NumericPages" PageButtonCount="20" horizontalAlign="right" NextPageText="next" PrevPageText="prev"/>
			<HeaderStyle cssclass="itemTableHeader"  />
			<AlternatingItemStyle cssclass="itemTableAlternating" />
			<ItemStyle cssclass="itemTableItem" />
			<EditItemStyle cssClass="itemTableSelected" />
			<columns>
				<asp:templateColumn headertext="<nobr>Form Name</nobr>" sortexpression="title">
					<itemStyle ></itemStyle>
					<itemTemplate>
						<%# DataBinder.Eval(Container.DataItem,"title")%>
					</itemTemplate>
					<editItemTemplate>
						<h4><%# DataBinder.Eval(Container.DataItem,"title")%></h4>
						<div style="height:200px;width:440px;overflow:auto;">
							<asp:placeholder id="holder" runat="server" />
						</div>
					
					</editItemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headertext="<nobr>Responses</nobr>">
					<itemStyle ></itemStyle>
					<itemTemplate>
						<%# GetResponseCount((int)DataBinder.Eval(Container.DataItem,"FormID")) %> 
					</itemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headertext="<nobr>Node / Page</nobr>" sortexpression="NodeID">
					<itemStyle ></itemStyle>
					<itemTemplate>
						<a href="<%# DataBinder.Eval(Container.DataItem,"NodeIDObject.URL")%>" title="click to visit form on public site" ><%# DataBinder.Eval(Container.DataItem,"NodeIDObject.Filename")%></a>
					</itemTemplate>
				</asp:templateColumn>
				
				<asp:templateColumn headertext="<nobr>Download Data</nobr>">
					<itemStyle ></itemStyle>
					<itemTemplate>
						<a href="download.aspx?formID=<%# DataBinder.Eval(Container.DataItem,"FormID")%>" title="click to download CSV" ><img src="images/csv.gif" border="0" alt="csv" /></a>
					</itemTemplate>
				</asp:templateColumn>
				
				<asp:EditCommandColumn
						EditText="<img src='images/zoom.gif' border='0' alt='zoom'/>"
						CancelText="<img src='images/cancel.gif' border='0' alt='cancel'/>"
						HeaderText="details">
					<itemStyle cssclass="center" />
				</asp:EditCommandColumn>
				
				
			</columns>
		</asp:datagrid>		
	</span>
</form>