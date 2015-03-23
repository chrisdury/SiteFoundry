/* 
	SiteFoundry Active Menu
	(c) 2006 Chris Dury -- All Rights Reserved.
*/
var QS = new Querystring();

// Node -- shared properties of Nodes.
var Node = Class.create();
//var Node = new Object();
Node.prototype = {
	initialize: function() {},
	id: 0,
	url: '',
	type: 0,
	label: '',
	hasChildren: false,
	userCanModify: false
};

//NodeBase -- javascript only methods and properties{
var NodeBase = {
	// private properties
	children: new Array(),
	parent: null,
	
	// public methods	
	Draw: function() {
		var target = (this.parent==null) ? 'ajaxMenu' : 'ajaxMenu_'+this.parent.id;
		var s = '';
		s += '<div id="ajaxMenu_' + this.id + '" class="menuContainer"><nobr>'
		s += this.renderSelf();
		s += '</nobr></div>';
		new Insertion.Bottom(target,s);	
	},
	
	DrawChildren: function() {
		this.renderChildren();
	},

	Find: function(nodeID) {	
		if (this.id == nodeID) { return this;}
		if (this.children != null && this.children.length > 0) {
			for(i=0;i<this.children.length;i++) {
				var n = this.children[i].Find(nodeID);
				if (n != null) return n;
			}
		}
		return null;
	},
	
	// private methods
	renderSelf: function() {
		var s = '';
		if (this.hasChildren) s += 	'<a id="parentLink_' + this.id + '" href="javascript:void(0);" onClick="toggleChildren(this,' + this.id + ');this.blur();" class="parentLink">+</a> ';
		var c = (this.hasChildren) ? '' : 'parentless';
		if (parseInt(QS.get('nodeID')) == this.id) c += ' selected';
		s += '<a href="javascript:showNode(' + this.id + ');" class="' + c + '" !onClick="selectNode(this);" ';
		if (this.userCanModify)
			s += 'onmouseover="showNodeMenu(this,' + this.id + ');\"';
		s += ' >' + this.label + '</a>';
		return s;
	},
	
	renderChildren: function() {
		if (this.hasChildren && this.children.length == 0) this.children = NodeFactory.GetNodeChildren(this.id);
		for(i=0;i<this.children.length;i++) this.children[i].Draw();
	}		
}

var NodeFactory = {
	GetNodeChildren: function(nodeID) {
		var n = eval('(' + Dury.SiteFoundry.Admin.cms.GetNodeChildren(nodeID).value + ')');
		var p = findNode(nodeID);
		var q = new Array();
		if (n.length > 0) {
			for (i=0;i<n.length;i++) {
				q[i] = Object.extend(n[i],NodeBase);
				q[i].parent = p;
				allNodes.push(q[i]);
			}
			return q;	
		} else {
			alert('no children!');
			return null;
		}
	},
	GetNode: function(nodeID) {
		var o = eval('(' + Dury.SiteFoundry.Admin.cms.GetNode(nodeID).value + ')');
		o = Object.extend(o,NodeBase);
		allNodes.push(o);
		return o;
	}
}

// node holder
var nodeRoot = new Node();
var allNodes = new Array();

function findNode(nodeID) {
	for(i=0;i<allNodes.length;i++)
		if (allNodes[i].id == nodeID) return allNodes[i];
	alert('cant find:' + nodeID)
}

// nodes that are open holder
var openNodes = new Array();
var menuCookieName = "ajaxMenuState";
window.onload = function() { loadMenu(); }
window.onunload = function(){ saveMenuState();}
var timer;

function loadMenu() {
	timer = window.setTimeout("loadMenuState()",150);
}

function loadMenuState() {
	var links = document.getElementsByClassName('parentLink');
	var openNodes = getCookie(menuCookieName).split(',');
	if (openNodes != '') {
		var m = openNodes.shift();
		if ($('parentLink_'+m) != null) {
			for (j=0;j<links.length;j++)
				if (links[j].id == 'parentLink_'+m.toString()) toggleChildren(links[j],m);
			setCookie(menuCookieName,openNodes,getExpirationDate(1));
		}
		loadMenu();
	} else {
		clearTimeout(timer);
		timer = null;
	}
}

function saveMenuState() {
	openNodes = openNodes.clear();
	var links = document.getElementsByClassName('parentLink');
	for (i=0;i<links.length;i++) {
		if (links[i].innerHTML == '-') {
			openNodes.push(links[i].id.split('_')[1]);
		}
	}
	setCookie(menuCookieName,openNodes, getExpirationDate(1));
}


function toggleChildren(source,nodeID) {	
	var n = findNode(nodeID);
	if (n==null) { alert('can\'t find node:' + nodeID); return;}
	source.innerHTML = (source.innerHTML == '+') ? '-' : '+';
	source.blur();
	if (n.children != null && n.children.length > 0) {
		for (i=0;i<n.children.length;i++) {
			var s = 'ajaxMenu_' + n.children[i].id;
			Element.visible(s) ? Element.hide(s) : Element.show(s);
		}		
	} else {
		n.DrawChildren();
	}
}


// draws the menu starting at the nodeID supplied. call
// multiple times to 'stack' menus in the menuHolder div.
function drawMenu(nodeID) {
	nodeRoot = NodeFactory.GetNode(nodeID);
	nodeRoot.Draw();
	nodeRoot.DrawChildren();	
}


function showNodeMenu(source,nodeID) {
	Position.prepare();
	//$('nodeMenu').style.top = Position.cumulativeOffset(source)[1] + 'px';
	//alert();
	var offset = (/Gecko/.test(navigator.userAgent)) ? -2 : 50;
	//alert(window.scrollPosition);
	$('nodeMenu').style.top = offset + source.offsetTop - Position.realOffset(source)[1] + 'px';
	$('nodeMenu').style.left = (source.offsetLeft + Element.getDimensions(source).width + 8) + 'px';
	$('upLink').href = 'javascript:upLinkClick(' + nodeID + ');';
	$('downLink').href = 'javascript:downLinkClick(' + nodeID + ');';
	$('editLink').href = 'javascript:editLinkClick(' + nodeID + ');';
	$('deleteLink').href = 'javascript:deleteLinkClick(' + nodeID + ');';	
	Element.show('nodeMenu');
	
}
function hideNodeMenu(source,nodeID) {
	Element.hide('nodeMenu');
}

function upLinkClick(nodeID) {
	location.href = '?nodeID=' + nodeID + '&dir=up';
}

function downLinkClick(nodeID) {
	location.href = '?nodeID=' + nodeID + '&dir=dn';
}
function editLinkClick(nodeID) {
	editNode(nodeID);
}
function deleteLinkClick(nodeID) {
	deleteNode(nodeID);
}



