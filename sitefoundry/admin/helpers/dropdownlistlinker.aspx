<%@ Page language="c#" Codebehind="dropdownlistlinker.aspx.cs" AutoEventWireup="false" Inherits="SiteFoundry.admin.helpers.dropdownlistlinker" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 
<html>
	<head>
		<title>DropDownList Linker</title>
	<link rel="stylesheet" href="../includes/common.css" type="text/css" />
	<link rel="stylesheet" href="../includes/admin.css" type="text/css" />
	<link rel="stylesheet" href="../includes/forms.css" type="text/css" />
	<script language="javascript" type="text/javascript" src="../includes/main.js"></script>
	<script language="javascript" type="text/javascript" src="../includes/prototype.js"></script>
	<script language="javascript">
		
		// item row { title, url };
		var items = new Array();		
		function drawTable() {
			updatePreviewSelect('dropdownlist');
			if (items.length == 0) Element.show('noitems');
			else Element.hide('noitems');
			
			clearTable();			
			for (i=0;i<items.length;i++) {
				addItemToTable(i,items[i][0],items[i][1]);			
			}
			
		}	
		
		function addItem() {
			var t = document.getElementById('newItemTitle').value;
			var u = document.getElementById('newItemUrl').value;
			if (t.length == 0) { alert('please enter a title');return;}
			items.push([t,u]);
			drawTable();					
		}
		
		function addItemToTable(index,title,url) {
			var tbl = document.getElementById('itemsTable');
			var lastRow = tbl.rows.length;
			// if there's no header row in the table, then iteration = lastRow + 1
			//var iteration = lastRow;
			var row = tbl.insertRow(index+1);
			  
			// cell 0
			var cell0 = row.insertCell(0);
			var textNode = document.createTextNode(index+1);
			cell0.appendChild(textNode);
			  
			// cell1 - title
			var cell1 = row.insertCell(1);
			var el = document.createTextNode(title);
			cell1.appendChild(el);
			
			//cell2 - url
			var cell2 = row.insertCell(2);
			var el = document.createTextNode(url);
			cell2.appendChild(el);
			  
			// cell3 - up/down buttons
			var cell3 = row.insertCell(3);
			var b1 = document.createElement('a');
			b1.innerText = 'up';
			b1.href = 'javascript:moveItem("up",' + index + ');';			
			cell3.appendChild(b1);
			cell3.appendChild(document.createTextNode(' | '));
			var b2 = document.createElement('a');
			b2.innerText = 'down';
			b2.href = 'javascript:moveItem("dn",' + index + ');';
			cell3.appendChild(b2);
			
			// cell4 - remove button
			var cell4 = row.insertCell(4);
			var br = document.createElement('a');
			br.innerText = 'remove';
			br.href = 'javascript:removeItem(' + index + ');';
			cell4.appendChild(br);
			
		}
		
		function clearTable() {
			var tbl = document.getElementById('itemsTable');
			for (i=tbl.rows.length;i>1;i--) {
				removeRowFromTable();
			}		
		}
	
		function removeRowFromTable() {
			var tbl = document.getElementById('itemsTable');
			var lastRow = tbl.rows.length;
			if (lastRow > 1) tbl.deleteRow(lastRow - 1);
		}
		// the direction "up" means going closer to 0, since items are listed ASC
		function moveItem(dir,index) {
			var d = (dir=="up") ? -1 : 1;
			
			if (index + d < 0) {
				//alert('less than 0');
				return;
			}
			if (index + d >= items.length) {
				//alert('more than array length');
				return;
			}
			var oldItem = items[index+d];
			var newItem = items[index];
			items[index+d] = newItem;
			items[index] = oldItem;
			drawTable();		
		}
		function removeItem(index) {
			var item = items[index];
			items = items.without(item);
			drawTable();
		}
		
		function updatePreviewSelect(obj) {
			clearSelect(obj);
			for(i=0;i<items.length;i++) {
				$(obj).add(new Option(items[i][0],items[i][1]));		
			}	
		}	
	
		function clearSelect(obj) {
			$(obj).options.length = 1;		
		}
		
		function insertAndClose() {
			var html = '';
			
			html += '<form><div><select id="droplistchanger" onChange="dropDownChanger(this);"><option value="">please select...</option>';
			for(i=0;i<items.length;i++) {
				html += '<option value="' + items[i][1] + '">' + items[i][0] + '</option>';
			}
			html += '</select></div></form>';
			
			//html = '<form action="somepage.asp" method="post"><p><label for="mylist">My list</label><br /><select id="mylist" name="mylist"><option value="a">ABC</option><option value="b">DEF</option><option value="c">GHI</option><option value="d">JKL</option></select></p></form>';
			//alert(html);
			window.opener.insertMarkup(html);
			window.close();		
		}	
		
	</script>
	<style type="text/css" />
		body { padding:5px; }
		
		
		#itemsTable th {
			text-align:left;
			white-space:nowrap;
			
		}
		
		#itemsTable td {
			border-bottom:1px dotted #ccc;
			background-color:#eee;
		
		}
		
		h2 {
			padding:10px 0px 2px 0px;
		}
		
		a {
			text-decoration:underline;
		}
		
		
	</style>
</head>
<body class="gallery" !onBeforeUnload="if(updateParent){closeEditWindow(false);}">
	<form runat="server">		
		<div style="float:right;"><a href="javascript:Element.toggle('help');" style="color:red;font-size:20px;" title="Click to show the instructions">?</a></div>
		<h1>DropDownList Linker </h1>
		
		<div id="help" style="display:none;">
			<h3>Instructions</h3>
			<ol>
				<li>Enter a Title for the item. This is what will appear in the DropDownList</li>
				<li>Enter a URL for the item. This is where the browser will be taken when the user selects the item</li>
				<li>Click "Add Item"</li>
				<li>Add additional items until satisfied</li>
				<li>Click "Insert and Close Helper" to insert the DropDownList at the current cursor position in the Content Editor</li>
			</ol>	
		</div>
		
		<div style="">
			<h2>Preview</h2>
			<p><select id="dropdownlist"><option value="">please select an option...</option></select></p>
		</div>
		
		<h2>Items</h2>
		<table border="0" cellpadding="3" cellspacing="0" id="itemsTable" width="590">
			<thead>
				<tr>
					<th>#</th>
					<th width="50%">Title</th>
					<th width="50%">URL</th>
					<th>Change Ranking</th>
					<th>Remove</th>
				</tr>
			</thead>
			<tbody>
			
			</tbody>
		</table>
		<div id="noitems" style="color:red;">no items in list</div>
		
		
		<h3>Add New Item</h3>
		<table border="0" cellpadding="0" cellspacing="0" >
			<tr>
				<td>title</td>
				<td>url or document</td>
			</tr>
			<tr>
				<td valign="top"><input type="text" style="width:200px;" id="newItemTitle" /></td>
				<td>
					<input type="text" style="width:150px;" id="newItemUrl" value="http://" /><br/>
				</td>
				<td valign="top"><input type="button" value="add item to list" onClick="addItem();" /></td>
			</tr>
		</table>
		
		<p style="padding-top:20px;">
			<input type="button" value="insert and close helper" onClick="insertAndClose();"/>
		</p>
		
	</form>
</body>
</html>